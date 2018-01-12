using log4net;
using MyProject.Entity;
using SignalRTest.DAL.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRTest.DAL {
    public class LiveDataDAL {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlSugar.SqlSugarClient db = null;

        public LiveDataDAL() {
            db = SugarDao.GetInstance();
        }

        public List<LiveData> FindList(System.Linq.Expressions.Expression<Func<LiveData, bool>> where) {
            List<LiveData> rc = null;

            try {
                rc = db.Queryable<LiveData>().WhereIF(where!=null,where).ToList();
            }
            catch (Exception ex) {
                log.Error("FindList",ex);
            }

            return rc;
        }

        public LiveData Find(System.Linq.Expressions.Expression<Func<LiveData, bool>> where) {
            LiveData rc = null;

            try {
                if(where!=null)
                    rc = db.Queryable<LiveData>().First(where);
            }
            catch (Exception ex) {
                log.Error("Find", ex);
            }

            return rc;
        }
    }
}