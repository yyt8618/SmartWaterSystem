using System.Windows.Forms;
namespace Common
{
    partial class ColorPanel
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbMax = new System.Windows.Forms.Label();
            this.lbMidMax = new System.Windows.Forms.Label();
            this.lbMidlle = new System.Windows.Forms.Label();
            this.lbMinMid = new System.Windows.Forms.Label();
            this.lbMin = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 226);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // lbMax
            // 
            this.lbMax.AutoSize = true;
            this.lbMax.Location = new System.Drawing.Point(12, 4);
            this.lbMax.Name = "lbMax";
            this.lbMax.Size = new System.Drawing.Size(23, 12);
            this.lbMax.TabIndex = 1;
            this.lbMax.Text = "123";
            // 
            // lbMidMax
            // 
            this.lbMidMax.AutoSize = true;
            this.lbMidMax.Location = new System.Drawing.Point(12, 56);
            this.lbMidMax.Name = "lbMidMax";
            this.lbMidMax.Size = new System.Drawing.Size(23, 12);
            this.lbMidMax.TabIndex = 1;
            this.lbMidMax.Text = "123";
            // 
            // lbMidlle
            // 
            this.lbMidlle.AutoSize = true;
            this.lbMidlle.Location = new System.Drawing.Point(12, 108);
            this.lbMidlle.Name = "lbMidlle";
            this.lbMidlle.Size = new System.Drawing.Size(23, 12);
            this.lbMidlle.TabIndex = 1;
            this.lbMidlle.Text = "123";
            // 
            // lbMinMid
            // 
            this.lbMinMid.AutoSize = true;
            this.lbMinMid.Location = new System.Drawing.Point(12, 160);
            this.lbMinMid.Name = "lbMinMid";
            this.lbMinMid.Size = new System.Drawing.Size(23, 12);
            this.lbMinMid.TabIndex = 1;
            this.lbMinMid.Text = "123";
            // 
            // lbMin
            // 
            this.lbMin.AutoSize = true;
            this.lbMin.Location = new System.Drawing.Point(13, 215);
            this.lbMin.Name = "lbMin";
            this.lbMin.Size = new System.Drawing.Size(29, 12);
            this.lbMin.TabIndex = 1;
            this.lbMin.Text = "(HZ)";
            // 
            // ColorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbMin);
            this.Controls.Add(this.lbMinMid);
            this.Controls.Add(this.lbMidlle);
            this.Controls.Add(this.lbMidMax);
            this.Controls.Add(this.lbMax);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "ColorPanel";
            this.Size = new System.Drawing.Size(48, 229);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbMax;
        private System.Windows.Forms.Label lbMidMax;
        private System.Windows.Forms.Label lbMidlle;
        private System.Windows.Forms.Label lbMinMid;
        private System.Windows.Forms.Label lbMin;
    }
}
