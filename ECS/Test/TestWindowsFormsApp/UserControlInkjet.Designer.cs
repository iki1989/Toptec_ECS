
namespace TestWindowsFormsApp
{
    partial class UserControlInkjet
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
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonEnablPrintCompleteAcknowledgementSend = new System.Windows.Forms.Button();
            this.buttonWriteAutoDataRecordSend = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.buttonReadAutoDataStatusSend = new System.Windows.Forms.Button();
            this.buttonClearAutoDataQueueSend = new System.Windows.Forms.Button();
            this.buttonReadInkLevelSend = new System.Windows.Forms.Button();
            this.buttonGetAutoDataStringSend = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(5, 286);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 13;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(86, 286);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 14;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonEnablPrintCompleteAcknowledgementSend
            // 
            this.buttonEnablPrintCompleteAcknowledgementSend.Location = new System.Drawing.Point(5, 315);
            this.buttonEnablPrintCompleteAcknowledgementSend.Name = "buttonEnablPrintCompleteAcknowledgementSend";
            this.buttonEnablPrintCompleteAcknowledgementSend.Size = new System.Drawing.Size(281, 23);
            this.buttonEnablPrintCompleteAcknowledgementSend.TabIndex = 15;
            this.buttonEnablPrintCompleteAcknowledgementSend.Text = "EnablPrintCompleteAcknowledgementSend";
            this.buttonEnablPrintCompleteAcknowledgementSend.UseVisualStyleBackColor = true;
            this.buttonEnablPrintCompleteAcknowledgementSend.Click += new System.EventHandler(this.buttonEnablPrintCompleteAcknowledgementSend_Click);
            // 
            // buttonWriteAutoDataRecordSend
            // 
            this.buttonWriteAutoDataRecordSend.Location = new System.Drawing.Point(5, 412);
            this.buttonWriteAutoDataRecordSend.Name = "buttonWriteAutoDataRecordSend";
            this.buttonWriteAutoDataRecordSend.Size = new System.Drawing.Size(281, 23);
            this.buttonWriteAutoDataRecordSend.TabIndex = 17;
            this.buttonWriteAutoDataRecordSend.Text = "WriteAutoDataRecordSend";
            this.buttonWriteAutoDataRecordSend.UseVisualStyleBackColor = true;
            this.buttonWriteAutoDataRecordSend.Click += new System.EventHandler(this.buttonWriteAutoDataRecordSend_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(291, 280);
            this.propertyGrid1.TabIndex = 19;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Right;
            this.richTextBoxLog.Location = new System.Drawing.Point(317, 0);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(406, 585);
            this.richTextBoxLog.TabIndex = 20;
            this.richTextBoxLog.Text = "";
            // 
            // buttonReadAutoDataStatusSend
            // 
            this.buttonReadAutoDataStatusSend.Location = new System.Drawing.Point(5, 344);
            this.buttonReadAutoDataStatusSend.Name = "buttonReadAutoDataStatusSend";
            this.buttonReadAutoDataStatusSend.Size = new System.Drawing.Size(281, 23);
            this.buttonReadAutoDataStatusSend.TabIndex = 21;
            this.buttonReadAutoDataStatusSend.Text = "ReadAutoDataStatusSend";
            this.buttonReadAutoDataStatusSend.UseVisualStyleBackColor = true;
            this.buttonReadAutoDataStatusSend.Click += new System.EventHandler(this.buttonReadAutoDataStatusSend_Click);
            // 
            // buttonClearAutoDataQueueSend
            // 
            this.buttonClearAutoDataQueueSend.Location = new System.Drawing.Point(5, 441);
            this.buttonClearAutoDataQueueSend.Name = "buttonClearAutoDataQueueSend";
            this.buttonClearAutoDataQueueSend.Size = new System.Drawing.Size(281, 23);
            this.buttonClearAutoDataQueueSend.TabIndex = 22;
            this.buttonClearAutoDataQueueSend.Text = "ClearAutoDataQueueSend";
            this.buttonClearAutoDataQueueSend.UseVisualStyleBackColor = true;
            this.buttonClearAutoDataQueueSend.Click += new System.EventHandler(this.buttonClearAutoDataQueueSend_Click);
            // 
            // buttonReadInkLevelSend
            // 
            this.buttonReadInkLevelSend.Location = new System.Drawing.Point(5, 470);
            this.buttonReadInkLevelSend.Name = "buttonReadInkLevelSend";
            this.buttonReadInkLevelSend.Size = new System.Drawing.Size(281, 23);
            this.buttonReadInkLevelSend.TabIndex = 23;
            this.buttonReadInkLevelSend.Text = "ReadInkLevelSend";
            this.buttonReadInkLevelSend.UseVisualStyleBackColor = true;
            this.buttonReadInkLevelSend.Click += new System.EventHandler(this.buttonReadInkLevelSend_Click);
            // 
            // buttonGetAutoDataStringSend
            // 
            this.buttonGetAutoDataStringSend.Location = new System.Drawing.Point(5, 383);
            this.buttonGetAutoDataStringSend.Name = "buttonGetAutoDataStringSend";
            this.buttonGetAutoDataStringSend.Size = new System.Drawing.Size(281, 23);
            this.buttonGetAutoDataStringSend.TabIndex = 24;
            this.buttonGetAutoDataStringSend.Text = "GetAutoDataStringSend";
            this.buttonGetAutoDataStringSend.UseVisualStyleBackColor = true;
            this.buttonGetAutoDataStringSend.Click += new System.EventHandler(this.buttonGetAutoDataStringSend_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(218, 286);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 27;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // UserControlInkjet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonGetAutoDataStringSend);
            this.Controls.Add(this.buttonReadInkLevelSend);
            this.Controls.Add(this.buttonClearAutoDataQueueSend);
            this.Controls.Add(this.buttonReadAutoDataStatusSend);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.buttonWriteAutoDataRecordSend);
            this.Controls.Add(this.buttonEnablPrintCompleteAcknowledgementSend);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.Name = "UserControlInkjet";
            this.Size = new System.Drawing.Size(723, 585);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonEnablPrintCompleteAcknowledgementSend;
        private System.Windows.Forms.Button buttonWriteAutoDataRecordSend;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Button buttonReadAutoDataStatusSend;
        private System.Windows.Forms.Button buttonClearAutoDataQueueSend;
        private System.Windows.Forms.Button buttonReadInkLevelSend;
        private System.Windows.Forms.Button buttonGetAutoDataStringSend;
        private System.Windows.Forms.Button buttonClear;
    }
}
