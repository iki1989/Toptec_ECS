using System.Data;
using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;
using ECS.Model;
using ECS.Model.Restfuls;
using ECS.Core.Managers;
using Urcis.SmartCode.Diagnostics;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;
using ECS.Model.Databases;
using System.Collections.Generic;
using System.Diagnostics;

namespace ECS.Core.WebServices
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class WcsRestfulServerService : BaseService, IWcsRestfulServerService
    {
        public WcsRestfulServerService()
        {
            string name = "WCS Service";
            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(name, EcsAppDirectory.RestfulWebServiceWcsLog));
            this.Logger.Setting.KeepingDays = EcsServerAppManager.Instance.Logger.Setting.KeepingDays;
        }

        [return: MessageParameter(Name = "Json")]
        public async Task<WcsResponse> OrderPostAsync(Stream body)
        {
            WcsResponse response = new WcsResponse().SetBadRequset();

            try
            {
                var order = await this.ParseBody<Order>(body);

                if (order == null)
                {
                    this.WrtieLog("[Send]", typeof(Order), response.ToJson());
                    return response;
                }

                EcsServerAppManager.Instance.DataBaseManagerForServer.InsertOrderAsync(order);

            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(Order), response.ToJson());
            return response;
        }

        [return: MessageParameter(Name = "Json")]
        public async Task<WcsResponse> OrderCancelPostAsync(Stream body)
        {
            WcsResponse response = new WcsResponse().SetBadRequset();
            try
            {
                var orderCancel = await this.ParseBody<OrderCancel>(body);
                if (orderCancel == null)
                {
                    this.WrtieLog("[Send]", typeof(OrderCancel), response.ToJson());
                    return response;
                }
                //주문삭제 정보를 저장하고 삭제 가능 여부 조회
                var res = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertOrderCancel(orderCancel);
                if (!res)
                {
                    response.result_cd = (int)ErrorCode.InternalServerError;
                    response.result_msg = "작업을 시작한 주문이 있습니다.";
                    this.WrtieLog("[Send]", typeof(OrderCancel), response.ToJson());
                    return response;
                }
                //삭제 가능할 경우 db에서 제거
                var boxNos = orderCancel.data.Select(x => x.BOX_NO).Distinct().ToHashSet();
                EcsServerAppManager.Instance.DataBaseManagerForServer.UpdateOrderCancel(boxNos.ToArray());


                #region 메모리 검색. 중량검수 이후 주문 취소 일 경우 전체 Cancel

                lock (EcsServerAppManager.Instance.Cache.ProductInfos)
                {
                    foreach (var info in EcsServerAppManager.Instance.Cache.ProductInfos.Values)
                    {
                        if (boxNos.Contains(info.BOX_NO))
                        {
                            EcsServerAppManager.Instance.Cache.ProductInfos.Remove(info.BOX_ID);
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }

            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(OrderCancel), response.ToJson());
            return response;
        }

        [return: MessageParameter(Name = "Json")]
        public async Task<WcsResponse> OrderDeletePostAsync(Stream body)
        {
            //미사용 운영. 구현만 요청받음
            WcsResponse response = new WcsResponse().SetBadRequset();

            try
            {
                var orderDelete = await this.ParseBody<OrderDelete>(body);
                if (orderDelete == null)
                {
                    this.WrtieLog("[Send]", typeof(OrderDelete), response.ToJson());
                    return response;
                }

                bool deleteResult = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertOrderDelete(orderDelete);
                if (deleteResult == false)
                {
                    response.result_cd = (int)ErrorCode.InternalServerError;
                    response.result_msg = "작업을 시작한 주문이 있습니다.";
                    this.WrtieLog("[Send]", typeof(OrderCancel), response.ToJson());
                    return response;
                }


            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }

            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(OrderDelete), response.ToJson());
            return response;
        }

        [return: MessageParameter(Name = "Json")]
        public async Task<WcsResponse> OperatorWeightResultPostAsync(Stream body)
        {
            WcsResponse response = new WcsResponse().SetBadRequset();

            try
            {
                var operatorWeightResult = await this.ParseBody<OperatorWeightResult>(body);
                if (operatorWeightResult == null)
                {
                    this.WrtieLog("[Send]", typeof(OperatorWeightResult), response.ToJson());
                    return response;
                }

                EcsServerAppManager.Instance.DataBaseManagerForServer.InsertManualWeightCheck(operatorWeightResult);

                #region 메모리 업데이트
                foreach (var d in operatorWeightResult.data)
                {
                    lock (EcsServerAppManager.Instance.Cache.ProductInfos)
                    {
                        if (EcsServerAppManager.Instance.Cache.ProductInfos.ContainsKey(d.BOX_ID))
                        {
                            var info = EcsServerAppManager.Instance.Cache.ProductInfos[d.BOX_ID];
                            {
                                //info.WH_ID = d.WH_ID;
                                //info.ORD_NO = d.ORD_NO;
                                info.WT_CHECK_FLAG = d.WT_CHECK_FLAG;
                                //info.EQP_ID = d.EQP_ID;
                                //info.INVOICE_ID = d.INVOICE_ID;
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }

            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(OperatorWeightResult), response.ToJson());
            return response;
        }

        [return: MessageParameter(Name = "Json")]
        public async Task<WcsResponse> SkuMasterPostAsync(Stream body)
        {
            WcsResponse response = new WcsResponse().SetBadRequset();

            try
            {
                var skuMaster = await this.ParseBody<SkuMaster>(body);
                if (skuMaster == null)
                {
                    this.WrtieLog("[Send]", response.ToJson());
                    return response;
                }

                EcsServerAppManager.Instance.DataBaseManagerForServer.InsertSkuMaster(skuMaster);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }

            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(SkuMaster), response.ToJson());
            return response;
        }

        [return: MessageParameter(Name = "Json")]
        public async Task<WcsResponse> ContainerMappingPostAsync(Stream body)
        {
            WcsResponse response = new WcsResponse().SetBadRequset();

            try
            {
                var picking = await this.ParseBody<Picking>(body);
                if (picking == null)
                {
                    this.WrtieLog("[Send]", response.ToJson());
                    return response;
                }

                DataTable dataTable = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertPicking(picking);
                if (dataTable == null)
                {
                    this.WrtieLog("[Send]", typeof(Picking), response.ToJson());
                    return response;
                }

                EcsServerAppManager.Instance.Cache.ProductInfoLoad(dataTable);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }

            response.SetSuccess();
            this.WrtieLog("[Send]", typeof(Picking), response.ToJson());
            return response;
        }
    }
}
