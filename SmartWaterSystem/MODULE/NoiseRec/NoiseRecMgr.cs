using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Protocol;
using Common;
using Entity;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace SmartWaterSystem
{
    public partial class NoiseRecMgr : BaseView, INoiseRecMgr
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("NoiseRecMgr");
        private bool isSetting;

        public NoiseRecMgr()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        public override void OnLoad()
        {
            SerialPortEvent(GlobalValue.portUtil.IsOpen);
            VisiableFlag(false);
        }

        #region 输入验证
        /// <summary>
        /// 验证记录仪管理选项卡输入是否正确
        /// </summary>
        private bool ValidateRecorderManageInput(out string msg)
        {
            bool ok;
            msg = string.Empty;
            //if (string.IsNullOrEmpty(txtRecNote.Text))
            //{
            //    txtRecNote.Focus();
            //    txtRecNote.SelectAll();
            //    msg = "记录仪备注未输入！";
            //    return false;
            //}
            ok = MetarnetRegex.IsUint(txtRecID.Text);
            if (!ok)
            {
                txtRecID.Focus();
                txtRecID.SelectAll();
                msg = "记录仪编号设置错误！";
                return ok;
            }
            //ok = MetarnetRegex.IsTime(txtComTime.Text);
            //if (!ok)
            //{
            //    txtComTime.Focus();
            //    txtComTime.SelectAll();
            //    msg = "通讯时间设置错误！";
            //    return ok;
            //}
            ok = MetarnetRegex.IsTime(txtColTimeStart.Text);
            if (!ok)
            {
                txtColTimeStart.Focus();
                txtColTimeStart.SelectAll();
                msg = "记录时间设置错误！";
                return ok;
            }
            ok = MetarnetRegex.IsUint(txtLeakValue.Text);
            if (!ok)
            {
                txtLeakValue.Focus();
                txtLeakValue.SelectAll();
                msg = "报漏幅度值设置错误！";
                return ok;
            }

            if (comboBoxDist.SelectedIndex == 1)
            {
                ok = MetarnetRegex.IsUint(txtConId.Text);
                if (!ok)
                {
                    txtConId.Focus();
                    txtConId.SelectAll();
                    msg = "控制器编号设置错误！";
                    return ok;
                }
                ok = MetarnetRegex.IsUint(txtConPort.Text);
                if (!ok)
                {
                    txtConPort.Focus();
                    txtConPort.SelectAll();
                    msg = "远传端口设置错误！";
                    return ok;
                }
                ok = MetarnetRegex.IsIPv4(txtConIP.Text);
                if (!ok)
                {
                    txtConIP.Focus();
                    txtConIP.SelectAll();
                    msg = "远传地址设置错误！";
                    return ok;
                }
            }

            // 通讯时间与记录时间不能重叠
            //int comTime = Convert.ToInt32(txtComTime.Text);
            //int recTime1 = Convert.ToInt32(txtColTimeStart.Text);
            //int recTime2 = Convert.ToInt32(txtColTimeEnd.Text);

            //if (comTime == recTime1 || comTime == recTime2 || (comTime > recTime1 && comTime < recTime2))
            //{
            //    txtComTime.Focus();
            //    txtComTime.SelectAll();
            //    msg = "通讯时间/记录时间设置重叠！";
            //    return false;
            //}

            return ok;
        }
        #endregion

        /// <summary>
        /// 绑定记录仪列表
        /// </summary>
        public void BindRecord()
        {
            DataTable dt = new DataTable("RecordTable");
            //DataColumn col = dt.Columns.Add("选择");
            //col.DataType = System.Type.GetType("System.Boolean"); ;
            dt.Columns.Add("编号");
            dt.Columns.Add("漏水警戒值");
            dt.Columns.Add("采集时间");
            dt.Columns.Add("远传功能");
            dt.Columns.Add("添加时间");
            dt.Columns.Add("备注");

            this.CombRemotingID.Properties.Items.Clear();
            for (int i = 0; i < GlobalValue.recorderList.Count; i++)
            {
                NoiseRecorder rec = GlobalValue.recorderList[i];
                dt.Rows.Add(new object[] { rec.ID, rec.LeakValue, rec.RecordTime, rec.ControlerPower, rec.AddDate, rec.Remark });

                this.CombRemotingID.Properties.Items.Add(new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, rec.ID.ToString()));
            }
            
            // 绑定数据
            MethodInvoker mi = (new MethodInvoker(() =>
            {
                gridControlRec.DataSource = dt;
                //gridControlGroup.DataSource = dt;
            }));

            this.Invoke(mi);
        }

        // 读取记录仪编号
        private void btnGetRecID_Click(object sender, EventArgs e)
        {
            Control cl = sender as Control;
            try
            {
                cl.Enabled = false;
                int id = GlobalValue.Noiselog.ReadNoiseLogID();

                this.txtRecID.Text = id.ToString();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("读取失败：" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cl.Enabled = true;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //new Action(() =>
            //{
            if (!isSetting)
            {
                short id = 0;
                bool isError = false;
                try
                {
                    if (!Regex.IsMatch(txtRecID.Text, @"^\d{1,5}$"))
                    {
                        XtraMessageBox.Show("请输入记录仪ID", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtRecID.Focus();
                        return;
                    }

                    EnableControls(false);
                    DisableRibbonBar();
                    DisableNavigateBar();
                    ShowWaitForm("", "正在启动...");
                    lblRecState.Text = "运行状态  正在启动";

                    id = Convert.ToInt16(txtRecID.Text);
                    btnStart.Enabled = false;

                    //short[] Originaldata = null;
                    //GlobalValue.Noiselog.CtrlStartOrStop(id, true, out Originaldata);
                    if (GlobalValue.NoiseSerialPortOptData == null)
                        GlobalValue.NoiseSerialPortOptData = new NoiseSerialPortOptEntity();
                    GlobalValue.NoiseSerialPortOptData.ID = id;
                    BeginSerialPortDelegate();
                    GlobalValue.SerialPortMgr.Send(SerialPortType.NoiseStart);
                    //if (Originaldata == null || (Originaldata != null && (NoiseDataHandler.GetAverage(Originaldata) < 450)))  //没有读到标准值，重试2次
                    //{
                    //    string startvalue = "";
                    //    if(Originaldata!=null)
                    //        foreach (double d in Originaldata)
                    //        {
                    //            startvalue+=d.ToString()+" ";
                    //        }
                    //    ShowDialog("启动失败,请重试["+startvalue+"]!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    isError = true;
                    //}

                    //string path = string.Format(Application.StartupPath + @"\Data\记录仪{0}\", id);
                    //StreamWriter sw = new StreamWriter(string.Format("{0}启动值.txt", path));
                    //for (int i = 0; i < Originaldata.Length; i++)
                    //{
                    //    sw.WriteLine(Originaldata[i]);
                    //}
                    //sw.Flush();
                    //sw.Close();

                    //NoiseRecorder rec = (from item in GlobalValue.recorderList.AsEnumerable()
                    //                     where item.ID == id
                    //                     select item).ToList()[0];
                    //rec.Power = 1;

                    //NoiseDataBaseHelper.UpdateRecorder(rec);
                    //if (NoiseDataBaseHelper.SaveStandData(rec.GroupID, rec.ID, Originaldata) < 0)
                    //{
                    //    ShowDialog("保存记录仪数据失败!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    isError = true;
                    //}
                }
                catch (TimeoutException)
                {
                    ShowDialog("记录仪" + id + "读取超时！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    isError = true;
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    btnStart.Enabled = true;
                }
                catch (ArgumentNullException)
                {
                    ShowDialog("记录仪" + id + "数据为空！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    isError = true;
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    btnStart.Enabled = true;
                }
                catch (Exception ex)
                {
                    ShowDialog(ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isError = true;
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    btnStart.Enabled = true;
                }
                finally
                {
                    //EnableControls(true);
                    //EnableRibbonBar();
                    //EnableNavigateBar();
                    //HideWaitForm();
                    //btnStart.Enabled = true;
                    //if (!isError)
                    //{
                    //    lblRecState.Text = "运行状态  已启动";
                    //    btnStart.Enabled = false;
                    //    btnStop.Enabled = true;
                    //}
                    //else
                    //{
                    //    lblRecState.Text = "运行状态  未知";
                    //    btnStart.Enabled = true;
                    //    btnStop.Enabled = false;
                    //}

                    //this.Refresh();
                }
            }
            //}).BeginInvoke(null, null);
        }

        /// <summary>
        /// Test
        /// </summary>
        /// <returns>获取随机数组(0-255)[32]</returns>
        private short[] GetRandomShortArray()
        {
            List<short> lstRd = new List<short>();
            Random rd = new Random();
            for (int i = 0; i < 32; i++)
                lstRd.Add((short)rd.Next(0, 255));

            return lstRd.ToArray();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //new Action(() =>
            //    {
            if (!isSetting)
            {
                bool isError = false;
                try
                {
                    if (!Regex.IsMatch(txtRecID.Text, @"^\d{1,5}$"))
                    {
                        XtraMessageBox.Show("请输入记录仪ID", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtRecID.Focus();
                        return;
                    }
                    EnableControls(false);
                    DisableRibbonBar();
                    DisableNavigateBar();
                    ShowWaitForm("", "正在停止...");
                    lblRecState.Text = "运行状态  正在停止";
                    btnStop.Enabled = false; ;
                    short id = Convert.ToInt16(txtRecID.Text);
                    //short[] origitydata = null;
                    //GlobalValue.Noiselog.CtrlStartOrStop(id, false, out origitydata);
                    if (GlobalValue.NoiseSerialPortOptData == null)
                        GlobalValue.NoiseSerialPortOptData = new NoiseSerialPortOptEntity();
                    GlobalValue.NoiseSerialPortOptData.ID = id;
                    BeginSerialPortDelegate();
                    GlobalValue.SerialPortMgr.Send(SerialPortType.NoiseStop);

                    //NoiseRecorder rec = (from item in GlobalValue.recorderList.AsEnumerable()
                    //                     where item.ID == id
                    //                     select item).ToList()[0];
                    //rec.Power = 0;
                    //NoiseDataBaseHelper.UpdateRecorder(rec);
                }
                catch (Exception ex)
                {
                    ShowDialog(ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isError = true;
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    btnStop.Enabled = true;
                }
                finally
                {
                    //EnableControls(true);
                    //EnableRibbonBar();
                    //EnableNavigateBar();
                    //HideWaitForm();
                    //btnStop.Enabled = true;
                    //if (!isError)
                    //{
                    //    lblRecState.Text = "运行状态  已停止";
                    //    btnStart.Enabled = true;
                    //    btnStop.Enabled = false;
                    //}
                    //this.Refresh();
                }
            }
            //}).BeginInvoke(null, null);
        }

        // 时间同步
        private void btnNowRec_Click(object sender, EventArgs e)
        {
            dateTimePickerRec.Value = DateTime.Now;
        }

        private void btnNowCon_Click(object sender, EventArgs e)
        {
            dateTimePickerCon.Value = DateTime.Now;
        }

        // 添加记录仪
        private void btnAddRec_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = string.Empty;
                if (!ValidateRecorderManageInput(out msg))
                {
                    throw new Exception(msg);
                }
                for(int i = 0; i < gridViewRecordList.RowCount; i++)
                {
                    int id=Convert.ToInt32(gridViewRecordList.GetRowCellValue(i,"编号"));
                    if (id == Convert.ToInt32(txtRecID.Text))
                    {
                        XtraMessageBox.Show("记录仪编号[" + id + "]已存在!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(txtStartStandValue.Text))
                {
                    if (!Regex.IsMatch(txtStartStandValue.Text, @"^[4-9]\d{2}$"))
                    {
                        XtraMessageBox.Show("请输入有效的启动值[400-999]!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtStartStandValue.Focus();
                        return;
                    }
                }

                NoiseRecorder newRec = new NoiseRecorder();
                newRec.ID = Convert.ToInt32(txtRecID.Text);
                newRec.AddDate = DateTime.Now;
                //newRec.CommunicationTime = Convert.ToInt32(txtComTime.Text);
                if (comboBoxDist.SelectedIndex == 1)
                    newRec.ControlerPower = 1;
                else if (comboBoxDist.SelectedIndex == 0)
                    newRec.ControlerPower = 0;
                else
                    throw new Exception("未选择远传功能！");

                newRec.LeakValue = Convert.ToInt32(txtLeakValue.Text);
                newRec.Remark = txtRecNote.Text;
                newRec.PickSpan = Convert.ToInt32(spinEditInterval.Value);
                newRec.RecordTime = Convert.ToInt32(txtColTimeStart.Text);

                int query = NoiseDataBaseHelper.AddRecorder(newRec);
                if (query != -1)
                {
                    GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
                    BindRecord();
                }
                else
                    throw new Exception("数据入库时发生错误。");

                if (!string.IsNullOrEmpty(txtStartStandValue.Text))
                {
                    int singledata=  Convert.ToInt32(txtStartStandValue.Text);
                    //short[] Originaldata = new short[32];
                    //for (int i = 0; i < 32; i++)
                    //{
                    //    Originaldata[i] = singledata;  //将录入值复制成32个数
                    //}
                    if (NoiseDataBaseHelper.SaveStandData(-1, newRec.ID, singledata) < 0)
                    {
                        ShowDialog("保存记录仪数据失败!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("添加失败：" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 删除勾选的记录仪
        private void btnDeleteRec_Click(object sender, EventArgs e)
        {
            try
            {
                int[] selectedRows = gridViewRecordList.GetSelectedRows();
                if (selectedRows != null && selectedRows.Length > 0)
                {
                    DialogResult dr = XtraMessageBox.Show("确定删除勾选的记录仪？", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        for (int i = 0; i < selectedRows.Length; i++)
                        {
                            int recID = Convert.ToInt32(gridViewRecordList.GetRowCellValue(selectedRows[i], "编号"));
                            NoiseDataBaseHelper.DeleteRecorder(recID);
                            if (NoiseDataBaseHelper.DeleteStandData(-1, recID) < 0)
                            {
                                ShowDialog("删除记录仪数据失败!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        GlobalValue.ClearText(groupControl1);
                        GlobalValue.ClearText(panelControl1);
                        GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
                        GlobalValue.groupList = NoiseDataBaseHelper.GetGroups();
                        //LoadRecorderList();
                        //LoadGroupTree();
                        //LoadGroupList();
                        BindRecord();
                        GlobalValue.reReadIdList.Clear();
                    }
                    else
                        return;
                }
                else
                    XtraMessageBox.Show("未勾选任何记录仪！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("删除失败：" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 读取模板参数
        private void btnReadT_Click(object sender, EventArgs e)
        {
            //txtComTime.Text = Settings.Instance.GetString(SettingKeys.ComTime_Template);
            txtColTimeStart.Text = Settings.Instance.GetString(SettingKeys.RecTime_Template);
            spinEditInterval.Value = Settings.Instance.GetInt(SettingKeys.Span_Template);
            txtRecNum.Text = (GlobalValue.Time * 60 / Settings.Instance.GetInt(SettingKeys.Span_Template)).ToString();
            txtLeakValue.Text = (new BLL.NoiseParmBLL()).GetParm(ConstValue.LeakValue_Template);

            int power = Settings.Instance.GetInt(SettingKeys.Power_Template);
            //comboBoxEditPower.SelectedIndex = power;

            //int conPower = Settings.Instance.GetInt(SettingKeys.ControlPower_Template);
            //if (conPower == 1)
            //{
                //comboBoxDist.SelectedIndex = conPower;
            //    txtConPort.Text = Settings.Instance.GetString(SettingKeys.Port_Template);
            //    txtConIP.Text = Settings.Instance.GetString(SettingKeys.Adress_Template);
            //}
            //else
            //    comboBoxDist.SelectedIndex = conPower;
        }

        private void EnableControls(bool enable)
        {
            btnGetRecID.Enabled = enable;
            txtRecID.Enabled = enable;
            btnAddRec.Enabled = enable;
            btnDeleteRec.Enabled = enable;
            btnUpdate.Enabled = enable;

            btnStart.Enabled = enable;
            btnStop.Enabled = enable;
            btnGetStandValue.Enabled = enable;
            btnSetStandValue.Enabled = enable;
            //btnCleanFlash.Enabled = enable;
            btnReadT.Enabled = enable;
            btnReadSet.Enabled = enable;
            btnApplySet.Enabled = enable;
            btnReadCtrlSet.Enabled = enable;
            btnApplyCtrlSet.Enabled = enable;

            gridControlRec.Enabled = enable;
        }

        // 读取设备参数
        private void btnReadSet_Click(object sender, EventArgs e)
        {
            try
            {
                GlobalValue.NoiseSerialPortOptData = new NoiseSerialPortOptEntity();
                bool haveread = false;
                if (!Regex.IsMatch(txtRecID.Text, @"^\d{1,5}$"))
                {
                    XtraMessageBox.Show("请输入记录仪ID", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtRecID.Focus();
                    return;
                }

                GlobalValue.NoiseSerialPortOptData.ID = Convert.ToInt16(txtRecID.Text);

                if (ceDTRec.Checked)
                {
                    GlobalValue.NoiseSerialPortOptData.IsOptDT = ceDTRec.Checked;
                    haveread = true;
                }
                if (ceComTime.Checked)
                {
                    GlobalValue.NoiseSerialPortOptData.IsOptComTime = ceComTime.Checked;
                    haveread = true;
                }
                if (ceColTime.Checked)
                {
                    GlobalValue.NoiseSerialPortOptData.IsOptColTime = ceColTime.Checked;
                    haveread = true;
                }
                if (ceInterval.Checked)
                {
                    GlobalValue.NoiseSerialPortOptData.IsOptInterval = ceInterval.Checked;
                    haveread = true;
                }
                if (ceRemoteSwitch.Checked)
                {
                    GlobalValue.NoiseSerialPortOptData.IsOptRemoteSwitch = ceRemoteSwitch.Checked;
                    haveread = true;
                }

                if (!haveread)
                {
                    XtraMessageBox.Show("请选择需要读取的参数!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                EnableControls(false);
                DisableRibbonBar();
                DisableNavigateBar();
                ShowWaitForm("", "正在读取...");
                BeginSerialPortDelegate();
                GlobalValue.SerialPortMgr.SerialPortScheduleEvent -= new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
                GlobalValue.SerialPortMgr.SerialPortScheduleEvent += new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
                Application.DoEvents();
                SetStaticItem("正在读取...");
                GlobalValue.SerialPortMgr.Send(SerialPortType.NoiseReadParm);
            }
            catch (Exception ex)
            {
                ShowDialog("读取失败：" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStaticItem("读取失败");
                EnableControls(true);
                EnableRibbonBar();
                EnableNavigateBar();
                HideWaitForm();
            }
            finally
            {
                //EnableRibbonBar();
                //EnableNavigateBar();
                //EnableControls(true);
                //HideWaitForm();
            }
            //}).BeginInvoke(null, null);
        }

        // 读取控制器设备参数
        private void btnReadCtrlSet_Click(object sender, EventArgs e)
        {
            try
            {
                GlobalValue.NoiseSerialPortOptData = new NoiseSerialPortOptEntity();
                
                bool haveread = false;
                if (ceConId.Checked)
                {
                    GlobalValue.NoiseSerialPortOptData.IsOptID = ceConId.Checked;
                    haveread = true;
                }
                else
                {
                    if (!Regex.IsMatch(txtCurConId.Text, @"^\d{1,3}$"))
                    {
                        XtraMessageBox.Show("请输入控制器ID", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCurConId.Focus();
                        return;
                    }
                    GlobalValue.NoiseSerialPortOptData.ID = Convert.ToInt16(txtCurConId.Text);

                    if(ceRemotingID.Checked)
                    {
                        GlobalValue.NoiseSerialPortOptData.IsOptRemoteId = ceRemotingID.Checked;
                        haveread = true;
                    }
                    if (ceDTCon.Checked)
                    {
                        GlobalValue.NoiseSerialPortOptData.IsOptDT = ceDTCon.Checked;
                        haveread = true;
                    }
                    if (ceRemoteSwitch.Checked)
                    {
                        GlobalValue.NoiseSerialPortOptData.IsOptRemoteSwitch = ceRemoteSwitch.Checked;
                        haveread = true;
                    }
                    if(ceComTime.Checked)
                    {
                        GlobalValue.NoiseSerialPortOptData.IsOptComTime = ceComTime.Checked;
                        haveread = true;
                    }
                    if(ceConIP.Checked)
                    {
                        GlobalValue.NoiseSerialPortOptData.IsOptIP = ceConIP.Checked;
                        haveread = true;
                    }
                    if(ceConPort.Checked)
                    {
                        GlobalValue.NoiseSerialPortOptData.IsOptPort = ceConPort.Checked;
                        haveread = true;
                    }
                }
                if (!haveread)
                {
                    XtraMessageBox.Show("请选择需要读取的参数!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                EnableControls(false);
                DisableRibbonBar();
                DisableNavigateBar();
                ShowWaitForm("", "正在读取远传控制器参数...");
                BeginSerialPortDelegate();
                GlobalValue.SerialPortMgr.SerialPortScheduleEvent -= new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
                GlobalValue.SerialPortMgr.SerialPortScheduleEvent += new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
                Application.DoEvents();
                SetStaticItem("正在读取远传控制器参数...");
                GlobalValue.SerialPortMgr.Send(SerialPortType.NoiseCtrlReadParm);
            }
            catch (Exception ex)
            {
                ShowDialog("读取失败：" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStaticItem("读取失败");
                EnableControls(true);
                EnableRibbonBar();
                EnableNavigateBar();
                HideWaitForm();
            }
            finally
            {
                //EnableRibbonBar();
                //EnableNavigateBar();
                //EnableControls(true);
                //HideWaitForm();
            }
        }

        // 设置控制器设备参数

        DistanceController alterCtrl = null;
        private void btnApplyCtrlSet_Click(object sender, EventArgs e)
        {
            string error_flag = "";
            try
            {
                btnApplySet.Enabled = false;
                //VisiableFlag(false);  //隐藏标记

                if (!Regex.IsMatch(txtCurConId.Text, @"^\d{1,3}$"))
                {
                    XtraMessageBox.Show("请输入控制器ID", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCurConId.Focus();
                    return;
                }
                GlobalValue.NoiseSerialPortOptData = new NoiseSerialPortOptEntity();
                bool haveset = false;
                if (ceConId.Checked)
                {
                    if (DialogResult.No == XtraMessageBox.Show("设置设备编号会初始化设备参数,是否继续?", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                    {
                        return;
                    }
                    GlobalValue.NoiseSerialPortOptData.IsOptID = ceConId.Checked;
                    GlobalValue.NoiseSerialPortOptData.ID = Convert.ToInt16(txtCurConId.Text);
                    GlobalValue.NoiseSerialPortOptData.SetID = Convert.ToInt16(txtConId.Text);
                    haveset = true;
                }
                else
                {
                    GlobalValue.NoiseSerialPortOptData.ID = Convert.ToInt16(txtCurConId.Text);

                    //alterCtrl = new DistanceController();
                    //alterRec = (from item in GlobalValue.recorderList
                    //            where item.ID == id
                    //            select item).ToList()[0];
                    //alterCtrl.ID = Convert.ToInt32(txtConId.Text);
                    //alterCtrl.RecordID = alterRec.ID;
                    //alterCtrl.Port = Convert.ToInt32(txtConPort.Text);
                    //alterCtrl.IPAdress = txtConIP.Text;
                    if (ceDTCon.Checked)
                    {
                        GlobalValue.NoiseSerialPortOptData.IsOptDT = ceDTCon.Checked;
                        GlobalValue.NoiseSerialPortOptData.dt = dateTimePickerCon.Value;
                        haveset = true;
                    }

                    if (ceRemotingID.Checked)
                    {
                        for(int i=0;i<CombRemotingID.Properties.Items.Count;i++)
                        {

                        }
                        GlobalValue.NoiseSerialPortOptData.IsOptRemoteId = ceRemotingID.Checked;
                        List<int> lstRemoteId = new List<int>();
                        string[] strids = CombRemotingID.Text.Split(',');
                        if(strids!=null && strids.Length>0)
                        {
                            for(int i = 0; i < strids.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(strids[i]))
                                    lstRemoteId.Add(Convert.ToInt32(strids[i]));
                            }
                        }
                        //校验
                        if (lstRemoteId.Count == 0)
                        {
                            XtraMessageBox.Show("请选择记录仪ID!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CombRemotingID.Focus();
                            return;
                        }
                        GlobalValue.NoiseSerialPortOptData.RemoteId = lstRemoteId;
                        haveset = true;
                    }
                    if (ceRemoteSwitch.Checked)
                    {
                        GlobalValue.NoiseSerialPortOptData.IsOptRemoteSwitch = ceRemoteSwitch.Checked;
                        GlobalValue.NoiseSerialPortOptData.RemoteSwitch = comboBoxDist.SelectedIndex == 1 ? true : false;
                        haveset = true;
                    }

                    if (ceComTime.Checked)
                    {
                        if (!Regex.IsMatch(txtComTime.Text, @"^\d{1,2}$"))
                        {
                            XtraMessageBox.Show("请输入通讯时间!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtComTime.Focus();
                            return;
                        }
                        GlobalValue.NoiseSerialPortOptData.IsOptComTime = ceComTime.Checked;
                        GlobalValue.NoiseSerialPortOptData.ComTime = Convert.ToInt32(txtComTime.Text);
                        haveset = true;
                    }
                    if (ceConIP.Checked)
                    {
                        if (!Regex.IsMatch(txtConIP.Text, @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$"))
                        {
                            XtraMessageBox.Show("请输入正确的IP地址!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtConIP.Focus();
                            return;
                        }
                        GlobalValue.NoiseSerialPortOptData.IsOptIP = ceConIP.Checked;
                        string str_ip = "";   //需要将ip处理成15位长度,如:192.168.010.125
                        string[] str_ips = txtConIP.Text.Split('.');
                        if (str_ips != null && str_ips.Length > 0)
                        {
                            foreach (string ippart in str_ips)
                            {
                                if (ippart.Length == 1)
                                {
                                    str_ip += "00" + ippart + ".";
                                }
                                else if (ippart.Length == 2)
                                {
                                    str_ip += "0" + ippart + ".";
                                }
                                else
                                {
                                    str_ip += ippart + ".";
                                }
                            }
                        }
                        if (str_ip.EndsWith("."))
                            str_ip = str_ip.Substring(0, str_ip.Length - 1);
                        GlobalValue.NoiseSerialPortOptData.IP = str_ip;

                        haveset = true;
                    }
                    if (ceConPort.Checked)
                    {
                        if (!Regex.IsMatch(txtConPort.Text, @"^\d{1,4}$"))
                        {
                            XtraMessageBox.Show("请输入正确的端口号(1-4位长度)!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtConPort.Focus();
                            return;
                        }
                        GlobalValue.NoiseSerialPortOptData.IsOptPort = ceConPort.Checked;
                        GlobalValue.NoiseSerialPortOptData.Port = Convert.ToInt32(txtConPort.Text);
                        haveset = true;
                    }
                    if (!haveset)
                    {
                        XtraMessageBox.Show("请选择需要设置的参数!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                
                EnableControls(false);
                DisableRibbonBar();
                DisableNavigateBar();
                ShowWaitForm("", "正在设置远传控制器参数...");
                BeginSerialPortDelegate();
                GlobalValue.SerialPortMgr.SerialPortScheduleEvent -= new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
                GlobalValue.SerialPortMgr.SerialPortScheduleEvent += new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
                Application.DoEvents();
                SetStaticItem("正在设置远传控制器参数...");
                GlobalValue.SerialPortMgr.Send(SerialPortType.NoiseCtrlSetParm);

            }
            catch (Exception ex)
            {
                SetErrorFlag(error_flag);
                ShowDialog("设置失败：" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStaticItem("设置失败");
            }
            finally
            {
                //EnableRibbonBar();
                //EnableNavigateBar();
                //HideWaitForm();
                //btnApplySet.Enabled = true;
                //isSetting = false;
            }
        }

        void SerialPortParm_SerialPortScheduleEvent(object sender, SerialPortScheduleEventArgs e)
        {
            if (e.OptType == SerialPortType.NoiseReadParm && !string.IsNullOrEmpty(e.Msg))
            {
                ShowWaitForm("", e.Msg);
                SetStaticItem(e.Msg);
            }
        }

        public override void OnSerialPortNotify(object sender, SerialPortEventArgs e)
        {
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.NoiseReadParm)
            {
                string message = string.Empty;
                if (e.Tag != null)
                    message = e.Tag.ToString();

                this.Enabled = true;
                HideWaitForm();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);

                EnableControls(true);
                EnableRibbonBar();
                EnableNavigateBar();
                HideWaitForm();
                if (e.TransactStatus == TransStatus.Success)
                {
                    if (e.Tag != null)
                    {
                        NoiseSerialPortOptEntity read_result = (NoiseSerialPortOptEntity)e.Tag;
                        //时间
                        if (GlobalValue.NoiseSerialPortOptData.IsOptDT)
                            this.dateTimePickerRec.Value = read_result.dt;
                        // 读取远传通讯时间
                        //if (GlobalValue.NoiseSerialPortOptData.IsOptComTime)
                        //    this.txtComTime.Text = read_result.ComTime.ToString();
                        //记录时间段
                        if (GlobalValue.NoiseSerialPortOptData.IsOptColTime)
                        {
                            txtColTimeStart.Text = read_result.colstarttime.ToString();
                            txtColTimeEnd.Text = read_result.colendtime.ToString();
                        }
                        //采集间隔
                        if (GlobalValue.NoiseSerialPortOptData.IsOptInterval)
                            spinEditInterval.Value = read_result.Interval;
                        //if (GlobalValue.NoiseSerialPortOptData.IsOptRemoteSwitch)
                        //{
                        //    if (read_result.RemoteSwitch)
                        //    {
                        //        comboBoxDist.SelectedIndex = 1;
                        //    }
                        //    else
                        //    {
                        //        comboBoxDist.SelectedIndex = 0;
                        //    }
                        //}
                    }
                    ShowDialog("读取记录仪参数成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ShowDialog("读取记录仪参数失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.NoiseSetParm)
            {
                string message = string.Empty;
                if (e.Tag != null)
                    message = e.Tag.ToString();

                this.Enabled = true;
                HideWaitForm();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                if (e.TransactStatus == TransStatus.Success)
                {
                    //SetFlagRemoteSwitch(true);
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    btnApplySet.Enabled = true;
                    isSetting = false;

                    //if (alterCtrl != null)
                    //    NoiseDataBaseHelper.UpdateControler(alterCtrl);
                    int query = NoiseDataBaseHelper.UpdateRecorder(alterRec);
                    if (query != -1)
                    {
                        ShowDialog("设置成功！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
                        GlobalValue.groupList = NoiseDataBaseHelper.GetGroups();
                        BindRecord();
                    }
                    else
                        throw new Exception("数据入库发生错误。");

                    //ShowDialog("设置成功！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
                    //GlobalValue.groupList = NoiseDataBaseHelper.GetGroups();
                    //BindRecord();
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    btnApplySet.Enabled = true;
                    isSetting = false;
                    XtraMessageBox.Show("设置失败!" + message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.NoiseCtrlReadParm)
            {
                string message = string.Empty;
                if (e.Tag != null)
                    message = e.Tag.ToString();

                this.Enabled = true;
                HideWaitForm();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);

                EnableControls(true);
                EnableRibbonBar();
                EnableNavigateBar();
                HideWaitForm();
                if (e.TransactStatus == TransStatus.Success)
                {
                    if (e.Tag != null)
                    {
                        NoiseSerialPortOptEntity read_result = (NoiseSerialPortOptEntity)e.Tag;
                        //ID
                        if (GlobalValue.NoiseSerialPortOptData.IsOptID)
                        {
                            txtCurConId.Text = read_result.ID.ToString();
                            txtConId.Text = read_result.ID.ToString();
                        }
                        //记录仪ID
                        if (GlobalValue.NoiseSerialPortOptData.IsOptRemoteId)
                        {
                            for(int i = 0; i<this.CombRemotingID.Properties.Items.Count;i++)  //将选中状态去除
                            {
                                CombRemotingID.Properties.Items[i].CheckState = CheckState.Unchecked;
                            }
                            if (read_result.RemoteId!=null && read_result.RemoteId.Count>0)
                            {
                                for(int i =0;i<read_result.RemoteId.Count;i++)
                                {
                                    bool bcontain = false;  //标记是否列表中已包含
                                    for (int j =0; j < CombRemotingID.Properties.Items.Count;j++)
                                    {
                                        if(read_result.RemoteId[i].ToString()== CombRemotingID.Properties.Items[j].Description)
                                        {
                                            bcontain = true;
                                            CombRemotingID.Properties.Items[j].CheckState = CheckState.Checked;
                                        }
                                    }
                                    if(!bcontain) //未包含的情况下添加进列表
                                    {
                                        this.CombRemotingID.Properties.Items.Add(new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null,read_result.RemoteId[i].ToString(),CheckState.Checked));
                                    }
                                }
                            }
                        }
                        
                        //时间
                        if (GlobalValue.NoiseSerialPortOptData.IsOptDT)
                            this.dateTimePickerCon.Value = read_result.dt;
                        //开关
                        if (GlobalValue.NoiseSerialPortOptData.IsOptRemoteSwitch)
                        {
                            if (read_result.RemoteSwitch)
                            {
                                comboBoxDist.SelectedIndex = 1;
                            }
                            else
                            {
                                comboBoxDist.SelectedIndex = 0;
                            }
                        }
                        // 读取远传通讯时间
                        if (GlobalValue.NoiseSerialPortOptData.IsOptComTime)
                            this.txtComTime.Text = read_result.ComTime.ToString();
                        // 读取远传IP
                        if (GlobalValue.NoiseSerialPortOptData.IsOptIP)
                            this.txtConIP.Text = read_result.IP.ToString();
                        //读取远传端口
                        if (GlobalValue.NoiseSerialPortOptData.IsOptPort)
                            this.txtConPort.Text = read_result.Port.ToString();
                    }
                    ShowDialog("读取远程控制器参数成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ShowDialog("读取远程控制器参数失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.NoiseCtrlSetParm)
            {
                string message = string.Empty;
                if (e.Tag != null)
                    message = e.Tag.ToString();

                this.Enabled = true;
                HideWaitForm();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                if (e.TransactStatus == TransStatus.Success)
                {
                    //SetFlagRemoteSwitch(true);
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    btnApplyCtrlSet.Enabled = true;
                    isSetting = false;

                    //if (alterCtrl != null)
                    //    NoiseDataBaseHelper.UpdateControler(alterCtrl);

                    ShowDialog("设置远传控制器参数成功！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
                    GlobalValue.groupList = NoiseDataBaseHelper.GetGroups();
                    BindRecord();
                }
                else
                {
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    btnApplySet.Enabled = true;
                    isSetting = false;
                    XtraMessageBox.Show("设置远传控制器参数失败!" + message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.NoiseStart)
            {
                string message = string.Empty;
                if (e.Tag != null)
                    message = e.Tag.ToString();

                this.Enabled = true;
                HideWaitForm();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);

                EnableControls(true);
                EnableRibbonBar();
                EnableNavigateBar();
                HideWaitForm();
                if (e.TransactStatus == TransStatus.Success)
                {
                    if (GlobalValue.NoiseSerialPortOptData != null)
                    {
                        if (e.Tag != null)
                        {
                            short[] Originaldata = (short[])e.Tag;
                            string path = string.Format(Application.StartupPath + @"\Data\记录仪{0}\", GlobalValue.NoiseSerialPortOptData.ID);
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            StreamWriter sw = new StreamWriter(string.Format("{0}启动值.txt", path));
                            for (int i = 0; i < Originaldata.Length; i++)
                            {
                                sw.WriteLine(Originaldata[i]);
                            }
                            sw.Flush();
                            sw.Close();

                        }
                        NoiseRecorder rec = (from item in GlobalValue.recorderList.AsEnumerable()
                                             where item.ID == GlobalValue.NoiseSerialPortOptData.ID
                                             select item).ToList()[0];
                        rec.Power = 1;

                        lblRecState.Text = "运行状态  已启动";
                        btnStart.Enabled = false;
                        btnStop.Enabled = true;
                    }
                }
                else
                {
                    ShowDialog(e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblRecState.Text = "运行状态  未知";
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                }
            }
            else if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.NoiseStop)
            {
                string message = string.Empty;
                if (e.Tag != null)
                    message = e.Tag.ToString();

                this.Enabled = true;
                HideWaitForm();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);

                EnableControls(true);
                EnableRibbonBar();
                EnableNavigateBar();
                HideWaitForm();
                if (e.TransactStatus == TransStatus.Success)
                {
                    if (GlobalValue.NoiseSerialPortOptData != null)
                    {
                        NoiseRecorder rec = (from item in GlobalValue.recorderList.AsEnumerable()
                                             where item.ID == GlobalValue.NoiseSerialPortOptData.ID
                                             select item).ToList()[0];
                        rec.Power = 0;
                        //NoiseDataBaseHelper.UpdateRecorder(rec);

                        lblRecState.Text = "运行状态  已停止";
                        btnStart.Enabled = true;
                        btnStop.Enabled = false;
                    }
                }
                else
                {
                    ShowDialog(e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblRecState.Text = "运行状态  未知";
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                }
            }
            else if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.NoiseClearData)
            {
                string message = string.Empty;
                if (e.Tag != null)
                    message = e.Tag.ToString();

                this.Enabled = true;
                HideWaitForm();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);

                EnableControls(true);
                EnableRibbonBar();
                EnableNavigateBar();
                HideWaitForm();
                if (e.TransactStatus == TransStatus.Success)
                {
                    ShowDialog("清除记录仪数据成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ShowDialog("清除记录仪数据失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.NoiseGetStandValue)
            {
                string message = string.Empty;
                this.Enabled = true;
                HideWaitForm();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);

                EnableControls(true);
                EnableRibbonBar();
                EnableNavigateBar();
                HideWaitForm();
                if (e.TransactStatus == TransStatus.Success)
                {
                    txtStartStandValue.Text = "0";
                    if (e.Tag != null)
                    {
                        NoiseSerialPortOptEntity read_result = (NoiseSerialPortOptEntity)e.Tag;
                        txtStartStandValue.Text = read_result.StandValue.ToString();
                    }
                    ShowDialog("读取启动值成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ShowDialog("读取启动值失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.NoiseSetStandValue)
            {
                string message = string.Empty;
                if (e.Tag != null)
                    message = e.Tag.ToString();

                this.Enabled = true;
                HideWaitForm();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);

                EnableControls(true);
                EnableRibbonBar();
                EnableNavigateBar();
                HideWaitForm();
                if (e.TransactStatus == TransStatus.Success)
                {
                    ShowDialog("设置启动值成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ShowDialog("设置启动值失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        // 应用设置
        NoiseRecorder alterRec = null;
        private void btnApplySet_Click(object sender, EventArgs e)
        {
            isSetting = true;

            //string error_flag = "";
            try
            {
                //btnApplySet.Enabled = false;
                GlobalValue.NoiseSerialPortOptData = new NoiseSerialPortOptEntity();
                if (!Regex.IsMatch(txtRecID.Text, @"^\d{1,5}$"))
                {
                    XtraMessageBox.Show("请输入记录仪ID", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtRecID.Focus();
                    return;
                }

                short id = Convert.ToInt16(txtRecID.Text);
                GlobalValue.NoiseSerialPortOptData.ID = id;
                alterRec = (from item in GlobalValue.recorderList
                            where item.ID == id
                            select item).ToList()[0];

                alterRec.ID = id;
                alterRec.LeakValue = Convert.ToInt32(txtLeakValue.Text);
                alterRec.Remark = txtRecNote.Text;
                
                alterCtrl = new DistanceController();
                //alterCtrl.ID = Convert.ToInt32(txtCurConId.Text);
                //alterCtrl.RecordID = alterRec.ID;
                //alterCtrl.Port = Convert.ToInt32(txtConPort.Text);
                //alterCtrl.IPAdress = txtConIP.Text;
                
                bool haveset = false;
                if (ceDTRec.Checked)
                {
                    GlobalValue.NoiseSerialPortOptData.IsOptDT = ceDTRec.Checked;
                    GlobalValue.NoiseSerialPortOptData.dt = dateTimePickerRec.Value;
                    haveset = true;
                }
                //if(ceComTime.Checked)
                //{
                //    if(!Regex.IsMatch(txtComTime.Text,@"^\d{1,2}$"))
                //    {
                //        XtraMessageBox.Show("请输入通讯时间!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        txtComTime.Focus();
                //        return;
                //    }
                //    //alterRec.CommunicationTime = Convert.ToInt32(txtComTime.Text);
                //    GlobalValue.NoiseSerialPortOptData.IsOptComTime = ceComTime.Checked;
                //    GlobalValue.NoiseSerialPortOptData.ComTime = Convert.ToInt32(txtComTime.Text);
                //    haveset = true;
                //}
                if(ceColTime.Checked)
                {
                    if (!Regex.IsMatch(txtColTimeStart.Text, @"^\d{1,2}$"))
                    {
                        XtraMessageBox.Show("请输入起始采集时间!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtColTimeStart.Focus();
                        return;
                    }
                    if (!Regex.IsMatch(txtColTimeEnd.Text, @"^\d{1,2}$"))
                    {
                        XtraMessageBox.Show("请输入最后采集时间!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtColTimeEnd.Focus();
                        return;
                    }
                    
                    alterRec.RecordTime = Convert.ToInt32(txtColTimeStart.Text);
                    GlobalValue.NoiseSerialPortOptData.IsOptColTime = ceColTime.Checked;
                    GlobalValue.NoiseSerialPortOptData.colstarttime = Convert.ToInt32(txtColTimeStart.Text);
                    GlobalValue.NoiseSerialPortOptData.colendtime = Convert.ToInt32(txtColTimeEnd.Text);

                    //通讯时间和采集时间不能重复
                    //if (ceComTime.Checked)
                    //{
                    //    int comtime = GlobalValue.NoiseSerialPortOptData.ComTime;
                    //    if (comtime == 0)
                    //        comtime = 24;
                    //    int endtime = GlobalValue.NoiseSerialPortOptData.colendtime;
                    //    if (GlobalValue.NoiseSerialPortOptData.colstarttime > endtime)
                    //    {
                    //        endtime += 24;
                    //    }
                    //    if(comtime >= GlobalValue.NoiseSerialPortOptData.colstarttime && comtime <= endtime)
                    //    {
                    //        XtraMessageBox.Show("通讯时间不能和采集时间重叠!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //        txtComTime.Focus();
                    //        return;
                    //    }
                    //}

                    haveset = true;
                }
                if(ceInterval.Checked)
                {
                    if (!Regex.IsMatch(spinEditInterval.Text, @"^\d{1,2}$") && spinEditInterval.Value<=0)
                    {
                        XtraMessageBox.Show("请输入正确采集间隔!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        spinEditInterval.Focus();
                        return;
                    }
                    alterRec.PickSpan = Convert.ToInt32(spinEditInterval.Value);
                    GlobalValue.NoiseSerialPortOptData.IsOptInterval = ceInterval.Checked;
                    GlobalValue.NoiseSerialPortOptData.Interval = Convert.ToInt32(spinEditInterval.Value);
                    haveset = true;
                }
                //if(ceRemoteSwitch.Checked)
                //{
                //    alterRec.ControlerPower = comboBoxDist.SelectedIndex;
                //    GlobalValue.NoiseSerialPortOptData.IsOptRemoteSwitch = ceRemoteSwitch.Checked;
                //    GlobalValue.NoiseSerialPortOptData.RemoteSwitch = comboBoxDist.SelectedIndex == 1 ? true : false;
                //    haveset = true;
                //}
                if (!haveset)
                {
                    XtraMessageBox.Show("请选择需要设置的参数!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                EnableControls(false);
                DisableRibbonBar();
                DisableNavigateBar();
                ShowWaitForm("", "正在设置...");
                BeginSerialPortDelegate();
                GlobalValue.SerialPortMgr.SerialPortScheduleEvent -= new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
                GlobalValue.SerialPortMgr.SerialPortScheduleEvent += new SerialPortScheduleHandle(SerialPortParm_SerialPortScheduleEvent);
                Application.DoEvents();
                SetStaticItem("正在设置...");
                GlobalValue.SerialPortMgr.Send(SerialPortType.NoiseSetParm);
            }
            catch (Exception ex)
            {
                //SetErrorFlag(error_flag);
                ShowDialog("设置失败：" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStaticItem("设置失败");
            }
            finally
            {
                //EnableRibbonBar();
                //EnableNavigateBar();
                //HideWaitForm();
                //btnApplySet.Enabled = true;
                //isSetting = false;
            }
            //}).BeginInvoke(null, null);
        }

        private void gridViewRecordList_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.Caption == "远传功能")
            {
                if (Convert.ToInt32(e.Value) == 1)
                    e.DisplayText = "开";
                else
                    e.DisplayText = "关";
            }
        }

        private void UcRecMgr_Load(object sender, EventArgs e)
        {
            //BindRecord();
            this.timer1.Interval = 1000;   //1s
            this.timer1.Tick += new EventHandler(timer1_Tick);
            this.timer1.Enabled = true;
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                DateTime datenow = this.dateTimePickerRec.Value;
                if (datenow.CompareTo(DateTime.Now.AddDays(-1)) > -1)
                    this.dateTimePickerRec.Value = datenow.AddSeconds(1);

                datenow = this.dateTimePickerCon.Value;
                if (datenow.CompareTo(DateTime.Now.AddDays(-1)) > -1)
                    this.dateTimePickerCon.Value = datenow.AddSeconds(1);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridViewRecordList_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try {
                txtConIP.Text = "";
                txtConId.Text = "";
                txtConPort.Text = "";

                NoiseRecorder rec = (from item in GlobalValue.recorderList.AsEnumerable()
                                     where item.ID == Convert.ToInt32(gridViewRecordList.GetFocusedRowCellValue("编号"))
                                     select item).ToList()[0];

                txtRecID.Text = rec.ID.ToString();
                //txtComTime.Text = rec.CommunicationTime.ToString();
                txtColTimeStart.Text = rec.RecordTime.ToString();
                short[] standvalue = NoiseDataBaseHelper.GetStandData(-1, rec.ID);
                if (standvalue != null && standvalue.Length > 0)
                {
                    int sumstandvalue = 0;
                    for (int i = 0; i < standvalue.Length; i++)
                    {
                        sumstandvalue += standvalue[i];
                    }
                    txtStartStandValue.Text = (sumstandvalue / standvalue.Length).ToString();
                }
                else
                    txtStartStandValue.Text = "";

                spinEditInterval.Value = rec.PickSpan;
                txtRecNum.Text = (GlobalValue.Time * 60 / rec.PickSpan).ToString();
                txtLeakValue.Text = rec.LeakValue.ToString();
                txtRecNote.Text = rec.Remark;
                if (rec.Power == 1)
                {
                    lblRecState.Text = "运行状态  已启动";
                    //comboBoxEditPower.SelectedIndex = rec.Power;
                }
                else if (rec.Power == 0)
                {
                    lblRecState.Text = "运行状态  已停止";
                    //comboBoxEditPower.SelectedIndex = rec.Power;
                }
                else
                {
                    lblRecState.Text = "运行状态  未知";
                    //comboBoxEditPower.SelectedIndex = 2;
                }


                if (rec.ControlerPower == 1)
                {
                    DistanceController dc = (from item in GlobalValue.controllerList.AsEnumerable()
                                             where item.ID == rec.ID
                                             select item).ToList()[0];

                    txtConId.Text = dc.ID.ToString();
                    txtConPort.Text = dc.Port.ToString();
                    txtConIP.Text = dc.IPAdress.ToString();
                }
                comboBoxDist.SelectedIndex = rec.ControlerPower;

                btnDeleteRec.Enabled = true;

                //if (GlobalValue.portUtil.IsOpen)
                //{
                //    btnStart.Enabled = true;
                //    btnStop.Enabled = true;
                //    btnApplySet.Enabled = true;
                //    btnNow.Enabled = true;
                //}
                //else
                //{
                //    btnStart.Enabled = false;
                //    btnStop.Enabled = false;
                //    btnApplySet.Enabled = false;
                //    btnNow.Enabled = false;
                //}

                SerialPortEvent(GlobalValue.portUtil.IsOpen);

                btnDeleteRec.Enabled = true;

                if (!btnUpdate.Enabled)
                    btnUpdate.Enabled = true;
            }catch
            { }
        }

        private void comboBoxDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (comboBoxDist.SelectedIndex == 1)
            //{
            //    txtConId.Enabled = true;
            //    txtConPort.Enabled = true;
            //    txtConIP.Enabled = true;
            //}
            //else
            //{
            //    txtConId.Enabled = false;
            //    txtConPort.Enabled = false;
            //    txtConIP.Enabled = false;
            //}
        }

        private void gridViewRecordList_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.Caption == "选择")
            {
                //for (int i = 0; i < gridViewRecordList.RowCount; i++)
                //{ 
                //    if(()gridViewRecordList.GetRowCellValue(i,"选择")
                //}
            }
        }

        private void txtRecTime_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraEditors.TextEdit obj = sender as DevExpress.XtraEditors.TextEdit;
                int t = 0;
                int t1 = 0;
                //switch (obj.Name)
                //{
                //    case "txtRecTime":
                        t = Convert.ToInt32(obj.Text);
                        t1 = t + GlobalValue.Time;
                        if (t1 > 24)
                            txtColTimeEnd.Text = (t1 - 24).ToString();
                        else
                            txtColTimeEnd.Text = (t + GlobalValue.Time).ToString();
                        //break;
                //}
            }
            catch (Exception)
            { }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLeakValue.Text))
            {
                XtraMessageBox.Show("记录仪信息不完整！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!string.IsNullOrEmpty(txtStartStandValue.Text))
            {
                if (!Regex.IsMatch(txtStartStandValue.Text, @"^[4-9]\d{2}$"))
                {
                    XtraMessageBox.Show("请输入有效的启动值[400-999]!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtStartStandValue.Focus();
                    return;
                }
            }

            int id = Convert.ToInt32(gridViewRecordList.GetFocusedRowCellValue("编号"));

            NoiseRecorder alterRec = (from item in GlobalValue.recorderList
                                      where item.ID == id
                                      select item).ToList()[0];
            alterRec.LeakValue = Convert.ToInt32(txtLeakValue.Text);
            alterRec.Remark = txtRecNote.Text;

            int query = NoiseDataBaseHelper.UpdateRecorder(alterRec);
            if (query != -1)
            {
                if (!string.IsNullOrEmpty(txtStartStandValue.Text))
                {
                    int standvalue = Convert.ToInt32(txtStartStandValue.Text);
                    //short[] Originaldata = new short[32];
                    //for (int i = 0; i < 32; i++)
                    //{
                    //    Originaldata[i] = singledata;  //将录入值复制成32个数
                    //}
                    if (NoiseDataBaseHelper.SaveStandData(-1, id, standvalue) < 0)
                    {
                        ShowDialog("保存记录仪数据失败!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                XtraMessageBox.Show("更新成功！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
                GlobalValue.groupList = NoiseDataBaseHelper.GetGroups();
                BindRecord();
            }
        }

        private void txtRecID_TextChanged(object sender, EventArgs e)
        {
            this.lblRecState.Text = "运行状态  未知";
            SerialPortEvent(GlobalValue.portUtil.IsOpen);
        }

        private void btnCleanFlash_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == DevExpress.XtraEditors.XtraMessageBox.Show("是否清除记录仪数据?", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
            {
                return;
            }
            //new Action(() =>
            //        {
                        if (!isSetting)
                        {
                            short id = 0;
                            try
                            {
                                if (string.IsNullOrEmpty(txtRecID.Text))
                                {
                                    XtraMessageBox.Show("请输入记录仪编号!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    txtRecID.Focus();
                                    return;
                                }

                                ShowWaitForm("", "正在清除数据...");
                                EnableControls(false);
                                DisableRibbonBar();
                                DisableNavigateBar();
                                id = Convert.ToInt16(txtRecID.Text);

                                SetStaticItem("正在清除数据...");

                                //bool result = GlobalValue.Noiselog.ClearData(id);

                                if (GlobalValue.NoiseSerialPortOptData == null)
                                    GlobalValue.NoiseSerialPortOptData = new NoiseSerialPortOptEntity();
                                GlobalValue.NoiseSerialPortOptData.ID = id;
                                BeginSerialPortDelegate();
                                GlobalValue.SerialPortMgr.Send(SerialPortType.NoiseClearData);

                                //if (result)
                                //{
                                //    ShowDialog("清除记录仪数据成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //}
                                //else
                                //{
                                //    ShowDialog("清除记录仪数据失败!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //}
                            }
                            catch (TimeoutException)
                            {
                                ShowDialog("记录仪" + id + "操作超时！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                EnableControls(true);
                                EnableRibbonBar();
                                EnableNavigateBar();
                                HideWaitForm();
                            }
                            catch (ArgumentNullException)
                            {
                                ShowDialog("记录仪" + id + "操作失败！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                EnableControls(true);
                                EnableRibbonBar();
                                EnableNavigateBar();
                                HideWaitForm();
                            }
                            catch (Exception ex)
                            {
                                ShowDialog(ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                EnableControls(true);
                                EnableRibbonBar();
                                EnableNavigateBar();
                                HideWaitForm();
                            }
                            finally
                            {
                                //EnableControls(true);
                                //SetStaticItem("清除数据完成!");
                            }
                        }
                    //}).BeginInvoke(null, null);
        }

        public override void SerialPortEvent(bool Enabled)
        {
            btnApplySet.Enabled = Enabled;
            btnReadSet.Enabled = Enabled;
            btnReadCtrlSet.Enabled = Enabled;
            btnApplyCtrlSet.Enabled = Enabled;
            btnGetRecID.Enabled = Enabled;
            //btnGetConID.Enabled = Enabled;
            btnNowRec.Enabled = Enabled;
            btnStart.Enabled = Enabled;
            btnStop.Enabled = Enabled;
            btnGetStandValue.Enabled = Enabled;
            btnSetStandValue.Enabled = Enabled;
            //btnCleanFlash.Enabled = Enabled;
        }

        private void spinEdit1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                txtRecNum.Text = (Math.Floor(GlobalValue.Time * 60 / spinEditInterval.Value)).ToString();
            }
            catch { }
        }

        private void groupControl2_BackColorChanged(object sender, EventArgs e)
        {
            //FlagTime.BackColor = groupControl2.BackColor;
            //FlagSendTime.BackColor = groupControl2.BackColor;
            //FlagStartEndTime.BackColor = groupControl2.BackColor;
            //FlagInterval.BackColor = groupControl2.BackColor;
            //FlagRemoteSwitch.BackColor = groupControl2.BackColor;
            //FlagConId.BackColor = groupControl2.BackColor;
            //FlagPort.BackColor = groupControl2.BackColor;
            //FlagIP.BackColor = groupControl2.BackColor;
        }

        #region 设置标志Flag
        private void VisiableFlag(bool isVisiable)
        {
            //FlagTime.Visible = isVisiable;
            //FlagSendTime.Visible = isVisiable;
            //FlagStartEndTime.Visible = isVisiable;
            //FlagInterval.Visible = isVisiable;
            //FlagRemoteSwitch.Visible = isVisiable;
            //FlagConId.Visible = isVisiable;
            //FlagPort.Visible = isVisiable;
            //FlagIP.Visible = isVisiable;
        }

        private void SetFlagImage(Bitmap bp)
        {
            //FlagTime.Image = bp;
            //FlagSendTime.Image = bp;
            //FlagStartEndTime.Image = bp;
            //FlagInterval.Image = bp;
            //FlagRemoteSwitch.Image = bp;
            //FlagConId.Image = bp;
            //FlagPort.Image = bp;
            //FlagIP.Image = bp;
        }

        private void SetFlagTime(bool isRight)
        {
            //FlagTime.Visible = true;
            //if (isRight)
            //    FlagTime.Image = SmartWaterSystem.Properties.Resources.right;
            //else
            //    FlagTime.Image = SmartWaterSystem.Properties.Resources.cross1;
            //FlagTime.Update();
            //this.Refresh();
        }

        private void SetFlagSendTime(bool isRight)
        {
            //FlagSendTime.Visible = true;
            //if (isRight)
            //    FlagSendTime.Image = SmartWaterSystem.Properties.Resources.right;
            //else
            //    FlagSendTime.Image = SmartWaterSystem.Properties.Resources.cross1;
            //FlagSendTime.Update();
            //this.Refresh();
        }

        private void SetFlagStartEndTime(bool isRight)
        {
            //FlagStartEndTime.Visible = true;
            //if (isRight)
            //    FlagStartEndTime.Image = SmartWaterSystem.Properties.Resources.right;
            //else
            //    FlagStartEndTime.Image = SmartWaterSystem.Properties.Resources.cross1;
            //FlagStartEndTime.Update();
            //this.Refresh();
        }

        private void SetFlagInterval(bool isRight)
        {
            //FlagInterval.Visible = true;
            //if (isRight)
            //    FlagInterval.Image = SmartWaterSystem.Properties.Resources.right;
            //else
            //    FlagInterval.Image = SmartWaterSystem.Properties.Resources.cross1;
            //FlagInterval.Update();
            //this.Refresh();
        }

        private void SetFlagRemoteSwitch(bool isRight)
        {
            //FlagRemoteSwitch.Visible = true;
            //if (isRight)
            //    FlagRemoteSwitch.Image = SmartWaterSystem.Properties.Resources.right;
            //else
            //    FlagRemoteSwitch.Image = SmartWaterSystem.Properties.Resources.cross1;
            //FlagRemoteSwitch.Update();
            //this.Refresh();
        }

        private void SetFlagConId(bool isRight)
        {
            //FlagConId.Visible = true;
            //if (isRight)
            //    FlagConId.Image = SmartWaterSystem.Properties.Resources.right;
            //else
            //    FlagConId.Image = SmartWaterSystem.Properties.Resources.cross1;
            //FlagConId.Update();
            //this.Refresh();
        }

        private void SetFlagPort(bool isRight)
        {
            //FlagPort.Visible = true;
            //if (isRight)
            //    FlagPort.Image = SmartWaterSystem.Properties.Resources.right;
            //else
            //    FlagPort.Image = SmartWaterSystem.Properties.Resources.cross1;
            //FlagPort.Update();
            //this.Refresh();
        }

        private void SetFlagIP(bool isRight)
        {
            //FlagIP.Visible = true;
            //if (isRight)
            //    FlagIP.Image = SmartWaterSystem.Properties.Resources.right;
            //else
            //    FlagIP.Image = SmartWaterSystem.Properties.Resources.cross1;
            //FlagIP.Update();
            //this.Refresh();
        }

        private void SetErrorFlag(string errorflag)
        {
            switch (errorflag)
            {
                case "writetime":
                    SetFlagTime(false);
                    break;
                case "writeremotesendtime":
                    SetFlagSendTime(false);
                    break;
                case "writestartendtime":
                    SetFlagStartEndTime(false);
                    break;
                case "writeInterval":
                    SetFlagInterval(false);
                    break;
                case "writeremoteswitch":
                    SetFlagRemoteSwitch(false);
                    break;
                case "writeportname":
                    SetFlagPort(false);
                    break;
                case "writeip":
                    SetFlagIP(false);
                    break;
                default :
                    break;
            }
        }

        #endregion

        private void btnGetStandValue_Click(object sender, EventArgs e)
        {
            if (!isSetting)
            {
                short id = 0;
                bool isError = false;
                try
                {
                    if (!Regex.IsMatch(txtRecID.Text, @"^\d{1,5}$"))
                    {
                        XtraMessageBox.Show("请输入记录仪ID", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtRecID.Focus();
                        return;
                    }
                    

                    EnableControls(false);
                    DisableRibbonBar();
                    DisableNavigateBar();
                    ShowWaitForm("", "正在读取启动值...");
                    id = Convert.ToInt16(txtRecID.Text);
                    btnGetStandValue.Enabled = false;
                    
                    if (GlobalValue.NoiseSerialPortOptData == null)
                        GlobalValue.NoiseSerialPortOptData = new NoiseSerialPortOptEntity();
                    GlobalValue.NoiseSerialPortOptData.ID = id;
                    
                    BeginSerialPortDelegate();
                    GlobalValue.SerialPortMgr.Send(SerialPortType.NoiseGetStandValue);
                }
                catch (TimeoutException)
                {
                    ShowDialog("记录仪" + id + "读取启动值超时！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    isError = true;
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    btnGetStandValue.Enabled = true;
                }
                catch (ArgumentNullException)
                {
                    ShowDialog("记录仪" + id + "读取启动值数据为空！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    isError = true;
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    btnGetStandValue.Enabled = true;
                }
                catch (Exception ex)
                {
                    ShowDialog(ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isError = true;
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    btnGetStandValue.Enabled = true;
                }
                finally
                {
                }
            }
        }

        private void btnSetStandValue_Click(object sender, EventArgs e)
        {
            if (!isSetting)
            {
                short id = 0;
                bool isError = false;
                try
                {
                    if (!Regex.IsMatch(txtRecID.Text, @"^\d{1,5}$"))
                    {
                        XtraMessageBox.Show("请输入记录仪ID", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtRecID.Focus();
                        return;
                    }
                    if (!Regex.IsMatch(txtStartStandValue.Text, @"^[4-9]\d{2}$"))
                    {
                        XtraMessageBox.Show("请输入有效的启动值[400-999]!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtStartStandValue.Focus();
                        return;
                    }

                    EnableControls(false);
                    DisableRibbonBar();
                    DisableNavigateBar();
                    ShowWaitForm("", "正在设置启动值...");

                    id = Convert.ToInt16(txtRecID.Text);
                    btnSetStandValue.Enabled = false;

                    if (GlobalValue.NoiseSerialPortOptData == null)
                        GlobalValue.NoiseSerialPortOptData = new NoiseSerialPortOptEntity();
                    GlobalValue.NoiseSerialPortOptData.ID = id;
                    GlobalValue.NoiseSerialPortOptData.StandValue = Convert.ToInt32(txtStartStandValue.Text);
                    BeginSerialPortDelegate();
                    GlobalValue.SerialPortMgr.Send(SerialPortType.NoiseSetStandValue);
                }
                catch (TimeoutException)
                {
                    ShowDialog("记录仪" + id + "启动值设置超时！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    isError = true;
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    btnSetStandValue.Enabled = true;
                }
                catch (ArgumentNullException)
                {
                    ShowDialog("记录仪" + id + "设置启动值数据为空！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    isError = true;
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    btnSetStandValue.Enabled = true;
                }
                catch (Exception ex)
                {
                    ShowDialog(ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isError = true;
                    EnableControls(true);
                    EnableRibbonBar();
                    EnableNavigateBar();
                    HideWaitForm();
                    btnSetStandValue.Enabled = true;
                }
                finally
                {
                }
            }
        }

    }
}
