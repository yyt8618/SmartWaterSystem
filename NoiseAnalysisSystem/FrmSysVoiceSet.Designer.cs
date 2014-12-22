namespace NoiseAnalysisSystem
{
    partial class FrmSysVoiceSet
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSysVoiceSet));
            this.button1 = new DevExpress.XtraEditors.SimpleButton();
            this.listBoxVoice = new System.Windows.Forms.ListBox();
            this.btnPlay = new DevExpress.XtraEditors.SimpleButton();
            this.btnSelectVoice = new DevExpress.XtraEditors.SimpleButton();
            this.label58 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.lblLeakVoice = new System.Windows.Forms.Label();
            this.lblRecordVoice = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(142, 257);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(66, 23);
            this.button1.TabIndex = 79;
            this.button1.Text = "设置";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBoxVoice
            // 
            this.listBoxVoice.FormattingEnabled = true;
            this.listBoxVoice.ItemHeight = 14;
            this.listBoxVoice.Location = new System.Drawing.Point(28, 16);
            this.listBoxVoice.Name = "listBoxVoice";
            this.listBoxVoice.Size = new System.Drawing.Size(180, 116);
            this.listBoxVoice.TabIndex = 77;
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(142, 146);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(66, 23);
            this.btnPlay.TabIndex = 75;
            this.btnPlay.Text = "试听";
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnSelectVoice
            // 
            this.btnSelectVoice.Location = new System.Drawing.Point(142, 197);
            this.btnSelectVoice.Name = "btnSelectVoice";
            this.btnSelectVoice.Size = new System.Drawing.Size(66, 23);
            this.btnSelectVoice.TabIndex = 72;
            this.btnSelectVoice.Text = "设置";
            this.btnSelectVoice.Click += new System.EventHandler(this.btnSelectVoice_Click);
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(35, 176);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(67, 14);
            this.label58.TabIndex = 44;
            this.label58.Text = "漏水报警音";
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(35, 238);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(79, 14);
            this.label57.TabIndex = 43;
            this.label57.Text = "记录仪提示音";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "*.wav";
            this.openFileDialog.Filter = "WAV音频文件|*.wav";
            this.openFileDialog.Title = "选择声音文件";
            // 
            // lblLeakVoice
            // 
            this.lblLeakVoice.AutoSize = true;
            this.lblLeakVoice.Location = new System.Drawing.Point(35, 203);
            this.lblLeakVoice.Name = "lblLeakVoice";
            this.lblLeakVoice.Size = new System.Drawing.Size(0, 14);
            this.lblLeakVoice.TabIndex = 80;
            // 
            // lblRecordVoice
            // 
            this.lblRecordVoice.AutoSize = true;
            this.lblRecordVoice.Location = new System.Drawing.Point(35, 265);
            this.lblRecordVoice.Name = "lblRecordVoice";
            this.lblRecordVoice.Size = new System.Drawing.Size(0, 14);
            this.lblRecordVoice.TabIndex = 81;
            // 
            // FrmSysVoiceSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(236, 304);
            this.Controls.Add(this.lblRecordVoice);
            this.Controls.Add(this.lblLeakVoice);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listBoxVoice);
            this.Controls.Add(this.label57);
            this.Controls.Add(this.label58);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnSelectVoice);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmSysVoiceSet";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "系统声音设置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnPlay;
        private DevExpress.XtraEditors.SimpleButton btnSelectVoice;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ListBox listBoxVoice;
        private DevExpress.XtraEditors.SimpleButton button1;
        private System.Windows.Forms.Label lblLeakVoice;
        private System.Windows.Forms.Label lblRecordVoice;

    }
}