using System;
using System.Windows.Forms;
using ECS.Core.Equipments;

namespace TestWindowsFormsApp
{
    public partial class UserControlBixolon : UserControl
    {
        LabelPrinterBixolonEquipment eq;
        public UserControlBixolon()
        {
            InitializeComponent();

            this.eq = new LabelPrinterBixolonEquipment(null);
            eq.Create();
            eq.Prepare();
        }

        private async void Print_Click(object sender, EventArgs e)
        {
            await this.eq.PrintSendAsync(this.textBoxText.Text);
        }

        private async void buttonStart_Click(object sender, EventArgs e)
        {
            eq.Setting.CommunicatorSetting.Ip = this.textBoxIP.Text;
            eq.Setting.CommunicatorSetting.Port = Convert.ToInt32(this.textBoxPort.Text);
            await eq.StartAsync();
        }

        private async void buttonStop_Click(object sender, EventArgs e)
        {
            await eq.StopAsync();
        }

        private async void buttonReprint_Click(object sender, EventArgs e)
        {
            await eq.ReprintSendAsync();
        }
    }
}
