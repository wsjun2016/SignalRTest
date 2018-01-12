using log4net;
using MyProject.Entity;
using SignalRTest.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRTest.BLL {
    public class LiveDataBLL {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private LiveDataDAL dal = null;

        public LiveDataBLL() {
            dal = new LiveDataDAL();
        }

        /// <summary>
        /// 根据条件查询出列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<LiveData> FindList(System.Linq.Expressions.Expression<Func<LiveData, bool>> where) {
            return dal.FindList(where);
        }

        /// <summary>
        /// 根据条件查询出单条数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public LiveData Find(System.Linq.Expressions.Expression<Func<LiveData, bool>> where) {
            return dal.Find(where);
        }
    }
}