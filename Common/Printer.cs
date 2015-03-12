using System;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Drawing;

namespace Common
{
    /// <summary>
    /// 提供对DataGridView中数据的打印
    /// </summary>
    public class Printer
    {
        private DataGridView dataview;
        private PrintDocument printDoc;
        //打印有效区域的宽度
        int width;
        int height;
        int columns;
        double Rate;
        bool hasMorePage = false;
        int currRow = 0;
        int rowHeight = 20;
        //打印页数
        int PageNumber;
        //当前打印页的行数
        int pageSize = 20;
        //当前打印的页码
        int PageIndex;

        private int PageWidth; //打印纸的宽度
        private int PageHeight; //打印纸的高度
        private int LeftMargin; //有效打印区距离打印纸的左边大小
        private int TopMargin;//有效打印区距离打印纸的上面大小
        private int RightMargin;//有效打印区距离打印纸的右边大小
        private int BottomMargin;//有效打印区距离打印纸的下边大小

        int rows;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataview">要打印的DateGridView</param>
        /// <param name="printDoc">PrintDocument用于获取打印机的设置</param>
        public Printer(DataGridView dataview, PrintDocument printDoc)
        {
            this.dataview = dataview;
            this.printDoc = printDoc;
            PageIndex = 0;
            //获取打印数据的具体行数
            this.rows = dataview.RowCount;

            this.columns = dataview.ColumnCount - 2;
            //判断打印设置是否是横向打印
            if (!printDoc.DefaultPageSettings.Landscape)
            {
                PageWidth = printDoc.DefaultPageSettings.PaperSize.Width;
                PageHeight = printDoc.DefaultPageSettings.PaperSize.Height;
            }
            else
            {
                PageHeight = printDoc.DefaultPageSettings.PaperSize.Width;
                PageWidth = printDoc.DefaultPageSettings.PaperSize.Height;
            }
            LeftMargin = printDoc.DefaultPageSettings.Margins.Left;
            TopMargin = printDoc.DefaultPageSettings.Margins.Top;
            RightMargin = printDoc.DefaultPageSettings.Margins.Right;
            BottomMargin = printDoc.DefaultPageSettings.Margins.Bottom;

            height = PageHeight - TopMargin - BottomMargin - 2;
            width = PageWidth - LeftMargin - RightMargin - 2;

            double tempheight = height;
            double temprowHeight = rowHeight;
            while (true)
            {
                string temp = Convert.ToString(tempheight / Math.Round(temprowHeight, 3));
                int i = temp.IndexOf('.');
                double tt = 100;
                if (i != -1)
                {
                    tt = Math.Round(Convert.ToDouble(temp.Substring(temp.IndexOf('.'))), 3);
                }
                if (tt <= 0.01)
                {
                    rowHeight = Convert.ToInt32(temprowHeight);
                    break;
                }
                else
                {
                    temprowHeight = temprowHeight + 0.01;
                }
            }
            pageSize = height / rowHeight;
            if ((rows + 1) <= pageSize)
            {
                pageSize = rows + 1;
                PageNumber = 1;
            }
            else
            {
                PageNumber = rows / (pageSize - 1);
                if (rows % (pageSize - 1) != 0)
                {
                    PageNumber = PageNumber + 1;
                }
            }
        }

        /// <summary>
        /// 初始化打印
        /// </summary>
        private void InitPrint()
        {
            PageIndex = PageIndex + 1;
            if (PageIndex == PageNumber)
            {
                hasMorePage = false;
                if (PageIndex != 1)
                {
                    pageSize = rows % (pageSize - 1) + 1;
                }
            }
            else
            {
                hasMorePage = true;
            }
        }

        /// <summary>
        /// 打印头
        /// </summary>
        private void DrawHeader(Graphics g)
        {
            Font font = new Font("宋体", 12, FontStyle.Bold);
            int temptop = (rowHeight / 2) + TopMargin + 1;
            int templeft = LeftMargin + 1;

            for (int i = 0; i < this.columns; i++)
            {
                string headString = this.dataview.Columns[i].HeaderText;
                float fontHeight = g.MeasureString(headString, font).Height;
                float fontwidth = g.MeasureString(headString, font).Width;
                float temp = temptop - (fontHeight) / 3;
                g.DrawString(headString, font, Brushes.Black, new PointF(templeft, temp));
                templeft = templeft + (int)(this.dataview.Columns[i].Width / Rate) + 1;
            }
        }

        /// <summary>
        /// 画表格
        /// </summary>
        private void DrawTable(Graphics g)
        {
            Rectangle border = new Rectangle(LeftMargin, TopMargin, width, (pageSize) * rowHeight);
            g.DrawRectangle(new Pen(Brushes.Black, 2), border);
            for (int i = 1; i < pageSize; i++)
            {
                if (i != 1)
                {
                    g.DrawLine(new Pen(Brushes.Black, 1), new Point(LeftMargin + 1, (rowHeight * i) + TopMargin + 1), new Point(width + LeftMargin, (rowHeight * i) + TopMargin + 1));
                }
                else
                {
                    g.DrawLine(new Pen(Brushes.Black, 2), new Point(LeftMargin + 1, (rowHeight * i) + TopMargin + 1), new Point(width + LeftMargin, (rowHeight * i) + TopMargin + 1));
                }
            }

            //计算出列的总宽度和打印纸比率
            Rate = Convert.ToDouble(GetDateViewWidth()) / Convert.ToDouble(width);
            int tempLeft = LeftMargin + 1;
            int endY = (pageSize) * rowHeight + TopMargin;
            for (int i = 1; i < columns; i++)
            {
                tempLeft = tempLeft + 1 + (int)(this.dataview.Columns[i - 1].Width / Rate);
                g.DrawLine(new Pen(Brushes.Black, 1), new Point(tempLeft, TopMargin), new Point(tempLeft, endY));
            }
        }

        /// <summary>
        /// 获取打印的列的总宽度
        /// </summary>
        private int GetDateViewWidth()
        {
            int total = 0;
            for (int i = 0; i < this.columns; i++)
            {
                total = total + this.dataview.Columns[i].Width;
            }
            return total;
        }

        /// <summary>
        /// 打印行数据
        /// </summary>
        private void DrawRows(Graphics g)
        {
            Font font = new Font("宋体", 12, FontStyle.Regular);
            int temptop = (rowHeight / 2) + TopMargin + 1 + rowHeight;

            for (int i = currRow; i < pageSize + currRow - 1; i++)
            {
                int templeft = LeftMargin + 1;
                for (int j = 0; j < columns; j++)
                {
                    string headString = this.dataview.Rows[i].Cells[j].FormattedValue.ToString();
                    float fontHeight = g.MeasureString(headString, font).Height;
                    float fontwidth = g.MeasureString(headString, font).Width;
                    float temp = temptop - (fontHeight) / 3;
                    while (true)
                    {
                        if (fontwidth <= (int)(this.dataview.Columns[j].Width / Rate))
                        {
                            break;
                        }
                        else
                        {
                            headString = headString.Substring(0, headString.Length - 1);
                            fontwidth = g.MeasureString(headString, font).Width;
                        }
                    }
                    g.DrawString(headString, font, Brushes.Black, new PointF(templeft, temp));
                    templeft = templeft + (int)(this.dataview.Columns[j].Width / Rate) + 1;
                }
                temptop = temptop + rowHeight;
            }
            currRow = pageSize + currRow - 1;
        }

        /// <summary>
        /// 在PrintDocument中的PrintPage方法中调用
        /// </summary>
        /// <param name="g">传入PrintPage中PrintPageEventArgs中的Graphics</param>
        /// <returns>是否还有打印页 有返回true，无则返回false</returns>
        public bool Print(Graphics g)
        {
            InitPrint();
            DrawTable(g);
            DrawHeader(g);
            DrawRows(g);

            //打印页码
            string pagestr = PageIndex + " / " + PageNumber;
            Font font = new Font("宋体", 12, FontStyle.Regular);
            g.DrawString(pagestr, font, Brushes.Black, new PointF((PageWidth / 2) - g.MeasureString(pagestr, font).Width, PageHeight - (BottomMargin / 2) - g.MeasureString(pagestr, font).Height));
            //打印查询的功能项名称
            //string temp = dataview.Tag.ToString() + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            //g.DrawString(temp, font, Brushes.Black, new PointF(PageWidth - 5 - g.MeasureString(temp, font).Width, PageHeight - 5 - g.MeasureString(temp, font).Height));
            return hasMorePage;
        }
    }
}