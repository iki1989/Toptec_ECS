
namespace TestWindowsFormsApp
{
    partial class UserControlZebraZt411
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlZebraZt411));
            this.Print = new System.Windows.Forms.Button();
            this.textBoxText = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonHostStatus = new System.Windows.Forms.Button();
            this.propertyGridState = new System.Windows.Forms.PropertyGrid();
            this.buttonCancelAll = new System.Windows.Forms.Button();
            this.buttonReprintAfterError = new System.Windows.Forms.Button();
            this.richTextBoxZplPrint = new System.Windows.Forms.RichTextBox();
            this.buttonZplPrint = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonNoWeightPrint = new System.Windows.Forms.Button();
            this.buttonDuplicatePrint = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Print
            // 
            this.Print.Location = new System.Drawing.Point(216, 315);
            this.Print.Name = "Print";
            this.Print.Size = new System.Drawing.Size(75, 23);
            this.Print.TabIndex = 7;
            this.Print.Text = "Print";
            this.Print.UseVisualStyleBackColor = true;
            this.Print.Click += new System.EventHandler(this.Print_Click);
            // 
            // textBoxText
            // 
            this.textBoxText.Location = new System.Drawing.Point(3, 315);
            this.textBoxText.Multiline = true;
            this.textBoxText.Name = "textBoxText";
            this.textBoxText.Size = new System.Drawing.Size(196, 21);
            this.textBoxText.TabIndex = 8;
            this.textBoxText.Text = "1G20005310";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(0, 286);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 10;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(81, 286);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 11;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonHostStatus
            // 
            this.buttonHostStatus.Location = new System.Drawing.Point(11, 546);
            this.buttonHostStatus.Name = "buttonHostStatus";
            this.buttonHostStatus.Size = new System.Drawing.Size(75, 23);
            this.buttonHostStatus.TabIndex = 12;
            this.buttonHostStatus.Text = "HostStatus";
            this.buttonHostStatus.UseVisualStyleBackColor = true;
            this.buttonHostStatus.Click += new System.EventHandler(this.buttonHostStatus_Click);
            // 
            // propertyGridState
            // 
            this.propertyGridState.HelpVisible = false;
            this.propertyGridState.Location = new System.Drawing.Point(309, 286);
            this.propertyGridState.Name = "propertyGridState";
            this.propertyGridState.Size = new System.Drawing.Size(411, 296);
            this.propertyGridState.TabIndex = 13;
            this.propertyGridState.ToolbarVisible = false;
            // 
            // buttonCancelAll
            // 
            this.buttonCancelAll.Location = new System.Drawing.Point(216, 546);
            this.buttonCancelAll.Name = "buttonCancelAll";
            this.buttonCancelAll.Size = new System.Drawing.Size(75, 23);
            this.buttonCancelAll.TabIndex = 14;
            this.buttonCancelAll.Text = "CancelAll";
            this.buttonCancelAll.UseVisualStyleBackColor = true;
            this.buttonCancelAll.Click += new System.EventHandler(this.buttonCancelAll_Click);
            // 
            // buttonReprintAfterError
            // 
            this.buttonReprintAfterError.Location = new System.Drawing.Point(92, 546);
            this.buttonReprintAfterError.Name = "buttonReprintAfterError";
            this.buttonReprintAfterError.Size = new System.Drawing.Size(118, 23);
            this.buttonReprintAfterError.TabIndex = 15;
            this.buttonReprintAfterError.Text = "ReprintAfterError";
            this.buttonReprintAfterError.UseVisualStyleBackColor = true;
            this.buttonReprintAfterError.Click += new System.EventHandler(this.buttonReprintAfterError_Click);
            // 
            // richTextBoxZplPrint
            // 
            this.richTextBoxZplPrint.Location = new System.Drawing.Point(3, 342);
            this.richTextBoxZplPrint.Name = "richTextBoxZplPrint";
            this.richTextBoxZplPrint.Size = new System.Drawing.Size(207, 198);
            this.richTextBoxZplPrint.TabIndex = 16;
            this.richTextBoxZplPrint.Text = resources.GetString("richTextBoxZplPrint.Text");
            // 
            // buttonZplPrint
            // 
            this.buttonZplPrint.Location = new System.Drawing.Point(216, 344);
            this.buttonZplPrint.Name = "buttonZplPrint";
            this.buttonZplPrint.Size = new System.Drawing.Size(75, 23);
            this.buttonZplPrint.TabIndex = 17;
            this.buttonZplPrint.Text = "ZplPrint";
            this.buttonZplPrint.UseVisualStyleBackColor = true;
            this.buttonZplPrint.Click += new System.EventHandler(this.buttonZplPrint_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(291, 280);
            this.propertyGrid1.TabIndex = 20;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Location = new System.Drawing.Point(309, 0);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(414, 280);
            this.richTextBoxLog.TabIndex = 21;
            this.richTextBoxLog.Text = "";
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(216, 286);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 22;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonNoWeightPrint
            // 
            this.buttonNoWeightPrint.Location = new System.Drawing.Point(216, 383);
            this.buttonNoWeightPrint.Name = "buttonNoWeightPrint";
            this.buttonNoWeightPrint.Size = new System.Drawing.Size(87, 23);
            this.buttonNoWeightPrint.TabIndex = 23;
            this.buttonNoWeightPrint.Text = "NoWeightPrint";
            this.buttonNoWeightPrint.UseVisualStyleBackColor = true;
            this.buttonNoWeightPrint.Click += new System.EventHandler(this.buttonNoWeightPrint_Click);
            // 
            // buttonDuplicatePrint
            // 
            this.buttonDuplicatePrint.Location = new System.Drawing.Point(216, 412);
            this.buttonDuplicatePrint.Name = "buttonDuplicatePrint";
            this.buttonDuplicatePrint.Size = new System.Drawing.Size(87, 23);
            this.buttonDuplicatePrint.TabIndex = 24;
            this.buttonDuplicatePrint.Text = "DuplicatePrint";
            this.buttonDuplicatePrint.UseVisualStyleBackColor = true;
            this.buttonDuplicatePrint.Click += new System.EventHandler(this.buttonDuplicatePrint_Click);
            // 
            // UserControlZebraZt411
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonDuplicatePrint);
            this.Controls.Add(this.buttonNoWeightPrint);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.buttonZplPrint);
            this.Controls.Add(this.richTextBoxZplPrint);
            this.Controls.Add(this.buttonReprintAfterError);
            this.Controls.Add(this.buttonCancelAll);
            this.Controls.Add(this.propertyGridState);
            this.Controls.Add(this.buttonHostStatus);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.textBoxText);
            this.Controls.Add(this.Print);
            this.Name = "UserControlZebraZt411";
            this.Size = new System.Drawing.Size(723, 585);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Print;
        private System.Windows.Forms.TextBox textBoxText;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonHostStatus;
        private System.Windows.Forms.PropertyGrid propertyGridState;
        private System.Windows.Forms.Button buttonCancelAll;
        private System.Windows.Forms.Button buttonReprintAfterError;
        private System.Windows.Forms.RichTextBox richTextBoxZplPrint;
        private System.Windows.Forms.Button buttonZplPrint;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonNoWeightPrint;
        private System.Windows.Forms.Button buttonDuplicatePrint;
    }
}
