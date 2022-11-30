using Microsoft.Data.Sqlite;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SignalRHub
{
	/// <summary>
	/// 在线用户静态缓存
	/// </summary>
	public static class Chat_User
	{
		/// <summary>
		/// 添加用户
		/// </summary>
		/// <param name="loginId"></param>
		/// <param name="connectionid"></param>
		/// <param name="name"></param>
		/// <param name="group"></param>
		public static void AddUser(string loginId, string connectionid, string name, string group)
		{
			StringBuilder builder = new StringBuilder();
			List<SqliteParameter> param = new List<SqliteParameter>();
			var userList = new SqliteHelper().GetList(@"SELECT * FROM base_onlineinfo;");
			var list = userList.Where(t => t.Connectionid == connectionid).ToList();
			if (list != null && list.Count > 0)
			{
				for (int i = 0; i < userList.Count; i++)
				{
					if (userList[i].Connectionid == connectionid)
					{
						builder.Append($" UPDATE base_onlineinfo SET LoginId =$LoginId,LoginName =$LoginName,ConnectionId=$ConnectionId,[group] =$group1 ");
						param.Add(new SqliteParameter("$LoginId", loginId));
						param.Add(new SqliteParameter("$ConnectionId", connectionid));
						param.Add(new SqliteParameter("$LoginName", name));
						param.Add(new SqliteParameter("$group1", group));
						var result = SqliteHelper.ExecuteNonQuery(builder.ToString(), param.ToArray());
					}
				}
			}
			else
			{
				builder.Append($" INSERT INTO base_onlineinfo (LoginId,LoginName,ConnectionId,[group]) VALUES  ($LoginId,$LoginName,$ConnectionId,$group1) ");
				param.Add(new SqliteParameter("$LoginId", loginId));
				param.Add(new SqliteParameter("$ConnectionId", connectionid));
				param.Add(new SqliteParameter("$LoginName", name));
				param.Add(new SqliteParameter("$group1", group));
				var result=  SqliteHelper.ExecuteNonQuery(builder.ToString(), param.ToArray());
			}
		}
		/// <summary>
		/// 移除用户
		/// </summary>
		/// <param name="connectionid">SignalRHub的链接Id</param>
		public static void RemoveUser(string connectionid)
		{
			var userList = new SqliteHelper().GetList(@"SELECT * FROM base_onlineinfo;");
			var list = userList.Where(t => t.Connectionid == connectionid).ToList();
			if (list != null && list.Count > 0)
			{
				StringBuilder builder = new StringBuilder();
				List<SqliteParameter> param = new List<SqliteParameter>();
				builder.Append($" DELETE FROM base_onlineinfo WHERE ConnectionId =$ConnectionId ");
				param.Add(new SqliteParameter("$ConnectionId", connectionid));
				SqliteHelper.ExecuteNonQuery(builder.ToString(), param.ToArray());
			}
		}

		/// <summary>
		///获取在线人数
		/// </summary>
		public static List<U_Info> GetUserList()
		{
			var userList = new SqliteHelper().GetList(@"SELECT * FROM base_onlineinfo;");
			return userList;
		}
	}

	/// <summary>
	/// 用户缓存
	/// </summary>
	public class U_Info
	{
		public Int64 OnlineId { get; set; }
		public string LoginId { get; set; }
		public string LoginName { get; set; }
		public string Connectionid { get; set; }
		public string group { get; set; }
	}

	/// <summary>
	/// 用户缓存
	/// </summary>
	public class ResInfo
	{
		public Int64 OnlineId { get; set; }
		public string LoginId { get; set; }
		public string LoginName { get; set; }
		public string Connectionid { get; set; }
		public string group { get; set; }

		public string LoginCompose { get; set; }
		public string ConnectionCompose { get; set; }
	}
}
