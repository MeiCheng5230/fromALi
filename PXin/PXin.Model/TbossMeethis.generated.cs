using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 会议报名历史表
    /// </summary>
    public partial class TbossMeethis
    {
        public TbossMeethis()
        { 
Hisid = 0;                                         
Infoid = 0;                                         
Nodeid = 0;                                         
Mobileno = null;                                         
Num = 1;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  tboss_meetinfo.infoid
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  报名人NODEID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  手机号码
        ///</summary>
        public string Mobileno { get; set; }
        /// <summary>
        ///  邀约人数,大于等于1
        ///</summary>
        public int Num { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  姓名
        ///</summary>
        public string Name { get; set; }
        /// <summary>
        ///  参会人员
        ///</summary>
        public string JoinPersons { get; set; }
    
        
    }
}