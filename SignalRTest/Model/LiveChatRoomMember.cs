using SqlSugar;
using System;
using System.Linq;
using System.Text;

namespace MyProject.Entity
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("LiveChatRoomMember")]
    public partial class LiveChatRoomMember:ModelContext
    {
           public LiveChatRoomMember(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public long ID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public long RoomID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string ConnectionID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? UserID {get;set;}

            [SugarColumn(IsIgnore =true)]
            public UserInfo User {
            get {
                return this.ID > 0 && this.UserID.HasValue && this.UserID.Value > 0
                    ? base.CreateMapping<UserInfo>().Where(it => it.UserID == this.UserID.Value).First()
                    : null;
            }
        }

    }
}
