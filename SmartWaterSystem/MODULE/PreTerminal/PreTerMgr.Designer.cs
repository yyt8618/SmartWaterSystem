namespace SmartWaterSystem
{
    partial class PreTerMgr
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
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl8 = new DevExpress.XtraEditors.GroupControl();
            this.textEdit2 = new DevExpress.XtraEditors.TextEdit();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.textEdit4 = new DevExpress.XtraEditors.TextEdit();
            this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.comboBoxEditPower = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton7 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl8)).BeginInit();
            this.groupControl8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditPower.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl1.Location = new System.Drawing.Point(14, 80);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(772, 400);
            this.gridControl1.TabIndex = 2;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gridView1.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "压力点";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            this.gridColumn1.Width = 70;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "区";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 70;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "安装地点";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
            this.gridColumn3.Width = 20;
            // 
            // groupControl8
            // 
            this.groupControl8.Controls.Add(this.gridControl1);
            this.groupControl8.Controls.Add(this.textEdit2);
            this.groupControl8.Controls.Add(this.textEdit1);
            this.groupControl8.Controls.Add(this.textEdit4);
            this.groupControl8.Controls.Add(this.comboBoxEdit1);
            this.groupControl8.Controls.Add(this.comboBoxEditPower);
            this.groupControl8.Controls.Add(this.labelControl2);
            this.groupControl8.Controls.Add(this.simpleButton7);
            this.groupControl8.Controls.Add(this.simpleButton2);
            this.groupControl8.Controls.Add(this.simpleButton1);
            this.groupControl8.Controls.Add(this.labelControl7);
            this.groupControl8.Controls.Add(this.labelControl5);
            this.groupControl8.Controls.Add(this.labelControl4);
            this.groupControl8.Controls.Add(this.labelControl3);
            this.groupControl8.Controls.Add(this.labelControl1);
            this.groupControl8.Location = new System.Drawing.Point(3, 6);
            this.groupControl8.Name = "groupControl8";
            this.groupControl8.Size = new System.Drawing.Size(791, 485);
            this.groupControl8.TabIndex = 4;
            this.groupControl8.Text = "压力终端配置与管理";
            // 
            // textEdit2
            // 
            this.textEdit2.Location = new System.Drawing.Point(265, 53);
            this.textEdit2.Name = "textEdit2";
            this.textEdit2.Size = new System.Drawing.Size(518, 20);
            this.textEdit2.TabIndex = 109;
            // 
            // textEdit1
            // 
            this.textEdit1.Location = new System.Drawing.Point(248, 28);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(89, 20);
            this.textEdit1.TabIndex = 109;
            // 
            // textEdit4
            // 
            this.textEdit4.Location = new System.Drawing.Point(84, 28);
            this.textEdit4.Name = "textEdit4";
            this.textEdit4.Size = new System.Drawing.Size(89, 20);
            this.textEdit4.TabIndex = 109;
            // 
            // comboBoxEdit1
            // 
            this.comboBoxEdit1.Location = new System.Drawing.Point(167, 53);
            this.comboBoxEdit1.Name = "comboBoxEdit1";
            this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit1.Properties.Items.AddRange(new object[] {
            "停止",
            "启动"});
            this.comboBoxEdit1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboBoxEdit1.Size = new System.Drawing.Size(63, 20);
            this.comboBoxEdit1.TabIndex = 108;
            // 
            // comboBoxEditPower
            // 
            this.comboBoxEditPower.Location = new System.Drawing.Point(84, 54);
            this.comboBoxEditPower.Name = "comboBoxEditPower";
            this.comboBoxEditPower.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEditPower.Properties.Items.AddRange(new object[] {
            "停止",
            "启动"});
            this.comboBoxEditPower.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboBoxEditPower.Size = new System.Drawing.Size(63, 20);
            this.comboBoxEditPower.TabIndex = 108;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(178, 31);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(52, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "设备编号:";
            // 
            // simpleButton7
            // 
            this.simpleButton7.Location = new System.Drawing.Point(715, 25);
            this.simpleButton7.Name = "simpleButton7";
            this.simpleButton7.Size = new System.Drawing.Size(68, 23);
            this.simpleButton7.TabIndex = 107;
            this.simpleButton7.Text = "删除";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(641, 25);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(68, 23);
            this.simpleButton2.TabIndex = 107;
            this.simpleButton2.Text = "修改";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(567, 25);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(68, 23);
            this.simpleButton1.TabIndex = 107;
            this.simpleButton1.Text = "新增";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(236, 57);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(24, 14);
            this.labelControl7.TabIndex = 1;
            this.labelControl7.Text = "详细";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(153, 57);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(12, 14);
            this.labelControl5.TabIndex = 1;
            this.labelControl5.Text = "路";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(72, 57);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(12, 14);
            this.labelControl4.TabIndex = 1;
            this.labelControl4.Text = "区";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(14, 59);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(52, 14);
            this.labelControl3.TabIndex = 1;
            this.labelControl3.Text = "安装地点:";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(14, 31);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(64, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "压力终端ID:";
            // 
            // PreTerMgr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl8);
            this.Name = "PreTerMgr";
            this.Size = new System.Drawing.Size(797, 494);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl8)).EndInit();
            this.groupControl8.ResumeLayout(false);
            this.groupControl8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditPower.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.GroupControl groupControl8;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEditPower;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.TextEdit textEdit4;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit textEdit2;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton simpleButton7;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;

    }
}