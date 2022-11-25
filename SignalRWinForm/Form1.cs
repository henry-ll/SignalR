using Microsoft.AspNetCore.SignalR.Client;
using System.Windows.Forms;
using Newtonsoft.Json;
using static SignalRHub.Web.WebChatHub;
using SignalRHub.Web;
using SignalRHub;

namespace SignalRWinForm
{
	public partial class Form1 : Form
	{
		HubConnection connection;
		public Form1()
		{
			InitializeComponent();
		}

		private string txtLoginIdText;

		#region 事件绑定
		private void Form1_Load(object sender, EventArgs e)
		{
			bindCbox();
			this.txtLoginId.Text= Guid.NewGuid().ToString();
			//connection = new HubConnectionBuilder().WithUrl("https://localhost:5001/WebChatHub").Build();
			connection = new HubConnectionBuilder().WithUrl("https://localhost:7121/WebChatHub").Build();

			#region 事件监听

			#region 登陆重试
			connection.Closed += async (error) =>
			{
				await Task.Delay(new Random().Next(0, 5) * 1000);
				await connection.StartAsync();
				try
				{
					var comboBox = (ComboBoxData)comboBox1.SelectedItem;
					await connection.InvokeAsync("LoginBind", comboBox.Id, this.txtLoginId.Text, this.txtLoginName.Text);
					this.Invoke(() =>
					{
						listBox1.Items.Add("【状态】 重新连接中...");
					});
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			};
			#endregion

			#region 全局广播 监听
			//监听聊天室（1）消息
			connection.On<string>("ReceiveMessage1", (message) =>
			{
				var result = JsonConvert.DeserializeObject<Msg_code>(message);
				var user = Chat_User.userList.Where(t => t.LoginId == txtLoginIdText).FirstOrDefault();
				this.Invoke(() =>
				{
					if (user!=null)
						listBox1.Items.Add("【聊天室（1）| 全体消息】" + result.Msg);
				});
			});
			//监听聊天室（2）消息
			connection.On<string>("ReceiveMessage2", (message) =>
			{
				this.Invoke(() =>
				{
					var result = JsonConvert.DeserializeObject<Msg_code>(message);
					listBox1.Items.Add("【聊天室（2）| 全体消息】" + result.Msg);
				});
			});
			//监听聊天室（3）消息
			connection.On<string>("ReceiveMessage3", (message) =>
			{
				this.Invoke(() =>
				{
					var result = JsonConvert.DeserializeObject<Msg_code>(message);
					listBox1.Items.Add("【聊天室（3）| 全体消息】" + result.Msg);
				});
			});

			#endregion

			#region 登陆结果 监听
			connection.On<string>("ReturnConId", (message) =>
			{
				this.Invoke(() =>
				{
					txtLoginResult.Text = message;
					var result = JsonConvert.DeserializeObject<Msg_code>(message);
					listBox1.Items.Add("【状态】 " + result.Msg);
				});
			});
			#endregion

			#region 接收个人信息
			connection.On<string, string>("ReceiveMsg", (ConnectionId, message) =>
			{
				this.Invoke(() =>
				{
					listBox1.Items.Add(ConnectionId + ":" + message);
				});
			});
			#endregion

			#region 接收退出信息
			connection.On<string>("Closed", (ConnectionId) =>
			{
				this.Invoke(() =>
				{
					listBox1.Items.Add(ConnectionId + "已退出");
				});
			});
			#endregion

			#endregion

			connection.StartAsync();
		}
		/// <summary>
		/// 登录 点击事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnLogin_Click(object sender, EventArgs e)
		{
			try
			{
				txtLoginIdText = this.txtLoginId.Text;
				var comboBox = (ComboBoxData)comboBox1.SelectedItem;
				connection.InvokeAsync("LoginBind", comboBox.Id, this.txtLoginId.Text, this.txtLoginName.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			this.btnLogin.Enabled = false;
			this.comboBox1.Enabled = false;
			this.txtLoginId.Enabled = false;
			this.txtLoginName.Enabled = false;
		}
		/// <summary>
		/// 注销登录 点击事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnLogOut_Click(object sender, EventArgs e)
		{
			//try
			//{
			//	var comboBox = (ComboBoxData)comboBox1.SelectedItem;
			//	connection.InvokeAsync("LoginBind", comboBox.Id, this.txtLoginId.Text, this.txtLoginName.Text);
			//}
			//catch (Exception ex)
			//{
			//	MessageBox.Show(ex.Message);
			//}
			this.btnLogin.Enabled = true;
			this.comboBox1.Enabled = true;
			this.txtLoginId.Enabled = true;
			this.txtLoginName.Enabled = true;
		}
		/// <summary>
		/// 给ConnectionId发消息 点击事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSendConnectionId_Click(object sender, EventArgs e)
		{
			try
			{
				#region ConnectionId发送个人信息
				connection.InvokeAsync("SendToMessage", txtConnectionId.Text, txtMessage.Text);
				#endregion
			}
			catch (Exception ex)
			{
				listBox1.Items.Add(ex.Message);
			}
		}
		/// <summary>
		/// 给用户Id发消息 点击事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSendUserId_Click(object sender, EventArgs e)
		{
			try
			{
				#region 用户id发送个人信息
				connection.InvokeAsync("SendLoginMessage", txtUserId.Text, txtMessage.Text);
				#endregion
			}
			catch (Exception ex)
			{
				listBox1.Items.Add(ex.Message);
			}
		}
		/// <summary>
		/// 给全部人发消息 点击事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAllSend_Click(object sender, EventArgs e)
		{
			try
			{
				#region 发送全体广播
				connection.InvokeAsync("SendAllMessage",
					 txtMessage.Text);
				#endregion
			}
			catch (Exception ex)
			{
				listBox1.Items.Add(ex.Message);
			}
		}
		#endregion

		#region 方法
        /// <summary>
		/// 下拉框 绑定数据源
		/// </summary>
		private void bindCbox()
		{
			List<ComboBoxData> infoList = new List<ComboBoxData>();
			ComboBoxData info1 = new ComboBoxData() { Id = "1", Name = "聊天室（1）" };
			ComboBoxData info2 = new ComboBoxData() { Id = "2", Name = "聊天室（2）" };
			ComboBoxData info3 = new ComboBoxData() { Id = "3", Name = "聊天室（3）" };
			infoList.Add(info1);
			infoList.Add(info2);
			infoList.Add(info3);
			comboBox1.DataSource = infoList;
			comboBox1.ValueMember = "Id";
			comboBox1.DisplayMember = "Name";
		}
		#endregion
	}
	public class ComboBoxData
	{
		public string Id { get; set; }
		public string Name { get; set; }
	}
}