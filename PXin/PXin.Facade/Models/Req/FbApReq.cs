using Common.Facade.Models;
using PXin.Facade.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Req
{

    /// <summary>
    /// 修改(充值商/代理人)名称Req
    /// </summary>
    public class UpdateUserJxsNameReq : Reqbase
    {
        /// <summary>
        /// 充值商/代理人id
        /// </summary>
        public int InfoId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string JxsName { get; set; }
    }
    /// <summary>
    /// 90天累积进货Req
    /// </summary>
    public class PurchaseWith90DaysReq : Reqbase
    {
        /// <summary>
        /// 充值商/代理人id
        /// </summary>
        public int InfoId { get; set; }
    }
    /// <summary>
    /// 库存记录Req
    /// </summary>
    public class StockRecordReq : Reqbase
    {
        /// <summary>
        /// 类型(1=SV零售库存记录,2=SV批发库存记录)
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 充值商/代理人id
        /// </summary>
        public int InfoId { get; set; }
    }

    /// <summary>
    /// 我的代理人Req
    /// </summary>
    public class MyUserJxsReq : Reqbase
    {
        /// <summary>
        /// 代理人id
        /// </summary>
        public int InfoId { get; set; }
    }
    /// <summary>
    /// 我的充值商Req
    /// </summary>
    public class MyUserCzsReq : Reqbase
    {
        /// <summary>
        /// 代理人id
        /// </summary>
        public int InfoId { get; set; }
    }
    /// <summary>
    /// 获取审核状态Req
    /// </summary>
    public class AuditStatusReq : Reqbase
    {
        /// <summary>
        /// 充值商/代理人id
        /// </summary>
        public int InfoId { get; set; }
    }
    /// <summary>
    /// 上传认证资料Req
    /// </summary>
    public class UploadAuthDataReq : Reqbase
    {
        /// <summary>
        /// 认证资料集合
        /// </summary>
        [Required]
        public List<UploadAuthData> AuthDatas { get; set; }
    }
    /// <summary>
    /// 认证资料集
    /// </summary>
    public class UploadAuthData
    {
        /// <summary>
        /// 认证资料类型(1=个人资料,2=公司资料,3=合同照片,4=补充资料)
        /// </summary>
        [Required]
        public AuthDataType AuthDataType { get; set; }
        /// <summary>
        /// 上传文件集合
        /// </summary>
        [Required]
        public List<UploadFile> UploadFiles { get; set; }
    }
    /// <summary>
    /// 上传文件请求
    /// </summary>
    public class UploadFile
    {
        /// <summary>
        /// 文件的的相对路径
        /// </summary>
        [Required]
        public string Url { get; set; }
        /// <summary>
        /// 图片作用类型
        /// (1：身份证正面图片；2：身份证反面图片;3:手持身份证正面照；4：公司照片;5:营业执照；6：开户许可证;7:租赁合同;8:合同;9:补充资料)
        /// </summary>
        public ImageActionType ImageActionType { get; set; }
    }
    /// <summary>
    /// 审核代理人资料Req
    /// </summary>
    public class AuditJxsInfoReq : Reqbase
    {
        /// <summary>
        /// 代理人ID
        /// </summary>
        [Required]
        public int InfoId { get; set; }
        /// <summary>
        /// 审核状态(1：通过，2：拒绝)
        /// </summary>
        [Required]
        public int Status { get; set; }
        /// <summary>
        /// 拒绝原因
        /// </summary>
        public string Remarks { get; set; }
    }
    /// <summary>
    /// 获取认证资料Req
    /// </summary>
    public class AuthDataReq : Reqbase
    {
        /// <summary>
        /// 认证资料类型(1=个人资料,2=公司资料,3=合同照片,4=补充资料)
        /// </summary>
        public AuthDataType AuthDataType { get; set; }

        /// <summary>
        /// 充值商/代理人id
        /// </summary>
        public int InfoId { get; set; }
    }
    /// <summary>
    /// 获取续费信息Req
    /// </summary>
    public class RenewInfoReq : Reqbase
    {
        /// <summary>
        /// 充值商/代理人Id
        /// </summary>
        public int InfoId { get; set; }
    }
    /// <summary>
    /// 续费Req
    /// </summary>
    public class RenewReq : Reqbase
    {
        /// <summary>
        /// 充值商/代理人Id
        /// </summary>
        public int InfoId { get; set; }
    }
    /// <summary>
    /// 库存余额Req
    /// </summary>
    public class StockBalanceReq : Reqbase
    {
        /// <summary>
        /// 1=dos库存,2=sv库存 
        /// </summary>
        public int Type { get; set; } = 1;
    }
    /// <summary>
    /// 用户查询Req
    /// </summary>
    public class SearchUserReq : Reqbase
    {
        /// <summary>
        /// 帐号或手机号
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 类型:1-开通代理人，2-普通用户申请充值商，3-代开充值商，4=代理人申请充值商
        /// </summary>
        public int Type { get; set; } = 1;
    }
    /// <summary>
    /// 新增代理人Req
    /// </summary>
    public class AddDealerReq : Reqbase
    {
        /// <summary>
        /// 新增经销商的NodeId
        /// </summary>
        public int NewNodeId { get; set; }
    }
    /// <summary>
    /// 用户信息Req
    /// </summary>
    public class UserInfoReq : Reqbase
    {
        /// <summary>
        /// 充值商/代理人Id
        /// </summary>
        [Required]
        public int InfoId { get; set; }
    }
    /// <summary>
    /// 获取兑换类型信息req
    /// </summary>
    public class ExchangeTypeInfoReq : Reqbase
    {
        /// <summary>
        /// 充值商/代理人Id
        /// </summary>
        [Required]
        public int InfoId { get; set; }
    }
    /// <summary>
    /// 优惠点兑换充值码(进货)Req
    /// </summary>
    public class ExChangeRechargeCodeReq : Reqbase
    {
        /// <summary>
        /// 规则Id
        /// </summary>
        [Required]
        public int RuleId { get; set; }
        /// <summary>
        /// 是否参与促销活动
        /// </summary>
        public bool IsPromotion { get; set; }
        /// <summary>
        /// 优谷账号
        /// </summary>
        public string Nodecode { get; set; }
    }
    /// <summary>
    /// 我的奖励Req
    /// </summary>
    public class UserPurseHisReq : Reqbase
    {
        /// <summary>
        /// 钱包Id
        /// </summary>
        [Required]
        public int PurseId { get; set; }
    }

    /// <summary>
    /// 获取充值商转向初始页面的值Req
    /// </summary>
    public class FbapInitPageReq : Reqbase
    {
    }
    /// <summary>
    /// 验证密码（支付或登录）请求参数
    /// </summary>
    public class VerifyPwdReq : Reqbase
    {
        /// <summary>
        /// 优谷账号
        /// </summary>
        [Required]
        public string Nodecode { get; set; }
        /// <summary>
        /// 密码类型 1-登录密码，2-支付密码
        /// </summary>
        [Required]
        [Range(1, 2)]
        public int Typeid { get; set; }
        /// <summary>
        /// 密码（BASE64加密）
        /// </summary>
        public string Pwd { get; set; }
    }
    /// <summary>
    /// 充值商申请
    /// </summary>
    public class ApplyFbapReq : Reqbase
    {
        /// <summary>
        /// 省
        /// </summary>
        [Required]
        public string Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        [Required]
        public string City { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        [Required]
        public string Region { get; set; }
    }
    /// <summary>
    /// 获取会议列表Req
    /// </summary>
    public class MeetInfoReq : Reqbase
    {
        /// <summary>
        /// 类型(1=未过期，2=全部)
        /// </summary>
        [Required]
        public int Type { get; set; }
    }
    /// <summary>
    /// 获取会议详情Req
    /// </summary>
    public class MeetInfoDetailReq : Reqbase
    {
        /// <summary>
        /// 会议ID
        /// </summary>
        [Required]
        public int Infoid { get; set; }
    }
    /// <summary>
    /// 参加会议
    /// </summary>
    public class JoinMeetingReq : Reqbase
    {
        /// <summary>
        /// 会议ID
        /// </summary>
        [Required]
        public int Infoid { get; set; }
        /// <summary>
        ///  姓名
        ///</summary>
        public string Name { get; set; }
        /// <summary>
        ///  手机号码
        ///</summary>
        public string Mobileno { get; set; }
        /// <summary>
        /// 参会人列表
        /// </summary>
        public List<JoinMeetingPersonReq> JoinMeetingPersons { get; set; }

    }
    /// <summary>
    /// 参会人
    /// </summary>
    public class JoinMeetingPersonReq
    {
        /// <summary>
        ///  参会人
        ///</summary>
        [Required]
        public string JoinPersonName { get; set; }
        /// <summary>
        ///  参会人手机号码
        ///</summary>
        [Required]
        public string JoinPersonMobileno { get; set; }
    }
    /// <summary>
    /// 查询充值商信息Req
    /// </summary>
    public class FbapInfoReq : Reqbase
    {
        /// <summary>
        /// 帐号或手机号
        /// </summary>
        public string Key { get; set; }
    }
    /// <summary>
    /// 更换充值商Req
    /// </summary>
    public class ChangeFbapReq : Reqbase
    {
        /// <summary>
        /// 代理人ID
        /// </summary>
        [Required]
        public int Infoid { get; set; }
        /// <summary>
        /// 充值商帐号或手机号
        /// </summary>
        public string Key { get; set; }
    }
    /// <summary>
    /// 开通充值商Req
    /// </summary>
    public class OpenCzsReq : Reqbase
    {
        /// <summary>
        /// 开通充值商商的NodeId
        /// </summary>
        [Required]
        public int NewNodeId { get; set; }
        ///// <summary>
        ///// 支付密码 
        ///// </summary>
        //[Required]
        //public string PayPassword { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        [Required]
        public string Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        [Required]
        public string City { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        [Required]
        public string Region { get; set; }
    }
    /// <summary>
    /// 检查开通充值商
    /// </summary>
    public class CheckOpenCzsReq : Reqbase
    {

    }
    /// <summary>
    /// 查询充值商添加代理人请求列表Req
    /// </summary>
    public class UserJxsConfirmsReq : Reqbase
    {

    }
    /// <summary>
    /// 同意充值商添加代理人请求
    /// </summary>
    public class UserJxsRequstReq : Reqbase
    {
        /// <summary>
        /// 同意的充值商nodeid
        /// </summary>
        public int CzsNodeid { get; set; }
    }
}
