namespace SmartWaterSystem
{
    partial class FrmParamViewer
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
            this.listView = new System.Windows.Forms.ListView();
            this.Key = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Value = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.OKPB = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Key,
            this.Value});
            this.listView.GridLines = true;
            this.listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView.Location = new System.Drawing.Point(8, 6);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(209, 77);
            this.listView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // Key
            // 
            this.Key.Text = "参数";
            this.Key.Width = 80;
            // 
            // Value
            // 
            this.Value.Text = "值";
            this.Value.Width = 120;
            // 
            // OKPB
            // 
            this.OKPB.Location = new System.Drawing.Point(76, 89);
            this.OKPB.Name = "OKPB";
            this.OKPB.Size = new System.Drawing.Size(72, 23);
            this.OKPB.TabIndex = 1;
            this.OKPB.Text = "确定";
            this.OKPB.Click += new System.EventHandler(this.OKPB_Click);
            // 
            // FrmParamViewer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(225, 118);
            this.Controls.Add(this.OKPB);
            this.Controls.Add(this.listView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.Name = "FrmParamViewer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "参数查看";
            this.ResumeLayout(false);

        }

        #endregion

		private DevExpress.XtraEditors.SimpleButton OKPB;
		private System.Windows.Forms.ColumnHeader Key;
		private System.Windows.Forms.ColumnHeader Value;
		private System.Windows.Forms.ListView listView;
    }
}