using log4net;
using MyProject.Entity;
using SignalRTest.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignalRTest {
    public partial class TLogin : System.Web.UI.Page {
        public static string USERINFO = "LIVEUSERINFO";        
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e) {
            
        }

        private bool LoginValid(string loginId, string pwd) {
            bool rc = false;

            try {
                UserInfo user = new UserInfoBLL().Find(it => it.LoginID == loginId);
                if (user != null && user.UserID > 0 && pwd=="1") {
                    // FormsAuthentication
                    //保存到Session中
                    string json= LitJson.JsonMapper.ToJson(new { Name = user.UserName, Pic = user.HeadPic });
                    
                    Response.Cookies.Add(new HttpCookie(USERINFO, HttpUtility.UrlEncode(json)));
                    Session[USERINFO] = user;
                    rc = Session[USERINFO]!=null;
                }
            }
            catch (Exception ex) {
                log.Error("LoginValid",ex);
            }

            return rc;
        }

        protected void btnSubmit_Click(object sender, EventArgs e) {
            string loginId = tbxUserName.Text.Trim();
            string pwd = tbxPassword.Text.Trim();

            if (LoginValid(loginId, pwd)) {
                lblMsg.Visible = false;
                lblMsg.Text = "";
                Response.Redirect("LiveDataList.aspx");
            }
            else {
                lblMsg.Visible = true;
                lblMsg.Text = "用户名或密码错误！";
            }
        }
    }
}