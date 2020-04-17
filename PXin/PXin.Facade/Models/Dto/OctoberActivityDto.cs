using Common.Facade.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class OctoberActivityCountReq : Reqbase
    {
        /// <summary>
        /// 活动id
        /// </summary>
        [Required]
        public int ActivityId { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class OctoberActivityListReq : Reqbase
    {
        /// <summary>
        /// 活动id
        /// </summary>
        [Required]
        public int ActivityId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OctoberActivityDosUEPrepareReq : Reqbase
    {
        /// <summary>
        /// 支付服务费
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 选择支付的数据Id,多个用下划线‘_’连接
        /// </summary>
        [Required]
        public string DataId { get; set; }

        /// <summary>
        /// 1已满足条件支付服务费,2领取手机支付服务费
        /// </summary>
        public int PayType { get; set; }
        /// <summary>
        /// 活动id
        /// </summary>
        [Required]
        public int ActivityId { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class GetExpressInfoReq : Reqbase
    {
        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNo { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OctoberActivityCountDto
    {
        /// <summary>
        /// 已满足条件支付服务费数量
        /// </summary>
        public int PayCount { get; set; }

        /// <summary>
        /// 领取手机数量
        /// </summary>
        public int ReceiveCount { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class OctoberActivityListDto
    {
        /// <summary>
        /// 已满足条件支付服务费列表
        /// </summary>
        public List<OctoberActivityPayDto> PayList { get; set; }

        /// <summary>
        /// 领取手机支付服务费列表
        /// </summary>
        public List<OctoberActivityPayDto> ReceiveList { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OctoberActivityPayDto
    {
        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  活动类型，1-代开充值商、2-代理人进货、3-零售SVC充值码并充值SV
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  对方的用户id
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  对方的用户账号
        ///</summary>
        public string Nodecode { get; set; }
        /// <summary>
        ///  对方的用户名称
        ///</summary>
        public string Nodename { get; set; }
        /// <summary>
        ///  用户看到的活动描述
        ///</summary>
        public string Note { get; set; }
        /// <summary>
        ///  支付金额
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  对方支付状态,0支付，1已支付
        ///</summary>
        public int PayStatus { get; set; }//Transferids

        /// <summary>
        ///  0-未支付，1-已支付，3-已发货，4-已退款
        ///</summary>
        public int MyStatus { get; set; }

        /// <summary>
        ///  物流单号
        ///</summary>
        public string Expressno { get; set; }

        /// <summary>
        /// 完成活动的时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///  领取手机用户转账ID，tnet_uepayhis.id
        ///</summary>
        [Newtonsoft.Json.JsonIgnore]
        public string Transferids { get; set; }

        /// <summary>
        ///  上级用户转账ID，tnet_uepayhis.id
        ///</summary>
        [Newtonsoft.Json.JsonIgnore]
        public string Ptransferids { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OctoberActivityDto2
    {
        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  活动类型，1-代开充值商、2-代理人进货、3-零售SVC充值码并充值SV
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  用户id
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  用户名称
        ///</summary>
        public string Nodename { get; set; }

        /// <summary>
        ///  上级用户名称
        ///</summary>
        public string PNodecode { get; set; }

        /// <summary>
        ///  上级用户id
        ///</summary>
        public int PNodeid { get; set; }
        /// <summary>
        ///  上级用户名称
        ///</summary>
        public string PNodename { get; set; }

        /// <summary>
        ///  用户名称
        ///</summary>
        public string Nodecode { get; set; }

        /// <summary>
        /// 手机
        ///</summary>
        public string Mobile { get; set; }
        /// <summary>
        ///  用户看到的活动描述
        ///</summary>
        public string Note { get; set; }

        /// <summary>
        ///  0-未支付，2-已支付，3-已发货，4-已退款
        ///</summary>
        public int Status { get; set; }

        /// <summary>
        ///  物流单号
        ///</summary>
        public string Expressno { get; set; }

        /// <summary>
        /// 完成活动的时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class VpxinOctoberActivityReq : Reqbase
    {
        /// <summary>
        /// 活动id
        /// </summary>
        [Required]
        public int ActivityId { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class NovemberActivityDosPayReq : Reqbase
    {
        /// <summary>
        /// 支付服务费
        /// </summary>
        [Required]
        public decimal Price { get; set; }
        /// <summary>
        /// 选择支付的数据Id,多个用下划线‘_’连接
        /// </summary>
        [Required]
        public string BusinessIdStr { get; set; }
        /// <summary>
        /// 1已满足条件支付服务费,2领取手机支付服务费
        /// </summary>
        [Required]
        public int PayType { get; set; }
        /// <summary>
        /// hisid 多个用下划线‘_’连接
        /// </summary>
        [Required]
        public string HisIdStr { get; set; }
        /// <summary>
        /// 活动id
        /// </summary>
        [Required]
        public int ActivityId { get; set; }
    }
    
    /// <summary>
    /// 是否绑定pcn帐号req
    /// </summary>
    public class HasBindActivityThirdpartyReq : Reqbase
    {
        /// <summary>
        /// 活动id
        /// </summary>
        [Required]
        public int ActivityId { get; set; }
    }
    /// <summary>
    /// 绑定pcn帐号req
    /// </summary>
    public class BindActivityThirdpartyReq : Reqbase
    {
        /// <summary>
        /// pcn帐号
        /// </summary>
        [Required]
        public string PcnAccount { get; set; }
        /// <summary>
        /// 活动id
        /// </summary>
        [Required]
        public int ActivityId { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class NovemberActivityCountDto
    {
        /// <summary>
        /// 已满足条件数量
        /// </summary>
        public int SatisfyCondiCount { get; set; }
        /// <summary>
        /// 已获得资格数量
        /// </summary>
        public int QualifyCount { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class VpxinOctoberActivityDto
    {
        /// <summary>
        /// 已满足条件列表
        /// </summary>
        public List<SatisfyCondiAndQualify> SatisfyCondiList { get; set; }
        /// <summary>
        /// 已获得资格列表
        /// </summary>
        public List<SatisfyCondiAndQualify> QualifyList { get; set; }

        /// <summary>
        /// 状态(0=未到缴费时间,1=缴费时间已过,2=缴费时间)
        /// </summary>
        public int Status { get; set; }
    }
    /// <summary>
    /// 已满足条件和已获得资格实体
    /// </summary>
    public class SatisfyCondiAndQualify
    {
        /// <summary>
        ///  PK
        ///</summary>
        public int HisId { get; set; }
        /// <summary>
        ///  PK
        ///</summary>
        public int Id
        {
            get
            {
                return TempId ?? 0;
            }
        }
        /// <summary>
        ///  活动类型，1-充值商进货、2-代理人进货、3-零售SVC充值码并充值SV
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  对方的用户id
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  对方的用户账号
        ///</summary>
        public string Nodecode { get; set; }
        /// <summary>
        ///  对方的用户名称
        ///</summary>
        public string Nodename { get; set; }
        /// <summary>
        ///  用户看到的活动描述
        ///</summary>
        public string Note { get; set; }
        /// <summary>
        ///  支付金额
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  对方支付状态,0未支付，1已支付
        ///</summary>
        public int PayStatus { get; set; }

        /// <summary>
        ///  0-未支付，1-已支付，3-已发货，4-已退款
        ///</summary>
        public int MyStatus
        {
            get
            {
                return Status ?? 0;
            }
        }

        /// <summary>
        /// 完成活动的时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return TempCreateTime ?? DateTime.Now;
            }
        }

        /// <summary>
        ///  领取手机用户转账ID，tnet_uepayhis.id
        ///</summary>
        [Newtonsoft.Json.JsonIgnore]
        public string Transferids { get; set; }

        /// <summary>
        ///  上级用户转账ID，tnet_uepayhis.id
        ///</summary>
        [Newtonsoft.Json.JsonIgnore]
        public string Ptransferids { get; set; }
        /// <summary>
        /// 0-未支付，1-部分支付，2-已支付，3-已发货，4-已退款
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public int? Status { get; set; }
        /// <summary>
        ///
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public int? TempId { get; set; }
        /// <summary>
        ///
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public DateTime? TempCreateTime { get; set; }
    }
    /// <summary>
    /// 十一月活动支付dto
    /// </summary>
    public class NovemberActivityDosPayDto
    {
        /// <summary>
        /// 续费信息字符串
        /// </summary>
        public string ChargeStr { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderNo { get; set; }
    }
    /// <summary>
    /// 获取活动列表dto
    /// </summary>
    public class ActivityDto
    {
        /// <summary>
        ///  主键值
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  活动名称
        ///</summary>
        public string ActivityName { get; set; }
        /// <summary>
        ///  活动开始时间
        ///</summary>
        public DateTime ActivityStarttime { get; set; }
        /// <summary>
        ///  活动结束时间
        ///</summary>
        public DateTime ActivityEndtime { get; set; }
        /// <summary>
        ///  缴费开始时间
        ///</summary>
        public DateTime PayStarttime { get; set; }
        /// <summary>
        ///  缴费结束时间
        ///</summary>
        public DateTime PayEndtime { get; set; }
        /// <summary>
        /// 活动状态（1=活动中，0=结束 ）
        /// </summary>
        public int Status
        {
            get
            {
                return DateTime.Now > PayEndtime ? 0 : 1;
            }
        }
        /// <summary>
        ///  封面图片
        ///</summary>
        public string Cover { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
    }
}
