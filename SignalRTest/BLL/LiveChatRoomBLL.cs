using log4net;
using MyProject.Entity;
using SignalRTest.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRTest.BLL {
    public class LiveChatRoomBLL {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private LiveChatRoomDAL dal = null;

        public long Add(LiveChatRoom model) {
            return dal.Add(model);
        }

        public LiveChatRoomBLL() {
            dal = new LiveChatRoomDAL();
        }

        /// <summary>
        /// 根据条件查询出列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<LiveChatRoom> FindList(System.Linq.Expressions.Expression<Func<LiveChatRoom, bool>> where) {
            return dal.FindList(where);
        }

        /// <summary>
        /// 根据条件查询出单条数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public LiveChatRoom Find(System.Linq.Expressions.Expression<Func<LiveChatRoom, bool>> where) {
            return dal.Find(where);
        }
    }
}