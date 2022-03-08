using ECS.Core.Managers;
using ECS.Core.Util;
using ECS.Model;
using ECS.Model.Plc;
using ECS.Model.Hub;
using System;
using System.IO;
using Urcis.SmartCode;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode.Io;
using Urcis.SmartCode.Events;
using Urcis.SmartCode.Net.Tcp;
using ECS.Model.Pcs;
using ECS.Model.Restfuls;
using System.Linq;
using System.Text.RegularExpressions;
using ECS.Model.Databases;
using ECS.Core.Communicators;
using System.Threading.Tasks;

namespace ECS.Core.Equipments
{
    public class PlcSmartPackingEquipment : PlcGeneralEquipment
    {
        #region Field
        private int BcrNoReadCount = 0;
        #endregion

        #region Prop
        private PlcSmartPackingIoHandler PlcSmartPackingIoHandler { get; set; } = new PlcSmartPackingIoHandler();
        public DeviceStatus SmartPackingDeviceStatus { get; private set; } = new DeviceStatus();
        public BcrCommunicator Bcr { get; set; }

        public new PlcSmartPackingSetting Setting
        {
            get => base.Setting as PlcSmartPackingSetting;
            private set => base.Setting = value;
        }
        #endregion

        #region Ctor
        public PlcSmartPackingEquipment(PlcSmartPackingSetting setting) : base(setting)
        {
            this.Setting = setting ?? new PlcSmartPackingSetting();

            DeviceInfo info = new DeviceInfo();
            info.deviceId = this.Setting.Id;
            info.deviceName = "친환경충진기";
            info.deviceTypeId = "EQ";
            info.deviceTypeName = "설비";
            this.SmartPackingDeviceStatus.deviceList.Add(info);
        }
        #endregion

        #region Method
        protected override void OnSetPlcIoHandler(string groupName)
        {
            try
            {
                base.OnSetPlcIoHandler(groupName);
                string blockName = string.Empty;
                string communicatorName = this.Communicator.Name;

                #region PLC Priority
                blockName = "PLC WORD";
                this.PlcSmartPackingIoHandler.IoPointSmartPackingEquipmentStatuse.StatusCd = IoServer.GetPoint(communicatorName, blockName, $"SmartPacking Equipment Status - StatusCd");
                this.PlcSmartPackingIoHandler.IoPointSmartPackingEquipmentStatuse.ErrorMsg = IoServer.GetPoint(communicatorName, blockName, $"SmartPacking Equipment Status - ErrorMsg");
                this.PlcSmartPackingIoHandler.InputResult = IoServer.GetHandler<DataEventIoHandler>(groupName, $"Input Result");
                this.PlcSmartPackingIoHandler.OutputResult = IoServer.GetHandler<DataEventIoHandler>(groupName, $"Output Result");
                #endregion

                #region PC Priority
                blockName = "SERVER BIT Command";
                this.PlcSmartPackingIoHandler.BcrAlarm = IoServer.GetPoint(communicatorName, blockName, $"BCR Alarm");

                blockName = "SERVER WORD";
                this.PlcSmartPackingIoHandler.BCRReadingResult = IoServer.GetHandler<DataCommandIoHandler>(groupName, $"BCR Reading Result");
                #endregion

            }
            catch (Exception ex)
            {
                this.Logger.Write(ex.Message);
            }
        }

        protected override void OnSubscribePlcIoHandler()
        {
            try
            {
                base.OnSubscribePlcIoHandler();

                if (this.PlcSmartPackingIoHandler.IoPointSmartPackingEquipmentStatuse.StatusCd != null) 
                    this.PlcSmartPackingIoHandler.IoPointSmartPackingEquipmentStatuse.StatusCd.ValueChanged += EquipmentStatusCd_ValueChanged;

                if (this.PlcSmartPackingIoHandler.InputResult != null)
                    this.PlcSmartPackingIoHandler.InputResult.DataRead += InputResult_DataRead;

                if (this.PlcSmartPackingIoHandler.OutputResult != null)
                    this.PlcSmartPackingIoHandler.OutputResult.DataRead += OutputResult_DataRead;          
            }
            catch (Exception ex)
            {
                this.Logger.Write(ex.Message);
            }
        }

        protected override void OnUnSubscribePlcIoHandler()
        {
            try
            {
                base.OnUnSubscribePlcIoHandler();

                if (this.PlcSmartPackingIoHandler.IoPointSmartPackingEquipmentStatuse.StatusCd != null)
                    this.PlcSmartPackingIoHandler.IoPointSmartPackingEquipmentStatuse.StatusCd.ValueChanged -= EquipmentStatusCd_ValueChanged;

                if (this.PlcSmartPackingIoHandler.InputResult != null)
                    this.PlcSmartPackingIoHandler.InputResult.DataRead -= InputResult_DataRead;

                if (this.PlcSmartPackingIoHandler.OutputResult != null)
                    this.PlcSmartPackingIoHandler.OutputResult.DataRead -= OutputResult_DataRead;
            }
            catch (Exception ex)
            {
                this.Logger.Write(ex.Message);
            }
        }

        protected override void OnSetOwnerPlcIoHandler()
        {
            base.OnSetOwnerPlcIoHandler();

            try
            {
                #region PLC Priority
                if (this.PlcSmartPackingIoHandler.InputResult != null)
                    this.PlcSmartPackingIoHandler.InputResult.Owner = this;

                if (this.PlcSmartPackingIoHandler.OutputResult != null)
                    this.PlcSmartPackingIoHandler.OutputResult.Owner = this;
                #endregion

                #region PC Priority
                if (this.PlcSmartPackingIoHandler.BCRReadingResult != null)
                    this.PlcSmartPackingIoHandler.BCRReadingResult.Owner = this;
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        #region Interface
        protected override void OnCreate()
        {
            this.LifeState = LifeCycleStateEnum.Creating;

            base.OnCreate();
            try
            {
                this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.PlcSmartPackingLog));

                this.OnSetPlcIoHandler(this.Setting.HandlerGroupName);

                this.Bcr = new BcrCommunicator(this);
                this.Setting.BcrCommunicatorSetting.LogDirectory = this.Logger.DirectoryPath;
                this.Bcr?.ApplySetting(this.Setting.BcrCommunicatorSetting);
            }
            catch (Exception ex) { this.Logger?.Write(ex.Message); }

            this.LifeState = LifeCycleStateEnum.Created;
        }
        protected override bool OnPrepare()
        {
            if (this.LifeState != LifeCycleStateEnum.Created)
                this.Create();

            base.OnPrepare();

            try
            {
                this.OnSubscribePlcIoHandler();
                this.OnSetOwnerPlcIoHandler();

                if (this.Bcr != null && this.Bcr.IsDisposed == false)
                {
                    this.Bcr.Noread += this.Bcr_Noread;
                    this.Bcr.ReadData += this.Bcr_ReadData;
                    this.Bcr.TcpConnectionStateChanged += this.Bcr_TcpConnectionStateChanged;
                    this.Bcr.OperationStateChanged += this.Bcr_OperationStateChanged;
                }
            }
            catch (Exception ex) { this.Logger?.Write(ex.Message); }

            this.LifeState = LifeCycleStateEnum.Prepared;

            return true;
        }
        protected override void OnTerminate()
        {
            this.LifeState = LifeCycleStateEnum.Terminating;
            this.Stop();

            base.OnTerminate();

            try
            {
                this.OnUnSubscribePlcIoHandler();
                this.Communicator.Terminate();

                if (this.Bcr != null && this.Bcr.IsDisposed == false)
                {
                    this.Bcr.Noread -= this.Bcr_Noread;
                    this.Bcr.ReadData -= this.Bcr_ReadData;
                    this.Bcr.TcpConnectionStateChanged -= this.Bcr_TcpConnectionStateChanged;
                    this.Bcr.OperationStateChanged -= this.Bcr_OperationStateChanged;
                    this.Bcr.Dispose();
                }
            }
            catch (Exception ex) { this.Logger?.Write(ex.Message); }

            this.LifeState = LifeCycleStateEnum.Terminated;
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (this.LifeState != LifeCycleStateEnum.Prepared)
            {
                this.Logger?.Write($"Prepare Falut : {this.LifeState}");
                return;
            }

            if (this.Bcr != null || (this.Bcr.IsDisposed == false))
            {
                if (this.Bcr.TcpConnectionState == TcpConnectionStateEnum.Disconnected)
                {
                    //동기가 느려서 비동기로 변경
                    //this.Communicator?.Start();
                    Task.Run(() => this.Bcr?.Start());
                    this.Logger?.Write("Bcr Communicator Start Async");
                }
            }
        }

        protected override void OnStop()
        {
            if (this.Bcr != null || (this.Bcr.IsDisposed == false))
            {
                if (this.Bcr.TcpConnectionState == TcpConnectionStateEnum.Connected)
                {
                    this.Bcr.Stop();
                    this.Logger?.Write("Bcr Communicator Stop");
                }
            }
        }
        #endregion

        #region Bcr
        public override void OnEquipmentConnectionUpdateTouchSend()
        {
            base.OnEquipmentConnectionUpdateTouchSend();

            try
            {
                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcSmartPackingEquipment>();
                if (eq != null)
                {
                    short eqstate = (short)this.PlcSmartPackingIoHandler.IoPointSmartPackingEquipmentStatuse.StatusCd.Value;

                    if (Enum.TryParse(eqstate.ToString(), out EquipmentStateEnum stateEnum))
                    {
                        bool erectorConnection = false;
                        switch (stateEnum)
                        {
                            case EquipmentStateEnum.Run:
                            case EquipmentStateEnum.Standby:
                            case EquipmentStateEnum.Error:
                                erectorConnection = true;
                                break;
                        }

                        eq.SmartPackingConnectionState.SmartPackingConnection = erectorConnection;

                    }
                    eq.SmartPackingConnectionState.SmartPackingBcrConnection = this.Bcr.TcpConnectionState == TcpConnectionStateEnum.Connected ? true : false;
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
            base.OnEquipmentStateRicpPost();

            try
            {
                if (this.IsConnected)
                {
                    for (int i = 0; i < this.SmartPackingDeviceStatus.deviceList.Count; i++)
                    {
                        var device = this.SmartPackingDeviceStatus.deviceList[i];

                        if (this.PlcSmartPackingIoHandler.IoPointSmartPackingEquipmentStatuse != null)
                        {
                            int statusCd = this.PlcSmartPackingIoHandler.IoPointSmartPackingEquipmentStatuse.StatusCd.GetInt16();
                            device.deviceStatusCd = statusCd;

                            if (statusCd == 0 || statusCd == 9)
                                device.deviceErrorMsg = "PLC Disconnect";
                            else
                                device.deviceErrorMsg = string.Empty;
                        }
                    }
                }
                else
                {
                    foreach (var device in this.SmartPackingDeviceStatus.deviceList)
                    {
                        device.deviceStatusCd = (int)EquipmentStateEnum.Error;
                        device.deviceErrorMsg = "PLC Disconnect";
                    }
                }

                var restfulRequseter = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                restfulRequseter.DeviceStatusPostAsync(this.SmartPackingDeviceStatus);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        #endregion
        public void BcrAlarmSet(bool isOn)
        {
            try
            {
                this.PlcSmartPackingIoHandler.BcrAlarm.SetValue(isOn);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        private void AlarmSetTouchSend(string reason)
        {
            try
            {
                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcSmartPackingEquipment>();
                if (eq != null)
                {
                    BcrAlarmSetReset bcrAlarmSetReset = new BcrAlarmSetReset();
                    bcrAlarmSetReset.Reason = reason;
                    bcrAlarmSetReset.AlarmResult = true;

                    eq.OnBcrAlarmSetResetSend(bcrAlarmSetReset);
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void BcrReadingResultExcute(string boxId, BcrResult result, SmartPackingNGCode ngCode)
        {
            try
            {
                ActUtlBCRReadingResult actUtlBCRReadingResult = new ActUtlBCRReadingResult();
                actUtlBCRReadingResult.BoxID = boxId;
                if (!string.IsNullOrEmpty(boxId))
                {
                    actUtlBCRReadingResult.BoxType = boxId.Substring(1, 1);
                }
                actUtlBCRReadingResult.Result = (short)result;
                actUtlBCRReadingResult.NGCode = (short)ngCode;

                this.PlcSmartPackingIoHandler.BCRReadingResult.Execute(actUtlBCRReadingResult);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        public void TouchManualBoxValidationRequest(SmartPackingManualBoxValidationRequest request)
        {
            try
            {
                this.Logger?.Write($"Touch Manual Box Validation : BoxId = {request.BoxId}, BufferCount = {request.BufferCount}");

                var data = EcsServerAppManager.Instance.DataBaseManagerForServer.UpdateSmartPacking(request.BoxId, true, request.BufferCount);

                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcSmartPackingEquipment>();
                if (eq != null)
                {
                    eq.SmartPackingDbIndexSend(data.INDEX);
                }

                if (EcsServerAppManager.Instance.Cache.ProductInfos.ContainsKey(request.BoxId))
                {
                    ProductInfo info = EcsServerAppManager.Instance.Cache.ProductInfos[request.BoxId];
                    info.TASK_ID = $"{TASK_IDEnum.Operator_Packing_Result}";
                }

                this.OutputDataCombine(data, false);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        public bool IsAlarmSet()
        {
            try
            {
                return this.PlcSmartPackingIoHandler.BcrAlarm.GetBoolean();
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
                return false;
            }
        }

        private string GetUriPath(string boxId, string result)
        {
            string path = EcsServerAppManager.Instance.Setting.PackageResultImagePath;
            string extension = EcsServerAppManager.Instance.Setting.PackageResultImageExtension;
            string year = DateTime.Now.ToString("yyyy");

            return $"{path}/{year}/{result}/2D_Image/{boxId}.{extension}";
        }
        #endregion

        #region Event Handler

        private void InputResult_DataRead(object sender, DataEventIoHandlerDataReadEventArgs e)
        {
            try
            {
                if (e.Data == null) return;

                ActUtlInputResultObject actUtlInputResultObject = e.Data[0] as ActUtlInputResultObject;

                double volume = Scale.GetDecimalConversion(actUtlInputResultObject.Volume, 1);
                double height = Scale.GetDecimalConversion(actUtlInputResultObject.Height, 4);

                if (Enum.TryParse($"{actUtlInputResultObject.Result}", out BcrResult bcrResult)) { }
                if (Enum.TryParse($"{actUtlInputResultObject.NGCode}", out SmartPackingNGCode ngCode)) { }

                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcSmartPackingEquipment>();

                long index = -1;

                if (string.IsNullOrEmpty(actUtlInputResultObject.BoxID))
                {
                    #region BoxID가 없을때 
                    index = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertSmartPacking(actUtlInputResultObject.BoxID, $"{SmartPackingNGCode.NOREAD}", volume, height);

                    this.InputDataCombine(actUtlInputResultObject, volume, height);

                    this.Logger?.Write($"Input Result Requset : BoxId = {actUtlInputResultObject.BoxID}, Invoice =  , BoxType = {actUtlInputResultObject.BoxType}, " +
                           $"Volume = {volume}, Height = {height}, Result = {bcrResult}, NG Code = {ngCode} is Noread");
                    #endregion
                }
                else if (EcsServerAppManager.Instance.Cache.ProductInfos.ContainsKey(actUtlInputResultObject.BoxID))
                {
                    #region  메모리가 있을때
                    ProductInfo info = EcsServerAppManager.Instance.Cache.ProductInfos[actUtlInputResultObject.BoxID];

                    if (bcrResult == BcrResult.BYPASS ||
                        info.TASK_ID == $"{(int)TASK_IDEnum.SmartPacking_Success}" ||
                        info.TASK_ID == $"{(int)TASK_IDEnum.Operator_Packing_Result}")
                    {
                        // TASKID 보고된 이력이 있어서 보고 안함
                        index = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertSmartPacking(actUtlInputResultObject.BoxID, $"{BcrResult.BYPASS}", volume, height);

                        if (eq != null)
                        {
                            eq.SmartPackingDbIndexSend(index);
                        }

                        this.Logger?.Write($"Input Result Requset : BoxId = {actUtlInputResultObject.BoxID}, Invoice = {info.INVOICE_ID}, Result = {BcrResult.BYPASS}, TASKID = {info.TASK_ID}, is {BcrResult.BYPASS}");

                        return;
                    }
                    else if (bcrResult == BcrResult.Reject)
                    {
                        index = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertSmartPacking(actUtlInputResultObject.BoxID, $"{ngCode}", volume, height);

                        this.InputDataCombine(actUtlInputResultObject, volume, height);

                        this.Logger?.Write($"Input Result Requset : BoxId = {actUtlInputResultObject.BoxID}, Invoice = {info.INVOICE_ID}, BoxType = {actUtlInputResultObject.BoxType}, " +
                            $"Volume = {volume}, Height = {height}, Result = {bcrResult}, NG Code = {ngCode}, is {BcrResult.Reject}");
                    }
                    else if (bcrResult == BcrResult.OK)
                    {
                        index = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertSmartPacking(actUtlInputResultObject.BoxID, $"{BcrResult.OK}", volume, height);

                        this.Logger?.Write($"Input Result Requset : BoxId = {actUtlInputResultObject.BoxID}, Invoice = {info.INVOICE_ID}, BoxType = {actUtlInputResultObject.BoxType}, " +
                           $"Volume = {volume}, Height = {height}, Result = {BcrResult.OK}, NG Code = {ngCode} is {BcrResult.OK}");
                    }
                    else
                    {
                        index = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertSmartPacking(actUtlInputResultObject.BoxID, $"UNKNOWN", volume, height);

                        this.InputDataCombine(actUtlInputResultObject, volume, height);

                        this.Logger?.Write($"Input Result Requset : BoxId = {actUtlInputResultObject.BoxID}, Invoice = {info.INVOICE_ID}, BoxType = {actUtlInputResultObject.BoxType}, " +
                           $"Volume = {volume}, Height = {height}, Result = UNKNOWN, NG Code = {ngCode} is Result UNKNOWN");
                    }
                    #endregion
                }
                else
                {
                    #region 메모리 없을 때 
                    index = EcsServerAppManager.Instance.DataBaseManagerForServer.InsertSmartPacking(actUtlInputResultObject.BoxID, $"{SmartPackingNGCode.NODATA}", volume, height);

                    this.InputDataCombine(actUtlInputResultObject, volume, height);

                    this.Logger?.Write($"Input Result Requset : BoxId = {actUtlInputResultObject.BoxID}, Invoice =  , BoxType = {actUtlInputResultObject.BoxType}, " +
                           $"Volume = {volume}, Height = {height}, Result = {bcrResult}, NG Code = {ngCode} have not ProductInfo");
                    #endregion
                }

                // TouchPC 보고
                if (eq != null)
                {
                    eq.SmartPackingDbIndexSend(index);
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void InputDataCombine(ActUtlInputResultObject resultObject, double volume, double height)
        {
            try
            {
                PackageResult packageResult = new PackageResult();
                packageResult.containerCode = resultObject.BoxID;
                packageResult.containerTypeCd = resultObject.BoxType;
                string inputTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff");
                packageResult.containerInputTime = Convert.ToDateTime(inputTime);
                packageResult.resultCode = $"{resultObject.NGCode}";
                packageResult.resultInVolume = $"{volume}";
                packageResult.resultHeight = $"{height}";
                packageResult.resultManualWork = false;
                packageResult.resultByPass = false;
                packageResult.resultBufferCount = 0;
                packageResult.resultOutputTime = packageResult.containerInputTime;
                packageResult.resultImagePath = this.GetUriPath(resultObject.BoxID, "NG");

                this.PostPackageresult(packageResult);
            }
            catch (Exception ex)
            {
                Log.WriteException(ex.ToString());
            }
            
        }

        private void OutputDataCombine(Model.Domain.Touch.SmartPackingData smartPackingData, bool byPass)
        {
            try
            {
                PackageResult packageResult = new PackageResult();
                packageResult.containerCode = smartPackingData.BOX_ID;
                packageResult.containerTypeCd = smartPackingData.BOX_TYPE;
                if (smartPackingData.INSERT_TIME != null)
                    packageResult.containerInputTime = Convert.ToDateTime(smartPackingData.INSERT_TIME);
                if (smartPackingData.VOLUME != null)
                    packageResult.resultInVolume = $"{smartPackingData.VOLUME}";
                if (smartPackingData.HEIGHT != null)
                    packageResult.resultHeight = $"{smartPackingData.HEIGHT}";
                if (smartPackingData.IS_MANUAL != null)
                    packageResult.resultManualWork = Convert.ToBoolean(smartPackingData.IS_MANUAL);
                packageResult.resultByPass = byPass;
                if (smartPackingData.PACKING_AMOUNT != null)
                    packageResult.resultBufferCount = Convert.ToDouble(smartPackingData.PACKING_AMOUNT);
                if (smartPackingData.OUT_TIME != null)
                    packageResult.resultOutputTime = Convert.ToDateTime(smartPackingData.OUT_TIME);

                if (Enum.TryParse($"{smartPackingData.RESULT}", out SmartPackingDBResult dbResult)) { }
                SmartPackingRicpResult ricpResult = (SmartPackingRicpResult)Enum.ToObject(typeof(SmartPackingRicpResult), (short)dbResult);
                string urlPath = string.Empty;

                if ((short)ricpResult == 0)
                {
                    packageResult.resultCode = string.Empty;
                    urlPath = this.GetUriPath(smartPackingData.BOX_ID, "OK");
                }
                else if ((short)ricpResult == 6)
                {
                    packageResult.resultCode = string.Empty;
                }
                else
                {
                    smartPackingData.RESULT = $"{(int)ricpResult}";
                    urlPath = this.GetUriPath(smartPackingData.BOX_ID, "NG");
                }

                packageResult.resultImagePath = urlPath;

                this.PostPackageresult(packageResult);
            }
            catch (Exception ex)
            {
                Log.WriteException(ex.ToString());
            }
        }

        private void PostPackageresult(PackageResult packageResult)
        {
            try
            {
                var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                if (manager != null)
                {
                    manager.PackageResultPostAsync(packageResult);
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex.ToString());
            }
        }

        private void OutputResult_DataRead(object sender, DataEventIoHandlerDataReadEventArgs e)
        {
            try
            {
                if (e.Data == null) return;

                ActUtlOutputResultObject actUtlOutputResultObject = e.Data[0] as ActUtlOutputResultObject;

                if (Enum.TryParse($"{actUtlOutputResultObject.Result}", out BcrResult bcrResult)) { }

                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcSmartPackingEquipment>();

                var data = EcsServerAppManager.Instance.DataBaseManagerForServer.UpdateSmartPacking(actUtlOutputResultObject.BoxID, false, actUtlOutputResultObject.BufferCount);

                bool byPass = false;

                if (eq != null)
                {
                    eq.SmartPackingDbIndexSend(data.INDEX);
                }

                if (EcsServerAppManager.Instance.Cache.ProductInfos.ContainsKey(actUtlOutputResultObject.BoxID))
                {
                    #region  메모리가 있을때
                    ProductInfo info = EcsServerAppManager.Instance.Cache.ProductInfos[actUtlOutputResultObject.BoxID];

                    if (bcrResult == BcrResult.BYPASS ||
                        info.TASK_ID == $"{(int)TASK_IDEnum.SmartPacking_Success}" ||
                        info.TASK_ID == $"{(int)TASK_IDEnum.Operator_Packing_Result}")
                    {
                        byPass = true;

                        this.Logger?.Write($"Output Result Requset : BoxId = {actUtlOutputResultObject.BoxID}, Invoice = {info.INVOICE_ID}, Result = {BcrResult.BYPASS}, TASKID = {info.TASK_ID}, is {BcrResult.BYPASS}");
                    }
                    else if (bcrResult == BcrResult.Reject)
                    {
                        info.TASK_ID = $"{(int)TASK_IDEnum.SmartPacking_Success}";
                        this.Logger?.Write($"Output Result Requset : BoxId = {actUtlOutputResultObject.BoxID}, Invoice = {info.INVOICE_ID}, Result = {BcrResult.Reject}, is {BcrResult.Reject}");
                    }
                    else if (bcrResult == BcrResult.OK)
                    {
                        info.TASK_ID = $"{(int)TASK_IDEnum.SmartPacking_Success}";
                        this.Logger?.Write($"Output Result Requset : BoxId = {actUtlOutputResultObject.BoxID}, Invoice = {info.INVOICE_ID}, Result = {BcrResult.OK}, is {BcrResult.OK}");
                    }
                    else
                    {
                        info.TASK_ID = $"{(int)TASK_IDEnum.SmartPacking_Success}";
                        this.Logger?.Write($"Output Result Requset : BoxId = {actUtlOutputResultObject.BoxID}, Invoice = {info.INVOICE_ID}, Result = UNKNOWN,  is Result UNKNOWN");
                    }
                    #endregion   
                }
                else
                {
                    this.Logger?.Write($"Output Result Requset : BoxId = {actUtlOutputResultObject.BoxID}, Result = {bcrResult}, BufferCount = {actUtlOutputResultObject.BufferCount} have not ProductInfo");
                }

                this.OutputDataCombine(data, byPass);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void EquipmentStatusCd_ValueChanged(object sender, IoPointValueChangedEventArgs e)
        {
            try
            {
                if (sender is IoPoint ioPoint)
                {
                    int errorMsg = -1;

                    var device = this.SmartPackingDeviceStatus.deviceList[0];
                    device.deviceStatusCd = e.GetCurrentInt16();
                    errorMsg = this.PlcSmartPackingIoHandler.IoPointSmartPackingEquipmentStatuse.ErrorMsg.GetInt16();
                    device.deviceErrorMsg = errorMsg.ToString();

                    #region Touch PC Send
                    var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcSmartPackingEquipment>();
                    if (eq != null)
                    {
                        if (Enum.TryParse(device.deviceStatusCd.ToString(), out EquipmentStateEnum stateEnum))
                        {
                            bool erectorConnection = false;
                            switch (stateEnum)
                            {
                                case EquipmentStateEnum.Run:
                                case EquipmentStateEnum.Standby:
                                case EquipmentStateEnum.Error:
                                    erectorConnection = true;
                                    break;
                            }

                            eq.SmartPackingConnectionState.SmartPackingConnection = erectorConnection;
                        }
                        eq.EquipmentCoonectionStateSend();
                    }
                    #endregion

                    #region RICP Send
                    var restfulRequseter = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                    restfulRequseter.DeviceStatusPostAsync(this.SmartPackingDeviceStatus);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        #region Bcr
        private void Bcr_TcpConnectionStateChanged(object sender, Urcis.SmartCode.Net.Tcp.TcpConnectionStateChangedEventArgs e)
        {
            if (e.Current == e.Previous) return;

            try
            {
                #region Bcr_TcpConnectionStateChanged
                this.Logger?.Write($"Bcr TcpConnectionStateChanged = {e.Current}");

                if (e.Current == TcpConnectionStateEnum.Connected)
                    this.BcrAlarmSet(false);
                else
                {
                    this.BcrAlarmSet(true);
                    this.AlarmSetTouchSend("BCR 미연결");
                }
             
                this.OnEquipmentConnectionUpdateTouchSend();
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void Bcr_OperationStateChanged(object sender, Urcis.SmartCode.Net.CommunicatorOperationStateChangedEventArgs e)
        {
            if (e.Current == e.Previous) return;

            this.Logger?.Write($"Bcr OperationStateChanged = {e.Current}");
        }

        private void Bcr_ReadData(object sender, string boxId)
        {
            this.Logger?.Write($"Bcr_ReadData = {boxId}");

            try
            {
                #region Bcr Alarm
                if (this.BcrNoReadCount >= 3)
                    this.BcrAlarmSet(false);

                this.BcrNoReadCount = 0;
                #endregion

                //출고에서 나간걸 다시 태울때가 있어서 BYPASS를 정보있을때만 체크
                if (EcsServerAppManager.Instance.Cache.ProductInfos.ContainsKey(boxId))
                {
                    // DB bypass
                    bool isbypass = EcsServerAppManager.Instance.DataBaseManagerForServer.SelectManualPacking(boxId);

                    if (isbypass)
                    {
                        this.BcrReadingResultExcute(boxId, BcrResult.BYPASS, SmartPackingNGCode.OK);
                        this.Logger?.Write($"BCR Reading Result Requset : BoxId = {boxId} is {BcrResult.BYPASS}");

                        return;
                    }

                    #region 정보고가 있을때
                    ProductInfo info = EcsServerAppManager.Instance.Cache.ProductInfos[boxId];
                    if (Enum.TryParse(info.MNL_PACKING_FLAG, out MNL_PACKING_FLAGEnum mnl_packing_flag)) { }

                    // RouteLogical에서 중량검수를 하지 않으면 무조건 SmartPacking에서 빼게되서 수정
                    var dynamicEq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<DynamicScaleEquipment>();
                    if (dynamicEq != null)
                    {
                        if (this.NoWeightCheck(info.STATUS, info.BOX_WT))
                        {
                            // NoWeight
                            info.TASK_ID = $"{(int)TASK_IDEnum.SmartPacking_Fail}";
                            this.BcrReadingResultExcute(boxId, BcrResult.Reject, SmartPackingNGCode.NOWEIGHT);
                            this.Logger?.Write($"BCR Reading Result Excute : BoxId = {boxId}, Invoice = {info.INVOICE_ID}, is {SmartPackingNGCode.NOWEIGHT}");
                        }
                        else if (dynamicEq.WeightFailCheck(info.WT_CHECK_FLAG, info.BOX_WT, info.WEIGHT_SUM) ||
                                 info.STATUS == $"{(int)statusEnum.insepct_reject_scale}")
                        {
                            // Weight Fail
                            info.TASK_ID = $"{(int)TASK_IDEnum.SmartPacking_Fail}";

                            this.BcrReadingResultExcute(boxId, BcrResult.Reject, SmartPackingNGCode.WEIGHT_FAIL);
                            this.Logger?.Write($"BCR Reading Result Excute : BoxId = {boxId}, Invoice = {info.INVOICE_ID}, is {SmartPackingNGCode.WEIGHT_FAIL}");
                        }
                        else if (mnl_packing_flag == MNL_PACKING_FLAGEnum.Y)
                        {
                            // 개별 포장이면
                            info.TASK_ID = $"{(int)TASK_IDEnum.Operator_Packing_Result}";
                            this.BcrReadingResultExcute(boxId, BcrResult.BYPASS, SmartPackingNGCode.OK);
                            this.Logger?.Write($"BCR Reading Result Excute : BoxId = {boxId}, Invoice = {info.INVOICE_ID}, is mnl Packing flag Normal");
                        }
                        else
                        {
                            // OK
                            this.BcrReadingResultExcute(boxId, BcrResult.OK, SmartPackingNGCode.OK);
                            this.Logger?.Write($"BCR Reading Result Excute : BoxId = {boxId}, Invoice = {info.INVOICE_ID}, is {SmartPackingNGCode.OK}");
                        }
                    }
                    #endregion
                }
                else
                {
                    // 메모리 없을 때
                    this.BcrReadingResultExcute(boxId, BcrResult.Reject, SmartPackingNGCode.NODATA);
                    this.Logger?.Write($"BCR Reading Result Excute : BoxId = {boxId} have not ProductInfo");
                }
            }
            catch (Exception ex)
            {
                this.Logger.Write(ex.Message);
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

        private void Bcr_Noread(object sender, EventArgs e)
        {
            try
            {
                #region BcrNoread
                this.Logger?.Write($"Bcr_ReadData = Noread");

                if (this.BcrNoReadCount <= 3)
                    this.BcrNoReadCount++;

                if (this.BcrNoReadCount >= 3)
                {
                    this.BcrAlarmSet(true);
                    this.AlarmSetTouchSend("BCR 미인식 3회");
                }
                    
                this.BcrReadingResultExcute(string.Empty, BcrResult.Reject, SmartPackingNGCode.NOREAD);
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        #endregion
        #endregion
    }

    [Serializable]
    public class PlcSmartPackingSetting : PlcEquipmentSetting
    {
        public BcrCommunicatorSetting BcrCommunicatorSetting { get; set; } = new BcrCommunicatorSetting();

        public PlcSmartPackingSetting()
        {
            this.Id = "GP21";
        }
    }
}
