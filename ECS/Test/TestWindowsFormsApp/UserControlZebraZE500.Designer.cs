
namespace TestWindowsFormsApp
{
    partial class UserControlZebraZE500
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlZebraZE500));
            this.STATE = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.propertyGridState = new System.Windows.Forms.PropertyGrid();
            this.richTextBoxZplPrint = new System.Windows.Forms.RichTextBox();
            this.buttonZplPrint = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonBufferClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // STATE
            // 
            this.STATE.Location = new System.Drawing.Point(216, 342);
            this.STATE.Name = "STATE";
            this.STATE.Size = new System.Drawing.Size(75, 23);
            this.STATE.TabIndex = 7;
            this.STATE.Text = "STATE";
            this.STATE.UseVisualStyleBackColor = true;
            this.STATE.Click += new System.EventHandler(this.State_Click);
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
            // propertyGridState
            // 
            this.propertyGridState.HelpVisible = false;
            this.propertyGridState.Location = new System.Drawing.Point(309, 286);
            this.propertyGridState.Name = "propertyGridState";
            this.propertyGridState.Size = new System.Drawing.Size(411, 296);
            this.propertyGridState.TabIndex = 13;
            this.propertyGridState.ToolbarVisible = false;
            // 
            // richTextBoxZplPrint
            // 
            this.richTextBoxZplPrint.Location = new System.Drawing.Point(3, 315);
            this.richTextBoxZplPrint.Name = "richTextBoxZplPrint";
            this.richTextBoxZplPrint.Size = new System.Drawing.Size(207, 198);
            this.richTextBoxZplPrint.TabIndex = 16;
            this.richTextBoxZplPrint.Text = resources.GetString("richTextBoxZplPrint.Text");
            // 
            // buttonZplPrint
            // 
            this.buttonZplPrint.Location = new System.Drawing.Point(216, 376);
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
            // buttonBufferClear
            // 
            this.buttonBufferClear.Location = new System.Drawing.Point(216, 405);
            this.buttonBufferClear.Name = "buttonBufferClear";
            this.buttonBufferClear.Size = new System.Drawing.Size(75, 23);
            this.buttonBufferClear.TabIndex = 23;
            this.buttonBufferClear.Text = "BufferClear";
            this.buttonBufferClear.UseVisualStyleBackColor = true;
            this.buttonBufferClear.Click += new System.EventHandler(this.buttonBufferClear_Click);
            // 
            // UserControlZebraZE500
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonBufferClear);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.buttonZplPrint);
            this.Controls.Add(this.richTextBoxZplPrint);
            this.Controls.Add(this.propertyGridState);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.STATE);
            this.Name = "UserControlZebraZE500";
            this.Size = new System.Drawing.Size(723, 585);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button STATE;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.PropertyGrid propertyGridState;
        private System.Windows.Forms.RichTextBox richTextBoxZplPrint;
        private System.Windows.Forms.Button buttonZplPrint;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonBufferClear;
    }
}
