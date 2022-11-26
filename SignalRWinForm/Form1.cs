using Microsoft.AspNetCore.SignalR.Client;
using System.Windows.Forms;
using Newtonsoft.Json;
using SignalRHub.Web;
using SignalRHub;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SignalRWinForm
{
	public partial class Form1 : Form
	{
		HubConnection connection;
		public Form1()
		{
			InitializeComponent();
		}

		private bool IsDisConnect = false;
		private List<User> userList = new List<User>();
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
				if (!IsDisConnect)
				{
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
				}
			};
			#endregion

			#region ��������Ϣ ����
			//���� ������ ��Ϣ
			connection.On<string>("ReceiveMessage", (message) =>
			{
				var messageType = "��δ֪��";
				var result = JsonConvert.DeserializeObject<Msg_code>(message);
				dynamic data = result.data;
				if (data.group=="0")
					messageType = $"��������| ȫ����Ϣ�� ";
				else
					messageType = $"�������ң�{data.group}���� ";
				this.Invoke(() =>
				{
					listBox1.Items.Add(messageType + data.Name + $"��{result.Msg}");
				});
			});
			#endregion

			#region ��״̬����Ϣ ����
			connection.On<string>("StateMessage", (message) =>
			{
				var result = JsonConvert.DeserializeObject<Msg_code>(message);
				dynamic data = result.data;
				this.Invoke(() =>
				{
					txtLoginResult.Text = message;

					if (result.code == 205)//��¼
					{
						cmbConnectionId.DataSource = result.uInfo;
						cmbConnectionId.ValueMember = "ConnectionId";
						cmbConnectionId.DisplayMember = "LoginName";
						cmbUserId.DataSource = result.uInfo;
						cmbUserId.ValueMember = "LoginId";
						cmbUserId.DisplayMember = "LoginName";
					}
					else if (result.code == 206)//ע����¼
					{
						cmbConnectionId.DataSource = result.uInfo;
						cmbConnectionId.ValueMember = "ConnectionId";
						cmbConnectionId.DisplayMember = "LoginName";
						cmbUserId.DataSource = result.uInfo;
						cmbUserId.ValueMember = "LoginId";
						cmbUserId.DisplayMember = "LoginName";
					}
					listBox1.Items.Add("��״̬�� " + result.Msg);
				});
			});
			#endregion

			#region ConnectionId������Ϣ ����
			connection.On<string>("ReceiveMsg", (message) =>
			{
				var result = JsonConvert.DeserializeObject<Msg_code>(message);
				dynamic data = result.data;
				var messageType = "��˽�ġ�";
				this.Invoke(() =>
				{
					listBox1.Items.Add(messageType+ data.SendName+"  ��  "+ data.ReceiveName +"˵��"+result.Msg);
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


			#region �����û� ����
			connection.On<string>("OnlineUser", (message) =>
			{
				var result = JsonConvert.DeserializeObject<Msg_code>(message);
				this.Invoke(() =>
				{
					cmbConnectionId.DataSource = result.uInfo;
					cmbConnectionId.ValueMember = "ConnectionId";
					cmbConnectionId.DisplayMember = "LoginName";
					cmbUserId.DataSource = result.uInfo;
					cmbUserId.ValueMember = "LoginId";
					cmbUserId.DisplayMember = "LoginName";
				});
			});
			#endregion
			#endregion
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
				if (string.IsNullOrWhiteSpace(this.txtLoginName.Text))
				{
					MessageBox.Show("�������û�����");
					return;
				}
				IsDisConnect = false;
				connection.StartAsync();
				var comboBox = (ComboBoxData)comboBox1.SelectedItem;
				connection.InvokeAsync("LoginBind", comboBox.Id, this.txtLoginId.Text, this.txtLoginName.Text);
				this.btnLogin.Enabled = false;
				this.comboBox1.Enabled = false;
				this.txtLoginName.Enabled = false;
				return;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		/// <summary>
		/// ע����¼ ����¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnLogOut_Click(object sender, EventArgs e)
		{
			try
			{
				IsDisConnect = true;
				var comboBox = (ComboBoxData)comboBox1.SelectedItem;
				connection.InvokeAsync("LoginDisConnect", comboBox.Id, this.txtLoginId.Text, this.txtLoginName.Text).Wait();
				connection.StopAsync();
				this.btnLogin.Enabled = true;
				this.comboBox1.Enabled = true;
				this.txtLoginId.Enabled = true;
				this.txtLoginName.Enabled = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
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
				connection.InvokeAsync("SendConnectionIdMessage", txtConnectionId.Text, txtMessage.Text).Wait();
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
				if (string.IsNullOrWhiteSpace(this.txtMessage.Text))
				{
					MessageBox.Show("��������Ϣ���ݣ�");
					return;
				}
				connection.InvokeAsync("SendAllMessage",this.txtMessage.Text).Wait();
			}
			catch (Exception ex)
			{
				listBox1.Items.Add(ex.Message);
			}
		}
		/// <summary>
		/// �����������ҷ�����Ϣ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnChatRoomSend_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(this.txtMessage.Text))
				{
					MessageBox.Show("��������Ϣ���ݣ�");
					return;
				}
				var comboBox = (ComboBoxData)comboBox1.SelectedItem;
				connection.InvokeAsync("SendGroupMessage", comboBox.Id, this.txtMessage.Text).Wait();
			}
			catch (Exception ex)
			{
				listBox1.Items.Add(ex.Message);
			}
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			connection.InvokeAsync("GetOnlineUser").Wait();
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
	public class User
	{
		public string ConnectionId { get; set; }
		public string UserId { get; set; }
		public string ConnectionIdName { get; set; }
		public string UserIdName { get; set; }
		public string group { get; set; }
	}
}