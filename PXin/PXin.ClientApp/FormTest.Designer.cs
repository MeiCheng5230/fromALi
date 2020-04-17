namespace PXin.ClientApp
{
    partial class FormTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCount = new System.Windows.Forms.TextBox();
            this.btnChatFee = new System.Windows.Forms.Button();
            this.btnClientCount = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtKeepAliveCount = new System.Windows.Forms.TextBox();
            this.btnActiveTest = new System.Windows.Forms.Button();
            this.txtRateQueryCount = new System.Windows.Forms.TextBox();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnRateQuery = new System.Windows.Forms.Button();
            this.txtTargetId = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSetRate = new System.Windows.Forms.Button();
            this.txtRate = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(227, 10);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 29);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "登录";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "连接数";
            // 
            // txtCount
            // 
            this.txtCount.Location = new System.Drawing.Point(76, 12);
            this.txtCount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(132, 25);
            this.txtCount.TabIndex = 2;
            this.txtCount.Text = "10";
            // 
            // btnChatFee
            // 
            this.btnChatFee.Location = new System.Drawing.Point(303, 64);
            this.btnChatFee.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnChatFee.Name = "btnChatFee";
            this.btnChatFee.Size = new System.Drawing.Size(100, 29);
            this.btnChatFee.TabIndex = 3;
            this.btnChatFee.Text = "聊天计费";
            this.btnChatFee.UseVisualStyleBackColor = true;
            this.btnChatFee.Click += new System.EventHandler(this.btnChatFee_Click);
            // 
            // btnClientCount
            // 
            this.btnClientCount.Location = new System.Drawing.Point(475, 10);
            this.btnClientCount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClientCount.Name = "btnClientCount";
            this.btnClientCount.Size = new System.Drawing.Size(100, 29);
            this.btnClientCount.TabIndex = 4;
            this.btnClientCount.Text = "连接数";
            this.btnClientCount.UseVisualStyleBackColor = true;
            this.btnClientCount.Click += new System.EventHandler(this.btnClientCount_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(87, 40);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(160, 23);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.Text = "单聊";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.comboBox1);
            this.groupBox3.Controls.Add(this.btnChatFee);
            this.groupBox3.Location = new System.Drawing.Point(19, 65);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Size = new System.Drawing.Size(676, 142);
            this.groupBox3.TabIndex = 47;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "聊天计费";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 98);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "聊天对象";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 44);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "聊天类型";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(87, 94);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(160, 25);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "3434909";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtKeepAliveCount);
            this.groupBox1.Controls.Add(this.btnActiveTest);
            this.groupBox1.Controls.Add(this.txtRateQueryCount);
            this.groupBox1.Controls.Add(this.cbType);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnRateQuery);
            this.groupBox1.Controls.Add(this.txtTargetId);
            this.groupBox1.Location = new System.Drawing.Point(19, 244);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(676, 131);
            this.groupBox1.TabIndex = 48;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "倍率查询";
            // 
            // txtKeepAliveCount
            // 
            this.txtKeepAliveCount.Location = new System.Drawing.Point(379, 80);
            this.txtKeepAliveCount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtKeepAliveCount.Name = "txtKeepAliveCount";
            this.txtKeepAliveCount.Size = new System.Drawing.Size(132, 25);
            this.txtKeepAliveCount.TabIndex = 47;
            this.txtKeepAliveCount.Text = "100";
            // 
            // btnActiveTest
            // 
            this.btnActiveTest.Location = new System.Drawing.Point(563, 78);
            this.btnActiveTest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnActiveTest.Name = "btnActiveTest";
            this.btnActiveTest.Size = new System.Drawing.Size(100, 29);
            this.btnActiveTest.TabIndex = 46;
            this.btnActiveTest.Text = "链路检测";
            this.btnActiveTest.UseVisualStyleBackColor = true;
            this.btnActiveTest.Click += new System.EventHandler(this.BtnActiveTest_Click);
            // 
            // txtRateQueryCount
            // 
            this.txtRateQueryCount.Location = new System.Drawing.Point(379, 42);
            this.txtRateQueryCount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRateQueryCount.Name = "txtRateQueryCount";
            this.txtRateQueryCount.Size = new System.Drawing.Size(132, 25);
            this.txtRateQueryCount.TabIndex = 45;
            this.txtRateQueryCount.Text = "100";
            // 
            // cbType
            // 
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cbType.Location = new System.Drawing.Point(57, 38);
            this.cbType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(81, 23);
            this.cbType.TabIndex = 44;
            this.cbType.Text = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 41);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 15);
            this.label5.TabIndex = 29;
            this.label5.Text = "type";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(171, 45);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 15);
            this.label6.TabIndex = 29;
            this.label6.Text = "senderId";
            // 
            // btnRateQuery
            // 
            this.btnRateQuery.Location = new System.Drawing.Point(563, 41);
            this.btnRateQuery.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRateQuery.Name = "btnRateQuery";
            this.btnRateQuery.Size = new System.Drawing.Size(100, 29);
            this.btnRateQuery.TabIndex = 43;
            this.btnRateQuery.Text = "倍率查询";
            this.btnRateQuery.UseVisualStyleBackColor = true;
            this.btnRateQuery.Click += new System.EventHandler(this.btnRateQuery_Click);
            // 
            // txtTargetId
            // 
            this.txtTargetId.Location = new System.Drawing.Point(255, 41);
            this.txtTargetId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTargetId.Name = "txtTargetId";
            this.txtTargetId.Size = new System.Drawing.Size(99, 25);
            this.txtTargetId.TabIndex = 28;
            this.txtTargetId.Text = "0";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.comboBox2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.btnSetRate);
            this.groupBox2.Controls.Add(this.txtRate);
            this.groupBox2.Location = new System.Drawing.Point(19, 421);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(676, 78);
            this.groupBox2.TabIndex = 49;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "设置倍率";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(333, 34);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 15);
            this.label8.TabIndex = 48;
            this.label8.Text = "Sender";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(412, 29);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(99, 25);
            this.textBox2.TabIndex = 47;
            this.textBox2.Text = "0";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.comboBox2.Location = new System.Drawing.Point(57, 30);
            this.comboBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(81, 23);
            this.comboBox2.TabIndex = 46;
            this.comboBox2.Text = "1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 34);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 15);
            this.label4.TabIndex = 45;
            this.label4.Text = "type";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(169, 34);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 15);
            this.label7.TabIndex = 29;
            this.label7.Text = "Rate";
            // 
            // btnSetRate
            // 
            this.btnSetRate.Location = new System.Drawing.Point(545, 29);
            this.btnSetRate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSetRate.Name = "btnSetRate";
            this.btnSetRate.Size = new System.Drawing.Size(100, 29);
            this.btnSetRate.TabIndex = 40;
            this.btnSetRate.Text = "设置倍率";
            this.btnSetRate.UseVisualStyleBackColor = true;
            this.btnSetRate.Click += new System.EventHandler(this.btnSetRate_Click);
            // 
            // txtRate
            // 
            this.txtRate.Location = new System.Drawing.Point(221, 30);
            this.txtRate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRate.Name = "txtRate";
            this.txtRate.Size = new System.Drawing.Size(99, 25);
            this.txtRate.TabIndex = 41;
            this.txtRate.Text = "2";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(353, 10);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 29);
            this.btnClose.TabIndex = 50;
            this.btnClose.Text = "退出";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 548);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnClientCount);
            this.Controls.Add(this.txtCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormTest";
            this.Text = "压力测试";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCount;
        private System.Windows.Forms.Button btnChatFee;
        private System.Windows.Forms.Button btnClientCount;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnRateQuery;
        private System.Windows.Forms.TextBox txtTargetId;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnSetRate;
        private System.Windows.Forms.TextBox txtRate;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox txtRateQueryCount;
        private System.Windows.Forms.Button btnActiveTest;
        private System.Windows.Forms.TextBox txtKeepAliveCount;
        private System.Windows.Forms.Button btnClose;
    }
}