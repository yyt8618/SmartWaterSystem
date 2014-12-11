using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using Protocol;
using NoiseAnalysisSystem;
using System.Collections;
using System.Net;

namespace Noise
{
    public partial class Form1 : Form
    {
        private readonly SerialPortUtil portUtil = SerialPortUtil.GetInstance();

        short NoiseLogID = 0;
        short NoiseTourID = 0;

        public Form1()
        {
            InitializeComponent();
            portUtil.PackageReceived += new PackageReceivedEventHandler(portUtil_DataReceived);
            portUtil.AppendBufLog += new AppendBufLogEventHandler(portUtil_AppendBufLog);
        }

        void portUtil_AppendBufLog(AppendBufLogEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    portUtil_AppendBufLog(e);
                }));
            }
            else
            {
                this.textBox4.AppendText(e.StrLogBuf.ToString());
                this.textBox4.ScrollToCaret();
            }
        }

        void portUtil_DataReceived(PackageReceivedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    portUtil_DataReceived(e);
                }));
            }
            else
            {

                this.textBox2.Text = e.DataReceived;
            }
        }


        private void btnTest_Click(object sender, EventArgs e)
        {
            //System.IO.FileStream sm = new System.IO.FileStream();
            //sm.Read(null, 0, 0);
            ////sm.BeginRead(
            //sm.EndRead(sm.BeginRead(new byte[] { 2 }, 0, 3, null, null));

            ////this.pictureBox1.Load("");



        }




        private void btnCancel_Click(object sender, EventArgs e)
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                portUtil.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.btnOpen.Enabled = !portUtil.IsOpen;
            this.btnClose.Enabled = portUtil.IsOpen;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            portUtil.Close();
            this.btnOpen.Enabled = !portUtil.IsOpen;
            this.btnClose.Enabled = portUtil.IsOpen;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            byte[] arr = ConvertHelper.HexStringToByteArray(this.textBox1.Text);
            portUtil.SendCommand(arr);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NoiseTour aa = new NoiseTour();
            MessageBox.Show(aa.ReadNoiseTourID().ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NoiseLog ad = new NoiseLog();
            MessageBox.Show(ad.ReadNoiseLogID().ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NoiseTour log = new NoiseTour();
            Control cl = sender as Control;
            try
            {
                cl.Enabled = false;
                int[] arr = log.ReadWireless(log.ReadNoiseTourID());
                this.textBox15.Text = arr[0].ToString();
                this.textBox16.Text = arr[1].ToString();
                this.textBox17.Text = arr[2].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误:" + ex.Message);
            }
            finally
            {
                cl.Enabled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (NoiseLogID == 0)
            {
                MessageBox.Show("请先读取记录仪编号");
                return;
            }
            NoiseLog log = new NoiseLog();
            Control cl = sender as Control;
            try
            {
                cl.Enabled = false;
                int[] arr = log.ReadWireless(NoiseLogID);
                this.textBox9.Text = arr[0].ToString();
                this.textBox12.Text = arr[1].ToString();
                this.textBox10.Text = arr[2].ToString();
                this.textBox13.Text = arr[3].ToString();
                this.textBox11.Text = arr[4].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误:" + ex.Message);
            }
            finally
            {
                cl.Enabled = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            NoiseLog dd = new NoiseLog();
            byte[] arr = dd.ReadStartEndTime(dd.ReadNoiseLogID());
            string r = "ID:" + dd.ReadNoiseLogID() + "\r\n";
            foreach (var item in arr)
            {
                r = r + item + "\r\n";
            }
            MessageBox.Show(r);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            NoiseLog dd = new NoiseLog();
            int a = dd.ReadInterval(dd.ReadNoiseLogID());
            string r = "ID:" + dd.ReadNoiseLogID() + "\r\n";
            r = r + "采集时间间隔：" + a;
            MessageBox.Show(r);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            NoiseLog dd = new NoiseLog();
            bool a = dd.ReadRemote(dd.ReadNoiseLogID());
            string r = "ID:" + dd.ReadNoiseLogID() + "\r\n";
            r = r + "记录仪远传功能：" + (a ? "已经打开" : "已经关闭");
            MessageBox.Show(r);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            NoiseLog dd = new NoiseLog();
            byte a = dd.ReadRemoteSendTime(dd.ReadNoiseLogID());
            string r = "ID:" + dd.ReadNoiseLogID() + "\r\n";
            r = r + "记录仪远传记录发送时间：" + a;
            MessageBox.Show(r);
        }


        NoiseLog addsfs = new NoiseLog();
        private void button10_Click(object sender, EventArgs e)
        {
            if (NoiseLogID == 0)
            {
                MessageBox.Show("请先读取记录仪编号");
                return;
            }

            addsfs.Reading -= new DataCompletedEventHandler(addsfs_ReadData);
            addsfs.Reading += new DataCompletedEventHandler(addsfs_ReadData);
            addsfs.ValueChanged -= new ReadDataChangedEventHandler(addsfs_ValueChanged);
            addsfs.ValueChanged += new ReadDataChangedEventHandler(addsfs_ValueChanged);
            addsfs.Read(NoiseLogID);
        }

        void addsfs_ValueChanged(object sender, ValueEventArgs e)
        {
            System.Windows.Forms.MethodInvoker invoke = () => { label24.Text = string.Format("进度：{0}bit/{1}bit", e.TotalStep, e.CurrentStep); };
            if (this.InvokeRequired)
            {
                this.BeginInvoke(invoke);
            }
            invoke();
        }

        void addsfs_ReadData(ReadDataEventArgs e)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                if (e.Data == null)
                {
                    return;

                }
                foreach (var item in e.Data)
                {
                    this.textBox2.AppendText(item.ToString() + " ");
                    this.textBox2.ScrollToCaret();
                }
                this.textBox2.AppendText(string.Format("\r\n 共计：{0}字节 ", e.Data.Length));
                this.textBox2.ScrollToCaret();

            }));
        }

        void bll_DataReceived(PackageReceivedEventArgs e)
        {

            this.Invoke(new MethodInvoker(() =>
            {
                if (e.Package == null)
                {
                    return;

                }
                this.textBox2.Text += e.Package.ToString() + "\r\n";
            }));

        }

        private void button11_Click(object sender, EventArgs e)
        {
            byte[] arr = ConvertHelper.HexStringToByteArray(this.textBox3.Text);
            portUtil.PackageReceived -= new PackageReceivedEventHandler(bll_DataReceived);
            portUtil.PackageReceived += new PackageReceivedEventHandler(bll_DataReceived);
            portUtil.SendData(arr);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.textBox2.Clear();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.textBox4.Clear();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox8.Text))
            {
                MessageBox.Show("请先读取记录仪编号");
                return;
            }

            NoiseLog log = new NoiseLog();
            Control cl = sender as Control;
            try
            {
                cl.Enabled = false;
                short id = Convert.ToInt16(this.textBox8.Text);
                byte[] tt = log.ReadTime(id);
                this.dateTimePicker1.Value = new DateTime(2013, 9, 4, tt[0], tt[1], tt[2]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误:" + ex.Message);
            }
            finally
            {
                cl.Enabled = true;
            }

        }


        private void button15_Click(object sender, EventArgs e)
        {
            if (NoiseLogID == 0)
            {
                MessageBox.Show("请先读取记录仪编号");
                return;
            }
            NoiseLog log = new NoiseLog();
            Control cl = sender as Control;
            try
            {

                cl.Enabled = false;
                if (log.WriteTime(NoiseLogID, this.dateTimePicker1.Value))
                {
                    MessageBox.Show("设置成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误:" + ex.Message);
            }
            finally
            {
                cl.Enabled = true;
            }

        }
        private void button17_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox8.Text))
            {
                MessageBox.Show("请先读取记录仪编号");
                return;
            }

            NoiseLog log = new NoiseLog();
            Control cl = sender as Control;
            try
            {

                cl.Enabled = false;
                byte[] tt = log.ReadStartEndTime(NoiseLogID);
                this.numericUpDown2.Value = tt[0];
                this.numericUpDown3.Value = tt[1];
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误:" + ex.Message);
            }
            finally
            {
                cl.Enabled = true;
            }

        }
        private void button16_Click(object sender, EventArgs e)
        {
            if (NoiseLogID == 0)
            {
                MessageBox.Show("请先读取记录仪编号");
                return;
            }
            NoiseLog log = new NoiseLog();
            Control cl = sender as Control;
            try
            {

                cl.Enabled = false;
                short id = log.ReadNoiseLogID();
                if (log.WriteStartEndTime(NoiseLogID, (int)this.numericUpDown2.Value, (int)this.numericUpDown3.Value))
                {
                    MessageBox.Show("设置成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误:" + ex.Message);
            }
            finally
            {
                cl.Enabled = true;
            }

        }
        private void button19_Click(object sender, EventArgs e)
        {
            if (NoiseLogID == 0)
            {
                MessageBox.Show("请先读取记录仪编号");
                return;
            }
            NoiseLog log = new NoiseLog();
            Control cl = sender as Control;
            try
            {
                cl.Enabled = false;
                this.numericUpDown1.Value = log.ReadInterval(NoiseLogID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误:" + ex.Message);
            }
            finally
            {
                cl.Enabled = true;
            }

        }


        private void button18_Click(object sender, EventArgs e)
        {
            if (NoiseLogID == 0)
            {
                MessageBox.Show("请先读取记录仪编号");
                return;
            }
            NoiseLog log = new NoiseLog();
            Control cl = sender as Control;
            try
            {
                cl.Enabled = false;
                if (log.WriteInterval(NoiseLogID, (int)this.numericUpDown1.Value))
                {
                    MessageBox.Show("设置成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误:" + ex.Message);
            }
            finally
            {
                cl.Enabled = true;
            }

        }




        Queue<Action> actions = new Queue<Action>();
        private void Operater(object sender, OperaterEventArgs e)
        {
            Control cl = sender as Control;
            cl.Enabled = false;
            new Action(() =>
            {
                if (e.newAction != null)
                {
                    actions.Enqueue(e.newAction);
                }
                while (actions.Count > 0)
                {
                    try
                    {
                        actions.Dequeue()();
                    }
                    catch (Exception)
                    {
                    }

                }
                cl.BeginInvoke(new MethodInvoker(() =>
                {
                    if (e.finished != null)
                    {
                        e.finished();
                    }
                    cl.Enabled = true;
                }));
            }).BeginInvoke(null, null);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (NoiseLogID == 0)
            {
                MessageBox.Show("请先读取记录仪编号");
                return;
            }
            NoiseLog log = new NoiseLog();
            Control cl = sender as Control;
            if (this.button20.Text == "启动")
            {
                try
                {
                    cl.Enabled = false;
                    if (log.CtrlStartOrStop(NoiseLogID, true))
                    {
                        MessageBox.Show("设置成功！");
                        label6.Text = "运行中";
                        this.button20.Text = "停止";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("发生错误:" + ex.Message);
                }
                finally
                {
                    cl.Enabled = true;
                }
            }
            else
            {
                try
                {
                    cl.Enabled = false;
                    short id = log.ReadNoiseLogID();
                    if (log.CtrlStartOrStop(id, false))
                    {
                        MessageBox.Show("设置成功！");
                        label6.Text = "已停止";
                        this.button20.Text = "启动";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("发生错误:" + ex.Message);
                }
                finally
                {
                    cl.Enabled = true;
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (NoiseLogID == 0)
            {
                MessageBox.Show("请先读取记录仪编号");
                return;
            }
            NoiseLog log = new NoiseLog();
            Control cl = sender as Control;
            try
            {

                cl.Enabled = false;
                
                int id = log.ReadNoiseLogID();

                //this.numericUpDown1.Value = log.read(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误:" + ex.Message);
            }
            finally
            {
                cl.Enabled = true;
            }

        }


        private void button21_Click(object sender, EventArgs e)
        {
            NoiseCtrl ctrl = new NoiseCtrl();
            Control cl = sender as Control;
            try
            {

                cl.Enabled = false;
                int id = ctrl.ReadNoiseCtrlID();

                this.textBox5.Text = id.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误:" + ex.Message);
            }
            finally
            {
                cl.Enabled = true;
            }

        }

        private void button22_Click(object sender, EventArgs e)
        {
            //NoiseCtrl ctrl = new NoiseCtrl();
            //Control cl = sender as Control;
            //try
            //{

            //    cl.Enabled = false;
            //    int id = ctrl.ReadNoiseCtrlID();

            //    this.textBox5.Text = ctrl.read
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("发生错误:" + ex.Message);
            //}
            //finally
            //{
            //    cl.Enabled = true;
            //}

        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (NoiseTourID==0)
            {
                MessageBox.Show("请先获取远传控制器ID");
                return;
            }
            NoiseCtrl ctrl = new NoiseCtrl();
            Control cl = sender as Control;
            try
            {

                cl.Enabled = false;
                this.textBox6.Text = ctrl.ReadPort(NoiseTourID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误:" + ex.Message);
            }
            finally
            {
                cl.Enabled = true;
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            if (NoiseTourID == 0)
            {
                MessageBox.Show("请先获取远传控制器ID");
                return;
            }
            NoiseCtrl ctrl = new NoiseCtrl();
            Control cl = sender as Control;
            try
            {

                cl.Enabled = false;
                this.textBox7.Text = ctrl.ReadIP(NoiseTourID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误:" + ex.Message);
            }
            finally
            {
                cl.Enabled = true;
            }
        }


        private void button4_Click_1(object sender, EventArgs e)
        {
            NoiseLog ctrl = new NoiseLog();
            Control cl = sender as Control;
            try
            {

                cl.Enabled = false;
                NoiseLogID = ctrl.ReadNoiseLogID();

                this.textBox8.Text = NoiseLogID.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误:" + ex.Message);
            }
            finally
            {
                cl.Enabled = true;
            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (NoiseLogID == 0)
            {
                MessageBox.Show("请先读取记录仪编号");
                return;
            }
            NoiseLog ctrl = new NoiseLog();
            Control cl = sender as Control;
            try
            {
                cl.Enabled = false;

                if (this.button2.Text == "打开")
                {
                    if (ctrl.WriteRemoteSwitch(NoiseLogID, true))
                    {
                        this.button2.Text = "关闭";
                        label13.Text = "已打开";
                    }
                }
                else
                {
                    if (ctrl.WriteRemoteSwitch(NoiseLogID, false))
                    {
                        this.button2.Text = "打开";
                        label13.Text = "已关闭";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误:" + ex.Message);
            }
            finally
            {
                cl.Enabled = true;
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if (NoiseLogID == 0)
            {
                MessageBox.Show("请先读取记录仪编号");
                return;
            }
            NoiseLog ctrl = new NoiseLog();
            Control cl = sender as Control;
            try
            {

                cl.Enabled = false;
                if (ctrl.ReadRemote(NoiseLogID))
                {
                    this.label13.Text = "已打开";
                    this.button2.Text = "关闭";
                }
                else
                {
                    this.label13.Text = "已关闭";
                    this.button2.Text = "打开";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误:" + ex.Message);
            }
            finally
            {
                cl.Enabled = true;
            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            if (NoiseLogID == 0)
            {
                MessageBox.Show("请先读取记录仪编号");
                return;
            }
            NoiseLog ctrl = new NoiseLog();
            Control cl = sender as Control;
            try
            {

                cl.Enabled = false;
                this.numericUpDown5.Value = ctrl.ReadRemoteSendTime(NoiseLogID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误:" + ex.Message);
            }
            finally
            {
                cl.Enabled = true;
            }

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            if (NoiseLogID == 0)
            {
                MessageBox.Show("请先读取记录仪编号");
                return;
            }
            NoiseLog ctrl = new NoiseLog();
            Control cl = sender as Control;
            try
            {
                cl.Enabled = false;
                short id = ctrl.ReadNoiseLogID();
                if (ctrl.WriteRemoteSendTime(NoiseLogID, (int)this.numericUpDown5.Value))
                {
                    MessageBox.Show("设置成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误:" + ex.Message);
            }
            finally
            {
                cl.Enabled = true;
            }

        }

        private void button28_Click(object sender, EventArgs e)
        {
            NoiseTour ctrl = new NoiseTour();
            Control cl = sender as Control;
            try
            {
                cl.Enabled = false;
                short id = ctrl.ReadNoiseTourID();
                this.textBox14.Text = id.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误:" + ex.Message);
            }
            finally
            {
                cl.Enabled = true;
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox8.Text))
            {
                MessageBox.Show("请先读取记录仪编号");
                return;
            }


            NoiseLog log = new NoiseLog();
            log.Reading -= new DataCompletedEventHandler(addsfs_ReadData);
            log.Reading += new DataCompletedEventHandler(addsfs_ReadData);

            try
            {

                new Action(() =>
                {

                    short[] arr = log.Read(NoiseLogID);

                    this.Invoke(new System.Windows.Forms.MethodInvoker(() =>
                    {
                        foreach (var item in arr)
                        {
                            textBox2.Text += " " + item;
                        }

                    }));

                }).BeginInvoke(null, null);

            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message);
            }

        }

        private void CallbackReaded(IAsyncResult result)
        {

            AsyncLoadDataEventHandler dn = (AsyncLoadDataEventHandler)result.AsyncState;

            short[] arr = dn.EndInvoke(result);

            return;





            //byte[] arr = result.AsyncState as byte[];
            this.Invoke(new MethodInvoker(() =>
            {
                foreach (var item in arr)
                {
                    this.textBox2.AppendText(item.ToString() + " ");
                    this.textBox2.ScrollToCaret();
                }
                this.textBox2.AppendText(string.Format("\r\n 共计：{0}条 ", arr.Length));
                this.textBox2.ScrollToCaret();

            }));
        }

        private void button30_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
        }

        private void button31_Click(object sender, EventArgs e)
        {


            new Action(() =>
            {
                //IAsyncResult result = log.BeginRead(id, null);

                //byte[] arr = log.EndRead(result);



                short[] arr = new short[] { 2, 4, 6, 8 };
                NoiseLog log = new NoiseLog();
                foreach (var item in arr)
                {
                    log.Read(item);
                    System.Threading.Thread.Sleep(3000);
                }

            }).BeginInvoke(null, null);



        }

    }

    public class OperaterEventArgs : EventArgs
    {
        public Action newAction;
        public Action finished;
        public OperaterEventArgs(Action operater, Action finished)
        {
            this.newAction = operater;
            this.finished = finished;
        }

    }
}
