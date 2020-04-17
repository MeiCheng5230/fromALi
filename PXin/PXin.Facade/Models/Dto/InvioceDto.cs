using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class InvioceStatisticsDto
    {
        /// <summary>
        /// 可开票金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 已开票金额
        /// </summary>
        public decimal AlreadyAmount { get; set; }
        /// <summary>
        /// 审核状态 0=未填写 1=审核中 2=审核通过 3=审核拒绝
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InvioceBase
    {
        /// <summary>
        /// 主键值
        /// </summary>
        public int IdNo { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        public string ShowName { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNum { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class InvioceMayApplyDto: InvioceBase
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InvioceAlreadyApplyDto : InvioceBase
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int Infoid { get; set; }
        /// <summary>
        /// 类型 1=电子普通发票 2=增值税专用发票
        /// </summary>
        public int Typeid { get; set; }
        /// <summary>
        /// 抬头
        /// </summary>
        public string Head { get; set; }
        /// <summary>
        /// 税号
        /// </summary>
        public string TaxNum { get; set; }
        /// <summary>
        /// 状态 1=审核中 2=已开票 3=审核不通过
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string Expressno { get; set; }
        /// <summary>
        /// 是否个人 0=企业 1=个人
        /// </summary>
        public int IsPerson { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class InvioceQualificaDto
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
        /// <summary>
        ///  拒绝理由
        ///</summary>
        public string Note { get; set; }
        /// <summary>
        ///  审核状态 1=审核中 2=审核拒绝 3=审核通过
        ///</summary>
        public int Status { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InvioceDetailDto
    {
        /// <summary>
        ///  发票文件地址
        ///</summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InvioceQualificaAdminDto: InvioceQualificaDto
    {
        /// <summary>
        ///  申请人nodeid
        ///</summary>
        public int Nodeid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class QualificaAdminDto
    {
        /// <summary>
        /// 列表
        /// </summary>
        public List<InvioceQualificaAdminDto> List { get; set; }
        /// <summary>
        ///  总条数
        ///</summary>
        public int Num { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InvioceAlreadyApplyAdminDto: InvioceAlreadyApplyDto
    {
        /// <summary>
        ///  申请人的用户id
        ///</summary>
        public string NodeCode { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AlreadyApplyAdminDto
    {
        /// <summary>
        /// 列表
        /// </summary>
        public List<InvioceAlreadyApplyAdminDto> List { get; set; }
        /// <summary>
        ///  总条数
        ///</summary>
        public int Num { get; set; }
    }

}
