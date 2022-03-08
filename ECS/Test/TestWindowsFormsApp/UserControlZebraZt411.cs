using System;
using System.Windows.Forms;
using ECS.Core.Equipments;
using ECS.Model;
using ECS.Model.LabelPrinter;

namespace TestWindowsFormsApp
{
    public partial class UserControlZebraZt411 : UserControl
    {
        LabelPrinterZebraZt411Equipment eq;

        public UserControlZebraZt411()
        {
            InitializeComponent();

            LabelPrinterZebraZt411EquipmentSetting setting = new LabelPrinterZebraZt411EquipmentSetting();
            setting.Name = HubServiceName.SmartLabelPrinterZebraZe500Equipment;
            this.propertyGrid1.SelectedObject = setting;
            this.propertyGrid1.ExpandAllGridItems();
            this.eq = new LabelPrinterZebraZt411Equipment(setting);
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

            this.eq.Communicator.HostStatusReturnRecived += this.Communicator_HostStatusReturnRecived;
            this.eq.Communicator.TcpConnectionStateChanged += this.Communicator_TcpConnectionStateChanged;
            this.eq.Communicator.OperationStateChanged += this.Communicator_OperationStateChanged;
        
        }
        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (this.propertyGrid1.SelectedObject is LabelPrinterZebraZt411EquipmentSetting setting)
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
        private void Print_Click(object sender, EventArgs e)
        {
            string text = this.textBoxText.Text;
            this.eq.RejectBoxPrintSend(text);
            this.WriteLog($"Sent : Print{text}");
        }

        private void buttonZplPrint_Click(object sender, EventArgs e)
        {
            this.eq.Communicator.SendMessage(this.richTextBoxZplPrint.Text);
            this.WriteLog($"Sent : ZplPrint");
        }

        private void buttonHostStatus_Click(object sender, EventArgs e)
        {
            this.eq.HostStatusReturnSend();
            this.WriteLog($"Sent : Host Status Return");
        }

        private void buttonCancelAll_Click(object sender, EventArgs e)
        {
            this.eq.CancelAllSend();
            this.WriteLog($"Sent : Cancel All");
        }

        private void buttonReprintAfterError_Click(object sender, EventArgs e)
        {
            this.eq.ReprintAfterErrorSend();
            this.WriteLog($"Sent : Reprint After Error");
        }

        private void Communicator_OperationStateChanged(object sender, Urcis.SmartCode.Net.CommunicatorOperationStateChangedEventArgs e)
        {
            this.WriteLog($"Operation State Changed : {e.Current}");
        }

        private void Communicator_TcpConnectionStateChanged(object sender, Urcis.SmartCode.Net.Tcp.TcpConnectionStateChangedEventArgs e)
        {
            this.WriteLog($"Tcp Connection State Changed : {e.Current}");
        }

        private void buttonNoWeightPrint_Click(object sender, EventArgs e)
        {
            this.eq.InvoicePrintSend("No Weight");
        }

        private void buttonDuplicatePrint_Click(object sender, EventArgs e)
        {
            this.eq.InvoicePrintSend("Duplicate");
        }
        #endregion


    }
}
