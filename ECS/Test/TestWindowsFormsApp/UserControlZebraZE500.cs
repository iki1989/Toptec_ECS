using System;
using System.Windows.Forms;
using ECS.Core.Equipments;
using ECS.Model;
using ECS.Model.LabelPrinter;

namespace TestWindowsFormsApp
{
    public partial class UserControlZebraZE500 : UserControl
    {
        public LabelPrinterZebraZe500Equipment eq;

        public UserControlZebraZE500()
        {
            InitializeComponent();

            LabelPrinterZebraZe500EquipmentSetting setting = new LabelPrinterZebraZe500EquipmentSetting();
            setting.Name = HubServiceName.SmartLabelPrinterZebraZe500Equipment;
            this.propertyGrid1.SelectedObject = setting;
            this.propertyGrid1.ExpandAllGridItems();
            this.eq = new LabelPrinterZebraZe500Equipment(setting);
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

            this.eq.Communicator.TcpConnectionStateChanged += this.Communicator_TcpConnectionStateChanged;
            this.eq.Communicator.OperationStateChanged += this.Communicator_OperationStateChanged;
            this.eq.Communicator.PrintSkip += Communicator_PrintSkip;
            this.eq.Communicator.PrintOkResponse += Communicator_PrintOkResponse;
            this.eq.Communicator.LabelRow += Communicator_LabelRow;
            this.eq.Communicator.LabelNormal += Communicator_LabelNormal;
            this.eq.Communicator.LabelError += Communicator_LabelError;
            this.eq.Communicator.LabelAttachCompleted += Communicator_LabelAttachCompleted1; ;
        }

        private void Communicator_LabelAttachCompleted1(object sender, string e)
        {
            this.WriteLog($"Recived : LabelAttachCompleted boxId = {e}");
        }

        private void Communicator_LabelAttachCompleted(object sender, EventArgs e)
        {
            this.WriteLog($"Recived : LabelAttachCompleted");
        }

        private void Communicator_LabelError(object sender, LabelErrorEnum e)
        {
            this.WriteLog($"Recived : LabelError");
        }

        private void Communicator_LabelNormal(object sender, EventArgs e)
        {
            this.WriteLog($"Recived : LabelNormal");
        }

        private void Communicator_LabelRow(object sender, EventArgs e)
        {
            this.WriteLog($"Recived : LabelRow");
        }

        private void Communicator_PrintOkResponse(object sender, EventArgs e)
        {
            this.WriteLog($"Recived : PrintOkResponse");
        }

        private void Communicator_PrintSkip(object sender, EventArgs e)
        {
            this.WriteLog($"Recived : PrintSkip");
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (this.propertyGrid1.SelectedObject is LabelPrinterZebraZe500EquipmentSetting setting)
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

        #region Recived
        private void Communicator_HostStatusReturnRecived(object sender, ECS.Model.LabelPrinter.HostStatusReturnArg e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate ()
                {
                    this.Communicator_HostStatusReturnRecived(sender, e);
                }));
            }
            else
            {
                this.WriteLog("Recievd : PrintCompleted");
                this.propertyGridState.SelectedObject = e;
            }
        }
        #endregion

        #region Send
        private void State_Click(object sender, EventArgs e)
        {
            this.eq.StateRequestSend();
            this.WriteLog($"Sent : StateRequestSend");
        }

        private void buttonZplPrint_Click(object sender, EventArgs e)
        {
            this.eq.Communicator.SendMessage(this.richTextBoxZplPrint.Text);
            this.WriteLog($"Sent : ZplPrint");
        }

        private void buttonBufferClear_Click(object sender, EventArgs e)
        {
            this.eq.BufferClearSend();
            this.WriteLog($"Sent : StateRequestSend");
        }

        private void Communicator_OperationStateChanged(object sender, Urcis.SmartCode.Net.CommunicatorOperationStateChangedEventArgs e)
        {
            this.WriteLog($"Operation State Changed : {e.Current}");
        }

        private void Communicator_TcpConnectionStateChanged(object sender, Urcis.SmartCode.Net.Tcp.TcpConnectionStateChangedEventArgs e)
        {
            this.WriteLog($"Tcp Connection State Changed : {e.Current}");
        }

        #endregion

        
    }
}
