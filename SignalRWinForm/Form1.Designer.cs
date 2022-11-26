namespace SignalRWinForm
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.txtLoginId = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtLoginResult = new System.Windows.Forms.TextBox();
			this.btnLogin = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.txtConnectionId = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtUserId = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.btnAllSend = new System.Windows.Forms.Button();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.txtLoginName = new System.Windows.Forms.TextBox();
			this.btnLogOut = new System.Windows.Forms.Button();
			this.btnChatRoomSend = new System.Windows.Forms.Button();
			this.cmbConnectionId = new System.Windows.Forms.ComboBox();
			this.cmbUserId = new System.Windows.Forms.ComboBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// txtLoginId
			// 
			this.txtLoginId.Location = new System.Drawing.Point(244, 12);
			this.txtLoginId.Name = "txtLoginId";
			this.txtLoginId.ReadOnly = true;
			this.txtLoginId.Size = new System.Drawing.Size(269, 23);
			this.txtLoginId.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(194, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(44, 17);
			this.label1.TabIndex = 1;
			this.label1.Text = "用户Id";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 17);
			this.label2.TabIndex = 3;
			this.label2.Text = "监听结果";
			// 
			// txtLoginResult
			// 
			this.txtLoginResult.Location = new System.Drawing.Point(71, 52);
			this.txtLoginResult.Multiline = true;
			this.txtLoginResult.Name = "txtLoginResult";
			this.txtLoginResult.Size = new System.Drawing.Size(899, 45);
			this.txtLoginResult.TabIndex = 2;
			// 
			// btnLogin
			// 
			this.btnLogin.Location = new System.Drawing.Point(773, 12);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.Size = new System.Drawing.Size(86, 23);
			this.btnLogin.TabIndex = 4;
			this.btnLogin.Text = "登录";
			this.btnLogin.UseVisualStyleBackColor = true;
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 118);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(85, 17);
			this.label3.TabIndex = 6;
			this.label3.Text = "ConnectionId";
			// 
			// txtConnectionId
			// 
			this.txtConnectionId.Location = new System.Drawing.Point(95, 64);
			this.txtConnectionId.Name = "txtConnectionId";
			this.txtConnectionId.Size = new System.Drawing.Size(259, 23);
			this.txtConnectionId.TabIndex = 5;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(437, 119);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(44, 17);
			this.label4.TabIndex = 8;
			this.label4.Text = "用户Id";
			// 
			// txtUserId
			// 
			this.txtUserId.Location = new System.Drawing.Point(437, 64);
			this.txtUserId.Name = "txtUserId";
			this.txtUserId.Size = new System.Drawing.Size(259, 23);
			this.txtUserId.TabIndex = 7;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 199);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(56, 17);
			this.label5.TabIndex = 10;
			this.label5.Text = "消息内容";
			// 
			// txtMessage
			// 
			this.txtMessage.Location = new System.Drawing.Point(74, 151);
			this.txtMessage.Multiline = true;
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.Size = new System.Drawing.Size(688, 110);
			this.txtMessage.TabIndex = 9;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(793, 207);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(178, 23);
			this.button2.TabIndex = 11;
			this.button2.Text = "给ConnectionId发消息";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.btnSendConnectionId_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(793, 238);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(178, 23);
			this.button3.TabIndex = 12;
			this.button3.Text = "给用户id发消息";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.btnSendUserId_Click);
			// 
			// btnAllSend
			// 
			this.btnAllSend.Location = new System.Drawing.Point(792, 148);
			this.btnAllSend.Name = "btnAllSend";
			this.btnAllSend.Size = new System.Drawing.Size(178, 23);
			this.btnAllSend.TabIndex = 13;
			this.btnAllSend.Text = "给全部人发消息";
			this.btnAllSend.UseVisualStyleBackColor = true;
			this.btnAllSend.Click += new System.EventHandler(this.btnAllSend_Click);
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 17;
			this.listBox1.Location = new System.Drawing.Point(12, 278);
			this.listBox1.Name = "listBox1";
			this.listBox1.ScrollAlwaysVisible = true;
			this.listBox1.Size = new System.Drawing.Size(958, 242);
			this.listBox1.TabIndex = 14;
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(95, 12);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(91, 25);
			this.comboBox1.TabIndex = 15;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(9, 15);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(80, 17);
			this.label6.TabIndex = 16;
			this.label6.Text = "请选择聊天室";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(519, 15);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(44, 17);
			this.label7.TabIndex = 18;
			this.label7.Text = "登录名";
			// 
			// txtLoginName
			// 
			this.txtLoginName.Location = new System.Drawing.Point(569, 12);
			this.txtLoginName.Name = "txtLoginName";
			this.txtLoginName.Size = new System.Drawing.Size(178, 23);
			this.txtLoginName.TabIndex = 17;
			// 
			// btnLogOut
			// 
			this.btnLogOut.Location = new System.Drawing.Point(874, 12);
			this.btnLogOut.Name = "btnLogOut";
			this.btnLogOut.Size = new System.Drawing.Size(86, 23);
			this.btnLogOut.TabIndex = 19;
			this.btnLogOut.Text = "注销登录";
			this.btnLogOut.UseVisualStyleBackColor = true;
			this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
			// 
			// btnChatRoomSend
			// 
			this.btnChatRoomSend.Location = new System.Drawing.Point(792, 177);
			this.btnChatRoomSend.Name = "btnChatRoomSend";
			this.btnChatRoomSend.Size = new System.Drawing.Size(178, 23);
			this.btnChatRoomSend.TabIndex = 20;
			this.btnChatRoomSend.Text = "给所在聊天室发消息";
			this.btnChatRoomSend.UseVisualStyleBackColor = true;
			this.btnChatRoomSend.Click += new System.EventHandler(this.btnChatRoomSend_Click);
			// 
			// cmbConnectionId
			// 
			this.cmbConnectionId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbConnectionId.FormattingEnabled = true;
			this.cmbConnectionId.Location = new System.Drawing.Point(95, 113);
			this.cmbConnectionId.Name = "cmbConnectionId";
			this.cmbConnectionId.Size = new System.Drawing.Size(315, 25);
			this.cmbConnectionId.TabIndex = 21;
			// 
			// cmbUserId
			// 
			this.cmbUserId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbUserId.FormattingEnabled = true;
			this.cmbUserId.Location = new System.Drawing.Point(487, 113);
			this.cmbUserId.Name = "cmbUserId";
			this.cmbUserId.Size = new System.Drawing.Size(315, 25);
			this.cmbUserId.TabIndex = 22;
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(983, 531);
			this.Controls.Add(this.cmbUserId);
			this.Controls.Add(this.cmbConnectionId);
			this.Controls.Add(this.btnChatRoomSend);
			this.Controls.Add(this.btnLogOut);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.txtLoginName);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.btnAllSend);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtMessage);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtUserId);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtConnectionId);
			this.Controls.Add(this.btnLogin);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtLoginResult);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtLoginId);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "SignalR版聊天室";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private TextBox txtLoginId;
		private Label label1;
		private Label label2;
		private TextBox txtLoginResult;
		private Button btnLogin;
		private Label label3;
		private TextBox txtConnectionId;
		private Label label4;
		private TextBox txtUserId;
		private Label label5;
		private TextBox txtMessage;
		private Button button2;
		private Button button3;
		private Button btnAllSend;
		private ListBox listBox1;
		private ComboBox comboBox1;
		private Label label6;
		private Label label7;
		private TextBox txtLoginName;
		private Button btnLogOut;
		private Button btnChatRoomSend;
		private ComboBox cmbConnectionId;
		private ComboBox cmbUserId;
		private System.Windows.Forms.Timer timer1;
	}
}