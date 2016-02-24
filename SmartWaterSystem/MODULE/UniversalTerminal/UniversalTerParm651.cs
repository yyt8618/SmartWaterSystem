using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;
using Entity;
using Common;
using System.Collections.Generic;

namespace SmartWaterSystem
{
    public partial class UniversalTerParm651 : BaseView,IUniversalTerParm651
    {
        public UniversalTerParm651()
        {
            InitializeComponent();
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            timer_GetWaitCmd.Tick += new EventHandler(timer_GetWaitCmd_Tick);
        }

        void timer_GetWaitCmd_Tick(object sender, EventArgs e)
        {
            GlobalValue.MSMQMgr.SendMessage(new MSMQEntity(ConstValue.MSMQTYPE.Get_SL651_WaitSendCmd, ""));      //获取待发送SL651命令
            GlobalValue.MSMQMgr.SendMessage(new MSMQEntity(ConstValue.MSMQTYPE.Get_SL651_AllowOnlineFlag, cbIsOnLine.Checked.ToString()));
        }

        private void UniversalTerParm651_Load(object sender, EventArgs e)
        {
            SetSerialPortCtrlStatus();
        }

        public override void SerialPortEvent(bool Enabled)
        {
            if (SwitchComunication.IsOn)  //串口
            {
                ButtonEnabled(Enabled);
            }
        }

        private void ButtonEnabled(bool Enabled)
        {
            btnChPwd.Enabled = Enabled;
            btnReadBasicConfig.Enabled = Enabled;
            btnSetBasicConfig.Enabled = Enabled;
            btnDel.Enabled = Enabled;
            btnReadRunConfig.Enabled = Enabled;
            btnSetRunConfig.Enabled = Enabled;
            btnQueryElement.Enabled = Enabled;
            btnSetPrecipitationConstantCtrl.Enabled = Enabled;
            btnQueryTime.Enabled = Enabled;
            btnQueryVersion.Enabled = Enabled;
            btnQueryCurData.Enabled = Enabled;
            btnQueryEvent.Enabled = Enabled;
            btnQueryAlarm.Enabled = Enabled;
            btnSetTime.Enabled = Enabled;
            btnInit.Enabled = Enabled;
            btnInitFlash.Enabled = Enabled;
            btnReadManualSetParm.Enabled = Enabled;
            btnSetManualSetParm.Enabled = Enabled;
            btnQueryPrecipitation.Enabled = Enabled;
            btnCalibration1.Enabled = Enabled;
            btnCalibration2.Enabled = Enabled;
            btnReadTimeintervalReportTime.Enabled = Enabled;
            btnSetTimeintervalReportTime.Enabled = Enabled;
        }

        void MSMQMgr_MSMQEvent(object sender, MSMQEventArgs e)
        {
            if (e.msmqEntity != null && e.msmqEntity.MsgType == ConstValue.MSMQTYPE.Get_SL651_AllowOnlineFlag)  //获取SL651协议终端是否允许在线标志
            {
                if (e.msmqEntity.Msg.Trim() == "true")
                {
                    this.cbIsOnLine.CheckedChanged -= new System.EventHandler(this.cbIsOnLine_CheckedChanged);
                    cbIsOnLine.Checked = true;
                    this.cbIsOnLine.CheckedChanged += new System.EventHandler(this.cbIsOnLine_CheckedChanged);
                }
                else if (e.msmqEntity.Msg.Trim() == "false")
                {
                    this.cbIsOnLine.CheckedChanged -= new System.EventHandler(this.cbIsOnLine_CheckedChanged);
                    cbIsOnLine.Checked = false;
                    this.cbIsOnLine.CheckedChanged += new System.EventHandler(this.cbIsOnLine_CheckedChanged);
                }
            }
            if (e.msmqEntity != null && e.msmqEntity.MsgType == ConstValue.MSMQTYPE.Get_SL651_WaitSendCmd)
            {
                if (e.msmqEntity.MsgType == ConstValue.MSMQTYPE.Get_SL651_WaitSendCmd && !string.IsNullOrEmpty(e.msmqEntity.Msg))
                {
                    try
                    {
                        List<Package651> lstPack= JSONSerialize.JsonDeserialize<List<Package651>>(e.msmqEntity.Msg);
                        DataTable dt = (DataTable)gridControl_WaitCmd.DataSource;
                        if (dt == null)
                        {
                            dt = new DataTable("waitsendTable");
                            dt.Columns.Add("A5");
                            dt.Columns.Add("A4");
                            dt.Columns.Add("A3");
                            dt.Columns.Add("A2");
                            dt.Columns.Add("A1");
                            dt.Columns.Add("funcode");
                        }

                        if (lstPack != null)
                        {
                            bool exist = false;
                            for (int i = 0; i < dt.Rows.Count; i++)  //如果绑定的表中的数据在lstPack中不存在,则删除
                            {
                                exist = false;
                                foreach (Package651 pack in lstPack)
                                {
                                    if(("0x" + string.Format("{0:X2}", pack.A5)) == dt.Rows[i]["A5"].ToString() && ("0x" + string.Format("{0:X2}", pack.A4)) == dt.Rows[i]["A4"].ToString() &&
                                        ("0x" + string.Format("{0:X2}", pack.A3)) == dt.Rows[i]["A3"].ToString() &&("0x" + string.Format("{0:X2}", pack.A2)) == dt.Rows[i]["A2"].ToString() &&
                                            ("0x" + string.Format("{0:X2}", pack.A1)) == dt.Rows[i]["A1"].ToString() && ("0x" + string.Format("{0:X2}", pack.FUNCODE)) == dt.Rows[i]["funcode"].ToString())
                                    {
                                        exist = true;
                                        break;
                                    }
                                }
                                if (!exist)
                                {
                                    dt.Rows.RemoveAt(i);
                                }
                            }
                            foreach (Package651 pack in lstPack)   //如果在lstPack中存在,而在绑定的表中不存在,则添加
                            {
                                exist = false;
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (("0x" + string.Format("{0:X2}", pack.A5)) == dr["A5"].ToString() && ("0x" + string.Format("{0:X2}", pack.A4)) == dr["A4"].ToString() &&
                                        ("0x" + string.Format("{0:X2}", pack.A3)) == dr["A3"].ToString() && ("0x" + string.Format("{0:X2}", pack.A2)) == dr["A2"].ToString() &&
                                            ("0x" + string.Format("{0:X2}", pack.A1)) == dr["A1"].ToString() && ("0x" + string.Format("{0:X2}", pack.FUNCODE)) == dr["funcode"].ToString())
                                    {
                                        exist = true;
                                        break;
                                    }
                                }
                                if (!exist)
                                {
                                    DataRow dr = dt.NewRow();
                                    dr["A5"] = "0x" + string.Format("{0:X2}", pack.A5);
                                    dr["A4"] = "0x" + string.Format("{0:X2}", pack.A4);
                                    dr["A3"] = "0x" + string.Format("{0:X2}", pack.A3);
                                    dr["A2"] = "0x" + string.Format("{0:X2}", pack.A2);
                                    dr["A1"] = "0x" + string.Format("{0:X2}", pack.A1);
                                    dr["funcode"] = "0x" + string.Format("{0:X2}", pack.FUNCODE);
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                        else
                        {
                            dt.Rows.Clear();
                        }
                        SetWaitListView_Cmd(dt);
                    }
                    catch { 
                    }
                }
            }

        }

        private delegate void SetWaitListViewHandle(DataTable lstPack);
        private void SetWaitListView_Cmd(DataTable lstPack)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((SetWaitListViewHandle)delegate(DataTable lstparm)
                {
                    gridControl_WaitCmd.DataSource = lstparm;
                },lstPack);
            }
            else
            {
                gridControl_WaitCmd.DataSource = lstPack;
            }
        }

        private bool ValidateA5To1()
        {
            if (string.IsNullOrEmpty(txtA5.Text) || !Regex.IsMatch(txtA5.Text, "^[a-zA-Z0-9]{1,2}$"))
            {
                XtraMessageBox.Show("请填写A5", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtA5.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtA4.Text) || !Regex.IsMatch(txtA4.Text, "^[a-zA-Z0-9]{1,2}$"))
            {
                XtraMessageBox.Show("请填写A4", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtA4.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtA3.Text) || !Regex.IsMatch(txtA3.Text, "^[a-zA-Z0-9]{1,2}$"))
            {
                XtraMessageBox.Show("请填写A3", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtA3.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtA2.Text) || !Regex.IsMatch(txtA2.Text, "^[a-zA-Z0-9]{1,2}$"))
            {
                XtraMessageBox.Show("请填写A2", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtA2.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtA1.Text) || !Regex.IsMatch(txtA1.Text, "^[a-zA-Z0-9]{1,2}$"))
            {
                XtraMessageBox.Show("请填写A1", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtA1.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtPwd0.Text) || !Regex.IsMatch(txtPwd0.Text, "^[a-zA-Z0-9]{1,2}$"))
            {
                XtraMessageBox.Show("请填写密码", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPwd0.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtPwd1.Text) || !Regex.IsMatch(txtPwd1.Text, "^[a-zA-Z0-9]{1,2}$"))
            {
                XtraMessageBox.Show("请填写密码", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPwd1.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtCenterAddr.Text) || !Regex.IsMatch(txtCenterAddr.Text, "^[a-zA-Z0-9]{1,2}$"))
            {
                XtraMessageBox.Show("请填写中心站地址", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCenterAddr.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 设置基本配置控件默认值
        /// </summary>
        private void ClearBasicInfoCtrls()
        {
            #region 设置基本配置控件默认值
            txtCA1.Text = "";
            txtCA2.Text = "";
            txtCA3.Text = "";
            txtCA4.Text = "";
            txtCA5.Text = "";
            txtCCenterAddr.Text = "";
            combWorkType.SelectedIndex = -1;
            txtElements1.Text = "";
            txtElements2.Text = "";
            txtElements3.Text = "";
            txtElements4.Text = "";
            txtElements5.Text = "";
            txtElements6.Text = "";
            txtElements7.Text = "";
            txtElements8.Text = "";
            combChannelType.SelectedIndex = 0;
            combChannel.SelectedIndex = -1;
            txtIP1.Text = "";
            txtIP2.Text = "";
            txtIP3.Text = "";
            txtIP4.Text = "";
            txtCPort.Text = "";
            combStandbyChannel.SelectedIndex = -1;
            txtStandbyChTelnum.Text = "";
            combIdentifyNum.SelectedIndex = -1;
            txtTelNum.Text = "";
            #endregion
        }

        private void ClearRunInfoCtrls()
        {
            #region 设置运行配置控件默认值
            combPeriodInterval.SelectedIndex = -1;
            txtAddInterval.Text = "";
            txtPrecipitationStartTime.Text = "";
            txtSampling.Text = "";
            txtWaterLevelInterval.Text = "";
            combRainFallRPrecision.SelectedIndex = -1;
            combWaterLevelGaugePrecision.SelectedIndex = -1;
            txtRainFallLimit.Text = "";
            txtWaterLevelBasic.Text = "";
            txtWaterLevelAmendLimit.Text = "";
            txtAddtionWaterLevel.Text = "";
            txtAddtionWaterLevelUpLimit.Text = "";
            txtAddtionWaterLevelLowLimit.Text = "";
            #endregion
        }

        public override void OnSerialPortNotify(object sender, SerialPortEventArgs e)
        {
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651ChPwd)
            {
                #region 设置密码
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    if (e.Tag != null && (e.Tag is Universal651SerialPortEntity))
                    {
                        Universal651SerialPortEntity spEntity = (Universal651SerialPortEntity)e.Tag;
                        if (spEntity.IsOptPwd)
                        {
                            txtPwd0.Text = string.Format("{0:X2}", spEntity.CPwd0);
                            txtPwd1.Text = string.Format("{0:X2}", spEntity.CPwd1);
                        }
                        XtraMessageBox.Show("修改密码成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("修改密码失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651SetBasicInfo)
            {
                #region 设置基本信息
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    XtraMessageBox.Show("设置基本信息成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("设置基本信息失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651ReadBasicInfo)
            {
                #region 读取基本信息
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    if (e.Tag != null && (e.Tag is Universal651SerialPortEntity))
                    {
                        Universal651SerialPortEntity spEntity = (Universal651SerialPortEntity)e.Tag;
                        if (spEntity.IsOptA1_A5)
                        {
                            txtCA1.Text = string.Format("{0:X2}", spEntity.CA1);
                            txtCA2.Text = string.Format("{0:X2}", spEntity.CA2);
                            txtCA3.Text = string.Format("{0:X2}", spEntity.CA3);
                            txtCA4.Text = string.Format("{0:X2}", spEntity.CA4);
                            txtCA5.Text = string.Format("{0:X2}", spEntity.CA5);
                        }
                        if (spEntity.IsOptCenterAddr)
                            txtCCenterAddr.Text=string.Format("{0:X2}", spEntity.CCenterAddr);
                        //if (spEntity.IsOptPwd)
                        //{
                        //    txtCPwd0.Text = string.Format("{0:X2}", spEntity.CPwd0);
                        //    txtCPwd1.Text = string.Format("{0:X2}", spEntity.CPwd1);
                        //}
                        if (spEntity.IsOptWorkType)
                            combWorkType.SelectedIndex = (spEntity.WorkType - 1) < 0 ? 0 : spEntity.WorkType - 1;
                        if (spEntity.IsOptElements && spEntity.Elements!=null && spEntity.Elements.Length == 8)
                        {
                            txtElements1.Text = string.Format("{0:X2}",spEntity.Elements[0]);
                            txtElements2.Text = string.Format("{0:X2}", spEntity.Elements[1]);
                            txtElements3.Text = string.Format("{0:X2}", spEntity.Elements[2]);
                            txtElements4.Text = string.Format("{0:X2}", spEntity.Elements[3]);

                            txtElements5.Text = string.Format("{0:X2}", spEntity.Elements[4]);
                            txtElements6.Text = string.Format("{0:X2}", spEntity.Elements[5]);
                            txtElements7.Text = string.Format("{0:X2}", spEntity.Elements[6]);
                            txtElements8.Text = string.Format("{0:X2}", spEntity.Elements[7]);
                        }
                        if (spEntity.IsOptCh)
                        {
                            combChannel.SelectedIndex = spEntity.Channel;
                            txtIP1.Text = spEntity.Ip1;
                            txtIP2.Text = spEntity.Ip2;
                            txtIP3.Text = spEntity.Ip3;
                            txtIP4.Text = spEntity.Ip4;
                            txtCPort.Text = spEntity.Port.ToString();
                            combChannelType.SelectedIndex = spEntity.ChannelType;
                        }
                        if (spEntity.IsOptStandbyCh)
                        {
                            combStandbyChannel.SelectedIndex = spEntity.StandByCh;
                            txtStandbyChTelnum.Text = spEntity.StandbyChTelnum;
                            combChannelType.SelectedIndex = spEntity.ChannelType;
                        }
                        if (spEntity.IsOptIdentifyNum)
                        {
                            combIdentifyNum.SelectedIndex = (spEntity.IdentifyNum - 1) < 0 ? 0 : spEntity.IdentifyNum - 1;
                            txtTelNum.Text = spEntity.telnum;
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("读取基本信息失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651ReadRunInfo)
            {
                #region 读取运行信息
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    if (e.Tag != null && (e.Tag is Universal651SerialPortEntity))
                    {
                        Universal651SerialPortEntity spEntity = (Universal651SerialPortEntity)e.Tag;
                        if (spEntity.IsOptPeriodInterval)
                            combPeriodInterval.Text = spEntity.PeriodInterval.ToString();
                        if (spEntity.IsOptAddInterval)
                            txtAddInterval.Text = spEntity.AddInterval.ToString();
                        if (spEntity.IsOptPrecipitationStartTime)
                            txtPrecipitationStartTime.Text = spEntity.PrecipitationStartTime.ToString();
                        if (spEntity.IsOptSamplingInterval)
                            txtSampling.Text = spEntity.SamplingInterval.ToString();
                        if (spEntity.IsOptWaterLevelInterval)
                            txtWaterLevelInterval.Text = spEntity.WaterLevelInterval.ToString();
                        if (spEntity.IsOptRainFallPrecision)
                        {
                            combRainFallRPrecision.Text = spEntity.RainFallPrecision.ToString();
                        }
                        if (spEntity.IsOptWaterLevelGaugePrecision)
                        {
                            combWaterLevelGaugePrecision.Text = spEntity.WaterLevelGaugePrecision.ToString();
                        }
                        if (spEntity.IsOptRainFallLimit)
                            txtRainFallLimit.Text = spEntity.RainFallLimit.ToString();
                        if (spEntity.IsOptWaterLevelBasic)
                            txtWaterLevelBasic.Text = spEntity.WaterLevelBasic.ToString();
                        if (spEntity.IsOptWaterLevelAmendLimit)
                            txtWaterLevelAmendLimit.Text = spEntity.WaterLevelAmendLimit.ToString();
                        if (spEntity.IsOptAddtionWaterLevel)
                            txtAddtionWaterLevel.Text = spEntity.AddtionWaterLevel.ToString();
                        if (spEntity.IsOptAddtionWaterLevelUpLimit)
                            txtAddtionWaterLevelUpLimit.Text = spEntity.AddtionWaterLevelUpLimit.ToString();
                        if (spEntity.IsOptAddtionWaterLevelLowLimit)
                            txtAddtionWaterLevelLowLimit.Text = spEntity.AddtionWaterLevelLowLimit.ToString();
                    }
                }
                else
                {
                    XtraMessageBox.Show("读取运行配置失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651SetRunInfo)
            {
                #region 设置运行信息
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    XtraMessageBox.Show("设置运行信息成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("设置运行信息失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651QueryElements)
            {
                #region 查询指定要素
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    XtraMessageBox.Show("查询指定要素成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("查询指定要素失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651QueryPrecipitation)
            {
                #region 查询时段降水量
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    XtraMessageBox.Show("查询时段降水量成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("查询时段降水量失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651SetPreConstCtrl)
            {
                #region 设置水量定值控制命令
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    XtraMessageBox.Show("设置水量定值控制命令成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("设置水量定值控制命令失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651QueryTime)
            {
                #region 通用终端SL651查询时间
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    if (e.Tag != null && (e.Tag is Universal651SerialPortEntity))
                    {
                        Universal651SerialPortEntity spEntity = (Universal651SerialPortEntity)e.Tag;
                        if (spEntity.IsOptQueryTime)
                            XtraMessageBox.Show("查询时间成功,当前时间:" + spEntity.QueryTime, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("查询时间失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651QueryVer)
            {
                #region 通用终端SL651查询版本
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    if (e.Tag != null && (e.Tag is Universal651SerialPortEntity))
                    {
                        Universal651SerialPortEntity spEntity = (Universal651SerialPortEntity)e.Tag;
                        if (spEntity.IsOptQueryVer)
                            XtraMessageBox.Show("查询版本成功,当前版本:" + spEntity.Ver, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("查询版本失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651QueryCurData)
            {
                #region 通用终端SL651查询实时数据
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    if (e.Tag != null && (e.Tag is Universal651SerialPortEntity))
                    {
                        Universal651SerialPortEntity spEntity = (Universal651SerialPortEntity)e.Tag;
                        if (spEntity.IsOptQueryCurData)
                            XtraMessageBox.Show("查询实时数据成功,数据:" + spEntity.QueryCurData, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("查询实时数据失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651QueryEvent)
            {
                #region 通用终端SL651查询事件
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    if (e.Tag != null && (e.Tag is Universal651SerialPortEntity))
                    {
                        Universal651SerialPortEntity spEntity = (Universal651SerialPortEntity)e.Tag;
                        if (spEntity.IsOptQueryEvent)
                            XtraMessageBox.Show("查询事件成功,数据:" + spEntity.QueryEvent, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("查询事件失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651QueryAlarm)
            {
                #region 通用终端SL651查询状态和报警
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    if (e.Tag != null && (e.Tag is Universal651SerialPortEntity))
                    {
                        Universal651SerialPortEntity spEntity = (Universal651SerialPortEntity)e.Tag;
                        if (spEntity.IsOptQueryAlarm)
                            XtraMessageBox.Show("查询状态和报警成功,数据:" + spEntity.QueryAlarm, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("查询状态和报警失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651SetTime)
            {
                #region 设置通用终端SL651时间
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    XtraMessageBox.Show("设置时间成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("设置时间失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651InitFlash)
            {
                #region 通用终端SL651初始化FLASH
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    XtraMessageBox.Show("初始化FLASH成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("初始化FLASH失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651Init)
            {
                #region 通用终端SL651恢复出厂设置
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    XtraMessageBox.Show("恢复出厂设置成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("恢复出厂设置失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651QueryManualSetParm)
            {
                #region 通用终端SL651查询人工置数
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    if (e.Tag != null && (e.Tag is Universal651SerialPortEntity))
                    {
                        Universal651SerialPortEntity spEntity = (Universal651SerialPortEntity)e.Tag;
                        if (spEntity.IsOptQueryManualSetParm)
                        {
                            XtraMessageBox.Show("查询人工置数成功,数据:" + spEntity.QueryManualSetParm, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    XtraMessageBox.Show("查询人工置数成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("查询人工置数失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651SetManualSetParm)
            {
                #region 通用终端SL651设置人工置数
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    XtraMessageBox.Show("设置人工置数成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("设置人工置数失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651SetCalibration)
            {
                #region 通用终端SL651设置相对水位校准值
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    if (e.Tag != null && (e.Tag is Universal651SerialPortEntity))
                    {
                        Universal651SerialPortEntity spEntity = (Universal651SerialPortEntity)e.Tag;
                        if (spEntity.IsOptSetCalibration)
                            XtraMessageBox.Show("设置水位校准值成功!校准值:" + spEntity.SetCalibration, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("设置水位校准值失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651ReadTimeintervalReportTime)
            {
                #region 通用终端SL651读取均匀时段报上传时间
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    if (e.Tag != null && (e.Tag is Universal651SerialPortEntity))
                    {
                        Universal651SerialPortEntity spEntity = (Universal651SerialPortEntity)e.Tag;
                        if (spEntity.IsOptTimeintervalReportTime)
                        {
                            seTimeintervalReportTime.Value = spEntity.TimeintervalReportTime;
                            //XtraMessageBox.Show("读取均匀时段报上传时间成功!上传时间:" + spEntity.TimeintervalReportTime, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("读取均匀时段报上传时间失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.Universal651SetTimeintervalReportTime)
            {
                #region 通用终端SL651设置均匀时段报上传时间
                OnSerialPortNotifyEnable();
                if (e.TransactStatus == TransStatus.Success)
                {
                    XtraMessageBox.Show("设置均匀时段报上传时间成功!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("设置均匀时段报上传时间失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
            }
        }

        private void OnSerialPortNotifyEnable()
        {
            this.Enabled = true;
            HideWaitForm();
            EnableControls(true);
            EnableRibbonBar();
            EnableNavigateBar();

            GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
        }

        #region button events
        private void EnableControls(bool enable)
        {
            btnChPwd.Enabled = enable;
            btnReadBasicConfig.Enabled = enable;
            btnSetBasicConfig.Enabled = enable;
            btnDel.Enabled = enable;
            btnReadRunConfig.Enabled = enable;
            btnSetRunConfig.Enabled = enable;
            btnQueryElement.Enabled = enable;
            btnSetPrecipitationConstantCtrl.Enabled = enable;
            btnQueryTime.Enabled = enable;
            btnQueryVersion.Enabled = enable;
            btnQueryCurData.Enabled = enable;
            btnQueryEvent.Enabled = enable;
            btnQueryAlarm.Enabled = enable;
            btnSetTime.Enabled = enable;
            btnInit.Enabled = enable;
            btnInitFlash.Enabled = enable;
            SwitchComunication.Enabled = enable;
            btnReadManualSetParm.Enabled = enable;
            btnSetManualSetParm.Enabled = enable;
            btnQueryPrecipitation.Enabled = enable;
            btnCalibration1.Enabled = enable;
            btnCalibration2.Enabled = enable;
            btnReadTimeintervalReportTime.Enabled = enable;
            btnSetTimeintervalReportTime.Enabled = enable;
        }

        private void ButtonSend(string tipmsg, SerialPortType spPort,Package651 pack,byte End)
        {
            if (SwitchComunication.IsOn)  //SerialPort
            {
                pack.End = End;
                byte[] bsenddata = pack.ToResponseArray();
                pack.CS = Package651.crc16(bsenddata, bsenddata.Length);

                GlobalValue.SerialPort651OptData = pack;
                EnableControls(false);
                DisableRibbonBar();
                DisableNavigateBar();
                ShowWaitForm("", tipmsg);
                BeginSerialPortDelegate();
                Application.DoEvents();
                SetStaticItem(tipmsg);
                GlobalValue.SerialPortMgr.Send(spPort);
            }
            else   //GPRS
            {
                pack.End = End;
                MSMQEntity msmqentity = new MSMQEntity();
                msmqentity.MsgType = ConstValue.MSMQTYPE.SL651_Cmd;
                msmqentity.Pack651 = pack;
                GlobalValue.MSMQMgr.SendMessage(msmqentity);
            }
        }

        private Package651 PartInitPack()
        {
            Package651 pack = new Package651();
            pack.A5 = Convert.ToByte(txtA5.Text, 16);
            pack.A4 = Convert.ToByte(txtA4.Text, 16);
            pack.A3 = Convert.ToByte(txtA3.Text, 16);
            pack.A2 = Convert.ToByte(txtA2.Text, 16);
            pack.A1 = Convert.ToByte(txtA1.Text, 16);
            pack.CenterAddr = Convert.ToByte(txtCenterAddr.Text);
            pack.dt = new byte[6];
            pack.dt[0] = ConvertHelper.StringToByte((DateTime.Now.Year - 2000).ToString())[0];
            pack.dt[1] = ConvertHelper.StringToByte(DateTime.Now.Month.ToString().PadLeft(2,'0'))[0];
            pack.dt[2] = ConvertHelper.StringToByte(DateTime.Now.Day.ToString().PadLeft(2, '0'))[0];
            pack.dt[3] = ConvertHelper.StringToByte(DateTime.Now.Hour.ToString().PadLeft(2, '0'))[0];
            pack.dt[4] = ConvertHelper.StringToByte(DateTime.Now.Minute.ToString().PadLeft(2, '0'))[0];
            pack.dt[5] = ConvertHelper.StringToByte(DateTime.Now.Second.ToString().PadLeft(2, '0'))[0];
            pack.PWD = new byte[2];
            pack.PWD[0] = Convert.ToByte(txtPwd0.Text, 16);
            pack.PWD[1] = Convert.ToByte(txtPwd1.Text, 16);

            return pack;
        }
        private void btnQueryTime_Click(object sender, EventArgs e)
        {
            if (ValidateA5To1())
            {
                Package651 pack = PartInitPack();
                pack.FUNCODE = (byte)SL651_COMMAND.QueryTime;

                byte[] lens = BitConverter.GetBytes((ushort)(8));
                pack.L0 = lens[0];
                pack.L1 = lens[1];
                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;
                pack.AddrFlag = PackageDefine.AddrFlagHeader;

                ButtonSend("正在查询时钟...", SerialPortType.Universal651QueryTime, pack, PackageDefine.ENQ);
            }
        }

        private void btnQueryVersion_Click(object sender, EventArgs e)
        {
            if (ValidateA5To1())
            {
                Package651 pack = PartInitPack();
                pack.FUNCODE = (byte)SL651_COMMAND.QueryVer;

                byte[] lens = BitConverter.GetBytes((ushort)(8));
                pack.L0 = lens[0];
                pack.L1 = lens[1];
                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;
                pack.AddrFlag = PackageDefine.AddrFlagHeader;

                ButtonSend("正在查询版本...", SerialPortType.Universal651QueryVer, pack, PackageDefine.ENQ);
            }
        }

        private void btnQueryCurData_Click(object sender, EventArgs e)
        {
            if (ValidateA5To1())
            {
                Package651 pack = PartInitPack();
                pack.FUNCODE = (byte)SL651_COMMAND.QueryCurData;

                byte[] lens = BitConverter.GetBytes((ushort)(8));
                pack.L0 = lens[0];
                pack.L1 = lens[1];
                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;
                pack.AddrFlag = PackageDefine.AddrFlagHeader;

                ButtonSend("正在查询实时数据...", SerialPortType.Universal651QueryCurData, pack, PackageDefine.ENQ);
            }
        }

        private void btnQueryEvent_Click(object sender, EventArgs e)
        {
            if (ValidateA5To1())
            {
                Package651 pack = PartInitPack();
                pack.FUNCODE = (byte)SL651_COMMAND.QueryEvent;

                byte[] lens = BitConverter.GetBytes((ushort)(8));
                pack.L0 = lens[0];
                pack.L1 = lens[1];
                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;
                pack.AddrFlag = PackageDefine.AddrFlagHeader;

                ButtonSend("正在查询事件...", SerialPortType.Universal651QueryEvent, pack, PackageDefine.ENQ);
            }
        }

        private void btnQueryAlarm_Click(object sender, EventArgs e)
        {
            if (ValidateA5To1())
            {
                Package651 pack = PartInitPack();
                pack.FUNCODE = (byte)SL651_COMMAND.QueryAlarm;

                byte[] lens = BitConverter.GetBytes((ushort)(8));
                pack.L0 = lens[0];
                pack.L1 = lens[1];
                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;
                pack.AddrFlag = PackageDefine.AddrFlagHeader;

                ButtonSend("正在查询状态和报警...", SerialPortType.Universal651QueryAlarm, pack, PackageDefine.ENQ);
            }
        }

        private void btnSetTime_Click(object sender, EventArgs e)
        {
            if (ValidateA5To1())
            {
                Package651 pack = PartInitPack();
                pack.FUNCODE = (byte)SL651_COMMAND.SetTime;

                byte[] lens = BitConverter.GetBytes((ushort)(8));
                pack.L0 = lens[0];
                pack.L1 = lens[1];
                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;
                pack.AddrFlag = PackageDefine.AddrFlagHeader;

                ButtonSend("正在设置时钟...", SerialPortType.Universal651SetTime, pack, PackageDefine.ENQ);
            }
        }

        private void btnInitFlash_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == XtraMessageBox.Show("是否初始化固态存储数据?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                return;
            if (ValidateA5To1())
            {
                Package651 pack = PartInitPack();
                pack.FUNCODE = (byte)SL651_COMMAND.InitFlash;

                byte[] lens = BitConverter.GetBytes((ushort)(10));
                pack.L0 = lens[0];
                pack.L1 = lens[1];
                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlagHeader;
                pack.Data = new byte[2];
                pack.Data[0] = 0x97; //初始化固态标识符
                pack.Data[1] = 0x00;

                ButtonSend("正在初始化固态存储数据...", SerialPortType.Universal651InitFlash, pack, PackageDefine.ENQ);
            }
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == XtraMessageBox.Show("是否恢复出厂设置?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                return;
            if (ValidateA5To1())
            {
                Package651 pack = PartInitPack();
                pack.FUNCODE = (byte)SL651_COMMAND.Init;

                byte[] lens = BitConverter.GetBytes((ushort)(10));
                pack.L0 = lens[0];
                pack.L1 = lens[1];
                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlagHeader;
                pack.Data = new byte[2];
                pack.Data[0] = 0x98; //恢复遥测站出厂设置标识符
                pack.Data[1] = 0x00;

                ButtonSend("正在恢复出厂设置...", SerialPortType.Universal651Init, pack, PackageDefine.ENQ);
            }
        }

        private void btnChPwd_Click(object sender, EventArgs e)
        {
            #region Change Password
            if (ValidateA5To1())
            {
                if (string.IsNullOrEmpty(txtNewPwd0.Text) || !Regex.IsMatch(txtNewPwd0.Text, "^\\d+$"))
                {
                    XtraMessageBox.Show("请填写密码", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNewPwd0.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtNewPwd1.Text) || !Regex.IsMatch(txtNewPwd1.Text, "^\\d+$"))
                {
                    XtraMessageBox.Show("请填写密码", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNewPwd1.Focus();
                    return;
                }

                Package651 pack = PartInitPack();
                pack.FUNCODE = (byte)SL651_COMMAND.ChPwd;

                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlagHeader;
                pack.Data = new byte[8];
                pack.Data[0] = 0x03; //密码标识符 0x03,0x10
                pack.Data[1] = 0x10;
                pack.Data[2] = Convert.ToByte(txtPwd0.Text);  //旧密码
                pack.Data[3] = Convert.ToByte(txtPwd1.Text);
                pack.Data[4] = 0x03; //密码标识符 0x03,0x10
                pack.Data[5] = 0x10;
                pack.Data[6] = Convert.ToByte(txtNewPwd0.Text, 16);
                pack.Data[7] = Convert.ToByte(txtNewPwd1.Text, 16);

                byte[] lens = BitConverter.GetBytes((ushort)(8 + pack.Data.Length));
                pack.L0 = lens[0];
                pack.L1 = lens[1];
                ButtonSend("正在设置密码...", SerialPortType.Universal651ChPwd, pack, PackageDefine.ENQ);
            }
            #endregion
        }

        private void btnReadBasicConfig_Click(object sender, EventArgs e)
        {
            #region ReadBasicConfig
            if (ValidateA5To1())
            {
                if (!(cbTerAddr.Checked | cbCenterAddr.Checked | cbChannel.Checked | cbStandbyChannel.Checked | cbWorkType.Checked | cbElements.Checked | cbIdentifyNum.Checked))
                {
                    XtraMessageBox.Show("请选择至少一项读取!");
                    return;
                }

                Package651 pack = PartInitPack();
                pack.FUNCODE = (byte)SL651_COMMAND.ReadBasiConfig;

                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlagHeader;

                List<byte> lstCentent = new List<byte>();
                if (cbTerAddr.Checked)
                    lstCentent.AddRange(PackageDefine.AddrFlagParm);  //遥测站地址标识符
                if (cbCenterAddr.Checked)
                    lstCentent.AddRange(PackageDefine.CenterAddrFlag);   //中心站地址标识符
                //if (cbPwd.Checked)
                //    lstCentent.AddRange(PackageDefine.PwdFlag);   //密码

                byte[] channelflag=new byte[2];byte[] standbychannelflag=new byte[2];
                if (cbChannel.Checked || cbStandbyChannel.Checked)
                {
                    if (combChannelType.SelectedIndex == 0)
                    {
                        Array.Copy(PackageDefine.Channel1Flag, channelflag, 2);
                        Array.Copy(PackageDefine.StandbyChannel1Flag, standbychannelflag, 2);
                    }
                    else if (combChannelType.SelectedIndex == 1)
                    {
                        Array.Copy(PackageDefine.Channel2Flag, channelflag, 2);
                        Array.Copy(PackageDefine.StandbyChannel2Flag, standbychannelflag, 2);
                    }
                    else if (combChannelType.SelectedIndex == 2)
                    {
                        Array.Copy(PackageDefine.Channel3Flag, channelflag, 2);
                        Array.Copy(PackageDefine.StandbyChannel3Flag, standbychannelflag, 2);
                    }
                    else if (combChannelType.SelectedIndex == 3)
                    {
                        Array.Copy(PackageDefine.Channel4Flag, channelflag, 2);
                        Array.Copy(PackageDefine.StandbyChannel4Flag, standbychannelflag, 2);
                    }
                }
                channelflag[1] &= 0x00;
                standbychannelflag[1] &= 0x00;

                if (cbChannel.Checked)
                    lstCentent.AddRange(channelflag); //中1-4主信道类型及地址
                if (cbStandbyChannel.Checked)
                    lstCentent.AddRange(standbychannelflag);  //中1-4备用信道类型及地址

                if (cbWorkType.Checked)
                    lstCentent.AddRange(PackageDefine.WorkTypeFlag);//工作方式标识符
                if (cbElements.Checked)
                    lstCentent.AddRange(PackageDefine.ElementsFlag);  //采集要素标识符
                if (cbIdentifyNum.Checked)
                    lstCentent.AddRange(PackageDefine.IdentifyNumFlag);//遥测站通信设备识别号标识符
                pack.Data = lstCentent.ToArray();

                byte[] lens = BitConverter.GetBytes((ushort)(8 + pack.Data.Length));
                pack.L0 = lens[0];
                pack.L1 = lens[1];

                ClearBasicInfoCtrls();

                ButtonSend("正在读取基本配置...", SerialPortType.Universal651ReadBasicInfo, pack, PackageDefine.ENQ);
            }
            #endregion
        }

        private void btnSetBasicConfig_Click(object sender, EventArgs e)
        {
            #region SetBasicConfig
            if (ValidateA5To1())
            {
                if (!(cbTerAddr.Checked | cbCenterAddr.Checked | cbChannel.Checked | cbStandbyChannel.Checked | cbWorkType.Checked | cbElements.Checked | cbIdentifyNum.Checked))
                {
                    XtraMessageBox.Show("请选择至少一项设置!");
                    return;
                }

                if (cbTerAddr.Checked)
                {
                    if (string.IsNullOrEmpty(txtCA5.Text) || !Regex.IsMatch(txtCA5.Text, "^[a-zA-Z0-9]{1,2}$"))
                    {
                        XtraMessageBox.Show("请填写终端地址A5", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtCA5.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtCA4.Text) || !Regex.IsMatch(txtCA4.Text, "^[a-zA-Z0-9]{1,2}$"))
                    {
                        XtraMessageBox.Show("请填写终端地址A4", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtCA4.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtCA3.Text) || !Regex.IsMatch(txtCA3.Text, "^[a-zA-Z0-9]{1,2}$"))
                    {
                        XtraMessageBox.Show("请填写终端地址A3", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtCA3.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtCA2.Text) || !Regex.IsMatch(txtCA2.Text, "^[a-zA-Z0-9]{1,2}$"))
                    {
                        XtraMessageBox.Show("请填写终端地址A2", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtCA2.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtCA1.Text) || !Regex.IsMatch(txtCA1.Text, "^[a-zA-Z0-9]{1,2}$"))
                    {
                        XtraMessageBox.Show("请填写终端地址A1", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtCA1.Focus();
                        return;
                    }
                }

                if (cbCenterAddr.Checked)
                {
                    if (string.IsNullOrEmpty(txtCCenterAddr.Text) || !Regex.IsMatch(txtCCenterAddr.Text, "^[a-zA-Z0-9]{1,2}$"))
                    {
                        XtraMessageBox.Show("请填写中心站地址", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtCCenterAddr.Focus();
                        return;
                    }
                }

                //if (cbPwd.Checked)
                //{
                //    if (string.IsNullOrEmpty(txtCPwd0.Text) || !Regex.IsMatch(txtCPwd0.Text, "^[a-zA-Z0-9]{1,2}$"))
                //    {
                //        XtraMessageBox.Show("请填写密码", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        txtCPwd0.Focus();
                //        return;
                //    }
                //    if (string.IsNullOrEmpty(txtCPwd1.Text) || !Regex.IsMatch(txtCPwd1.Text, "^[a-zA-Z0-9]{1,2}$"))
                //    {
                //        XtraMessageBox.Show("请填写密码", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        txtCPwd1.Focus();
                //        return;
                //    }
                //}

                if (cbChannel.Checked)
                {
                    if (combChannel.SelectedIndex < 0)
                    {
                        XtraMessageBox.Show("请选择信道类型", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        combChannel.Focus();
                        return;
                    }

                    if (combChannel.SelectedIndex > 0)
                    {
                        if (string.IsNullOrEmpty(txtIP1.Text) || string.IsNullOrEmpty(txtIP2.Text) ||
                        string.IsNullOrEmpty(txtIP3.Text) || string.IsNullOrEmpty(txtIP4.Text))
                        {
                            XtraMessageBox.Show("请填写完整的IP地址!");
                            txtIP1.Focus();
                            return;
                        }

                        if (!Regex.IsMatch(txtCPort.Text, @"^\d{3,5}$"))
                        {
                            XtraMessageBox.Show("请输入端口号!");
                            txtCPort.Focus();
                            return;
                        }
                    }
                }

                if (cbStandbyChannel.Checked)
                {
                    if (combStandbyChannel.SelectedIndex < 0)
                    {
                        XtraMessageBox.Show("请选择备用信道类型", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        combStandbyChannel.Focus();
                        return;
                    }
                    if (combStandbyChannel.SelectedIndex > 0)
                    {
                        if (!Regex.IsMatch(txtStandbyChTelnum.Text, @"^\d{11,12}$"))
                        {
                            XtraMessageBox.Show("请输入备用信道号码!");
                            txtStandbyChTelnum.Focus();
                            return;
                        }
                    }
                }

                if (cbWorkType.Checked && combWorkType.SelectedIndex < 0)
                {
                    XtraMessageBox.Show("请选择工作方式", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    combWorkType.Focus();
                    return;
                }

                if (cbElements.Checked && (string.IsNullOrEmpty(txtElements1.Text) || string.IsNullOrEmpty(txtElements2.Text) || string.IsNullOrEmpty(txtElements3.Text) || string.IsNullOrEmpty(txtElements4.Text) || 
                string.IsNullOrEmpty(txtElements5.Text) || string.IsNullOrEmpty(txtElements6.Text) || string.IsNullOrEmpty(txtElements7.Text) || string.IsNullOrEmpty(txtElements8.Text)))
                {
                    XtraMessageBox.Show("请输入正确的采集要素", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtElements1.Focus();
                    return;
                }

                if (cbIdentifyNum.Checked)
                {
                    if (combIdentifyNum.SelectedIndex < 0)
                    {
                        XtraMessageBox.Show("请选择卡类型", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        combIdentifyNum.Focus();
                        return;
                    }
                    if (!Regex.IsMatch(txtTelNum.Text, "^[0,1]\\d{10,11}$"))
                    {
                        XtraMessageBox.Show("请输入卡号!");
                        txtTelNum.Focus();
                        return;
                    }
                }

                Package651 pack = PartInitPack();
                pack.FUNCODE = (byte)SL651_COMMAND.SetBasicConfig;

                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlagHeader;

                List<byte> lstCentent = new List<byte>();
                if (cbTerAddr.Checked)
                {
                    lstCentent.AddRange(PackageDefine.AddrFlagParm);  //遥测站地址标识符
                    lstCentent.AddRange(new byte[] { Convert.ToByte(txtCA5.Text,16), Convert.ToByte(txtCA4.Text,16), 
                        Convert.ToByte(txtCA3.Text,16), Convert.ToByte(txtCA2.Text,16), Convert.ToByte(txtCA1.Text,16) });
                }
                if (cbCenterAddr.Checked)
                {
                    lstCentent.AddRange(PackageDefine.CenterAddrFlag);   //中心站地址标识符
                    lstCentent.Add(ConvertHelper.HexToBCD(Convert.ToByte(txtCCenterAddr.Text)));
                }
                //if (cbPwd.Checked)
                //{
                //    lstCentent.AddRange(PackageDefine.PwdFlag);   //密码
                //    lstCentent.AddRange(new byte[] { Convert.ToByte(txtCPwd0.Text, 16), Convert.ToByte(txtCPwd1.Text, 16) });
                //}
                if (cbChannel.Checked)
                {
                    if (combChannelType.SelectedIndex == 0)
                        lstCentent.AddRange(PackageDefine.Channel1Flag); //中1主信道类型及地址
                    else if (combChannelType.SelectedIndex == 1)
                        lstCentent.AddRange(PackageDefine.Channel2Flag); //中2主信道类型及地址
                    else if (combChannelType.SelectedIndex == 2)
                        lstCentent.AddRange(PackageDefine.Channel3Flag); //中3主信道类型及地址
                    else if (combChannelType.SelectedIndex == 3)
                        lstCentent.AddRange(PackageDefine.Channel4Flag); //中4主信道类型及地址

                    lstCentent.Add(Convert.ToByte(combChannel.SelectedIndex));
                    string ip = txtIP1.Text.PadLeft(3, '0') + txtIP2.Text.PadLeft(3, '0') + txtIP3.Text.PadLeft(3, '0') + txtIP4.Text.PadLeft(3, '0');
                    lstCentent.AddRange(ConvertHelper.StringToByte(ip));

                    string port = txtCPort.Text.PadLeft(6, '0');
                    lstCentent.AddRange(ConvertHelper.StringToByte(port));
                }
                if (cbStandbyChannel.Checked)
                {
                    if (combChannelType.SelectedIndex == 0)
                        lstCentent.AddRange(PackageDefine.StandbyChannel1Flag); //中1备用信道类型及地址
                    else if (combChannelType.SelectedIndex == 1)
                        lstCentent.AddRange(PackageDefine.StandbyChannel2Flag); //中2备用信道类型及地址
                    else if (combChannelType.SelectedIndex == 2)
                        lstCentent.AddRange(PackageDefine.StandbyChannel3Flag); //中3备用信道类型及地址
                    else if (combChannelType.SelectedIndex == 3)
                        lstCentent.AddRange(PackageDefine.StandbyChannel4Flag); //中4备用信道类型及地址

                    lstCentent.Add(Convert.ToByte(combStandbyChannel.SelectedIndex));
                    //if (combStandbyChannel.SelectedIndex > 0)
                    lstCentent.AddRange(ConvertHelper.StringToByte(txtStandbyChTelnum.Text.Trim().PadLeft(12, '0')));
                    //else
                        //lstCentent.AddRange(ConvertHelper.StringToByte("".PadLeft(12, '0')));  //补齐12个0
                }
                if (cbWorkType.Checked)
                {
                    lstCentent.AddRange(PackageDefine.WorkTypeFlag);//工作方式标识符
                    lstCentent.Add(Convert.ToByte(combWorkType.SelectedIndex + 1));
                }
                if (cbElements.Checked)
                {
                    lstCentent.AddRange(PackageDefine.ElementsFlag);  //采集要素标识符
                    lstCentent.Add(Convert.ToByte(txtElements1.Text,16));
                    lstCentent.Add(Convert.ToByte(txtElements2.Text, 16));
                    lstCentent.Add(Convert.ToByte(txtElements3.Text, 16));
                    lstCentent.Add(Convert.ToByte(txtElements4.Text, 16));
                    lstCentent.Add(Convert.ToByte(txtElements5.Text, 16));
                    lstCentent.Add(Convert.ToByte(txtElements6.Text, 16));
                    lstCentent.Add(Convert.ToByte(txtElements7.Text, 16));
                    lstCentent.Add(Convert.ToByte(txtElements8.Text, 16));
                }
                if (cbIdentifyNum.Checked)
                {
                    lstCentent.AddRange(PackageDefine.IdentifyNumFlag);//遥测站通信设备识别号标识符
                    //lstCentent.Add(Convert.ToByte(combIdentifyNum.SelectedIndex + 1));
                    //lstCentent.AddRange(PackageDefine.TelNumFlag);  //卡识别号/主备信道标识符
                    char[] telchars = ((combIdentifyNum.SelectedIndex + 1).ToString() + txtTelNum.Text.PadLeft(12,'0')).ToCharArray();
                    if (telchars != null && telchars.Length > 0)
                    {
                        for (int i = 0; i < telchars.Length; i++)
                        {
                            lstCentent.Add(Convert.ToByte((ushort)telchars[i]));  //ASCII  +(ushort)30
                        }
                    }
                }

                pack.Data = lstCentent.ToArray();
                byte[] lens = BitConverter.GetBytes((ushort)(8 + pack.Data.Length));
                pack.L0 = lens[0];
                pack.L1 = lens[1];

                ButtonSend("正在设置基本配置...", SerialPortType.Universal651SetBasicInfo, pack, PackageDefine.ENQ);
            }
            #endregion
        }

        private void btnReadRunConfig_Click(object sender, EventArgs e)
        {
            #region ReadRunConfig
            if (ValidateA5To1())
            {
                if (!(cbPeriodInterval.Checked | cbAddInterval.Checked | cbPrecipitationStartTime.Checked | cbSampling.Checked | cbWaterLevelInterval.Checked | cbRainFallRPrecision.Checked |
                    cbWaterLevelGaugePrecision.Checked | cbRainFallLimit.Checked | cbWaterLevelBasic.Checked | cbWaterLevelAmendLimit.Checked | cbAddtionWaterLevel.Checked | cbAddtionWaterLevelUpLimit.Checked |
                    cbAddtionWaterLevelLowLimit.Checked))
                {
                    XtraMessageBox.Show("请选择至少一项读取!");
                    return;
                }

                Package651 pack = PartInitPack();
                pack.FUNCODE = (byte)SL651_COMMAND.ReadRunConfig;

                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlagHeader;

                List<byte> lstCentent = new List<byte>();
                if (cbPeriodInterval.Checked)
                    lstCentent.AddRange(PackageDefine.PeriodIntervalFlag);  //定时报时间间隔
                if (cbAddInterval.Checked)
                    lstCentent.AddRange(PackageDefine.AddIntervalFlag);   //加报时间间隔
                if (cbPrecipitationStartTime.Checked)
                    lstCentent.AddRange(PackageDefine.PrecipitationStartTimeFlag);   //降水量日起始时间
                if (cbSampling.Checked)
                    lstCentent.AddRange(PackageDefine.SamplingFlag); //采样间隔
                if (cbWaterLevelInterval.Checked)
                    lstCentent.AddRange(PackageDefine.WaterLevelIntervalFlag);//水位数据存储间隔
                if (cbRainFallRPrecision.Checked)
                    lstCentent.AddRange(PackageDefine.RainFallPrecisionFlag);  //雨量计分辨率
                if (cbWaterLevelGaugePrecision.Checked)
                    lstCentent.AddRange(PackageDefine.WaterLevelGaugePrecisionFlag);//水位计分辨率
                if (cbRainFallLimit.Checked)
                    lstCentent.AddRange(PackageDefine.RainFallLimitFlag);//雨量加报阀值
                if (cbWaterLevelBasic.Checked)
                    lstCentent.AddRange(PackageDefine.WaterLevelBasicFlag);//水位基值
                if (cbWaterLevelAmendLimit.Checked)
                    lstCentent.AddRange(PackageDefine.WaterLevelAmendLimitFlag);//水位修正基值
                if (cbAddtionWaterLevel.Checked)
                    lstCentent.AddRange(PackageDefine.AddtionWaterLevelFlag);//加报水位
                if (cbAddtionWaterLevelUpLimit.Checked)
                    lstCentent.AddRange(PackageDefine.AddtionWaterLevelUpLimitFlag); //加报水位以上加报阀值
                if (cbAddtionWaterLevelLowLimit.Checked)
                    lstCentent.AddRange(PackageDefine.AddtionWaterLevelLowLimitFlag); //加报水位以下加报阀值
                pack.Data = lstCentent.ToArray();
                byte[] lens = BitConverter.GetBytes((ushort)(8 + pack.Data.Length));
                pack.L0 = lens[0];
                pack.L1 = lens[1];

                ClearRunInfoCtrls();

                ButtonSend("正在读取运行配置...", SerialPortType.Universal651ReadRunInfo, pack, PackageDefine.ENQ);
            }
            #endregion
        }

        private void btnSetRunConfig_Click(object sender, EventArgs e)
        {
            #region SetRunConfig
            if (ValidateA5To1())
            {
                if (!(cbPeriodInterval.Checked | cbAddInterval.Checked | cbPrecipitationStartTime.Checked | cbSampling.Checked | cbWaterLevelInterval.Checked | cbRainFallRPrecision.Checked |
                    cbWaterLevelGaugePrecision.Checked | cbRainFallLimit.Checked | cbWaterLevelBasic.Checked | cbWaterLevelAmendLimit.Checked | cbAddtionWaterLevel.Checked | cbAddtionWaterLevelUpLimit.Checked |
                    cbAddtionWaterLevelLowLimit.Checked))
                {
                    XtraMessageBox.Show("请选择至少一项设置!");
                    return;
                }

                if (cbPeriodInterval.Checked && combPeriodInterval.SelectedIndex < 0)
                {
                    XtraMessageBox.Show("请选择定时报时间间隔", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    combPeriodInterval.Focus();
                    return;
                }
                if (cbAddInterval.Checked && string.IsNullOrEmpty(txtAddInterval.Text))
                {
                    XtraMessageBox.Show("请输入加报时间间隔", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtAddInterval.Focus();
                    return;
                }
                if (cbPrecipitationStartTime.Checked && !Regex.IsMatch(txtPrecipitationStartTime.Text, @"(^0|[1-9]|1[0-9]{1}|2[0-3]{1})$"))
                {
                    XtraMessageBox.Show("请输入正确降水量日起始时间(0-23h)", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrecipitationStartTime.Focus();
                    return;
                }
                if (cbSampling.Checked && !Regex.IsMatch(txtSampling.Text, @"^\d{1,4}$"))
                {
                    XtraMessageBox.Show("请输入正确采样间隔数据(0-9999s)", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSampling.Focus();
                    return;
                }
                if (cbWaterLevelInterval.Checked && !Regex.IsMatch(txtWaterLevelInterval.Text, @"^([1-9]{1}|[1-5]{1}[0-9]{1})$"))
                {
                    XtraMessageBox.Show("请输入正确的水位数据存储间隔(1-59min)", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtWaterLevelInterval.Focus();
                    return;
                }
                if (cbRainFallRPrecision.Checked && combRainFallRPrecision.SelectedIndex < 0)
                {
                    XtraMessageBox.Show("请选择雨量计分辨力", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    combRainFallRPrecision.Focus();
                    return;
                }
                if (cbWaterLevelGaugePrecision.Checked && combWaterLevelGaugePrecision.SelectedIndex < 0)
                {
                    XtraMessageBox.Show("请选择水位计分辨力", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    combWaterLevelGaugePrecision.Focus();
                    return;
                }
                if (cbRainFallLimit.Checked && !Regex.IsMatch(txtRainFallLimit.Text, @"^\d{1,2}$"))
                {
                    XtraMessageBox.Show("请输入雨量加报阀值", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRainFallLimit.Focus();
                    return;
                }
                if (cbWaterLevelBasic.Checked && !Regex.IsMatch(txtWaterLevelBasic.Text, @"^[-]?\d{1,4}([.]\d{1,3})?$")) //(7,3)
                {
                    XtraMessageBox.Show("请输入水位基值", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtWaterLevelBasic.Focus();
                    return;
                }
                if (cbWaterLevelAmendLimit.Checked && !Regex.IsMatch(txtWaterLevelAmendLimit.Text, @"^[-]?\d{1,2}([.]\d{1,3})?$")) //(5,3)
                {
                    XtraMessageBox.Show("请输入水位修正基值", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtWaterLevelAmendLimit.Focus();
                    return;
                }
                if (cbAddtionWaterLevel.Checked && !Regex.IsMatch(txtAddtionWaterLevel.Text, @"^\d{1,2}([.]\d{1,2})?$")) //(4,2)
                {
                    XtraMessageBox.Show("请输入加报水位值", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtAddtionWaterLevel.Focus();
                    return;
                }
                if (cbAddtionWaterLevelUpLimit.Checked && !Regex.IsMatch(txtAddtionWaterLevelUpLimit.Text, @"^\d{1}([.]\d{1,2})?$")) //(3,2)
                {
                    XtraMessageBox.Show("请输入加报水位以上修正阀值", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtAddtionWaterLevelUpLimit.Focus();
                    return;
                }
                if (cbAddtionWaterLevelLowLimit.Checked && !Regex.IsMatch(txtAddtionWaterLevelLowLimit.Text, @"^\d{1}([.]\d{1,2})?$")) //(3,2)
                {
                    XtraMessageBox.Show("请输入加报水位以下修正阀值", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtAddtionWaterLevelLowLimit.Focus();
                    return;
                }

                Package651 pack = PartInitPack();
                pack.FUNCODE = (byte)SL651_COMMAND.SetRunConfig;

                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlagHeader;

                List<byte> lstCentent = new List<byte>();
                if (cbPeriodInterval.Checked)
                {
                    lstCentent.AddRange(PackageDefine.PeriodIntervalFlag);  //定时报时间间隔
                    lstCentent.Add(ConvertHelper.StringToByte(combPeriodInterval.Text.PadLeft(2,'0'))[0]);
                }
                if (cbAddInterval.Checked)
                {
                    lstCentent.AddRange(PackageDefine.AddIntervalFlag);   //加报时间间隔
                    lstCentent.Add(ConvertHelper.StringToByte(txtAddInterval.Text.PadLeft(2, '0'))[0]);
                    //lstCentent.Add(ConvertHelper.HexToBCD(Convert.ToByte(txtAddInterval.Text)));
                }
                if (cbPrecipitationStartTime.Checked)
                {
                    lstCentent.AddRange(PackageDefine.PrecipitationStartTimeFlag);   //降水量日起始时间
                    lstCentent.Add(ConvertHelper.StringToByte(txtPrecipitationStartTime.Text.PadLeft(2, '0'))[0]);
                    //lstCentent.Add(ConvertHelper.HexToBCD(Convert.ToByte(txtPrecipitationStartTime.Text)));
                }
                if (cbSampling.Checked)
                {
                    lstCentent.AddRange(PackageDefine.SamplingFlag); //采样间隔
                    string str_sampling = txtSampling.Text.PadLeft(4, '0');
                    lstCentent.AddRange(ConvertHelper.StringToByte(str_sampling));
                }
                if (cbWaterLevelInterval.Checked)
                {
                    lstCentent.AddRange(PackageDefine.WaterLevelIntervalFlag);//水位数据存储间隔
                    //lstCentent.Add(ConvertHelper.HexToBCD(Convert.ToByte(txtWaterLevelInterval.Text)));
                    lstCentent.Add(ConvertHelper.StringToByte(txtWaterLevelInterval.Text.PadLeft(2, '0'))[0]);
                }
                if (cbRainFallRPrecision.Checked)
                {
                    lstCentent.AddRange(PackageDefine.RainFallPrecisionFlag);  //雨量计分辨率
                    byte b_pre  = ConvertHelper.BCDToHex((int)(Convert.ToSingle(combRainFallRPrecision.Text.Trim()) * 10));
                    lstCentent.Add(b_pre);
                }
                if (cbWaterLevelGaugePrecision.Checked)
                {
                    lstCentent.AddRange(PackageDefine.WaterLevelGaugePrecisionFlag);//水位计分辨率
                    byte b_pre = ConvertHelper.BCDToHex((int)(Convert.ToSingle(combWaterLevelGaugePrecision.Text.Trim()) * 10));
                    lstCentent.Add(b_pre);
                }
                if (cbRainFallLimit.Checked)
                {
                    lstCentent.AddRange(PackageDefine.RainFallLimitFlag);//雨量加报阀值
                    lstCentent.Add(ConvertHelper.StringToByte(txtRainFallLimit.Text.PadLeft(2,'0'))[0]);
                }
                if (cbWaterLevelBasic.Checked)
                {
                    lstCentent.AddRange(PackageDefine.WaterLevelBasicFlag);//水位基值
                    int basic = (int)(Convert.ToDouble(txtWaterLevelBasic.Text)*1000);
                    if (basic < 0)
                    {
                        lstCentent.Add(0xFF);  //负数一个字节，正数三个字节,负数第一个字节是0xFF
                        basic *= -1;
                        //lstCentent.AddRange(ConvertHelper.StringToByte((basic).ToString().PadLeft(6, '0')));
                    }
                    //else
                        lstCentent.AddRange(ConvertHelper.StringToByte((basic).ToString().PadLeft(8, '0')));
                }
                if (cbWaterLevelAmendLimit.Checked)
                {
                    lstCentent.AddRange(PackageDefine.WaterLevelAmendLimitFlag);//水位修正基值 
                    int basic = (int)(Convert.ToDouble(txtWaterLevelAmendLimit.Text)*1000);
                    if (basic < 0)
                    {
                        lstCentent.Add(0xFF);  //负数一个字节，正数三个字节,负数第一个字节是0xFF
                        basic *= -1;
                        //lstCentent.AddRange(ConvertHelper.StringToByte((basic * 1000).ToString().PadLeft(4, '0')));
                    }
                    //else
                        lstCentent.AddRange(ConvertHelper.StringToByte((basic).ToString().PadLeft(6, '0')));
                }
                if (cbAddtionWaterLevel.Checked)
                {
                    lstCentent.AddRange(PackageDefine.AddtionWaterLevelFlag);//加报水位  
                    int waterlevel = (int)(Convert.ToDouble(txtAddtionWaterLevel.Text) * 100);
                    lstCentent.AddRange(ConvertHelper.StringToByte(waterlevel.ToString().PadLeft(4, '0')));
                    //lstCentent.AddRange(ConvertHelper.StrToBCD(waterlevel.ToString()));
                }
                if (cbAddtionWaterLevelUpLimit.Checked)
                {
                    lstCentent.AddRange(PackageDefine.AddtionWaterLevelUpLimitFlag); //加报水位以上加报阀值
                    int waterlevelLimit = (int)(Convert.ToDouble(txtAddtionWaterLevelUpLimit.Text) * 100);
                    lstCentent.AddRange(ConvertHelper.StringToByte(waterlevelLimit.ToString().PadLeft(4, '0')));
                    //byte[] bs = ConvertHelper.StrToBCD(waterlevelLimit.ToString());
                    //if (bs.Length == 1)
                    //    lstCentent.Add(0x00);

                    //lstCentent.AddRange(bs);
                }
                if (cbAddtionWaterLevelLowLimit.Checked)
                {
                    lstCentent.AddRange(PackageDefine.AddtionWaterLevelLowLimitFlag); //加报水位以下加报阀值
                    int waterlevelLimit = (int)(Convert.ToDouble(txtAddtionWaterLevelLowLimit.Text) * 100);
                    lstCentent.AddRange(ConvertHelper.StringToByte(waterlevelLimit.ToString().PadLeft(4, '0')));
                    //byte[] bs = ConvertHelper.StrToBCD(waterlevelLimit.ToString());
                    //if (bs.Length == 1)
                    //    lstCentent.Add(0x00);

                    //lstCentent.AddRange(bs);
                }

                pack.Data = lstCentent.ToArray();
                byte[] lens = BitConverter.GetBytes((ushort)(8 + pack.Data.Length));
                pack.L0 = lens[0];
                pack.L1 = lens[1];

                ButtonSend("正在设置运行配置...", SerialPortType.Universal651SetRunInfo, pack, PackageDefine.ENQ);
            }
            #endregion
        }

        private void btnQueryElement_Click(object sender, EventArgs e)
        {
            #region QueryElement
            if (ValidateA5To1())
            {
                int checkedcount = 0;
                if (cbParmPrecipitation.Checked)
                    checkedcount++;
                if (cbParmDayPrecipitation.Checked)
                    checkedcount++;
                if (cbParmInstantWaterLevel.Checked)
                    checkedcount++;
                if (cbParmRainFallAddup.Checked)
                    checkedcount++;
                if (cb1minPrecipitation.Checked)
                    checkedcount++;
                if (cb5minPrecipitation.Checked)
                    checkedcount++;
                if (cb10minPrecipitation.Checked)
                    checkedcount++;
                if (cb30minPrecipitation.Checked)
                    checkedcount++;
                if (cbParm1h5minPrecipitation.Checked)
                    checkedcount++;
                if (cbParm1h5minWaterLevel.Checked)
                    checkedcount++;
                if (checkedcount==0)
                {
                    XtraMessageBox.Show("请选择至少一项读取!");
                    return;
                }

                Package651 pack = PartInitPack();
                pack.FUNCODE = (byte)SL651_COMMAND.QueryElements;

                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlagHeader;

                List<byte> lstCentent = new List<byte>();
                //lstCentent.Add(0x48);   //河道

                bool haveObservationTime = false;  //是否有观测时间
                if (cbParmPrecipitation.Checked)
                    lstCentent.AddRange(PackageDefine.PrecipitationFlag);  //当前降水量
                if (cbParmDayPrecipitation.Checked)
                    lstCentent.AddRange(PackageDefine.DayPrecipitationFlag);  //日降水量
                if (cbParmRainFallAddup.Checked)
                    lstCentent.AddRange(PackageDefine.RainFallAddupFlag);   //累计降雨量
                if (cbParmInstantWaterLevel.Checked)
                    lstCentent.AddRange(PackageDefine.InstantWaterlevelFlag);   //瞬时水位
                if (cb1minPrecipitation.Checked)
                    lstCentent.AddRange(PackageDefine.Precipitation1min);   //1min时段降水
                if (cb5minPrecipitation.Checked)
                    lstCentent.AddRange(PackageDefine.Precipitation5min);   //5min时段降水
                if (cb10minPrecipitation.Checked)
                    lstCentent.AddRange(PackageDefine.Precipitation10min);   //10min时段降水
                if (cb30minPrecipitation.Checked)
                    lstCentent.AddRange(PackageDefine.Precipitation30min);   //30min时段降水
                if (cbParm1h5minPrecipitation.Checked)
                {
                    lstCentent.AddRange(PackageDefine.Precipitation5MinFlag);  //1小时5分钟时段雨量
                    haveObservationTime = true;
                }
                if (cbParm1h5minWaterLevel.Checked)
                {
                    lstCentent.AddRange(PackageDefine.Waterlevel5MinFlag);  //1小时5分钟间隔相对水位
                    haveObservationTime = true;
                }
                if (haveObservationTime)
                {
                    DateTime dt = DateTime.Now;
                    int itmp = dt.Minute % 5;  //往前取整5分钟的时间
                    if (itmp > 0)
                        dt.AddMinutes((-1) * itmp);  

                    List<byte> lstdt = new List<byte>();
                    lstdt.AddRange(PackageDefine.ObservationTimeFlag);
                    lstdt.Add(ConvertHelper.StringToByte((dt.Year - 2000).ToString())[0]);
                    lstdt.Add(ConvertHelper.StringToByte(dt.Month.ToString().PadLeft(2, '0'))[0]);
                    lstdt.Add(ConvertHelper.StringToByte(dt.Day.ToString().PadLeft(2, '0'))[0]);
                    lstdt.Add(ConvertHelper.StringToByte(dt.Hour.ToString().PadLeft(2, '0'))[0]);
                    lstdt.Add(ConvertHelper.StringToByte(dt.Minute.ToString().PadLeft(2, '0'))[0]);

                    lstCentent.InsertRange(0, lstdt);
                }
                pack.Data = lstCentent.ToArray();

                byte[] lens = BitConverter.GetBytes((ushort)(8 + pack.Data.Length));
                pack.L0 = lens[0];
                pack.L1 = lens[1];

                ButtonSend("正在查询要素...", SerialPortType.Universal651QueryElements, pack, PackageDefine.ENQ);
            }
            #endregion
        }

        private void btnQueryPrecipitation_Click(object sender, EventArgs e)
        {
            #region QueryPrecipitation
            if (ValidateA5To1())
            {
                int checkedcount = 0;
                if (cbParm1h5minPrecipitation.Checked)
                    checkedcount++;
                if (cbParm1h5minWaterLevel.Checked)
                    checkedcount++;
                if (rgPrecipitation.SelectedIndex>-1)
                    checkedcount++;
                if (checkedcount==0)
                {
                    XtraMessageBox.Show("请选择至少一项读取!");
                    return;
                }
                if (checkedcount >1)
                {
                    XtraMessageBox.Show("不能选择多项读取!");
                    return;
                }

                Package651 pack = PartInitPack();
                pack.FUNCODE = (byte)SL651_COMMAND.QueryPrecipitation;

                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlagHeader;

                List<byte> lstCentent = new List<byte>();

                DateTime begindt = DateTime.Now;  //起始时间
                byte[] timestep = new byte[] { PackageDefine.TimeStepFlag[0], PackageDefine.TimeStepFlag[1], 0x00, 0x00, 0x05 };
                
                if (cbParm1h5minPrecipitation.Checked)
                {
                    lstCentent.AddRange(PackageDefine.Precipitation5MinFlag);  //1小时5分钟时段雨量  时间步长默认5分钟
                    begindt = begindt.AddHours(-1);
                }
                else if (cbParm1h5minWaterLevel.Checked)
                {
                    lstCentent.AddRange(PackageDefine.Waterlevel5MinFlag);  //1小时5分钟间隔相对水位  时间步长默认5分钟
                    begindt = begindt.AddHours(-1);
                }
                else
                {
                    switch (rgPrecipitation.SelectedIndex)
                    {
                        case 0:
                            begindt = begindt.AddHours(-1); //1h时段降水
                            break;
                        case 1:
                            begindt = begindt.AddHours(-2); //2h时段降水
                            break;
                        case 2:
                            begindt = begindt.AddHours(-3); //3h时段降水
                            break;
                        case 3:
                            begindt = begindt.AddHours(-6); //6h时段降水
                            break;
                        case 4:
                            begindt = begindt.AddHours(-12); //12h时段降水
                            break;
                    }
                    switch (combTimeStep.SelectedIndex)
                    {
                        case 0:
                            timestep = new byte[] { PackageDefine.TimeStepFlag[0], PackageDefine.TimeStepFlag[1], 0x00, 0x00, 0x01 };
                            lstCentent.AddRange(PackageDefine.Precipitation1min);
                            break;
                        case 1:
                            timestep = new byte[] { PackageDefine.TimeStepFlag[0], PackageDefine.TimeStepFlag[1], 0x00, 0x00, 0x05 };
                            lstCentent.AddRange(PackageDefine.Precipitation5min);
                            break;
                        case 2:
                            timestep = new byte[] { PackageDefine.TimeStepFlag[0], PackageDefine.TimeStepFlag[1], 0x00, 0x00, 0x10 };
                            lstCentent.AddRange(PackageDefine.Precipitation10min);
                            break;
                        case 3:
                            timestep = new byte[] { PackageDefine.TimeStepFlag[0], PackageDefine.TimeStepFlag[1], 0x00, 0x00, 0x30 };
                            lstCentent.AddRange(PackageDefine.Precipitation30min);
                            break;
                        case 4:
                            timestep = new byte[] { PackageDefine.TimeStepFlag[0], PackageDefine.TimeStepFlag[1], 0x00, 0x01, 0x00 };
                            lstCentent.AddRange(PackageDefine.Precipitation1h);
                            break;
                        case 5:
                            timestep = new byte[] { PackageDefine.TimeStepFlag[0], PackageDefine.TimeStepFlag[1], 0x00, 0x02, 0x00 };
                            lstCentent.AddRange(PackageDefine.Precipitation2h);
                            break;
                        case 6:
                            timestep = new byte[] { PackageDefine.TimeStepFlag[0], PackageDefine.TimeStepFlag[1], 0x00, 0x03, 0x00 };
                            lstCentent.AddRange(PackageDefine.Precipitation3h);
                            break;
                        case 7:
                            timestep = new byte[] { PackageDefine.TimeStepFlag[0], PackageDefine.TimeStepFlag[1], 0x00, 0x06, 0x00 };
                            lstCentent.AddRange(PackageDefine.Precipitation6h);
                            break;
                        case 8:
                            timestep = new byte[] { PackageDefine.TimeStepFlag[0], PackageDefine.TimeStepFlag[1], 0x00, 0x12, 0x00 };
                            lstCentent.AddRange(PackageDefine.Precipitation12h);
                            break;
                    }
                }
                
                List<byte> lstdt = new List<byte>();
                lstdt.Add(ConvertHelper.StringToByte((begindt.Year - 2000).ToString())[0]);
                lstdt.Add(ConvertHelper.StringToByte(begindt.Month.ToString().PadLeft(2, '0'))[0]);
                lstdt.Add(ConvertHelper.StringToByte(begindt.Day.ToString().PadLeft(2, '0'))[0]);
                lstdt.Add(ConvertHelper.StringToByte(begindt.Hour.ToString().PadLeft(2, '0'))[0]);

                DateTime enddt = DateTime.Now;  //结束时间
                lstdt.Add(ConvertHelper.StringToByte((enddt.Year - 2000).ToString())[0]);
                lstdt.Add(ConvertHelper.StringToByte(enddt.Month.ToString().PadLeft(2, '0'))[0]);
                lstdt.Add(ConvertHelper.StringToByte(enddt.Day.ToString().PadLeft(2, '0'))[0]);
                lstdt.Add(ConvertHelper.StringToByte(enddt.Hour.ToString().PadLeft(2, '0'))[0]);

                //时间步长
                lstdt.AddRange(timestep);

                lstCentent.InsertRange(0, lstdt);
                    
                pack.Data = lstCentent.ToArray();

                byte[] lens = BitConverter.GetBytes((ushort)(8 + pack.Data.Length));
                pack.L0 = lens[0];
                pack.L1 = lens[1];

                ButtonSend("正在查询时段降水量...", SerialPortType.Universal651QueryPrecipitation, pack, PackageDefine.ENQ);
            }
            #endregion
        }

        private void btnSetPrecipitationConstantCtrl_Click(object sender, EventArgs e)
        {
            if (ValidateA5To1())
            {
                Package651 pack = PartInitPack();
                pack.FUNCODE = (byte)SL651_COMMAND.PrecipitationConstantCtrl;

                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlagHeader;

                if (combPrecipitationConstantCtrl.SelectedIndex == 0)
                    pack.Data = new byte[] { 0xFF };   //投入
                else
                    pack.Data = new byte[] { 0x00 };   //投出

                byte[] lens = BitConverter.GetBytes((ushort)(8 + pack.Data.Length));
                pack.L0 = lens[0];
                pack.L1 = lens[1];

                ButtonSend("正在设置水量定值控制...", SerialPortType.Universal651SetPreConstCtrl, pack, PackageDefine.ENQ);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            int[] selectrows=gridView_WaitCmd.GetSelectedRows();
            if (selectrows != null && selectrows.Length > 0)
            {
                string a1 = gridView_WaitCmd.GetRowCellValue(selectrows[0], "A1").ToString();
                string a2 = gridView_WaitCmd.GetRowCellValue(selectrows[0], "A2").ToString();
                string a3 = gridView_WaitCmd.GetRowCellValue(selectrows[0], "A3").ToString();
                string a4 = gridView_WaitCmd.GetRowCellValue(selectrows[0], "A4").ToString();
                string a5 = gridView_WaitCmd.GetRowCellValue(selectrows[0], "A5").ToString();
                string funcode = gridView_WaitCmd.GetRowCellValue(selectrows[0], "funcode").ToString();
                MSMQEntity msmqentity = new MSMQEntity();
                msmqentity.A1 = Convert.ToByte(a1.Replace("0x",""), 16);
                msmqentity.A2 = Convert.ToByte(a2.Replace("0x", ""), 16);
                msmqentity.A3 = Convert.ToByte(a3.Replace("0x", ""), 16);
                msmqentity.A4 = Convert.ToByte(a4.Replace("0x", ""), 16);
                msmqentity.A5 = Convert.ToByte(a5.Replace("0x", ""), 16);
                msmqentity.SL651Funcode = Convert.ToByte(funcode.Replace("0x", ""), 16);
                msmqentity.MsgType = ConstValue.MSMQTYPE.Del_SL651_WaitSendCmd;
                GlobalValue.MSMQMgr.SendMessage(msmqentity);
            }
        }

        private void btnReadManualSetParm_Click(object sender, EventArgs e)
        {
            #region 查询人工置数
            if (ValidateA5To1())
            {
                Package651 pack = PartInitPack();
                pack.FUNCODE = (byte)SL651_COMMAND.QueryManualSetParm;

                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlagHeader;

                //pack.Data = lstCentent.ToArray();

                byte[] lens = BitConverter.GetBytes((ushort)(8));
                pack.L0 = lens[0];
                pack.L1 = lens[1];

                ButtonSend("正在查询人工置数...", SerialPortType.Universal651QueryManualSetParm, pack, PackageDefine.ENQ);
            }
            #endregion
        }

        private void btnAddManualSetParm_Click(object sender, EventArgs e)
        {
            if (combManualSetParm.SelectedIndex < 0)
            {
                XtraMessageBox.Show("请选择人工置数类型标识符!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                combManualSetParm.Focus();
                return;
            }
            if (!Regex.IsMatch(txtManualSetParm.Text.Trim(), "^[0-9a-fA-F]{2}( [0-9a-fA-F]{2}){0,}$"))
            {
                XtraMessageBox.Show("请填写人工置数内容!格式(16进制):12 34 56 78 90 ab ...", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtManualSetParm.Focus();
                return;
            }
            int parmlen = Convert.ToInt32(combManualSetParm.Text.Substring(combManualSetParm.Text.IndexOf('-')+1));
            string[] strsetparms=txtManualSetParm.Text.Trim().Split(new char[]{' '});
            if (strsetparms.Length != parmlen)
            {
                XtraMessageBox.Show("请人工置数内容字节数不正确!", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtManualSetParm.Focus();
                return;
            }

            string str_parm = combManualSetParm.Text.Substring(combManualSetParm.Text.IndexOf('(') + 1, 5);
            str_parm += " 20 ";   //20是空格
            str_parm += txtManualSetParm.Text.Trim();

            if (!string.IsNullOrEmpty(memo_ManualSetParm.Text)) //&& 
            {
                if (!memo_ManualSetParm.Text.EndsWith(" "))
                    memo_ManualSetParm.Text += " 20 ";
                else
                    memo_ManualSetParm.Text += "20 ";
            }

            memo_ManualSetParm.Text += str_parm;
            txtManualSetParm.Text = "";
        }

        private void btnSetManualSetParm_Click(object sender, EventArgs e)
        {
            #region 设置人工置数
            if (ValidateA5To1())
            {
                try
                {
                    if (!Regex.IsMatch(memo_ManualSetParm.Text.Trim(), "^[0-9a-fA-F]{2}( [0-9a-fA-F]{2}){0,}$"))
                    {
                        XtraMessageBox.Show("请填写需要设置的人工置数内容!格式(16进制):12 34 56 78 90 ab ...", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtManualSetParm.Focus();
                        return;
                    }

                    Package651 pack = PartInitPack();
                    pack.FUNCODE = (byte)SL651_COMMAND.SetManualSetParm;

                    pack.CStart = PackageDefine.CStart;

                    pack.SNum = new byte[2];
                    pack.SNum[0] = 0;
                    pack.SNum[1] = 0;
                    pack.IsUpload = false;

                    pack.AddrFlag = PackageDefine.AddrFlagHeader;

                    string setparm = memo_ManualSetParm.Text.Trim();
                    setparm = setparm.Replace(" ", "");
                    byte[] data = ConvertHelper.StringToByte(setparm);
                    pack.Data = data;

                    byte[] lens = BitConverter.GetBytes((ushort)(8 + data.Length));
                    pack.L0 = lens[0];
                    pack.L1 = lens[1];

                    ButtonSend("正在设置人工置数...", SerialPortType.Universal651SetManualSetParm, pack, PackageDefine.ENQ);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("发生异常,请检查设置的人工置数数据是否正确! ex:" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            #endregion
        }

        private void btnCalibration_Click(object sender, EventArgs e)
        {
            #region 第1、2路校准值
            if (ValidateA5To1())
            {
                try
                {
                    Package651 pack = PartInitPack();
                    SimpleButton btn=(SimpleButton)sender;
                    if (btn.Text == "校准水位1")
                        pack.FUNCODE = (byte)SL651_COMMAND.SetCalibration1;
                    else
                        pack.FUNCODE = (byte)SL651_COMMAND.SetCalibration2;

                    pack.CStart = PackageDefine.CStart;

                    pack.SNum = new byte[2];
                    pack.SNum[0] = 0;
                    pack.SNum[1] = 0;
                    pack.IsUpload = false;

                    pack.AddrFlag = PackageDefine.AddrFlagHeader;
                    byte[] lens = BitConverter.GetBytes((ushort)(8));
                    pack.L0 = lens[0];
                    pack.L1 = lens[1];

                    ButtonSend("正在设置" + btn.Text + "...", SerialPortType.Universal651SetCalibration, pack, PackageDefine.ENQ);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("发生异常,请检查设置的人工置数数据是否正确! ex:" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            #endregion
        }

        private void btnReadTimeintervalReportTime_Click(object sender, EventArgs e)
        {
            #region 读取均匀时段报上传时间
            if (ValidateA5To1())
            {
                try
                {
                    Package651 pack = PartInitPack();
                    pack.FUNCODE = (byte)SL651_COMMAND.ReadTimeIntervalReportTime;

                    pack.CStart = PackageDefine.CStart;

                    pack.SNum = new byte[2];
                    pack.SNum[0] = 0;
                    pack.SNum[1] = 0;
                    pack.IsUpload = false;

                    pack.AddrFlag = PackageDefine.AddrFlagHeader;

                    byte[] lens = BitConverter.GetBytes((ushort)(8));
                    pack.L0 = lens[0];
                    pack.L1 = lens[1];

                    ButtonSend("正在读取均匀时段报上传时间...", SerialPortType.Universal651ReadTimeintervalReportTime, pack, PackageDefine.ENQ);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("发生异常! ex:" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            #endregion
        }

        private void btnSetTimeintervalReportTime_Click(object sender, EventArgs e)
        {
            #region 设置均匀时段报上传时间
            if (ValidateA5To1())
            {
                try
                {
                    Package651 pack = PartInitPack();
                    pack.FUNCODE = (byte)SL651_COMMAND.SetTimeIntervalReportTime;

                    pack.CStart = PackageDefine.CStart;

                    pack.SNum = new byte[2];
                    pack.SNum[0] = 0;
                    pack.SNum[1] = 0;
                    pack.IsUpload = false;

                    pack.AddrFlag = PackageDefine.AddrFlagHeader;

                    string setparm = seTimeintervalReportTime.Text.Trim();
                    setparm = setparm.Replace(" ", "");
                    byte[] data = new byte[] { Convert.ToByte(seTimeintervalReportTime.Value) };
                    pack.Data = data;

                    byte[] lens = BitConverter.GetBytes((ushort)(8 + data.Length));
                    pack.L0 = lens[0];
                    pack.L1 = lens[1];

                    ButtonSend("正在设置均匀时段报上传时间...", SerialPortType.Universal651SetTimeintervalReportTime, pack, PackageDefine.ENQ);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("发生异常,请检查设置的上传时间是否正确! ex:" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            #endregion
        }
        #endregion

        #region key event
        void txt_onebyte_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextEdit txtbox = (TextEdit)sender;
            if (e.KeyChar == '\b') //backspace
            {
                e.Handled = false;
            }
            else if (e.KeyChar == ' ' || e.KeyChar == '\r' || e.KeyChar == '.')
            {
                e.Handled = false;
            }
            else if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar >= 65 && e.KeyChar <= 70) || (e.KeyChar >= 97 && e.KeyChar <= 102) || e.KeyChar == 120)
            {
                string text = txtbox.Text;
                if (txtbox.SelectedText.Length > 0)
                {
                    text = text.Remove(txtbox.SelectionStart, txtbox.SelectionLength);

                }
                text = text.Insert(txtbox.SelectionStart, e.KeyChar.ToString());

                if (Regex.IsMatch(text, @"^(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[0-9]?[0-9])$")
                    || Regex.IsMatch(text, @"^(0x|0x[0-9a-fA-F]{1,2})$"))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
                e.Handled = true;
        }

        void txt_twobyte_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextEdit txtbox = (TextEdit)sender;
            if (e.KeyChar == '\b') //backspace
            {
                e.Handled = false;
            }
            else if (e.KeyChar == ' ' || e.KeyChar == '\r' || e.KeyChar == '.')
            {
                e.Handled = false;
            }
            else if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar >= 65 && e.KeyChar <= 70) || (e.KeyChar >= 97 && e.KeyChar <= 102) || e.KeyChar == 120)
            {
                string text = txtbox.Text;
                if (txtbox.SelectedText.Length > 0)
                {
                    text = text.Remove(txtbox.SelectionStart, txtbox.SelectionLength);
                    text = text.Insert(txtbox.SelectionStart, e.KeyChar.ToString());
                }
                text = text.Insert(txtbox.SelectionStart, e.KeyChar.ToString());
                if ((Regex.IsMatch(text, @"^\d{1,5}$") && Convert.ToInt32(text) <= 65535)
                || Regex.IsMatch(text, @"^(0x|0x[0-9a-fA-F]{1,4})$"))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
                e.Handled = true;
        }

        void txt_morebyte_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextEdit txtbox = (TextEdit)sender;
            if (e.KeyChar == '\b') //backspace
            {
                e.Handled = false;
            }
            else if (e.KeyChar == ' ' || e.KeyChar == '\r' || e.KeyChar == '.')
            {
                e.Handled = false;
            }
            else if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar >= 65 && e.KeyChar <= 70) || (e.KeyChar >= 97 && e.KeyChar <= 102) || e.KeyChar == 120)
            {
                string text = txtbox.Text;
                if (txtbox.SelectedText.Length > 0)
                {
                    text = text.Remove(txtbox.SelectionStart, txtbox.SelectionLength);

                }
                text = text.Insert(txtbox.SelectionStart, e.KeyChar.ToString());

                if (Regex.IsMatch(text, @"^[0-9a-fA-F ]+$"))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
                e.Handled = true;
        }
        #endregion

        private void combChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combChannel.SelectedIndex <= 0)
            {
                txtIP1.Enabled = false;
                txtIP2.Enabled = false;
                txtIP3.Enabled = false;
                txtIP4.Enabled = false;
                txtCPort.Enabled = false;
            }
            else
            {
                txtIP1.Enabled = true;
                txtIP2.Enabled = true;
                txtIP3.Enabled = true;
                txtIP4.Enabled = true;
                txtCPort.Enabled = true;
            }
        }

        private void combStandbyChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combStandbyChannel.SelectedIndex <= 0)
                txtStandbyChTelnum.Enabled = false;
            else
                txtStandbyChTelnum.Enabled = true;
        }

        private void combChannelType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbIsOnLine_CheckedChanged(object sender, EventArgs e)
        {
            GlobalValue.MSMQMgr.SendMessage(new MSMQEntity(ConstValue.MSMQTYPE.Set_SL651_AllowOnlineFlag, cbIsOnLine.Checked.ToString()));
        }

        private void SwitchComunication_Click(object sender, EventArgs e)
        {
            if (SwitchComunication.IsOn)  //Grps
            {
                SetGprsCtrlStatus();
            }
            else   //串口
            {
                SetSerialPortCtrlStatus();
            }
        }

        private void SetGprsCtrlStatus()
        {
            ButtonEnabled(true);
            GlobalValue.MSMQMgr.SendMessage(new MSMQEntity(ConstValue.MSMQTYPE.Get_SL651_WaitSendCmd, ""));  
            //GlobalValue.MSMQMgr.SendMessage(new MSMQEntity(ConstValue.MSMQTYPE.Get_SL651_WaitSendCmd, ""));      //获取待发送SL651命令
            GlobalValue.MSMQMgr.SendMessage(new MSMQEntity(ConstValue.MSMQTYPE.Set_SL651_AllowOnlineFlag, cbIsOnLine.Checked.ToString())); //设置SL651协议终端是否允许在线标志
            timer_GetWaitCmd.Enabled = true;  //启用查询GPRS待发送命令定时器 10s

            GlobalValue.MSMQMgr.MSMQEvent -= new MSMQHandler(MSMQMgr_MSMQEvent);
            GlobalValue.MSMQMgr.MSMQEvent += new MSMQHandler(MSMQMgr_MSMQEvent);

            group_waitcmd.Visible = true;
            group_manualSetParm.Visible = false;
            this.cbIsOnLine.CheckedChanged -= new System.EventHandler(this.cbIsOnLine_CheckedChanged);
            cbIsOnLine.Enabled = true;
            this.cbIsOnLine.CheckedChanged += new System.EventHandler(this.cbIsOnLine_CheckedChanged);
        }

        private void SetSerialPortCtrlStatus()
        {
            timer_GetWaitCmd.Enabled = false;  //停用查询GPRS待发送命令定时器 10s

            ButtonEnabled(GlobalValue.portUtil.IsOpen);
            group_waitcmd.Visible = false;
            group_manualSetParm.Visible = true;
            this.cbIsOnLine.CheckedChanged -= new System.EventHandler(this.cbIsOnLine_CheckedChanged);
            cbIsOnLine.Enabled = false;
            this.cbIsOnLine.CheckedChanged += new System.EventHandler(this.cbIsOnLine_CheckedChanged);

            GlobalValue.MSMQMgr.MSMQEvent -= new MSMQHandler(MSMQMgr_MSMQEvent);
        }

        private void cbParm1h5minWaterLevel_CheckedChanged(object sender, EventArgs e)
        {
            rgPrecipitation.SelectedIndex = -1;
        }

        



    }
}
