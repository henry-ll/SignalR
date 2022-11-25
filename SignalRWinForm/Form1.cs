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

		#region �¼���
		private void Form1_Load(object sender, EventArgs e)
		{
			bindCbox();
			this.txtLoginId.Text= Guid.NewGuid().ToString();
			//connection = new HubConnectionBuilder().WithUrl("https://localhost:5001/WebChatHub").Build();
			connection = new HubConnectionBuilder().WithUrl("https://localhost:7121/WebChatHub").Build();

			#region �¼�����

			#region ��½����
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
						listBox1.Items.Add("��״̬�� ����������...");
					});
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			};
			#endregion

			#region ȫ�ֹ㲥 ����
			//���������ң�1����Ϣ
			connection.On<string>("ReceiveMessage1", (message) =>
			{
				var result = JsonConvert.DeserializeObject<Msg_code>(message);
				var user = Chat_User.userList.Where(t => t.LoginId == txtLoginIdText).FirstOrDefault();
				this.Invoke(() =>
				{
					if (user!=null)
						listBox1.Items.Add("�������ң�1��| ȫ����Ϣ��" + result.Msg);
				});
			});
			//���������ң�2����Ϣ
			connection.On<string>("ReceiveMessage2", (message) =>
			{
				this.Invoke(() =>
				{
					var result = JsonConvert.DeserializeObject<Msg_code>(message);
					listBox1.Items.Add("�������ң�2��| ȫ����Ϣ��" + result.Msg);
				});
			});
			//���������ң�3����Ϣ
			connection.On<string>("ReceiveMessage3", (message) =>
			{
				this.Invoke(() =>
				{
					var result = JsonConvert.DeserializeObject<Msg_code>(message);
					listBox1.Items.Add("�������ң�3��| ȫ����Ϣ��" + result.Msg);
				});
			});

			#endregion

			#region ��½��� ����
			connection.On<string>("ReturnConId", (message) =>
			{
				this.Invoke(() =>
				{
					txtLoginResult.Text = message;
					var result = JsonConvert.DeserializeObject<Msg_code>(message);
					listBox1.Items.Add("��״̬�� " + result.Msg);
				});
			});
			#endregion

			#region ���ո�����Ϣ
			connection.On<string, string>("ReceiveMsg", (ConnectionId, message) =>
			{
				this.Invoke(() =>
				{
					listBox1.Items.Add(ConnectionId + ":" + message);
				});
			});
			#endregion

			#region �����˳���Ϣ
			connection.On<string>("Closed", (ConnectionId) =>
			{
				this.Invoke(() =>
				{
					listBox1.Items.Add(ConnectionId + "���˳�");
				});
			});
			#endregion

			#endregion

			connection.StartAsync();
		}
		/// <summary>
		/// ��¼ ����¼�
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
		/// ע����¼ ����¼�
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
		/// ��ConnectionId����Ϣ ����¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSendConnectionId_Click(object sender, EventArgs e)
		{
			try
			{
				#region ConnectionId���͸�����Ϣ
				connection.InvokeAsync("SendToMessage", txtConnectionId.Text, txtMessage.Text);
				#endregion
			}
			catch (Exception ex)
			{
				listBox1.Items.Add(ex.Message);
			}
		}
		/// <summary>
		/// ���û�Id����Ϣ ����¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSendUserId_Click(object sender, EventArgs e)
		{
			try
			{
				#region �û�id���͸�����Ϣ
				connection.InvokeAsync("SendLoginMessage", txtUserId.Text, txtMessage.Text);
				#endregion
			}
			catch (Exception ex)
			{
				listBox1.Items.Add(ex.Message);
			}
		}
		/// <summary>
		/// ��ȫ���˷���Ϣ ����¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAllSend_Click(object sender, EventArgs e)
		{
			try
			{
				#region ����ȫ��㲥
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

		#region ����
        /// <summary>
		/// ������ ������Դ
		/// </summary>
		private void bindCbox()
		{
			List<ComboBoxData> infoList = new List<ComboBoxData>();
			ComboBoxData info1 = new ComboBoxData() { Id = "1", Name = "�����ң�1��" };
			ComboBoxData info2 = new ComboBoxData() { Id = "2", Name = "�����ң�2��" };
			ComboBoxData info3 = new ComboBoxData() { Id = "3", Name = "�����ң�3��" };
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