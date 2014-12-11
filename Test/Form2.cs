using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NoiseAnalysisSystem;
using Protocol;

namespace Noise
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] bytes = ConvertHelper.HexStringToByteArray(this.textBox1.Text);
            int s = 0;
            for (int i = 0; i < bytes.Length - 2; i++)
            {
                s = s + bytes[i];
            }
            this.label1.Text = ((byte)s).ToString("x");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] bytes = ConvertHelper.HexStringToByteArray(this.textBox2.Text);
            Array.Reverse(bytes);
            List<byte> lst = bytes.ToList();

            while (lst.Count<4)
            {
                lst.Add(0);
            }

            this.label2.Text = BitConverter.ToInt32(lst.ToArray(), 0).ToString();

        }
    }
}
