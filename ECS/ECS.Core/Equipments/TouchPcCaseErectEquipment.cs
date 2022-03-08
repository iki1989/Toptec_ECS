using System;
using Urcis.Secl;
using Urcis.SmartCode;
using Urcis.SmartCode.Diagnostics;
using ECS.Model.Pcs;
using ECS.Model;
using ECS.Model.Hub;
using ECS.Model.Restfuls;
using ECS.Core.Managers;
using ECS.Model.Plc;
using System.Text;

namespace ECS.Core.Equipments
{
    public class TouchPcCaseErectEquipment : TouchPcGeneralEquipment
    {
        #region Field
        public ErectorConnectionState ErectorConnectionState { get; set; } = new ErectorConnectionState();
        private int[] InkjectLevel { get; set; } = new int[2];
        #endregion

        #region Ctor
        public TouchPcCaseErectEquipment(PcEquipmenttSetting setting) : base(setting) 
        {
            this.Setting = setting ?? new PcEquipmenttSetting(); 
        }
        #endregion

        #region Method

        protected override void OnCreate()
        {
            base.OnCreate();

            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.TouchPcCaseErect));

            this.Setting.CommunicatorSetting.LogDirectory = this.Logger.DirectoryPath;
            this.Communicator?.ApplySetting(this.Setting.CommunicatorSetting);

            this.LifeState = LifeCycleStateEnum.Created;
        }

        protected override void OnParseFrame(PcMessageFrame touchMessageFrame)
        {
            base.OnParseFrame(touchMessageFrame);

            #region OnParseFrame
            try
            {
                var name = touchMessageFrame.Type;
                if (typeof(InkjectIgnorelackOfInk).AssemblyQualifiedName == name)
                {
                    var InkjectIgnorelackOfInk = this.JsonDeserialize<InkjectIgnorelackOfInk>(touchMessageFrame.Data);
                    this.Logger?.Write($"Recieved {name} : {InkjectIgnorelackOfInk}");
                }
                else if (typeof(InkjectResume).AssemblyQualifiedName == name)
                {
                    var InkjectResume = this.JsonDeserialize<InkjectResume>(touchMessageFrame.Data);
                    this.Logger?.Write($"Recieved {name} : {InkjectResume}");

                    // 잉크젯 초기화
                    CaseErectInkjectResumeArgs args = new CaseErectInkjectResumeArgs();
                    args.Line = InkjectResume.Line;

                    if (args.Line == 1)
                    {
                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.InkjectEquipment1, args);
                        this.InkjectInkSend(args.Line, this.InkjectLevel[0]);
                    }
                    else if (args.Line == 2)
                    {
                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.InkjectEquipment2, args);
                        this.InkjectInkSend(args.Line, this.InkjectLevel[1]);
                    }
                        
                }
                else if (typeof(ManualBoxValidationRequest).AssemblyQualifiedName == name)
                {
                    var ManualBoxValidationRequest = this.JsonDeserialize<ManualBoxValidationRequest>(touchMessageFrame.Data);
                    this.Logger?.Write($"Recieved {name} : {ManualBoxValidationRequest}");

                    // 수동 검증 요청
                    CaseErectBcrResultArgs args = new CaseErectBcrResultArgs();
                    args.BoxId = ManualBoxValidationRequest.BoxId;
                    args.BoxType = ManualBoxValidationRequest.BoxType;
                    args.BcrId = 0;
                    args.EqpId = this.Setting.Id;

                    // 디비 저장
                    EcsServerAppManager.Instance.DataBaseManagerForServer.InsertBox(args.BoxId, "0");

                    // RICP Send
                    EcsServerAppManager.Instance.Hub.Send(HubServiceName.RicpPost, args);
                    // WCS Send
                    EcsServerAppManager.Instance.Hub.Send(HubServiceName.WcsPost, args);
                }
                else if (typeof(BoxNumber).AssemblyQualifiedName == name)
                {
                    var BoxNumber = this.JsonDeserialize<BoxNumber>(touchMessageFrame.Data);
                    this.Logger?.Write($"Recieved {name} : {BoxNumber}");

                    CaseErectBoxNumberArgs caseErectBoxNumberArgs = new CaseErectBoxNumberArgs();
                    caseErectBoxNumberArgs.Line = BoxNumber.Line;

                    if (caseErectBoxNumberArgs.Line == 1)
                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.InkjectEquipment1, caseErectBoxNumberArgs);
                    else if (caseErectBoxNumberArgs.Line == 2)
                        EcsServerAppManager.Instance.Hub.Send(HubServiceName.InkjectEquipment2, caseErectBoxNumberArgs);

                    EcsServerAppManager.Instance.Hub.Send(HubServiceName.TopBcrEquipment, caseErectBoxNumberArgs);
                }
                else
                    this.Logger?.Write($"Recieved {name} : is Not Define Message");
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
            #endregion
        }

        public override void OnHub_Recived(EventArgs e)
        {
            #region OnHub_Recived
            try
            {
                if (e is InkjectInkInformationArgs informationArgs)
                {
                    if (informationArgs.Line == 1)
                        this.InkjectLevel[0] = informationArgs.InkPercent;
                    else
                        this.InkjectLevel[1] = informationArgs.InkPercent;

                    this.InkjectInkSend(informationArgs.Line, informationArgs.InkPercent);
                }
                else if (e is CaseErectBcrResultArgs caseErectBcrResultArgs)
                {
                    this.BcrIndexSend(caseErectBcrResultArgs.BcrIndex);
                }
                else if (e is CaseErectEquipmentConnectionArgs connStateChangedEventArgs)
                {
                    this.EquipmentCoonectionStateSend(connStateChangedEventArgs.ConnectionState);
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
            #endregion
        }

        #region Send
        private void InkjectInkSend(int line, int inkPercent)
        {
            #region InkjectInkSend
            try
            {
                this.SendMessage(new InkjectInk { Line = line, InkPercent = inkPercent });
                this.Logger?.Write($"InkjectInk - Line : {line}, InkPercent : {inkPercent}");
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
            #endregion
        }

        private void BcrIndexSend(long bcrIndex)
        {
            #region BcrIndexSend
            try
            {
                this.Logger?.Write($"bcrIndex - {bcrIndex}");
                this.SendMessage(new CaseErectBcrRead { CaseErectIndex = bcrIndex });
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
            #endregion
        }

        private void EquipmentCoonectionStateSend(ErectorConnectionState erectorState)
        {
            try
            {
                #region EquipmentCoonectionStateSend
                this.SendMessage(erectorState);

                StringBuilder sb = new StringBuilder();
                sb.Append("Erector1Connection :").Append($"{erectorState.Erector1Connection}, ");
                sb.Append("Erector2Connection :").Append($"{erectorState.Erector2Connection}, ");
                sb.Append("Inkjet1Connection :").Append($"{erectorState.Inkjet1Connection}, ");
                sb.Append("Inkjet2Connection :").Append($"{erectorState.Inkjet2Connection}, ");
                sb.Append("ErectorBcr1Connection :").Append($"{erectorState.ErectorBcr1Connection}, ");
                sb.Append("ErectorBcr2Connection :").Append($"{erectorState.ErectorBcr2Connection}");

                this.Logger?.Write(sb.ToString());
                #endregion
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
        }
        public void EquipmentCoonectionStateSend()
        {
            this.EquipmentCoonectionStateSend(this.ErectorConnectionState);
        }

        protected override void OnEquipmentStateChangeSend(bool isConnect)
        {
            var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
            
            if (manager != null)
            {
                DeviceStatus deviceStatus = new DeviceStatus();
               
                DeviceInfo device = new DeviceInfo()
                {
                    deviceId = this.Id,
                    deviceName = "Box Reject",
                    deviceTypeId = "REJECT",
                    deviceTypeName = "배출부",
                };

                if (isConnect)
                    device.deviceStatusCd = (int)EquipmentStateEnum.Run;
                else
                {
                    device.deviceStatusCd = (int)EquipmentStateEnum.Error;
                    device.deviceErrorMsg = "Disconnect";
                }

                deviceStatus.deviceList.Add(device);

                manager.DeviceStatusPostAsync(deviceStatus);
            }
        }
        #endregion
        #endregion

        #region Event Handler
        protected override void OnCommunicator_HsmsConnectionStateChanged(object sender, HsmsConnectionStateChangedEventArgs e)
        {
            if (e.Current == e.Previous) return;

            base.OnCommunicator_HsmsConnectionStateChanged(sender, e);

            this.EquipmentCoonectionStateSend(this.ErectorConnectionState);

            {
                var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.InkjectEquipment1];
                if (eq != null)
                {
                    if (eq is InkjectEquipment inkEq)
                    {
                        if (inkEq.Communicator.TcpConnectionState == Urcis.SmartCode.Net.Tcp.TcpConnectionStateEnum.Connected)
                        {
                            inkEq.ReadInkLevelSend();
                            Urcis.SmartCode.Threading.ScThread.Sleep(1000);
                            this.InkjectLevel[0] = inkEq.InkPercet;
                            this.InkjectInkSend(1, this.InkjectLevel[0]);
                        }
                    }
                }
            }

            {
                var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.InkjectEquipment2];
                if (eq != null)
                {
                    if (eq is InkjectEquipment inkEq)
                    {
                        if (inkEq.Communicator.TcpConnectionState == Urcis.SmartCode.Net.Tcp.TcpConnectionStateEnum.Connected)
                        {
                            inkEq.ReadInkLevelSend();
                            Urcis.SmartCode.Threading.ScThread.Sleep(1000);
                            this.InkjectLevel[1] = inkEq.InkPercet;
                            this.InkjectInkSend(2, this.InkjectLevel[1]);
                        }
                    }
                }
            } 
        }

        #endregion
    }
}
