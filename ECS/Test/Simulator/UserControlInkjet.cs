using ECS.Model.Inkject;
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
using ECS.Core.Managers;

namespace Simulator
{
    public partial class UserControlInkjet : UserControl
    {
        private SimulatorInkject simnulator;
       
        public UserControlInkjet(string name, int port) : base()
        {
            InitializeComponent();
            this.groupBox1.Text = name;

            this.simnulator = new SimulatorInkject(this);
            this.simnulator.CurrentIdUpdate += this.Simnulator_CurrentIdUpdate;

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
                this.buttonPrintComplete.Enabled = enable;
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
            this.simnulator.InkPer = (int)this.numericUpDownInk.Value;
            this.simnulator.CurerntId = this.textBoxCurrentId.Text;

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

        private void buttonPrintComplete_Click(object sender, EventArgs e)
        {
            string msg = "ACK-Print Complete";
            this.simnulator.SendMessage(msg);
            this.WriteLog(msg);
            
            if (string.IsNullOrEmpty(this.simnulator.CurerntId)) return;
            string id = this.simnulator.CurerntId;
            if (id.Length > 3)
            {
                string pre = id.Substring(0, 3);
                string no = id.Substring(3, id.Length - 3);
                int noLength = id.Length - 3;
                if (long.TryParse(no, out long result))
                {
                    string sResult = $"{++result}";

                    this.simnulator.CurerntId = $"{pre}{sResult.PadLeft(noLength, '0')}";
                    this.Simnulator_CurrentIdUpdate(this, this.simnulator.CurerntId);
                }
            }

        }

        private void textBoxCurrentId_TextChanged(object sender, EventArgs e)
        {
            this.simnulator.CurerntId = this.textBoxCurrentId.Text;
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

        private void Simnulator_CurrentIdUpdate(object sender, string e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate ()
                {
                    this.Simnulator_CurrentIdUpdate(sender, e);
                }));
            }
            else
                this.textBoxCurrentId.Text = e;
        }

        private void numericUpDownInk_ValueChanged(object sender, EventArgs e)
        {
            this.simnulator.InkPer = (int)this.numericUpDownInk.Value;
        }
    }

    public class SimulatorInkject : TcpCommunicator
    {
        public event EventHandler<string> CurrentIdUpdate;

        private const char LF = (char)0x0A; //End

        public string CurerntId { get; set; }

        public int InkPer { get; set; }

        public SimulatorInkject(object owner) : base(owner)
        {
            this.TcpConnectionStateChanged += this.SimulatorInkject_TcpConnectionStateChanged;
        }

        public void ApplySetting(TcpCommunicatorSetting setting)
        {
            setting = setting ?? new TcpCommunicatorSetting();
            base.ApplySetting(setting);
        }

        protected override bool OnReceive()
        {
            byte[] bytes = new byte[128];
            int result = 0;

            try
            {
                if (this.Client != null)
                    result = this.Client.Receive(bytes);

                if (result == 0)
                {
                    this.StartRestart(RestartReasonEnum.CommunicationFailure);
                    return false;
                }

                string message = Encoding.ASCII.GetString(bytes, 0, result);
                if (message.EndsWith(LF.ToString()))
                {
                    string msg = message.Remove(message.Length - 1);
                    this.WriteLog($"Recived : {msg}");

                    string sentText = string.Empty;
                    if (msg.StartsWith("A"))
                        sentText = "ACK-Print Complete Enabled";
                    else
                    if (msg.StartsWith("D"))
                    {
                        this.CurerntId = msg.Remove(0, 1).Replace(" ", "0");
                        this.CurrentIdUpdate?.Invoke(this, this.CurerntId);

                        sentText = "ACK-Auto Data Received";
                    }
                    else if (msg.StartsWith("o"))
                    {
                        string sInkPer = $"{this.InkPer}".PadLeft(3, '0');
                        sentText = $"ACK-Ink Level={sInkPer}%";
                    }
                    else if (msg.StartsWith("C"))
                        sentText = "ACK-Auto Data XON";
                    else if (msg.StartsWith("GET_AUTO_DATA_STRING"))
                        sentText = $"ACK-AUTO_DATA_STRING={this.CurerntId}";
                    #region Old
                    //else if (msg.StartsWith("T"))
                    //    sentText = $"ACK-DateTime = {DateTime.Now.ToString("MM/dd/yyyy HH:mm")}";
                    //else if (msg.StartsWith("PRODUCTION_COUNTER="))
                    //{
                    //    string[] splited = msg.Split('=');
                    //    if (splited.Length == 2)
                    //    {
                    //        if (int.TryParse(splited[1], out int count))
                    //        {
                    //            string id = this.CurerntId;
                    //            if (id.Length > 3)
                    //            {
                    //                string pre = id.Substring(0, 3);
                    //                int noLength = id.Length - 3;
                    //                string sResult = count.ToString();
                    //                this.CurerntId = $"{pre}{sResult.PadLeft(noLength, '0')}";
                    //                this.CurrentIdUpdate?.Invoke(this, this.CurerntId);
                    //                sentText = $"ACK-Production_Counter={count}";
                    //            }
                    //            else
                    //                sentText = $"ACK-Production_Counter=ERROR";
                    //        }
                    //        else
                    //        {
                    //            string id = this.CurerntId;
                    //            if (id.Length > 3)
                    //            {
                    //                string pre = id.Substring(0, 3);
                    //                string no = id.Substring(3, id.Length - 3);
                    //                int noLength = id.Length - 3;
                    //                if (int.TryParse(no, out int result2))
                    //                {
                    //                    string sResult = result2.ToString();
                    //                    this.CurerntId = $"{pre}{sResult.PadLeft(noLength, '0')}";
                    //                    sentText = $"ACK-Production_Counter={result2}";
                    //                }
                    //            }
                    //            else
                    //                sentText = $"ACK-Production_Counter=ERROR";
                    //        }
                    //    }
                    //}
                    #endregion
                    else if (msg.StartsWith("GET_COUNTER_INFO"))
                        sentText = $"ACK-GET_COUNTER_INFO=1, 2, 3, {DirectionEnum.Down}, {TypeEnum.Numeric}";
                    else if (msg.StartsWith("SET_COUNTER_INFO"))
                    {
                        sentText = $"ACK-SET_COUNTER_INFO Successful, New vlaue is {100}";
                    }

                    this.SendMessage(sentText);

                        if (string.IsNullOrEmpty(sentText) == false)
                            this.WriteLog($"Reply Sent : {sentText}");
                    }

                return true;
            }
            catch (Exception ex)
            {

                if (ex is ObjectDisposedException || ex is NullReferenceException) { }
                else
                    this.StartRestart(RestartReasonEnum.CommunicationFailure);

                return false;
            }
        }

        public void SendMessage(string message)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(message + LF);
            this.SendBytes(bytes);
        }

        private void WriteLog(string text)
        {
            if (this.Owner is UserControlInkjet inkjet)
                inkjet.WriteLog(text);
        }

        private void SimulatorInkject_TcpConnectionStateChanged(object sender, TcpConnectionStateChangedEventArgs e)
        {
            if (this.TcpConnectionState == TcpConnectionStateEnum.Connected)
            {
                string text = "Connected to Copilot printer";
                this.SendMessage(text);
                this.WriteLog(text);
            }
        }
    }
}
