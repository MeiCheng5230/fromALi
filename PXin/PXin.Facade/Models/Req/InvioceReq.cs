using Common.Facade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Req
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplyInvioceQualificaReq:Reqbase
    {
        /// <summary>
        ///  公司名
        ///</summary>
        public string Company { get; set; }
        /// <summary>
        ///  税号
        ///</summary>
        public string Taxnum { get; set; }
        /// <summary>
        ///  公司地址
        ///</summary>
        public string Address { get; set; }
        /// <summary>
        ///  公司电话
        ///</summary>
        public string Mobile { get; set; }
        /// <summary>
        ///  开户银行
        ///</summary>
        public string Bank { get; set; }
        /// <summary>
        ///  银行卡号
        ///</summary>
        public string Cardno { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ApplyWriteInvioceReq:Reqbase
    {
        /// <summary>
        ///  充值码卡id（逗号分割）
        ///</summary>
        public string IdNo { get; set; }
        /// <summary>
        ///  1=电子普通发票 2=增值税专用发票
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  是否个人  1=个人 0=企业
        ///</summary>
        public int Isperson { get; set; }
        /// <summary>
        ///  抬头,个人为抬头,企业为企业名
        ///</summary>
        public string Head { get; set; }
        /// <summary>
        ///  企业识别码
        ///</summary>
        public string Code { get; set; }
        /// <summary>
        ///  地址id，Typeid=2时填写
        ///</summary>
        public int Address { get; set; }
        /// <summary>
        ///  邮箱地址，Typeid=1时填写
        ///</summary>
        public string Email { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SendEmailReq : Reqbase
    {
        /// <summary>
        ///  邮箱地址
        ///</summary>
        public string Email { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InvioceDetailReq : Reqbase
    {
        /// <summary>
        ///  主键id
        ///</summary>
        public int ID { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class VerifyInvioceReq
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 是否通过审核 0=否 1=是
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 拒绝原因
        /// </summary>
        public string Note { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VerifyWriteInvioceReq
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 是否通过审核 0=否 1=是
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNo { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetInvioceQualificaListReq
    {
        /// <summary>
        /// 申请状态 -1=全部 1=申请中 2=已通过 3=审核拒绝
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageNum { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetWriteInvioceListReq: GetInvioceQualificaListReq
    {
        /// <summary>
        /// 票据类型 1=电子发票 2=增值发票
        /// </summary>
        public int Typeid { get; set; }
        /// <summary>
        /// 用户类型 0=企业 1=个人
        /// </summary>
        public int IsPerson { get; set; }
    }


}
