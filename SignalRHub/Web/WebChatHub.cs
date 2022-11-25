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
		/// 用户登陆 上线用户绑定id号  返回唯一标识id
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
				code = new Msg_code(200, "登陆成功", data);
				await Clients.Client(Context.ConnectionId).SendAsync("ReturnConId", code.ToJson());

				var receiveMessage = "ReceiveMessage" + group;
				code = new Msg_code(201, "用户：" +username + "，加入聊天室", data);
				await Clients.All.SendAsync(receiveMessage, code.ToJson());

				return;

			}
			catch (Exception ex)
			{
				code = new Msg_code(500, "登陆失败");
				await Clients.Client(Context.ConnectionId).SendAsync("ReturnConId", code.ToJson());
				return;
			}


		}

		/// <summary>
		/// 发送消息--发送给所有连接的客户端
		/// </summary>
		/// <param name="msg">消息内容</param>
		/// <returns></returns>
		public Task SendAllMessage(string msg)
		{
			Msg_code code = new Msg_code();
			var user = getUserinfo();
			if (user == null)
			{
				code = new Msg_code(500, "登陆已失效,请重新登陆");
				return Clients.Client(Context.ConnectionId).SendAsync("ReturnConId", code.ToJson());
			}
			msg = pbstr(msg);
			var data = new
			{
				ConnectionId = Context.ConnectionId,
				UserId = user.LoginId,
				Name = user.LoginName,
				Msg = msg
			};
			code = new Msg_code(200, "发送成功", data);
			return Clients.All.SendAsync("ReceiveMessage", user.LoginName, msg);
		}


		//发送消息--发送特定组发信息
		public Task SendGroupMessage(string group, string msg)
		{
			Msg_code code = new Msg_code();
			try
			{

				if (string.IsNullOrWhiteSpace(msg))
				{
					code = new Msg_code(500, "消息不能为空");
					return Clients.Client(Context.ConnectionId).SendAsync("ReturnConId", code.ToJson());
				}

				var user = getUserinfo();
				if (user == null)
				{
					code = new Msg_code(500, "登陆已失效,请重新登陆");
					return Clients.Client(Context.ConnectionId).SendAsync("ReturnConId", code.ToJson());
				}
				msg = pbstr(msg);
				var data = new
				{
					ConnectionId = Context.ConnectionId,
					UserId = user.LoginId,
					Name = user.LoginName,
					group = group,
					Msg = msg
				};
				code = new Msg_code(200, "发送成功", data);
				return Clients.All.SendAsync("ReceiveMessage" + group, code.ToJson());

			}
			catch (Exception)
			{
				code = new Msg_code(500, "发送失败");
				return Clients.Client(Context.ConnectionId).SendAsync("ReturnConId", code.ToJson());
				//throw;
			}


		}

		/// <summary>
		/// 给指定用户发送信息
		/// </summary>
		/// <param name="user">conid</param>
		/// <param name="msg">内容</param>
		/// <returns></returns>
		public Task SendToMessage(string conid, string msg)
		{


			Msg_code code = new Msg_code();
			try
			{

				if (string.IsNullOrWhiteSpace(msg))
				{
					code = new Msg_code(500, "消息不能为空");
					return Clients.Client(Context.ConnectionId).SendAsync("ReturnConId", code.ToJson());
				}

				var user = getUserinfo();
				if (user == null)
				{
					code = new Msg_code(500, "登陆已失效,请重新登陆");
					return Clients.Client(Context.ConnectionId).SendAsync("ReturnConId", code.ToJson());
				}
				msg = pbstr(msg);
				var data = new
				{
					ConnectionId = Context.ConnectionId,
					UserId = user.LoginId,
					Name = user.LoginName,
					Msg = msg
				};
				code = new Msg_code(200, "发送成功", data);
				return Clients.All.SendAsync("ReceiveMsg", code.ToJson());

			}
			catch (Exception)
			{
				code = new Msg_code(500, "发送失败");
				return Clients.Client(conid).SendAsync("ReturnConId", code.ToJson());
				//throw;
			}
		}


		/// <summary>
		/// 给登陆id用户发送信息
		/// </summary>
		/// <param name="user">conid</param>
		/// <param name="msg">内容</param>
		/// <returns></returns>
		public async Task SendLoginMessage(string LoginId, string msg)
		{
			Msg_code code = new Msg_code();
			var userlist = Chat_User.userList.Where(t => t.LoginId == LoginId);
			var user = getUserinfo();
			if (user == null)
			{
				code = new Msg_code(500, "登陆已失效,请重新登陆");
				await Clients.Client(Context.ConnectionId).SendAsync("ReturnConId", code.ToJson());
				return;
			}
			msg = pbstr(msg);
			var data = new
			{
				ConnectionId = Context.ConnectionId,
				UserId = user.LoginId,
				Name = user.LoginName,
				Msg = msg
			};
			code = new Msg_code(200, "发送成功", data);
			foreach (var item in userlist)
			{
				await Clients.Client(item.Connectionid).SendAsync("ReceiveMsg", code.ToJson());
			}
		}

		public Task RemoveLogin(string ConnectionId)
		{
			Chat_User.RemoveUser(ConnectionId);
			return Clients.All.SendAsync("Closed", ConnectionId);
		}

		//重写上线监控
		public override async Task OnConnectedAsync()
		{
			var ConnectionId = Context.ConnectionId;

			Msg_code code = new Msg_code(202, ConnectionId + "链接成功", ConnectionId);
			await Clients.All.SendAsync("SystemNotice", code.ToJson());
		}


		//重写下线监控
		public override async Task OnDisconnectedAsync(Exception? exception)
		{

			var ConnectionId = Context.ConnectionId;

			var user = getUserinfo();
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

		}


		private string pbstr(string str)
		{
			var isbll = mgstr.Where(t => str.IndexOf(t) >= 0).ToList();
			if (isbll.Count > 0)
			{
				foreach (var item in isbll)
				{
					str = str.Replace(item, "**");
				}
			}
			return str;

		}


		private U_Info getUserinfo()
		{
			var user = Chat_User.userList.Where(t => t.Connectionid == Context.ConnectionId).FirstOrDefault();
			return user;
		}
	}
	public class Msg_code
	{
		public Msg_code(int _code = 0, string _Msg = "", object _data = null)
		{
			code = _code;
			Msg = _Msg;
			data = _data;
		}

		public int code = 0;
		public string Msg = "";
		public object data = null;
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
