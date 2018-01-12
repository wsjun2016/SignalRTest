using log4net;
using MyProject.Entity;
using SignalRTest.DAL.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRTest.DAL {
    public class LiveChatRoomDAL {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlSugar.SqlSugarClient db = null;

        public LiveChatRoomDAL() {
            db = SugarDao.GetInstance();
        }

        public long Add(LiveChatRoom model) {
            long rc = 0;

            try {
                if (model != null)
                    rc = db.Insertable(model).ExecuteReturnBigIdentity();
            }
            catch (Exception ex) {
                log.Error("Add",ex);
            }

            return rc;
        }

        public List<LiveChatRoom> FindList(System.Linq.Expressions.Expression<Func<LiveChatRoom, bool>> where) {
            List<LiveChatRoom> rc = null;

            try {
                rc = db.Queryable<LiveChatRoom>().WhereIF(where != null, where).ToList();
            }
            catch (Exception ex) {
                log.Error("FindList", ex);
            }

            return rc;
        }

        public LiveChatRoom Find(System.Linq.Expressions.Expression<Func<LiveChatRoom, bool>> where) {
            LiveChatRoom rc = null;

            try {
                if (where != null)
                    rc = db.Queryable<LiveChatRoom>().First(where);
            }
            catch (Exception ex) {
                log.Error("Find", ex);
            }

            return rc;
        }
    }
}