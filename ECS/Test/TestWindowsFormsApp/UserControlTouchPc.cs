using System;
using System.Windows.Forms;
using ECS.Core.Equipments;
using ECS.Model.Pcs;

namespace TestWindowsFormsApp
{
    public partial class UserControlTouchPc : UserControl
    {
        TouchPcBcrLcdEquipment eq;

        public UserControlTouchPc()
        {
            InitializeComponent();

            PcEquipmenttSetting setting = new PcEquipmenttSetting();
            this.propertyGrid1.SelectedObject = setting;
            this.propertyGrid1.ExpandAllGridItems();
            this.eq = new TouchPcBcrLcdEquipment(setting);
        }

        private void WriteLog(string text)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    this.WriteLog(text);
                }));
            }
            else
            {
                this.richTextBoxLog.AppendText($"{DateTime.Now.ToString("HH:mm:ss.fff")} > {text}{Environment.NewLine}");
                this.richTextBoxLog.ScrollToCaret();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.eq.Create();
            this.eq.Prepare();


            this.eq.Communicator.HsmsConnectionStateChanged += Communicator_HsmsConnectionStateChanged;
            this.eq.Communicator.TcpConnectionStateChanged += this.Communicator_TcpConnectionStateChanged;
            this.eq.Communicator.OperationStateChanged += this.Communicator_OperationStateChanged;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (this.propertyGrid1.SelectedObject is PcEquipmenttSetting setting)
            {
                this.eq.Setting.CommunicatorSetting.Ip = setting.CommunicatorSetting.Ip;
                this.eq.Setting.CommunicatorSetting.Port = setting.CommunicatorSetting.Port;
                this.eq.Setting.Name = setting.Name;
                this.eq.Start();
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.eq.Stop();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.richTextBoxLog.Clear();
        }

        private void Communicator_OperationStateChanged(object sender, Urcis.SmartCode.Net.CommunicatorOperationStateChangedEventArgs e)
        {
            this.WriteLog($"Operation State Changed : {e.Current}");
        }

        private void Communicator_TcpConnectionStateChanged(object sender, Urcis.SmartCode.Net.Tcp.TcpConnectionStateChangedEventArgs e)
        {
            this.WriteLog($"Tcp Connection State Changed : {e.Current}");
        }

        private void Communicator_HsmsConnectionStateChanged(object sender, Urcis.Secl.HsmsConnectionStateChangedEventArgs e)
        {
            this.WriteLog($"Hsms Connection State Changed : {e.Current}");
        }
    }
}
