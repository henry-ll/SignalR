using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRHub
{
    /// <summary>
    /// 在线用户静态缓存
    /// </summary>
    public static class Chat_User
    {
        public static List<U_Info> userList = new List<U_Info>();
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="connectionid"></param>
        /// <param name="name"></param>
        /// <param name="group"></param>
        public static void AddUser(string loginId, string connectionid, string name, string group)
        {
            var list = userList.Where(t => t.Connectionid == connectionid).ToList();
            if (list!=null && list.Count>0)
            {
                for (int i = 0; i < userList.Count; i++)
                {
                    if (userList[i].Connectionid == connectionid)
                    {
                        userList[i].LoginId = loginId;
                        userList[i].LoginName = name;
                        userList[i].LoginName = group;
                    }
                }
            }
            else
                userList.Add(new U_Info(loginId, connectionid, name, group));
        }
		/// <summary>
		/// 移除用户
		/// </summary>
		/// <param name="connectionid">SignalRHub的链接Id</param>
		public static void RemoveUser(string connectionid)
        {
            var list = userList.Where(t => t.Connectionid == connectionid);
            if (list.Count() > 0)
            {
                for (int i = 0; i < userList.Count; i++)
                {
                    if (userList[i].Connectionid == connectionid)
                        userList.RemoveAt(i);
                }
            }
        }
    }

    /// <summary>
    /// 用户缓存
    /// </summary>
    public class U_Info
    {
        public U_Info(string id, string cid, string loginName, string _group)
        {
            LoginId = id;
            Connectionid = cid;
            LoginName = loginName;
            group = _group;
        }
        public string LoginId { get; set; }
        public string LoginName { get; set; }
        public string Connectionid { get; set; }
        public string group { get; set; }
    }
}
