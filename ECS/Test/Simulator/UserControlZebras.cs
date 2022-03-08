using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulator
{
    public partial class UserControlZebras : UserControl
    {
        private UserControlZebraZt411 zebra1;
        private UserControlZebraZt411 zebra2;
        private UserControlZebraZt411 zebra3;
        private UserControlZebraZt411 zebra4;

        public UserControlZebras()
        {
            InitializeComponent();

            this.zebra1 = new UserControlZebraZt411("Zebra#Smart", 6101);
            this.zebra2 = new UserControlZebraZt411("Zebra#Normal", 6102);
            this.zebra3 = new UserControlZebraZt411("Zebra#CaseErectReject", 6103);
            this.zebra4 = new UserControlZebraZt411("Zebra#InvoiceReject", 6104);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.flowLayoutPanel1.Controls.Add(this.zebra1);
            this.flowLayoutPanel1.Controls.Add(this.zebra2);
            this.flowLayoutPanel1.Controls.Add(this.zebra3);
            this.flowLayoutPanel1.Controls.Add(this.zebra4);
        }
    }
}
