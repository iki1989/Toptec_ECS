using System;
using System.Windows.Controls;
using System.Windows.Threading;
using Urcis.SmartCode.Io;
using Urcis.SmartCode.WindowsForms;
using ECS.Model;
using System.Collections.Generic;
using System.Windows;

namespace ECS.Server.Views
{
    /// <summary>
    /// Interaction logic for PlcIoMonitoring.xaml
    /// </summary>
    public partial class MonitoringPlcIoScreen : Page
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private EventHandler tickEventHandler;

        private IMaintenanceControl IoMonitorPicking1;
        private IMaintenanceControl IoMonitorPicking2;
        private IMaintenanceControl IoMonitorCaseErect;
        private IMaintenanceControl IoMonitorWeightInvoice;
        private IMaintenanceControl IoMonitorSmartpacking;

        private bool isLoaded;

        public MonitoringPlcIoScreen()
        {
            InitializeComponent();

            this.timer.Interval = TimeSpan.FromMilliseconds(500);

            this.InitMonitoring(HubServiceName.PlcPicking1Equipment, this.ControlPicking1);
            this.InitMonitoring(HubServiceName.PlcPicking2Equipment, this.ControlPicking2);
            this.InitMonitoring(HubServiceName.PlcCaseErectEquipment, this.ControlCaseErect);
            this.InitMonitoring(HubServiceName.PlcWeightInvoiceEquipment, this.ControlWeightInvoice);
            this.InitMonitoring(HubServiceName.PlcSmartPackingEquipment, this.ControlSmartPacking);

            this.tickEventHandler = new EventHandler(timer_Tick);

            this.Loaded += MonitoringPlcIoScreen_Loaded;
            this.Unloaded += MonitoringPlcIoScreen_Unloaded;
        }


        private void InitMonitoring(string communicatorName, System.Windows.Forms.Control control)
        {
            IMaintenanceControlProvider maintenanceControlProvider = IoServer.GetCommunicator(communicatorName) as IMaintenanceControlProvider;
            if (maintenanceControlProvider != null)
            {
                System.Windows.Forms.Control ioControl = null;
                if (communicatorName == HubServiceName.PlcPicking1Equipment)
                {
                    this.IoMonitorPicking1 = maintenanceControlProvider.GetMaintenanceControl();
                    ioControl = this.IoMonitorPicking1 as System.Windows.Forms.Control;
                }
                else if (communicatorName == HubServiceName.PlcPicking2Equipment)
                {
                    this.IoMonitorPicking2 = maintenanceControlProvider.GetMaintenanceControl();
                    ioControl = this.IoMonitorPicking2 as System.Windows.Forms.Control;
                }
                else if (communicatorName == HubServiceName.PlcCaseErectEquipment)
                {
                    this.IoMonitorCaseErect = maintenanceControlProvider.GetMaintenanceControl();
                    ioControl = this.IoMonitorCaseErect as System.Windows.Forms.Control;
                }
                else if (communicatorName == HubServiceName.PlcWeightInvoiceEquipment)
                {
                    this.IoMonitorWeightInvoice = maintenanceControlProvider.GetMaintenanceControl();
                    ioControl = this.IoMonitorWeightInvoice as System.Windows.Forms.Control;
                }
                else if (communicatorName == HubServiceName.PlcSmartPackingEquipment)
                {
                    this.IoMonitorSmartpacking = maintenanceControlProvider.GetMaintenanceControl();
                    ioControl = this.IoMonitorSmartpacking as System.Windows.Forms.Control;
                }

                if (ioControl != null)
                {
                    ioControl.Dock = System.Windows.Forms.DockStyle.Fill;
                    control.Controls.Add(ioControl);
                }
            }
        }

        private void Display()
        {
            if (this.tab.SelectedItem is TabItem tab)
            {
                if (tab.Name == "Picking1Tab")
                    this.IoMonitorPicking1?.Display();
                else if (tab.Name == "Picking2Tab")
                    this.IoMonitorPicking2?.Display();
                else if (tab.Name == "CaseErectTab")
                    this.IoMonitorCaseErect?.Display();
                else if (tab.Name == "WeightInvoiceTab")
                    this.IoMonitorWeightInvoice?.Display();
                else if (tab.Name == "SmartPackingTab")
                    this.IoMonitorSmartpacking?.Display();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (this.isLoaded && this.Visibility == Visibility.Visible)
                this.Display();
        }

        private void MonitoringPlcIoScreen_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.isLoaded = true;
            this.timer.Tick += this.tickEventHandler;
            this.timer.Start();
        }

        private void MonitoringPlcIoScreen_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.isLoaded = false;
            this.timer.Stop();
            this.timer.Tick -= this.tickEventHandler;
        }
    }
}
