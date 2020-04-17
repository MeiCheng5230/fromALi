namespace PXin.ClientApp
{
    partial class FormClient
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
            this.btnSendMsgLogin = new System.Windows.Forms.Button();
            this.btnEnd = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnServiceClientCount = new System.Windows.Forms.Button();
            this.txtDTN = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnPM = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnSendMsgLogout = new System.Windows.Forms.Button();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.btnSendMsgActive = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSetRate = new System.Windows.Forms.Button();
            this.txtRate = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNodeId = new System.Windows.Forms.TextBox();
            this.btnRateQuery = new System.Windows.Forms.Button();
            this.txtTargetId = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rtbState = new System.Windows.Forms.RichTextBox();
            this.rtbMsg = new System.Windows.Forms.RichTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtRecvTargetid = new System.Windows.Forms.TextBox();
            this.comRecvType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnChatFee = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSendMsgLogin
            // 
            this.btnSendMsgLogin.Location = new System.Drawing.Point(321, 19);
            this.btnSendMsgLogin.Name = "btnSendMsgLogin";
            this.btnSendMsgLogin.Size = new System.Drawing.Size(75, 23);
            this.btnSendMsgLogin.TabIndex = 5;
            this.btnSendMsgLogin.Text = "登陆消息";
            this.btnSendMsgLogin.UseVisualStyleBackColor = true;
            this.btnSendMsgLogin.Click += new System.EventHandler(this.btnSendMsg_Click);
            // 
            // btnEnd
            // 
            this.btnEnd.Location = new System.Drawing.Point(84, 56);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(75, 23);
            this.btnEnd.TabIndex = 4;
            this.btnEnd.Text = "关闭";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(3, 56);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "连接";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(987, 194);
            this.panel1.TabIndex = 6;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnClear);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.btnServiceClientCount);
            this.groupBox3.Controls.Add(this.txtDTN);
            this.groupBox3.Controls.Add(this.btnSendMsgLogin);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.btnPM);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.btnTest);
            this.groupBox3.Controls.Add(this.btnSendMsgLogout);
            this.groupBox3.Controls.Add(this.txtPwd);
            this.groupBox3.Controls.Add(this.btnSendMsgActive);
            this.groupBox3.Controls.Add(this.btnStart);
            this.groupBox3.Controls.Add(this.btnEnd);
            this.groupBox3.Location = new System.Drawing.Point(9, 14);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Size = new System.Drawing.Size(527, 171);
            this.groupBox3.TabIndex = 46;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "基本操作";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(327, 104);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 42;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(165, 104);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 23);
            this.button1.TabIndex = 41;
            this.button1.Text = "聊天计费重复发送";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnServiceClientCount
            // 
            this.btnServiceClientCount.Location = new System.Drawing.Point(5, 104);
            this.btnServiceClientCount.Name = "btnServiceClientCount";
            this.btnServiceClientCount.Size = new System.Drawing.Size(125, 23);
            this.btnServiceClientCount.TabIndex = 40;
            this.btnServiceClientCount.Text = "服务器连接数量";
            this.btnServiceClientCount.UseVisualStyleBackColor = true;
            this.btnServiceClientCount.Click += new System.EventHandler(this.btnServiceClientCount_Click);
            // 
            // txtDTN
            // 
            this.txtDTN.Location = new System.Drawing.Point(74, 18);
            this.txtDTN.Name = "txtDTN";
            this.txtDTN.Size = new System.Drawing.Size(100, 21);
            this.txtDTN.TabIndex = 26;
            this.txtDTN.Text = "15000000";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 27;
            this.label8.Text = "Nodecode";
            // 
            // btnPM
            // 
            this.btnPM.Location = new System.Drawing.Point(327, 56);
            this.btnPM.Name = "btnPM";
            this.btnPM.Size = new System.Drawing.Size(75, 23);
            this.btnPM.TabIndex = 37;
            this.btnPM.Text = "粘包模拟";
            this.btnPM.UseVisualStyleBackColor = true;
            this.btnPM.Click += new System.EventHandler(this.btnPM_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(179, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 39;
            this.label1.Text = "Pwd";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(410, 19);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 35;
            this.btnTest.Text = "压力测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnSendMsgLogout
            // 
            this.btnSendMsgLogout.Location = new System.Drawing.Point(246, 56);
            this.btnSendMsgLogout.Name = "btnSendMsgLogout";
            this.btnSendMsgLogout.Size = new System.Drawing.Size(75, 23);
            this.btnSendMsgLogout.TabIndex = 7;
            this.btnSendMsgLogout.Text = "退出消息";
            this.btnSendMsgLogout.UseVisualStyleBackColor = true;
            this.btnSendMsgLogout.Click += new System.EventHandler(this.btnSendMsgLogout_Click);
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(208, 20);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(100, 21);
            this.txtPwd.TabIndex = 38;
            this.txtPwd.Text = "123456";
            // 
            // btnSendMsgActive
            // 
            this.btnSendMsgActive.Location = new System.Drawing.Point(165, 56);
            this.btnSendMsgActive.Name = "btnSendMsgActive";
            this.btnSendMsgActive.Size = new System.Drawing.Size(75, 23);
            this.btnSendMsgActive.TabIndex = 6;
            this.btnSendMsgActive.Text = "链路测试";
            this.btnSendMsgActive.UseVisualStyleBackColor = true;
            this.btnSendMsgActive.Click += new System.EventHandler(this.btnSendMsgActive_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.btnSetRate);
            this.groupBox2.Controls.Add(this.txtRate);
            this.groupBox2.Location = new System.Drawing.Point(541, 73);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(441, 54);
            this.groupBox2.TabIndex = 45;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "设置倍率";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 29;
            this.label4.Text = "Rate";
            // 
            // btnSetRate
            // 
            this.btnSetRate.Location = new System.Drawing.Point(184, 18);
            this.btnSetRate.Name = "btnSetRate";
            this.btnSetRate.Size = new System.Drawing.Size(75, 23);
            this.btnSetRate.TabIndex = 40;
            this.btnSetRate.Text = "设置倍率";
            this.btnSetRate.UseVisualStyleBackColor = true;
            this.btnSetRate.Click += new System.EventHandler(this.btnSetRate_Click);
            // 
            // txtRate
            // 
            this.txtRate.Location = new System.Drawing.Point(79, 20);
            this.txtRate.Name = "txtRate";
            this.txtRate.Size = new System.Drawing.Size(100, 21);
            this.txtRate.TabIndex = 41;
            this.txtRate.Text = "2";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbType);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtNodeId);
            this.groupBox1.Controls.Add(this.btnRateQuery);
            this.groupBox1.Controls.Add(this.txtTargetId);
            this.groupBox1.Location = new System.Drawing.Point(541, 14);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(441, 54);
            this.groupBox1.TabIndex = 44;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "倍率查询";
            // 
            // cbType
            // 
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cbType.Location = new System.Drawing.Point(44, 22);
            this.cbType.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(62, 20);
            this.cbType.TabIndex = 44;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 29;
            this.label5.Text = "type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(110, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 29;
            this.label2.Text = "nodeid";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(220, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 29;
            this.label3.Text = "targetId";
            // 
            // txtNodeId
            // 
            this.txtNodeId.Location = new System.Drawing.Point(157, 22);
            this.txtNodeId.Name = "txtNodeId";
            this.txtNodeId.Size = new System.Drawing.Size(59, 21);
            this.txtNodeId.TabIndex = 28;
            this.txtNodeId.Text = "4230136";
            // 
            // btnRateQuery
            // 
            this.btnRateQuery.Location = new System.Drawing.Point(361, 20);
            this.btnRateQuery.Name = "btnRateQuery";
            this.btnRateQuery.Size = new System.Drawing.Size(75, 23);
            this.btnRateQuery.TabIndex = 43;
            this.btnRateQuery.Text = "倍率查询";
            this.btnRateQuery.UseVisualStyleBackColor = true;
            this.btnRateQuery.Click += new System.EventHandler(this.btnRateQuery_Click);
            // 
            // txtTargetId
            // 
            this.txtTargetId.Location = new System.Drawing.Point(280, 23);
            this.txtTargetId.Name = "txtTargetId";
            this.txtTargetId.Size = new System.Drawing.Size(58, 21);
            this.txtTargetId.TabIndex = 28;
            this.txtTargetId.Text = "3435030";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rtbState);
            this.panel2.Controls.Add(this.rtbMsg);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 194);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(987, 553);
            this.panel2.TabIndex = 7;
            // 
            // rtbState
            // 
            this.rtbState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbState.Location = new System.Drawing.Point(509, 0);
            this.rtbState.Name = "rtbState";
            this.rtbState.Size = new System.Drawing.Size(478, 553);
            this.rtbState.TabIndex = 1;
            this.rtbState.Text = "";
            // 
            // rtbMsg
            // 
            this.rtbMsg.Dock = System.Windows.Forms.DockStyle.Left;
            this.rtbMsg.Location = new System.Drawing.Point(0, 0);
            this.rtbMsg.Name = "rtbMsg";
            this.rtbMsg.Size = new System.Drawing.Size(509, 553);
            this.rtbMsg.TabIndex = 0;
            this.rtbMsg.Text = "";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnChatFee);
            this.groupBox4.Controls.Add(this.comRecvType);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.txtRecvTargetid);
            this.groupBox4.Location = new System.Drawing.Point(541, 131);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(441, 54);
            this.groupBox4.TabIndex = 47;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "聊天计费";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(118, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 29;
            this.label9.Text = "targetId";
            // 
            // txtRecvTargetid
            // 
            this.txtRecvTargetid.Location = new System.Drawing.Point(180, 20);
            this.txtRecvTargetid.Name = "txtRecvTargetid";
            this.txtRecvTargetid.Size = new System.Drawing.Size(58, 21);
            this.txtRecvTargetid.TabIndex = 28;
            this.txtRecvTargetid.Text = "3435030";
            // 
            // comRecvType
            // 
            this.comRecvType.FormattingEnabled = true;
            this.comRecvType.Items.AddRange(new object[] {
            "1",
            "2"});
            this.comRecvType.Location = new System.Drawing.Point(47, 20);
            this.comRecvType.Margin = new System.Windows.Forms.Padding(2);
            this.comRecvType.Name = "comRecvType";
            this.comRecvType.Size = new System.Drawing.Size(62, 20);
            this.comRecvType.TabIndex = 46;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 45;
            this.label6.Text = "type";
            // 
            // btnChatFee
            // 
            this.btnChatFee.Location = new System.Drawing.Point(244, 19);
            this.btnChatFee.Name = "btnChatFee";
            this.btnChatFee.Size = new System.Drawing.Size(75, 23);
            this.btnChatFee.TabIndex = 47;
            this.btnChatFee.Text = "聊天计费";
            this.btnChatFee.UseVisualStyleBackColor = true;
            this.btnChatFee.Click += new System.EventHandler(this.btnChatFee_Click);
            // 
            // FormClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 747);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FormClient";
            this.Text = "客户端";
            this.Load += new System.EventHandler(this.FormClient_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSendMsgLogin;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox rtbMsg;
        private System.Windows.Forms.Button btnSendMsgActive;
        private System.Windows.Forms.Button btnSendMsgLogout;
        private System.Windows.Forms.RichTextBox rtbState;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDTN;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnPM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.Button btnSetRate;
        private System.Windows.Forms.Button btnRateQuery;
        private System.Windows.Forms.TextBox txtRate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTargetId;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNodeId;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnServiceClientCount;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox comRecvType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtRecvTargetid;
        private System.Windows.Forms.Button btnChatFee;
    }
}

