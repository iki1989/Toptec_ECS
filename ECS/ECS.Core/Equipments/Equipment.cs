using ECS.Core.Managers;
using ECS.Model;
using ECS.Model.Pcs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Urcis.SmartCode;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode.Net;
using Urcis.SmartCode.Net.Tcp;
using Urcis.SmartCode.Threading;


namespace ECS.Core.Equipments
{
    public abstract class Equipment : INameable, ILifeCycleable, IHaveLogger, IDrive, INotifyPropertyChanged
    {
        #region event
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Prop
        public string Id { get; set; }
        public string Name { get; set; }

        public Logger Logger { get; protected set; }

        protected Communicator Communicator { get; set; }

        protected EquipmentSetting Setting { get; set; }

        private LifeCycleStateEnum m_LifeState;
        public LifeCycleStateEnum LifeState
        {
            get => this.m_LifeState;
            protected set
            {
                if (this.m_LifeState == value) return;

                this.m_LifeState = value;
                this.Logger?.Write(this.m_LifeState.ToString());
            }
        }
        #endregion

        #region Ctor
        public Equipment() { }
        #endregion

        #region Method
        #region ILifeCycleable
        public void Create() => this.OnCreate();

        public ScTask CreateAsync() => ScTask.Run(() => this.Create());

        public void Prepare() => this.OnPrepare();

        public ScTask PrepareAsync() => ScTask.Run(() => this.Prepare());

        public void Terminate() => this.OnTerminate();

        public ScTask TerminateAsync() => ScTask.Run(() => this.Terminate());

        public void Start() => this.OnStart();

        public void Stop() => this.OnStop();

        public async Task StartAsync() => await Task.Run(() => this.Start());

        public async Task StopAsync() => await Task.Run(() => this.Stop());

        protected virtual void OnCreate()
        {
            this.Id = this.Setting.Id;
            this.Name = this.Setting.Name;
        }

        protected virtual bool OnPrepare() => true;

        protected virtual void OnTerminate() { }
        #endregion

        #region IDrive
        protected virtual void OnStart() { }

        protected virtual void OnStop() { }
        #endregion

        #region INotifyPropertyChanged
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, args);
        }
        #endregion

        public virtual void OnEquipmentConnectionUpdateTouchSend() { }

        protected virtual void OnEquipmentStateRicpPost() { }
        #endregion

        #region Event Handler
        public virtual void OnHub_Recived(EventArgs e) { }
        #endregion
    }

    [Serializable]
    public class EquipmentSetting : Setting, INameable
    {
        public TcpCommunicatorSetting CommunicatorSetting { get; set; } = new TcpCommunicatorSetting();

        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class EquipmentDictionary : Dictionary<string, Equipment>
    {
        public T GetByEquipmentType<T>()
        {
            foreach (var item in this.Values)
            {
                if (item.GetType() == typeof(T))
                    return (T)Convert.ChangeType(item, typeof(T));
            }

            return default;
        }

        public List<PlcGeneralEquipment> GetPlcEquipments()
        {
            List<PlcGeneralEquipment> plcs = new List<PlcGeneralEquipment>();
            foreach (var item in this.Values)
            {
                if (item is PlcGeneralEquipment plcEq)
                    plcs.Add(plcEq);
            }

            return plcs;
        }

        public void CreateAll()
        {
            LifeCycleable.CreateByTask(this.Values);
        }

        public void PrepareAndStartAll()
        {
            List<Task> tasks = new List<Task>();
            foreach (var equipment in this.Values)
            {
                if (equipment != null)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        equipment.Prepare();
                        equipment.Start();
                    }));
                }
            }
            ScTask.WaitAll(tasks);
        }

        public void TerminateAll()
        {
            LifeCycleable.TerminateByTask(this.Values);
        }
    }

    [Serializable]
    public class EquipmentCollectionForServerSetting : Setting
    {
        public PlcEquipmentSetting[] PlcEquipmentSettings { get; set; } = new PlcEquipmentSetting[5];

        public DynamicScaleEquipmentSetting DynamicScaleEquipmentSetting { get; set; } = new DynamicScaleEquipmentSetting();

        public InkjectEquipmentSetting[] InkjectEquipmentSettings { get; set; } = new InkjectEquipmentSetting[2];

        public LabelPrinterZebraZe500EquipmentSetting[] LabelPrinterZebraZe500EquipmentSettings { get; set; } = new LabelPrinterZebraZe500EquipmentSetting[2];

        public TopBcrEquipmentSetting TopBcrEquipmentSetting { get; set; } = new TopBcrEquipmentSetting();

        public RouteLogicalEquipmentSetting RouteLogicalEquipmentSetting { get; set; } = new RouteLogicalEquipmentSetting();

        public OutLogicalEquipmentSetting OutLogicalEquipmentSetting { get; set; } = new OutLogicalEquipmentSetting();


        public PcEquipmenttSetting[] PcEquipmenttSettings { get; set; } = new PcEquipmenttSetting[7];

        public EquipmentCollectionForServerSetting()
        {
            string bcrName = string.Empty;

            this.PlcEquipmentSettings[0] = new PlcEquipmentSetting();
            this.PlcEquipmentSettings[0].Name = HubServiceName.PlcPicking1Equipment;
            this.PlcEquipmentSettings[0].CommunicatorName = this.PlcEquipmentSettings[0].Name;
            this.PlcEquipmentSettings[0].HandlerGroupName = this.PlcEquipmentSettings[0].Name;

            this.PlcEquipmentSettings[1] = new PlcEquipmentSetting();
            this.PlcEquipmentSettings[1].Name = HubServiceName.PlcPicking2Equipment;
            this.PlcEquipmentSettings[1].CommunicatorName = this.PlcEquipmentSettings[1].Name;
            this.PlcEquipmentSettings[1].HandlerGroupName = this.PlcEquipmentSettings[1].Name;

            this.PlcEquipmentSettings[2] = new PlcEquipmentSetting();
            this.PlcEquipmentSettings[2].Name = HubServiceName.PlcCaseErectEquipment;
            this.PlcEquipmentSettings[2].CommunicatorName = this.PlcEquipmentSettings[2].Name;
            this.PlcEquipmentSettings[2].HandlerGroupName = this.PlcEquipmentSettings[2].Name;

            this.PlcEquipmentSettings[3] = new PlcEquipmentSetting();
            this.PlcEquipmentSettings[3].Name = HubServiceName.PlcWeightInvoiceEquipment;
            this.PlcEquipmentSettings[3].CommunicatorName = this.PlcEquipmentSettings[3].Name;
            this.PlcEquipmentSettings[3].HandlerGroupName = this.PlcEquipmentSettings[3].Name;

            var smartPackingSetting = new PlcSmartPackingSetting();
            this.PlcEquipmentSettings[4] = smartPackingSetting;
            this.PlcEquipmentSettings[4].Name = HubServiceName.PlcSmartPackingEquipment;
            this.PlcEquipmentSettings[4].CommunicatorName = this.PlcEquipmentSettings[4].Name;
            this.PlcEquipmentSettings[4].HandlerGroupName = this.PlcEquipmentSettings[4].Name;

            bcrName = $"BCR({HubServiceName.PlcSmartPackingEquipment})";
            smartPackingSetting.BcrCommunicatorSetting.Name = $"{bcrName} Communicator";

            bcrName = $"BCR#1-1({HubServiceName.InkjectEquipment1} Rear)";
            this.InkjectEquipmentSettings[0] = new InkjectEquipmentSetting() { Name = HubServiceName.InkjectEquipment1 };
            this.InkjectEquipmentSettings[0].BcrCommunicatorSetting.Name = $"{bcrName} Communicator";
            this.InkjectEquipmentSettings[0].CommunicatorSetting.Name = $"{HubServiceName.InkjectEquipment1} Communicator";

            bcrName = $"BCR#1-2({HubServiceName.InkjectEquipment2} Rear)";
            this.InkjectEquipmentSettings[1] = new InkjectEquipmentSetting() { Name = HubServiceName.InkjectEquipment2 };
            this.InkjectEquipmentSettings[1].BcrCommunicatorSetting.Name = $"{bcrName} Communicator";
            this.InkjectEquipmentSettings[1].CommunicatorSetting.Name = $"{HubServiceName.InkjectEquipment2} Communicator";

            bcrName = $"BCR#2({HubServiceName.DynamicScaleEquipment})";
            this.DynamicScaleEquipmentSetting.BcrCommunicatorSetting.Name = $"{bcrName} Communicator";

            bcrName = $"BCR#3({HubServiceName.RouteLogicalEquipment})";
            this.RouteLogicalEquipmentSetting.Id = "GBC21";
            this.RouteLogicalEquipmentSetting.BcrCommunicatorSetting.Name = $"{bcrName} Communicator";

            bcrName = $"BCR#4-1({HubServiceName.NormalLabelPrinterZebraZe500Equipment} Front)";
            this.LabelPrinterZebraZe500EquipmentSettings[0] = new LabelPrinterZebraZe500EquipmentSetting() { Name = HubServiceName.NormalLabelPrinterZebraZe500Equipment, Id = "GL21" };
            this.LabelPrinterZebraZe500EquipmentSettings[0].BcrCommunicatorSetting.Name = $"{bcrName} Communicator";
            this.LabelPrinterZebraZe500EquipmentSettings[0].CommunicatorSetting.Name = $"{HubServiceName.NormalLabelPrinterZebraZe500Equipment} Communicator";

            bcrName = $"BCR#4-2({HubServiceName.SmartLabelPrinterZebraZe500Equipment} Front)";
            this.LabelPrinterZebraZe500EquipmentSettings[1] = new LabelPrinterZebraZe500EquipmentSetting() { Name = HubServiceName.SmartLabelPrinterZebraZe500Equipment, Id = "GL22" };
            this.LabelPrinterZebraZe500EquipmentSettings[1].BcrCommunicatorSetting.Name = $"{bcrName} Communicator";
            this.LabelPrinterZebraZe500EquipmentSettings[1].CommunicatorSetting.Name = $"{HubServiceName.SmartLabelPrinterZebraZe500Equipment} Communicator";

            //BCR#5는 Top BCR과 인터페이스 통합됨

            bcrName = $"BCR#6({HubServiceName.OutLogicalEquipment})";
            this.OutLogicalEquipmentSetting.Id = "GOP21";
            this.OutLogicalEquipmentSetting.BcrCommunicatorSetting.Name = $"{bcrName} Communicator";

            this.PcEquipmenttSettings[0] = new PcEquipmenttSetting();
            this.PcEquipmenttSettings[0].Id = "GBR21";
            this.PcEquipmenttSettings[0].Name = HubServiceName.TouchPcCaseErectEquipment;
            this.PcEquipmenttSettings[0].CommunicatorSetting.Name = $"{this.PcEquipmenttSettings[0].Name} Communicator";

            this.PcEquipmenttSettings[1] = new PcEquipmenttSetting();
            this.PcEquipmenttSettings[0].Id = "GWR21";
            this.PcEquipmenttSettings[1].Name = HubServiceName.TouchPcWeightInspectorEquipment;
            this.PcEquipmenttSettings[1].CommunicatorSetting.Name = $"{this.PcEquipmenttSettings[1].Name} Communicator";

            this.PcEquipmenttSettings[2] = new PcEquipmenttSetting();
            this.PcEquipmenttSettings[0].Id = "GOR21";
            this.PcEquipmenttSettings[2].Name = HubServiceName.TouchPcInvoiceRejectEquipment;
            this.PcEquipmenttSettings[2].CommunicatorSetting.Name = $"{this.PcEquipmenttSettings[2].Name} Communicator";

            this.PcEquipmenttSettings[3] = new PcEquipmenttSetting();
            this.PcEquipmenttSettings[3].Name = HubServiceName.TouchPcBcrLcdEquipment;
            this.PcEquipmenttSettings[3].CommunicatorSetting.Name = $"{this.PcEquipmenttSettings[3].Name} Communicator";

            this.PcEquipmenttSettings[4] = new PcEquipmenttSetting();
            this.PcEquipmenttSettings[4].Name = HubServiceName.TouchPcConveyorEquipment1;
            this.PcEquipmenttSettings[4].CommunicatorSetting.Name = $"{this.PcEquipmenttSettings[4].Name} Communicator";

            this.PcEquipmenttSettings[5] = new PcEquipmenttSetting();
            this.PcEquipmenttSettings[5].Name = HubServiceName.TouchPcConveyorEquipment2;
            this.PcEquipmenttSettings[5].CommunicatorSetting.Name = $"{this.PcEquipmenttSettings[5].Name} Communicator";

            this.PcEquipmenttSettings[6] = new PcEquipmenttSetting();
            this.PcEquipmenttSettings[6].Name = HubServiceName.TouchPcSmartPackingEquipment;
            this.PcEquipmenttSettings[6].CommunicatorSetting.Name = $"{this.PcEquipmenttSettings[6].Name} Communicator";
        }
    }

    [Serializable]
    public class EquipmentCollectionForTouchSetting : Setting
    {
        public PcEquipmenttSetting PcEquipmenttSetting { get; set; } = new PcEquipmenttSetting();

        public LabelPrinterZebraZt411EquipmentSetting LabelPrinterZebraEquipmentSetting { get; set; }
        
        public EquipmentCollectionForTouchSetting(EcsTouchType touchType)
        {
            switch (touchType)
            {
                case EcsTouchType.InvoiceReject:
                case EcsTouchType.CaseErect:
                    this.LabelPrinterZebraEquipmentSetting = new LabelPrinterZebraZt411EquipmentSetting();
                    break;
            }
        }

        public EquipmentCollectionForTouchSetting()
        {
            this.LabelPrinterZebraEquipmentSetting = new LabelPrinterZebraZt411EquipmentSetting();
        }
    }
}
