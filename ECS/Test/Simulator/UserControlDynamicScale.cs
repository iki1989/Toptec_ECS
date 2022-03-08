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
    public partial class UserControlDynamicScale : UserControl
    {
        private SimulatorDynamicScale simnulator;

        public UserControlDynamicScale(string name, int port) : base()
        {
            InitializeComponent();
            this.groupBox1.Text = name;

            this.simnulator = new SimulatorDynamicScale(this);

            TcpCommunicatorSetting setting = new TcpCommunicatorSetting();
            setting.Name = name;
            setting.Port = port;
            setting.Active = false;

            this.propertyGrid1.SelectedObject = setting;
        }

        private void SendButtonEnable(bool enable)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    this.SendButtonEnable(enable);
                }));
            }
            else
            {
                this.buttonSend.Enabled = enable;
            }
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

            this.SendButtonEnable(false);
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

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string weight = this.textBoxWeight.Text;
            string uinit = "Kg";
            this.simnulator.SendMessage(weight, uinit);
            this.WriteLog($"Sent : weight={weight} uinit={uinit}");
        }

        private void Simnulator_TcpConnectionStateChanged(object sender, TcpConnectionStateChangedEventArgs e)
        {
            this.WriteLog($"Tcp Connection State Changed = {e.Current}");

            if (e.Current == TcpConnectionStateEnum.Connected)
                this.SendButtonEnable(true);
            else
                this.SendButtonEnable(false);
        }

        private void Simnulator_OperationStateChanged(object sender, Urcis.SmartCode.Net.CommunicatorOperationStateChangedEventArgs e)
        {
            this.WriteLog($"Operation State Changed = {e.Current}");
        }

    }

    public class SimulatorDynamicScale : TcpCommunicator
    {
        private const char CR = (char)0x0D;
        private const char LF = (char)0X0A;

        public SimulatorDynamicScale(object owner) : base(owner) { }

        public void ApplySetting(TcpCommunicatorSetting setting)
        {
            setting = setting ?? new TcpCommunicatorSetting();
            base.ApplySetting(setting);
        }

        protected override bool OnReceive() => this.ReceiveBytes(new byte[128]);

        public void SendMessage(string weight, string unit)
        {
            string message = $"{weight.PadLeft(10, ' ')} {unit.PadLeft(3, ' ')}{CR}{LF}";

            byte[] bytes = Encoding.ASCII.GetBytes(message);
            this.SendBytes(bytes);
        }
    }
}
