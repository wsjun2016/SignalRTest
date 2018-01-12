using log4net;
using MyProject.Entity;
using SignalRTest.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignalRTest {
    public partial class wfrmOne : System.Web.UI.Page {
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public int sid = Convert.ToInt32(RequestData("sid"));
        public LiveData _model = new LiveData();
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                if (sid > 0) {
                    _model = new LiveDataBLL().Find(it => it.ID == sid)??new LiveData();
                }
            }
        }





        public static string RequestData(string param) {
            string rc = "";

            try {
                if (!string.IsNullOrWhiteSpace(param))
                    rc = HttpContext.Current.Request[param];
            }
            catch (Exception ex) {
                log.Error("RequestData", ex);
            }

            return rc;
        }

    }    
}