using MyProject.Entity;
using SignalRTest.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignalRTest {
    public partial class LiveDataList : System.Web.UI.Page {
        public List<LiveData> _list = new List<LiveData>();
        protected void Page_Load(object sender, EventArgs e) {
            if (Session[TLogin.USERINFO] == null)
                Response.Redirect("TLogin.aspx");
            if (!IsPostBack) {
                Init();
            }
        }

        private void Init() {
            _list = new LiveDataBLL().FindList(null);
        }
    }
}