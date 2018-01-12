using log4net;
using MyProject.Entity;
using SignalRTest.DAL.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRTest.DAL {
    public class UserInfoDAL {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlSugar.SqlSugarClient db = null;

        public UserInfoDAL() {
            db = SugarDao.GetInstance();
        }

        public List<UserInfo> FindList(System.Linq.Expressions.Expression<Func<UserInfo, bool>> where) {
            List<UserInfo> rc = null;

            try {
                rc = db.Queryable<UserInfo>().WhereIF(where != null, where).ToList();
            }
            catch (Exception ex) {
                log.Error("FindList", ex);
            }

            return rc;
        }

        public UserInfo Find(System.Linq.Expressions.Expression<Func<UserInfo, bool>> where) {
            UserInfo rc = null;

            try {
                if (where != null)
                    rc = db.Queryable<UserInfo>().First(where);
            }
            catch (Exception ex) {
                log.Error("Find", ex);
            }

            return rc;
        }
    }
}