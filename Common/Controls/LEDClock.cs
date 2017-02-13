using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace Common
{
    public partial class LEDClock : PictureBox
    {
        public LEDClock()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
            BackColor = Color.Black;
            Timer = new Timer { Interval = Interval, Enabled = true };
            Timer.Tick += Timer_Tick;
            InitColors();
            InitNumber();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            var bmp = new Bitmap(Width, Height);
            var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (Type != TimeType.时分秒)
            {
                var year1 = Convert.ToInt32(DateTime.Now.Year.ToString("0000")[0].ToString(CultureInfo.InvariantCulture));
                var year2 = Convert.ToInt32(DateTime.Now.Year.ToString("0000")[1].ToString(CultureInfo.InvariantCulture));
                var year3 = Convert.ToInt32(DateTime.Now.Year.ToString("0000")[2].ToString(CultureInfo.InvariantCulture));
                var year4 = Convert.ToInt32(DateTime.Now.Year.ToString("0000")[3].ToString(CultureInfo.InvariantCulture));
                DrawNumber(g, 0, year1);
                DrawNumber(g, 1, year2);
                DrawNumber(g, 2, year3);
                DrawNumber(g, 3, year4);
                DrawNumber(g, 4, -1);

                var month1 = Convert.ToInt32(DateTime.Now.Month.ToString("00")[0].ToString(CultureInfo.InvariantCulture));
                var month2 = Convert.ToInt32(DateTime.Now.Month.ToString("00")[1].ToString(CultureInfo.InvariantCulture));
                DrawNumber(g, 5, month1);
                DrawNumber(g, 6, month2);
                DrawNumber(g, 7, -1);

                var day1 = Convert.ToInt32(DateTime.Now.Day.ToString("00")[0].ToString(CultureInfo.InvariantCulture));
                var day2 = Convert.ToInt32(DateTime.Now.Day.ToString("00")[1].ToString(CultureInfo.InvariantCulture));
                DrawNumber(g, 8, day1);
                DrawNumber(g, 9, day2);
            }
            if (Type != TimeType.年月日)
            {
                var hour1 = Convert.ToInt32(DateTime.Now.Hour.ToString("00")[0].ToString(CultureInfo.InvariantCulture));
                var hour2 = Convert.ToInt32(DateTime.Now.Hour.ToString("00")[1].ToString(CultureInfo.InvariantCulture));
                DrawNumber(g, Type == TimeType.时分秒 ? 0 : 10, hour1);
                DrawNumber(g, Type == TimeType.时分秒 ? 1 : 11, hour2);

                DrawColon(g, 0);
                var minute1 =
                    Convert.ToInt32(DateTime.Now.Minute.ToString("00")[0].ToString(CultureInfo.InvariantCulture));
                var minute2 =
                    Convert.ToInt32(DateTime.Now.Minute.ToString("00")[1].ToString(CultureInfo.InvariantCulture));
                DrawNumber(g, Type == TimeType.时分秒 ? 2 : 12, minute1);
                DrawNumber(g, Type == TimeType.时分秒 ? 3 : 13, minute2);

                DrawColon(g, 1);
                var second1 =
                    Convert.ToInt32(DateTime.Now.Second.ToString("00")[0].ToString(CultureInfo.InvariantCulture));
                var second2 =
                    Convert.ToInt32(DateTime.Now.Second.ToString("00")[1].ToString(CultureInfo.InvariantCulture));
                DrawNumber(g, Type == TimeType.时分秒 ? 4 : 14, second1);
                DrawNumber(g, Type == TimeType.时分秒 ? 5 : 15, second2);
            }
            BackgroundImage = bmp;
        }

        /// <summary>
        /// 绘制数字
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="index">数字位置</param>
        /// <param name="value">数值</param>
        private void DrawNumber(Graphics g, int index, int value)
        {
            if (_numbers.Length > index && WordColor.ContainsKey(value))
            {
                foreach (var key in _numbers[index].Keys)
                {
                    var colors = WordColor[value].Where(s => s.Type == key).ToList();
                    if (colors.Any())
                    {
                        var c = colors[0].Color;
                        using (var brush = new SolidBrush(c))
                        {
                            g.FillPath(brush, _numbers[index][key]);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 绘制冒号
        /// </summary>
        /// <param name="g">GDI</param>
        /// <param name="index">绘制第几个</param>
        private void DrawColon(Graphics g, int index)
        {
            if (_colons.Length > index)
            {
                using (var brush = new SolidBrush(BrightColor))
                {
                    g.FillRectangles(brush, _colons[index]);
                }
            }
        }

        #region 属性
        private Timer Timer { set; get; }
        private TimeType _type = TimeType.时分秒;
        private int _interval = 1000;
        [Description("时钟显示的时间类型"), DefaultValue(typeof(TimeType)), Browsable(true)]
        public TimeType Type
        {
            set
            {
                Stop();
                _type = value;
                InitNumber();
                Start();
                Invalidate();
            }
            get { return _type; }
        }

        /// <summary>
        /// 数字路径
        /// </summary>
        private Dictionary<WordType, GraphicsPath>[] _numbers = new Dictionary<WordType, GraphicsPath>[0];
        /// <summary>
        /// 冒号
        /// </summary>
        private readonly RectangleF[][] _colons = new RectangleF[2][];

        /// <summary>
        /// 时钟跳动间隔
        /// </summary>
        [Description("时钟的跳动间隔（毫秒）"), DefaultValue(typeof(Int32)), Browsable(true)]
        public int Interval
        {
            set
            {
                _interval = value;
                Invalidate();
            }
            get { return _interval; }
        }

        private readonly Color[] _colors = { Color.FromArgb(0, 255, 1), Color.FromArgb(0, 60, 0) };

        /// <summary>
        /// 亮色
        /// </summary>
        [Description("数字的颜色"), DefaultValue(typeof(Color)), Browsable(true)]
        public Color BrightColor
        {
            set
            {
                _colors[0] = value;
                InitColors();
                Invalidate();
            }
            get { return _colors[0]; }
        }

        /// <summary>
        /// 暗色
        /// </summary>
        [Description("数字的背景颜色"), DefaultValue(typeof(Color)), Browsable(true)]
        public Color DarkColor
        {
            set
            {
                _colors[1] = value;
                InitColors();
                Invalidate();
            }
            get { return _colors[1]; }
        }
        /// <summary>
        /// 绘制路径
        /// </summary>
        private Dictionary<WordType, GraphicsPath> Paths { set; get; }
        /// <summary>
        /// 字体颜色设置
        /// </summary>
        private Dictionary<int, Word[]> WordColor { set; get; }

        /// <summary>
        /// 初始化字体颜色
        /// </summary>
        private void InitColors()
        {
            WordColor = new Dictionary<int, Word[]>
            {
                {
                    -1, new[]
                    {
                        new Word(WordType.TopHor, DarkColor),
                        new Word(WordType.LeftTopVer, DarkColor),
                        new Word(WordType.RightTopVer, DarkColor),
                        new Word(WordType.MiddleHor, BrightColor),
                        new Word(WordType.LeftBottomVer, DarkColor),
                        new Word(WordType.RightBottomVer, DarkColor),
                        new Word(WordType.BottomHor, DarkColor)
                    }
                },
                {
                    0, new[]
                    {
                        new Word(WordType.TopHor, BrightColor),
                        new Word(WordType.LeftTopVer, BrightColor),
                        new Word(WordType.RightTopVer, BrightColor),
                        new Word(WordType.MiddleHor, DarkColor),
                        new Word(WordType.LeftBottomVer, BrightColor),
                        new Word(WordType.RightBottomVer, BrightColor),
                        new Word(WordType.BottomHor, BrightColor)
                    }
                },
                {
                    1, new[]
                    {
                        new Word(WordType.TopHor, DarkColor),
                        new Word(WordType.LeftTopVer, DarkColor),
                        new Word(WordType.RightTopVer, BrightColor),
                        new Word(WordType.MiddleHor, DarkColor),
                        new Word(WordType.LeftBottomVer, DarkColor),
                        new Word(WordType.RightBottomVer, BrightColor),
                        new Word(WordType.BottomHor, DarkColor)
                    }
                },
                {
                    2, new[]
                    {
                        new Word(WordType.TopHor, BrightColor),
                        new Word(WordType.LeftTopVer, DarkColor),
                        new Word(WordType.RightTopVer, BrightColor),
                        new Word(WordType.MiddleHor, BrightColor),
                        new Word(WordType.LeftBottomVer, BrightColor),
                        new Word(WordType.RightBottomVer, DarkColor),
                        new Word(WordType.BottomHor, BrightColor)
                    }
                },
                {
                    3, new[]
                    {
                        new Word(WordType.TopHor, BrightColor),
                        new Word(WordType.LeftTopVer, DarkColor),
                        new Word(WordType.RightTopVer, BrightColor),
                        new Word(WordType.MiddleHor, BrightColor),
                        new Word(WordType.LeftBottomVer, DarkColor),
                        new Word(WordType.RightBottomVer, BrightColor),
                        new Word(WordType.BottomHor, BrightColor)
                    }
                },
                {
                    4, new[]
                    {
                        new Word(WordType.TopHor, DarkColor),
                        new Word(WordType.LeftTopVer, BrightColor),
                        new Word(WordType.RightTopVer, BrightColor),
                        new Word(WordType.MiddleHor, BrightColor),
                        new Word(WordType.LeftBottomVer, DarkColor),
                        new Word(WordType.RightBottomVer, BrightColor),
                        new Word(WordType.BottomHor, DarkColor)
                    }
                },
                {
                    5, new[]
                    {
                        new Word(WordType.TopHor, BrightColor),
                        new Word(WordType.LeftTopVer, BrightColor),
                        new Word(WordType.RightTopVer, DarkColor),
                        new Word(WordType.MiddleHor, BrightColor),
                        new Word(WordType.LeftBottomVer, DarkColor),
                        new Word(WordType.RightBottomVer, BrightColor),
                        new Word(WordType.BottomHor, BrightColor)
                    }
                },
                {
                    6, new[]
                    {
                        new Word(WordType.TopHor, BrightColor),
                        new Word(WordType.LeftTopVer, BrightColor),
                        new Word(WordType.RightTopVer, DarkColor),
                        new Word(WordType.MiddleHor, BrightColor),
                        new Word(WordType.LeftBottomVer, BrightColor),
                        new Word(WordType.RightBottomVer, BrightColor),
                        new Word(WordType.BottomHor, BrightColor)
                    }
                },
                {
                    7, new[]
                    {
                        new Word(WordType.TopHor, BrightColor),
                        new Word(WordType.LeftTopVer, DarkColor),
                        new Word(WordType.RightTopVer, BrightColor),
                        new Word(WordType.MiddleHor, DarkColor),
                        new Word(WordType.LeftBottomVer, DarkColor),
                        new Word(WordType.RightBottomVer, BrightColor),
                        new Word(WordType.BottomHor, DarkColor)
                    }
                },
                {
                    8, new[]
                    {
                        new Word(WordType.TopHor, BrightColor),
                        new Word(WordType.LeftTopVer, BrightColor),
                        new Word(WordType.RightTopVer, BrightColor),
                        new Word(WordType.MiddleHor, BrightColor),
                        new Word(WordType.LeftBottomVer, BrightColor),
                        new Word(WordType.RightBottomVer, BrightColor),
                        new Word(WordType.BottomHor, BrightColor)
                    }
                },
                {
                    9, new[]
                    {
                        new Word(WordType.TopHor, BrightColor),
                        new Word(WordType.LeftTopVer, BrightColor),
                        new Word(WordType.RightTopVer, BrightColor),
                        new Word(WordType.MiddleHor, BrightColor),
                        new Word(WordType.LeftBottomVer, DarkColor),
                        new Word(WordType.RightBottomVer, BrightColor),
                        new Word(WordType.BottomHor, DarkColor)
                    }
                }
            };
        }

        #endregion

        /// <summary>
        /// 启动时钟
        /// </summary>
        public void Start()
        {
            if (Timer != null)
            {
                Timer.Start();
            }
        }

        /// <summary>
        /// 关闭时钟
        /// </summary>
        public void Stop()
        {
            if (Timer != null)
            {
                Timer.Stop();
            }
        }

        /// <summary>
        /// 初始化数字路径
        /// </summary>
        private void InitNumber()
        {
            var length = 16;
            switch (Type)
            {
                case TimeType.年月日:
                    _numbers = new Dictionary<WordType, GraphicsPath>[10];
                    length = 10;
                    break;
                case TimeType.年月日时分秒:
                    _numbers = new Dictionary<WordType, GraphicsPath>[16];
                    length = 18;
                    break;
                case TimeType.时分秒:
                    length = 8;
                    _numbers = new Dictionary<WordType, GraphicsPath>[6];
                    break;
            }
            var w = Height / 2;
            int x = 5;
            int index = 0;
            for (int i = 0; i < length; i++)
            {
                switch (Type)
                {
                    case TimeType.时分秒:
                        if (i == 2 || i == 5)
                            _colons[i == 2 ? 0 : 1] = Colon(x, 5, Height - 10);
                        else
                        {
                            _numbers[index] = Number(x, 5, Height - 10);
                            index++;
                        }
                        x = (i == 2 || i == 5) ? x + 15 : w + x + 5;
                        break;
                    case TimeType.年月日:
                        _numbers[i] = Number(x, 5, Height - 10);
                        x = w + x + 5;
                        break;
                    case TimeType.年月日时分秒:
                        if (i != 12 && i != 15)
                        {
                            _numbers[index] = Number(x, 5, Height - 10);
                            index++;
                        }
                        x = (i == 9) ? w * 2 + x : (i == 12 || i == 15) ? x : w + x + 5;
                        if (i == 12 || i == 15)
                        {
                            _colons[i == 12 ? 0 : 1] = Colon(x, 5, Height - 10);
                            x = x + w / 2;
                        }
                        break;
                    default:
                        x = w + x + 5;
                        _numbers[i] = Number(x, 5, Height - 10);
                        break;
                }
            }
        }

        #region 绘制图像方法

        /// <summary>
        /// 绘制冒号 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private RectangleF[] Colon(float x, float y, float height)
        {
            var h = height / 20;
            var r1 = new RectangleF(x, y + h * 6, h * 2, h * 2);
            var r2 = new RectangleF(x, y + h * 12, h * 2, h * 2);
            return new[] { r1, r2 };
        }

        /// <summary>
        /// 绘制数字
        /// </summary>
        /// <param name="x">左上角X轴坐标</param>
        /// <param name="y">左上角Y轴坐标</param>
        /// <param name="height">数字高度</param>
        private Dictionary<WordType, GraphicsPath> Number(float x, float y, float height)
        {
            Paths = new Dictionary<WordType, GraphicsPath>();
            var h = height / 20;
            var w = h;
            var width = w * 10;
            var x1 = x;
            var x2 = x + w;
            var x3 = x + w * 2;
            var x4 = x + width - w * 2;
            var x5 = x + width - w;
            var x6 = x + width;
            var y1 = y;
            var y2 = y + h;
            var y3 = y + h * 2;
            var y4 = y + (height / 2 - h);
            var y5 = y + (height / 2);
            var y6 = y + (height / 2 + h);
            var y7 = y + height - h * 2;
            var y8 = y + height - h;
            var y9 = y + height;
            const float offset = 0.5f;
            var hor1 = new GraphicsPath();
            hor1.AddLines(new[]
            {
                new PointF(x2 + offset, y2 - offset),
                new PointF(x3 + offset, y1 + offset),
                new PointF(x4 - offset, y1 + offset),
                new PointF(x5 - offset, y2 - offset),
                new PointF(x4 - offset, y3 - offset),
                new PointF(x3 + offset, y3 - offset),
                new PointF(x2 + offset, y2 - offset)
            });
            hor1.CloseFigure();
            Paths.Add(WordType.TopHor, hor1);
            var hor2 = new GraphicsPath();
            hor2.AddLines(new[]
            {
                new PointF(x2 +offset, y5),
                new PointF(x3 + offset, y4 + offset),
                new PointF(x4 - offset, y4 + offset),
                new PointF(x5 - offset, y5),
                new PointF(x4 - offset, y6 - offset),
                new PointF(x3 + offset, y6 - offset),
                new PointF(x2 + offset, y5)
            });
            hor2.CloseFigure();
            Paths.Add(WordType.MiddleHor, hor2);
            var hor3 = new GraphicsPath();
            hor3.AddLines(new[]
            {
                new PointF(x2 + offset, y8),
                new PointF(x3 + offset, y7),
                new PointF(x4 - offset, y7),
                new PointF(x5 - offset, y8),
                new PointF(x4 - offset, y9),
                new PointF(x3 + offset, y9),
                new PointF(x2 + offset, y8)
            });
            hor3.CloseFigure();
            Paths.Add(WordType.BottomHor, hor3);
            var ver1 = new GraphicsPath();
            ver1.AddLines(new[]
            {
                new PointF(x1, y3 + offset),
                new PointF(x2, y2 + offset),
                new PointF(x3, y3 + offset),
                new PointF(x3, y4 - offset),
                new PointF(x2, y5 - offset),
                new PointF(x1, y4 - offset)
            });
            ver1.CloseFigure();
            Paths.Add(WordType.LeftTopVer, ver1);
            var ver2 = new GraphicsPath();
            ver2.AddLines(new[]
            {
                new PointF(x4, y3 + offset),
                new PointF(x5, y2 + offset),
                new PointF(x6, y3 + offset),
                new PointF(x6, y4 - offset),
                new PointF(x5, y5 - offset),
                new PointF(x4, y4 - offset)
            });
            ver2.CloseFigure();
            Paths.Add(WordType.RightTopVer, ver2);
            var ver3 = new GraphicsPath();
            ver3.AddLines(new[]
            {
                new PointF(x1, y6 + offset),
                new PointF(x2, y5 + offset),
                new PointF(x3, y6 + offset),
                new PointF(x3, y7 - offset),
                new PointF(x2, y8 - offset),
                new PointF(x1, y7 - offset)
            });
            ver3.CloseFigure();
            Paths.Add(WordType.LeftBottomVer, ver3);
            var ver4 = new GraphicsPath();
            ver4.AddLines(new[]
            {
                new PointF(x4, y6 + offset),
                new PointF(x5, y5 + offset),
                new PointF(x6, y6 + offset),
                new PointF(x6, y7 - offset),
                new PointF(x5, y8 - offset),
                new PointF(x4, y7 - offset)
            });
            ver4.CloseFigure();
            Paths.Add(WordType.RightBottomVer, ver4);
            return Paths;
        }

        #endregion

        protected override void OnResize(EventArgs e)
        {
            InitNumber();
            base.OnResize(e);
        }

        /// <summary>
        /// 数字线条结构
        /// </summary>
        public struct Word
        {
            public Word(WordType type, Color color)
                : this()
            {
                Type = type;
                Color = color;
            }
            /// <summary>
            /// 线条类型
            /// </summary>
            public WordType Type { set; get; }
            /// <summary>
            /// 线条颜色
            /// </summary>
            public Color Color { set; get; }
        }
        #region 隐藏的属性
        [Description("背景图像"), DefaultValue(null), Browsable(false)]
        public new Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }
        [Description("背景图像布局"), DefaultValue(typeof(ImageLayout)), Browsable(false)]
        public new ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }
        [Description("显示的图像"), DefaultValue(null), Browsable(false)]
        public new Image Image { get { return base.Image; } set { base.Image = value; } }
        [Description("加载错误时显示的图像"), DefaultValue(null), Browsable(false)]
        public new Image ErrorImage { get { return base.ErrorImage; } set { base.ErrorImage = value; } }
        [Description("加初始化时显示的图像"), DefaultValue(null), Browsable(false)]
        public new Image InitialImage { set { base.InitialImage = value; } get { return base.InitialImage; } }
        [Description("只是如何显示图像"), DefaultValue(typeof(PictureBoxSizeMode)), Browsable(false)]
        public new PictureBoxSizeMode SizeMode { set { base.SizeMode = value; } get { return base.SizeMode; } }
        #endregion
    }

    /// <summary>
    /// 数字线条枚举
    /// </summary>
    public enum WordType
    {
        TopHor = 0,
        LeftTopVer = 1,
        RightTopVer = 2,
        MiddleHor = 3,
        LeftBottomVer = 4,
        RightBottomVer = 5,
        BottomHor = 6
    }

    public enum TimeType
    {
        年月日,
        时分秒,
        年月日时分秒
    }
}
