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
using Entity;
using System.Collections;
using Common;
using BLL;

namespace SmartWaterSystem
{
    public partial class NoiseDataMgr : BaseView, INoiseDataMgr
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("NoiseDataMgr");

        private bool isReading;
        private List<NoiseRecorder> selectList = new List<NoiseRecorder>();
        private int rowHandle = 0;
        Queue ReadIdList = new Queue();
        List<NoiseData> dataList = new List<NoiseData>();
        List<NoiseResult> resultList = new List<NoiseResult>();

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
                ClearView();
            }
            catch { }
        }

        private void ClearView()
        {
            selectList = new List<NoiseRecorder>();
            gridControlGroup.DataSource = null;
            gridControlResult.DataSource = null;
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
                List<NoiseRecorder> lstTmpRec = new List<NoiseRecorder>();
                lstTmpRec.AddRange(GlobalValue.groupList[i].RecorderList.OrderBy(a => a.ID));  //记录仪按ID排序
                for (int j = 0; j < lstTmpRec.Count; j++)
                {
                    double p = 0; 
                    if (lstTmpRec[j].Result == null)
                        p = 0;
                    else
                        p = 100;

                    dt.Rows.Add(new object[] { false, GlobalValue.groupList[i].ID, GlobalValue.groupList[i].Name, GlobalValue.groupList[i].Remark, lstTmpRec[j].ID, lstTmpRec[j].Remark, p });
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
            gridControlResult.DataSource = null;
            DataTable dt = new DataTable();
            ; dt.Columns.Add("记录仪编号");
            dt.Columns.Add("幅度");
            dt.Columns.Add("频率");
            dt.Columns.Add("读取时间");
            dt.Columns.Add("漏水状态");
            dt.Columns.Add("漏水概率");
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
                    else if (re.IsLeak == -1)
                        str = "漏水";
                    dt.Rows.Add(new object[] { gp.RecorderList[i].ID, re.LeakAmplitude.ToString(), re.LeakFrequency.ToString(), re.ReadTime.ToString("yyyy-MM-dd HH:mm:ss"), str,(re.LeakProbability*100).ToString("f1")+"%" });
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
            dt.Columns.Add("漏水概率");

            List<NoiseResult> lstResult= NoiseDataBaseHelper.GetRecordHistoryResult(rec.ID);
            if (lstResult != null)
            {
                foreach(NoiseResult re in lstResult)
                {
                    string str = "不漏水";
                    if (re.IsLeak == 0)
                        str = "不漏水";
                    else if (re.IsLeak == 1)
                        str = "漏水";
                    else if (re.IsLeak == -1)
                        str = "漏水";
                    dt.Rows.Add(new object[] { rec.ID, re.LeakAmplitude.ToString(), re.LeakFrequency.ToString(), re.ReadTime.ToString("yyyy-MM-dd HH:mm:ss"), str, (re.LeakProbability * 100).ToString("f1") + "%" });
                }
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

        //int isLeak = 0;
        private void gridViewResultList_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.Caption == "记录仪编号")
            {
                int tempId = Convert.ToInt32(e.CellValue);
                //NoiseResult temp = (from item in GlobalValue.recorderList
                //                    where item.ID == tempId
                //                    select item).ToList()[0].Result;
                //if (temp != null)
                //    isLeak = temp.IsLeak;
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
                if (e.CellValue.ToString().Trim() == "不漏水")
                {
                    //e.DisplayText = "不漏水";
                    e.Appearance.BackColor = Color.SpringGreen;
                }
                else if(e.CellValue.ToString().Trim() == "漏水")
                {
                    //e.DisplayText = "漏水";
                    e.Appearance.BackColor = Color.Red;
                }
                else
                {
                    //e.DisplayText = "漏水";
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
                        DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs arg = new DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs(i, gridViewGroupList.Columns["选择"], false);
                        gridViewGroupList_CellValueChanging(null, arg);
                    }
                    else
                    {
                        gridViewGroupList.SetRowCellValue(i, "选择", true);
                        DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs arg = new DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs(i, gridViewGroupList.Columns["选择"], true);
                        gridViewGroupList_CellValueChanging(null, arg);
                    }
                }
            }
        }

        // 读取数据
        private void simpleButtonRead_Click(object sender, EventArgs e)
        {
            try
            {
                if (!GlobalValue.portUtil.IsOpen)
                {
                    return;
                }

                List<int> readIdtmp = new List<int>(); // 需要读取的ID列表
                                                       //bool isError = false;

                for (int j = 0; j < selectList.Count; j++)
                {
                    if (!readIdtmp.Contains(selectList[j].ID))
                        readIdtmp.Add(selectList[j].ID);
                }
                if (selectList.Count != 0)
                {
                    dataList = new List<NoiseData>();
                    resultList = new List<NoiseResult>();

                    List<NoiseRecorder> lstSelecttmp = new List<NoiseRecorder>();
                    lstSelecttmp.AddRange(selectList.OrderBy(a => a.ID));
                    selectList = lstSelecttmp;
                    foreach (var id in readIdtmp)
                    {
                        ReadIdList.Enqueue(id);
                    }

                    simpleButtonRead.Enabled = false;
                    simpleButtonSelectAll.Enabled = false;
                    simpleButtonUnSelect.Enabled = false;

                    GlobalValue.Noiselog.ValueChanged -= new ReadDataChangedEventHandler(log_ValueChanged);
                    GlobalValue.Noiselog.ValueChanged += new ReadDataChangedEventHandler(log_ValueChanged);

                    NoiseDataHandler.FourierData.Clear();
                    isReading = true;
                    //new Action(() =>
                    //{
                    try
                    {
                        ReadData(Convert.ToInt16(ReadIdList.Dequeue()));
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                        //isReading = false;
                        //SetStaticItem("数据读取完成");
                        //simpleButtonRead.Enabled = true;
                        //simpleButtonSelectAll.Enabled = true;
                        //simpleButtonUnSelect.Enabled = true;
                        //HideWaitForm();
                        //EnableRibbonBar();
                        //EnableNavigateBar();

                        //if (dataList.Count != 0)
                        //{
                        //    DialogResult dr = XtraMessageBox.Show("已成功读取" + dataList.Count + "条数据，是否保存到数据库？", GlobalValue.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        //    if (dr == System.Windows.Forms.DialogResult.Yes)
                        //    {
                        //        for (int i = 0; i < dataList.Count; i++)
                        //        {
                        //            NoiseDataBaseHelper.AddNoiseData(dataList[i]);
                        //            NoiseDataBaseHelper.AddNoiseResult(resultList[i]);
                        //            GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
                        //            GlobalValue.groupList = NoiseDataBaseHelper.GetGroups();
                        //        }
                        //    }
                        //}
                    }
                    //}).BeginInvoke(null, null);
                }
                else
                {
                    XtraMessageBox.Show("请勾选需要读取的记录仪！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorException("simpleButtonRead_Click", ex);
                logger.Info(ex.StackTrace);
            }
        }

        private void ReadData(short id)
        {
            try
            {
                DisableRibbonBar();
                DisableNavigateBar();
                //Thread.Sleep(1000);
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


                SetStaticItem(string.Format("正在读取记录仪{0}...", id));
                ShowWaitForm("", string.Format("正在读取记录仪{0}...", id));
                if (GlobalValue.NoiseSerialPortOptData == null)
                    GlobalValue.NoiseSerialPortOptData = new NoiseSerialPortOptEntity();
                GlobalValue.NoiseSerialPortOptData.ID = id;
                BeginSerialPortDelegate();
                GlobalValue.SerialPortMgr.Send(SerialPortType.NoiseReadData);
                //Dictionary<short, short[]> result = new Dictionary<short, short[]>();
                //short[] arr = GlobalValue.Noiselog.Read((short)id);
                //result.Add((short)id, arr);
                //CallbackReaded(result, selectList);
                //GlobalValue.reReadIdList.Remove(id);

                //NoiseRecorder gpRec = (from item in GlobalValue.recorderList.AsEnumerable()
                //                       where item.ID == id
                //                       select item).ToList()[0];

                //dataList.Add(gpRec.Data);
                //resultList.Add(gpRec.Result);

                //BindResult(gpRec);
            }
            catch (TimeoutException)
            {
                simpleButtonRead.Enabled = true;
                simpleButtonSelectAll.Enabled = true;
                simpleButtonUnSelect.Enabled = true;
                HideWaitForm();
                EnableRibbonBar();
                EnableNavigateBar();
                if (!GlobalValue.reReadIdList.Contains(id))
                    GlobalValue.reReadIdList.Add(id);
                ShowDialog("记录仪" + id + "读取超时！", GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //isError = true;

            }
        }

        public override void OnSerialPortNotify(object sender, SerialPortEventArgs e)
        {
            if (e.TransactStatus != TransStatus.Start && e.OptType == SerialPortType.NoiseReadData)
            {
                string message = string.Empty;
                if (e.Tag != null)
                    message = e.Tag.ToString();

                this.Enabled = true;
                simpleButtonRead.Enabled = true;
                simpleButtonSelectAll.Enabled = true;
                simpleButtonUnSelect.Enabled = true;
                HideWaitForm();
                EnableRibbonBar();
                EnableNavigateBar();

                GlobalValue.SerialPortMgr.SerialPortEvent -= new SerialPortHandle(SerialPortNotify);
                if (e.TransactStatus == TransStatus.Success)
                {
                    Dictionary<short, short[]> result = new Dictionary<short, short[]>();
                    result.Add(GlobalValue.NoiseSerialPortOptData.ID, (short[])e.Tag);
                    string TestPath = Application.StartupPath + @"\Data\记录仪{0}\";
                    
                    string errmsg = NoiseDataHandler.CallbackReaded(result, selectList, TestPath, ref GlobalValue.recorderList);
                    if (!string.IsNullOrEmpty(errmsg))
                    {
                        ShowDialog("分析数据发生错误,errmsg:" + errmsg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    GlobalValue.reReadIdList.Remove(GlobalValue.NoiseSerialPortOptData.ID);

                    NoiseRecorder gpRec = (from item in GlobalValue.recorderList.AsEnumerable()
                                           where item.ID == GlobalValue.NoiseSerialPortOptData.ID
                                           select item).ToList()[0];
                    
                    dataList.Add(gpRec.Data);
                    resultList.Add(gpRec.Result);

                    BindResult(gpRec);

                    if (ReadIdList != null && ReadIdList.Count > 0)
                    {
                        ReadData(Convert.ToInt16(ReadIdList.Dequeue()));
                    }
                    else
                    {
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
                    gridViewResultList.RefreshData();
                }
                else
                {
                    XtraMessageBox.Show("读取数据失败!" + e.Msg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (ReadIdList != null && ReadIdList.Count > 0)
                    {
                        ReadData(Convert.ToInt16(ReadIdList.Dequeue()));
                    }
                    else
                    {
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
                    gridViewResultList.RefreshData();
                }
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
                    if (!selectList.Contains(rec))
                        selectList.Add(rec);
                }
                else
                {
                    for (int i = 0; i < selectList.Count; i++)
                    {
                        if (selectList[i].ID == rec.ID && selectList[i].GroupID == rec.GroupID)
                            selectList.RemoveAt(i);
                    }
                }
            }
        }

        // 重绘进度条
        private void gridViewGroupList_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.Caption == "读取进度")
            {
                try
                {
                    decimal percent = Convert.ToDecimal(e.CellValue);
                    int width = (int)(percent * e.Bounds.Width / 100);//涨跌幅最大为10%，所以要乘以10来计算比例，沾满一个单元格为10%  
                    Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y, width, e.Bounds.Height);
                    Brush b = Brushes.LightSteelBlue; //MediumTurquoise;
                    e.Graphics.FillRectangle(b, rect);
                }
                catch { }
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
        //查看详细
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
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
                GlobalValue.MainForm.LoadView(typeof(NoiseDataCompare));
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
                List<NoiseRecorder> lstSelecttmp = new List<NoiseRecorder>();
                lstSelecttmp.AddRange(selectList.OrderBy(a => a.ID));
                selectList = lstSelecttmp;

                btnReadFromFold.Enabled = false;
                List<NoiseData> dataList = new List<NoiseData>();
                List<NoiseResult> resultList = new List<NoiseResult>();
                GlobalValue.Noiselog.ValueChanged -= new ReadDataChangedEventHandler(log_ValueChanged);
                GlobalValue.Noiselog.ValueChanged += new ReadDataChangedEventHandler(log_ValueChanged);

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
                                string TestPath = Application.StartupPath + @"\Data\记录仪{0}\";
                                string errmsg = NoiseDataHandler.CallbackReaded(result, selectList, TestPath, ref GlobalValue.recorderList);
                                if (!string.IsNullOrEmpty(errmsg))
                                {
                                    ShowDialog("记录仪" + id + "分析数据发生错误,errmsg:" + errmsg, GlobalValue.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    isError = true;
                                    return;
                                }
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

        private void btnRefreshData_Click(object sender, EventArgs e)
        {
            GlobalValue.groupList = NoiseDataBaseHelper.GetGroups();
            GlobalValue.recorderList = NoiseDataBaseHelper.GetRecorders();
            GlobalValue.controllerList = NoiseDataBaseHelper.GetController();
            this.ClearView();
            this.BindGroup();
        }

        private void dataNavigator1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            //下一页  
            if ((e.Button).ButtonType == DevExpress.XtraEditors.NavigatorButtonType.NextPage)
            {
            }
            //上一页  
            if ((e.Button).ButtonType == DevExpress.XtraEditors.NavigatorButtonType.PrevPage)
            {
            }
            //首页  
            if ((e.Button).ButtonType == DevExpress.XtraEditors.NavigatorButtonType.First)
            {
            }
            //尾页  
            if ((e.Button).ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Last)
            {
            }
        }
    }
}
