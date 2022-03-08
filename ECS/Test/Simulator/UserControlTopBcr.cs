using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Urcis.SmartCode.Net.Tcp;

namespace Simulator
{
    public partial class UserControlTopBcr : UserControl
    {
        private SimulatorBcr simnulator;

        public UserControlTopBcr(string name, int port) : base()
        {
            InitializeComponent();
            this.groupBox1.Text = name;

            this.simnulator = new SimulatorBcr(this);

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
                this.buttonNoReadSend.Enabled = enable;
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
            string text = $"{this.textBoxSide.Text},{this.textBoxTop.Text}";
            this.simnulator.SendMessage(text);
            this.WriteLog($"Sent : {text}");
        }

        private void buttonNoReadSend_Click(object sender, EventArgs e)
        {
            string noReadText = "NOREAD,NOREAD";
            this.simnulator.SendMessage(noReadText);
            this.WriteLog($"Sent : {noReadText}");
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

    public class SimulatorTopBcr : TcpCommunicator
    {
        private const char STX = (char)0x02;
        private const char ETX = (char)0x03;

        public SimulatorTopBcr(object owner) : base(owner) { }

        public void ApplySetting(TcpCommunicatorSetting setting)
        {
            setting = setting ?? new TcpCommunicatorSetting();
            base.ApplySetting(setting);
        }

        protected override bool OnReceive() => this.ReceiveBytes(new byte[128]);

        public void SendMessage(string text)
        {
            string message = $"{STX}{text}{ETX}";

            byte[] bytes = Encoding.ASCII.GetBytes(message);
            this.SendBytes(bytes);
        }
    }
}
