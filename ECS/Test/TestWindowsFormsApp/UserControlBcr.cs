using ECS.Core.Communicators;
using ECS.Core.Equipments;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Urcis.SmartCode.Net.Tcp;

namespace TestWindowsFormsApp
{
    public partial class UserControlBcr : UserControl
    {
        BcrCommunicator Bcr;
        LabelPrinterZebraZe500Equipment Label;

        public UserControlBcr()
        {
            InitializeComponent();

            BcrCommunicatorSetting setting = new BcrCommunicatorSetting();
            this.propertyGrid1.SelectedObject = setting;
            this.propertyGrid1.ExpandAllGridItems();
            this.Bcr = new BcrCommunicator(this);

        }

        public void SetPirnter(LabelPrinterZebraZe500Equipment label)
        {
            Label = label;
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


            this.Bcr.ReadData += this.Communicator_ReadData;
            this.Bcr.Noread += this.Communicator_Noread;
            this.Bcr.TcpConnectionStateChanged += this.Communicator_TcpConnectionStateChanged;
            this.Bcr.OperationStateChanged += this.Communicator_OperationStateChanged;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (this.propertyGrid1.SelectedObject is BcrCommunicatorSetting setting)
            {
                if (this.Bcr.Setting == null)
                {
                    setting.Name += "test";
                    this.Bcr.ApplySetting(setting);
                }
                this.Bcr.Setting.Ip = setting.Ip;
                this.Bcr.Setting.Port = setting.Port;
                this.Bcr.Setting.Name = setting.Name;

                //this.Bcr?.ApplySetting(this.Bcr.Setting);
                this.Bcr.Start();
            }
            Bcr.Start();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            Bcr.Stop();
        }

        private void Communicator_TcpConnectionStateChanged(object sender, TcpConnectionStateChangedEventArgs e)
        {
            this.WriteLog($"Tcp Connection State Changed : {e.Current}");
        }

        private void Communicator_OperationStateChanged(object sender, Urcis.SmartCode.Net.CommunicatorOperationStateChangedEventArgs e)
        {
            this.WriteLog($"Operation State Changed : {e.Current}");
        }

        private void Communicator_Noread(object sender, EventArgs e)
        {
            this.WriteLog($"Recived : Noread");

            string zpl = File.ReadAllText("D:\\test.txt");

            Thread.Sleep(500);
            Label.ZplPrintSend("NOREAD0001", zpl);
        }

        private void Communicator_ReadData(object sender, string e)
        {
            this.WriteLog($"Recived : {e}");

            string zpl = File.ReadAllText("D:\\test.txt");

            Thread.Sleep(500);
            Label.ZplPrintSend($"{e}", zpl);
        }
    }
}
