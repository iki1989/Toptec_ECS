using ECS.Core.Equipments;
using ECS.Core.Managers;
using ECS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Urcis.SmartCode.Net.Tcp;

namespace ECS.Core.ViewModels.Server
{
    public class ConnectionInfoEquipmentsModel : Notifier
    {
        #region Field
        private EquipmentDictionary Equipments => EcsServerAppManager.Instance.Equipments;
        #endregion

        #region Prop
        private ObservableCollection<EquipmentConnectonInfo> m_EquipmentConnectonInfos;
        public ObservableCollection<EquipmentConnectonInfo> EquipmentConnectonInfos
        {
            get => this.m_EquipmentConnectonInfos;
            private set
            {
                this.m_EquipmentConnectonInfos = value;
                this.OnPropertyChanged(nameof(this.EquipmentConnectonInfos));
            }
        }
        #endregion

        #region Ctor
        public ConnectionInfoEquipmentsModel()
        {
            this.EquipmentConnectonInfos = new ObservableCollection<EquipmentConnectonInfo>();
            this.UpdateEquipmentConnectonInfos();
        }
        #endregion

        #region Method

        private void PlcEquipmentConnectonInfosAdd(string hubServiceName, ref int no)
        {
            var eq = this.Equipments[hubServiceName];
            if (eq != null)
            {
                if (eq is PlcGeneralEquipment plcEq)
                {
                    EquipmentConnectonInfo info = new EquipmentConnectonInfo();
                    info.No = no;
                    info.Id = plcEq.Id;
                    info.Name = plcEq.Name;
                    info.Ip = "Mx Component";
                    info.IsConnected = plcEq.IsConnected ? $"{TcpConnectionStateEnum.Connected}" : $"{TcpConnectionStateEnum.Disconnected}";

                    this.EquipmentConnectonInfos.Add(info); no++;
                }
            }
        }

        private void InkjectEquipmentConnectonInfosAdd(string hubServiceName, ref int no)
        {
            var eq = this.Equipments[hubServiceName];
            if (eq != null)
            {
                if (eq is InkjectEquipment castingEq)
                {
                    EquipmentConnectonInfo info = new EquipmentConnectonInfo();
                    info.No = no;
                    info.Id = castingEq.Id;
                    info.Name = castingEq.Name;
                    info.Ip = castingEq.Communicator.Setting.Ip;
                    info.Port = castingEq.Communicator.Setting.Port;
                    info.Active = $"{castingEq.Communicator.Setting.Active}";
                    info.IsConnected = $"{castingEq.Communicator.TcpConnectionState}";

                    this.EquipmentConnectonInfos.Add(info); no++;

                    EquipmentConnectonInfo bcrInfo = new EquipmentConnectonInfo();
                    bcrInfo.No = no;
                    bcrInfo.Name = castingEq.Bcr.Name;
                    bcrInfo.Ip = castingEq.Bcr.Setting.Ip;
                    bcrInfo.Port = castingEq.Bcr.Setting.Port;
                    bcrInfo.Active = $"{castingEq.Bcr.Setting.Active}";
                    bcrInfo.IsConnected = $"{castingEq.Bcr.TcpConnectionState}";

                    this.EquipmentConnectonInfos.Add(bcrInfo); no++;
                }
            }
        }

        private void LabelPrinterZebraZe500EquipmentConnectonInfosAdd(string hubServiceName, ref int no)
        {
            var eq = this.Equipments[hubServiceName];
            if (eq != null)
            {
                if (eq is LabelPrinterZebraZe500Equipment castingEq)
                {
                    EquipmentConnectonInfo info = new EquipmentConnectonInfo();
                    info.No = no;
                    info.Id = castingEq.Id;
                    info.Name = castingEq.Name;
                    info.Ip = castingEq.Communicator.Setting.Ip;
                    info.Port = castingEq.Communicator.Setting.Port;
                    info.Active = $"{castingEq.Communicator.Setting.Active}";
                    info.IsConnected = $"{castingEq.Communicator.TcpConnectionState}";

                    this.EquipmentConnectonInfos.Add(info); no++;

                    EquipmentConnectonInfo bcrInfo = new EquipmentConnectonInfo();
                    bcrInfo.No = no;
                    bcrInfo.Name = castingEq.Bcr.Name;
                    bcrInfo.Ip = castingEq.Bcr.Setting.Ip;
                    bcrInfo.Port = castingEq.Bcr.Setting.Port;
                    bcrInfo.Active = $"{castingEq.Bcr.Setting.Active}";
                    bcrInfo.IsConnected = $"{castingEq.Bcr.TcpConnectionState}";

                    this.EquipmentConnectonInfos.Add(bcrInfo); no++;
                }
            }
        }

        private void LogicalEquipmentConnectonInfosAdd(string hubServiceName, ref int no)
        {
            var eq = this.Equipments[hubServiceName];
            if (eq != null)
            {
                if (eq is LogicalEquipment castingEq)
                {
                    EquipmentConnectonInfo bcrInfo = new EquipmentConnectonInfo();
                    bcrInfo.No = no;
                    bcrInfo.Id = castingEq.Id;
                    bcrInfo.Name = castingEq.Bcr.Name;
                    bcrInfo.Ip = castingEq.Bcr.Setting.Ip;
                    bcrInfo.Port = castingEq.Bcr.Setting.Port;
                    bcrInfo.Active = $"{castingEq.Bcr.Setting.Active}";
                    bcrInfo.IsConnected = $"{castingEq.Bcr.TcpConnectionState}";

                    this.EquipmentConnectonInfos.Add(bcrInfo); no++;
                }
            }
        }

        private void TouchPcEquipmentConnectonInfosAdd(string hubServiceName, ref int no)
        {
            var eq = this.Equipments[hubServiceName];
            if (eq != null)
            {
                if (eq is PcEquipment pcEq)
                {
                    EquipmentConnectonInfo info = new EquipmentConnectonInfo();
                    info.No = no;
                    info.Id = pcEq.Id;
                    info.Name = pcEq.Name;
                    info.Ip = pcEq.Setting.CommunicatorSetting.Ip;
                    info.Port = pcEq.Setting.CommunicatorSetting.Port;
                    info.Active = $"{pcEq.Setting.CommunicatorSetting.Active}";
                    info.IsConnected = $"{pcEq.Communicator.TcpConnectionState}";

                    this.EquipmentConnectonInfos.Add(info); no++;
                }
            }
        }

        public void UpdateEquipmentConnectonInfos()
        {
            try
            {
                this.EquipmentConnectonInfos.Clear();

                int no = 1;

                this.PlcEquipmentConnectonInfosAdd(HubServiceName.PlcPicking1Equipment, ref no);
                this.PlcEquipmentConnectonInfosAdd(HubServiceName.PlcPicking2Equipment, ref no);
                this.PlcEquipmentConnectonInfosAdd(HubServiceName.PlcCaseErectEquipment, ref no);
                this.PlcEquipmentConnectonInfosAdd(HubServiceName.PlcWeightInvoiceEquipment, ref no);
                this.PlcEquipmentConnectonInfosAdd(HubServiceName.PlcSmartPackingEquipment, ref no);

                //SmartPackingEquipment BCR
                {
                    var eq = this.Equipments[HubServiceName.PlcSmartPackingEquipment];
                    if (eq != null)
                    {
                        if (eq is PlcSmartPackingEquipment plcEq)
                        {
                            EquipmentConnectonInfo info = new EquipmentConnectonInfo();
                            info.No = no;
                            info.Name = plcEq.Bcr.Name;
                            info.Ip = plcEq.Bcr.Setting.Ip;
                            info.Port = plcEq.Bcr.Setting.Port;
                            info.Active = $"{plcEq.Bcr.Setting.Active}";
                            info.IsConnected = $"{plcEq.Bcr.TcpConnectionState}";

                            this.EquipmentConnectonInfos.Add(info); no++;
                        }
                    }
                }

                this.InkjectEquipmentConnectonInfosAdd(HubServiceName.InkjectEquipment1, ref no);
                this.InkjectEquipmentConnectonInfosAdd(HubServiceName.InkjectEquipment2, ref no);

                //DynamicScaleEquipment
                {
                    var eq = this.Equipments[HubServiceName.DynamicScaleEquipment];
                    if (eq != null)
                    {
                        if (eq is DynamicScaleEquipment castingEq)
                        {
                            EquipmentConnectonInfo info = new EquipmentConnectonInfo();
                            info.No = no;
                            info.Id = castingEq.Id;
                            info.Name = castingEq.Name;
                            info.Ip = castingEq.Communicator.Setting.Ip;
                            info.Port = castingEq.Communicator.Setting.Port;
                            info.Active = $"{castingEq.Communicator.Setting.Active}";
                            info.IsConnected = $"{castingEq.Communicator.TcpConnectionState}";

                            this.EquipmentConnectonInfos.Add(info); no++;

                            EquipmentConnectonInfo bcrInfo = new EquipmentConnectonInfo();
                            bcrInfo.No = no;
                            bcrInfo.Name = castingEq.Bcr.Name;
                            bcrInfo.Ip = castingEq.Bcr.Setting.Ip;
                            bcrInfo.Port = castingEq.Bcr.Setting.Port;
                            bcrInfo.Active = $"{castingEq.Bcr.Setting.Active}";
                            bcrInfo.IsConnected = $"{castingEq.Bcr.TcpConnectionState}";

                            this.EquipmentConnectonInfos.Add(bcrInfo); no++;
                        }
                    }
                }

                this.LabelPrinterZebraZe500EquipmentConnectonInfosAdd(HubServiceName.NormalLabelPrinterZebraZe500Equipment, ref no);
                this.LabelPrinterZebraZe500EquipmentConnectonInfosAdd(HubServiceName.SmartLabelPrinterZebraZe500Equipment, ref no);

                this.LogicalEquipmentConnectonInfosAdd(HubServiceName.RouteLogicalEquipment, ref no);

                //TopBcrEquipment
                {
                    var eq = this.Equipments[HubServiceName.TopBcrEquipment];
                    if (eq != null)
                    {
                        if (eq is TopBcrEquipment castingEq)
                        {
                            EquipmentConnectonInfo info = new EquipmentConnectonInfo();
                            info.No = no;
                            info.Id = castingEq.Id;
                            info.Name = castingEq.Name;
                            info.Ip = castingEq.Communicator.Setting.Ip;
                            info.Port = castingEq.Communicator.Setting.Port;
                            info.Active = $"{castingEq.Communicator.Setting.Active}";
                            info.IsConnected = $"{castingEq.Communicator.TcpConnectionState}";

                            this.EquipmentConnectonInfos.Add(info); no++;
                        }
                    }
                }

                this.LogicalEquipmentConnectonInfosAdd(HubServiceName.OutLogicalEquipment, ref no);

                this.TouchPcEquipmentConnectonInfosAdd(HubServiceName.TouchPcBcrLcdEquipment, ref no);
                this.TouchPcEquipmentConnectonInfosAdd(HubServiceName.TouchPcCaseErectEquipment, ref no);
                this.TouchPcEquipmentConnectonInfosAdd(HubServiceName.TouchPcWeightInspectorEquipment, ref no);
                this.TouchPcEquipmentConnectonInfosAdd(HubServiceName.TouchPcInvoiceRejectEquipment, ref no);
                this.TouchPcEquipmentConnectonInfosAdd(HubServiceName.TouchPcConveyorEquipment1, ref no);
                this.TouchPcEquipmentConnectonInfosAdd(HubServiceName.TouchPcConveyorEquipment2, ref no);
                this.TouchPcEquipmentConnectonInfosAdd(HubServiceName.TouchPcSmartPackingEquipment, ref no);
            }
            catch (Exception) { }
        }
        #endregion

        public struct EquipmentConnectonInfo
        {
            public int No { get; set; }

            public string Id { get; set; }

            public string Name { get; set; }

            public string Ip { get; set; }

            public int Port { get; set; }

            public string Active { get; set; }

            public string IsConnected { get; set; }
        }
    }
}
