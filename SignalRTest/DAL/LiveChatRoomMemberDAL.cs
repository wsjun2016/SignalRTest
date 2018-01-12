using log4net;
using MyProject.Entity;
using SignalRTest.DAL.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRTest.DAL {
    public class LiveChatRoomMemberDAL {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlSugar.SqlSugarClient db = null;

        public LiveChatRoomMemberDAL() {
            db = SugarDao.GetInstance();
        }

        public long Add(LiveChatRoomMember model) {
            long rc = 0;

            try {
                if (model != null)
                    rc = db.Insertable(model).ExecuteReturnBigIdentity();
            }
            catch (Exception ex) {
                log.Error("Add", ex);
            }

            return rc;
        }

        public List<LiveChatRoomMember> FindList(System.Linq.Expressions.Expression<Func<LiveChatRoomMember, bool>> where) {
            List<LiveChatRoomMember> rc = null;

            try {
                rc = db.Queryable<LiveChatRoomMember>().WhereIF(where != null, where).ToList();
            }
            catch (Exception ex) {
                log.Error("FindList", ex);
            }

            return rc;
        }

        public LiveChatRoomMember Find(System.Linq.Expressions.Expression<Func<LiveChatRoomMember, bool>> where) {
            LiveChatRoomMember rc = null;

            try {
                if (where != null)
                    rc = db.Queryable<LiveChatRoomMember>().First(where);
            }
            catch (Exception ex) {
                log.Error("Find", ex);
            }

            return rc;
        }

        public bool Delete(long id) {
            bool rc = false;

            try {
                if (id > 0)
                    rc = db.Deleteable<LiveChatRoomMember>().Where(it => it.ID == id).ExecuteCommand() > 0;
            }
            catch (Exception ex) {
                log.Error("Delete",ex);
            }

            return rc;
        }
    }
}