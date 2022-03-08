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
using ECS.Model.Pcs;
using ECS.Model.Restfuls;
using System.Linq;
using System.Text.RegularExpressions;

namespace ECS.Core.Equipments
{
    public class PlcWeightInvoiceEquipment : PlcGeneralEquipment
    {
        #region Prop
        private PlcWeightInvoiceIoHandler PlcWeightInvoiceIoHandler { get; set; } = new PlcWeightInvoiceIoHandler();

        public WeightInvoiceRatio WeightInvoiceRatio { get; set; } = new WeightInvoiceRatio();
        private WeightInvoiceCvSpeed[] WeightInvoiceCvSpeed { get; set; }
        #endregion

        #region Ctor
        public PlcWeightInvoiceEquipment(PlcEquipmentSetting setting) : base(setting) 
        {
            this.WeightInvoiceCvSpeed = new WeightInvoiceCvSpeed[this.PlcWeightInvoiceIoHandler.IoPointCVSpeeds.Length];
            for (int i = 0; i < this.WeightInvoiceCvSpeed.Length; i++)
            {
                this.WeightInvoiceCvSpeed[i] = new WeightInvoiceCvSpeed();
            }
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
                blockName = "PLC BIT Event";
                for (int i = 0; i < this.PlcWeightInvoiceIoHandler.IoPointRollTainers.Length; i++)
                {
                    for (int j = 0; j < this.PlcWeightInvoiceIoHandler.IoPointRollTainers[i].Sensors.Length; j++)
                        this.PlcWeightInvoiceIoHandler.IoPointRollTainers[i].Sensors[j] = IoServer.GetPoint(communicatorName, blockName, $"RollTainer#{i + 5} - Sensor#{j + 1}");
                }

                this.PlcWeightInvoiceIoHandler.P500EMGBitEvent = IoServer.GetHandler<BitEventIoHandler>(groupName, $"P500 EMG");

                this.PlcWeightInvoiceIoHandler.WeightBCR_CV_StuckOn = IoServer.GetPoint(communicatorName, blockName, $"Weight BCR C/V Stuck On");
                this.PlcWeightInvoiceIoHandler.InvoiceBCR_CV_StuckOn = IoServer.GetPoint(communicatorName, blockName, $"Invoice BCR C/V Stuck On");

                blockName = "PLC WORD";
                for (int i = 0; i < this.PlcWeightInvoiceIoHandler.IoPointCVSpeeds.Length; i++)
                {
                    this.PlcWeightInvoiceIoHandler.IoPointCVSpeeds[i].SV = IoServer.GetPoint(communicatorName, blockName, $"C/V Speed#{i + 1} - SV");
                    this.PlcWeightInvoiceIoHandler.IoPointCVSpeeds[i].PV = IoServer.GetPoint(communicatorName, blockName, $"C/V Speed#{i + 1} - PV");
                }

                this.PlcWeightInvoiceIoHandler.IoPointCurrentSmartBoxRouteMode.IoPointMode = IoServer.GetPoint(communicatorName, blockName, $"Current Smart Box Route Mode - Mode");
                this.PlcWeightInvoiceIoHandler.IoPointCurrentSmartBoxRouteMode.IoPointSmartWayRatio = IoServer.GetPoint(communicatorName, blockName, $"Current Smart Box Route Mode - Smart Way Ratio");
                this.PlcWeightInvoiceIoHandler.IoPointCurrentSmartBoxRouteMode.IoPointNormalWayRatio = IoServer.GetPoint(communicatorName, blockName, $"Current Smart Box Route Mode - Normal Way Ratio");

                this.PlcWeightInvoiceIoHandler.BoxHeightSensorDataEvent = IoServer.GetHandler<DataEventIoHandler>(groupName, $"Box Height Sensor");
                #endregion

                #region PC Priority
                blockName = "SERVER BIT Command";

                this.PlcWeightInvoiceIoHandler.BcrsAlarms[0] = IoServer.GetPoint(communicatorName, blockName, $"BCR#2 Alarm");
                this.PlcWeightInvoiceIoHandler.BcrsAlarms[1] = IoServer.GetPoint(communicatorName, blockName, $"BCR#3 Alarm");
                this.PlcWeightInvoiceIoHandler.BcrsAlarms[2] = IoServer.GetPoint(communicatorName, blockName, $"BCR#4-1 Alarm");
                this.PlcWeightInvoiceIoHandler.BcrsAlarms[3] = IoServer.GetPoint(communicatorName, blockName, $"BCR#4-2 Alarm");
                this.PlcWeightInvoiceIoHandler.BcrsAlarms[4] = IoServer.GetPoint(communicatorName, blockName, $"BCR#5 Alarm");
                this.PlcWeightInvoiceIoHandler.BcrsAlarms[5] = IoServer.GetPoint(communicatorName, blockName, $"BCR#6 Alarm");

                this.PlcWeightInvoiceIoHandler.DynamicScaleAlarm = IoServer.GetPoint(communicatorName, blockName, $"Dynamic Scale Alarm");

                for (int i = 0; i < this.PlcWeightInvoiceIoHandler.InvocieLabelPrinterAlarms.Length; i++)
                    this.PlcWeightInvoiceIoHandler.InvocieLabelPrinterAlarms[i] = IoServer.GetPoint(communicatorName, blockName, $"Invoice Label Printer#{i + 1} Alarm");

                this.PlcWeightInvoiceIoHandler.TopBcrAlarm = IoServer.GetPoint(communicatorName, blockName, $"Top BCR Alarm");

                this.PlcWeightInvoiceIoHandler.WeightInspectionResult = IoServer.GetHandler<DataCommandIoHandler>(groupName, $"Weight Inspection Result");
                this.PlcWeightInvoiceIoHandler.RouteResult = IoServer.GetHandler<DataCommandIoHandler>(groupName, $"Route Result");
                this.PlcWeightInvoiceIoHandler.InvoiceResult = IoServer.GetHandler<DataCommandIoHandler>(groupName, $"Invoice Result");
                this.PlcWeightInvoiceIoHandler.SmartBoxRouteMode = IoServer.GetHandler<DataCommandIoHandler>(groupName, $"Smart Box Route Mode");

                for (int i = 0; i < this.PlcWeightInvoiceIoHandler.CVSpeeds.Length; i++)
                    this.PlcWeightInvoiceIoHandler.CVSpeeds[i] = IoServer.GetHandler<DataCommandIoHandler>(groupName, $"C/V Speed#{i + 1}");

                this.PlcWeightInvoiceIoHandler.OutResult = IoServer.GetHandler<DataCommandIoHandler>(groupName, $"Out Result");

                blockName = "SERVER WORD";
                //RollTainer #5, #6
                for (int i = 0; i < this.PlcWeightInvoiceIoHandler.RollTainersTowerLampIoPoints.Length; i++)
                    this.PlcWeightInvoiceIoHandler.RollTainersTowerLampIoPoints[i] = IoServer.GetPoint(communicatorName, blockName, $"RollTainer#{i + 5} - Tower Lamp On");
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

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (this.PlcWeightInvoiceIoHandler.IoPointRollTainers[i].Sensors[j] != null)
                            this.PlcWeightInvoiceIoHandler.IoPointRollTainers[i].Sensors[j].ValueChanged += RollTainersSensers_ValueChanged;
                    }
                }

                if (this.PlcWeightInvoiceIoHandler.IoPointCurrentSmartBoxRouteMode.IoPointMode != null)
                    this.PlcWeightInvoiceIoHandler.IoPointCurrentSmartBoxRouteMode.IoPointMode.ValueChanged += IoPointMode_ValueChanged;

                if (this.PlcWeightInvoiceIoHandler.IoPointCurrentSmartBoxRouteMode.IoPointSmartWayRatio != null)
                    this.PlcWeightInvoiceIoHandler.IoPointCurrentSmartBoxRouteMode.IoPointSmartWayRatio.ValueChanged += IoPointSmartWayRatio_ValueChanged;

                if (this.PlcWeightInvoiceIoHandler.IoPointCurrentSmartBoxRouteMode.IoPointNormalWayRatio != null)
                    this.PlcWeightInvoiceIoHandler.IoPointCurrentSmartBoxRouteMode.IoPointNormalWayRatio.ValueChanged += IoPointNormalWayRatio_ValueChanged;


                for (int i = 0; i < 20; i++)
                {
                    if (this.PlcWeightInvoiceIoHandler.IoPointCVSpeeds[i].SV != null)
                        this.PlcWeightInvoiceIoHandler.IoPointCVSpeeds[i].SV.ValueChanged += SV_ValueChanged;

                    if (this.PlcWeightInvoiceIoHandler.IoPointCVSpeeds[i].PV != null)
                        this.PlcWeightInvoiceIoHandler.IoPointCVSpeeds[i].PV.ValueChanged += PV_ValueChanged;
                }

                if (this.PlcWeightInvoiceIoHandler.P500EMGBitEvent != null)
                    this.PlcWeightInvoiceIoHandler.P500EMGBitEvent.EventOccurred += P500EMGBitEvent_EventOccurred;

                if (this.PlcWeightInvoiceIoHandler.WeightBCR_CV_StuckOn != null)
                    this.PlcWeightInvoiceIoHandler.WeightBCR_CV_StuckOn.ValueChanged += WeightBCR_CV_StuckOn_ValueChanged;

                if (this.PlcWeightInvoiceIoHandler.InvoiceBCR_CV_StuckOn != null)
                    this.PlcWeightInvoiceIoHandler.InvoiceBCR_CV_StuckOn.ValueChanged += InvoiceBCR_CV_StuckOn_ValueChanged;

                if (this.PlcWeightInvoiceIoHandler.BoxHeightSensorDataEvent != null)
                    this.PlcWeightInvoiceIoHandler.BoxHeightSensorDataEvent.DataRead += BoxHeightSensorDataEvent_DataRead;
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

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (this.PlcWeightInvoiceIoHandler.IoPointRollTainers[i].Sensors[j] != null)
                            this.PlcWeightInvoiceIoHandler.IoPointRollTainers[i].Sensors[j].ValueChanged -= RollTainersSensers_ValueChanged;
                    }
                }

                if (this.PlcWeightInvoiceIoHandler.IoPointCurrentSmartBoxRouteMode.IoPointMode != null)
                    this.PlcWeightInvoiceIoHandler.IoPointCurrentSmartBoxRouteMode.IoPointMode.ValueChanged -= IoPointMode_ValueChanged;

                if (this.PlcWeightInvoiceIoHandler.IoPointCurrentSmartBoxRouteMode.IoPointSmartWayRatio != null)
                    this.PlcWeightInvoiceIoHandler.IoPointCurrentSmartBoxRouteMode.IoPointSmartWayRatio.ValueChanged -= IoPointSmartWayRatio_ValueChanged;

                if (this.PlcWeightInvoiceIoHandler.IoPointCurrentSmartBoxRouteMode.IoPointNormalWayRatio != null)
                    this.PlcWeightInvoiceIoHandler.IoPointCurrentSmartBoxRouteMode.IoPointNormalWayRatio.ValueChanged -= IoPointNormalWayRatio_ValueChanged;


                for (int i = 0; i < 20; i++)
                {
                    if (this.PlcWeightInvoiceIoHandler.IoPointCVSpeeds[i].SV != null)
                        this.PlcWeightInvoiceIoHandler.IoPointCVSpeeds[i].SV.ValueChanged -= SV_ValueChanged;

                    if (this.PlcWeightInvoiceIoHandler.IoPointCVSpeeds[i].PV != null)
                        this.PlcWeightInvoiceIoHandler.IoPointCVSpeeds[i].PV.ValueChanged -= PV_ValueChanged;
                }

                if (this.PlcWeightInvoiceIoHandler.P500EMGBitEvent != null)
                    this.PlcWeightInvoiceIoHandler.P500EMGBitEvent.EventOccurred -= P500EMGBitEvent_EventOccurred;

                if (this.PlcWeightInvoiceIoHandler.WeightBCR_CV_StuckOn != null)
                    this.PlcWeightInvoiceIoHandler.WeightBCR_CV_StuckOn.ValueChanged -= WeightBCR_CV_StuckOn_ValueChanged;

                if (this.PlcWeightInvoiceIoHandler.InvoiceBCR_CV_StuckOn != null)
                    this.PlcWeightInvoiceIoHandler.InvoiceBCR_CV_StuckOn.ValueChanged -= InvoiceBCR_CV_StuckOn_ValueChanged;

                if (this.PlcWeightInvoiceIoHandler.BoxHeightSensorDataEvent != null)
                    this.PlcWeightInvoiceIoHandler.BoxHeightSensorDataEvent.DataRead -= BoxHeightSensorDataEvent_DataRead;
            }
            catch (Exception ex)
            {
                this.Logger.Write(ex.Message);
            }
        }

        protected override void OnSetOwnerPlcIoHandler()
        {
            base.OnSetOwnerPlcIoHandler();

            #region PLC Priority
            if (this.PlcWeightInvoiceIoHandler.P500EMGBitEvent != null)
                this.PlcWeightInvoiceIoHandler.P500EMGBitEvent.Owner = this;

            if (this.PlcWeightInvoiceIoHandler.BoxHeightSensorDataEvent != null)
                this.PlcWeightInvoiceIoHandler.BoxHeightSensorDataEvent.Owner = this;
            #endregion

            #region PC Priority

            if (this.PlcWeightInvoiceIoHandler.WeightInspectionResult.Owner != null)
                this.PlcWeightInvoiceIoHandler.WeightInspectionResult.Owner = this;

            if (this.PlcWeightInvoiceIoHandler.RouteResult.Owner != null)
                this.PlcWeightInvoiceIoHandler.RouteResult.Owner = this;

            if (this.PlcWeightInvoiceIoHandler.InvoiceResult.Owner != null)
                this.PlcWeightInvoiceIoHandler.InvoiceResult.Owner = this;

            if (this.PlcWeightInvoiceIoHandler.SmartBoxRouteMode.Owner != null)
                this.PlcWeightInvoiceIoHandler.SmartBoxRouteMode.Owner = this;

            for (int i = 0; i < 20; i++)
                this.PlcWeightInvoiceIoHandler.CVSpeeds[i].Owner = this;

            if (this.PlcWeightInvoiceIoHandler.OutResult.Owner != null)
                this.PlcWeightInvoiceIoHandler.OutResult.Owner = this;
            #endregion
        }

        private void InitCurrentData()
        {
            try
            {
                #region CurrentSmartBoxRouteMode
                this.WeightInvoiceRatio.NormalWayRatio = this.PlcWeightInvoiceIoHandler.IoPointCurrentSmartBoxRouteMode.IoPointNormalWayRatio.GetInt16();
                this.WeightInvoiceRatio.SmartWayRatio = this.PlcWeightInvoiceIoHandler.IoPointCurrentSmartBoxRouteMode.IoPointSmartWayRatio.GetInt16();
                this.WeightInvoiceRatio.Mode = this.PlcWeightInvoiceIoHandler.IoPointCurrentSmartBoxRouteMode.IoPointMode.GetInt16();
                #endregion

                #region CVSpeed
                for (int i = 0; i < this.PlcWeightInvoiceIoHandler.IoPointCVSpeeds.Length; i++)
                {
                    this.WeightInvoiceCvSpeed[i].Sv = Scale.GetDecimalConversion(this.PlcWeightInvoiceIoHandler.IoPointCVSpeeds[i].SV.GetInt16(), 2);
                    this.WeightInvoiceCvSpeed[i].Pv = Scale.GetDecimalConversion(this.PlcWeightInvoiceIoHandler.IoPointCVSpeeds[i].PV.GetInt16(), 2);
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        public ConveyorCvSpeed GetConveyorCvSpeed(int conveyorSpeed)
        {
            ConveyorCvSpeed conveyorCvSpeed = new ConveyorCvSpeed();
            conveyorCvSpeed.ConveyorSpeed = conveyorSpeed;

            try
            {  //제함 ConveyorSpeed 5개 제외
                conveyorSpeed -= 5;

                if (this.WeightInvoiceCvSpeed.Length > conveyorSpeed && conveyorSpeed >= 0)
                {
                    var cvSpeed = this.WeightInvoiceCvSpeed[conveyorSpeed];
                    conveyorCvSpeed.Pv = cvSpeed.Pv;
                    conveyorCvSpeed.Sv = cvSpeed.Sv;
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }

            return conveyorCvSpeed;
        }

        public void ConveyorCvSpeedExcute(ConveyorCvSpeed conveyorSpeed)
        {
            try
            {
                if (this.PlcWeightInvoiceIoHandler.CVSpeeds != null)
                {
                    //제함 ConveyorSpeed 5개 제외
                    var index = conveyorSpeed.ConveyorSpeed - 5;
                    double floatingpointnumbers = conveyorSpeed.Sv * 100;
                    short sv = Convert.ToInt16(floatingpointnumbers);

                    if (this.PlcWeightInvoiceIoHandler.CVSpeeds.Length > index && index >= 0)
                        this.PlcWeightInvoiceIoHandler.CVSpeeds[index].Execute(sv);
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        public void RouteModeExcute(RouteMode routeMode)
        {
            try
            {
                if (this.PlcWeightInvoiceIoHandler.SmartBoxRouteMode != null)
                {
                    short mode = routeMode.Mode == $"{RouteModeResult.Auto}" ? (short)RouteModeResult.Auto : (short)RouteModeResult.Ratio;
                    short smartRatio = Convert.ToInt16(routeMode.SmartRatio);
                    short normalRatio = Convert.ToInt16(routeMode.NormalRatio);
                    
                    this.PlcWeightInvoiceIoHandler.SmartBoxRouteMode.Execute(
                        mode,
                        smartRatio,
                        normalRatio);
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        public void RolltainerTowerLamapSetValue(TowerLampEnum towerLampEnum)
        {
            try
            {
                // PLC 담당자가 롤테이너 타워램프가 한개라서 한군데에 내려달라고 요청
                this.PlcWeightInvoiceIoHandler.RollTainersTowerLampIoPoints[0]?.SetValue((short)towerLampEnum);
                //for (int i = 0; i < this.PlcWeightInvoiceIoHandler.RollTainersTowerLampOn.Length; i++)
                //{
                //    if (this.PlcWeightInvoiceIoHandler.RollTainersTowerLampOn[i]?.Name == locationStatus.IoName)
                //    {
                //        this.PlcWeightInvoiceIoHandler.RollTainersTowerLampOn[i]?.SetValue((short)locationStatus.RollTainterTowerLamp);
                //        break;
                //    }
                //}
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        public void OutResultExcute(BcrResult bcrResult, string boxId)
        {
            try
            {
                if (this.PlcWeightInvoiceIoHandler.OutResult != null)
                    this.PlcWeightInvoiceIoHandler.OutResult.Execute((short)bcrResult, boxId);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        public bool IsAlarmSet(WeightInvoicBcrEnum bcr)
        {
            try
            {
                switch (bcr)
                {
                    case WeightInvoicBcrEnum.DynamicScale:
                        return this.PlcWeightInvoiceIoHandler.BcrsAlarms[(int)WeightInvoicBcrEnum.DynamicScale].GetBoolean();
                    case WeightInvoicBcrEnum.Route:
                        return this.PlcWeightInvoiceIoHandler.BcrsAlarms[(int)WeightInvoicBcrEnum.Route].GetBoolean();
                    case WeightInvoicBcrEnum.SmartLabel:
                        return this.PlcWeightInvoiceIoHandler.BcrsAlarms[(int)WeightInvoicBcrEnum.SmartLabel].GetBoolean();
                    case WeightInvoicBcrEnum.NormalLabel:
                        return this.PlcWeightInvoiceIoHandler.BcrsAlarms[(int)WeightInvoicBcrEnum.NormalLabel].GetBoolean();
                    //case WeightInvoicBcrEnum.TopSide:
                    //    return this.PlcWeightInvoiceIoHandler.BcrsAlarms[(int)WeightInvoicBcrEnum.TopSide].GetBoolean();
                    case WeightInvoicBcrEnum.Out:
                        return this.PlcWeightInvoiceIoHandler.BcrsAlarms[(int)WeightInvoicBcrEnum.Out].GetBoolean();
                    case WeightInvoicBcrEnum.Top:
                        return this.PlcWeightInvoiceIoHandler.TopBcrAlarm.GetBoolean();
                }

                return false;
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
                return false;
            }    
        }

        public override void OnEquipmentConnectionUpdateTouchSend()
        {
            base.OnEquipmentConnectionUpdateTouchSend();

            var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TouchPcBcrLcdEquipment>();
            if (eq != null)
                eq.InvoiceBcrState.PlcConnection = this.IsConnected;

            eq.EquipmentCoonectionStateSend();
        }

        #region Interface
        protected override void OnCreate()
        {
            this.LifeState = LifeCycleStateEnum.Creating;

            base.OnCreate();
            try
            {
                this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.PlcWeightInvoiceLog));

                this.OnSetPlcIoHandler(this.Setting.HandlerGroupName);
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

                this.InitCurrentData();
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
            }
            catch (Exception ex) { this.Logger?.Write(ex.Message); }

            this.LifeState = LifeCycleStateEnum.Terminated;
        }
        #endregion

        public override void OnHub_Recived(EventArgs e)
        {
            try
            {
                if (e is WeightOrInvoiceResultArgs weightInspectionResultArgs)
                {
                    if (string.IsNullOrEmpty(weightInspectionResultArgs.BoxId))
                        weightInspectionResultArgs.BoxId = string.Empty;

                    this.PlcWeightInvoiceIoHandler.WeightInspectionResult.Execute((short)weightInspectionResultArgs.Result, weightInspectionResultArgs.BoxId);
                    this.Communicator.Logger.Write($"BoxId = {weightInspectionResultArgs.BoxId}, Result = {weightInspectionResultArgs.Result}");
                }
                else if (e is RouteResultArgs weightRouteResultArgs)
                {
                    if (string.IsNullOrEmpty(weightRouteResultArgs.BoxId))
                        weightRouteResultArgs.BoxId = string.Empty;

                    this.PlcWeightInvoiceIoHandler.RouteResult.Execute((short)weightRouteResultArgs.Result, weightRouteResultArgs.BoxId);
                    this.Communicator.Logger.Write($"BoxId = {weightRouteResultArgs.BoxId}, Result = {weightRouteResultArgs.Result}");
                }
                else if (e is TopBcrResultArgs topBcrInvoiceResultArgs)
                {
                    if (string.IsNullOrEmpty(topBcrInvoiceResultArgs.BoxId))
                        topBcrInvoiceResultArgs.BoxId = string.Empty;

                    this.PlcWeightInvoiceIoHandler.InvoiceResult.Execute((short)topBcrInvoiceResultArgs.Result, topBcrInvoiceResultArgs.BoxId);
                    this.Communicator.Logger.Write($"BoxId = {topBcrInvoiceResultArgs.BoxId}, Invoice = {topBcrInvoiceResultArgs.Invoice}, Result = {topBcrInvoiceResultArgs.Result}");
                }
                else if (e is WeightInvoiceDynamiScaleAlarmArgs weightInvoiceDynamiScaleAlarmArgs)
                {
                    this.PlcWeightInvoiceIoHandler.DynamicScaleAlarm.SetValue(weightInvoiceDynamiScaleAlarmArgs.Result);
                }
                else if (e is WeightInvoiceBcrAlarmArgs bcrAlarmArgs)
                {
                    if (bcrAlarmArgs.BcrType == WeightInvoicBcrEnum.Top)
                        this.PlcWeightInvoiceIoHandler.TopBcrAlarm.SetValue(bcrAlarmArgs.Result);
                    else
                        this.PlcWeightInvoiceIoHandler.BcrsAlarms[(int)bcrAlarmArgs.BcrType].SetValue(bcrAlarmArgs.Result);
                }
                else if (e is WeightInvoiceLabelPrintAlarmArgs weightInvoiceLabelPrintAlarmArgs)
                {
                    int printNumber = weightInvoiceLabelPrintAlarmArgs.LabelPrintNumber - 1;
                    if (0 <= printNumber && printNumber < this.PlcWeightInvoiceIoHandler.InvocieLabelPrinterAlarms.Length)
                        this.PlcWeightInvoiceIoHandler.InvocieLabelPrinterAlarms[printNumber].SetValue(weightInvoiceLabelPrintAlarmArgs.Result);
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        #endregion

        #region Event Handler

        private void IoPointNormalWayRatio_ValueChanged(object sender, IoPointValueChangedEventArgs e)
        {
            try
            {
                if (sender is IoPoint ioPoint)
                    this.WeightInvoiceRatio.NormalWayRatio = ioPoint.GetInt16();
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void IoPointSmartWayRatio_ValueChanged(object sender, IoPointValueChangedEventArgs e)
        {
            try
            {
                if (sender is IoPoint ioPoint)
                    this.WeightInvoiceRatio.SmartWayRatio = ioPoint.GetInt16();
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }            
        }

        private void IoPointMode_ValueChanged(object sender, IoPointValueChangedEventArgs e)
        {
            try
            {
                if (sender is IoPoint ioPoint)
                    this.WeightInvoiceRatio.Mode = ioPoint.GetInt16();
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void SV_ValueChanged(object sender, IoPointValueChangedEventArgs e)
        {
            try
            {
                if (sender is IoPoint ioPoint)
                {
                    for (int i = 0; i < this.PlcWeightInvoiceIoHandler.IoPointCVSpeeds.Length; i++)
                    {
                        if (this.PlcWeightInvoiceIoHandler.IoPointCVSpeeds[i].SV == ioPoint)
                        {
                            this.WeightInvoiceCvSpeed[i].Sv = Scale.GetDecimalConversion(ioPoint.GetInt16(), 2);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        
        private void PV_ValueChanged(object sender, IoPointValueChangedEventArgs e)
        {
            try
            {
                if (sender is IoPoint ioPoint)
                {
                    for (int i = 0; i < this.PlcWeightInvoiceIoHandler.IoPointCVSpeeds.Length; i++)
                    {
                        if (this.PlcWeightInvoiceIoHandler.IoPointCVSpeeds[i].PV == ioPoint)
                        {
                            this.WeightInvoiceCvSpeed[i].Pv = Scale.GetDecimalConversion(ioPoint.GetInt16(), 2);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        
        private void RollTainersSensers_ValueChanged(object sender, IoPointValueChangedEventArgs e)
        {
            try
            {
                if (sender is IoPoint ioPoint)
                {
                    var rollTainers = this.PlcWeightInvoiceIoHandler.IoPointRollTainers;

                    for (int i = 0; i < rollTainers.Length; i++)
                    {
                        for (int j = 0; j < rollTainers[i].Sensors.Length; j++)
                        {
                            if (ioPoint == rollTainers[i].Sensors[j])
                            {
                                bool[] senssorsOn = new bool[rollTainers[i].Sensors.Length];
                                for (int index = 0; index < senssorsOn.Length; index++)
                                    senssorsOn[index] = rollTainers[i].Sensors[index].GetBoolean();

                                var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                                if (manager != null)
                                {
                                    Regex regex = new Regex(@"RollTainer#[0-9]{1}");
                                    if (regex.IsMatch(ioPoint.Name))
                                    {
                                        Match match = regex.Match(ioPoint.Name);
                                        string name = match.Groups[0].ToString();

                                        if (senssorsOn.All(x => x))
                                            manager.RolltainerSensingStatusPostAsync(name, sensorStatusEnum.ON);
                                        else if (senssorsOn.All(x => x == false))
                                            manager.RolltainerSensingStatusPostAsync(name, sensorStatusEnum.OFF);
                                        else
                                        {
                                            foreach (var station in EcsServerAppManager.Instance.Cache.LocationStatusPikcingStations)
                                            {
                                                LocationStatus status = manager.FindlocationStatusByIoName(station, name);
                                                if (status != null)
                                                {
                                                    if (status.RollTainterTowerLamp != RollTainterTowerLampEnum.N)
                                                        this.RolltainerTowerLamapSetValue(TowerLampEnum.RED_On);
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
           
        }

        private void P500EMGBitEvent_EventOccurred(object sender, EventArgs e)
        {
            bool emgState = true;

            emgState = this.GetP500EMGState();

            if (emgState == false)
            {
                this.Logger?.Write($"{HubServiceName.PlcWeightInvoiceEquipment} P500 EMG State :{emgState}");
                return;
            }

            var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
            if (manager != null)
            {
                manager.P500EmgRequested = true;
                manager.RmsStatusSettingPostAsync(SystemCodeEnum.BOX_MOVING_RMS, InstructionEnum.SYSTEM_STOP);
            }
               
        }

        private void WeightBCR_CV_StuckOn_ValueChanged(object sender, IoPointValueChangedEventArgs e)
        {
            this.Logger?.Write($"WeightBCR_CV_StuckOn : {(bool)e.Current}");

            var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<DynamicScaleEquipment>();
            if (eq != null)
            {
                if (e.Current is bool c)
                    eq.SetIsStuck(c);
            }
        }

        private void InvoiceBCR_CV_StuckOn_ValueChanged(object sender, IoPointValueChangedEventArgs e) { }

        protected override void OnFireAlarmPoint_ValueChanged(object sender, IoPointValueChangedEventArgs e)
        {
            base.OnFireAlarmPoint_ValueChanged(sender, e);

            try
            {
                if (e.Current is bool isOn)
                {
                    var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                    if (manager != null)
                    {
                        if (isOn)
                        {
                            {
                                var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking1Equipment] as PlcPickingEquipment;
                                if (eq != null)
                                    eq.FireAlarmExcute(isOn);
                            }

                            {
                                var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking2Equipment] as PlcPickingEquipment;
                                if (eq != null)
                                    eq.FireAlarmExcute(isOn);
                            }

                            {
                                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<PlcCaseErectEquipment>();
                                if (eq != null)
                                    eq.FireAlarmExcute(isOn);
                            }

                            manager.RmsStatusSettingPostAsync(SystemCodeEnum.ROBOT_PICKING_RMS, InstructionEnum.FIRE_STOP);
                            manager.RmsStatusSettingPostAsync(SystemCodeEnum.BOX_MOVING_RMS, InstructionEnum.FIRE_STOP);
                            manager.RmsStatusSettingPostAsync(SystemCodeEnum.ROLLTAINER_MOVING_RMS, InstructionEnum.FIRE_STOP);
                        }
                        else
                        {
                            // PLC 1,2번만 사용
                            {
                                var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking1Equipment] as PlcPickingEquipment;
                                if (eq != null)
                                    eq.FireAlarmResetExcute(true);
                            }

                            {
                                var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking2Equipment] as PlcPickingEquipment;
                                if (eq != null)
                                    eq.FireAlarmResetExcute(true);
                            }

                            manager.RmsStatusSettingPostAsync(SystemCodeEnum.ROBOT_PICKING_RMS, InstructionEnum.SYSTEM_RECOVER);
                            manager.RmsStatusSettingPostAsync(SystemCodeEnum.BOX_MOVING_RMS, InstructionEnum.SYSTEM_RECOVER);
                            manager.RmsStatusSettingPostAsync(SystemCodeEnum.ROLLTAINER_MOVING_RMS, InstructionEnum.SYSTEM_RECOVER);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }

        private void BoxHeightSensorDataEvent_DataRead(object sender, DataEventIoHandlerDataReadEventArgs e)
        {
            if (e.Data.Length != 1 || e.Data[0] == null) return;

            if (int.TryParse(e.Data[0].ToString(), out int value))
            {
                var eq = EcsServerAppManager.Instance.Equipments.GetByEquipmentType<TopBcrEquipment>();
                if (eq != null)
                    eq.CurrrentHeightSensorValue = value;
            }
        }
        #endregion
    }
}
