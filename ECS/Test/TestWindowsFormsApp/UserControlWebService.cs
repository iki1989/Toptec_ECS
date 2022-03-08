using ECS.Core.Managers;
using System;
using System.Windows.Forms;

namespace TestWindowsFormsApp
{
    public partial class UserControlWebService : UserControl
    {
        WebServiceManager WebService;
        public UserControlWebService(string ip, int port)
        {
            InitializeComponent();

            WebServiceManagerSetting setting = new WebServiceManagerSetting();
            setting.WebServiceName = "Restful Server";
            setting.IP = ip;
            setting.Port = port;
            this.propertyGrid1.SelectedObject = setting;

            WebService = new WebServiceManager(setting);
            WebService.Create();
            WebService.Prepare();

            this.buttonStart_Click(this, null);
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

        private void buttonStart_Click(object sender, EventArgs e)
        {
            this.WebService.Start();
            this.WriteLog("WebService Start");
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.WebService.Stop();
            this.WriteLog("WebService Stop");
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.richTextBoxLog.Clear();
        }
    }
}
