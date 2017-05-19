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
            this.components = new System.ComponentModel.Container();
            this.cb_sim_starttime = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.cb_sim_coltime1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.cb_sim_coltime2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.cb_sim_sendtime = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtVolLower = new DevExpress.XtraEditors.TextEdit();
            this.txtIP = new DevExpress.XtraEditors.TextEdit();
            this.cb485BaudRate = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtVolInterval = new DevExpress.XtraEditors.TextEdit();
            this.txtAlarmLen = new DevExpress.XtraEditors.TextEdit();
            this.txtSMSInterval = new DevExpress.XtraEditors.TextEdit();
            this.txtHeart = new DevExpress.XtraEditors.TextEdit();
            this.cbPluseUnit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbModbusExeFlag = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbNetworkType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ceModbusExeFlag = new DevExpress.XtraEditors.CheckEdit();
            this.ceNetWorkType = new DevExpress.XtraEditors.CheckEdit();
            this.txtCellPhone = new DevExpress.XtraEditors.TextEdit();
            this.ceVolInterval = new DevExpress.XtraEditors.CheckEdit();
            this.txtTime = new DevExpress.XtraEditors.TextEdit();
            this.ce485Baud = new DevExpress.XtraEditors.CheckEdit();
            this.ceHeart = new DevExpress.XtraEditors.CheckEdit();
            this.ceAlarmLen = new DevExpress.XtraEditors.CheckEdit();
            this.cePluseUnit = new DevExpress.XtraEditors.CheckEdit();
            this.ceSMSInterval = new DevExpress.XtraEditors.CheckEdit();
            this.ceVolLower = new DevExpress.XtraEditors.CheckEdit();
            this.txtPort = new DevExpress.XtraEditors.TextEdit();
            this.cbComType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ceComType = new DevExpress.XtraEditors.CheckEdit();
            this.ceTime = new DevExpress.XtraEditors.CheckEdit();
            this.txtID = new DevExpress.XtraEditors.TextEdit();
            this.ceCellPhone = new DevExpress.XtraEditors.CheckEdit();
            this.cePort = new DevExpress.XtraEditors.CheckEdit();
            this.ceID = new DevExpress.XtraEditors.CheckEdit();
            this.ceIP = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl7 = new DevExpress.XtraEditors.GroupControl();
            this.cePluseState = new DevExpress.XtraEditors.CheckEdit();
            this.ceDigitPreStatus = new DevExpress.XtraEditors.CheckEdit();
            this.ceRS485State = new DevExpress.XtraEditors.CheckEdit();
            this.ceSimulate2State = new DevExpress.XtraEditors.CheckEdit();
            this.ceSimulate1State = new DevExpress.XtraEditors.CheckEdit();
            this.ceColConfig = new DevExpress.XtraEditors.CheckEdit();
            this.ceCollectRS485 = new DevExpress.XtraEditors.CheckEdit();
            this.btnEnableCollect = new DevExpress.XtraEditors.SimpleButton();
            this.btnCheckingTime = new DevExpress.XtraEditors.SimpleButton();
            this.btnReadParm = new DevExpress.XtraEditors.SimpleButton();
            this.btnSetParm = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl_Simulate = new DevExpress.XtraGrid.GridControl();
            this.gridView_Simulate = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.gridControl_RS485 = new DevExpress.XtraGrid.GridControl();
            this.gridView_RS485 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_RS485_starttime = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_RS485_coltime = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_RS485_sendtime = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.SwitchComunication = new DevExpress.XtraEditors.ToggleSwitch();
            this.btnSetPluseBasic = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.ceCollectSimulate = new DevExpress.XtraEditors.CheckEdit();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
            this.ceCollectModbus = new DevExpress.XtraEditors.CheckEdit();
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
            this.xtraTabPage4 = new DevExpress.XtraTab.XtraTabPage();
            this.groupControl5 = new DevExpress.XtraEditors.GroupControl();
            this.ceCollectPluse = new DevExpress.XtraEditors.CheckEdit();
            this.gridControl_Pluse = new DevExpress.XtraGrid.GridControl();
            this.gridView_Pluse = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_pluse_starttime = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_pre_coltime = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_pluse_coltime = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn17 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cb_pluse_sendtime = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.groupControl8 = new DevExpress.XtraEditors.GroupControl();
            this.cbSlopLowLimitEnable = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbSlopUpLimitEnable = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbPreLowLimitEnable = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cePreRange = new DevExpress.XtraEditors.CheckEdit();
            this.cbPreUpLimitEnable = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtSlopLowLimit = new DevExpress.XtraEditors.TextEdit();
            this.txtOffsetBaseV = new DevExpress.XtraEditors.TextEdit();
            this.cbPreFlag = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ceOffsetBaseV = new DevExpress.XtraEditors.CheckEdit();
            this.txtSlopUpLimit = new DevExpress.XtraEditors.TextEdit();
            this.ceSlopLowLimitEnable = new DevExpress.XtraEditors.CheckEdit();
            this.txtPreRange = new DevExpress.XtraEditors.TextEdit();
            this.txtPreLowLimit = new DevExpress.XtraEditors.TextEdit();
            this.ceSlopUpLimitEnable = new DevExpress.XtraEditors.CheckEdit();
            this.txtPreUpLimit = new DevExpress.XtraEditors.TextEdit();
            this.ceSlopLowLimit = new DevExpress.XtraEditors.CheckEdit();
            this.cePreLowLimitEnable = new DevExpress.XtraEditors.CheckEdit();
            this.ceSlopUpLimit = new DevExpress.XtraEditors.CheckEdit();
            this.cePreLowLimit = new DevExpress.XtraEditors.CheckEdit();
            this.cePreUpLimitEnable = new DevExpress.XtraEditors.CheckEdit();
            this.cePreUpLimit = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl12 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl11 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl10 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl9 = new DevExpress.XtraEditors.GroupControl();
            this.dropbtnCalibrationSimualte = new DevExpress.XtraEditors.DropDownButton();
            this.popupMenuCalibration = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barbtnCalibrationSimualte1 = new DevExpress.XtraBars.BarButtonItem();
            this.barbtnCalibrationSimualte2 = new DevExpress.XtraBars.BarButtonItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.btnStartAlarm = new DevExpress.XtraBars.BarButtonItem();
            this.btnStopAlarm = new DevExpress.XtraBars.BarButtonItem();
            this.btnReset1 = new DevExpress.XtraBars.BarButtonItem();
            this.btnReset2 = new DevExpress.XtraBars.BarButtonItem();
            this.btnReset3 = new DevExpress.XtraBars.BarButtonItem();
            this.groupControl6 = new DevExpress.XtraEditors.GroupControl();
            this.treeSocketType = new DevExpress.XtraEditors.TreeListLookUpEdit();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.btnCallClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnCallOpen = new DevExpress.XtraEditors.SimpleButton();
            this.btnCallData = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl13 = new DevExpress.XtraEditors.GroupControl();
            this.btnVer = new DevExpress.XtraEditors.SimpleButton();
            this.btnFieldStrength = new DevExpress.XtraEditors.SimpleButton();
            this.timer_GetWaitCmd = new System.Windows.Forms.Timer(this.components);
            this.gridControl_WaitCmd = new DevExpress.XtraGrid.GridControl();
            this.gridView_WaitCmd = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn18 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn20 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn21 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.类型 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnDel = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl14 = new DevExpress.XtraEditors.GroupControl();
            this.btnRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.btnEnableAlarm = new DevExpress.XtraEditors.SimpleButton();
            this.dropDownButtonReset = new DevExpress.XtraEditors.DropDownButton();
            this.popupMenuReset = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.cb_sim_starttime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_sim_coltime1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_sim_coltime2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_sim_sendtime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVolLower.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb485BaudRate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVolInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlarmLen.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSMSInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbPluseUnit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbModbusExeFlag.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbNetworkType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceModbusExeFlag.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceNetWorkType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCellPhone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceVolInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ce485Baud.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceHeart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceAlarmLen.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePluseUnit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSMSInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceVolLower.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbComType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceComType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceCellPhone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePort.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceIP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl7)).BeginInit();
            this.groupControl7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cePluseState.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceDigitPreStatus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceRS485State.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSimulate2State.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSimulate1State.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceColConfig.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceCollectRS485.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_Simulate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_Simulate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_RS485)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_RS485)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_RS485_starttime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_RS485_coltime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_RS485_sendtime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SwitchComunication.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceCollectSimulate.Properties)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
            this.groupControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceCollectModbus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_485protocol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_485protocol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_485protocol_baud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_485protocol_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_485protocol_funcode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_485protocol_regbeginaddr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_485protocol_regcount)).BeginInit();
            this.xtraTabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).BeginInit();
            this.groupControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceCollectPluse.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_Pluse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_Pluse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_pluse_starttime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_pre_coltime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_pluse_coltime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_pluse_sendtime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl8)).BeginInit();
            this.groupControl8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbSlopLowLimitEnable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbSlopUpLimitEnable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbPreLowLimitEnable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePreRange.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbPreUpLimitEnable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSlopLowLimit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOffsetBaseV.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbPreFlag.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceOffsetBaseV.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSlopUpLimit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSlopLowLimitEnable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPreRange.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPreLowLimit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSlopUpLimitEnable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPreUpLimit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSlopLowLimit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePreLowLimitEnable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSlopUpLimit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePreLowLimit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePreUpLimitEnable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePreUpLimit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuCalibration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).BeginInit();
            this.groupControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeSocketType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl13)).BeginInit();
            this.groupControl13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_WaitCmd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_WaitCmd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl14)).BeginInit();
            this.groupControl14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuReset)).BeginInit();
            this.SuspendLayout();
            // 
            // cb_sim_starttime
            // 
            this.cb_sim_starttime.AutoHeight = false;
            this.cb_sim_starttime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_sim_starttime.Name = "cb_sim_starttime";
            // 
            // cb_sim_coltime1
            // 
            this.cb_sim_coltime1.AutoHeight = false;
            this.cb_sim_coltime1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_sim_coltime1.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.cb_sim_coltime1.MaxLength = 4;
            this.cb_sim_coltime1.Name = "cb_sim_coltime1";
            // 
            // cb_sim_coltime2
            // 
            this.cb_sim_coltime2.AutoHeight = false;
            this.cb_sim_coltime2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_sim_coltime2.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.cb_sim_coltime2.MaxLength = 4;
            this.cb_sim_coltime2.Name = "cb_sim_coltime2";
            // 
            // cb_sim_sendtime
            // 
            this.cb_sim_sendtime.AutoHeight = false;
            this.cb_sim_sendtime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_sim_sendtime.Name = "cb_sim_sendtime";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.txtVolLower);
            this.groupControl1.Controls.Add(this.txtIP);
            this.groupControl1.Controls.Add(this.cb485BaudRate);
            this.groupControl1.Controls.Add(this.txtVolInterval);
            this.groupControl1.Controls.Add(this.txtAlarmLen);
            this.groupControl1.Controls.Add(this.txtSMSInterval);
            this.groupControl1.Controls.Add(this.txtHeart);
            this.groupControl1.Controls.Add(this.cbPluseUnit);
            this.groupControl1.Controls.Add(this.cbModbusExeFlag);
            this.groupControl1.Controls.Add(this.cbNetworkType);
            this.groupControl1.Controls.Add(this.ceModbusExeFlag);
            this.groupControl1.Controls.Add(this.ceNetWorkType);
            this.groupControl1.Controls.Add(this.txtCellPhone);
            this.groupControl1.Controls.Add(this.ceVolInterval);
            this.groupControl1.Controls.Add(this.txtTime);
            this.groupControl1.Controls.Add(this.ce485Baud);
            this.groupControl1.Controls.Add(this.ceHeart);
            this.groupControl1.Controls.Add(this.ceAlarmLen);
            this.groupControl1.Controls.Add(this.cePluseUnit);
            this.groupControl1.Controls.Add(this.ceSMSInterval);
            this.groupControl1.Controls.Add(this.ceVolLower);
            this.groupControl1.Controls.Add(this.txtPort);
            this.groupControl1.Controls.Add(this.cbComType);
            this.groupControl1.Controls.Add(this.ceComType);
            this.groupControl1.Controls.Add(this.ceTime);
            this.groupControl1.Controls.Add(this.txtID);
            this.groupControl1.Controls.Add(this.ceCellPhone);
            this.groupControl1.Controls.Add(this.cePort);
            this.groupControl1.Controls.Add(this.ceID);
            this.groupControl1.Controls.Add(this.ceIP);
            this.groupControl1.Location = new System.Drawing.Point(6, -1);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(791, 105);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "基本信息参数";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(449, 78);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(19, 14);
            this.labelControl3.TabIndex = 29;
            this.labelControl3.Text = "min";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(150, 78);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(19, 14);
            this.labelControl2.TabIndex = 23;
            this.labelControl2.Text = "min";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(443, 51);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(5, 14);
            this.labelControl4.TabIndex = 16;
            this.labelControl4.Text = "s";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(295, 78);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(12, 14);
            this.labelControl1.TabIndex = 26;
            this.labelControl1.Text = "%";
            // 
            // txtVolLower
            // 
            this.txtVolLower.EditValue = "";
            this.txtVolLower.Location = new System.Drawing.Point(263, 75);
            this.txtVolLower.Name = "txtVolLower";
            this.txtVolLower.Size = new System.Drawing.Size(31, 20);
            this.txtVolLower.TabIndex = 25;
            // 
            // txtIP
            // 
            this.txtIP.EditValue = "";
            this.txtIP.Location = new System.Drawing.Point(43, 48);
            this.txtIP.Name = "txtIP";
            this.txtIP.Properties.Mask.EditMask = "(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[" +
    "1-9]?[0-9])\\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\\.(25[0-5]|2[0-4][0-9]" +
    "|1[0-9][0-9]|[1-9]?[0-9])";
            this.txtIP.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtIP.Properties.MaxLength = 15;
            this.txtIP.Size = new System.Drawing.Size(104, 20);
            this.txtIP.TabIndex = 11;
            // 
            // cb485BaudRate
            // 
            this.cb485BaudRate.Location = new System.Drawing.Point(531, 48);
            this.cb485BaudRate.Name = "cb485BaudRate";
            this.cb485BaudRate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb485BaudRate.Properties.Items.AddRange(new object[] {
            "1200",
            "2400",
            "4800",
            "9600"});
            this.cb485BaudRate.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cb485BaudRate.Size = new System.Drawing.Size(73, 20);
            this.cb485BaudRate.TabIndex = 18;
            // 
            // txtVolInterval
            // 
            this.txtVolInterval.EditValue = "";
            this.txtVolInterval.Location = new System.Drawing.Point(104, 75);
            this.txtVolInterval.Name = "txtVolInterval";
            this.txtVolInterval.Size = new System.Drawing.Size(43, 20);
            this.txtVolInterval.TabIndex = 22;
            // 
            // txtAlarmLen
            // 
            this.txtAlarmLen.Location = new System.Drawing.Point(717, 75);
            this.txtAlarmLen.Name = "txtAlarmLen";
            this.txtAlarmLen.Size = new System.Drawing.Size(68, 20);
            this.txtAlarmLen.TabIndex = 33;
            // 
            // txtSMSInterval
            // 
            this.txtSMSInterval.Location = new System.Drawing.Point(403, 75);
            this.txtSMSInterval.Name = "txtSMSInterval";
            this.txtSMSInterval.Size = new System.Drawing.Size(45, 20);
            this.txtSMSInterval.TabIndex = 28;
            // 
            // txtHeart
            // 
            this.txtHeart.Location = new System.Drawing.Point(375, 48);
            this.txtHeart.Name = "txtHeart";
            this.txtHeart.Size = new System.Drawing.Size(65, 20);
            this.txtHeart.TabIndex = 15;
            // 
            // cbPluseUnit
            // 
            this.cbPluseUnit.Location = new System.Drawing.Point(566, 75);
            this.cbPluseUnit.Name = "cbPluseUnit";
            this.cbPluseUnit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbPluseUnit.Properties.Items.AddRange(new object[] {
            "0.01",
            "0.1",
            "0.2",
            "0.5",
            "1",
            "10",
            "100"});
            this.cbPluseUnit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbPluseUnit.Size = new System.Drawing.Size(68, 20);
            this.cbPluseUnit.TabIndex = 31;
            // 
            // cbModbusExeFlag
            // 
            this.cbModbusExeFlag.Location = new System.Drawing.Point(717, 48);
            this.cbModbusExeFlag.Name = "cbModbusExeFlag";
            this.cbModbusExeFlag.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbModbusExeFlag.Properties.Items.AddRange(new object[] {
            "不执行",
            "执行"});
            this.cbModbusExeFlag.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbModbusExeFlag.Size = new System.Drawing.Size(68, 20);
            this.cbModbusExeFlag.TabIndex = 20;
            // 
            // cbNetworkType
            // 
            this.cbNetworkType.Location = new System.Drawing.Point(717, 23);
            this.cbNetworkType.Name = "cbNetworkType";
            this.cbNetworkType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbNetworkType.Properties.Items.AddRange(new object[] {
            "不连网",
            "低功耗模式",
            "实时在线"});
            this.cbNetworkType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbNetworkType.Size = new System.Drawing.Size(68, 20);
            this.cbNetworkType.TabIndex = 9;
            // 
            // ceModbusExeFlag
            // 
            this.ceModbusExeFlag.Location = new System.Drawing.Point(606, 49);
            this.ceModbusExeFlag.Name = "ceModbusExeFlag";
            this.ceModbusExeFlag.Properties.Caption = "modbus执行标识";
            this.ceModbusExeFlag.Size = new System.Drawing.Size(113, 19);
            this.ceModbusExeFlag.TabIndex = 19;
            this.ceModbusExeFlag.CheckedChanged += new System.EventHandler(this.cePort_CheckedChanged);
            // 
            // ceNetWorkType
            // 
            this.ceNetWorkType.Location = new System.Drawing.Point(648, 24);
            this.ceNetWorkType.Name = "ceNetWorkType";
            this.ceNetWorkType.Properties.Caption = "联网模式";
            this.ceNetWorkType.Size = new System.Drawing.Size(71, 19);
            this.ceNetWorkType.TabIndex = 8;
            this.ceNetWorkType.CheckedChanged += new System.EventHandler(this.cePort_CheckedChanged);
            // 
            // txtCellPhone
            // 
            this.txtCellPhone.EditValue = "";
            this.txtCellPhone.Location = new System.Drawing.Point(396, 25);
            this.txtCellPhone.Name = "txtCellPhone";
            this.txtCellPhone.Size = new System.Drawing.Size(92, 20);
            this.txtCellPhone.TabIndex = 5;
            // 
            // ceVolInterval
            // 
            this.ceVolInterval.Location = new System.Drawing.Point(6, 76);
            this.ceVolInterval.Name = "ceVolInterval";
            this.ceVolInterval.Properties.Caption = "电压时间间隔";
            this.ceVolInterval.Size = new System.Drawing.Size(97, 19);
            this.ceVolInterval.TabIndex = 21;
            this.ceVolInterval.CheckedChanged += new System.EventHandler(this.cePort_CheckedChanged);
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(215, 25);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(120, 20);
            this.txtTime.TabIndex = 3;
            // 
            // ce485Baud
            // 
            this.ce485Baud.Location = new System.Drawing.Point(455, 49);
            this.ce485Baud.Name = "ce485Baud";
            this.ce485Baud.Properties.Caption = "485波特率";
            this.ce485Baud.Size = new System.Drawing.Size(83, 19);
            this.ce485Baud.TabIndex = 17;
            this.ce485Baud.CheckedChanged += new System.EventHandler(this.cePort_CheckedChanged);
            // 
            // ceHeart
            // 
            this.ceHeart.Location = new System.Drawing.Point(306, 49);
            this.ceHeart.Name = "ceHeart";
            this.ceHeart.Properties.Caption = "心跳间隔";
            this.ceHeart.Size = new System.Drawing.Size(71, 19);
            this.ceHeart.TabIndex = 14;
            this.ceHeart.CheckedChanged += new System.EventHandler(this.cePort_CheckedChanged);
            // 
            // ceAlarmLen
            // 
            this.ceAlarmLen.Location = new System.Drawing.Point(648, 76);
            this.ceAlarmLen.Name = "ceAlarmLen";
            this.ceAlarmLen.Properties.Caption = "报警次数";
            this.ceAlarmLen.Size = new System.Drawing.Size(94, 19);
            this.ceAlarmLen.TabIndex = 32;
            this.ceAlarmLen.CheckedChanged += new System.EventHandler(this.cePort_CheckedChanged);
            // 
            // cePluseUnit
            // 
            this.cePluseUnit.Location = new System.Drawing.Point(472, 76);
            this.cePluseUnit.Name = "cePluseUnit";
            this.cePluseUnit.Properties.Caption = "脉冲计数单位";
            this.cePluseUnit.Size = new System.Drawing.Size(99, 19);
            this.cePluseUnit.TabIndex = 30;
            this.cePluseUnit.CheckedChanged += new System.EventHandler(this.cePort_CheckedChanged);
            // 
            // ceSMSInterval
            // 
            this.ceSMSInterval.Location = new System.Drawing.Point(307, 76);
            this.ceSMSInterval.Name = "ceSMSInterval";
            this.ceSMSInterval.Properties.Caption = "短信发送间隔";
            this.ceSMSInterval.Size = new System.Drawing.Size(99, 19);
            this.ceSMSInterval.TabIndex = 27;
            this.ceSMSInterval.CheckedChanged += new System.EventHandler(this.cePort_CheckedChanged);
            // 
            // ceVolLower
            // 
            this.ceVolLower.Location = new System.Drawing.Point(171, 76);
            this.ceVolLower.Name = "ceVolLower";
            this.ceVolLower.Properties.Caption = "电压报警下限";
            this.ceVolLower.Size = new System.Drawing.Size(99, 19);
            this.ceVolLower.TabIndex = 24;
            this.ceVolLower.CheckedChanged += new System.EventHandler(this.cePort_CheckedChanged);
            // 
            // txtPort
            // 
            this.txtPort.EditValue = "";
            this.txtPort.Location = new System.Drawing.Point(229, 48);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(65, 20);
            this.txtPort.TabIndex = 13;
            // 
            // cbComType
            // 
            this.cbComType.Location = new System.Drawing.Point(563, 23);
            this.cbComType.Name = "cbComType";
            this.cbComType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbComType.Properties.Items.AddRange(new object[] {
            "移动GSM",
            "GPRS方式",
            "CDMA方式",
            "电信GSM"});
            this.cbComType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbComType.Size = new System.Drawing.Size(83, 20);
            this.cbComType.TabIndex = 7;
            // 
            // ceComType
            // 
            this.ceComType.Location = new System.Drawing.Point(494, 24);
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
            this.ceCellPhone.Location = new System.Drawing.Point(338, 26);
            this.ceCellPhone.Name = "ceCellPhone";
            this.ceCellPhone.Properties.Caption = "手机号";
            this.ceCellPhone.Size = new System.Drawing.Size(74, 19);
            this.ceCellPhone.TabIndex = 4;
            // 
            // cePort
            // 
            this.cePort.Location = new System.Drawing.Point(171, 49);
            this.cePort.Name = "cePort";
            this.cePort.Properties.Caption = "端口号";
            this.cePort.Size = new System.Drawing.Size(63, 19);
            this.cePort.TabIndex = 12;
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
            // ceIP
            // 
            this.ceIP.Location = new System.Drawing.Point(6, 49);
            this.ceIP.Name = "ceIP";
            this.ceIP.Properties.Caption = "IP";
            this.ceIP.Size = new System.Drawing.Size(35, 19);
            this.ceIP.TabIndex = 10;
            // 
            // groupControl7
            // 
            this.groupControl7.Controls.Add(this.cePluseState);
            this.groupControl7.Controls.Add(this.ceDigitPreStatus);
            this.groupControl7.Controls.Add(this.ceRS485State);
            this.groupControl7.Controls.Add(this.ceSimulate2State);
            this.groupControl7.Controls.Add(this.ceSimulate1State);
            this.groupControl7.Controls.Add(this.ceColConfig);
            this.groupControl7.Location = new System.Drawing.Point(6, 183);
            this.groupControl7.Name = "groupControl7";
            this.groupControl7.ShowCaption = false;
            this.groupControl7.Size = new System.Drawing.Size(788, 31);
            this.groupControl7.TabIndex = 2;
            // 
            // cePluseState
            // 
            this.cePluseState.Enabled = false;
            this.cePluseState.Location = new System.Drawing.Point(275, 5);
            this.cePluseState.Name = "cePluseState";
            this.cePluseState.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.cePluseState.Properties.Appearance.Options.UseForeColor = true;
            this.cePluseState.Properties.Caption = "脉冲量";
            this.cePluseState.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.cePluseState.Size = new System.Drawing.Size(60, 19);
            this.cePluseState.TabIndex = 2;
            // 
            // ceDigitPreStatus
            // 
            this.ceDigitPreStatus.Enabled = false;
            this.ceDigitPreStatus.Location = new System.Drawing.Point(168, 5);
            this.ceDigitPreStatus.Name = "ceDigitPreStatus";
            this.ceDigitPreStatus.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.ceDigitPreStatus.Properties.Appearance.Options.UseForeColor = true;
            this.ceDigitPreStatus.Properties.Caption = "数字量压力";
            this.ceDigitPreStatus.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.ceDigitPreStatus.Size = new System.Drawing.Size(90, 19);
            this.ceDigitPreStatus.TabIndex = 1;
            // 
            // ceRS485State
            // 
            this.ceRS485State.Enabled = false;
            this.ceRS485State.Location = new System.Drawing.Point(352, 5);
            this.ceRS485State.Name = "ceRS485State";
            this.ceRS485State.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.ceRS485State.Properties.Appearance.Options.UseForeColor = true;
            this.ceRS485State.Properties.Caption = "RS485";
            this.ceRS485State.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.ceRS485State.Size = new System.Drawing.Size(60, 19);
            this.ceRS485State.TabIndex = 3;
            // 
            // ceSimulate2State
            // 
            this.ceSimulate2State.Enabled = false;
            this.ceSimulate2State.Location = new System.Drawing.Point(536, 5);
            this.ceSimulate2State.Name = "ceSimulate2State";
            this.ceSimulate2State.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.ceSimulate2State.Properties.Appearance.Options.UseForeColor = true;
            this.ceSimulate2State.Properties.Caption = "第2路模拟量";
            this.ceSimulate2State.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.ceSimulate2State.Size = new System.Drawing.Size(90, 19);
            this.ceSimulate2State.TabIndex = 5;
            // 
            // ceSimulate1State
            // 
            this.ceSimulate1State.Enabled = false;
            this.ceSimulate1State.Location = new System.Drawing.Point(429, 5);
            this.ceSimulate1State.Name = "ceSimulate1State";
            this.ceSimulate1State.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.ceSimulate1State.Properties.Appearance.Options.UseForeColor = true;
            this.ceSimulate1State.Properties.Caption = "第1路模拟量";
            this.ceSimulate1State.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.ceSimulate1State.Size = new System.Drawing.Size(90, 19);
            this.ceSimulate1State.TabIndex = 4;
            // 
            // ceColConfig
            // 
            this.ceColConfig.Location = new System.Drawing.Point(13, 5);
            this.ceColConfig.Name = "ceColConfig";
            this.ceColConfig.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ceColConfig.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.ceColConfig.Properties.Appearance.Options.UseFont = true;
            this.ceColConfig.Properties.Appearance.Options.UseForeColor = true;
            this.ceColConfig.Properties.Caption = "采集功能配置";
            this.ceColConfig.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.ceColConfig.Size = new System.Drawing.Size(104, 19);
            this.ceColConfig.TabIndex = 0;
            this.ceColConfig.CheckedChanged += new System.EventHandler(this.ceColConfig_CheckedChanged);
            // 
            // ceCollectRS485
            // 
            this.ceCollectRS485.Location = new System.Drawing.Point(4, 2);
            this.ceCollectRS485.Name = "ceCollectRS485";
            this.ceCollectRS485.Properties.Caption = "采集流量/RS485";
            this.ceCollectRS485.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.ceCollectRS485.Size = new System.Drawing.Size(117, 19);
            this.ceCollectRS485.TabIndex = 3;
            this.ceCollectRS485.CheckedChanged += new System.EventHandler(this.ceCollectRS485_CheckedChanged);
            // 
            // btnEnableCollect
            // 
            this.btnEnableCollect.Location = new System.Drawing.Point(329, 433);
            this.btnEnableCollect.Name = "btnEnableCollect";
            this.btnEnableCollect.Size = new System.Drawing.Size(82, 26);
            this.btnEnableCollect.TabIndex = 7;
            this.btnEnableCollect.Text = "启动采集";
            this.btnEnableCollect.Click += new System.EventHandler(this.btnEnableCollect_Click);
            // 
            // btnCheckingTime
            // 
            this.btnCheckingTime.Location = new System.Drawing.Point(329, 464);
            this.btnCheckingTime.Name = "btnCheckingTime";
            this.btnCheckingTime.Size = new System.Drawing.Size(82, 26);
            this.btnCheckingTime.TabIndex = 13;
            this.btnCheckingTime.Text = "校时";
            this.btnCheckingTime.Click += new System.EventHandler(this.btnCheckingTime_Click);
            // 
            // btnReadParm
            // 
            this.btnReadParm.Location = new System.Drawing.Point(415, 433);
            this.btnReadParm.Name = "btnReadParm";
            this.btnReadParm.Size = new System.Drawing.Size(82, 26);
            this.btnReadParm.TabIndex = 8;
            this.btnReadParm.Text = "读取设备参数";
            this.btnReadParm.Click += new System.EventHandler(this.btnReadParm_Click);
            // 
            // btnSetParm
            // 
            this.btnSetParm.Location = new System.Drawing.Point(415, 464);
            this.btnSetParm.Name = "btnSetParm";
            this.btnSetParm.Size = new System.Drawing.Size(82, 26);
            this.btnSetParm.TabIndex = 14;
            this.btnSetParm.Text = "设置设备参数";
            this.btnSetParm.Click += new System.EventHandler(this.btnSetParm_Click);
            // 
            // gridControl_Simulate
            // 
            this.gridControl_Simulate.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_Simulate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_Simulate.Location = new System.Drawing.Point(2, 21);
            this.gridControl_Simulate.MainView = this.gridView_Simulate;
            this.gridControl_Simulate.Name = "gridControl_Simulate";
            this.gridControl_Simulate.Size = new System.Drawing.Size(543, 138);
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
            this.gridColumn1.Width = 65;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "采集时间间隔1(s)";
            this.gridColumn2.ColumnEdit = this.cb_sim_coltime1;
            this.gridColumn2.FieldName = "collecttime1";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn2.OptionsFilter.AllowFilter = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 95;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "采集时间间隔2(s)";
            this.gridColumn8.ColumnEdit = this.cb_sim_coltime2;
            this.gridColumn8.FieldName = "collecttime2";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn8.OptionsFilter.AllowFilter = false;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 2;
            this.gridColumn8.Width = 95;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "发送时间间隔(m)";
            this.gridColumn3.ColumnEdit = this.cb_sim_sendtime;
            this.gridColumn3.FieldName = "sendtime";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn3.OptionsFilter.AllowFilter = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
            this.gridColumn3.Width = 99;
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.ceCollectRS485);
            this.groupControl3.Controls.Add(this.gridControl_RS485);
            this.groupControl3.Location = new System.Drawing.Point(339, 7);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(208, 154);
            this.groupControl3.TabIndex = 3;
            // 
            // gridControl_RS485
            // 
            this.gridControl_RS485.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_RS485.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_RS485.Location = new System.Drawing.Point(2, 21);
            this.gridControl_RS485.MainView = this.gridView_RS485;
            this.gridControl_RS485.Name = "gridControl_RS485";
            this.gridControl_RS485.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cb_RS485_starttime,
            this.cb_RS485_coltime,
            this.cb_RS485_sendtime});
            this.gridControl_RS485.Size = new System.Drawing.Size(204, 131);
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
            this.gridView_RS485.IndicatorWidth = 20;
            this.gridView_RS485.Name = "gridView_RS485";
            this.gridView_RS485.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView_RS485.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.Inplace;
            this.gridView_RS485.OptionsCustomization.AllowColumnMoving = false;
            this.gridView_RS485.OptionsCustomization.AllowGroup = false;
            this.gridView_RS485.OptionsCustomization.AllowSort = false;
            this.gridView_RS485.OptionsFilter.AllowFilterEditor = false;
            this.gridView_RS485.OptionsView.ShowGroupPanel = false;
            this.gridView_RS485.OptionsView.ShowIndicator = false;
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
            this.gridColumn6.Width = 56;
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
            this.gridColumn7.Caption = "采集间隔(s)";
            this.gridColumn7.ColumnEdit = this.cb_RS485_coltime;
            this.gridColumn7.FieldName = "collecttime";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn7.OptionsFilter.AllowFilter = false;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 1;
            this.gridColumn7.Width = 71;
            // 
            // cb_RS485_coltime
            // 
            this.cb_RS485_coltime.AutoHeight = false;
            this.cb_RS485_coltime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_RS485_coltime.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.cb_RS485_coltime.MaxLength = 4;
            this.cb_RS485_coltime.Name = "cb_RS485_coltime";
            // 
            // gridColumn15
            // 
            this.gridColumn15.Caption = "发送间隔(m)";
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
            // SwitchComunication
            // 
            this.SwitchComunication.EditValue = true;
            this.SwitchComunication.Location = new System.Drawing.Point(10, 434);
            this.SwitchComunication.Name = "SwitchComunication";
            this.SwitchComunication.Properties.OffText = "GPRS";
            this.SwitchComunication.Properties.OnText = "串口";
            this.SwitchComunication.Size = new System.Drawing.Size(136, 25);
            this.SwitchComunication.TabIndex = 15;
            this.SwitchComunication.Click += new System.EventHandler(this.SwitchComunication_Click);
            // 
            // btnSetPluseBasic
            // 
            this.btnSetPluseBasic.Location = new System.Drawing.Point(243, 464);
            this.btnSetPluseBasic.Name = "btnSetPluseBasic";
            this.btnSetPluseBasic.Size = new System.Drawing.Size(82, 26);
            this.btnSetPluseBasic.TabIndex = 12;
            this.btnSetPluseBasic.Text = "设置脉冲基准";
            this.btnSetPluseBasic.Click += new System.EventHandler(this.btnSetPluseBasic_Click);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(2, 21);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(553, 190);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.xtraTabPage4});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.groupControl2);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(547, 161);
            this.xtraTabPage1.Text = "模拟量";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.ceCollectSimulate);
            this.groupControl2.Controls.Add(this.gridControl_Simulate);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 0);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(547, 161);
            this.groupControl2.TabIndex = 1;
            // 
            // ceCollectSimulate
            // 
            this.ceCollectSimulate.Location = new System.Drawing.Point(2, -1);
            this.ceCollectSimulate.Name = "ceCollectSimulate";
            this.ceCollectSimulate.Properties.Caption = "采集模拟量";
            this.ceCollectSimulate.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.ceCollectSimulate.Size = new System.Drawing.Size(85, 19);
            this.ceCollectSimulate.TabIndex = 0;
            this.ceCollectSimulate.CheckedChanged += new System.EventHandler(this.ceCollectSimulate_CheckedChanged);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.groupControl4);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(547, 161);
            this.xtraTabPage2.Text = "modbus协议";
            // 
            // groupControl4
            // 
            this.groupControl4.Controls.Add(this.ceCollectModbus);
            this.groupControl4.Controls.Add(this.gridControl_485protocol);
            this.groupControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl4.Location = new System.Drawing.Point(0, 0);
            this.groupControl4.Name = "groupControl4";
            this.groupControl4.Size = new System.Drawing.Size(547, 161);
            this.groupControl4.TabIndex = 6;
            // 
            // ceCollectModbus
            // 
            this.ceCollectModbus.Location = new System.Drawing.Point(3, 0);
            this.ceCollectModbus.Name = "ceCollectModbus";
            this.ceCollectModbus.Properties.Caption = "RS485采集modbus协议配置";
            this.ceCollectModbus.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.ceCollectModbus.Size = new System.Drawing.Size(177, 19);
            this.ceCollectModbus.TabIndex = 6;
            this.ceCollectModbus.CheckedChanged += new System.EventHandler(this.ceCollectModbus_CheckedChanged);
            // 
            // gridControl_485protocol
            // 
            this.gridControl_485protocol.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_485protocol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_485protocol.Location = new System.Drawing.Point(2, 21);
            this.gridControl_485protocol.MainView = this.gridView_485protocol;
            this.gridControl_485protocol.Name = "gridControl_485protocol";
            this.gridControl_485protocol.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.txt_485protocol_ID,
            this.txt_485protocol_funcode,
            this.txt_485protocol_regbeginaddr,
            this.txt_485protocol_regcount,
            this.cb_485protocol_baud});
            this.gridControl_485protocol.Size = new System.Drawing.Size(543, 138);
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
            this.gridView_485protocol.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gridView_485protocol_CustomRowCellEdit);
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
            // xtraTabPage4
            // 
            this.xtraTabPage4.Controls.Add(this.groupControl5);
            this.xtraTabPage4.Controls.Add(this.groupControl3);
            this.xtraTabPage4.Name = "xtraTabPage4";
            this.xtraTabPage4.Size = new System.Drawing.Size(547, 161);
            this.xtraTabPage4.Text = "脉冲量/RS485间隔";
            // 
            // groupControl5
            // 
            this.groupControl5.Controls.Add(this.ceCollectPluse);
            this.groupControl5.Controls.Add(this.gridControl_Pluse);
            this.groupControl5.Location = new System.Drawing.Point(2, 7);
            this.groupControl5.Name = "groupControl5";
            this.groupControl5.Size = new System.Drawing.Size(333, 154);
            this.groupControl5.TabIndex = 4;
            // 
            // ceCollectPluse
            // 
            this.ceCollectPluse.Location = new System.Drawing.Point(2, 2);
            this.ceCollectPluse.Name = "ceCollectPluse";
            this.ceCollectPluse.Properties.Caption = "采集压力/脉冲量";
            this.ceCollectPluse.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.ceCollectPluse.Size = new System.Drawing.Size(110, 19);
            this.ceCollectPluse.TabIndex = 2;
            this.ceCollectPluse.CheckedChanged += new System.EventHandler(this.ceCollectPluse_CheckedChanged);
            // 
            // gridControl_Pluse
            // 
            this.gridControl_Pluse.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_Pluse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_Pluse.Location = new System.Drawing.Point(2, 21);
            this.gridControl_Pluse.MainView = this.gridView_Pluse;
            this.gridControl_Pluse.Name = "gridControl_Pluse";
            this.gridControl_Pluse.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cb_pluse_starttime,
            this.cb_pluse_coltime,
            this.cb_pluse_sendtime,
            this.cb_pre_coltime});
            this.gridControl_Pluse.Size = new System.Drawing.Size(329, 131);
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
            this.gridColumn9,
            this.gridColumn17});
            this.gridView_Pluse.GridControl = this.gridControl_Pluse;
            this.gridView_Pluse.IndicatorWidth = 20;
            this.gridView_Pluse.Name = "gridView_Pluse";
            this.gridView_Pluse.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView_Pluse.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.Inplace;
            this.gridView_Pluse.OptionsCustomization.AllowColumnMoving = false;
            this.gridView_Pluse.OptionsCustomization.AllowGroup = false;
            this.gridView_Pluse.OptionsCustomization.AllowSort = false;
            this.gridView_Pluse.OptionsFilter.AllowFilterEditor = false;
            this.gridView_Pluse.OptionsView.ShowGroupPanel = false;
            this.gridView_Pluse.OptionsView.ShowIndicator = false;
            this.gridView_Pluse.OptionsView.ShowPreviewRowLines = DevExpress.Utils.DefaultBoolean.True;
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
            this.gridColumn5.Width = 58;
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
            this.gridColumn16.Caption = "压力采集间隔(s)";
            this.gridColumn16.ColumnEdit = this.cb_pre_coltime;
            this.gridColumn16.FieldName = "precollecttime";
            this.gridColumn16.Name = "gridColumn16";
            this.gridColumn16.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn16.OptionsFilter.AllowFilter = false;
            this.gridColumn16.Visible = true;
            this.gridColumn16.VisibleIndex = 1;
            this.gridColumn16.Width = 97;
            // 
            // cb_pre_coltime
            // 
            this.cb_pre_coltime.AutoHeight = false;
            this.cb_pre_coltime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_pre_coltime.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.cb_pre_coltime.MaxLength = 4;
            this.cb_pre_coltime.Name = "cb_pre_coltime";
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "脉冲采集间隔(s)";
            this.gridColumn9.ColumnEdit = this.cb_pluse_coltime;
            this.gridColumn9.FieldName = "plusecollecttime";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn9.OptionsFilter.AllowFilter = false;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 2;
            this.gridColumn9.Width = 97;
            // 
            // cb_pluse_coltime
            // 
            this.cb_pluse_coltime.AutoHeight = false;
            this.cb_pluse_coltime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_pluse_coltime.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.cb_pluse_coltime.MaxLength = 4;
            this.cb_pluse_coltime.Name = "cb_pluse_coltime";
            // 
            // gridColumn17
            // 
            this.gridColumn17.Caption = "发送间隔(m)";
            this.gridColumn17.ColumnEdit = this.cb_pluse_sendtime;
            this.gridColumn17.FieldName = "sendtime";
            this.gridColumn17.Name = "gridColumn17";
            this.gridColumn17.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn17.OptionsFilter.AllowFilter = false;
            this.gridColumn17.Visible = true;
            this.gridColumn17.VisibleIndex = 3;
            this.gridColumn17.Width = 78;
            // 
            // cb_pluse_sendtime
            // 
            this.cb_pluse_sendtime.AutoHeight = false;
            this.cb_pluse_sendtime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_pluse_sendtime.Name = "cb_pluse_sendtime";
            // 
            // groupControl8
            // 
            this.groupControl8.Controls.Add(this.cbSlopLowLimitEnable);
            this.groupControl8.Controls.Add(this.cbSlopUpLimitEnable);
            this.groupControl8.Controls.Add(this.cbPreLowLimitEnable);
            this.groupControl8.Controls.Add(this.cePreRange);
            this.groupControl8.Controls.Add(this.cbPreUpLimitEnable);
            this.groupControl8.Controls.Add(this.txtSlopLowLimit);
            this.groupControl8.Controls.Add(this.txtOffsetBaseV);
            this.groupControl8.Controls.Add(this.cbPreFlag);
            this.groupControl8.Controls.Add(this.ceOffsetBaseV);
            this.groupControl8.Controls.Add(this.txtSlopUpLimit);
            this.groupControl8.Controls.Add(this.ceSlopLowLimitEnable);
            this.groupControl8.Controls.Add(this.txtPreRange);
            this.groupControl8.Controls.Add(this.txtPreLowLimit);
            this.groupControl8.Controls.Add(this.ceSlopUpLimitEnable);
            this.groupControl8.Controls.Add(this.txtPreUpLimit);
            this.groupControl8.Controls.Add(this.ceSlopLowLimit);
            this.groupControl8.Controls.Add(this.cePreLowLimitEnable);
            this.groupControl8.Controls.Add(this.ceSlopUpLimit);
            this.groupControl8.Controls.Add(this.cePreLowLimit);
            this.groupControl8.Controls.Add(this.cePreUpLimitEnable);
            this.groupControl8.Controls.Add(this.cePreUpLimit);
            this.groupControl8.Controls.Add(this.groupControl12);
            this.groupControl8.Controls.Add(this.groupControl11);
            this.groupControl8.Controls.Add(this.groupControl10);
            this.groupControl8.Controls.Add(this.groupControl9);
            this.groupControl8.Location = new System.Drawing.Point(6, 106);
            this.groupControl8.Name = "groupControl8";
            this.groupControl8.Size = new System.Drawing.Size(788, 76);
            this.groupControl8.TabIndex = 0;
            this.groupControl8.Text = "采集/阀值参数";
            // 
            // cbSlopLowLimitEnable
            // 
            this.cbSlopLowLimitEnable.Location = new System.Drawing.Point(722, 48);
            this.cbSlopLowLimitEnable.Name = "cbSlopLowLimitEnable";
            this.cbSlopLowLimitEnable.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbSlopLowLimitEnable.Properties.Items.AddRange(new object[] {
            "关闭",
            "开启"});
            this.cbSlopLowLimitEnable.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbSlopLowLimitEnable.Size = new System.Drawing.Size(56, 20);
            this.cbSlopLowLimitEnable.TabIndex = 19;
            // 
            // cbSlopUpLimitEnable
            // 
            this.cbSlopUpLimitEnable.Location = new System.Drawing.Point(559, 48);
            this.cbSlopUpLimitEnable.Name = "cbSlopUpLimitEnable";
            this.cbSlopUpLimitEnable.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbSlopUpLimitEnable.Properties.Items.AddRange(new object[] {
            "关闭",
            "开启"});
            this.cbSlopUpLimitEnable.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbSlopUpLimitEnable.Size = new System.Drawing.Size(56, 20);
            this.cbSlopUpLimitEnable.TabIndex = 15;
            // 
            // cbPreLowLimitEnable
            // 
            this.cbPreLowLimitEnable.Location = new System.Drawing.Point(396, 48);
            this.cbPreLowLimitEnable.Name = "cbPreLowLimitEnable";
            this.cbPreLowLimitEnable.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbPreLowLimitEnable.Properties.Items.AddRange(new object[] {
            "关闭",
            "开启"});
            this.cbPreLowLimitEnable.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbPreLowLimitEnable.Size = new System.Drawing.Size(56, 20);
            this.cbPreLowLimitEnable.TabIndex = 11;
            // 
            // cePreRange
            // 
            this.cePreRange.Location = new System.Drawing.Point(17, 28);
            this.cePreRange.Name = "cePreRange";
            this.cePreRange.Properties.Caption = "量程:";
            this.cePreRange.Size = new System.Drawing.Size(62, 19);
            this.cePreRange.TabIndex = 0;
            // 
            // cbPreUpLimitEnable
            // 
            this.cbPreUpLimitEnable.Location = new System.Drawing.Point(238, 49);
            this.cbPreUpLimitEnable.Name = "cbPreUpLimitEnable";
            this.cbPreUpLimitEnable.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbPreUpLimitEnable.Properties.Items.AddRange(new object[] {
            "关闭",
            "开启"});
            this.cbPreUpLimitEnable.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbPreUpLimitEnable.Size = new System.Drawing.Size(56, 20);
            this.cbPreUpLimitEnable.TabIndex = 7;
            // 
            // txtSlopLowLimit
            // 
            this.txtSlopLowLimit.EditValue = "";
            this.txtSlopLowLimit.Location = new System.Drawing.Point(722, 24);
            this.txtSlopLowLimit.Name = "txtSlopLowLimit";
            this.txtSlopLowLimit.Size = new System.Drawing.Size(56, 20);
            this.txtSlopLowLimit.TabIndex = 17;
            // 
            // txtOffsetBaseV
            // 
            this.txtOffsetBaseV.Location = new System.Drawing.Point(84, 51);
            this.txtOffsetBaseV.Name = "txtOffsetBaseV";
            this.txtOffsetBaseV.Size = new System.Drawing.Size(56, 20);
            this.txtOffsetBaseV.TabIndex = 3;
            // 
            // cbPreFlag
            // 
            this.cbPreFlag.Location = new System.Drawing.Point(92, 2);
            this.cbPreFlag.Name = "cbPreFlag";
            this.cbPreFlag.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbPreFlag.Properties.Items.AddRange(new object[] {
            "压力1-01",
            "压力2-02",
            "模拟量1-03",
            "模拟量2-04",
            "流量-05",
            "分体式液位-06"});
            this.cbPreFlag.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbPreFlag.Size = new System.Drawing.Size(108, 20);
            this.cbPreFlag.TabIndex = 20;
            this.cbPreFlag.SelectedIndexChanged += new System.EventHandler(this.cbPreFlag_SelectedIndexChanged);
            // 
            // ceOffsetBaseV
            // 
            this.ceOffsetBaseV.Location = new System.Drawing.Point(17, 52);
            this.ceOffsetBaseV.Name = "ceOffsetBaseV";
            this.ceOffsetBaseV.Properties.Caption = "偏移量:";
            this.ceOffsetBaseV.Size = new System.Drawing.Size(70, 19);
            this.ceOffsetBaseV.TabIndex = 2;
            // 
            // txtSlopUpLimit
            // 
            this.txtSlopUpLimit.Location = new System.Drawing.Point(559, 24);
            this.txtSlopUpLimit.Name = "txtSlopUpLimit";
            this.txtSlopUpLimit.Size = new System.Drawing.Size(56, 20);
            this.txtSlopUpLimit.TabIndex = 13;
            // 
            // ceSlopLowLimitEnable
            // 
            this.ceSlopLowLimitEnable.Location = new System.Drawing.Point(636, 49);
            this.ceSlopLowLimitEnable.Name = "ceSlopLowLimitEnable";
            this.ceSlopLowLimitEnable.Properties.Caption = "投退状态:";
            this.ceSlopLowLimitEnable.Size = new System.Drawing.Size(74, 19);
            this.ceSlopLowLimitEnable.TabIndex = 18;
            // 
            // txtPreRange
            // 
            this.txtPreRange.Location = new System.Drawing.Point(84, 27);
            this.txtPreRange.Name = "txtPreRange";
            this.txtPreRange.Size = new System.Drawing.Size(56, 20);
            this.txtPreRange.TabIndex = 1;
            // 
            // txtPreLowLimit
            // 
            this.txtPreLowLimit.Location = new System.Drawing.Point(396, 25);
            this.txtPreLowLimit.Name = "txtPreLowLimit";
            this.txtPreLowLimit.Size = new System.Drawing.Size(56, 20);
            this.txtPreLowLimit.TabIndex = 9;
            // 
            // ceSlopUpLimitEnable
            // 
            this.ceSlopUpLimitEnable.Location = new System.Drawing.Point(473, 49);
            this.ceSlopUpLimitEnable.Name = "ceSlopUpLimitEnable";
            this.ceSlopUpLimitEnable.Properties.Caption = "投退状态:";
            this.ceSlopUpLimitEnable.Size = new System.Drawing.Size(74, 19);
            this.ceSlopUpLimitEnable.TabIndex = 14;
            // 
            // txtPreUpLimit
            // 
            this.txtPreUpLimit.Location = new System.Drawing.Point(238, 25);
            this.txtPreUpLimit.Name = "txtPreUpLimit";
            this.txtPreUpLimit.Size = new System.Drawing.Size(56, 20);
            this.txtPreUpLimit.TabIndex = 5;
            // 
            // ceSlopLowLimit
            // 
            this.ceSlopLowLimit.Location = new System.Drawing.Point(636, 25);
            this.ceSlopLowLimit.Name = "ceSlopLowLimit";
            this.ceSlopLowLimit.Properties.Caption = "斜率下限值:";
            this.ceSlopLowLimit.Size = new System.Drawing.Size(87, 19);
            this.ceSlopLowLimit.TabIndex = 16;
            // 
            // cePreLowLimitEnable
            // 
            this.cePreLowLimitEnable.Location = new System.Drawing.Point(314, 50);
            this.cePreLowLimitEnable.Name = "cePreLowLimitEnable";
            this.cePreLowLimitEnable.Properties.Caption = "投退状态:";
            this.cePreLowLimitEnable.Size = new System.Drawing.Size(74, 19);
            this.cePreLowLimitEnable.TabIndex = 10;
            // 
            // ceSlopUpLimit
            // 
            this.ceSlopUpLimit.Location = new System.Drawing.Point(473, 25);
            this.ceSlopUpLimit.Name = "ceSlopUpLimit";
            this.ceSlopUpLimit.Properties.Caption = "斜率上限值:";
            this.ceSlopUpLimit.Size = new System.Drawing.Size(87, 19);
            this.ceSlopUpLimit.TabIndex = 12;
            // 
            // cePreLowLimit
            // 
            this.cePreLowLimit.Location = new System.Drawing.Point(314, 26);
            this.cePreLowLimit.Name = "cePreLowLimit";
            this.cePreLowLimit.Properties.Caption = "下限值:";
            this.cePreLowLimit.Size = new System.Drawing.Size(74, 19);
            this.cePreLowLimit.TabIndex = 8;
            // 
            // cePreUpLimitEnable
            // 
            this.cePreUpLimitEnable.Location = new System.Drawing.Point(157, 48);
            this.cePreUpLimitEnable.Name = "cePreUpLimitEnable";
            this.cePreUpLimitEnable.Properties.Caption = "投退状态:";
            this.cePreUpLimitEnable.Size = new System.Drawing.Size(74, 19);
            this.cePreUpLimitEnable.TabIndex = 6;
            // 
            // cePreUpLimit
            // 
            this.cePreUpLimit.Location = new System.Drawing.Point(157, 26);
            this.cePreUpLimit.Name = "cePreUpLimit";
            this.cePreUpLimit.Properties.Caption = "上限值:";
            this.cePreUpLimit.Size = new System.Drawing.Size(74, 19);
            this.cePreUpLimit.TabIndex = 4;
            // 
            // groupControl12
            // 
            this.groupControl12.Location = new System.Drawing.Point(149, 0);
            this.groupControl12.Name = "groupControl12";
            this.groupControl12.Size = new System.Drawing.Size(2, 80);
            this.groupControl12.TabIndex = 27;
            this.groupControl12.Text = "groupControl9";
            // 
            // groupControl11
            // 
            this.groupControl11.Location = new System.Drawing.Point(625, 0);
            this.groupControl11.Name = "groupControl11";
            this.groupControl11.Size = new System.Drawing.Size(2, 80);
            this.groupControl11.TabIndex = 27;
            this.groupControl11.Text = "groupControl9";
            // 
            // groupControl10
            // 
            this.groupControl10.Location = new System.Drawing.Point(463, 0);
            this.groupControl10.Name = "groupControl10";
            this.groupControl10.Size = new System.Drawing.Size(2, 80);
            this.groupControl10.TabIndex = 27;
            this.groupControl10.Text = "groupControl9";
            // 
            // groupControl9
            // 
            this.groupControl9.Location = new System.Drawing.Point(305, 0);
            this.groupControl9.Name = "groupControl9";
            this.groupControl9.Size = new System.Drawing.Size(2, 80);
            this.groupControl9.TabIndex = 27;
            this.groupControl9.Text = "groupControl9";
            // 
            // dropbtnCalibrationSimualte
            // 
            this.dropbtnCalibrationSimualte.DropDownControl = this.popupMenuCalibration;
            this.dropbtnCalibrationSimualte.Location = new System.Drawing.Point(243, 434);
            this.dropbtnCalibrationSimualte.Name = "dropbtnCalibrationSimualte";
            this.dropbtnCalibrationSimualte.Size = new System.Drawing.Size(82, 26);
            this.dropbtnCalibrationSimualte.TabIndex = 6;
            this.dropbtnCalibrationSimualte.Text = "模拟量校准";
            // 
            // popupMenuCalibration
            // 
            this.popupMenuCalibration.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barbtnCalibrationSimualte1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barbtnCalibrationSimualte2)});
            this.popupMenuCalibration.Manager = this.barManager1;
            this.popupMenuCalibration.Name = "popupMenuCalibration";
            // 
            // barbtnCalibrationSimualte1
            // 
            this.barbtnCalibrationSimualte1.Caption = "第一路校准";
            this.barbtnCalibrationSimualte1.Id = 0;
            this.barbtnCalibrationSimualte1.Name = "barbtnCalibrationSimualte1";
            this.barbtnCalibrationSimualte1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnCalibrationSimualte1_ItemClick);
            // 
            // barbtnCalibrationSimualte2
            // 
            this.barbtnCalibrationSimualte2.Caption = "第二路校准";
            this.barbtnCalibrationSimualte2.Id = 1;
            this.barbtnCalibrationSimualte2.Name = "barbtnCalibrationSimualte2";
            this.barbtnCalibrationSimualte2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barbtnCalibrationSimualte2_ItemClick);
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barbtnCalibrationSimualte1,
            this.barbtnCalibrationSimualte2,
            this.btnStartAlarm,
            this.btnStopAlarm,
            this.btnReset1,
            this.btnReset2,
            this.btnReset3});
            this.barManager1.MaxItemId = 7;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(797, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 494);
            this.barDockControlBottom.Size = new System.Drawing.Size(797, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 494);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(797, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 494);
            // 
            // btnStartAlarm
            // 
            this.btnStartAlarm.Caption = "启用报警";
            this.btnStartAlarm.Id = 2;
            this.btnStartAlarm.Name = "btnStartAlarm";
            // 
            // btnStopAlarm
            // 
            this.btnStopAlarm.Caption = "取消报警";
            this.btnStopAlarm.Id = 3;
            this.btnStopAlarm.Name = "btnStopAlarm";
            // 
            // btnReset1
            // 
            this.btnReset1.Caption = "恢复所有出厂设置";
            this.btnReset1.Id = 4;
            this.btnReset1.Name = "btnReset1";
            this.btnReset1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnReset_ItemClick);
            // 
            // btnReset2
            // 
            this.btnReset2.Caption = "恢复除IP端口外设置";
            this.btnReset2.Id = 5;
            this.btnReset2.Name = "btnReset2";
            this.btnReset2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnReset_ItemClick);
            // 
            // btnReset3
            // 
            this.btnReset3.Caption = "系统复位";
            this.btnReset3.Id = 6;
            this.btnReset3.Name = "btnReset3";
            this.btnReset3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnReset_ItemClick);
            // 
            // groupControl6
            // 
            this.groupControl6.Controls.Add(this.treeSocketType);
            this.groupControl6.Controls.Add(this.btnCallClose);
            this.groupControl6.Controls.Add(this.btnCallOpen);
            this.groupControl6.Controls.Add(this.btnCallData);
            this.groupControl6.Location = new System.Drawing.Point(506, 428);
            this.groupControl6.Name = "groupControl6";
            this.groupControl6.Size = new System.Drawing.Size(286, 61);
            this.groupControl6.TabIndex = 15;
            this.groupControl6.Text = "招测";
            // 
            // treeSocketType
            // 
            this.treeSocketType.Location = new System.Drawing.Point(118, 27);
            this.treeSocketType.Name = "treeSocketType";
            this.treeSocketType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.treeSocketType.Properties.NullText = "请选择";
            this.treeSocketType.Properties.TreeList = this.treeList1;
            this.treeSocketType.Size = new System.Drawing.Size(98, 20);
            this.treeSocketType.TabIndex = 2;
            // 
            // treeList1
            // 
            this.treeList1.Location = new System.Drawing.Point(180, -44);
            this.treeList1.Name = "treeList1";
            this.treeList1.OptionsBehavior.Editable = false;
            this.treeList1.OptionsFilter.AllowFilterEditor = false;
            this.treeList1.OptionsFind.ShowFindButton = false;
            this.treeList1.OptionsLayout.AddNewColumns = false;
            this.treeList1.OptionsMenu.EnableColumnMenu = false;
            this.treeList1.OptionsMenu.EnableFooterMenu = false;
            this.treeList1.OptionsMenu.ShowAutoFilterRowItem = false;
            this.treeList1.OptionsView.ShowCheckBoxes = true;
            this.treeList1.OptionsView.ShowColumns = false;
            this.treeList1.OptionsView.ShowIndentAsRowStyle = true;
            this.treeList1.OptionsView.ShowIndicator = false;
            this.treeList1.OptionsView.ShowRoot = false;
            this.treeList1.Size = new System.Drawing.Size(100, 160);
            this.treeList1.TabIndex = 0;
            this.treeList1.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.Light;
            // 
            // btnCallClose
            // 
            this.btnCallClose.Location = new System.Drawing.Point(58, 21);
            this.btnCallClose.Name = "btnCallClose";
            this.btnCallClose.Size = new System.Drawing.Size(50, 26);
            this.btnCallClose.TabIndex = 1;
            this.btnCallClose.Text = "招测关";
            this.btnCallClose.Click += new System.EventHandler(this.btnCallClose_Click);
            // 
            // btnCallOpen
            // 
            this.btnCallOpen.Location = new System.Drawing.Point(5, 21);
            this.btnCallOpen.Name = "btnCallOpen";
            this.btnCallOpen.Size = new System.Drawing.Size(50, 26);
            this.btnCallOpen.TabIndex = 0;
            this.btnCallOpen.Text = "招测开";
            this.btnCallOpen.Click += new System.EventHandler(this.btnCallOpen_Click);
            // 
            // btnCallData
            // 
            this.btnCallData.Enabled = false;
            this.btnCallData.Location = new System.Drawing.Point(222, 24);
            this.btnCallData.Name = "btnCallData";
            this.btnCallData.Size = new System.Drawing.Size(58, 26);
            this.btnCallData.TabIndex = 3;
            this.btnCallData.Text = "招测";
            this.btnCallData.Click += new System.EventHandler(this.btnCallData_Click);
            // 
            // groupControl13
            // 
            this.groupControl13.Controls.Add(this.xtraTabControl1);
            this.groupControl13.Location = new System.Drawing.Point(6, 215);
            this.groupControl13.Name = "groupControl13";
            this.groupControl13.Size = new System.Drawing.Size(557, 213);
            this.groupControl13.TabIndex = 3;
            this.groupControl13.Text = "时间间隔";
            // 
            // btnVer
            // 
            this.btnVer.Location = new System.Drawing.Point(157, 464);
            this.btnVer.Name = "btnVer";
            this.btnVer.Size = new System.Drawing.Size(82, 26);
            this.btnVer.TabIndex = 11;
            this.btnVer.Text = "版本号";
            this.btnVer.Click += new System.EventHandler(this.btnVer_Click);
            // 
            // btnFieldStrength
            // 
            this.btnFieldStrength.Location = new System.Drawing.Point(157, 434);
            this.btnFieldStrength.Name = "btnFieldStrength";
            this.btnFieldStrength.Size = new System.Drawing.Size(82, 26);
            this.btnFieldStrength.TabIndex = 5;
            this.btnFieldStrength.Text = "场强\\电压";
            this.btnFieldStrength.Click += new System.EventHandler(this.btnFieldStrength_Click);
            // 
            // timer_GetWaitCmd
            // 
            this.timer_GetWaitCmd.Interval = 10000;
            // 
            // gridControl_WaitCmd
            // 
            this.gridControl_WaitCmd.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_WaitCmd.Dock = System.Windows.Forms.DockStyle.Top;
            this.gridControl_WaitCmd.Enabled = false;
            this.gridControl_WaitCmd.Location = new System.Drawing.Point(2, 21);
            this.gridControl_WaitCmd.MainView = this.gridView_WaitCmd;
            this.gridControl_WaitCmd.Name = "gridControl_WaitCmd";
            this.gridControl_WaitCmd.Size = new System.Drawing.Size(221, 149);
            this.gridControl_WaitCmd.TabIndex = 0;
            this.gridControl_WaitCmd.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView_WaitCmd,
            this.gridView1});
            // 
            // gridView_WaitCmd
            // 
            this.gridView_WaitCmd.ActiveFilterEnabled = false;
            this.gridView_WaitCmd.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn18,
            this.gridColumn20,
            this.gridColumn21,
            this.gridColumn10,
            this.类型});
            this.gridView_WaitCmd.GridControl = this.gridControl_WaitCmd;
            this.gridView_WaitCmd.Name = "gridView_WaitCmd";
            this.gridView_WaitCmd.OptionsBehavior.AlignGroupSummaryInGroupRow = DevExpress.Utils.DefaultBoolean.False;
            this.gridView_WaitCmd.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView_WaitCmd.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.gridView_WaitCmd.OptionsBehavior.Editable = false;
            this.gridView_WaitCmd.OptionsCustomization.AllowColumnMoving = false;
            this.gridView_WaitCmd.OptionsCustomization.AllowFilter = false;
            this.gridView_WaitCmd.OptionsCustomization.AllowGroup = false;
            this.gridView_WaitCmd.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridView_WaitCmd.OptionsCustomization.AllowSort = false;
            this.gridView_WaitCmd.OptionsFilter.AllowFilterEditor = false;
            this.gridView_WaitCmd.OptionsSelection.MultiSelect = true;
            this.gridView_WaitCmd.OptionsView.ShowGroupPanel = false;
            this.gridView_WaitCmd.OptionsView.ShowIndicator = false;
            // 
            // gridColumn18
            // 
            this.gridColumn18.Caption = "类型";
            this.gridColumn18.FieldName = "StrDevType";
            this.gridColumn18.Name = "gridColumn18";
            this.gridColumn18.OptionsColumn.AllowEdit = false;
            this.gridColumn18.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn18.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn18.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn18.OptionsFilter.AllowFilter = false;
            this.gridColumn18.Visible = true;
            this.gridColumn18.VisibleIndex = 3;
            this.gridColumn18.Width = 56;
            // 
            // gridColumn20
            // 
            this.gridColumn20.Caption = "终端号";
            this.gridColumn20.FieldName = "ID";
            this.gridColumn20.Name = "gridColumn20";
            this.gridColumn20.Visible = true;
            this.gridColumn20.VisibleIndex = 0;
            this.gridColumn20.Width = 48;
            // 
            // gridColumn21
            // 
            this.gridColumn21.Caption = "功能码";
            this.gridColumn21.FieldName = "Funcode";
            this.gridColumn21.Name = "gridColumn21";
            this.gridColumn21.Visible = true;
            this.gridColumn21.VisibleIndex = 1;
            this.gridColumn21.Width = 46;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "数据";
            this.gridColumn10.FieldName = "Data";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.AllowEdit = false;
            this.gridColumn10.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn10.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn10.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn10.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn10.OptionsFilter.AllowFilter = false;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 2;
            this.gridColumn10.Width = 53;
            // 
            // 类型
            // 
            this.类型.Caption = "gridColumn19";
            this.类型.FieldName = "DevType";
            this.类型.Name = "类型";
            this.类型.OptionsColumn.AllowEdit = false;
            this.类型.OptionsColumn.AllowFocus = false;
            this.类型.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.True;
            this.类型.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.类型.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.类型.Width = 20;
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl_WaitCmd;
            this.gridView1.Name = "gridView1";
            // 
            // btnDel
            // 
            this.btnDel.Enabled = false;
            this.btnDel.Location = new System.Drawing.Point(130, 176);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(67, 26);
            this.btnDel.TabIndex = 2;
            this.btnDel.Text = "删除";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // groupControl14
            // 
            this.groupControl14.Controls.Add(this.btnRefresh);
            this.groupControl14.Controls.Add(this.btnDel);
            this.groupControl14.Controls.Add(this.gridControl_WaitCmd);
            this.groupControl14.Location = new System.Drawing.Point(569, 215);
            this.groupControl14.Name = "groupControl14";
            this.groupControl14.Size = new System.Drawing.Size(225, 213);
            this.groupControl14.TabIndex = 4;
            this.groupControl14.Text = "待发送指令";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Enabled = false;
            this.btnRefresh.Location = new System.Drawing.Point(30, 176);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(67, 26);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnEnableAlarm
            // 
            this.btnEnableAlarm.Location = new System.Drawing.Point(86, 464);
            this.btnEnableAlarm.Name = "btnEnableAlarm";
            this.btnEnableAlarm.Size = new System.Drawing.Size(69, 26);
            this.btnEnableAlarm.TabIndex = 10;
            this.btnEnableAlarm.Text = "设置报警";
            this.btnEnableAlarm.Click += new System.EventHandler(this.btnEnableAlarm_Click);
            // 
            // dropDownButtonReset
            // 
            this.dropDownButtonReset.DropDownControl = this.popupMenuReset;
            this.dropDownButtonReset.Location = new System.Drawing.Point(6, 464);
            this.dropDownButtonReset.Name = "dropDownButtonReset";
            this.dropDownButtonReset.Size = new System.Drawing.Size(77, 26);
            this.dropDownButtonReset.TabIndex = 9;
            this.dropDownButtonReset.Text = "设备复位";
            // 
            // popupMenuReset
            // 
            this.popupMenuReset.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnReset1),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnReset2),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnReset3)});
            this.popupMenuReset.Manager = this.barManager1;
            this.popupMenuReset.Name = "popupMenuReset";
            // 
            // UniversalTerParm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl14);
            this.Controls.Add(this.groupControl13);
            this.Controls.Add(this.groupControl6);
            this.Controls.Add(this.dropDownButtonReset);
            this.Controls.Add(this.dropbtnCalibrationSimualte);
            this.Controls.Add(this.groupControl8);
            this.Controls.Add(this.SwitchComunication);
            this.Controls.Add(this.btnEnableCollect);
            this.Controls.Add(this.btnSetPluseBasic);
            this.Controls.Add(this.btnFieldStrength);
            this.Controls.Add(this.btnVer);
            this.Controls.Add(this.btnEnableAlarm);
            this.Controls.Add(this.btnCheckingTime);
            this.Controls.Add(this.groupControl7);
            this.Controls.Add(this.btnSetParm);
            this.Controls.Add(this.btnReadParm);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "UniversalTerParm";
            this.Size = new System.Drawing.Size(797, 494);
            this.Load += new System.EventHandler(this.UniversalTerParm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cb_sim_starttime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_sim_coltime1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_sim_coltime2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_sim_sendtime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVolLower.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb485BaudRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVolInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlarmLen.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSMSInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbPluseUnit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbModbusExeFlag.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbNetworkType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceModbusExeFlag.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceNetWorkType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCellPhone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceVolInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ce485Baud.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceHeart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceAlarmLen.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePluseUnit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSMSInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceVolLower.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbComType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceComType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceCellPhone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePort.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceIP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl7)).EndInit();
            this.groupControl7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cePluseState.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceDigitPreStatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceRS485State.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSimulate2State.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSimulate1State.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceColConfig.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceCollectRS485.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_Simulate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_Simulate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_RS485)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_RS485)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_RS485_starttime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_RS485_coltime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_RS485_sendtime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SwitchComunication.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ceCollectSimulate.Properties)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
            this.groupControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ceCollectModbus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_485protocol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_485protocol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_485protocol_baud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_485protocol_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_485protocol_funcode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_485protocol_regbeginaddr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_485protocol_regcount)).EndInit();
            this.xtraTabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).EndInit();
            this.groupControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ceCollectPluse.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_Pluse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_Pluse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_pluse_starttime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_pre_coltime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_pluse_coltime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_pluse_sendtime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl8)).EndInit();
            this.groupControl8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cbSlopLowLimitEnable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbSlopUpLimitEnable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbPreLowLimitEnable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePreRange.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbPreUpLimitEnable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSlopLowLimit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOffsetBaseV.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbPreFlag.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceOffsetBaseV.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSlopUpLimit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSlopLowLimitEnable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPreRange.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPreLowLimit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSlopUpLimitEnable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPreUpLimit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSlopLowLimit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePreLowLimitEnable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSlopUpLimit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePreLowLimit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePreUpLimitEnable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePreUpLimit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuCalibration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).EndInit();
            this.groupControl6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeSocketType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl13)).EndInit();
            this.groupControl13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_WaitCmd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_WaitCmd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl14)).EndInit();
            this.groupControl14.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuReset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private DevExpress.XtraEditors.SimpleButton btnEnableCollect;
        private DevExpress.XtraEditors.SimpleButton btnCheckingTime;
        private DevExpress.XtraEditors.SimpleButton btnReadParm;
        private DevExpress.XtraEditors.SimpleButton btnSetParm;
        private DevExpress.XtraEditors.CheckEdit ceColConfig;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.CheckEdit cePort;
        private DevExpress.XtraEditors.GroupControl groupControl7;
        private DevExpress.XtraEditors.CheckEdit ceCollectRS485;
        private DevExpress.XtraEditors.TextEdit txtTime;
        private DevExpress.XtraGrid.GridControl gridControl_Simulate;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_Simulate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.GridControl gridControl_RS485;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_RS485;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_RS485_starttime;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_RS485_coltime;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn15;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_RS485_sendtime;
        private DevExpress.XtraEditors.ToggleSwitch SwitchComunication;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraEditors.CheckEdit cePluseState;
        private DevExpress.XtraEditors.CheckEdit ceRS485State;
        private DevExpress.XtraEditors.CheckEdit ceSimulate1State;
        private DevExpress.XtraEditors.CheckEdit ceDigitPreStatus;
        private DevExpress.XtraEditors.CheckEdit ceSimulate2State;
        private DevExpress.XtraEditors.SimpleButton btnSetPluseBasic;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.CheckEdit ceCollectSimulate;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage4;
        private DevExpress.XtraEditors.CheckEdit ceNetWorkType;
        private DevExpress.XtraEditors.CheckEdit ceVolInterval;
        private DevExpress.XtraEditors.CheckEdit ce485Baud;
        private DevExpress.XtraEditors.CheckEdit ceHeart;
        private DevExpress.XtraEditors.CheckEdit ceVolLower;
        private DevExpress.XtraEditors.GroupControl groupControl8;
        private DevExpress.XtraEditors.ComboBoxEdit cbSlopLowLimitEnable;
        private DevExpress.XtraEditors.ComboBoxEdit cbSlopUpLimitEnable;
        private DevExpress.XtraEditors.ComboBoxEdit cbPreLowLimitEnable;
        private DevExpress.XtraEditors.ComboBoxEdit cbPreUpLimitEnable;
        private DevExpress.XtraEditors.TextEdit txtOffsetBaseV;
        private DevExpress.XtraEditors.TextEdit txtPreRange;
        private DevExpress.XtraEditors.TextEdit txtSlopLowLimit;
        private DevExpress.XtraEditors.TextEdit txtSlopUpLimit;
        private DevExpress.XtraEditors.CheckEdit ceOffsetBaseV;
        private DevExpress.XtraEditors.CheckEdit ceSlopLowLimitEnable;
        private DevExpress.XtraEditors.TextEdit txtPreLowLimit;
        private DevExpress.XtraEditors.CheckEdit ceSlopUpLimitEnable;
        private DevExpress.XtraEditors.TextEdit txtPreUpLimit;
        private DevExpress.XtraEditors.CheckEdit cePreRange;
        private DevExpress.XtraEditors.CheckEdit ceSlopLowLimit;
        private DevExpress.XtraEditors.CheckEdit cePreLowLimitEnable;
        private DevExpress.XtraEditors.CheckEdit ceSlopUpLimit;
        private DevExpress.XtraEditors.CheckEdit cePreLowLimit;
        private DevExpress.XtraEditors.CheckEdit cePreUpLimitEnable;
        private DevExpress.XtraEditors.CheckEdit cePreUpLimit;
        private DevExpress.XtraEditors.ComboBoxEdit cbNetworkType;
        private DevExpress.XtraEditors.TextEdit txtVolLower;
        private DevExpress.XtraEditors.TextEdit txtHeart;
        private DevExpress.XtraEditors.DropDownButton dropbtnCalibrationSimualte;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_sim_starttime;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_sim_coltime1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_sim_coltime2;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_sim_sendtime;
        private DevExpress.XtraBars.PopupMenu popupMenuCalibration;
        private DevExpress.XtraBars.BarButtonItem barbtnCalibrationSimualte1;
        private DevExpress.XtraBars.BarButtonItem barbtnCalibrationSimualte2;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.GroupControl groupControl6;
        private DevExpress.XtraEditors.SimpleButton btnCallData;
        private DevExpress.XtraEditors.TreeListLookUpEdit treeSocketType;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraEditors.GroupControl groupControl9;
        private DevExpress.XtraEditors.GroupControl groupControl12;
        private DevExpress.XtraEditors.GroupControl groupControl11;
        private DevExpress.XtraEditors.GroupControl groupControl10;
        private DevExpress.XtraEditors.GroupControl groupControl5;
        private DevExpress.XtraEditors.CheckEdit ceCollectPluse;
        private DevExpress.XtraGrid.GridControl gridControl_Pluse;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_Pluse;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_pluse_starttime;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_pluse_coltime;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn17;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_pluse_sendtime;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.GroupControl groupControl4;
        private DevExpress.XtraEditors.CheckEdit ceCollectModbus;
        private DevExpress.XtraGrid.GridControl gridControl_485protocol;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_485protocol;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_485protocol_baud;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txt_485protocol_ID;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txt_485protocol_funcode;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txt_485protocol_regbeginaddr;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txt_485protocol_regcount;
        private DevExpress.XtraEditors.ComboBoxEdit cb485BaudRate;
        private DevExpress.XtraEditors.TextEdit txtIP;
        private DevExpress.XtraEditors.TextEdit txtVolInterval;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cbModbusExeFlag;
        private DevExpress.XtraEditors.CheckEdit ceModbusExeFlag;
        private DevExpress.XtraEditors.ComboBoxEdit cbPreFlag;
        private DevExpress.XtraEditors.GroupControl groupControl13;
        private DevExpress.XtraEditors.SimpleButton btnVer;
        private DevExpress.XtraEditors.TextEdit txtSMSInterval;
        private DevExpress.XtraEditors.ComboBoxEdit cbPluseUnit;
        private DevExpress.XtraEditors.CheckEdit cePluseUnit;
        private DevExpress.XtraEditors.CheckEdit ceSMSInterval;
        private DevExpress.XtraEditors.SimpleButton btnFieldStrength;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cb_pre_coltime;
        private DevExpress.XtraEditors.SimpleButton btnCallClose;
        private DevExpress.XtraEditors.SimpleButton btnCallOpen;
        private System.Windows.Forms.Timer timer_GetWaitCmd;
        private DevExpress.XtraEditors.CheckEdit ceAlarmLen;
        private DevExpress.XtraEditors.TextEdit txtAlarmLen;
        private DevExpress.XtraEditors.GroupControl groupControl14;
        private DevExpress.XtraEditors.SimpleButton btnDel;
        private DevExpress.XtraGrid.GridControl gridControl_WaitCmd;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_WaitCmd;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn18;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn20;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn21;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn 类型;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraBars.BarButtonItem btnStartAlarm;
        private DevExpress.XtraBars.BarButtonItem btnStopAlarm;
        private DevExpress.XtraEditors.SimpleButton btnEnableAlarm;
        private DevExpress.XtraEditors.SimpleButton btnRefresh;
        private DevExpress.XtraEditors.DropDownButton dropDownButtonReset;
        private DevExpress.XtraBars.BarButtonItem btnReset1;
        private DevExpress.XtraBars.BarButtonItem btnReset2;
        private DevExpress.XtraBars.BarButtonItem btnReset3;
        private DevExpress.XtraBars.PopupMenu popupMenuReset;
    }
}