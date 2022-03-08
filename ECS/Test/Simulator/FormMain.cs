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
   
    public partial class FormMain : Form
    {
        UserControlWebService userControlWebService = new UserControlWebService("127.0.0.1", 8081);
        UserControlRESTfulWcs userControlRESTfulWcs = new UserControlRESTfulWcs();
        UserControlRESTfulRicp userControlRESTfulRicp = new UserControlRESTfulRicp();
        UserControlInkjets userControlInkjects = new UserControlInkjets();
        UserControlDynamicScale userControlDynamicScale = new UserControlDynamicScale("DynamicScale", 1749);
        UserControlBcrs UserControlBcrs = new UserControlBcrs();
        UserControlZebras userControlZebras = new UserControlZebras();
        UserControlTopBcr userControlTopBcr = new UserControlTopBcr("127.0.0.1", 8004);

        public FormMain()
        {
            InitializeComponent();

            this.tabPage1.Controls.Add(userControlWebService);
            this.tabPage2.Controls.Add(userControlRESTfulWcs);
            this.tabPage3.Controls.Add(userControlRESTfulRicp);
            this.tabPage4.Controls.Add(userControlInkjects);
            this.tabPage5.Controls.Add(userControlDynamicScale);
            this.tabPage6.Controls.Add(UserControlBcrs);
            this.tabPage7.Controls.Add(userControlZebras);
            this.tabPage8.Controls.Add(userControlTopBcr);

            this.tabPage1.Text = "WebService";
            this.tabPage2.Text = "RESTfulWCS";
            this.tabPage3.Text = "RESTfulRicp";
            this.tabPage4.Text = "Inkjects";
            this.tabPage5.Text = "DynamicScale";
            this.tabPage6.Text = "BCRs";
            this.tabPage7.Text = "Zebras";
            this.tabPage8.Text = "TopBcr";

            this.userControlWebService.Dock = DockStyle.Fill;
            this.userControlRESTfulRicp.Dock = DockStyle.Fill;
            this.userControlInkjects.Dock = DockStyle.Fill;
            this.userControlDynamicScale.Dock = DockStyle.Fill;
            this.UserControlBcrs.Dock = DockStyle.Fill;
            this.UserControlBcrs.Dock = DockStyle.Fill;
            this.userControlZebras.Dock = DockStyle.Fill;
            this.userControlTopBcr.Dock = DockStyle.Fill;

            this.tabControl1.SelectedTab = this.tabPage8;
        }
    }
}
