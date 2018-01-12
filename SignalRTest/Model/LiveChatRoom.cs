using SqlSugar;
using System;
using System.Linq;
using System.Text;

namespace MyProject.Entity {
    ///<summary>
    ///
    ///</summary>
    [SugarTable("LiveChatRoom")]
    public partial class LiveChatRoom
    {
           public LiveChatRoom(){


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
           public int LiveID {get;set;}

        /// <summary>
        /// Desc:1:开放 0:关闭
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int Status { get; set; }

    }
}
