using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Urcis.SmartCode;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode.Net.Tcp;
using ECS.Core.Communicators;
using ECS.Core.Managers;
using ECS.Model.Hub;
using ECS.Model;
using ECS.Model.Plc;
using ECS.Model.Restfuls;
using ECS.Model.Databases;
using ECS.Model.Domain.Touch;
using ECS.Model.Pcs;
using System.Data;

namespace ECS.Core.Equipments
{
    public class TopBcrEquipment : Equipment
    {
        #region Field
        private DeviceStatus TopBcrDeviceStatus = new DeviceStatus();

        private int NoReadCount = 0;

        private Dictionary<string, BoxInfoData> BoxInfos = new Dictionary<string, BoxInfoData>();
        #endregion

        #region Prop
        public new TopAndSideBcrCommunicator Communicator
        {
            get => base.Communicator as TopAndSideBcrCommunicator;
            private set => base.Communicator = value;
        }

        public new TopBcrEquipmentSetting Setting
        {
            get => base.Setting as TopBcrEquipmentSetting;
            private set => base.Setting = value;
        }

        private int m_CurrrentHeightSensorValue;
        public int CurrrentHeightSensorValue
        {
            get => this.m_CurrrentHeightSensorValue;
            set
            {
                this.m_CurrrentHeightSensorValue = value;
                this.Logger?.Write($"HeightSensor update : {this.m_CurrrentHeightSensorValue}");
            }
        }
        #endregion

        #region Ctor
        public TopBcrEquipment(TopBcrEquipmentSetting setting)
        {
            this.Setting = setting ?? new TopBcrEquipmentSetting();

            {
                DeviceInfo info = new DeviceInfo();
                info.deviceId = this.Setting.Id;
                info.deviceName = "상면 BCR";
                info.deviceTypeId = "EQ";
                info.deviceTypeName = "설비";
                this.TopBcrDeviceStatus.deviceList.Add(info);
            }
        }
        #endregion

        #region Method
        #region Interface
        protected override void OnCreate()
        {
            this.LifeState = LifeCycleStateEnum.Creating;

            base.OnCreate();
            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.TopBcr));

            this.Communicator = new TopAndSideBcrCommunicator(this);
            this.Setting.CommunicatorSetting.LogDirectory = this.Logger.DirectoryPath;
            this.Communicator?.ApplySetting(this.Setting.CommunicatorSetting);

            this.LifeState = LifeCycleStateEnum.Created;
        }

        protected override bool OnPrepare()
        {
            if (this.LifeState != LifeCycleStateEnum.Created)
                this.Create();

            this.LifeState = LifeCycleStateEnum.Preparing;

            try
            {
                if (this.Communicator != null && this.Communicator.IsDisposed == false)
                {
                    this.Communicator.ReadDatas += this.Communicator_ReadDatas;
                    this.Communicator.Noread += this.Communicator_Noread;
                    this.Communicator.TcpConnectionStateChanged += this.Communicator_TcpConnectionStateChanged;
                    this.Communicator.OperationStateChanged += this.Communicator_OperationStateChanged;
                }

                this.BoxInfos.Clear();
                this.CreateOrUpdateBoxInfoData();
            }
            catch (Exception ex) { this.Logger?.Write(ex.Message); return false; }

            this.LifeState = LifeCycleStateEnum.Prepared;
            return true;
        }

        protected override void OnTerminate()
        {
            this.LifeState = LifeCycleStateEnum.Terminating;
            this.Stop();

            try
            {
                if (this.Communicator != null && this.Communicator.IsDisposed == false)
                {
                    this.Communicator.ReadDatas -= this.Communicator_ReadDatas;
                    this.Communicator.Noread -= this.Communicator_Noread;
                    this.Communicator.TcpConnectionStateChanged -= this.Communicator_TcpConnectionStateChanged;
                    this.Communicator.OperationStateChanged -= this.Communicator_OperationStateChanged;
                }
            }
            catch (Exception ex) { this.Logger?.Write(ex.Message); }

            this.LifeState = LifeCycleStateEnum.Terminated;
        }

        protected override void OnStart()
        {
            if (this.LifeState != LifeCycleStateEnum.Prepared)
            {
                this.Logger?.Write($"Prepare Falut : {this.LifeState}");
                return;
            }

            if (this.Communicator != null || (this.Communicator.IsDisposed == false))
            {
                if (this.Communicator.TcpConnectionState == TcpConnectionStateEnum.Disconnected)
                {
                    //동기가 느려서 비동기로 변경
                    //this.Communicator?.Start();
                    Task.Run(() => this.Communicator?.Start());
                    this.Logger?.Write("Communicator Start Async");
                }
            }
        }

        protected override void OnStop()
        {
            if (this.Communicator != null || (this.Communicator.IsDisposed == false))
            {
                if (this.Communicator.TcpConnectionState == TcpConnectionStateEnum.Connected)
                {
                    this.Communicator.Stop();
                    this.Logger?.Write("Communicator Stop");
                }
            }
        }
        #endregion

        #region Top Bcr

        public override void OnEquipmentConnectionUpdateTouchSend()
        {
            try
            {
                var topBcrConnection = this.Communicator.TcpConnectionState == TcpConnectionStateEnum.Connected ? true : false;

                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcBcrLcdEquipment>();
                if (eq != null)
                {
                    eq.InvoiceBcrState.TopBcrConnection = topBcrConnection;
                    eq.EquipmentCoonectionStateSend();
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        protected override void OnEquipmentStateRicpPost()
        {
            try
            {
                #region OnEquipmentStateRicdpPost
                base.OnEquipmentStateRicpPost();

                var device = this.TopBcrDeviceStatus.deviceList[0];
                if (this.Communicator.TcpConnectionState == TcpConnectionStateEnum.Connected)
                {
                    device.deviceStatusCd = 1;
                    device.deviceErrorMsg = string.Empty;
                }
                else
                {
                    device.deviceStatusCd = 0;
                    device.deviceErrorMsg = "상면 BCR";
                }

                var restfulRequseter = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                restfulRequseter.DeviceStatusPostAsync(this.TopBcrDeviceStatus);
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private bool OnetimeWcsPosted(string status)
        {
            if (int.TryParse(status, out int s))
            {
                if (s >= (int)statusEnum.top_invoice)
                    return true;
            }
            else
                return false;

            return false;
        }

        //WCS에 중복보고 금지!
        private void InvoiceWcsPost(ProductInfo info)
        {
            if (info == null) return;

            if (this.OnetimeWcsPosted(info.STATUS) == false)
            {
                info.STATUS = $"{(int)statusEnum.top_invoice}";

                #region WCS Format Set
                WeightAndInvoice weightAndInvoice = new WeightAndInvoice();

                WeightAndInvoice.DataClass dataClass = new WeightAndInvoice.DataClass();
                {
                    dataClass.wh_id = info.WH_ID;
                    dataClass.cst_cd = info.CST_CD;
                    dataClass.wave_no = info.WAVE_NO;
                    dataClass.wave_line_no = info.WAVE_LINE_NO;
                    dataClass.ord_no = info.ORD_NO;
                    dataClass.ord_line_no = info.ORD_LINE_NO;
                    dataClass.box_id = info.BOX_ID;
                    dataClass.box_no = info.BOX_NO;
                    dataClass.store_loc_cd = info.STORE_LOC_CD;
                    dataClass.box_type = info.BOX_TYPE;
                    dataClass.floor = "2";
                    dataClass.invoice_id = info.INVOICE_ID;
                    dataClass.status = info.STATUS;
                    dataClass.eqp_id = this.Id;
                    if (double.TryParse(info.BOX_WT, out double boxWt)) { }
                    dataClass.box_wt = boxWt;
                    dataClass.result_cd = "E000";
                    dataClass.result_text = "정상처리";
                }
                weightAndInvoice.data.Add(dataClass);

                #endregion

                var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetWcsManager>();
                if (manager != null)
                    manager.InvoicePostAsync(weightAndInvoice);
            }
        }

        public void BcrAlarmSet(bool isOn, WeightInvoicBcrEnum weightInvoicBcrEnum)
        {
            try
            {
                WeightInvoiceBcrAlarmArgs bcrAlarmArgs = new WeightInvoiceBcrAlarmArgs();
                bcrAlarmArgs.BcrType = weightInvoicBcrEnum;
                bcrAlarmArgs.Result = isOn;

                EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, bcrAlarmArgs);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void AlarmSetTouchSend(string reason)
        {
            var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcBcrLcdEquipment>();
            if (eq != null)
            {
                BcrAlarmSetReset bcrAlarmSetReset = new BcrAlarmSetReset();
                bcrAlarmSetReset.Reason = reason;
                bcrAlarmSetReset.AlarmResult = true;

                eq.OnBcrAlarmSetResetSend(bcrAlarmSetReset);
            }
        }
        #endregion

        #region OnHub
        public override void OnHub_Recived(EventArgs e)
        {
            try
            {
                if (e is TopRequestBoxIdArgs topRequestBoxIdArgs)
                {
                    #region 메모리 확인
                    if (EcsServerAppManager.Instance.Cache.ProductInfos.ContainsKey(topRequestBoxIdArgs.BoxId))
                    {
                        ProductInfo info = EcsServerAppManager.Instance.Cache.ProductInfos[topRequestBoxIdArgs.BoxId];
                        info.TASK_ID = $"{(int)TASK_IDEnum.Operator_Invoice_Result}";
                        EcsServerAppManager.Instance.DataBaseManagerForServer.InsertTop(topRequestBoxIdArgs.BoxId, info.INVOICE_ID, $"{VerificationType.OK}", true);

                        this.InvoiceWcsPost(info);

                        TopBcrResultArgs topBcrResultArgs = new TopBcrResultArgs();
                        topBcrResultArgs.BoxId = topRequestBoxIdArgs.BoxId;
                        topBcrResultArgs.Invoice = info.INVOICE_ID;
                        topBcrResultArgs.Result = BcrResult.OK;

                        topRequestBoxIdArgs.Result = true;

                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcInvoiceRejectEquipment, topRequestBoxIdArgs);
                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.RicpPost, topBcrResultArgs);

                        this.Logger?.Write($"Reprint BoxId ={topRequestBoxIdArgs.BoxId}, Invoice = {info.INVOICE_ID}, Result = {VerificationType.OK}");
                    }
                    else
                    {
                        topRequestBoxIdArgs.Result = false;
                        EcsServerAppManager.Instance.DataBaseManagerForServer.InsertTop(topRequestBoxIdArgs.BoxId, string.Empty, $"{VerificationType.NOREAD}", true);
                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcInvoiceRejectEquipment, topRequestBoxIdArgs);

                        this.Logger?.Write($"Reprint BoxId ={topRequestBoxIdArgs.BoxId}, Invoice = {string.Empty}, Result = {VerificationType.NOREAD}");
                    }
                    #endregion
                }
                else if (e is CaseErectBoxNumberArgs caseErectBoxNumberArgs)
                {
                    this.CreateOrUpdateBoxInfoData();
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        #endregion

        #region Sensor
        private void CreateOrUpdateBoxInfoData()
        {
            foreach (var data in EcsServerAppManager.Instance.DataBaseManagerForServer.SelectBoxInfos())
            {
                lock (this.BoxInfos)
                {
                    if (this.BoxInfos.ContainsKey(data.BoxTypeCd))
                    {
                        this.BoxInfos[data.BoxTypeCd] = data;
                        this.Logger.Write($"Update BoxInfoData : BoxType = {data.BoxTypeCd}, HeightSensor = {data.HeightSensor}");
                    }
                    else
                    {
                        this.BoxInfos.Add(data.BoxTypeCd, data);
                        this.Logger.Write($"Create BoxInfoData : BoxType = {data.BoxTypeCd}, HeightSensor = {data.HeightSensor}");
                    }
                }
            }
        }
        #endregion

        #endregion

        #region Event Handler
        #region Top Bcr
        private void Communicator_TcpConnectionStateChanged(object sender, TcpConnectionStateChangedEventArgs e)
        {
            if (e.Current == e.Previous) return;

            this.Logger?.Write($"Bcr TcpConnectionStateChanged = {e.Current}");

            try
            {
                #region Bcr_TcpConnectionStateChanged
                if (e.Current == TcpConnectionStateEnum.Connected)
                    this.BcrAlarmSet(false, WeightInvoicBcrEnum.Top);
                else
                {
                    this.BcrAlarmSet(true, WeightInvoicBcrEnum.Top);
                    this.AlarmSetTouchSend("Top BCR 미연결");
                }

                this.OnEquipmentConnectionUpdateTouchSend();
                this.OnEquipmentStateRicpPost();
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void Communicator_OperationStateChanged(object sender, Urcis.SmartCode.Net.CommunicatorOperationStateChangedEventArgs e)
        {
            if (e.Current == e.Previous) return;

            this.Logger?.Write(e.Current.ToString());
        }

        private void Communicator_Noread(object sender, string[] readData)
        {
            try
            {
                string boxId = readData[0];
                string InvoiceId = readData[1];

                this.Logger?.Write($"Noread = boxId : {boxId}, InvoiceId : {InvoiceId}");

                #region Bcr Alarm
                if (this.NoReadCount <= 3)
                    this.NoReadCount++;

                if (this.NoReadCount >= 3)
                {
                    this.BcrAlarmSet(true, WeightInvoicBcrEnum.Top);
                    this.AlarmSetTouchSend("Top BCR 미인식 3회");
                }

                #endregion

                TopBcrResultArgs topBcrResultArgs = new TopBcrResultArgs();
                topBcrResultArgs.Result = BcrResult.Reject;
                topBcrResultArgs.BoxId = boxId;
                EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, topBcrResultArgs);

                BcrLcdIndexArgs bcrLcdIndexArgs = new BcrLcdIndexArgs();
                bcrLcdIndexArgs.BcrIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertTop(boxId, InvoiceId, $"{VerificationType.NOREAD}");
                EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcBcrLcdEquipment, bcrLcdIndexArgs);
                EcsServerAppManager.Instance.Hub.Send(HubServiceName.RicpPost, topBcrResultArgs);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void Communicator_ReadDatas(object sender, string[] readData)
        {
            try
            {
                string boxId = readData[0];
                string InvoiceId = readData[1];

                this.Logger?.Write($"ReadDatas = boxId : {boxId}, InvoiceId : {InvoiceId}");

                #region Bcr Alarm
                if (this.NoReadCount >= 3)
                    this.BcrAlarmSet(false, WeightInvoicBcrEnum.Top);

                this.NoReadCount = 0;
                #endregion

                TopBcrResultArgs topBcrResultArgs = new TopBcrResultArgs();
                topBcrResultArgs.BoxId = boxId;
                topBcrResultArgs.Invoice = InvoiceId;
                topBcrResultArgs.Result = BcrResult.Reject;
                BcrLcdIndexArgs bcrLcdIndexArgs = new BcrLcdIndexArgs();

                #region 메모리 확인
                if (EcsServerAppManager.Instance.Cache.ProductInfos.ContainsKey(boxId))
                {
                    ProductInfo info = EcsServerAppManager.Instance.Cache.ProductInfos[boxId];
                    info.TASK_ID = ((int)TASK_IDEnum.TopBcr_Fail).ToString();

                    if (info.INVOICE_ID == InvoiceId)
                    {
                        var dynamicEq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<DynamicScaleEquipment>();
                        if (dynamicEq != null)
                        {
                            if (this.NoWeightCheck(info.STATUS, info.BOX_WT))
                            {
                                // NoWeight
                                EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, topBcrResultArgs);
                                bcrLcdIndexArgs.BcrIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertTop(boxId, InvoiceId, $"{VerificationType.NOREAD}",true);

                                this.Logger?.Write($"BoxId ={boxId}, Invoice = {InvoiceId}, Result = {topBcrResultArgs.Result} NoWeight");
                            }
                            else if (dynamicEq.WeightFailCheck(info.WT_CHECK_FLAG, info.BOX_WT, info.WEIGHT_SUM) ||
                                     info.STATUS == $"{(int)statusEnum.insepct_reject_scale}")
                            {
                                //PlC 속도를 위해 우선처리
                                EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, topBcrResultArgs);
                                bcrLcdIndexArgs.BcrIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertTop(boxId, InvoiceId, $"{VerificationType.NOREAD}");

                                this.Logger?.Write($"BoxId ={boxId}, Invoice = {InvoiceId}, Result = {topBcrResultArgs.Result} Weight Fail");
                            }
                            else
                            {
                                info.TASK_ID = $"{(int)TASK_IDEnum.TopBcr_Success}";

                                topBcrResultArgs.Result = BcrResult.OK;
                                //PlC 속도를 위해 우선처리
                                EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, topBcrResultArgs);
                                bcrLcdIndexArgs.BcrIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertTop(boxId, InvoiceId, $"{VerificationType.OK}");
                                this.InvoiceWcsPost(info);

                                this.Logger?.Write($"BoxId ={boxId}, Invoice = {InvoiceId}, Result = {topBcrResultArgs.Result}");
                            }
                        }
                    }
                    else
                    {
                        //PlC 속도를 위해 우선처리
                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, topBcrResultArgs);
                        bcrLcdIndexArgs.BcrIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertTop(boxId, InvoiceId, $"{VerificationType.MISMATCH}");
                        this.Logger?.Write($"BoxId ={boxId} Invoice is misMatch(TopBcrRead:{InvoiceId}, ProductInfo{info.INVOICE_ID})");

                        //this.BcrAlarmSet(true, WeightInvoicBcrEnum.Top);
                        this.AlarmSetTouchSend("Top BCR 미스매치 발생");

                        if (info.MNL_PACKING_FLAG == $"{MNL_PACKING_FLAGEnum.Y}")
                        {
                            var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.NormalLabelPrinterZebraZe500Equipment];
                            if (eq is LabelPrinterZebraZe500Equipment printer)
                                printer.LabelPrinterAlarmSet(true);
                        }
                        else
                        {
                            var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.SmartLabelPrinterZebraZe500Equipment];
                            if (eq is LabelPrinterZebraZe500Equipment printer)
                                printer.LabelPrinterAlarmSet(true);
                        }
                    }
                }
                else
                {
                    //PlC 속도를 위해 우선처리
                    EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, topBcrResultArgs);
                    bcrLcdIndexArgs.BcrIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertTop(boxId, InvoiceId, $"{VerificationType.NOREAD}");
                    this.Logger?.Write($"BoxId ={boxId} have not ProductInfo");
                }
                #endregion

                EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcBcrLcdEquipment, bcrLcdIndexArgs);
                EcsServerAppManager.Instance.Hub.Send(HubServiceName.RicpPost, topBcrResultArgs);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private bool NoWeightCheck(string status, string BOX_WT)
        {
            if (int.TryParse(status, out int s) == false)
                return true;

            if (double.TryParse(BOX_WT, out double box_wt) == false)
                return true;

            if ((s >= (int)statusEnum.inspect_pass_sys)
                   && (box_wt > 0))
                return false;

            return true;
        }

        #region 상면 높이 센서 추가시 활성화 필요
        //private void Communicator_ReadDatas(object sender, string[] readData)
        //{
        //    try
        //    {
        //        string boxId = readData[0];
        //        string InvoiceId = readData[1];

        //        this.Logger?.Write($"ReadDatas = boxId : {boxId}, InvoiceId : {InvoiceId}");

        //        #region Bcr Alarm
        //        if (this.NoReadCount >= 3)
        //            this.BcrAlarmSet(false, WeightInvoicBcrEnum.Top);

        //        this.NoReadCount = 0;
        //        #endregion

        //        TopBcrResultArgs topBcrResultArgs = new TopBcrResultArgs();
        //        topBcrResultArgs.Invoice = InvoiceId;
        //        topBcrResultArgs.Result = BcrResult.Reject;
        //        BcrLcdIndexArgs bcrLcdIndexArgs = new BcrLcdIndexArgs();

        //        #region 메모리 확인
        //        if (EcsServerAppManager.Instance.Cache.ProductInfos.ContainsKey(boxId))
        //        {
        //            ProductInfo info = EcsServerAppManager.Instance.Cache.ProductInfos[boxId];
        //            info.TASK_ID = ((int)TASK_IDEnum.TopBcr_Fail).ToString();

        //            if (info.INVOICE_ID == InvoiceId)
        //            {

        //                var dynamicEq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<DynamicScaleEquipment>();
        //                if (dynamicEq != null)
        //                {
        //                    if (dynamicEq.WeightFailCheck(info.WT_CHECK_FLAG, info.BOX_WT, info.WEIGHT_SUM))
        //                    {
        //                        //PlC 속도를 위해 우선처리
        //                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, topBcrResultArgs);
        //                        bcrLcdIndexArgs.BcrIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertTop(boxId, InvoiceId, $"{VerificationType.NOREAD}");

        //                        this.Logger?.Write($"BoxId ={boxId}, Invoice = {InvoiceId}, Result = {topBcrResultArgs.Result} Weight Fail");
        //                    }
        //                    else
        //                    {
        //                        string boxType = $"{boxId[1]}";
        //                        if (this.BoxInfos.ContainsKey(boxType))
        //                        {
        //                            BoxInfoData boxInfoData = this.BoxInfos[boxType];

        //                            if (boxInfoData.HeightSensor == this.CurrrentHeightSensorValue)
        //                            {
        //                               info.TASK_ID = $"{(int)TASK_IDEnum.TopBcr_Success}";
        //                                topBcrResultArgs.Result = BcrResult.OK;
        //                                //PlC 속도를 위해 우선처리
        //                                EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, topBcrResultArgs);
        //                                bcrLcdIndexArgs.BcrIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertTop(boxId, InvoiceId, $"{VerificationType.OK}");
        //                                this.InvoiceWcsPost(info);

        //                                this.Logger?.Write($"BoxId ={boxId}, Invoice = {InvoiceId}, Result = {topBcrResultArgs.Result}");

        //                                this.CurrrentHeightSensorValue = 0;
        //                            }
        //                            else
        //                            {
        //                                //PlC 속도를 위해 우선처리
        //                                EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, topBcrResultArgs);
        //                                bcrLcdIndexArgs.BcrIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertTop(boxId, InvoiceId, $"{VerificationType.HEIGHT_MISMATCH}");

        //                                this.Logger?.Write($"BoxId ={boxId}, Invoice = {InvoiceId}, BoxInfo Height = {boxInfoData.HeightSensor}, Box Current Height = {this.CurrrentHeightSensorValue}, Result = {topBcrResultArgs.Result} HEIGHT_MISMATCH");
        //                            }
        //                        }
        //                        else
        //                        {
        //                            //PlC 속도를 위해 우선처리
        //                            EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, topBcrResultArgs);
        //                            bcrLcdIndexArgs.BcrIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertTop(boxId, InvoiceId, $"{VerificationType.NOREAD}");

        //                            this.Logger?.Write($"BoxId ={boxId}, Invoice = {InvoiceId}, Result = {topBcrResultArgs.Result} is Not Exist BoxInfos");
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                //PlC 속도를 위해 우선처리
        //                EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, topBcrResultArgs);
        //                bcrLcdIndexArgs.BcrIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertTop(boxId, InvoiceId, $"{VerificationType.MISMATCH}");
        //                this.Logger?.Write($"BoxId ={boxId} Invoice is misMatch(TopBcrRead:{InvoiceId}, ProductInfo{info.INVOICE_ID})");

        //                this.BcrAlarmSet(true, WeightInvoicBcrEnum.Top);

        //                if (info.MNL_PACKING_FLAG == $"{MNL_PACKING_FLAGEnum.N}")
        //                    this.BcrAlarmSet(true, WeightInvoicBcrEnum.SmartLabel);
        //                else
        //                    this.BcrAlarmSet(true, WeightInvoicBcrEnum.NormalLabel);

        //                this.AlarmSetTouchSend("Top BCR 미스매치 발생");
        //            }
        //        }
        //        else
        //        {
        //            //PlC 속도를 위해 우선처리
        //            EcsServerAppManager.Instance.Hub.Send(HubServiceName.PlcWeightInvoiceEquipment, topBcrResultArgs);
        //            bcrLcdIndexArgs.BcrIndex = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertTop(boxId, InvoiceId, $"{VerificationType.NOREAD}");
        //            this.Logger?.Write($"BoxId ={boxId} have not ProductInfo");
        //        }
        //        #endregion

        //        EcsServerAppManager.Instance.Hub.Send(HubServiceName.TouchPcBcrLcdEquipment, bcrLcdIndexArgs);
        //        EcsServerAppManager.Instance.Hub.Send(HubServiceName.RicpPost, topBcrResultArgs);
        //    }
        //    catch (Exception ex)
        //    {
        //        this.Logger?.Write(ex.Message);
        //    }
        //}
        #endregion

        #endregion
        #endregion
    }


    [Serializable]
    public class TopBcrEquipmentSetting : EquipmentSetting
    {
        public new TopAndSideBcrCommunicatorSetting CommunicatorSetting
        {
            get => base.CommunicatorSetting as TopAndSideBcrCommunicatorSetting;
            set => base.CommunicatorSetting = value;
        }


        public TopBcrEquipmentSetting()
        {
            this.CommunicatorSetting = new TopAndSideBcrCommunicatorSetting();
            this.Name = HubServiceName.TopBcrEquipment;
            this.Id = "GUB21";
        }
    }
}
