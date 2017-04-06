namespace SmartWaterSystem
{
    partial class FrmOffset
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOffset));
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txt_id = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_tertype = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txt_funcode = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txt_offsetvalue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_id)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_tertype)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_funcode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_offsetvalue)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(243, 318);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(72, 33);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "设置";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.txt_id,
            this.cb_tertype,
            this.txt_funcode,
            this.txt_offsetvalue});
            this.gridControl1.Size = new System.Drawing.Size(557, 302);
            this.gridControl1.TabIndex = 7;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn4});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.IndicatorWidth = 10;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView1.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsFind.AllowFindPanel = false;
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridView1_KeyDown);
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "编号";
            this.gridColumn3.ColumnEdit = this.txt_id;
            this.gridColumn3.FieldName = "Id";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn3.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn3.OptionsColumn.AllowMove = false;
            this.gridColumn3.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn3.OptionsColumn.Printable = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn3.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn3.OptionsFilter.AllowFilter = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            this.gridColumn3.Width = 84;
            // 
            // txt_id
            // 
            this.txt_id.AutoHeight = false;
            this.txt_id.Mask.EditMask = "[0-9]{1,8}";
            this.txt_id.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txt_id.Name = "txt_id";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "设备类型";
            this.gridColumn1.ColumnEdit = this.cb_tertype;
            this.gridColumn1.FieldName = "TerType";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn1.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn1.OptionsColumn.AllowMove = false;
            this.gridColumn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn1.OptionsColumn.Printable = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn1.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn1.OptionsFilter.AllowFilter = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            this.gridColumn1.Width = 150;
            // 
            // cb_tertype
            // 
            this.cb_tertype.AutoHeight = false;
            this.cb_tertype.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_tertype.DropDownRows = 10;
            this.cb_tertype.Name = "cb_tertype";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "功能码";
            this.gridColumn2.ColumnEdit = this.txt_funcode;
            this.gridColumn2.FieldName = "FunCode";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn2.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn2.OptionsColumn.AllowMove = false;
            this.gridColumn2.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn2.OptionsColumn.Printable = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn2.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn2.OptionsFilter.AllowFilter = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 150;
            // 
            // txt_funcode
            // 
            this.txt_funcode.AutoHeight = false;
            this.txt_funcode.Name = "txt_funcode";
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "纠偏值";
            this.gridColumn4.ColumnEdit = this.txt_offsetvalue;
            this.gridColumn4.FieldName = "OffsetValue";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 155;
            // 
            // txt_offsetvalue
            // 
            this.txt_offsetvalue.AutoHeight = false;
            this.txt_offsetvalue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txt_offsetvalue.Name = "txt_offsetvalue";
            // 
            // FrmOffset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(557, 363);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.btnConfirm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmOffset";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "纠偏设置";
            this.Load += new System.EventHandler(this.FrmOffset_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_id)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_tertype)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_funcode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_offsetvalue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txt_offsetvalue;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txt_funcode;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_tertype;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txt_id;
    }
}