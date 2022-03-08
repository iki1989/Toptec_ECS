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
    public partial class UserControlInkjets : UserControl
    {
        private UserControlInkjet inkjet1;
        private UserControlInkjet inkjet2;

        public UserControlInkjets()
        {
            InitializeComponent();

            this.inkjet1 = new UserControlInkjet("inkjet#1", 4000);
            this.inkjet2 = new UserControlInkjet("inkjet#2", 4001);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.flowLayoutPanel1.Controls.Add(this.inkjet1);
            this.flowLayoutPanel1.Controls.Add(this.inkjet2);
        }
    }
}
