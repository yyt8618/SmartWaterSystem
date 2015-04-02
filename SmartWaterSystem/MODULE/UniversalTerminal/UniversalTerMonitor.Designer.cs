using Common;
namespace SmartWaterSystem
{
    partial class UniversalTerMonitor
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
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.gridControl_data = new DevExpress.XtraGrid.GridControl();
            this.advBandedGridView1 = new DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.groupControl6 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_data)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advBandedGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).BeginInit();
            this.groupControl6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.gridControl_data);
            this.groupControl3.Location = new System.Drawing.Point(117, 5);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(680, 483);
            this.groupControl3.TabIndex = 4;
            this.groupControl3.Text = "数据列表";
            // 
            // gridControl_data
            // 
            this.gridControl_data.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_data.Location = new System.Drawing.Point(2, 22);
            this.gridControl_data.MainView = this.advBandedGridView1;
            this.gridControl_data.Name = "gridControl_data";
            this.gridControl_data.Size = new System.Drawing.Size(676, 459);
            this.gridControl_data.TabIndex = 1;
            this.gridControl_data.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.advBandedGridView1});
            // 
            // advBandedGridView1
            // 
            this.advBandedGridView1.GridControl = this.gridControl_data;
            this.advBandedGridView1.Name = "advBandedGridView1";
            this.advBandedGridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.advBandedGridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.advBandedGridView1.OptionsBehavior.ReadOnly = true;
            this.advBandedGridView1.OptionsView.ShowGroupPanel = false;
            this.advBandedGridView1.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.advBandedGridView1_CustomDrawRowIndicator);
            // 
            // treeList1
            // 
            this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList1.Location = new System.Drawing.Point(2, 22);
            this.treeList1.Name = "treeList1";
            this.treeList1.Size = new System.Drawing.Size(105, 459);
            this.treeList1.TabIndex = 5;
            // 
            // groupControl6
            // 
            this.groupControl6.Controls.Add(this.treeList1);
            this.groupControl6.Location = new System.Drawing.Point(4, 5);
            this.groupControl6.Name = "groupControl6";
            this.groupControl6.Size = new System.Drawing.Size(109, 483);
            this.groupControl6.TabIndex = 4;
            this.groupControl6.Text = "列表";
            // 
            // UniversalTerMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl6);
            this.Controls.Add(this.groupControl3);
            this.Name = "UniversalTerMonitor";
            this.Size = new System.Drawing.Size(797, 494);
            this.Load += new System.EventHandler(this.UniversalTerParm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_data)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advBandedGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).EndInit();
            this.groupControl6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraGrid.GridControl gridControl_data;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraEditors.GroupControl groupControl6;
        private DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView advBandedGridView1;

    }
}