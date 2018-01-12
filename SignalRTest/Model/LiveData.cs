using SqlSugar;
using System;
using System.Linq;
using System.Text;

namespace MyProject.Entity
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("LiveData")]
    public partial class LiveData
    {
           public LiveData(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int ID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Title {get;set;}

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string FrontCover { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int ViewCount {get;set;}

    }
}
