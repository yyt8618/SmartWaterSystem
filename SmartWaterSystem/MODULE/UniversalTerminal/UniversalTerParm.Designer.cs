using Common;
namespace SmartWaterSystem
{
    partial class UniversalTerParm
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.txtCellPhone = new DevExpress.XtraEditors.TextEdit();
            this.txtTime = new DevExpress.XtraEditors.TextEdit();
            this.ceColConfig = new DevExpress.XtraEditors.CheckEdit();
            this.txtPort = new DevExpress.XtraEditors.TextEdit();
            this.groupControl7 = new DevExpress.XtraEditors.GroupControl();
            this.cePluseState = new DevExpress.XtraEditors.CheckEdit();
            this.cemodbusprotocolstatus = new DevExpress.XtraEditors.CheckEdit();
            this.ceRS485State = new DevExpress.XtraEditors.CheckEdit();
            this.ceSimulate2State = new DevExpress.XtraEditors.CheckEdit();
            this.ceSimulate1State = new DevExpress.XtraEditors.CheckEdit();
            this.cbComType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ceComType = new DevExpress.XtraEditors.CheckEdit();
            this.ceTime = new DevExpress.XtraEditors.CheckEdit();
            this.txtID = new DevExpress.XtraEditors.TextEdit();
            this.ceCellPhone = new DevExpress.XtraEditors.CheckEdit();
            this.txtNum4 = new DevExpress.XtraEditors.TextEdit();
            this.cePort = new DevExpress.XtraEditors.CheckEdit();
            this.ceID = new DevExpress.XtraEditors.CheckEdit();
            this.txtNum3 = new DevExpress.XtraEditors.TextEdit();
            this.txtNum2 = new DevExpress.XtraEditors.TextEdit();
            this.txtNum1 = new DevExpress.XtraEditors.TextEdit();
            this.ceIP = new DevExpress.XtraEditors.CheckEdit();
            this.ceModbusExeFlag = new DevExpress.XtraEditors.CheckEdit();
            this.ceCollectRS485 = new DevExpress.XtraEditors.CheckEdit();
            this.ceCollectPluse = new DevExpress.XtraEditors.CheckEdit();
            this.ceCollectSimulate = new DevExpress.XtraEditors.CheckEdit();
            this.btnEnableCollect = new DevExpress.XtraEditors.SimpleButton();
            this.btnCheckingTime = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
            this.gridControl_485protocol = new DevExpress.XtraGrid.GridControl();
            this.gridView_485protocol = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_485protocol_baud = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txt_485protocol_ID = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txt_485protocol_funcode = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txt_485protocol_regbeginaddr = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txt_485protocol_regcount = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.btnReadParm = new DevExpress.XtraEditors.SimpleButton();
            this.btnSetParm = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.btnCalibrationSimualte2 = new DevExpress.XtraEditors.SimpleButton();
            this.btnCalibrationSimualte1 = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl_Simulate = new DevExpress.XtraGrid.GridControl();
            this.gridView_Simulate = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_sim_starttime = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_sim_coltime1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_sim_coltime2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_sim_sendtime = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.gridControl_RS485 = new DevExpress.XtraGrid.GridControl();
            this.gridView_RS485 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_RS485_starttime = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_RS485_coltime = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_RS485_sendtime = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.groupControl5 = new DevExpress.XtraEditors.GroupControl();
            this.gridControl_Pluse = new DevExpress.XtraGrid.GridControl();
            this.gridView_Pluse = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_pluse_starttime = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_pluse_coltime = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn17 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_pluse_sendtime = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.btnReset = new DevExpress.XtraEditors.SimpleButton();
            this.SwitchComunication = new DevExpress.XtraEditors.ToggleSwitch();
            this.btnSetPluseBasic = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCellPhone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceColConfig.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl7)).BeginInit();
            this.groupControl7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cePluseState.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cemodbusprotocolstatus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceRS485State.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSimulate2State.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSimulate1State.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbComType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceComType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceCellPhone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNum4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePort.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNum3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNum2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNum1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceIP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceModbusExeFlag.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceCollectRS485.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceCollectPluse.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceCollectSimulate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
            this.groupControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_485protocol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_485protocol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_485protocol_baud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_485protocol_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_485protocol_funcode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_485protocol_regbeginaddr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_485protocol_regcount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_Simulate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_Simulate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_sim_starttime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_sim_coltime1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_sim_coltime2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_sim_sendtime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_RS485)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_RS485)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_RS485_starttime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_RS485_coltime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_RS485_sendtime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).BeginInit();
            this.groupControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_Pluse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_Pluse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_pluse_starttime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_pluse_coltime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_pluse_sendtime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SwitchComunication.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.txtCellPhone);
            this.groupControl1.Controls.Add(this.txtTime);
            this.groupControl1.Controls.Add(this.ceColConfig);
            this.groupControl1.Controls.Add(this.txtPort);
            this.groupControl1.Controls.Add(this.groupControl7);
            this.groupControl1.Controls.Add(this.cbComType);
            this.groupControl1.Controls.Add(this.ceComType);
            this.groupControl1.Controls.Add(this.ceTime);
            this.groupControl1.Controls.Add(this.txtID);
            this.groupControl1.Controls.Add(this.ceCellPhone);
            this.groupControl1.Controls.Add(this.txtNum4);
            this.groupControl1.Controls.Add(this.cePort);
            this.groupControl1.Controls.Add(this.ceID);
            this.groupControl1.Controls.Add(this.txtNum3);
            this.groupControl1.Controls.Add(this.txtNum2);
            this.groupControl1.Controls.Add(this.txtNum1);
            this.groupControl1.Controls.Add(this.ceIP);
            this.groupControl1.Location = new System.Drawing.Point(6, -1);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(791, 80);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "基本信息参数";
            // 
            // txtCellPhone
            // 
            this.txtCellPhone.EditValue = "";
            this.txtCellPhone.Location = new System.Drawing.Point(396, 25);
            this.txtCellPhone.Name = "txtCellPhone";
            this.txtCellPhone.Size = new System.Drawing.Size(92, 20);
            this.txtCellPhone.TabIndex = 5;
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(215, 24);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(120, 20);
            this.txtTime.TabIndex = 3;
            // 
            // ceColConfig
            // 
            this.ceColConfig.Location = new System.Drawing.Point(460, 53);
            this.ceColConfig.Name = "ceColConfig";
            this.ceColConfig.Properties.Caption = "采集功能配置";
            this.ceColConfig.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.ceColConfig.Properties.LookAndFeel.TouchUIMode = DevExpress.LookAndFeel.TouchUIMode.False;
            this.ceColConfig.Size = new System.Drawing.Size(90, 19);
            this.ceColConfig.TabIndex = 15;
            this.ceColConfig.CheckedChanged += new System.EventHandler(this.ceColConfig_CheckedChanged);
            // 
            // txtPort
            // 
            this.txtPort.EditValue = "";
            this.txtPort.Location = new System.Drawing.Point(396, 53);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(58, 20);
            this.txtPort.TabIndex = 14;
            // 
            // groupControl7
            // 
            this.groupControl7.Controls.Add(this.cePluseState);
            this.groupControl7.Controls.Add(this.cemodbusprotocolstatus);
            this.groupControl7.Controls.Add(this.ceRS485State);
            this.groupControl7.Controls.Add(this.ceSimulate2State);
            this.groupControl7.Controls.Add(this.ceSimulate1State);
            this.groupControl7.Location = new System.Drawing.Point(556, 24);
            this.groupControl7.Name = "groupControl7";
            this.groupControl7.ShowCaption = false;
            this.groupControl7.Size = new System.Drawing.Size(230, 53);
            this.groupControl7.TabIndex = 18;
            // 
            // cePluseState
            // 
            this.cePluseState.Enabled = false;
            this.cePluseState.Location = new System.Drawing.Point(101, 5);
            this.cePluseState.Name = "cePluseState";
            this.cePluseState.Properties.Caption = "脉冲量";
            this.cePluseState.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.cePluseState.Properties.LookAndFeel.TouchUIMode = DevExpress.LookAndFeel.TouchUIMode.False;
            this.cePluseState.Size = new System.Drawing.Size(60, 19);
            this.cePluseState.TabIndex = 1;
            // 
            // cemodbusprotocolstatus
            // 
            this.cemodbusprotocolstatus.Enabled = false;
            this.cemodbusprotocolstatus.Location = new System.Drawing.Point(5, 5);
            this.cemodbusprotocolstatus.Name = "cemodbusprotocolstatus";
            this.cemodbusprotocolstatus.Properties.Caption = "modbus协议";
            this.cemodbusprotocolstatus.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.cemodbusprotocolstatus.Properties.LookAndFeel.TouchUIMode = DevExpress.LookAndFeel.TouchUIMode.False;
            this.cemodbusprotocolstatus.Size = new System.Drawing.Size(90, 19);
            this.cemodbusprotocolstatus.TabIndex = 0;
            // 
            // ceRS485State
            // 
            this.ceRS485State.Enabled = false;
            this.ceRS485State.Location = new System.Drawing.Point(162, 5);
            this.ceRS485State.Name = "ceRS485State";
            this.ceRS485State.Properties.Caption = "RS485";
            this.ceRS485State.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.ceRS485State.Properties.LookAndFeel.TouchUIMode = DevExpress.LookAndFeel.TouchUIMode.False;
            this.ceRS485State.Size = new System.Drawing.Size(72, 19);
            this.ceRS485State.TabIndex = 2;
            // 
            // ceSimulate2State
            // 
            this.ceSimulate2State.Enabled = false;
            this.ceSimulate2State.Location = new System.Drawing.Point(101, 30);
            this.ceSimulate2State.Name = "ceSimulate2State";
            this.ceSimulate2State.Properties.Caption = "第2路模拟量";
            this.ceSimulate2State.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.ceSimulate2State.Properties.LookAndFeel.TouchUIMode = DevExpress.LookAndFeel.TouchUIMode.False;
            this.ceSimulate2State.Size = new System.Drawing.Size(90, 19);
            this.ceSimulate2State.TabIndex = 4;
            // 
            // ceSimulate1State
            // 
            this.ceSimulate1State.Enabled = false;
            this.ceSimulate1State.Location = new System.Drawing.Point(5, 30);
            this.ceSimulate1State.Name = "ceSimulate1State";
            this.ceSimulate1State.Properties.Caption = "第1路模拟量";
            this.ceSimulate1State.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.ceSimulate1State.Properties.LookAndFeel.TouchUIMode = DevExpress.LookAndFeel.TouchUIMode.False;
            this.ceSimulate1State.Size = new System.Drawing.Size(90, 19);
            this.ceSimulate1State.TabIndex = 3;
            // 
            // cbComType
            // 
            this.cbComType.Location = new System.Drawing.Point(75, 52);
            this.cbComType.Name = "cbComType";
            this.cbComType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbComType.Properties.Items.AddRange(new object[] {
            "GSM",
            "GPRS"});
            this.cbComType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbComType.Size = new System.Drawing.Size(65, 20);
            this.cbComType.TabIndex = 7;
            // 
            // ceComType
            // 
            this.ceComType.Enabled = false;
            this.ceComType.Location = new System.Drawing.Point(6, 53);
            this.ceComType.Name = "ceComType";
            this.ceComType.Properties.Caption = "通讯方式";
            this.ceComType.Size = new System.Drawing.Size(74, 19);
            this.ceComType.TabIndex = 6;
            // 
            // ceTime
            // 
            this.ceTime.Location = new System.Drawing.Point(146, 26);
            this.ceTime.Name = "ceTime";
            this.ceTime.Properties.Caption = "设备时间";
            this.ceTime.Size = new System.Drawing.Size(75, 19);
            this.ceTime.TabIndex = 2;
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(75, 25);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(65, 20);
            this.txtID.TabIndex = 1;
            // 
            // ceCellPhone
            // 
            this.ceCellPhone.Enabled = false;
            this.ceCellPhone.Location = new System.Drawing.Point(338, 26);
            this.ceCellPhone.Name = "ceCellPhone";
            this.ceCellPhone.Properties.Caption = "手机号";
            this.ceCellPhone.Size = new System.Drawing.Size(74, 19);
            this.ceCellPhone.TabIndex = 4;
            // 
            // txtNum4
            // 
            this.txtNum4.EditValue = "";
            this.txtNum4.Location = new System.Drawing.Point(305, 52);
            this.txtNum4.Name = "txtNum4";
            this.txtNum4.Properties.MaxLength = 3;
            this.txtNum4.Size = new System.Drawing.Size(30, 20);
            this.txtNum4.TabIndex = 12;
            this.txtNum4.Tag = "4";
            this.txtNum4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNum_KeyPress);
            // 
            // cePort
            // 
            this.cePort.Location = new System.Drawing.Point(338, 53);
            this.cePort.Name = "cePort";
            this.cePort.Properties.Caption = "端口号";
            this.cePort.Size = new System.Drawing.Size(63, 19);
            this.cePort.TabIndex = 13;
            this.cePort.CheckedChanged += new System.EventHandler(this.cePort_CheckedChanged);
            // 
            // ceID
            // 
            this.ceID.Location = new System.Drawing.Point(6, 26);
            this.ceID.Name = "ceID";
            this.ceID.Properties.Caption = "设备编号";
            this.ceID.Size = new System.Drawing.Size(75, 19);
            this.ceID.TabIndex = 0;
            // 
            // txtNum3
            // 
            this.txtNum3.EditValue = "";
            this.txtNum3.Location = new System.Drawing.Point(275, 52);
            this.txtNum3.Name = "txtNum3";
            this.txtNum3.Properties.MaxLength = 3;
            this.txtNum3.Size = new System.Drawing.Size(30, 20);
            this.txtNum3.TabIndex = 11;
            this.txtNum3.Tag = "3";
            this.txtNum3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNum_KeyPress);
            // 
            // txtNum2
            // 
            this.txtNum2.EditValue = "";
            this.txtNum2.Location = new System.Drawing.Point(245, 52);
            this.txtNum2.Name = "txtNum2";
            this.txtNum2.Properties.MaxLength = 3;
            this.txtNum2.Size = new System.Drawing.Size(30, 20);
            this.txtNum2.TabIndex = 10;
            this.txtNum2.Tag = "2";
            this.txtNum2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNum_KeyPress);
            // 
            // txtNum1
            // 
            this.txtNum1.EditValue = "";
            this.txtNum1.Location = new System.Drawing.Point(215, 52);
            this.txtNum1.Name = "txtNum1";
            this.txtNum1.Properties.MaxLength = 3;
            this.txtNum1.Size = new System.Drawing.Size(30, 20);
            this.txtNum1.TabIndex = 9;
            this.txtNum1.Tag = "1";
            this.txtNum1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNum_KeyPress);
            // 
            // ceIP
            // 
            this.ceIP.Location = new System.Drawing.Point(145, 53);
            this.ceIP.Name = "ceIP";
            this.ceIP.Properties.Caption = "IP";
            this.ceIP.Size = new System.Drawing.Size(35, 19);
            this.ceIP.TabIndex = 8;
            this.ceIP.CheckedChanged += new System.EventHandler(this.ceIP_CheckedChanged);
            // 
            // ceModbusExeFlag
            // 
            this.ceModbusExeFlag.Location = new System.Drawing.Point(3, 0);
            this.ceModbusExeFlag.Name = "ceModbusExeFlag";
            this.ceModbusExeFlag.Properties.Caption = "RS485采集modbus协议配置";
            this.ceModbusExeFlag.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.ceModbusExeFlag.Properties.LookAndFeel.TouchUIMode = DevExpress.LookAndFeel.TouchUIMode.False;
            this.ceModbusExeFlag.Size = new System.Drawing.Size(177, 19);
            this.ceModbusExeFlag.TabIndex = 6;
            this.ceModbusExeFlag.CheckedChanged += new System.EventHandler(this.ceModbusExeFlag_CheckedChanged);
            // 
            // ceCollectRS485
            // 
            this.ceCollectRS485.Location = new System.Drawing.Point(4, 2);
            this.ceCollectRS485.Name = "ceCollectRS485";
            this.ceCollectRS485.Properties.Caption = "采集RS485";
            this.ceCollectRS485.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.ceCollectRS485.Properties.LookAndFeel.TouchUIMode = DevExpress.LookAndFeel.TouchUIMode.False;
            this.ceCollectRS485.Size = new System.Drawing.Size(117, 19);
            this.ceCollectRS485.TabIndex = 3;
            this.ceCollectRS485.CheckedChanged += new System.EventHandler(this.ceCollectRS485_CheckedChanged);
            // 
            // ceCollectPluse
            // 
            this.ceCollectPluse.Location = new System.Drawing.Point(2, 2);
            this.ceCollectPluse.Name = "ceCollectPluse";
            this.ceCollectPluse.Properties.Caption = "采集脉冲量";
            this.ceCollectPluse.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.ceCollectPluse.Properties.LookAndFeel.TouchUIMode = DevExpress.LookAndFeel.TouchUIMode.False;
            this.ceCollectPluse.Size = new System.Drawing.Size(87, 19);
            this.ceCollectPluse.TabIndex = 2;
            this.ceCollectPluse.CheckedChanged += new System.EventHandler(this.ceCollectPluse_CheckedChanged);
            // 
            // ceCollectSimulate
            // 
            this.ceCollectSimulate.Location = new System.Drawing.Point(2, -1);
            this.ceCollectSimulate.Name = "ceCollectSimulate";
            this.ceCollectSimulate.Properties.Caption = "采集模拟量";
            this.ceCollectSimulate.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.ceCollectSimulate.Properties.LookAndFeel.TouchUIMode = DevExpress.LookAndFeel.TouchUIMode.False;
            this.ceCollectSimulate.Size = new System.Drawing.Size(85, 19);
            this.ceCollectSimulate.TabIndex = 0;
            this.ceCollectSimulate.CheckedChanged += new System.EventHandler(this.ceCollectSimulate_CheckedChanged);
            // 
            // btnEnableCollect
            // 
            this.btnEnableCollect.Enabled = false;
            this.btnEnableCollect.Location = new System.Drawing.Point(502, 462);
            this.btnEnableCollect.Name = "btnEnableCollect";
            this.btnEnableCollect.Size = new System.Drawing.Size(88, 26);
            this.btnEnableCollect.TabIndex = 8;
            this.btnEnableCollect.Text = "启动采集";
            this.btnEnableCollect.Click += new System.EventHandler(this.btnEnableCollect_Click);
            // 
            // btnCheckingTime
            // 
            this.btnCheckingTime.Enabled = false;
            this.btnCheckingTime.Location = new System.Drawing.Point(399, 462);
            this.btnCheckingTime.Name = "btnCheckingTime";
            this.btnCheckingTime.Size = new System.Drawing.Size(88, 26);
            this.btnCheckingTime.TabIndex = 7;
            this.btnCheckingTime.Text = "校时";
            this.btnCheckingTime.Click += new System.EventHandler(this.btnCheckingTime_Click);
            // 
            // groupControl4
            // 
            this.groupControl4.Controls.Add(this.ceModbusExeFlag);
            this.groupControl4.Controls.Add(this.gridControl_485protocol);
            this.groupControl4.Location = new System.Drawing.Point(402, 86);
            this.groupControl4.Name = "groupControl4";
            this.groupControl4.Size = new System.Drawing.Size(395, 186);
            this.groupControl4.TabIndex = 4;
            // 
            // gridControl_485protocol
            // 
            this.gridControl_485protocol.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_485protocol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_485protocol.Location = new System.Drawing.Point(2, 22);
            this.gridControl_485protocol.MainView = this.gridView_485protocol;
            this.gridControl_485protocol.Name = "gridControl_485protocol";
            this.gridControl_485protocol.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.txt_485protocol_ID,
            this.txt_485protocol_funcode,
            this.txt_485protocol_regbeginaddr,
            this.txt_485protocol_regcount,
            this.cb_485protocol_baud});
            this.gridControl_485protocol.Size = new System.Drawing.Size(391, 162);
            this.gridControl_485protocol.TabIndex = 4;
            this.gridControl_485protocol.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView_485protocol});
            // 
            // gridView_485protocol
            // 
            this.gridView_485protocol.ActiveFilterEnabled = false;
            this.gridView_485protocol.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn4,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn13,
            this.gridColumn14});
            this.gridView_485protocol.GridControl = this.gridControl_485protocol;
            this.gridView_485protocol.IndicatorWidth = 30;
            this.gridView_485protocol.Name = "gridView_485protocol";
            this.gridView_485protocol.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView_485protocol.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.Inplace;
            this.gridView_485protocol.OptionsCustomization.AllowColumnMoving = false;
            this.gridView_485protocol.OptionsCustomization.AllowGroup = false;
            this.gridView_485protocol.OptionsCustomization.AllowSort = false;
            this.gridView_485protocol.OptionsFilter.AllowFilterEditor = false;
            this.gridView_485protocol.OptionsView.ShowGroupPanel = false;
            this.gridView_485protocol.OptionsView.ShowPreviewRowLines = DevExpress.Utils.DefaultBoolean.True;
            this.gridView_485protocol.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView_CustomDrawRowIndicator);
            this.gridView_485protocol.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView_485protocol_CellValueChanged);
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "波特率";
            this.gridColumn4.ColumnEdit = this.cb_485protocol_baud;
            this.gridColumn4.FieldName = "baud";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn4.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn4.OptionsColumn.AllowMove = false;
            this.gridColumn4.OptionsEditForm.StartNewRow = true;
            this.gridColumn4.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn4.OptionsFilter.AllowFilter = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 0;
            this.gridColumn4.Width = 71;
            // 
            // cb_485protocol_baud
            // 
            this.cb_485protocol_baud.AutoHeight = false;
            this.cb_485protocol_baud.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_485protocol_baud.Name = "cb_485protocol_baud";
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "设备ID";
            this.gridColumn11.ColumnEdit = this.txt_485protocol_ID;
            this.gridColumn11.FieldName = "ID";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn11.OptionsFilter.AllowFilter = false;
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 1;
            this.gridColumn11.Width = 52;
            // 
            // txt_485protocol_ID
            // 
            this.txt_485protocol_ID.AutoHeight = false;
            this.txt_485protocol_ID.Name = "txt_485protocol_ID";
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption = "功能码";
            this.gridColumn12.ColumnEdit = this.txt_485protocol_funcode;
            this.gridColumn12.FieldName = "funcode";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn12.OptionsFilter.AllowFilter = false;
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 2;
            this.gridColumn12.Width = 63;
            // 
            // txt_485protocol_funcode
            // 
            this.txt_485protocol_funcode.AutoHeight = false;
            this.txt_485protocol_funcode.Name = "txt_485protocol_funcode";
            // 
            // gridColumn13
            // 
            this.gridColumn13.Caption = "寄存器起始地址";
            this.gridColumn13.ColumnEdit = this.txt_485protocol_regbeginaddr;
            this.gridColumn13.FieldName = "regbeginaddr";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn13.OptionsFilter.AllowFilter = false;
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 3;
            this.gridColumn13.Width = 97;
            // 
            // txt_485protocol_regbeginaddr
            // 
            this.txt_485protocol_regbeginaddr.AutoHeight = false;
            this.txt_485protocol_regbeginaddr.Name = "txt_485protocol_regbeginaddr";
            // 
            // gridColumn14
            // 
            this.gridColumn14.Caption = "寄存器数量";
            this.gridColumn14.ColumnEdit = this.txt_485protocol_regcount;
            this.gridColumn14.FieldName = "regcount";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn14.OptionsFilter.AllowFilter = false;
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 4;
            this.gridColumn14.Width = 76;
            // 
            // txt_485protocol_regcount
            // 
            this.txt_485protocol_regcount.AutoHeight = false;
            this.txt_485protocol_regcount.Name = "txt_485protocol_regcount";
            // 
            // btnReadParm
            // 
            this.btnReadParm.Enabled = false;
            this.btnReadParm.Location = new System.Drawing.Point(605, 462);
            this.btnReadParm.Name = "btnReadParm";
            this.btnReadParm.Size = new System.Drawing.Size(88, 26);
            this.btnReadParm.TabIndex = 9;
            this.btnReadParm.Text = "读取设备参数";
            this.btnReadParm.Click += new System.EventHandler(this.btnReadParm_Click);
            // 
            // btnSetParm
            // 
            this.btnSetParm.Enabled = false;
            this.btnSetParm.Location = new System.Drawing.Point(708, 462);
            this.btnSetParm.Name = "btnSetParm";
            this.btnSetParm.Size = new System.Drawing.Size(88, 26);
            this.btnSetParm.TabIndex = 10;
            this.btnSetParm.Text = "设置设备参数";
            this.btnSetParm.Click += new System.EventHandler(this.btnSetParm_Click);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.btnCalibrationSimualte2);
            this.groupControl2.Controls.Add(this.btnCalibrationSimualte1);
            this.groupControl2.Controls.Add(this.ceCollectSimulate);
            this.groupControl2.Controls.Add(this.gridControl_Simulate);
            this.groupControl2.Location = new System.Drawing.Point(6, 86);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(395, 186);
            this.groupControl2.TabIndex = 1;
            // 
            // btnCalibrationSimualte2
            // 
            this.btnCalibrationSimualte2.Location = new System.Drawing.Point(170, 1);
            this.btnCalibrationSimualte2.Name = "btnCalibrationSimualte2";
            this.btnCalibrationSimualte2.Size = new System.Drawing.Size(71, 20);
            this.btnCalibrationSimualte2.TabIndex = 3;
            this.btnCalibrationSimualte2.Text = "第2路校准";
            this.btnCalibrationSimualte2.Click += new System.EventHandler(this.btnCalibrationSimualte2_Click);
            // 
            // btnCalibrationSimualte1
            // 
            this.btnCalibrationSimualte1.Location = new System.Drawing.Point(93, 1);
            this.btnCalibrationSimualte1.Name = "btnCalibrationSimualte1";
            this.btnCalibrationSimualte1.Size = new System.Drawing.Size(71, 20);
            this.btnCalibrationSimualte1.TabIndex = 2;
            this.btnCalibrationSimualte1.Text = "第1路校准";
            this.btnCalibrationSimualte1.Click += new System.EventHandler(this.btnCalibrationSimualte1_Click);
            // 
            // gridControl_Simulate
            // 
            this.gridControl_Simulate.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_Simulate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_Simulate.Location = new System.Drawing.Point(2, 22);
            this.gridControl_Simulate.MainView = this.gridView_Simulate;
            this.gridControl_Simulate.Name = "gridControl_Simulate";
            this.gridControl_Simulate.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cb_sim_starttime,
            this.cb_sim_coltime1,
            this.cb_sim_sendtime,
            this.cb_sim_coltime2});
            this.gridControl_Simulate.Size = new System.Drawing.Size(391, 162);
            this.gridControl_Simulate.TabIndex = 1;
            this.gridControl_Simulate.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView_Simulate});
            // 
            // gridView_Simulate
            // 
            this.gridView_Simulate.ActiveFilterEnabled = false;
            this.gridView_Simulate.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn8,
            this.gridColumn3});
            this.gridView_Simulate.GridControl = this.gridControl_Simulate;
            this.gridView_Simulate.IndicatorWidth = 30;
            this.gridView_Simulate.Name = "gridView_Simulate";
            this.gridView_Simulate.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView_Simulate.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.Inplace;
            this.gridView_Simulate.OptionsCustomization.AllowColumnMoving = false;
            this.gridView_Simulate.OptionsCustomization.AllowGroup = false;
            this.gridView_Simulate.OptionsCustomization.AllowSort = false;
            this.gridView_Simulate.OptionsFilter.AllowFilterEditor = false;
            this.gridView_Simulate.OptionsView.ShowGroupPanel = false;
            this.gridView_Simulate.OptionsView.ShowPreviewRowLines = DevExpress.Utils.DefaultBoolean.True;
            this.gridView_Simulate.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView_CustomDrawRowIndicator);
            this.gridView_Simulate.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gridView_Simulate_CustomRowCellEdit);
            this.gridView_Simulate.CustomRowCellEditForEditing += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gridView_Simulate_CustomRowCellEditForEditing);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "起始时间";
            this.gridColumn1.ColumnEdit = this.cb_sim_starttime;
            this.gridColumn1.FieldName = "starttime";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn1.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn1.OptionsColumn.AllowMove = false;
            this.gridColumn1.OptionsEditForm.StartNewRow = true;
            this.gridColumn1.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn1.OptionsFilter.AllowFilter = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // cb_sim_starttime
            // 
            this.cb_sim_starttime.AutoHeight = false;
            this.cb_sim_starttime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_sim_starttime.Name = "cb_sim_starttime";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "采集时间间隔1";
            this.gridColumn2.ColumnEdit = this.cb_sim_coltime1;
            this.gridColumn2.FieldName = "collecttime1";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn2.OptionsFilter.AllowFilter = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // cb_sim_coltime1
            // 
            this.cb_sim_coltime1.AutoHeight = false;
            this.cb_sim_coltime1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_sim_coltime1.Name = "cb_sim_coltime1";
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "采集时间间隔2";
            this.gridColumn8.ColumnEdit = this.cb_sim_coltime2;
            this.gridColumn8.FieldName = "collecttime2";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn8.OptionsFilter.AllowFilter = false;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 2;
            // 
            // cb_sim_coltime2
            // 
            this.cb_sim_coltime2.AutoHeight = false;
            this.cb_sim_coltime2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_sim_coltime2.Name = "cb_sim_coltime2";
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "发送时间间隔";
            this.gridColumn3.ColumnEdit = this.cb_sim_sendtime;
            this.gridColumn3.FieldName = "sendtime";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn3.OptionsFilter.AllowFilter = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
            // 
            // cb_sim_sendtime
            // 
            this.cb_sim_sendtime.AutoHeight = false;
            this.cb_sim_sendtime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_sim_sendtime.Name = "cb_sim_sendtime";
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.ceCollectRS485);
            this.groupControl3.Controls.Add(this.gridControl_RS485);
            this.groupControl3.Location = new System.Drawing.Point(402, 274);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(395, 186);
            this.groupControl3.TabIndex = 3;
            // 
            // gridControl_RS485
            // 
            this.gridControl_RS485.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_RS485.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_RS485.Location = new System.Drawing.Point(2, 22);
            this.gridControl_RS485.MainView = this.gridView_RS485;
            this.gridControl_RS485.Name = "gridControl_RS485";
            this.gridControl_RS485.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cb_RS485_starttime,
            this.cb_RS485_coltime,
            this.cb_RS485_sendtime});
            this.gridControl_RS485.Size = new System.Drawing.Size(391, 162);
            this.gridControl_RS485.TabIndex = 3;
            this.gridControl_RS485.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView_RS485});
            // 
            // gridView_RS485
            // 
            this.gridView_RS485.ActiveFilterEnabled = false;
            this.gridView_RS485.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn15});
            this.gridView_RS485.GridControl = this.gridControl_RS485;
            this.gridView_RS485.IndicatorWidth = 30;
            this.gridView_RS485.Name = "gridView_RS485";
            this.gridView_RS485.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView_RS485.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.Inplace;
            this.gridView_RS485.OptionsCustomization.AllowColumnMoving = false;
            this.gridView_RS485.OptionsCustomization.AllowGroup = false;
            this.gridView_RS485.OptionsCustomization.AllowSort = false;
            this.gridView_RS485.OptionsFilter.AllowFilterEditor = false;
            this.gridView_RS485.OptionsView.ShowGroupPanel = false;
            this.gridView_RS485.OptionsView.ShowPreviewRowLines = DevExpress.Utils.DefaultBoolean.True;
            this.gridView_RS485.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView_CustomDrawRowIndicator);
            this.gridView_RS485.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gridView_RS485_CustomRowCellEdit);
            this.gridView_RS485.CustomRowCellEditForEditing += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gridView_RS485_CustomRowCellEditForEditing);
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "起始时间";
            this.gridColumn6.ColumnEdit = this.cb_RS485_starttime;
            this.gridColumn6.FieldName = "starttime";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn6.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn6.OptionsColumn.AllowMove = false;
            this.gridColumn6.OptionsEditForm.StartNewRow = true;
            this.gridColumn6.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn6.OptionsFilter.AllowFilter = false;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 0;
            // 
            // cb_RS485_starttime
            // 
            this.cb_RS485_starttime.AutoHeight = false;
            this.cb_RS485_starttime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_RS485_starttime.Name = "cb_RS485_starttime";
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "采集时间间隔";
            this.gridColumn7.ColumnEdit = this.cb_RS485_coltime;
            this.gridColumn7.FieldName = "collecttime";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn7.OptionsFilter.AllowFilter = false;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 1;
            // 
            // cb_RS485_coltime
            // 
            this.cb_RS485_coltime.AutoHeight = false;
            this.cb_RS485_coltime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_RS485_coltime.Name = "cb_RS485_coltime";
            // 
            // gridColumn15
            // 
            this.gridColumn15.Caption = "发送时间间隔";
            this.gridColumn15.ColumnEdit = this.cb_RS485_sendtime;
            this.gridColumn15.FieldName = "sendtime";
            this.gridColumn15.Name = "gridColumn15";
            this.gridColumn15.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn15.OptionsFilter.AllowFilter = false;
            this.gridColumn15.Visible = true;
            this.gridColumn15.VisibleIndex = 2;
            // 
            // cb_RS485_sendtime
            // 
            this.cb_RS485_sendtime.AutoHeight = false;
            this.cb_RS485_sendtime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_RS485_sendtime.Name = "cb_RS485_sendtime";
            // 
            // groupControl5
            // 
            this.groupControl5.Controls.Add(this.ceCollectPluse);
            this.groupControl5.Controls.Add(this.gridControl_Pluse);
            this.groupControl5.Location = new System.Drawing.Point(6, 274);
            this.groupControl5.Name = "groupControl5";
            this.groupControl5.Size = new System.Drawing.Size(395, 186);
            this.groupControl5.TabIndex = 2;
            // 
            // gridControl_Pluse
            // 
            this.gridControl_Pluse.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_Pluse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_Pluse.Location = new System.Drawing.Point(2, 22);
            this.gridControl_Pluse.MainView = this.gridView_Pluse;
            this.gridControl_Pluse.Name = "gridControl_Pluse";
            this.gridControl_Pluse.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cb_pluse_starttime,
            this.cb_pluse_coltime,
            this.cb_pluse_sendtime});
            this.gridControl_Pluse.Size = new System.Drawing.Size(391, 162);
            this.gridControl_Pluse.TabIndex = 3;
            this.gridControl_Pluse.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView_Pluse});
            // 
            // gridView_Pluse
            // 
            this.gridView_Pluse.ActiveFilterEnabled = false;
            this.gridView_Pluse.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn5,
            this.gridColumn16,
            this.gridColumn17});
            this.gridView_Pluse.GridControl = this.gridControl_Pluse;
            this.gridView_Pluse.IndicatorWidth = 30;
            this.gridView_Pluse.Name = "gridView_Pluse";
            this.gridView_Pluse.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView_Pluse.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.Inplace;
            this.gridView_Pluse.OptionsCustomization.AllowColumnMoving = false;
            this.gridView_Pluse.OptionsCustomization.AllowGroup = false;
            this.gridView_Pluse.OptionsCustomization.AllowSort = false;
            this.gridView_Pluse.OptionsFilter.AllowFilterEditor = false;
            this.gridView_Pluse.OptionsView.ShowGroupPanel = false;
            this.gridView_Pluse.OptionsView.ShowPreviewRowLines = DevExpress.Utils.DefaultBoolean.True;
            this.gridView_Pluse.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView_CustomDrawRowIndicator);
            this.gridView_Pluse.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gridView_Pluse_CustomRowCellEdit);
            this.gridView_Pluse.CustomRowCellEditForEditing += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gridView_Pluse_CustomRowCellEditForEditing);
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "起始时间";
            this.gridColumn5.ColumnEdit = this.cb_pluse_starttime;
            this.gridColumn5.FieldName = "starttime";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn5.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn5.OptionsColumn.AllowMove = false;
            this.gridColumn5.OptionsEditForm.StartNewRow = true;
            this.gridColumn5.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn5.OptionsFilter.AllowFilter = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 0;
            // 
            // cb_pluse_starttime
            // 
            this.cb_pluse_starttime.AutoHeight = false;
            this.cb_pluse_starttime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_pluse_starttime.Name = "cb_pluse_starttime";
            // 
            // gridColumn16
            // 
            this.gridColumn16.Caption = "采集时间间隔";
            this.gridColumn16.ColumnEdit = this.cb_pluse_coltime;
            this.gridColumn16.FieldName = "collecttime";
            this.gridColumn16.Name = "gridColumn16";
            this.gridColumn16.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn16.OptionsFilter.AllowFilter = false;
            this.gridColumn16.Visible = true;
            this.gridColumn16.VisibleIndex = 1;
            // 
            // cb_pluse_coltime
            // 
            this.cb_pluse_coltime.AutoHeight = false;
            this.cb_pluse_coltime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_pluse_coltime.Name = "cb_pluse_coltime";
            // 
            // gridColumn17
            // 
            this.gridColumn17.Caption = "发送时间间隔";
            this.gridColumn17.ColumnEdit = this.cb_pluse_sendtime;
            this.gridColumn17.FieldName = "sendtime";
            this.gridColumn17.Name = "gridColumn17";
            this.gridColumn17.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn17.OptionsFilter.AllowFilter = false;
            this.gridColumn17.Visible = true;
            this.gridColumn17.VisibleIndex = 2;
            // 
            // cb_pluse_sendtime
            // 
            this.cb_pluse_sendtime.AutoHeight = false;
            this.cb_pluse_sendtime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_pluse_sendtime.Name = "cb_pluse_sendtime";
            // 
            // btnReset
            // 
            this.btnReset.Enabled = false;
            this.btnReset.Location = new System.Drawing.Point(296, 462);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(88, 26);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "设备复位";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // SwitchComunication
            // 
            this.SwitchComunication.EditValue = true;
            this.SwitchComunication.Location = new System.Drawing.Point(29, 462);
            this.SwitchComunication.Name = "SwitchComunication";
            this.SwitchComunication.Properties.OffText = "GPRS";
            this.SwitchComunication.Properties.OnText = "串口";
            this.SwitchComunication.Size = new System.Drawing.Size(117, 25);
            this.SwitchComunication.TabIndex = 11;
            this.SwitchComunication.Click += new System.EventHandler(this.SwitchComunication_Click);
            // 
            // btnSetPluseBasic
            // 
            this.btnSetPluseBasic.Enabled = false;
            this.btnSetPluseBasic.Location = new System.Drawing.Point(193, 462);
            this.btnSetPluseBasic.Name = "btnSetPluseBasic";
            this.btnSetPluseBasic.Size = new System.Drawing.Size(88, 26);
            this.btnSetPluseBasic.TabIndex = 5;
            this.btnSetPluseBasic.Text = "设置脉冲基准";
            this.btnSetPluseBasic.Click += new System.EventHandler(this.btnSetPluseBasic_Click);
            // 
            // UniversalTerParm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SwitchComunication);
            this.Controls.Add(this.btnEnableCollect);
            this.Controls.Add(this.btnSetPluseBasic);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnCheckingTime);
            this.Controls.Add(this.btnSetParm);
            this.Controls.Add(this.btnReadParm);
            this.Controls.Add(this.groupControl3);
            this.Controls.Add(this.groupControl5);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl4);
            this.Controls.Add(this.groupControl1);
            this.Name = "UniversalTerParm";
            this.Size = new System.Drawing.Size(797, 494);
            this.Load += new System.EventHandler(this.UniversalTerParm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtCellPhone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceColConfig.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl7)).EndInit();
            this.groupControl7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cePluseState.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cemodbusprotocolstatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceRS485State.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSimulate2State.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSimulate1State.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbComType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceComType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceCellPhone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNum4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePort.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNum3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNum2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNum1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceIP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceModbusExeFlag.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceCollectRS485.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceCollectPluse.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceCollectSimulate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
            this.groupControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_485protocol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_485protocol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_485protocol_baud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_485protocol_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_485protocol_funcode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_485protocol_regbeginaddr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_485protocol_regcount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_Simulate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_Simulate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_sim_starttime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_sim_coltime1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_sim_coltime2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_sim_sendtime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_RS485)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_RS485)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_RS485_starttime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_RS485_coltime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_RS485_sendtime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).EndInit();
            this.groupControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_Pluse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_Pluse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_pluse_starttime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_pluse_coltime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_pluse_sendtime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SwitchComunication.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.CheckEdit ceTime;
        private DevExpress.XtraEditors.TextEdit txtID;
        private DevExpress.XtraEditors.CheckEdit ceID;
        private DevExpress.XtraEditors.TextEdit txtPort;
        private DevExpress.XtraEditors.TextEdit txtCellPhone;
        private DevExpress.XtraEditors.ComboBoxEdit cbComType;
        private DevExpress.XtraEditors.CheckEdit ceCellPhone;
        private DevExpress.XtraEditors.CheckEdit ceIP;
        private DevExpress.XtraEditors.CheckEdit ceComType;
        private DevExpress.XtraEditors.CheckEdit ceModbusExeFlag;
        private DevExpress.XtraEditors.TextEdit txtNum1;
        private DevExpress.XtraEditors.TextEdit txtNum2;
        private DevExpress.XtraEditors.TextEdit txtNum4;
        private DevExpress.XtraEditors.TextEdit txtNum3;
        private DevExpress.XtraEditors.SimpleButton btnEnableCollect;
        private DevExpress.XtraEditors.SimpleButton btnCheckingTime;
        private DevExpress.XtraEditors.GroupControl groupControl4;
        private DevExpress.XtraEditors.SimpleButton btnReadParm;
        private DevExpress.XtraEditors.SimpleButton btnSetParm;
        private DevExpress.XtraEditors.CheckEdit ceColConfig;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.GroupControl groupControl5;
        private DevExpress.XtraEditors.CheckEdit cePort;
        private DevExpress.XtraEditors.GroupControl groupControl7;
        private DevExpress.XtraEditors.CheckEdit ceCollectRS485;
        private DevExpress.XtraEditors.CheckEdit ceCollectPluse;
        private DevExpress.XtraEditors.CheckEdit ceCollectSimulate;
        private DevExpress.XtraEditors.SimpleButton btnReset;
        private DevExpress.XtraEditors.TextEdit txtTime;
        private DevExpress.XtraGrid.GridControl gridControl_Simulate;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_Simulate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_sim_starttime;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_sim_coltime1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_sim_sendtime;
        private DevExpress.XtraGrid.GridControl gridControl_Pluse;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_Pluse;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_pluse_starttime;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_pluse_coltime;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn17;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_pluse_sendtime;
        private DevExpress.XtraGrid.GridControl gridControl_RS485;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_RS485;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_RS485_starttime;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_RS485_coltime;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn15;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_RS485_sendtime;
        private DevExpress.XtraGrid.GridControl gridControl_485protocol;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_485protocol;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txt_485protocol_ID;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txt_485protocol_funcode;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txt_485protocol_regbeginaddr;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txt_485protocol_regcount;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_485protocol_baud;
        private DevExpress.XtraEditors.ToggleSwitch SwitchComunication;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_sim_coltime2;
        private DevExpress.XtraEditors.CheckEdit cePluseState;
        private DevExpress.XtraEditors.CheckEdit ceRS485State;
        private DevExpress.XtraEditors.CheckEdit ceSimulate1State;
        private DevExpress.XtraEditors.CheckEdit cemodbusprotocolstatus;
        private DevExpress.XtraEditors.SimpleButton btnCalibrationSimualte2;
        private DevExpress.XtraEditors.SimpleButton btnCalibrationSimualte1;
        private DevExpress.XtraEditors.CheckEdit ceSimulate2State;
        private DevExpress.XtraEditors.SimpleButton btnSetPluseBasic;

    }
}