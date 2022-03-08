using ECS.Model.LabelPrinter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Urcis.SmartCode.Net.Tcp;

namespace Simulator
{
    public partial class UserControlZebraZt411 : UserControl
    {
        private SimulatorZebraZt411 simnulator;

        public UserControlZebraZt411(string name, int port) : base()
        {
            InitializeComponent();
            this.groupBox1.Text = name;

            this.simnulator = new SimulatorZebraZt411(this);

            TcpCommunicatorSetting setting = new TcpCommunicatorSetting();
            setting.Name = name;
            setting.Port = port;
            setting.Active = false;

            this.propertyGrid1.SelectedObject = setting;
            this.propertyGridStatus.SelectedObject = this.simnulator.hostStatusReturnArg;
        }

        public void WriteLog(string text)
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

            this.simnulator.OperationStateChanged += this.Simnulator_OperationStateChanged;
            this.simnulator.TcpConnectionStateChanged += this.Simnulator_TcpConnectionStateChanged;
            this.buttonStart_Click(this, null);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (this.propertyGrid1.SelectedObject is TcpCommunicatorSetting setting)
            {
                if (this.simnulator.TcpConnectionState == TcpConnectionStateEnum.Disconnected)
                {
                    this.simnulator.ApplySetting(setting);
                }

                this.simnulator.Start();
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.simnulator.Stop();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.richTextBoxLog.Clear();
        }


        private void Simnulator_TcpConnectionStateChanged(object sender, TcpConnectionStateChangedEventArgs e)
        {
            this.WriteLog($"Tcp Connection State Changed = {e.Current}");
        }

        private void Simnulator_OperationStateChanged(object sender, Urcis.SmartCode.Net.CommunicatorOperationStateChangedEventArgs e)
        {
            this.WriteLog($"Operation State Changed = {e.Current}");
        }
    }

    public class SimulatorZebraZt411 : TcpCommunicator
    {
        public HostStatusReturnArg hostStatusReturnArg { get; set; } = new HostStatusReturnArg();

        public SimulatorZebraZt411(object owner) : base(owner) { }

        protected override bool OnReceive() => this.ReceiveBytes(new byte[128]);

        public void ApplySetting(TcpCommunicatorSetting setting)
        {
            setting = setting ?? new TcpCommunicatorSetting();
            base.ApplySetting(setting);
        }
    }
}
