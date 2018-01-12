using log4net;
using MyProject.Entity;
using SignalRTest.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRTest.BLL {
    public class LiveChatRoomMemberBLL {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private LiveChatRoomMemberDAL dal = null;

        public LiveChatRoomMemberBLL() {
            dal = new LiveChatRoomMemberDAL();
        }

        public long Add(LiveChatRoomMember model) {
            return dal.Add(model);
        }

        /// <summary>
        /// 根据条件查询出列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<LiveChatRoomMember> FindList(System.Linq.Expressions.Expression<Func<LiveChatRoomMember, bool>> where) {
            return dal.FindList(where);
        }

        /// <summary>
        /// 根据条件查询出单条数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public LiveChatRoomMember Find(System.Linq.Expressions.Expression<Func<LiveChatRoomMember, bool>> where) {
            return dal.Find(where);
        }

        public bool Delete(long id) {
            return dal.Delete(id);
        }
    }
}