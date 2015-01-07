using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Protocol;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

namespace NoiseAnalysisSystem
{
    public partial class NoiseDataMgr : BaseView, INoiseDataMgr
    {
        private bool isReading;
        private List<NoiseRecorder> selectList = new List<NoiseRecorder>();
        private int rowHandle = 0;

        string OriginalFilePath = Application.StartupPath + @"\OriginalDatas\";

        public NoiseDataMgr()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        public override void OnLoad()
        {
            try
            {
                if (!Directory.Exists(OriginalFilePath))
                {
                    Directory.CreateDirectory(OriginalFilePath);
                }
                SerialPortEvent(GlobalValue.portUtil.IsOpen);
            }
            catch { }
        }

        /// <summary>
        /// 绑定分组列表
        /// </summary>
        public void BindGroup()
        {
            DataTable dt = new DataTable("GroupTable");
            DataColumn col = dt.Columns.Add("选择");
            col.DataType = System.Type.GetType("System.Boolean");
            dt.Columns.Add("分组编号");
            dt.Columns.Add("分组名称");
            dt.Columns.Add("分组备注");
            dt.Columns.Add("记录仪编号");
            dt.Columns.Add("记录仪备注");
            dt.Columns.Add("读取进度");
            for (int i = 0; i < GlobalValue.groupList.Count; i++)
            {
                for (int j = 0; j < GlobalValue.groupList[i].RecorderList.Count; j++)
                {
                    double p = 0;
                    if (GlobalValue.groupList[i].RecorderList[j].Result == null)
                        p = 0;
                    else
                        p = 100;

                    dt.Rows.Add(new object[] { false, GlobalValue.groupList[i].ID, GlobalValue.groupList[i].Name, GlobalValue.groupList[i].Remark, GlobalValue.groupList[i].RecorderList[j].ID, GlobalValue.groupList[i].RecorderList[j].Remark, p });
                }
            }

            // 绑定数据
            MethodInvoker mi = (new MethodInvoker(() =>
            {
                gridControlGroup.DataSource = dt;
            }));

            this.Invoke(mi);

            // 添加分组
            DevExpress.XtraGrid.Columns.GridColumn column = gridViewGroupList.Columns["分组编号"];//获取要分组的列
            if (column == null) return;
            column.GroupIndex = 0;  //未分组情况下，列的GroupIndex为-1，所有都是一个组

            gridViewGroupList.ExpandAllGroups();
        }

        /// <summary>
        /// 绑定结果列表
        /// </summary>
        private void BindResult(NoiseRecorderGroup gp)
        {
            DataTable dt = new DataTable();
            ; dt.Columns.Add("记录仪编号");
            dt.Columns.Add("幅度");
            dt.Columns.Add("频率");
            dt.Columns.Add("读取时间");
            dt.Columns.Add("漏水状态");
            for (int i = 0; i < gp.RecorderList.Count; i++)
            {

                if (gp.RecorderList[i].Result != null)
                {
                    NoiseResult re = gp.RecorderList[i].Result;
                    string str = "不漏水";
                    if (re.IsLeak == 0)
                        str = "不漏水";
                    else if (re.IsLeak == 1)
                        str = "漏水";
                    dt.Rows.Add(new object[] { GlobalValue.recorderList[i].ID, re.LeakAmplitude.ToString(), re.LeakFrequency.ToString(), re.ReadTime.ToString("yyyy-MM-dd HH:mm:ss"), str });
                }

            }

            // 绑定数据
            MethodInvoker mi = (new MethodInvoker(() =>
            {
                gridControlResult.DataSource = dt;
            }));

            this.Invoke(mi);
        }

        /// <summary>
        /// 绑定结果列表
        /// </summary>
        private void BindResult(NoiseRecorder rec)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("记录仪编号");
            dt.Columns.Add("幅度");
            dt.Columns.Add("频率");
            dt.Columns.Add("读取时间");
            dt.Columns.Add("漏水状态");
            if (rec.Result != null)
            {
                NoiseResult re = rec.Result;
                string str = "不漏水";
                if (re.IsLeak == 0)
                    str = "不漏水";
                else if (re.IsLeak == 1)
                    str = "漏水";

                dt.Rows.Add(new object[] { rec.ID, re.LeakAmplitude.ToString(), re.LeakFrequency.ToString(), re.ReadTime.ToString("yyyy-MM-dd HH:mm:ss"), str });
            }

            // 绑定数据
            MethodInvoker mi = (new MethodInvoker(() =>
            {
                gridControlResult.DataSource = dt;
            }));

            this.Invoke(mi);
        }

        private void gridViewGroupList_CustomDrawGroupRow(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
            GridGroupRowInfo GridGroupRowInfo = e.Info as GridGroupRowInfo;

            GridView gridview = sender as GridView;
            int index = gridview.GetDataRowHandleByGroupRowHandle(e.RowHandle);

            GridGroupRowInfo.GroupText = "名称：" +
                gridview.GetRowCellValue(index, "分组名称").ToString() + ",备注：" + gridview.GetRowCellValue(index, "分组备注").ToString();
        }

        private void gridViewGroupList_RowClick(object sender, RowClickEventArgs e)
        {
            GridView gridview = sender as GridView;
            if (gridview.IsGroupRow(e.RowHandle))
            {
                int id = Convert.ToInt32(gridview.GetGroupRowValue(e.RowHandle));
                NoiseRecorderGroup gp = (from item in GlobalValue.groupList
                                         where item.ID == id
                                         select item).ToList()[0];
                BindResult(gp);
            }
            else
            {
                int id = Convert.ToInt32(gridview.GetRowCellValue(e.RowHandle, "记录仪编号"));
                NoiseRecorder rec = (from item in GlobalValue.recorderList
                                     where item.ID == id
                                     select item).ToList()[0];
                BindResult(rec);
            }

            //simpleButtonAny.Enabled = false;
        }

        int isLeak = 0;
        private void gridViewResultList_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.Caption == "记录仪编号")
            {
                int tempId = Convert.ToInt32(e.CellValue);
                NoiseResult temp = (from item in GlobalValue.recorderList
                                    where item.ID == tempId
                                    select item).ToList()[0].Result;
                isLeak = temp.IsLeak;
            }
            else if (e.Column.Caption == "幅度")
            {
                e.Appearance.BackColor = Color.DodgerBlue;
            }
            else if (e.Column.Caption == "频率")
            {
                e.Appearance.BackColor = Color.MediumSeaGreen;
            }
            else if (e.Column.Caption == "漏水状态")
            {
                if (isLeak == 0)
                {
                    e.DisplayText = "不漏水";
                    e.Appearance.BackColor = Color.SpringGreen;
                }
                else
                {
                    e.DisplayText = "漏水";
                    e.Appearance.BackColor = Color.Red;
                }
            }
        }

        private void simpleButtonSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridViewGroupList.RowCount; i++)
            {
                if (gridViewGroupList.GetRowCellValue(i, "选择") != null)
                {
                    gridViewGroupList.SetRowCellValue(i, "选择", true);
                    DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs arg = new DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs(i, gridViewGroupList.Columns["选择"], true);
                    gridViewGroupList_CellValueChanging(null, arg);
                }
            }
        }

        private void simpleButtonUnSelect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridViewGroupList.RowCount; i++)
            {
                if (gridViewGroupList.GetRowCellValue(i, "选择") != null)
                {
                    if ((bool)gridViewGroupList.GetRowCellValue(i, "选择"))
                    {
                        gridViewGroupList.SetRowCellValue(i, "选择", false);
                        DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs arg = new DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs(i, gridViewGroupList.Columns["选择"], true);
                        gridViewGroupList_CellValueChanging(null, arg);
                    }
                    else
                    {
                        gridViewGroupList.SetRowCellValue(i, "选择", true);
                        DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs arg = new DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs(i, gridViewGroupList.Columns["选择"], false);
                        gridViewGroupList_CellValueChanging(null, arg);
                    }
                }
            }
        }

        // 读取数据
        private void simpleButtonRead_Click(object sender, EventArgs e)
        {
            if (!GlobalValue.portUtil.IsOpen)
            {
                return;
            }

            List<int> readIdList = new List<int>(); // 需要读取的ID列表
            bool isError = false;

            // 如果存在需要重新读取的ID列表（即上次读取超时的ID列表）
            //if (GlobalValue.reReadIdList.Count != 0)
            //{
            //    for (int j = 0; j < GlobalValue.reReadIdList.Count; j++)
            //    {
            //        readIdList.Add(GlobalValue.reReadIdList[j]);
            //    }
            //}
            //else
            //{
            //xxxxx
            //}
            for (int j = 0; j < selectList.Count; j++)
            {
                if (!readIdList.Contains(selectList[j].ID))
                    readIdList.Add(selectList[j].ID);
            }
            if (selectList.Count != 0)
            {
                simpleButtonRead.Enabled = false;
                simpleButtonSelectAll.Enabled = false;
                simpleButtonUnSelect.Enabled = false;
                List<NoiseData> dataList = new List<NoiseData>();
                List<NoiseResult> resultList = new List<NoiseResult>();
                GlobalValue.log.ValueChanged -= new ReadDataChangedEventHandler(log_ValueChanged);
                GlobalValue.log.ValueChanged += new ReadDataChangedEventHandler(log_ValueChanged);

                NoiseDataHandler.FourierData.Clear();
                isReading = true;
                new Action(() =>
                {
                    try
                    {
                        foreach (var id in readIdList)
                        {
                            try
                            {
                                DisableRibbonBar();
                                DisableNavigateBar();
                                Thread.Sleep(1000);
                                this.Invoke(new MethodInvoker(() =>
                                    {
                                        for (int i = 0; i < gridViewGroupList.RowCount; i++)
                                        {
                                            if (gridViewGroupList.GetRowCellValue(i, "选择") != null)
                                            {
                                                if (gridViewGroupList.GetRowCellValue(i, "记录仪编号").ToString() == id.ToString())
                                                {
                                                    gridViewGroupList.SetRowCellValue(i, "读取进度", 0);
                                                    this.rowHandle = i;
                                                    break;
                                                }
                                            }
                                        }
                                    }));

                                Dictionary<short, short[]> result = new Dictionary<short, short[]>();
                                SetStaticItem(string.Format("正在读取记录仪{0}...", id));
                                ShowWaitForm("", string.Format("正在读取记录仪{0}...", id));
                                short[] arr = GlobalValue.log.Read((short)id);
                                result.Add((short)id, arr);
                                CallbackReaded(result, selectList);
                                GlobalValue.reReadIdList.Remove(id);

                                NoiseRecorder gpRec = (from item in GlobalValue.recorderList.AsEnumerable()
                                                       where item.ID == id
                                                       select item).ToList()[0];

                                dataList.Add(gpRec.Data);
                                resultList.Add(gpRec.Result);

                                BindResult(gpRec);
                            }
                            catch (TimeoutException)
                            {
                                if (!GlobalValue.reReadIdList.Contains(id))
                                    GlobalValue.reReadIdList.Add(id);
                                ShowDialog("记录仪" + id + "读取超时！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                isError = true;
                            }
                            catch (ArgumentNullException)
                            {
                                if (!GlobalValue.reReadIdList.Contains(id))
                                    GlobalValue.reReadIdList.Add(id);
                                ShowDialog("记录仪" + id + "数据为空！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                isError = true;
                            }

                        }
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                        isReading = false;
                        SetStaticItem("数据读取完成");
                        simpleButtonRead.Enabled = true;
                        simpleButtonSelectAll.Enabled = true;
                        simpleButtonUnSelect.Enabled = true;
                        HideWaitForm();
                        EnableRibbonBar();
                        EnableNavigateBar();

                        if (dataList.Count != 0)
                        {
                            DialogResult dr = XtraMessageBox.Show("已成功读取" + dataList.Count + "条数据，是否保存到数据库？", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr == System.Windows.Forms.DialogResult.Yes)
                            {
                                for (int i = 0; i < dataList.Count; i++)
                                {
                                    NoiseDataBaseHelper.AddNoiseData(dataList[i]);
                                    NoiseDataBaseHelper.AddNoiseResult(resultList[i]);
                                    GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
                                    GlobalValue.groupList = NoiseDataBaseHelper.GetGroups();
                                }
                            }
                        }
                    }
                }).BeginInvoke(null, null);
            }
            else
            {
                XtraMessageBox.Show("请勾选需要读取的记录仪！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 读取数据响应进度条
        private void log_ValueChanged(object sender, ValueEventArgs e)
        {
            System.Windows.Forms.MethodInvoker invoke = () =>
            {
                double v = (double)e.CurrentStep / (double)e.TotalStep;
                if (v < 0)
                    v = 0;
                if (v > 1)
                    v = 1;

                gridViewGroupList.SetRowCellValue(rowHandle, "读取进度", (v * 100.0).ToString("f1"));
            };

            if (this.InvokeRequired)
            {
                this.BeginInvoke(invoke);
            }
            else
            {
                invoke();
            }
        }

        /// <summary>
        /// 读取数据的回调函数
        /// </summary>
        private void CallbackReaded(Dictionary<short, short[]> result, List<NoiseRecorder> recList)
        {
            try
            {
                if (result == null)
                {
                    return;
                }

                int fla = 1;
                foreach (short key in result.Keys)
                {
                    NoiseRecorder recorder = (from item in recList.AsEnumerable()
                                              where item.ID == key
                                              select item).ToList()[0];

                    //int recNum = recorder.RecordNum;
                    int spanCount = 32; // 连续采集32个点
                    List<double[]> data = new List<double[]>();// 将采集数据分32个点为一组存放
                    List<double[]> data_isleak1 = new List<double[]>();// 将采集数据分32个点为一组存放,用于IsLeak1函数运算

                    for (int j = 0; j < result[key].Length / spanCount; j++)
                    {
                        double[] tmpData = new double[spanCount];
                        double[] tmpData_IsLeak = new double[spanCount];
                        for (int k = 0; k < spanCount; k++)
                        {
                            tmpData[k] = result[key][k + j * spanCount];
                        }

                        if (tmpData.Max() < 0)
                        {
                            ; //无效数据
                        }
                        else
                        {
                            //////////////////////////////////////////////////////////////////////////////////////////
                            // for test
                            GlobalValue.TestPath = string.Format(Application.StartupPath + @"\Data\记录仪{0}\", recorder.ID);
                            if (!Directory.Exists(GlobalValue.TestPath))
                                Directory.CreateDirectory(GlobalValue.TestPath);

                            StreamWriter sw = new StreamWriter(string.Format("{0}转换前数据{1}.txt", GlobalValue.TestPath, fla));
                            foreach (var item in tmpData)
                            {
                                sw.WriteLine(item);
                            }
                            sw.Flush();
                            sw.Close();

                            //////////////////////////////////////////////////////////////////////////////////////////

                            data.Add(tmpData);
                            tmpData.CopyTo(tmpData_IsLeak, 0);
                            data_isleak1.Add(tmpData_IsLeak);
                            fla++;
                        }
                    }
                    double[] amp = null;
                    double[] frq = null;
                    // 计算每一个频段下的最小幅度
                    NoiseDataHandler.InitData(data.Count);//result[key].Length / spanCount);
                    NoiseDataHandler.AmpCalc(data, ref amp, ref frq);

                    NoiseData da = new NoiseData();
                    da.RecorderID = recorder.ID;
                    da.GroupID = recorder.GroupID;
                    da.UploadTime = DateTime.Now;
                    da.ReadTime = DateTime.Now;
                    da.OriginalData = result[key];
                    da.Frequency = frq;
                    da.Amplitude = amp;
                    da.UploadFlag = result[key].Length / spanCount;
                    recorder.Data = da;

                    double[] ru = new double[2];

                    int isLeak1 = NoiseDataHandler.IsLeak1(recorder.GroupID, recorder.ID, data_isleak1);

                    int isLeak2 = NoiseDataHandler.IsLeak2(amp, recorder.LeakValue, ref ru);

                    NoiseResult re = new NoiseResult();
                    re.GroupID = recorder.GroupID;
                    re.IsLeak = (isLeak1 & isLeak2);
                    re.RecorderID = recorder.ID;
                    re.LeakAmplitude = ru[0];
                    re.LeakFrequency = ru[1];
                    re.UploadTime = DateTime.Now;
                    re.ReadTime = recorder.Data.ReadTime;
                    recorder.Result = re;


                    for (int i = 0; i < GlobalValue.recorderList.Count; i++)
                    {
                        if (GlobalValue.recorderList[i].ID == recorder.ID)
                            GlobalValue.recorderList[i] = recorder;
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("读取发生错误：" + ex.Message, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridViewGroupList_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.Caption == "选择")
            {
                int id = Convert.ToInt32(gridViewGroupList.GetRowCellValue(e.RowHandle, "记录仪编号"));
                NoiseRecorder rec = (from item in GlobalValue.recorderList
                                     where item.ID == id
                                     select item).ToList()[0];
                if ((bool)e.Value)
                {
                    selectList.Add(rec);
                }
                else
                {
                    selectList.Remove(rec);
                }
            }
        }

        // 重绘进度条
        private void gridViewGroupList_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.Caption == "读取进度")
            {
                decimal percent = Convert.ToDecimal(e.CellValue);
                int width = (int)(percent * e.Bounds.Width / 100);//涨跌幅最大为10%，所以要乘以10来计算比例，沾满一个单元格为10%  
                Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y, width, e.Bounds.Height);
                Brush b = Brushes.LightSteelBlue; //MediumTurquoise;
                e.Graphics.FillRectangle(b, rect);
            }
        }

        // 弹出右键菜单
        private void gridViewResultList_MouseUp(object sender, MouseEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi = this.gridViewResultList.CalcHitInfo(e.Location);
            if (hi.InRow && e.Button == MouseButtons.Right)
            {
                popupMenu.ShowPopup(Control.MousePosition);
            }
        }

        // 导出原始数据
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            List<NoiseRecorder> temp = new List<NoiseRecorder>();

            for (int i = 0; i < gridViewResultList.RowCount; i++)
            {
                object obj = gridViewResultList.GetRowCellValue(i, "记录仪编号");
                int recId = Convert.ToInt32(gridViewResultList.GetRowCellValue(i, "记录仪编号"));
                NoiseRecorder rec = (from item in GlobalValue.recorderList.AsEnumerable()
                                     where item.ID == recId
                                     select item).ToList()[0];

                if (rec.Data != null)
                {
                    if (rec.Data.OriginalData != null)
                        temp.Add(rec);
                }
            }

            if (temp.Count == 0)
            {
                XtraMessageBox.Show("请勾选需导出的记录仪/所勾选记录仪不存在原始数据，请重新读取！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "导出原始数据";
            fileDialog.Filter = "Excel文件|*.csv";
            fileDialog.FileName = "Data_" + DateTime.Now.ToString("yyMMddHHmm");
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = fileDialog.FileName;
                StreamWriter sw = new StreamWriter(fileName, false, Encoding.Default);

                foreach (var item in temp)
                {
                    if (item.Data.OriginalData != null)
                    {
                        sw.Write(item.ID + ",");
                        for (int i = 0; i < item.Data.OriginalData.Length; i++)
                        {
                            sw.Write(item.Data.OriginalData[i] + ",");
                        }
                        sw.WriteLine();
                    }
                }

                sw.Flush();
                sw.Close();
                sw.Dispose();

                XtraMessageBox.Show("导出成功！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // 导出分析结果
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "导出分析结果";
            fileDialog.Filter = "Excel文件|*.xls";
            fileDialog.FileName = "Result_" + DateTime.Now.ToString("yyMMddHHmm");

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                gridControlResult.ExportToXls(fileDialog.FileName);
                XtraMessageBox.Show("导出成功！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButtonAny_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(gridViewResultList.GetFocusedRowCellValue("记录仪编号"));
            if (id != 0)
            {
                NoiseRecorder rec = (from item in GlobalValue.recorderList
                                     where item.ID == id
                                     select item).ToList()[0];

                FrmDataAnalysis fda = new FrmDataAnalysis();
                fda.Recorder = rec;
                fda.ShowDialog();
            }
            else
            {
                XtraMessageBox.Show("请选择一组数据分析结果！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gridViewResultList_RowClick(object sender, RowClickEventArgs e)
        {
            GridView gridview = sender as GridView;
            if (!simpleButtonAny.Enabled)
                simpleButtonAny.Enabled = true;
        }

        private void gridViewResultList_DoubleClick(object sender, EventArgs e)
        {
            if (gridViewResultList.SelectedRowsCount == 0)
                return;

            int id = Convert.ToInt32(gridViewResultList.GetFocusedRowCellValue("记录仪编号"));
            if (id != 0)
            {
                NoiseRecorder rec = (from item in GlobalValue.recorderList
                                     where item.ID == id
                                     select item).ToList()[0];

                FrmDataAnalysis fda = new FrmDataAnalysis();
                fda.Recorder = rec;
                fda.ShowDialog();
            }
            else
            {
                XtraMessageBox.Show("请选择一组数据分析结果！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButtonComapre_Click(object sender, EventArgs e)
        {
            if (GlobalValue.recorderList.Count > 0)
            {
                MDIView.LoadView(typeof(NoiseDataCompare));
            }
            else
            {
                XtraMessageBox.Show("当前不存在任何记录仪！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnReadFromFold_Click(object sender, EventArgs e)
        {
            //if (!Regex.IsMatch(txtID.Text, @"^\d+$"))
            //{
            //    XtraMessageBox.Show("请输入一个记录仪ID！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    txtID.Focus();
            //    return;
            //}

            if (!CheckFileExist())
            {
                if (DialogResult.Yes == XtraMessageBox.Show("请将原始数据文件(txt)放入路径:" + OriginalFilePath + ",是否打开目录?", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                {
                    btnOpenFold_Click(null, null);
                }
                return;
            }


            List<int> readIdList = new List<int>(); // 需要读取的ID列表
            //readIdList.Add(Convert.ToInt32(txtID.Text));
            bool isError = false;

            for (int j = 0; j < selectList.Count; j++)
            {
                if (!readIdList.Contains(selectList[j].ID))
                    readIdList.Add(selectList[j].ID);
            }
            if (selectList.Count != 0)
            {

                btnReadFromFold.Enabled = false;
                List<NoiseData> dataList = new List<NoiseData>();
                List<NoiseResult> resultList = new List<NoiseResult>();
                GlobalValue.log.ValueChanged -= new ReadDataChangedEventHandler(log_ValueChanged);
                GlobalValue.log.ValueChanged += new ReadDataChangedEventHandler(log_ValueChanged);

                NoiseDataHandler.FourierData.Clear();
                isReading = true;
                new Action(() =>
                {
                    try
                    {
                        foreach (var id in readIdList)
                        {
                            try
                            {
                                DisableRibbonBar();
                                DisableNavigateBar();
                                Thread.Sleep(1000);
                                this.Invoke(new MethodInvoker(() =>
                                {
                                    for (int i = 0; i < gridViewGroupList.RowCount; i++)
                                    {
                                        if (gridViewGroupList.GetRowCellValue(i, "选择") != null)
                                        {
                                            if (gridViewGroupList.GetRowCellValue(i, "记录仪编号").ToString() == id.ToString())
                                            {
                                                gridViewGroupList.SetRowCellValue(i, "读取进度", 0);
                                                this.rowHandle = i;
                                                break;
                                            }
                                        }
                                    }
                                }));

                                Dictionary<short, short[]> result = new Dictionary<short, short[]>();
                                SetStaticItem(string.Format("正在读取记录仪{0}...", id));
                                ShowWaitForm("", string.Format("正在读取记录仪{0}...", id));
                                short[] arr = GetDataFromFiles();
                                if (arr == null || arr.Length == 0)
                                    throw new ArgumentNullException("数据获取失败");
                                result.Add((short)id, arr);
                                CallbackReaded(result, selectList);
                                GlobalValue.reReadIdList.Remove(id);

                                NoiseRecorder gpRec = (from item in GlobalValue.recorderList.AsEnumerable()
                                                       where item.ID == id
                                                       select item).ToList()[0];

                                dataList.Add(gpRec.Data);
                                resultList.Add(gpRec.Result);

                                BindResult(gpRec);
                            }
                            catch (TimeoutException)
                            {
                                ShowDialog("记录仪" + id + "读取超时！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                isError = true;
                            }
                            catch (ArgumentNullException)
                            {
                                ShowDialog("记录仪" + id + "数据为空！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                isError = true;
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                        isReading = false;
                        SetStaticItem("数据读取完成");
                        btnReadFromFold.Enabled = true;
                        HideWaitForm();
                        EnableRibbonBar();
                        EnableNavigateBar();

                        if (dataList.Count != 0)
                        {
                            DialogResult dr = XtraMessageBox.Show("已成功读取" + dataList.Count + "条数据，是否保存到数据库？", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr == System.Windows.Forms.DialogResult.Yes)
                            {
                                for (int i = 0; i < dataList.Count; i++)
                                {
                                    NoiseDataBaseHelper.AddNoiseData(dataList[i]);
                                    NoiseDataBaseHelper.AddNoiseResult(resultList[i]);
                                    GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
                                    GlobalValue.groupList = NoiseDataBaseHelper.GetGroups();
                                }
                            }
                        }
                    }
                }).BeginInvoke(null, null);
            }
            else
            {
                XtraMessageBox.Show("请勾选需要读取的记录仪！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool CheckFileExist()
        {
            string[] files = Directory.GetFiles(OriginalFilePath, "*.txt");
            if (files != null && files.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private short[] GetDataFromFiles()
        {
            try
            {
                string[] files = Directory.GetFiles(OriginalFilePath, "*.txt");
                if (files != null && files.Length > 0)
                {
                    List<short> lstData = new List<short>();
                    foreach (string file in files)
                    {
                        using (StreamReader reader = new StreamReader(file))
                        {
                            string strdata = reader.ReadToEnd();
                            if (!string.IsNullOrEmpty(strdata))
                            {
                                string[] str_datas = strdata.Split(new char[] { '\r', '\n' });
                                if (str_datas != null && str_datas.Length > 0)
                                {
                                    foreach (string data in str_datas)
                                    {
                                        if (!string.IsNullOrEmpty(data))
                                        {
                                            lstData.Add(Convert.ToInt16(data));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return lstData.ToArray();
                }
                return null;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("读取文件发生错误,请检查文件", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void btnOpenFold_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", OriginalFilePath);
            }
            catch
            {
                XtraMessageBox.Show("打开文件夹异常", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void SerialPortEvent(bool Enabled)
        {
            simpleButtonRead.Enabled = Enabled;
        }

    }
}
