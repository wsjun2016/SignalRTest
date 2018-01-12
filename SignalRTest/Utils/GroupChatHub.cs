using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using MyProject.Entity;
using SignalRTest.BLL;

namespace SignalRTest.Utils {
    [HubName("groupChatHub")]
    public class GroupChatHub:Hub {     
        [HubMethodName("joinRoom")]
        //创建聊天的房间
        public void JoinRoom(string liveId) {
            try {    
                //查询出该房间是否开放
                LiveChatRoomBLL roomBiz = new LiveChatRoomBLL();
                LiveChatRoom room = roomBiz.Find(it => it.LiveID == Convert.ToInt32(liveId) && it.Status == 1);
                //如果房间为空，则创建该聊天房间
                if (room == null) {
                    room = new LiveChatRoom { LiveID = Convert.ToInt32(liveId), Status = 1 };
                    room.ID = roomBiz.Add(room);
                }

                if (room != null && room.ID > 0) {
                    //将ConnectionID发送给自己
                    Clients.Client(Context.ConnectionId).intoRoom(Context.ConnectionId);
                }                
            }
            catch (Exception ex) {

            }
        }

        /// <summary>
        /// 在线用户
        /// </summary>
        private static Dictionary<string, int> _onlineUser = new Dictionary<string, int>();

        public override Task OnConnected() {           
            return base.OnConnected();
        }

        public override Task OnReconnected() {           
            return base.OnReconnected();
        }      

        public override Task OnDisconnected(bool stopCalled) {   
             LiveChatRoomMemberBLL biz = new LiveChatRoomMemberBLL();
            //根据ConnectionID找到当前的聊天室信息
            LiveChatRoomMember member = biz.Find(it => it.ConnectionID == Context.ConnectionId);
            if (member != null && member.ID > 0) {
                //从该房间清除该人员
                if (biz.Delete(member.ID)) {
                    //发送退出消息
                    Clients.Groups(new List<string> { member.RoomID.ToString() }).publishMsg(FormatMsg("系统消息", member.User.UserName + "  退出聊天", 0));
                    //从组中移除该ConnectionID
                    Groups.Remove(Context.ConnectionId, member.RoomID.ToString());
                }
            }
            
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// 发送消息，供客户端调用
        /// </summary>
        /// <param name="user">用户名，如果为0，则是发送给所有人</param>
        /// <param name="msg">消息</param>
        public void SendMsg(string user,string msg) {
            //通过ConnectionID找到当前聊天室的信息
            LiveChatRoomMember member = new LiveChatRoomMemberBLL().Find(it => it.ConnectionID == Context.ConnectionId);
            if (member != null && member.ID > 0) {
                //向当前聊天室发送消息
                Clients.Groups(new List<string> { member.RoomID.ToString() }).publishMsg(FormatMsg(member.User.UserName, msg, 1, member.User.HeadPic));
            }          
        }

        

        //type 0:系统消息 1:用户消息
        public static dynamic FormatMsg(string name, string msg, int type=1,string pic="") {
            return new {IType=type, Name=name,Msg=HttpUtility.HtmlEncode(msg),Pic= HttpUtility.HtmlEncode(pic), Time=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")};
        }
    }
}