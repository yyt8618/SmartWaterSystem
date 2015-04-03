namespace SmartWaterSystem
{
    partial class FrmGPRSConsole
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGPRSConsole));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.txtControl = new DevExpress.XtraEditors.MemoEdit();
            this.timerCtrl = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txtControl.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "*.wav";
            this.openFileDialog.Filter = "WAV音频文件|*.wav";
            this.openFileDialog.Title = "选择声音文件";
            // 
            // txtControl
            // 
            this.txtControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtControl.Location = new System.Drawing.Point(0, 0);
            this.txtControl.Name = "txtControl";
            this.txtControl.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.txtControl.Properties.Appearance.ForeColor = System.Drawing.Color.Lime;
            this.txtControl.Properties.Appearance.Options.UseBackColor = true;
            this.txtControl.Properties.Appearance.Options.UseForeColor = true;
            this.txtControl.Properties.ReadOnly = true;
            this.txtControl.ShowToolTips = false;
            this.txtControl.Size = new System.Drawing.Size(766, 426);
            this.txtControl.TabIndex = 0;
            this.txtControl.TabStop = false;
            // 
            // FrmGPRSConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(766, 426);
            this.Controls.Add(this.txtControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmGPRSConsole";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "远传监控";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmGPRSConsole_FormClosing);
            this.Load += new System.EventHandler(this.FrmGPRSConsole_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtControl.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private DevExpress.XtraEditors.MemoEdit txtControl;
        private System.Windows.Forms.Timer timerCtrl;

    }
}