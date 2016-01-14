using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid.Views.Grid;
using Entity;
using System.Collections;
using BLL;
using Common;
using System.Collections.Generic;

namespace SmartWaterSystem
{
    public partial class UniversalTerParm651 : BaseView,IUniversalTerParm651
    {
        public UniversalTerParm651()
        {
            InitializeComponent();
            GlobalValue.MSMQMgr.MSMQEvent += new MSMQHandler(MSMQMgr_MSMQEvent);
        }

        private void UniversalTerParm651_Load(object sender, EventArgs e)
        {
            InitControls();
        }

        private void InitControls()
        {
            GlobalValue.MSMQMgr.SendMessage(new MSMQEntity(ConstValue.MSMQTYPE.Get_SL651_AllowOnlineFlag, ""));  //获取SL651协议终端是否允许在线标志
            GlobalValue.MSMQMgr.SendMessage(new MSMQEntity(ConstValue.MSMQTYPE.Get_SL651_WaitSendCmd, ""));      //获取待发送SL651命令
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
                        gridControl_WaitCmd.DataSource = null;
                        gridControl_WaitCmd.RefreshDataSource();
                        if (lstPack != null)
                        {
                            DataTable dt = new DataTable("waitsendTable");
                            dt.Columns.Add("A5");
                            dt.Columns.Add("A4");
                            dt.Columns.Add("A3");
                            dt.Columns.Add("A2");
                            dt.Columns.Add("A1");
                            dt.Columns.Add("funcode");
                            foreach (Package651 pack in lstPack)
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
                            SetWaitListView_Cmd(dt);
                        }
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
                    gridControl_WaitCmd.RefreshDataSource();
                },lstPack);
            }
            else
            {
                gridControl_WaitCmd.DataSource = lstPack;
                gridControl_WaitCmd.RefreshDataSource();
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

        #region button events
        private void ButtonSend(Package651 pack)
        {
            if (SwitchComunication.IsOn)  //SerialPort
            {
                   
            }
            else   //GPRS
            {
                MSMQEntity msmqentity = new MSMQEntity();
                msmqentity.MsgType = ConstValue.MSMQTYPE.SL651_Cmd;
                msmqentity.Pack651 = pack;
                GlobalValue.MSMQMgr.SendMessage(msmqentity);
            }
        }
        private void btnQueryTime_Click(object sender, EventArgs e)
        {
            if (ValidateA5To1())
            {
                Package651 pack = new Package651();
                pack.A5 = Convert.ToByte(txtA5.Text, 16);
                pack.A4 = Convert.ToByte(txtA4.Text, 16);
                pack.A3 = Convert.ToByte(txtA3.Text, 16);
                pack.A2 = Convert.ToByte(txtA2.Text, 16);
                pack.A1 = Convert.ToByte(txtA1.Text, 16);
                pack.CenterAddr = Convert.ToByte(txtCenterAddr.Text);
                pack.dt = new byte[6];
                pack.dt[0] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Year - 2000));
                pack.dt[1] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Month));
                pack.dt[2] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Day));
                pack.dt[3] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Hour));
                pack.dt[4] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Minute));
                pack.dt[5] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Second));
                pack.PWD = new byte[2];
                pack.PWD[0] = Convert.ToByte(txtPwd0.Text, 16);
                pack.PWD[1] = Convert.ToByte(txtPwd1.Text, 16);
                pack.FUNCODE = (byte)SL651_COMMAND.QueryTime;

                byte[] lens = BitConverter.GetBytes((ushort)(8));
                pack.L0 = lens[0];
                pack.L1 = lens[1];
                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;
                pack.AddrFlag = PackageDefine.AddrFlag;

                ButtonSend(pack);
            }
        }

        private void btnQueryVersion_Click(object sender, EventArgs e)
        {
            if (ValidateA5To1())
            {
                Package651 pack = new Package651();
                pack.A5 = Convert.ToByte(txtA5.Text, 16);
                pack.A4 = Convert.ToByte(txtA4.Text, 16);
                pack.A3 = Convert.ToByte(txtA3.Text, 16);
                pack.A2 = Convert.ToByte(txtA2.Text, 16);
                pack.A1 = Convert.ToByte(txtA1.Text, 16);
                pack.CenterAddr = Convert.ToByte(txtCenterAddr.Text);
                pack.dt = new byte[6];
                pack.dt[0] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Year - 2000));
                pack.dt[1] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Month));
                pack.dt[2] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Day));
                pack.dt[3] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Hour));
                pack.dt[4] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Minute));
                pack.dt[5] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Second));
                pack.PWD = new byte[2];
                pack.PWD[0] = Convert.ToByte(txtPwd0.Text, 16);
                pack.PWD[1] = Convert.ToByte(txtPwd1.Text, 16);
                pack.FUNCODE = (byte)SL651_COMMAND.QueryVer;

                //if (ListviewRepeat(pack.A1, pack.A2, pack.A3, pack.A4, pack.A5, pack.FUNCODE))
                //{
                //    XtraMessageBox.Show("已存在待发送指令-查询版本号，请勿重复!");
                //    return;
                //}

                byte[] lens = BitConverter.GetBytes((ushort)(8));
                pack.L0 = lens[0];
                pack.L1 = lens[1];
                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;
                pack.AddrFlag = PackageDefine.AddrFlag;

                ButtonSend(pack);
            }
        }

        private void btnQueryCurData_Click(object sender, EventArgs e)
        {
            if (ValidateA5To1())
            {
                Package651 pack = new Package651();
                pack.A5 = Convert.ToByte(txtA5.Text, 16);
                pack.A4 = Convert.ToByte(txtA4.Text, 16);
                pack.A3 = Convert.ToByte(txtA3.Text, 16);
                pack.A2 = Convert.ToByte(txtA2.Text, 16);
                pack.A1 = Convert.ToByte(txtA1.Text, 16);
                pack.CenterAddr = Convert.ToByte(txtCenterAddr.Text);
                pack.dt = new byte[6];
                pack.dt[0] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Year - 2000));
                pack.dt[1] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Month));
                pack.dt[2] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Day));
                pack.dt[3] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Hour));
                pack.dt[4] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Minute));
                pack.dt[5] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Second));
                pack.PWD = new byte[2];
                pack.PWD[0] = Convert.ToByte(txtPwd0.Text, 16);
                pack.PWD[1] = Convert.ToByte(txtPwd1.Text, 16);
                pack.FUNCODE = (byte)SL651_COMMAND.QueryCurData;

                //if (ListviewRepeat(pack.A1, pack.A2, pack.A3, pack.A4, pack.A5, pack.FUNCODE))
                //{
                //    XtraMessageBox.Show("已存在待发送指令-查询实时数据，请勿重复!");
                //    return;
                //}

                byte[] lens = BitConverter.GetBytes((ushort)(8));
                pack.L0 = lens[0];
                pack.L1 = lens[1];
                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;
                pack.AddrFlag = PackageDefine.AddrFlag;

                ButtonSend(pack);
            }
        }

        private void btnQueryEvent_Click(object sender, EventArgs e)
        {
            if (ValidateA5To1())
            {
                Package651 pack = new Package651();
                pack.A5 = Convert.ToByte(txtA5.Text, 16);
                pack.A4 = Convert.ToByte(txtA4.Text, 16);
                pack.A3 = Convert.ToByte(txtA3.Text, 16);
                pack.A2 = Convert.ToByte(txtA2.Text, 16);
                pack.A1 = Convert.ToByte(txtA1.Text, 16);
                pack.CenterAddr = Convert.ToByte(txtCenterAddr.Text);
                pack.dt = new byte[6];
                pack.dt[0] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Year - 2000));
                pack.dt[1] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Month));
                pack.dt[2] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Day));
                pack.dt[3] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Hour));
                pack.dt[4] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Minute));
                pack.dt[5] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Second));
                pack.PWD = new byte[2];
                pack.PWD[0] = Convert.ToByte(txtPwd0.Text, 16);
                pack.PWD[1] = Convert.ToByte(txtPwd1.Text, 16);
                pack.FUNCODE = (byte)SL651_COMMAND.QueryEvent;

                //if (ListviewRepeat(pack.A1, pack.A2, pack.A3, pack.A4, pack.A5, pack.FUNCODE))
                //{
                //    XtraMessageBox.Show("已存在待发送指令-查询事件，请勿重复!");
                //    return;
                //}

                byte[] lens = BitConverter.GetBytes((ushort)(8));
                pack.L0 = lens[0];
                pack.L1 = lens[1];
                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;
                pack.AddrFlag = PackageDefine.AddrFlag;

                ButtonSend(pack);
            }
        }

        private void btnQueryAlarm_Click(object sender, EventArgs e)
        {
            if (ValidateA5To1())
            {
                Package651 pack = new Package651();
                pack.A5 = Convert.ToByte(txtA5.Text, 16);
                pack.A4 = Convert.ToByte(txtA4.Text, 16);
                pack.A3 = Convert.ToByte(txtA3.Text, 16);
                pack.A2 = Convert.ToByte(txtA2.Text, 16);
                pack.A1 = Convert.ToByte(txtA1.Text, 16);
                pack.CenterAddr = Convert.ToByte(txtCenterAddr.Text);
                pack.dt = new byte[6];
                pack.dt[0] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Year - 2000));
                pack.dt[1] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Month));
                pack.dt[2] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Day));
                pack.dt[3] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Hour));
                pack.dt[4] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Minute));
                pack.dt[5] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Second));
                pack.PWD = new byte[2];
                pack.PWD[0] = Convert.ToByte(txtPwd0.Text, 16);
                pack.PWD[1] = Convert.ToByte(txtPwd1.Text, 16);
                pack.FUNCODE = (byte)SL651_COMMAND.QueryAlarm;

                //if (ListviewRepeat(pack.A1, pack.A2, pack.A3, pack.A4, pack.A5, pack.FUNCODE))
                //{
                //    XtraMessageBox.Show("已存在待发送指令-查询状态和报警信息，请勿重复!");
                //    return;
                //}

                byte[] lens = BitConverter.GetBytes((ushort)(8));
                pack.L0 = lens[0];
                pack.L1 = lens[1];
                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;
                pack.AddrFlag = PackageDefine.AddrFlag;

                ButtonSend(pack);
            }
        }

        private void btnSetTime_Click(object sender, EventArgs e)
        {
            if (ValidateA5To1())
            {
                Package651 pack = new Package651();
                pack.A5 = Convert.ToByte(txtA5.Text, 16);
                pack.A4 = Convert.ToByte(txtA4.Text, 16);
                pack.A3 = Convert.ToByte(txtA3.Text, 16);
                pack.A2 = Convert.ToByte(txtA2.Text, 16);
                pack.A1 = Convert.ToByte(txtA1.Text, 16);
                pack.CenterAddr = Convert.ToByte(txtCenterAddr.Text);
                pack.dt = new byte[6];
                pack.dt[0] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Year - 2000));
                pack.dt[1] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Month));
                pack.dt[2] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Day));
                pack.dt[3] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Hour));
                pack.dt[4] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Minute));
                pack.dt[5] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Second));
                pack.PWD = new byte[2];
                pack.PWD[0] = Convert.ToByte(txtPwd0.Text, 16);
                pack.PWD[1] = Convert.ToByte(txtPwd1.Text, 16);
                pack.FUNCODE = (byte)SL651_COMMAND.SetTime;

                //if (ListviewRepeat(pack.A1, pack.A2, pack.A3, pack.A4, pack.A5, pack.FUNCODE))
                //{
                //    XtraMessageBox.Show("已存在待发送指令-设置遥测站时钟，请勿重复!");
                //    return;
                //}

                byte[] lens = BitConverter.GetBytes((ushort)(8));
                pack.L0 = lens[0];
                pack.L1 = lens[1];
                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;
                pack.AddrFlag = PackageDefine.AddrFlag;

                ButtonSend(pack);
            }
        }

        private void btnInitFlash_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == XtraMessageBox.Show("是否初始化固态存储数据?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                return;
            if (ValidateA5To1())
            {
                Package651 pack = new Package651();
                pack.A5 = Convert.ToByte(txtA5.Text, 16);
                pack.A4 = Convert.ToByte(txtA4.Text, 16);
                pack.A3 = Convert.ToByte(txtA3.Text, 16);
                pack.A2 = Convert.ToByte(txtA2.Text, 16);
                pack.A1 = Convert.ToByte(txtA1.Text, 16);
                pack.CenterAddr = Convert.ToByte(txtCenterAddr.Text);
                pack.dt = new byte[6];
                pack.dt[0] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Year - 2000));
                pack.dt[1] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Month));
                pack.dt[2] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Day));
                pack.dt[3] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Hour));
                pack.dt[4] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Minute));
                pack.dt[5] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Second));
                pack.PWD = new byte[2];
                pack.PWD[0] = Convert.ToByte(txtPwd0.Text, 16);
                pack.PWD[1] = Convert.ToByte(txtPwd1.Text, 16);
                pack.FUNCODE = (byte)SL651_COMMAND.InitFlash;

                //if (ListviewRepeat(pack.A1, pack.A2, pack.A3, pack.A4, pack.A5, pack.FUNCODE))
                //{
                //    XtraMessageBox.Show("已存在待发送指令-初始化固态存储数据，请勿重复!");
                //    return;
                //}

                byte[] lens = BitConverter.GetBytes((ushort)(10));
                pack.L0 = lens[0];
                pack.L1 = lens[1];
                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlag;
                pack.Data = new byte[2];
                pack.Data[0] = 0x97; //初始化固态标识符
                pack.Data[1] = 0x00;

                ButtonSend(pack);
            }
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == XtraMessageBox.Show("是否恢复出厂设置?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                return;
            if (ValidateA5To1())
            {
                Package651 pack = new Package651();
                pack.A5 = Convert.ToByte(txtA5.Text, 16);
                pack.A4 = Convert.ToByte(txtA4.Text, 16);
                pack.A3 = Convert.ToByte(txtA3.Text, 16);
                pack.A2 = Convert.ToByte(txtA2.Text, 16);
                pack.A1 = Convert.ToByte(txtA1.Text, 16);
                pack.CenterAddr = Convert.ToByte(txtCenterAddr.Text);
                pack.dt = new byte[6];
                pack.dt[0] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Year - 2000));
                pack.dt[1] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Month));
                pack.dt[2] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Day));
                pack.dt[3] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Hour));
                pack.dt[4] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Minute));
                pack.dt[5] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Second));
                pack.PWD = new byte[2];
                pack.PWD[0] = Convert.ToByte(txtPwd0.Text, 16);
                pack.PWD[1] = Convert.ToByte(txtPwd1.Text, 16);
                pack.FUNCODE = (byte)SL651_COMMAND.Init;

                //if (ListviewRepeat(pack.A1, pack.A2, pack.A3, pack.A4, pack.A5, pack.FUNCODE))
                //{
                //    XtraMessageBox.Show("已存在待发送指令-恢复出厂设置，请勿重复!");
                //    return;
                //}

                byte[] lens = BitConverter.GetBytes((ushort)(10));
                pack.L0 = lens[0];
                pack.L1 = lens[1];
                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlag;
                pack.Data = new byte[2];
                pack.Data[0] = 0x98; //恢复遥测站出厂设置标识符
                pack.Data[1] = 0x00;

                ButtonSend(pack);
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

                //if (string.IsNullOrEmpty(txtNewPwd1.Text) || !Regex.IsMatch(txtNewPwd1.Text, "^\\d+$"))
                //{
                //    XtraMessageBox.Show("请填写密码", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    txtNewPwd1.Focus();
                //    return ;
                //}
                Package651 pack = new Package651();
                pack.A5 = Convert.ToByte(txtA5.Text, 16);
                pack.A4 = Convert.ToByte(txtA4.Text, 16);
                pack.A3 = Convert.ToByte(txtA3.Text, 16);
                pack.A2 = Convert.ToByte(txtA2.Text, 16);
                pack.A1 = Convert.ToByte(txtA1.Text, 16);
                pack.CenterAddr = Convert.ToByte(txtCenterAddr.Text);
                pack.dt = new byte[6];
                pack.dt[0] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Year - 2000));
                pack.dt[1] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Month));
                pack.dt[2] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Day));
                pack.dt[3] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Hour));
                pack.dt[4] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Minute));
                pack.dt[5] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Second));
                pack.PWD = new byte[2];
                pack.PWD[0] = Convert.ToByte(txtPwd0.Text, 16);
                pack.PWD[1] = Convert.ToByte(txtPwd1.Text, 16);
                pack.FUNCODE = (byte)SL651_COMMAND.ChPwd;

                //if (ListviewRepeat(pack.A1, pack.A2, pack.A3, pack.A4, pack.A5, pack.FUNCODE))
                //{
                //    XtraMessageBox.Show("已存在待发送指令-修改密码，请勿重复!");
                //    return;
                //}

                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlag;
                pack.Data = new byte[8];
                pack.Data[0] = 0x03; //密码标识符 0x03,0x10
                pack.Data[1] = 0x10;
                pack.Data[2] = Convert.ToByte(txtPwd0.Text);  //旧密码
                pack.Data[3] = Convert.ToByte(txtPwd1.Text);
                pack.Data[4] = 0x03; //密码标识符 0x03,0x10
                pack.Data[5] = 0x10;
                //pack.Data[6] = Convert.ToByte(txtNewPwd.Text);  //新密码
                pack.Data[6] = Convert.ToByte(txtNewPwd0.Text, 16);
                pack.Data[7] = Convert.ToByte(txtNewPwd1.Text, 16);

                byte[] lens = BitConverter.GetBytes((ushort)(8 + pack.Data.Length));
                pack.L0 = lens[0];
                pack.L1 = lens[1];

                ButtonSend(pack);
            }
            #endregion
        }

        private void btnReadBasicConfig_Click(object sender, EventArgs e)
        {
            #region ReadBasicConfig
            if (ValidateA5To1())
            {
                if (!(cbTerAddr.Checked | cbCenterAddr.Checked | cbPwd.Checked | cbChannel.Checked | cbStandbyChannel.Checked | cbWorkType.Checked | cbElements.Checked | cbIdentifyNum.Checked))
                {
                    XtraMessageBox.Show("请选择至少一项读取!");
                    return;
                }

                Package651 pack = new Package651();
                pack.A5 = Convert.ToByte(txtA5.Text, 16);
                pack.A4 = Convert.ToByte(txtA4.Text, 16);
                pack.A3 = Convert.ToByte(txtA3.Text, 16);
                pack.A2 = Convert.ToByte(txtA2.Text, 16);
                pack.A1 = Convert.ToByte(txtA1.Text, 16);
                pack.CenterAddr = Convert.ToByte(txtCenterAddr.Text);
                pack.dt = new byte[6];
                pack.dt[0] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Year - 2000));
                pack.dt[1] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Month));
                pack.dt[2] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Day));
                pack.dt[3] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Hour));
                pack.dt[4] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Minute));
                pack.dt[5] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Second));
                pack.PWD = new byte[2];
                pack.PWD[0] = Convert.ToByte(txtPwd0.Text, 16);
                pack.PWD[1] = Convert.ToByte(txtPwd1.Text, 16);
                pack.FUNCODE = (byte)SL651_COMMAND.ReadBasiConfig;

                //if (ListviewRepeat(pack.A1, pack.A2, pack.A3, pack.A4, pack.A5, pack.FUNCODE))
                //{
                //    XtraMessageBox.Show("已存在待发送指令-读取基本配置表，请勿重复!");
                //    return;
                //}

                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlag;

                List<byte> lstCentent = new List<byte>();
                if (cbTerAddr.Checked)
                    lstCentent.AddRange(PackageDefine.AddrFlag);  //遥测站地址标识符
                if (cbCenterAddr.Checked)
                    lstCentent.AddRange(PackageDefine.CenterAddrFlag);   //中心站地址标识符
                if (cbPwd.Checked)
                    lstCentent.AddRange(PackageDefine.PwdFlag);   //密码

                byte[] channelflag=new byte[2];byte[] standbychannelflag=new byte[2];
                if (cbChannel.Checked || cbStandbyChannel.Checked)
                {
                    if (combChannelType.SelectedIndex == 0)
                    {
                        channelflag = PackageDefine.Channel1Flag;
                        standbychannelflag = PackageDefine.StandbyChannel1Flag;
                    }
                    else if (combChannelType.SelectedIndex == 1)
                    {
                        channelflag = PackageDefine.Channel2Flag;
                        standbychannelflag = PackageDefine.StandbyChannel2Flag;
                    }
                    else if (combChannelType.SelectedIndex == 2)
                    {
                        channelflag = PackageDefine.Channel3Flag;
                        standbychannelflag = PackageDefine.StandbyChannel3Flag;
                    }
                    else if (combChannelType.SelectedIndex == 3)
                    {
                        channelflag = PackageDefine.Channel4Flag;
                        standbychannelflag = PackageDefine.StandbyChannel4Flag;
                    }
                }
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

                ButtonSend(pack);
            }
            #endregion
        }

        private void btnSetBasicConfig_Click(object sender, EventArgs e)
        {
            #region SetBasicConfig
            if (ValidateA5To1())
            {
                if (!(cbTerAddr.Checked | cbCenterAddr.Checked | cbPwd.Checked | cbChannel.Checked | cbStandbyChannel.Checked | cbWorkType.Checked | cbElements.Checked | cbIdentifyNum.Checked))
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

                if (cbPwd.Checked)
                {
                    if (string.IsNullOrEmpty(txtCPwd0.Text) || !Regex.IsMatch(txtCPwd0.Text, "^[a-zA-Z0-9]{1,2}$"))
                    {
                        XtraMessageBox.Show("请填写密码", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtCPwd0.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtCPwd1.Text) || !Regex.IsMatch(txtCPwd1.Text, "^[a-zA-Z0-9]{1,2}$"))
                    {
                        XtraMessageBox.Show("请填写密码", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtCPwd1.Focus();
                        return;
                    }
                }

                if (cbChannel.Checked)
                {
                    if (combChannel.SelectedIndex < 0)
                    {
                        XtraMessageBox.Show("请选择信道类型", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        combChannel.Focus();
                        return;
                    }

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

                if (cbStandbyChannel.Checked)
                {
                    if (combStandbyChannel.SelectedIndex < 0)
                    {
                        XtraMessageBox.Show("请选择备用信道类型", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        combStandbyChannel.Focus();
                        return;
                    }
                    if (!Regex.IsMatch(combStandbyChannel.Text, @"^\d{11,12}$"))
                    {
                        XtraMessageBox.Show("请输入备用信道号码!");
                        txtStandbyChTelnum.Focus();
                        return;
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
                        XtraMessageBox.Show("请选择通信设备识别号", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        combIdentifyNum.Focus();
                        return;
                    }
                    if (!Regex.IsMatch(txtTelNum.Text, "^1\\d{10}$"))
                    {
                        XtraMessageBox.Show("请输入卡识别号!Regex:^1\\d{10}$");
                        txtTelNum.Focus();
                        return;
                    }
                }

                Package651 pack = new Package651();
                pack.A5 = Convert.ToByte(txtA5.Text, 16);
                pack.A4 = Convert.ToByte(txtA4.Text, 16);
                pack.A3 = Convert.ToByte(txtA3.Text, 16);
                pack.A2 = Convert.ToByte(txtA2.Text, 16);
                pack.A1 = Convert.ToByte(txtA1.Text, 16);
                pack.CenterAddr = Convert.ToByte(txtCenterAddr.Text);
                pack.dt = new byte[6];
                pack.dt[0] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Year - 2000));
                pack.dt[1] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Month));
                pack.dt[2] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Day));
                pack.dt[3] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Hour));
                pack.dt[4] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Minute));
                pack.dt[5] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Second));
                pack.PWD = new byte[2];
                pack.PWD[0] = Convert.ToByte(txtPwd0.Text, 16);
                pack.PWD[1] = Convert.ToByte(txtPwd1.Text, 16);
                pack.FUNCODE = (byte)SL651_COMMAND.SetBasicConfig;

                //if (ListviewRepeat(pack.A1, pack.A2, pack.A3, pack.A4, pack.A5, pack.FUNCODE))
                //{
                //    XtraMessageBox.Show("已存在待发送指令-设置基本配置表，请勿重复!");
                //    return;
                //}

                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlag;

                List<byte> lstCentent = new List<byte>();
                if (cbTerAddr.Checked)
                {
                    lstCentent.AddRange(PackageDefine.AddrFlag);  //遥测站地址标识符
                    lstCentent.AddRange(new byte[] { Convert.ToByte(txtCA5.Text,16), Convert.ToByte(txtCA4.Text,16), 
                        Convert.ToByte(txtCA3.Text,16), Convert.ToByte(txtCA2.Text,16), Convert.ToByte(txtCA1.Text,16) });
                }
                if (cbCenterAddr.Checked)
                {
                    lstCentent.AddRange(PackageDefine.CenterAddrFlag);   //中心站地址标识符
                    lstCentent.Add(ConvertHelper.HexToBCD(Convert.ToByte(txtCCenterAddr.Text)));
                }
                if (cbPwd.Checked)
                {
                    lstCentent.AddRange(PackageDefine.PwdFlag);   //密码
                    lstCentent.AddRange(new byte[] { Convert.ToByte(txtCPwd0.Text, 16), Convert.ToByte(txtCPwd1.Text, 16) });
                }
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
                    lstCentent.AddRange(ConvertHelper.StrToBCD(ip));

                    string port = txtCPort.Text.PadLeft(6, '0');
                    lstCentent.AddRange(ConvertHelper.StrToBCD(port));
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
                    lstCentent.AddRange(ConvertHelper.StrToBCD(txtStandbyChTelnum.Text.Trim()));
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
                    lstCentent.Add(Convert.ToByte(combIdentifyNum.SelectedIndex + 1));
                    //lstCentent.AddRange(PackageDefine.TelNumFlag);  //卡识别号/主备信道标识符
                    char[] telchars = txtTelNum.Text.ToCharArray();
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

                ButtonSend(pack);
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

                Package651 pack = new Package651();
                pack.A5 = Convert.ToByte(txtA5.Text, 16);
                pack.A4 = Convert.ToByte(txtA4.Text, 16);
                pack.A3 = Convert.ToByte(txtA3.Text, 16);
                pack.A2 = Convert.ToByte(txtA2.Text, 16);
                pack.A1 = Convert.ToByte(txtA1.Text, 16);
                pack.CenterAddr = Convert.ToByte(txtCenterAddr.Text);
                pack.dt = new byte[6];
                pack.dt[0] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Year - 2000));
                pack.dt[1] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Month));
                pack.dt[2] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Day));
                pack.dt[3] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Hour));
                pack.dt[4] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Minute));
                pack.dt[5] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Second));
                pack.PWD = new byte[2];
                pack.PWD[0] = Convert.ToByte(txtPwd0.Text, 16);
                pack.PWD[1] = Convert.ToByte(txtPwd1.Text, 16);
                pack.FUNCODE = (byte)SL651_COMMAND.ReadRunConfig;

                //if (ListviewRepeat(pack.A1, pack.A2, pack.A3, pack.A4, pack.A5, pack.FUNCODE))
                //{
                //    XtraMessageBox.Show("已存在待发送指令-读取运行配置表，请勿重复!");
                //    return;
                //}

                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlag;

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

                ButtonSend(pack);
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
                if (cbPrecipitationStartTime.Checked && !Regex.IsMatch(txtPrecipitationStartTime.Text, @"(^[1-9]|1[0-9]{1}|2[0-3]{1})$"))
                {
                    XtraMessageBox.Show("请输入正确降水量日起始时间(1-23h)", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                Package651 pack = new Package651();
                pack.A5 = Convert.ToByte(txtA5.Text, 16);
                pack.A4 = Convert.ToByte(txtA4.Text, 16);
                pack.A3 = Convert.ToByte(txtA3.Text, 16);
                pack.A2 = Convert.ToByte(txtA2.Text, 16);
                pack.A1 = Convert.ToByte(txtA1.Text, 16);
                pack.CenterAddr = Convert.ToByte(txtCenterAddr.Text);
                pack.dt = new byte[6];
                pack.dt[0] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Year - 2000));
                pack.dt[1] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Month));
                pack.dt[2] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Day));
                pack.dt[3] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Hour));
                pack.dt[4] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Minute));
                pack.dt[5] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Second));
                pack.PWD = new byte[2];
                pack.PWD[0] = Convert.ToByte(txtPwd0.Text, 16);
                pack.PWD[1] = Convert.ToByte(txtPwd1.Text, 16);
                pack.FUNCODE = (byte)SL651_COMMAND.SetRunConfig;

                //if (ListviewRepeat(pack.A1, pack.A2, pack.A3, pack.A4, pack.A5, pack.FUNCODE))
                //{
                //    XtraMessageBox.Show("已存在待发送指令-读取运行配置表，请勿重复!");
                //    return;
                //}

                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlag;

                List<byte> lstCentent = new List<byte>();
                if (cbPeriodInterval.Checked)
                {
                    lstCentent.AddRange(PackageDefine.PeriodIntervalFlag);  //定时报时间间隔
                    lstCentent.Add(ConvertHelper.HexToBCD(Convert.ToByte(combPeriodInterval.Text)));
                }
                if (cbAddInterval.Checked)
                {
                    lstCentent.AddRange(PackageDefine.AddIntervalFlag);   //加报时间间隔
                    lstCentent.Add(ConvertHelper.HexToBCD(Convert.ToByte(txtAddInterval.Text)));
                }
                if (cbPrecipitationStartTime.Checked)
                {
                    lstCentent.AddRange(PackageDefine.PrecipitationStartTimeFlag);   //降水量日起始时间
                    lstCentent.Add(ConvertHelper.HexToBCD(Convert.ToByte(txtPrecipitationStartTime.Text)));
                }
                if (cbSampling.Checked)
                {
                    lstCentent.AddRange(PackageDefine.SamplingFlag); //采样间隔
                    string str_sampling = txtSampling.Text.PadLeft(4, '0');
                    lstCentent.AddRange(ConvertHelper.StrToBCD(str_sampling));
                }
                if (cbWaterLevelInterval.Checked)
                {
                    lstCentent.AddRange(PackageDefine.WaterLevelIntervalFlag);//水位数据存储间隔
                    lstCentent.Add(ConvertHelper.HexToBCD(Convert.ToByte(txtWaterLevelInterval.Text)));
                }
                if (cbRainFallRPrecision.Checked)
                {
                    lstCentent.AddRange(PackageDefine.RainFallPrecisionFlag);  //雨量计分辨率
                    byte b_pre = 0x01;
                    switch (combRainFallRPrecision.Text.Trim())
                    {
                        case "0.1mm":
                            b_pre = 0x01;
                            break;
                        case "0.2mm":
                            b_pre = 0x02;
                            break;
                        case "0.5mm":
                            b_pre = 0x05;
                            break;
                        case "1mm":
                            b_pre = 0x10;
                            break;
                        default:
                            b_pre = 0x01;
                            break;
                    }
                    lstCentent.Add(b_pre);
                }
                if (cbWaterLevelGaugePrecision.Checked)
                {
                    lstCentent.AddRange(PackageDefine.WaterLevelGaugePrecisionFlag);//水位计分辨率
                    byte b_pre = 0x01;
                    switch (combWaterLevelGaugePrecision.Text.Trim())
                    {
                        case "0.1cm":
                            b_pre = 0x01;
                            break;
                        case "0.5cm":
                            b_pre = 0x05;
                            break;
                        case "1cm":
                            b_pre = 0x10;
                            break;
                        default:
                            b_pre = 0x01;
                            break;
                    }
                    lstCentent.Add(b_pre);
                }
                if (cbRainFallLimit.Checked)
                {
                    lstCentent.AddRange(PackageDefine.RainFallLimitFlag);//雨量加报阀值
                    lstCentent.Add(ConvertHelper.HexToBCD(Convert.ToByte(txtRainFallLimit.Text)));
                }
                if (cbWaterLevelBasic.Checked)
                {
                    lstCentent.AddRange(PackageDefine.WaterLevelBasicFlag);//水位基值
                    int basic = (int)(Convert.ToDouble(txtWaterLevelBasic.Text) * 1000);
                    if (basic < 0)
                    {
                        lstCentent.Add(0xFF);  //负数四个字节，正数三个字节,负数第一个字节是0xFF
                        basic *= -1;
                    }
                    lstCentent.AddRange(ConvertHelper.StrToBCD(basic.ToString().PadLeft(8, '0')));
                }
                if (cbWaterLevelAmendLimit.Checked)
                {
                    lstCentent.AddRange(PackageDefine.WaterLevelAmendLimitFlag);//水位修正基值 
                    int amendbasic = (int)(Convert.ToDouble(txtWaterLevelAmendLimit.Text) * 1000);
                    if (amendbasic < 0)
                    {
                        lstCentent.Add(0xFF);  //负数四个字节，正数三个字节,负数第一个字节是0xFF
                        amendbasic *= -1;
                    }
                    lstCentent.AddRange(ConvertHelper.StrToBCD(amendbasic.ToString().PadLeft(6, '0')));
                }
                if (cbAddtionWaterLevel.Checked)
                {
                    lstCentent.AddRange(PackageDefine.AddtionWaterLevelFlag);//加报水位  
                    int waterlevel = (int)(Convert.ToDouble(txtAddtionWaterLevel.Text) * 100);
                    lstCentent.AddRange(ConvertHelper.StrToBCD(waterlevel.ToString()));
                }
                if (cbAddtionWaterLevelUpLimit.Checked)
                {
                    lstCentent.AddRange(PackageDefine.AddtionWaterLevelUpLimitFlag); //加报水位以上加报阀值
                    int waterlevelLimit = (int)(Convert.ToDouble(txtAddtionWaterLevelUpLimit.Text) * 100);
                    byte[] bs = ConvertHelper.StrToBCD(waterlevelLimit.ToString());
                    if (bs.Length == 1)
                        lstCentent.Add(0x00);

                    lstCentent.AddRange(bs);
                }
                if (cbAddtionWaterLevelLowLimit.Checked)
                {
                    lstCentent.AddRange(PackageDefine.AddtionWaterLevelLowLimitFlag); //加报水位以下加报阀值
                    int waterlevelLimit = (int)(Convert.ToDouble(txtAddtionWaterLevelLowLimit.Text) * 100);
                    byte[] bs = ConvertHelper.StrToBCD(waterlevelLimit.ToString());
                    if (bs.Length == 1)
                        lstCentent.Add(0x00);

                    lstCentent.AddRange(bs);
                }

                pack.Data = lstCentent.ToArray();
                byte[] lens = BitConverter.GetBytes((ushort)(8 + pack.Data.Length));
                pack.L0 = lens[0];
                pack.L1 = lens[1];

                ButtonSend(pack);
            }
            #endregion
        }

        private void btnQueryElement_Click(object sender, EventArgs e)
        {
            #region QueryElement
            if (ValidateA5To1())
            {
                if (!(cbParmPrecipitation.Checked | cbParmRainFallAddup.Checked | cbParmInstantWaterLevel.Checked))
                {
                    XtraMessageBox.Show("请选择至少一项读取!");
                    return;
                }

                Package651 pack = new Package651();
                pack.A5 = Convert.ToByte(txtA5.Text, 16);
                pack.A4 = Convert.ToByte(txtA4.Text, 16);
                pack.A3 = Convert.ToByte(txtA3.Text, 16);
                pack.A2 = Convert.ToByte(txtA2.Text, 16);
                pack.A1 = Convert.ToByte(txtA1.Text, 16);
                pack.CenterAddr = Convert.ToByte(txtCenterAddr.Text);
                pack.dt = new byte[6];
                pack.dt[0] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Year - 2000));
                pack.dt[1] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Month));
                pack.dt[2] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Day));
                pack.dt[3] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Hour));
                pack.dt[4] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Minute));
                pack.dt[5] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Second));
                pack.PWD = new byte[2];
                pack.PWD[0] = Convert.ToByte(txtPwd0.Text, 16);
                pack.PWD[1] = Convert.ToByte(txtPwd1.Text, 16);
                pack.FUNCODE = (byte)SL651_COMMAND.QueryElements;

                //if (ListviewRepeat(pack.A1, pack.A2, pack.A3, pack.A4, pack.A5, pack.FUNCODE))
                //{
                //    XtraMessageBox.Show("已存在待发送指令-查询指定要素，请勿重复!");
                //    return;
                //}

                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlag;

                List<byte> lstCentent = new List<byte>();
                if (cbParmPrecipitation.Checked)
                    lstCentent.AddRange(PackageDefine.PrecipitationFlag);  //当前降水量
                if (cbParmDayPrecipitation.Checked)
                    lstCentent.AddRange(PackageDefine.DayPrecipitationFlag);  //日降水量
                if (cbParmRainFallAddup.Checked)
                    lstCentent.AddRange(PackageDefine.RainFallAddupFlag);   //累计降雨量
                if (cbParmInstantWaterLevel.Checked)
                    lstCentent.AddRange(PackageDefine.InstantWaterlevelFlag);   //瞬时水位
                if (cbParm1h5minPrecipitation.Checked)
                    lstCentent.AddRange(PackageDefine.Precipitation5MinFlag);  //1小时5分钟时段雨量
                if (cbParm1h5minWaterLevel.Checked)
                    lstCentent.AddRange(PackageDefine.Waterlevel5MinFlag);  //1小时5分钟间隔相对水位
                pack.Data = lstCentent.ToArray();

                byte[] lens = BitConverter.GetBytes((ushort)(8 + pack.Data.Length));
                pack.L0 = lens[0];
                pack.L1 = lens[1];

                ButtonSend(pack);
            }
            #endregion
        }


        private void btnSetPrecipitationConstantCtrl_Click(object sender, EventArgs e)
        {
            if (ValidateA5To1())
            {
                Package651 pack = new Package651();
                pack.A5 = Convert.ToByte(txtA5.Text, 16);
                pack.A4 = Convert.ToByte(txtA4.Text, 16);
                pack.A3 = Convert.ToByte(txtA3.Text, 16);
                pack.A2 = Convert.ToByte(txtA2.Text, 16);
                pack.A1 = Convert.ToByte(txtA1.Text, 16);
                pack.CenterAddr = Convert.ToByte(txtCenterAddr.Text);
                pack.dt = new byte[6];
                pack.dt[0] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Year - 2000));
                pack.dt[1] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Month));
                pack.dt[2] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Day));
                pack.dt[3] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Hour));
                pack.dt[4] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Minute));
                pack.dt[5] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Second));
                pack.PWD = new byte[2];
                pack.PWD[0] = Convert.ToByte(txtPwd0.Text, 16);
                pack.PWD[1] = Convert.ToByte(txtPwd1.Text, 16);
                pack.FUNCODE = (byte)SL651_COMMAND.PrecipitationConstantCtrl;

                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlag;

                if (combPrecipitationConstantCtrl.SelectedIndex == 0)
                    pack.Data = new byte[] { 0xFF };   //投入
                else
                    pack.Data = new byte[] { 0x00 };   //投出

                byte[] lens = BitConverter.GetBytes((ushort)(8 + pack.Data.Length));
                pack.L0 = lens[0];
                pack.L1 = lens[1];

                ButtonSend(pack);
            }
        }

        private void btnReadPrecipitationConstantCtrl_Click(object sender, EventArgs e)
        {
            if (ValidateA5To1())
            {
                Package651 pack = new Package651();
                pack.A5 = Convert.ToByte(txtA5.Text, 16);
                pack.A4 = Convert.ToByte(txtA4.Text, 16);
                pack.A3 = Convert.ToByte(txtA3.Text, 16);
                pack.A2 = Convert.ToByte(txtA2.Text, 16);
                pack.A1 = Convert.ToByte(txtA1.Text, 16);
                pack.CenterAddr = Convert.ToByte(txtCenterAddr.Text);
                pack.dt = new byte[6];
                pack.dt[0] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Year - 2000));
                pack.dt[1] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Month));
                pack.dt[2] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Day));
                pack.dt[3] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Hour));
                pack.dt[4] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Minute));
                pack.dt[5] = ConvertHelper.HexToBCD(Convert.ToByte(DateTime.Now.Second));
                pack.PWD = new byte[2];
                pack.PWD[0] = Convert.ToByte(txtPwd0.Text, 16);
                pack.PWD[1] = Convert.ToByte(txtPwd1.Text, 16);
                pack.FUNCODE = (byte)SL651_COMMAND.PrecipitationConstantCtrl;

                pack.CStart = PackageDefine.CStart;

                pack.SNum = new byte[2];
                pack.SNum[0] = 0;
                pack.SNum[1] = 0;
                pack.IsUpload = false;

                pack.AddrFlag = PackageDefine.AddrFlag;

                byte[] lens = BitConverter.GetBytes((ushort)(8 + pack.Data.Length));
                pack.L0 = lens[0];
                pack.L1 = lens[1];

                ButtonSend(pack);
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
        #endregion

        #region key event
        void txt_onebyte_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
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

                if (Regex.IsMatch(text, @"^(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])$")
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
            TextBox txtbox = (TextBox)sender;
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
            GlobalValue.MSMQMgr.SendMessage(new MSMQEntity(ConstValue.MSMQTYPE.Get_SL651_AllowOnlineFlag,cbIsOnLine.Checked.ToString()));
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
            group_waitcmd.Visible = true;
        }

        private void SetSerialPortCtrlStatus()
        {
            group_waitcmd.Visible = false;
        }


    }
}
