using SqlSugar;
using System;
using System.Linq;
using System.Text;

namespace MyProject.Entity
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("UserInfo")]
    public partial class UserInfo
    {
           public UserInfo(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int UserID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string UserName {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string HeadPic {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string LoginID {get;set;}

    }
}
