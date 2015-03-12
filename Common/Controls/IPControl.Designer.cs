namespace Common
{
    partial class IPControl
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
            this.txtNum1 = new System.Windows.Forms.TextBox();
            this.txtNum2 = new System.Windows.Forms.TextBox();
            this.txtNum3 = new System.Windows.Forms.TextBox();
            this.txtNum4 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtNum1
            // 
            this.txtNum1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNum1.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtNum1.Location = new System.Drawing.Point(2, 1);
            this.txtNum1.MaxLength = 3;
            this.txtNum1.Name = "txtNum1";
            this.txtNum1.Size = new System.Drawing.Size(21, 15);
            this.txtNum1.TabIndex = 0;
            this.txtNum1.Tag = "1";
            this.txtNum1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtNum1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNum_KeyPress);
            // 
            // txtNum2
            // 
            this.txtNum2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNum2.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtNum2.Location = new System.Drawing.Point(26, 1);
            this.txtNum2.MaxLength = 3;
            this.txtNum2.Name = "txtNum2";
            this.txtNum2.Size = new System.Drawing.Size(21, 15);
            this.txtNum2.TabIndex = 1;
            this.txtNum2.Tag = "2";
            this.txtNum2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtNum2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNum_KeyPress);
            // 
            // txtNum3
            // 
            this.txtNum3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNum3.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtNum3.Location = new System.Drawing.Point(50, 1);
            this.txtNum3.MaxLength = 3;
            this.txtNum3.Name = "txtNum3";
            this.txtNum3.Size = new System.Drawing.Size(21, 15);
            this.txtNum3.TabIndex = 2;
            this.txtNum3.Tag = "3";
            this.txtNum3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtNum3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNum_KeyPress);
            // 
            // txtNum4
            // 
            this.txtNum4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNum4.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtNum4.Location = new System.Drawing.Point(74, 1);
            this.txtNum4.MaxLength = 3;
            this.txtNum4.Name = "txtNum4";
            this.txtNum4.Size = new System.Drawing.Size(21, 15);
            this.txtNum4.TabIndex = 3;
            this.txtNum4.Tag = "4";
            this.txtNum4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtNum4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNum_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(18, -5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = ".";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(42, -5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = ".";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(66, -5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 21);
            this.label3.TabIndex = 1;
            this.label3.Text = ".";
            // 
            // IPControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.txtNum4);
            this.Controls.Add(this.txtNum3);
            this.Controls.Add(this.txtNum2);
            this.Controls.Add(this.txtNum1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "IPControl";
            this.Size = new System.Drawing.Size(98, 17);
            this.Load += new System.EventHandler(this.IPControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNum1;
        private System.Windows.Forms.TextBox txtNum2;
        private System.Windows.Forms.TextBox txtNum3;
        private System.Windows.Forms.TextBox txtNum4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
