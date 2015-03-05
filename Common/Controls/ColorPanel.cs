using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace NoiseAnalysisSystem
{
    public partial class ColorPanel : UserControl
    {
        private Color[] colorbarvalue = null;
        public Color[] ColorBarValue
        {
            set{ colorbarvalue = value;}
        }
        

        public ColorPanel()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = e.ClipRectangle;
            if ((rect.X == 0) && (rect.Y == 0) &&(rect.Width == 0) && (rect.Height == 0))
                return;
            if(colorbarvalue == null)
                return;
            
            LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);

            ColorBlend blend = new ColorBlend();
            blend.Positions = new float[] { 0.0f, 0.5f, 1.0f };
            blend.Colors = colorbarvalue;
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
