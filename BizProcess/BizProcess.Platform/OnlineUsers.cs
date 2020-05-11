using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizProcess.Platform
{
    public class OnlineUsers
    {
        /// <summary>
        /// 缓存键
        /// </summary>
        private string key = BizProcess.Utility.Keys.CacheKeys.OnlineUsers.ToString();

        /// <summary>
        /// 得到所有在线用户表
        /// </summary>
        /// <returns></returns>
        public List<BizProcess.Data.Model.OnlineUsers> GetAll()
        {
            object obj = BizProcess.Cache.IO.Opation.Get(key);
            return obj != null && obj is List<BizProcess.Data.Model.OnlineUsers> ? obj as List<BizProcess.Data.Model.OnlineUsers> : new List<BizProcess.Data.Model.OnlineUsers>();
        }

        private void set(List<BizProcess.Data.Model.OnlineUsers> list)
        {
            if (list == null) return;
            BizProcess.Cache.IO.Opation.Set(key, list);
        }

        /// <summary>
        /// 添加一个用户到在线用户表
        /// </summary>
        public bool Add(BizProcess.Data.Model.Users user, Guid uniqueID)
        {
            if (user == null) return false;
            var onList = GetAll();
            bool isadd = false;
            var onUser = onList.Find(p => p.ID == user.ID);
            if (onUser == null)
            {
                isadd = true;
                onUser = new BizProcess.Data.Model.OnlineUsers();
                var station = new UsersRelation().GetMainByUserID(user.ID);
                if (station != null)
                {
                    onUser.OrgName = new Organize().GetAllParentNames(station.OrganizeID);
                }
            }
            onUser.ID = user.ID;
            onUser.ClientInfo = string.Concat("操作系统：", BizProcess.Utility.Tools.GetOSName(), "  浏览器：", BizProcess.Utility.Tools.GetBrowse());
            onUser.IP = BizProcess.Utility.Tools.GetIPAddress();
            onUser.LastPage = "";
            onUser.LoginTime = BizProcess.Utility.DateTimeNew.Now;
            onUser.UniqueID = uniqueID;
            onUser.UserName = user.Name;
            if (isadd)
            {
                onList.Add(onUser);
            }
            set(onList);
            return true;
        }

        /// <summary>
        /// 删除一个在线用户
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool Remove(Guid userID)
        {
            var list = GetAll();
            var user = list.Find(p => p.ID == userID);
            if (user != null)
            {
                list.Remove(user);
            }
            set(list);
            return true;
        }

        /// <summary>
        /// 清除所有在线用户
        /// </summary>
        /// <returns></returns>
        public bool RemoveAll()
        {
            var list = new List<BizProcess.Data.Model.OnlineUsers>();
            BizProcess.Cache.IO.Opation.Set(key, list);
            return true;
        }

        /// <summary>
        /// 查询一个在线用户实体
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public BizProcess.Data.Model.OnlineUsers Get(Guid userID)
        {
            var list = GetAll();
            return list.Find(p => p.ID == userID);
        }

    }
}
