using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using Common;
using DevExpress.XtraEditors;

namespace SmartWaterSystem
{
    public partial class PreParmConfigForm : DevExpress.XtraEditors.XtraForm
    {
        public static string TerminalID;
        public PreParmConfigForm()
        {
            InitializeComponent();
        }

        private void PreParmConfigForm_Load(object sender, EventArgs e)
        {
            txtDeviceId.Text = TerminalID;
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL_Del = "UPDATE DL_ParamToDev SET SendedFlag=1 WHERE DeviceId='" + txtDeviceId.Text + "' AND DevTypeId=0";
                SQLHelper.ExecuteNonQuery(SQL_Del);
                //cbSendInterval.Text
                string SQL_Insert = @"INSERT INTO DL_ParamToDev(DeviceId,DevTypeId,CtrlCode,FunCode,DataValue,DataLenth,SetDate,SendedFlag) VALUES(
                                @DeviceId,@DevTypeId,@CtrlCode,@FunCode,@DataValue,@DataLenth,@SetDate,@SendedFlag)";
                SqlParameter[] parms = new SqlParameter[]{
                    new SqlParameter("DeviceId",SqlDbType.Int),
                    new SqlParameter("DevTypeId",SqlDbType.Int),  //数据类型:	 0 - 压力,  1 - 流量,  2 - 报警,  3 - 故障
                    new SqlParameter("CtrlCode",SqlDbType.Int),
                    new SqlParameter("FunCode",SqlDbType.Int),
                    new SqlParameter("DataValue",SqlDbType.VarChar,512),

                    new SqlParameter("DataLenth",SqlDbType.Int),
                    new SqlParameter("SetDate",SqlDbType.DateTime),
                    new SqlParameter("SendedFlag",SqlDbType.Int)
                };

                string value = "";
                int starttime = 0;  //起始时间从0开始
                int sendtimeinterval = Convert.ToInt32(cbSendInterval.Text); //发送时间间隔
                int Pressure1colinterval = Convert.ToInt32(cbCollectInterval.Text) ; //压力1采集时间间隔
                int Pressure2colinterval = 0; //压力2采集时间间隔 参数不使用，设为0

                value += starttime.ToString("X2");
                value += sendtimeinterval.ToString("X4");
                value += Pressure1colinterval.ToString("X4");
                value += Pressure2colinterval.ToString("X4");

                parms[0].Value = txtDeviceId.Text;
                parms[1].Value = 0;
                parms[2].Value = 0x80;
                parms[3].Value = 0x11;
                parms[4].Value = value;

                parms[5].Value = value.Length;
                parms[6].Value = DateTime.Now;
                parms[7].Value = 0;

                int result=SQLHelper.ExecuteNonQuery(SQL_Insert, parms);
                if (result > 0)
                {
                    //GlobalValue.Instance.SocketSQLMag.Send(SQLType.GetSendParm);
                    XtraMessageBox.Show("设置成功");
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }




        
    }
}
