using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 经销商名单/充值商+代理人
    /// </summary>
    public partial class TblUserJxs
    {
        public TblUserJxs()
        {
            Infoid = 0;
            Nodeid = 0;
            Note = null;
            Opnodeid = 0;
            Typeid = 5;
            Sid = 81123;
            Status = 0;
            Stocknum = 0;
            Istrain = 1;
            Status2 = 0;
            Znhy = 0;
            Status3 = 0;
            LicenseStatus = 0;
            Znhyprice = 1;
            Nochecktime = DateTime.Now;
            Isfirst = 1;
            Starttime = DateTime.Now;
            Endtime = DateTime.Now.AddDays(365);
            Chgtypedate = DateTime.Now;
            Stocknum2 = 0;
        }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  父ID,自关联
        ///</summary>
        public int? Pinfoid { get; set; }
        /// <summary>
        ///  用户ID,唯一
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  省
        ///</summary>
        public string Province { get; set; }
        /// <summary>
        ///  市
        ///</summary>
        public string City { get; set; }
        /// <summary>
        ///  区
        ///</summary>
        public string Region { get; set; }
        /// <summary>
        ///  对外的描述
        ///</summary>
        public string Note { get; set; }
        /// <summary>
        ///  操作人
        ///</summary>
        public int Opnodeid { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  5=代理人, 4=充值商
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  appSTOREID
        ///</summary>
        public int Sid { get; set; }
        /// <summary>
        ///  状态，0-未审核，1-审核通过，2-审核拒绝
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  最后进货时间
        ///</summary>
        public DateTime? Lastdate { get; set; }
        /// <summary>
        ///  经销/专营商名称
        ///</summary>
        public string Jsxname { get; set; }
        /// <summary>
        ///  合同照片,多张，用逗号隔开
        ///</summary>
        public string PicContract { get; set; }
        /// <summary>
        ///  公司照片
        ///</summary>
        public string PicCompany { get; set; }
        /// <summary>
        ///  专营商本人照片
        ///</summary>
        public string PicHold { get; set; }
        /// <summary>
        ///  身份证正面照片
        ///</summary>
        public string PicIdentfront { get; set; }
        /// <summary>
        ///  身份证反面照片
        ///</summary>
        public string PicIdentback { get; set; }
        /// <summary>
        ///  营业执照
        ///</summary>
        public string PicLicense { get; set; }
        /// <summary>
        ///  专营商有效，批发码库存，相信APP SVC批发库存
        ///</summary>
        public int Stocknum { get; set; }
        /// <summary>
        ///  经销商有效，是否参与培训，1-参与[专营商=0时，进货50万50个码，经销商20万10个码]
        ///</summary>
        public int Istrain { get; set; }
        /// <summary>
        ///  状态2，3-冻结,4-锁定
        ///</summary>
        public int Status2 { get; set; }
        /// <summary>
        ///  智能合约股份总额
        ///</summary>
        public int Znhy { get; set; }
        /// <summary>
        ///  补充资料照片,多张,用逗号隔开
        ///</summary>
        public string PicExt { get; set; }
        /// <summary>
        ///  补充资料状态，0-默认，1-审核通过，2-审核拒绝
        ///</summary>
        public int Status3 { get; set; }
        /// <summary>
        ///  补充资料审核备注
        ///</summary>
        public string Remarks3 { get; set; }
        /// <summary>
        ///  营业执照状态，0-未识别，1-识别成功
        ///</summary>
        public int LicenseStatus { get; set; }
        /// <summary>
        ///  智能合约每股价值
        ///</summary>
        public decimal Znhyprice { get; set; }
        /// <summary>
        ///  开户许可证
        ///</summary>
        public string PicPermit { get; set; }
        /// <summary>
        ///  租赁合同
        ///</summary>
        public string PicLease { get; set; }
        /// <summary>
        ///  免考核期
        ///</summary>
        public DateTime Nochecktime { get; set; }
        /// <summary>
        ///  是否首次进货
        ///</summary>
        public int Isfirst { get; set; }
        /// <summary>
        ///  开始时间
        ///</summary>
        public DateTime Starttime { get; set; }
        /// <summary>
        ///  结束
        ///</summary>
        public DateTime Endtime { get; set; }
        /// <summary>
        ///  转换类型日期
        ///</summary>
        public DateTime Chgtypedate { get; set; }
        /// <summary>
        ///  相信APP SVC零售码库存
        ///</summary>
        public int Stocknum2 { get; set; }


    }
}