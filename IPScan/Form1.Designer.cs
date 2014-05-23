namespace IPScan
{
    partial class Form1
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Stop = new System.Windows.Forms.Button();
            this.Scan = new System.Windows.Forms.Button();
            this.EndIp = new System.Windows.Forms.TextBox();
            this.StartIp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ScanProLab = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.IPList = new System.Windows.Forms.ListBox();
            this.printActive = new System.Windows.Forms.Button();
            this.clear = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Stop);
            this.groupBox1.Controls.Add(this.Scan);
            this.groupBox1.Controls.Add(this.EndIp);
            this.groupBox1.Controls.Add(this.StartIp);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(274, 240);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "参数设置";
            // 
            // Stop
            // 
            this.Stop.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Stop.ForeColor = System.Drawing.SystemColors.Desktop;
            this.Stop.Location = new System.Drawing.Point(147, 170);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(75, 23);
            this.Stop.TabIndex = 3;
            this.Stop.Text = "停止";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // Scan
            // 
            this.Scan.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Scan.ForeColor = System.Drawing.SystemColors.Desktop;
            this.Scan.Location = new System.Drawing.Point(14, 170);
            this.Scan.Name = "Scan";
            this.Scan.Size = new System.Drawing.Size(75, 23);
            this.Scan.TabIndex = 2;
            this.Scan.Text = "扫描";
            this.Scan.UseVisualStyleBackColor = true;
            this.Scan.Click += new System.EventHandler(this.Scan_Click);
            // 
            // EndIp
            // 
            this.EndIp.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.EndIp.Location = new System.Drawing.Point(116, 108);
            this.EndIp.Name = "EndIp";
            this.EndIp.Size = new System.Drawing.Size(116, 23);
            this.EndIp.TabIndex = 3;
            // 
            // StartIp
            // 
            this.StartIp.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.StartIp.Location = new System.Drawing.Point(116, 37);
            this.StartIp.Name = "StartIp";
            this.StartIp.Size = new System.Drawing.Size(116, 23);
            this.StartIp.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(11, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "结束地址";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(11, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "起始地址";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ScanProLab);
            this.groupBox2.Controls.Add(this.progressBar1);
            this.groupBox2.Location = new System.Drawing.Point(12, 267);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(274, 57);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "扫描进度";
            // 
            // ScanProLab
            // 
            this.ScanProLab.AutoSize = true;
            this.ScanProLab.Location = new System.Drawing.Point(12, 31);
            this.ScanProLab.Name = "ScanProLab";
            this.ScanProLab.Size = new System.Drawing.Size(53, 12);
            this.ScanProLab.TabIndex = 2;
            this.ScanProLab.Text = "扫描进度";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(88, 28);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(180, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // IPList
            // 
            this.IPList.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IPList.FormattingEnabled = true;
            this.IPList.ItemHeight = 14;
            this.IPList.Location = new System.Drawing.Point(313, 24);
            this.IPList.Name = "IPList";
            this.IPList.Size = new System.Drawing.Size(253, 228);
            this.IPList.TabIndex = 2;
            // 
            // printActive
            // 
            this.printActive.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.printActive.Location = new System.Drawing.Point(313, 295);
            this.printActive.Name = "printActive";
            this.printActive.Size = new System.Drawing.Size(145, 23);
            this.printActive.TabIndex = 3;
            this.printActive.Text = "打印活动主机";
            this.printActive.UseVisualStyleBackColor = true;
            this.printActive.Click += new System.EventHandler(this.printActive_Click);
            // 
            // clear
            // 
            this.clear.Location = new System.Drawing.Point(477, 295);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(75, 23);
            this.clear.TabIndex = 4;
            this.clear.Text = "清空";
            this.clear.UseVisualStyleBackColor = true;
            this.clear.Click += new System.EventHandler(this.clear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 347);
            this.Controls.Add(this.clear);
            this.Controls.Add(this.printActive);
            this.Controls.Add(this.IPList);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox EndIp;
        private System.Windows.Forms.TextBox StartIp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Scan;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label ScanProLab;
        private System.Windows.Forms.ListBox IPList;
        private System.Windows.Forms.Button printActive;
        private System.Windows.Forms.Button clear;
    }
}

