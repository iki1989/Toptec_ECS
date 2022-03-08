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
    public partial class UserControlBcrs : UserControl
    {
        private UserControlBcr bcr1;
        private UserControlBcr bcr2;
        private UserControlBcr bcr3;
        private UserControlBcr bcr4;
        private UserControlBcr bcr5;
        private UserControlBcr bcr6;

        public UserControlBcrs()
        {
            InitializeComponent();

            this.bcr1 = new UserControlBcr("bcr#1", 8005);
            this.bcr2 = new UserControlBcr("bcr#2", 8006);
            this.bcr3 = new UserControlBcr("bcr#3", 8007);
            this.bcr4 = new UserControlBcr("bcr#4", 8008);
            this.bcr5 = new UserControlBcr("bcr#5", 8009);
            this.bcr6 = new UserControlBcr("bcr#6", 8010);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.flowLayoutPanel1.Controls.Add(this.bcr1);
            this.flowLayoutPanel1.Controls.Add(this.bcr2);
            this.flowLayoutPanel1.Controls.Add(this.bcr3);
            this.flowLayoutPanel1.Controls.Add(this.bcr4);
            this.flowLayoutPanel1.Controls.Add(this.bcr5);
            this.flowLayoutPanel1.Controls.Add(this.bcr6);
        }
    }
}
