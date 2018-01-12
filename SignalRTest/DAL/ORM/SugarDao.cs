using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRTest.DAL.ORM {
    public class SugarDao {
        private SugarDao() { }

        private static readonly string ConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connection");

        /// <summary>
        /// 新建一个SqlSugarClient实例对象
        /// </summary>
        public static SqlSugar.SqlSugarClient Instance {
            get {
                return new SqlSugar.SqlSugarClient(new SqlSugar.ConnectionConfig
                {
                    ConnectionString = ConnectionString,
                    DbType = SqlSugar.DbType.SqlServer,
                    IsAutoCloseConnection = true,
                    InitKeyType = SqlSugar.InitKeyType.SystemTable
                });
            }
        }

        /// <summary>
        /// 新建一个带日志记录的SqlSugarClient
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="log"></param>
        /// <returns></returns>
        //public static SqlSugar.SqlSugarClient GetInstance<T>(T log) where T : ILog {

        //    SqlSugar.SqlSugarClient db = new SqlSugar.SqlSugarClient(new SqlSugar.ConnectionConfig
        //    {
        //        ConnectionString = ConnectionString,
        //        DbType = SqlSugar.DbType.SqlServer,
        //        IsAutoCloseConnection = true,
        //        InitKeyType = SqlSugar.InitKeyType.SystemTable
        //    });

        //    db.Ado.IsEnableLogEvent = true;
        //    db.Ado.LogEventStarting = (sql, pars) => {
        //        //log.InfoFormat("{0} \r\n {1}", sql, pars);
        //    };

        //    return db;
        //}

        /// <summary>
        /// 新建一个不带日志记录的SqlSugarClient
        /// </summary>
        /// <returns></returns>
        public static SqlSugar.SqlSugarClient GetInstance() {
            return Instance;
        }
    }
}