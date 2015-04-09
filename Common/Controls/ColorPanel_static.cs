using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Common
{
    public partial class ColorPanel_static : UserControl
    {
        private Color[] colorbarvalue = null;
        public Color[] ColorBarValue
        {
            set{ colorbarvalue = value;}
        }

        public ColorPanel_static()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = e.ClipRectangle;// new Rectangle(697, 39, 8, 298);
            if ((rect.X == 0) && (rect.Y == 0) && (rect.Width == 0) && (rect.Height == 0))
                return;

            LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
            Color[] colors = new Color[3];
            colors[0] = Color.Yellow;
            colors[1] = Color.Red;
            colors[2] = Color.Blue;

            ColorBlend blend = new ColorBlend();
            blend.Positions = new float[] { 0.0f, 0.5f, 1.0f };
            blend.Colors = colors;
            brush.InterpolationColors = blend;
            e.Graphics.FillRectangle(brush, rect);
            brush.Dispose();
        }

        public void SetColorPanel(int min,int minmid,int mid,int midmax,int max)
        {
            lbMin.Text = min + "HZ";
            lbMinMid.Text = minmid.ToString();
            lbMidlle.Text = mid.ToString();
            lbMidMax.Text = midmax.ToString();
            lbMax.Text = max.ToString();
        }
    }
}
