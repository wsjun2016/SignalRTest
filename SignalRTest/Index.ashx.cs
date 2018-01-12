using LitJson;
using log4net;
using Microsoft.AspNet.SignalR;
using MyProject.Entity;
using SignalRTest.Base;
using SignalRTest.BLL;
using SignalRTest.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Code = SignalRTest.Base.ErrorCode;

namespace SignalRTest {
    /// <summary>
    /// Index 的摘要说明
    /// </summary>
    public class Index : IHttpHandler,IRequiresSessionState {
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static UserInfo User {
            get {
                var obj= HttpContext.Current.Session[TLogin.USERINFO];
                return obj == null ? null : (obj as UserInfo);
            }
        }
        public void ProcessRequest(HttpContext context) {
            BaseResponseResult rc = new BaseResponseResult((int)Code.OperationError, "操作失败！");

            try {
                //获取接口     
                string action = GetAction();

                //操作请求数据
                switch (action) {                    
                    case "joinroom"://将用户ID与ConnectionID关联，并加入到聊天房间中
                        rc = new JoinRoomHandler().Process();
                        break;
                    default:
                        rc.SetResult((int)Code.NoAction, "无此接口！");
                        break;
                }
            }
            catch (Exception ex) {
                log.Error("直播操作接口出现异常.", ex);
                rc.SetResult((int)Code.SystemError, "系统错误！");
            }

            rc.Response();
        }

        /// <summary>
        /// 获取Action接口名称
        /// </summary>
        /// <returns></returns>
        private string GetAction() {
            string rc = "";

            try {
                Stream stream = HttpContext.Current.Request.InputStream;
                if (stream != null) {
                    byte[] buffer = new byte[stream.Length];
                    int count = stream.Read(buffer, 0, buffer.Length);
                    if (count > 0) {
                        string requestParam = Encoding.UTF8.GetString(buffer);
                        JsonData jd = JsonMapper.ToObject(requestParam);
                        if (jd != null && jd.Keys.Contains("action"))
                            rc = jd["action"].ToString();
                    }
                }
            }
            catch (Exception ex) {
                log.Error("GetAction", ex);
            }

            return rc;
        }

        public bool IsReusable {
            get {
                return false;
            }
        }
    }

    public class JoinRoomReq:BaseClass {
        [Required(ErrorMessage ="请输入直播ID")]        
        public int liveId { get; set; }
        [Required(ErrorMessage = "请输入当前的ConnectionID")]
        public string connectionId { get; set; }
    }

    public class JoinRoomHandler : LiveHandler<JoinRoomReq> {
        public JoinRoomHandler() : base("JoinRoomHandler") { }

        protected override BaseResponseResult DoWork(JoinRoomReq param) {
            BaseResponseResult rc = new BaseResponseResult((int)Code.OperationError, "操作失败！");

            if (Index.User != null) {
                if (param.liveId > 0) {
                    //找到当前直播对应的房间号
                    LiveChatRoom room = new LiveChatRoomBLL().Find(it => it.LiveID == param.liveId && it.Status == 1);
                    //如果房间存在
                    if (room != null && room.ID > 0) {
                        //将ConnectionID与UserID绑定到当前直播的房间中
                        LiveChatRoomMember member = new LiveChatRoomMember
                        {
                            ConnectionID = param.connectionId,
                            RoomID = room.ID,
                            UserID = Index.User.UserID
                        };

                        member.ID = new LiveChatRoomMemberBLL().Add(member);
                        //如果当前登陆的人员 成功 加入到直播聊天室
                        if (member.ID > 0) {
                            //这里的代码很重要，这是在外部调用GroupChatHub
                            var context = GlobalHost.ConnectionManager.GetHubContext<GroupChatHub>();
                            //将当前的ConnectionID加入到 以房间ID为名称的组中
                            context.Groups.Add(param.connectionId, room.ID.ToString());
                            //向客户端发送新加入人员信息
                            context.Clients.Group(room.ID.ToString()).publishMsg(GroupChatHub.FormatMsg("系统消息", Index.User.UserName + "  加入聊天", 0,Index.User.HeadPic));
                            rc.SetResult(0,"成功加入聊天室！");
                        }
                        else
                            rc.SetResult(3, "加入聊天房间失败！");
                    }
                    else
                        rc.SetResult(1, "当前聊天房间不存在！");
                }
                else
                    rc.SetResult(1, "当前聊天房间不存在！");
            }
            else
                rc.SetResult(2,"未登录！");

            return rc;
        }
    }
}