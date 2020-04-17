namespace PXin.ServerApp
{
    partial class FormServer
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
            this.btnStart = new System.Windows.Forms.Button();
            this.btnEnd = new System.Windows.Forms.Button();
            this.btnSendMsgActive = new System.Windows.Forms.Button();
            this.rtbMsg = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClientCount = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(4, 15);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 29);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnEnd
            // 
            this.btnEnd.Location = new System.Drawing.Point(112, 15);
            this.btnEnd.Margin = new System.Windows.Forms.Padding(4);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(100, 29);
            this.btnEnd.TabIndex = 1;
            this.btnEnd.Text = "关闭";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // btnSendMsgActive
            // 
            this.btnSendMsgActive.Location = new System.Drawing.Point(220, 15);
            this.btnSendMsgActive.Margin = new System.Windows.Forms.Padding(4);
            this.btnSendMsgActive.Name = "btnSendMsgActive";
            this.btnSendMsgActive.Size = new System.Drawing.Size(100, 29);
            this.btnSendMsgActive.TabIndex = 2;
            this.btnSendMsgActive.Text = "链路测试";
            this.btnSendMsgActive.UseVisualStyleBackColor = true;
            this.btnSendMsgActive.Click += new System.EventHandler(this.btnSendActiveMsg_Click);
            // 
            // rtbMsg
            // 
            this.rtbMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbMsg.Location = new System.Drawing.Point(0, 0);
            this.rtbMsg.Margin = new System.Windows.Forms.Padding(4);
            this.rtbMsg.Name = "rtbMsg";
            this.rtbMsg.Size = new System.Drawing.Size(1164, 634);
            this.rtbMsg.TabIndex = 3;
            this.rtbMsg.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClientCount);
            this.panel1.Controls.Add(this.btnEnd);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Controls.Add(this.btnSendMsgActive);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1164, 64);
            this.panel1.TabIndex = 4;
            // 
            // btnClientCount
            // 
            this.btnClientCount.Location = new System.Drawing.Point(328, 15);
            this.btnClientCount.Margin = new System.Windows.Forms.Padding(4);
            this.btnClientCount.Name = "btnClientCount";
            this.btnClientCount.Size = new System.Drawing.Size(100, 29);
            this.btnClientCount.TabIndex = 3;
            this.btnClientCount.Text = "客户端数";
            this.btnClientCount.UseVisualStyleBackColor = true;
            this.btnClientCount.Click += new System.EventHandler(this.btnClientCount_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rtbMsg);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 64);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1164, 634);
            this.panel2.TabIndex = 5;
            // 
            // FormServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 698);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormServer";
            this.Text = "服务器";
            this.Load += new System.EventHandler(this.FormServer_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Button btnSendMsgActive;
        private System.Windows.Forms.RichTextBox rtbMsg;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnClientCount;
    }
}

