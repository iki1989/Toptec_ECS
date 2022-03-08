using System;
using System.Windows.Forms;
using ECS.Core.Equipments;
using ECS.Model;
using Urcis.SmartCode.Net.Tcp;

namespace TestWindowsFormsApp
{
    public partial class UserControlInkjet : UserControl
    {
        InkjectEquipment eq;

        public UserControlInkjet()
        {
            InitializeComponent();

            InkjectEquipmentSetting setting = new InkjectEquipmentSetting();
            setting.Name = HubServiceName.InkjectEquipment1;
            this.propertyGrid1.SelectedObject = setting;
            this.propertyGrid1.ExpandAllGridItems();
            this.eq = new InkjectEquipment(setting);
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

            this.eq.Communicator.PrintCompleteResponse += this.Communicator_OnPrintComplete;
            this.eq.Communicator.FirstConnectedRecived += this.Communicator_OnFirstConnectedRecived;
            this.eq.Communicator.EnablePrintComplete += this.Communicator_EnablePrintComplete;
            this.eq.Communicator.AutoDataRecordResponse += this.Communicator_AutoDataRecordResponse;
            this.eq.Communicator.WriteAutoDataRecivedResponse += this.Communicator_WriteAutoDataRecivedResponse;
            this.eq.Communicator.WriteAutoDataQueueClearResponse += this.Communicator_WriteAutoDataQueueClearResponse;
            this.eq.Communicator.ReadInkLevelResponse += this.Communicator_ReadInkLevelResponse;
            this.eq.Communicator.GetAutoDataStringResponse += this.Communicator_GetAutoDataStringResponse;
            //this.eq.Communicator.WrtieSystemDateAndTimeResponse += this.Communicator_WrtieSystemDateAndTimeResponse;
            //this.eq.Communicator.ProductionCounterResponse += this.Communicator_ProductionCounterResponse;
            //this.eq.Communicator.GetCounterInfoResponse += Communicator_GetCounterInfoResponse;
            this.eq.Communicator.TcpConnectionStateChanged += this.Communicator_TcpConnectionStateChanged;
            this.eq.Communicator.OperationStateChanged += this.Communicator_OperationStateChanged;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (this.propertyGrid1.SelectedObject is InkjectEquipmentSetting setting)
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

        #region Send
        private void buttonEnablPrintCompleteAcknowledgementSend_Click(object sender, EventArgs e)
        {
            this.eq.EnablPrintCompleteAcknowledgementSend();
            this.WriteLog("Sent : Enabl Print Complete Acknowledgement");
        }

        private void buttonReadAutoDataStatusSend_Click(object sender, EventArgs e)
        {
            this.eq.ReadAutoDataStatusSend();
            this.WriteLog("Sent : Read Auto Data Status");
        }

        int i = 1;
        private void buttonWriteAutoDataRecordSend_Click(object sender, EventArgs e)
        {
            this.eq.WriteAutoDataRecordSend(2, "A", 2, i);
            i++;
            this.WriteLog("Sent : Write Auto Data Record");
        }

        private void buttonClearAutoDataQueueSend_Click(object sender, EventArgs e)
        {
            this.eq.ClearAutoDataQueueSend();
            this.WriteLog("Sent : Clear Auto Data Queue");
        }

        private void buttonReadInkLevelSend_Click(object sender, EventArgs e)
        {
            this.eq.ReadInkLevelSend();
            this.WriteLog("Sent : Read Ink Level");
        }

        private void buttonGetAutoDataStringSend_Click(object sender, EventArgs e)
        {
            this.eq.GetAutoDataStringSend();
            this.WriteLog("Sent : Get Auto Data String");
        }
        #endregion

        #region Recive
        private void Communicator_OnPrintComplete(object sender, EventArgs e)
        {
            this.WriteLog("Recived : Print Complete");
        }

        private void Communicator_OnFirstConnectedRecived(object sender, bool e)
        {
            this.WriteLog($"Recived : First Connected Recived : {e}");
        }

        private void Communicator_EnablePrintComplete(object sender, EventArgs e)
        {
            this.WriteLog("Recived : Enable Print Complete");
        }

        private void Communicator_GetAutoDataStringResponse(object sender, string e)
        {
            this.WriteLog($"Recived : Get Auto Data String Response : {e}");
        }

        private void Communicator_WrtieSystemDateAndTimeResponse(object sender, DateTime e)
        {
            this.WriteLog($"Recived : Wrtie System Date And Time Response : {e}");
        }

        private void Communicator_ReadInkLevelResponse(object sender, int e)
        {
            this.WriteLog($"Recived : Read Ink Level Response : {e}");
        }

        private void Communicator_AutoDataRecordResponse(object sender, ECS.Model.Inkject.AutoDataResponseEnum e)
        {
            this.WriteLog($"Recived : Read Auto Data Record Response : {e}");
        }

        private void Communicator_WriteAutoDataRecivedResponse(object sender, EventArgs e)
        {
            this.WriteLog($"Recived : Write Auto Data Recived Response");
        }

        private void Communicator_WriteAutoDataQueueClearResponse(object sender, EventArgs e)
        {
            this.WriteLog($"Recived : Write Auto Data Queue Clear Response");
        }

        private void Communicator_ProductionCounterResponse(object sender, int e)
        {
            this.WriteLog($"Recived : Production Counter Response : {e}");
        }

        private void Communicator_GetCounterInfoResponse(object sender, ECS.Model.Inkject.CounterInfo e)
        {
            this.WriteLog($"Get Counter Info Response : {e.Current}");
        }

        private void Communicator_TcpConnectionStateChanged(object sender, Urcis.SmartCode.Net.Tcp.TcpConnectionStateChangedEventArgs e)
        {
            this.WriteLog($"Tcp Connection State Changed : {e.Current}");
        }

        private void Communicator_OperationStateChanged(object sender, Urcis.SmartCode.Net.CommunicatorOperationStateChangedEventArgs e)
        {
            this.WriteLog($"Operation State Changed : {e.Current}");
        }

        #endregion

       
    }
}
