using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRHub.Web
{
	public class WebChatHub : Hub
	{
		public List<string> mgstr = "日".Split(',').ToArray().ToList();//关键词屏蔽

		private readonly IServiceProvider _serviceProvider;
		public WebChatHub(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		/// <summary>
		/// 获取当前在线用户列表
		/// </summary>
		/// <returns></returns>
		public async Task GetOnlineUser()
		{
			Msg_code code = new Msg_code();
			try
			{
				code = new Msg_code(205, "获取成功", null, Chat_User.userList);
				await Clients.All.SendAsync("OnlineUser", code.ToJson());
				return;
			}
			catch (Exception ex)
			{
				code = new Msg_code(500, "获取在线用户列表失败");
				await Clients.Client(Context.ConnectionId).SendAsync("OnlineUser", code.ToJson());
				return;
			}
		}


		/// <summary>
		/// 注销登录
		/// </summary>
		/// <param name="group">聊天室Id</param>
		/// <param name="loginId">登录Id（用户Id）</param>
		/// <param name="username">用户名</param>
		/// <returns></returns>
		public async Task LoginDisConnect(string group, string loginId, string username)
		{
			Msg_code code = new Msg_code();
			try
			{
				Chat_User.RemoveUser(Context.ConnectionId);
				var data = new
				{
					ConnectionId = Context.ConnectionId,
					UserId = loginId,
					Name = username,
					group = group,
				};
				code = new Msg_code(206, "用户：" + username + $"，退出登录",data);
				await Clients.All.SendAsync("StateMessage", code.ToJson());
				return;
			}
			catch (Exception ex)
			{
				code = new Msg_code(500, "出现异常");
				await Clients.All.SendAsync("StateMessage", code.ToJson());
				return;
			}
		}

		/// <summary>
		/// 用户登陆
		/// </summary>
		/// <param name="group">聊天室Id</param>
		/// <param name="loginId">登录Id（用户Id）</param>
		/// <param name="username">用户名</param>
		/// <returns></returns>
		public async Task LoginBind(string group, string loginId, string username)
		{
			Msg_code code = new Msg_code();
			try
			{
				//将用户添加到缓存
				Chat_User.AddUser(loginId, Context.ConnectionId, username, group);
				var data = new
				{
					ConnectionId = Context.ConnectionId,
					UserId = loginId,
					Name = username,
					group = group,
				};
				code = new Msg_code(205, "登陆成功", data, Chat_User.userList);
				await Clients.Client(Context.ConnectionId).SendAsync("StateMessage", code.ToJson());

				code = new Msg_code(201, $"加入聊天室（{group}）", data);
				await Clients.All.SendAsync("ReceiveMessage", code.ToJson());
				return;
			}
			catch (Exception ex)
			{
				code = new Msg_code(500, "登陆失败");
				await Clients.Client(Context.ConnectionId).SendAsync("StateMessage", code.ToJson());
				return;
			}
		}

		/// <summary>
		/// 发送消息--发送给所有连接的客户端
		/// </summary>
		/// <param name="msg">消息内容</param>
		/// <returns></returns>
		public async Task SendAllMessage(string msg)
		{
			Msg_code code = new Msg_code();
			var user = GetUserinfo();
			if (user == null)
			{
				code = new Msg_code(500, "登陆已失效,请重新登陆");
				await Clients.Client(Context.ConnectionId).SendAsync("StateMessage", code.ToJson());
				return;
			}
			msg = pbstr(msg);
			var data = new
			{
				ConnectionId = Context.ConnectionId,
				UserId = user.LoginId,
				Name = user.LoginName,
				group = "0"
			};
			code = new Msg_code(200, msg, data);
			await Clients.All.SendAsync("ReceiveMessage", code.ToJson());
			return;
		}

		//发送消息--发送特定聊天室发信息
		public async Task SendGroupMessage(string group, string msg)
		{
			Msg_code code = new Msg_code();
			try
			{
				var user = GetUserinfo();
				if (user == null)
				{
					code = new Msg_code(500, "登陆已失效,请重新登陆");
					await Clients.Client(Context.ConnectionId).SendAsync("StateMessage", code.ToJson());
					return;
				}
				msg = pbstr(msg);
				var data = new
				{
					ConnectionId = Context.ConnectionId,
					UserId = user.LoginId,
					Name = user.LoginName,
					group = group
				};
				code = new Msg_code(200, msg, data);
				var clients = GetUserList().Where(x => x.group == group).Select(x=>x.Connectionid).ToList();
				await Clients.Clients(clients).SendAsync("ReceiveMessage", code.ToJson());
				return;
			}
			catch (Exception)
			{
				code = new Msg_code(500, "发送失败");
				await Clients.Client(Context.ConnectionId).SendAsync("StateMessage", code.ToJson());
				return;
			}
		}

		/// <summary>
		/// 给指定ConnectionId用户发送信息
		/// </summary>
		/// <param name="connectionId">connectionId</param>
		/// <param name="msg">内容</param>
		/// <returns></returns>
		public async Task SendConnectionIdMessage(string connectionId, string msg)
		{
			Msg_code code = new Msg_code();
			try
			{
				var user = GetUserinfo();
				if (user == null)
				{
					code = new Msg_code(500, "登陆已失效,请重新登陆");
					await Clients.Client(Context.ConnectionId).SendAsync("StateMessage", code.ToJson());
					return;
				}
				msg = pbstr(msg);
				var receiveuser = GetUserList().Where(x => x.Connectionid == connectionId).FirstOrDefault();
				if (receiveuser == null)
				{
					code = new Msg_code(500, "对方已离线");
					await Clients.Client(Context.ConnectionId).SendAsync("StateMessage", code.ToJson());
					return;
				}
				var data = new
				{
					SendConnectionId = Context.ConnectionId,
					UserId = user.LoginId,
					SendName = user.LoginName,
					ReceiveName = receiveuser?.LoginName,
					ReceiveConnectionId = connectionId
				};
				code = new Msg_code(200, msg, data);
				await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMsg", code.ToJson());
				await Clients.Client(connectionId).SendAsync("ReceiveMsg", code.ToJson());
				return;
			}
			catch (Exception)
			{
				code = new Msg_code(500, "发送失败");
				await Clients.Client(Context.ConnectionId).SendAsync("StateMessage", code.ToJson());
				return;
			}
		}

		/// <summary>
		/// 给UserId用户发送信息
		/// </summary>
		/// <param name="userId">userId</param>
		/// <param name="msg">内容</param>
		/// <returns></returns>
		public async Task SendLoginMessage(string userId, string msg)
		{
			Msg_code code = new Msg_code();
			try
			{
				var receiveuserinfo = Chat_User.userList.Where(t => t.LoginId == userId).FirstOrDefault();
				if (receiveuserinfo == null)
				{
					code = new Msg_code(500, "对方已离线");
					await Clients.Client(Context.ConnectionId).SendAsync("StateMessage", code.ToJson());
					return;
				}
				var user = GetUserinfo();
				if (user == null)
				{
					code = new Msg_code(500, "登陆已失效,请重新登陆");
					await Clients.Client(Context.ConnectionId).SendAsync("StateMessage", code.ToJson());
					return;
				}
				msg = pbstr(msg);
				var receiveuser = GetUserList().Where(x => x.Connectionid == receiveuserinfo.Connectionid).FirstOrDefault();
				var data = new
				{
					SendConnectionId = Context.ConnectionId,
					UserId = user.LoginId,
					SendName = user.LoginName,
					ReceiveName = receiveuser?.LoginName,
					ReceiveConnectionId = receiveuserinfo.Connectionid
				};
				code = new Msg_code(200, msg, data);
				await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMsg", code.ToJson());
				await Clients.Client(receiveuserinfo.Connectionid).SendAsync("ReceiveMsg", code.ToJson());
			}
			catch (Exception)
			{
				code = new Msg_code(500, "发送失败");
				await Clients.Client(Context.ConnectionId).SendAsync("StateMessage", code.ToJson());
				return;
			}
		}

		public async Task RemoveLogin(string ConnectionId)
		{
			Chat_User.RemoveUser(ConnectionId);
			await Clients.All.SendAsync("Closed", ConnectionId);
			return;
		}

		//重写上线监控
		public override async Task OnConnectedAsync()
		{
			var ConnectionId = Context.ConnectionId;
			Msg_code code = new Msg_code(202, ConnectionId + "链接成功", ConnectionId);
			await Clients.All.SendAsync("SystemNotice", code.ToJson());
			return;
		}

		//重写下线监控
		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			var ConnectionId = Context.ConnectionId;
			var user = GetUserinfo();
			if (user == null)
			{
				Msg_code code = new Msg_code(203, ConnectionId + "下线", ConnectionId);
				await Clients.All.SendAsync("SystemNotice", code.ToJson());
			}
			else
			{
				Msg_code code = new Msg_code(203, user.LoginName + "下线", user);
				await Clients.All.SendAsync("SystemNotice", code.ToJson());
			}
			Chat_User.RemoveUser(ConnectionId);
			return;
		}

		private string pbstr(string str)
		{
			var isbll = mgstr.Where(t => str.IndexOf(t) >= 0).ToList();
			if (isbll.Count > 0)
			{
				foreach (var item in isbll)
					str = str.Replace(item, "**");
			}
			return str;
		}

		private U_Info GetUserinfo()
		{
			var user = Chat_User.userList.Where(t => t.Connectionid == Context.ConnectionId).FirstOrDefault();
			return user;
		}

		private  List<U_Info>  GetUserList()
		{
			var user = Chat_User.userList.ToList();
			return user;
		}

	}
	public class Msg_code
	{
		public Msg_code(int _code = 0, string _Msg = "", object _data = null, List<U_Info> _uInfo =null)
		{
			code = _code;
			Msg = _Msg;
			data = _data;
			uInfo = _uInfo;
		}

		public int code = 0;
		public string Msg = "";
		public object data = null;
		public List<U_Info> uInfo = null;
	}
	public static class JsonExtension
	{
		public static string ToJson(this object jsonstr)
		{
			var result = JsonConvert.SerializeObject(jsonstr);
			return result;
		}
	}
}
