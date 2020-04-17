using Newtonsoft.Json;
using PXin.Facade.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Dto
{
    /// <summary>
    /// 充值商配车情况Dto
    /// </summary>
    public class JxsPeiCheDto
    {
        /// <summary>
        /// 充值商标识
        /// </summary>
        public int Infoid { get; set; }
        /// <summary>
        /// 配车状态
        /// </summary>
        public int PeicheStatus { get; set; }
        /// <summary>
        /// 配车状态显示【配车状态 0=未配车 1=已配车 2=待回收 3=已回收】
        /// </summary>
        public string PeicheStatusShow { get; set; }
        /// <summary>
        /// 审批状态【0, '待审核', 1, '审核通过', 2, '审核拒绝'】
        /// </summary>
        public int ApprovalStatus { get; set; }
        /// <summary>
        /// 审批状态显示
        /// </summary>
        public string ApprovalStatusShow { get; set; }
        /// <summary>
        /// 冻结状态【0, '正常', 非零 , '冻结中'】
        /// </summary>
        public int FreezeStatus { get; set; }
        /// <summary>
        /// 冻结状态显示
        /// </summary>
        public string FreezeStatusShow { get; set; }
        /// <summary>
        /// 批发码总计
        /// </summary>
        public decimal PFM { get; set; }
        /// <summary>
        /// 回收SVC充值码总计
        /// </summary>
        public decimal SVC { get; set; }
    }


    /// <summary>
    /// (充值商/代理人)信息Dto
    /// </summary>
    public class UserJxsDto
    {
        /// <summary>
        /// 充值商/代理人(90天内累计进货/我的代理人/认证资料)的需参数
        /// </summary>
        public int InfoId { get; set; }
        /// <summary>
        /// 充值商/代理人名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Nodename { get; set; }
        /// <summary>
        /// 审核状态(0-未审核，1-审核通过，2-审核拒绝)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态2（3-冻结,4-锁定）
        /// </summary>
        public int StatusTwo { get; set; }
        /// <summary>
        /// 审核状态描述
        /// </summary>
        public string StatusDesc
        {
            //状态，Status = 0-未审核，1-审核通过，2-审核拒绝，
            // Status2 = 3 -冻结,4-锁定
            get
            {
                switch (Status + StatusTwo)
                {
                    case 0:
                        {
                            if (!IsUploaded)
                            {
                                return "未上传资料";
                            }
                            else
                            {
                                return "等待审核";
                            }
                        }
                    case 1:
                        return "审核通过";
                    case 2:
                        return "审核拒绝";
                    case 3:
                        return "冻结";
                    case 4:
                        return "冻结";
                    default:
                        return "审核拒绝";
                };
            }
        }
        /// <summary>
        /// 零售码库存
        /// </summary>
        public int RetailCodeStock { get; set; }
        /// <summary>
        /// 批发码库存
        /// </summary>
        public int WholesaleCodeStock { get; set; }
        /// <summary>
        /// 4=充值商, 5=代理人
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string RegionDesc
        {
            get
            {
                //1=总公司 2=省 3=市 4=区 5=经销商, <=4专营商
                switch (TypeId)
                {
                    case 1:
                        return "全部";
                    case 2:
                        return Province;
                    case 3:
                        return Province + "-" + City;
                    case 4:
                    case 5:
                        return Province + "-" + City + "-" + Region;
                    default:
                        return "未知";
                }
            }
        }
        /// <summary>
        /// 是否为充值商，是则显示我的充值商，反之则为我的代理人
        /// </summary>
        public bool IsCzs { get => TypeId <= 4; }
        /// <summary>
        /// 级别描述
        /// </summary>
        public string TypeIdDesc
        {
            get
            {
                //5=代理人, =4充值商
                switch (TypeId)
                {
                    case 4:
                        return "充值商";
                    case 5:
                        return "代理人";
                    default:
                        return "未知";
                }
            }
        }
        /// <summary>
        /// 帐号到期时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 90天内累计进货
        /// </summary>
        public decimal InputStock { get; set; }
        /// <summary>
        /// 钱包ID(我的奖励所需参数)
        /// </summary>
        public int PurseId { get; set; }
        /// <summary>
        /// 钱改变原因(我的奖励所需参数)
        /// </summary>
        public int Reason { get { return 33099; } }
        /// <summary>
        /// 90天内最少进货次数
        /// </summary>
        public int LeastStock { get => 3; }
        /// <summary>
        /// 是否显示进货
        /// </summary>
        public bool IsShowStock { get; set; }
        /// <summary>
        /// 是否显示续费
        /// </summary>
        public bool IsShowRenew { get; set; }
        /// <summary>
        /// 是否显示我的奖励
        /// </summary>
        public bool IsShowReward { get => TypeId <= 4 ? true : false; }
        /// <summary>
        /// 充值商/代理人是否过期
        /// </summary>
        public bool IsExpire { get; set; }
        /// <summary>
        /// 上级充值商/代理人是否过期
        /// </summary>
        public bool PIsExpire { get; set; }
        /// <summary>
        /// 充值商用户名
        /// </summary>
        public string PNodeName { get; set; }
        /// <summary>
        /// 专营商手机号
        /// </summary>
        public string PMobileno { get; set; }
        /// <summary>
        /// SV余额
        /// </summary>
        public decimal SVBalance { get; set; }
        /// <summary>
        /// sv生成充值码满足条件
        /// </summary>
        public bool IsGenerateCode { get; set; }
        /// <summary>
        /// 绑定UE状态(1:成功，-1：异常，-2：未绑定)
        /// </summary>
        public int BindUEStatus { get; set; }
        /// <summary>
        /// 当日sv还能生成充值码(SVC)数量
        /// </summary>
        public decimal TodayCount { get; set; }

        #region CommomProp
        /// <summary>
        /// 帐号到期时间
        /// </summary>
        [JsonIgnore]
        public DateTime TempEndTime { get; set; }

        /// <summary>
        /// 公司照片
        /// </summary>
        [JsonIgnore]
        public string PicCompany { get; set; }
        /// <summary>
        /// 合同照片,多张，用逗号隔开
        /// </summary>
        [JsonIgnore]
        public string PicContract { get; set; }
        /// <summary>
        /// 手持照片
        /// </summary>
        [JsonIgnore]
        public string PicHold { get; set; }
        /// <summary>
        /// 身份证反面照片
        /// </summary>
        [JsonIgnore]
        public string PicIdentback { get; set; }
        /// <summary>
        /// 营业执照
        /// </summary>
        [JsonIgnore]
        public string PicLicense { get; set; }
        /// <summary>
        /// 身份证正面照片
        /// </summary>
        [JsonIgnore]
        public string PicIdentfront { get; set; }
        /// <summary>
        /// 是否上传全部的认证资料
        /// </summary>
        [JsonIgnore]
        public bool IsUploaded
        {
            get
            {
                var result = !(string.IsNullOrEmpty(PicHold) || string.IsNullOrEmpty(PicIdentback) || string.IsNullOrEmpty(PicIdentfront));
                if (this.TypeId == 4)//充值商
                {
                    return result && !string.IsNullOrEmpty(PicLicense);
                }
                else
                {
                    return result;
                }
            }
        }
        /// <summary>
        /// 省
        /// </summary>
        [JsonIgnore]
        public string Province { get; set; }
        /// <summary>
        ///  市
        ///</summary>
        [JsonIgnore]
        public string City { get; set; }
        /// <summary>
        ///  区
        ///</summary>
        [JsonIgnore]
        public string Region { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        [JsonIgnore]
        public DateTime ChgTypeDate { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        [JsonIgnore]
        public DateTime? PChgTypeDate { get; set; }
        /// <summary>
        ///  nodeid
        ///</summary>
        [JsonIgnore]
        public int Nodeid { get; set; }

        #endregion
    }

    /// <summary>
    /// 修改(充值商/代理人)名称Dto
    /// </summary>
    public class UpdateUserJxsNameDto
    {
        /// <summary>
        /// 修改后的名称
        /// </summary>
        public string JxsName { get; set; }
    }
    /// <summary>
    /// 90天累积进货Dto
    /// </summary>
    public class PurchaseWith90DaysDto
    {
        /// <summary>
        /// 是否显示免考核信息
        /// </summary>
        public bool IsShowNoCheck { get; set; }
        /// <summary>
        /// 免考核时间
        /// </summary>
        public string NoCheckTime { get; set; }
        ///// <summary>
        ///// 目标时间（90天前的时间）
        ///// </summary>
        //public string TargetTime { get; set; }
        ///// <summary>
        ///// 目标时间内进货数量
        ///// </summary>
        //public string StockNum { get; set; }
        /// <summary>
        /// 目标时间内进货数量
        /// </summary>
        public string TargetTimeStockNum { get; set; }
        /// <summary>
        /// 90天内进货历史列表
        /// </summary>
        public List<StockhisWith90Days> StockHis { get; set; } = new List<StockhisWith90Days>();
    }
    /// <summary>
    /// 库存记录Dto
    /// </summary>
    public class StockRecordDto
    {
        /// <summary>
        /// 进货时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 进货数量
        /// </summary>
        public decimal Number { get; set; }
        /// <summary>
        /// 进货类型(1-零售码进货，2-批发码进货，3-零售码出货，4-批发码出货，5-零售，6-回收，7-代开充值商扣减零售码)
        /// </summary>
        public int TypeId { get; set; }
    }
    /// <summary>
    /// 库库记录信息
    /// </summary>
    public class StockRecordInfo
    {
        /// <summary>
        /// 进货时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Number { get; set; }
        /// <summary>
        /// 进货类型
        /// </summary>
        public int TypeId { get; set; }
    }
    /// <summary>
    /// 90天内进货历史
    /// </summary>
    public class StockhisWith90Days
    {
        /// <summary>
        /// 进货时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 进货数量
        /// </summary>
        public int StockNum { get; set; }
        /// <summary>
        /// 进货类型(1-零售码进货，2-批发码进货，3-零售码出货，4-批发码出货)
        /// </summary>
        public int Stocktype { get; set; }
    }
    /// <summary>
    /// 我的代理人Dto
    /// </summary>
    public class MyUserJxsDto
    {
        /// <summary>
        /// 代理人Id
        /// </summary>
        public int InfoId { get; set; }
        /// <summary>
        /// 代理人名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 审核状态描述
        /// </summary>
        public string StatusDesc
        {
            //状态，Status = 0-未审核，1-审核通过，2-审核拒绝，
            // Status2 = 3 -冻结,4-锁定
            get
            {
                switch (Status + StatusTwo)
                {
                    case 0:
                        {
                            if (!IsUploaded)
                            {
                                return "未上传资料";
                            }
                            else
                            {
                                return "等待审核";
                            }
                        }
                    case 1:
                        return "审核通过";
                    case 2:
                        return "审核拒绝";
                    case 3:
                        return "冻结";
                    case 4:
                        return "冻结";
                    default:
                        return "审核拒绝";
                };
            }
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        ///  开始时间
        ///</summary>
        public string StarttimeFormat { get => this.StartTime.ToString("yyyy-MM-dd"); }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        ///  结束
        ///</summary>
        public string EndtimeFormat { get => this.EndTime.ToString("yyyy-MM-dd"); }
        /// <summary>
        /// 审核状态(0-未审核，1-审核通过，2-审核拒绝)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态2（3-冻结,4-锁定）
        /// </summary>
        public int StatusTwo { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public int Counts { get; set; }

        #region CommonProp
        /// <summary>
        /// 公司照片
        /// </summary>
        [JsonIgnore]
        public string PicCompany { get; set; }
        /// <summary>
        /// 合同照片,多张，用逗号隔开
        /// </summary>
        [JsonIgnore]
        public string PicContract { get; set; }
        /// <summary>
        /// 手持照片
        /// </summary>
        [JsonIgnore]
        public string PicHold { get; set; }
        /// <summary>
        /// 身份证反面照片
        /// </summary>
        [JsonIgnore]
        public string PicIdentback { get; set; }
        /// <summary>
        /// 营业执照
        /// </summary>
        [JsonIgnore]
        public string PicLicense { get; set; }
        /// <summary>
        /// 身份证正面照片
        /// </summary>
        [JsonIgnore]
        public string PicIdentfront { get; set; }
        /// <summary>
        /// 是否上传全部的认证资料
        /// </summary>
        [JsonIgnore]
        public bool IsUploaded
        {
            get
            {
                return !(string.IsNullOrEmpty(PicHold) || string.IsNullOrEmpty(PicIdentback) || string.IsNullOrEmpty(PicIdentfront));
            }
        }
        #endregion
    }
    /// <summary>
    /// 我的充值商Dto
    /// </summary>
    public class MyUserCzsDto
    {
        /// <summary>
        /// 所属充值商
        /// </summary>
        public string ParentName { get; set; }
        /// <summary>
        /// 所属充值商的用户名称
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 所属充值商手机号码
        /// </summary>
        public string Mobileno { get; set; }
        /// <summary>
        /// 是否更改过充值商
        /// </summary>
        public bool HsChanged { get; set; }
    }
    /// <summary>
    /// 审核状态Dto
    /// </summary>
    public class AuditStatusDto
    {
        /// <summary>
        /// 审核状态描述
        /// </summary>
        public string StatusDesc
        {
            //状态，Status = 0-未审核，1-审核通过，2-审核拒绝，
            // Status2 = 3 -冻结,4-锁定
            get
            {
                switch (Status + StatusTwo)
                {
                    case 0:
                        {
                            if (!IsUploaded)
                            {
                                return "未上传资料";
                            }
                            else
                            {
                                return "等待审核";
                            }
                        }
                    case 1:
                        return "审核通过";
                    case 2:
                        return "审核拒绝";
                    case 3:
                        return "冻结";
                    case 4:
                        return "冻结";
                    default:
                        return "审核拒绝";
                };
            }
        }
        /// <summary>
        /// 补充资料审核状态
        /// </summary>
        public string SupplementStatusDesc
        {
            get
            {

                switch (StatusThree)
                {
                    case 0:
                        if (string.IsNullOrEmpty(PicExt))
                        {
                            return "未上传资料";
                        }
                        else
                        {
                            return "等待审核";
                        };
                    case 1:
                        return "审核通过";
                    case 2:
                        return "审核拒绝";
                    default:
                        return "审核拒绝";
                }
            }
        }
        /// <summary>
        /// 审核拒绝原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 类型(4=充值商，5=代理人)
        /// </summary>
        public int TypeId { get; set; }

        #region CommonProp
        /// <summary>
        /// 公司照片
        /// </summary>
        [JsonIgnore]
        public string PicCompany { get; set; }
        /// <summary>
        /// 合同照片,多张，用逗号隔开
        /// </summary>
        [JsonIgnore]
        public string PicContract { get; set; }
        /// <summary>
        /// 手持照片
        /// </summary>
        [JsonIgnore]
        public string PicHold { get; set; }
        /// <summary>
        /// 身份证反面照片
        /// </summary>
        [JsonIgnore]
        public string PicIdentback { get; set; }
        /// <summary>
        /// 营业执照
        /// </summary>
        [JsonIgnore]
        public string PicLicense { get; set; }
        /// <summary>
        /// 身份证正面照片
        /// </summary>
        [JsonIgnore]
        public string PicIdentfront { get; set; }
        /// <summary>
        /// 补充资料
        /// </summary>
        [JsonIgnore]
        public string PicExt { get; set; }
        /// <summary>
        /// 是否上传全部的认证资料
        /// </summary>
        [JsonIgnore]
        public bool IsUploaded
        {
            get
            {
                var result = !(string.IsNullOrEmpty(PicHold) || string.IsNullOrEmpty(PicIdentback) || string.IsNullOrEmpty(PicIdentfront));
                if (this.TypeId == 4)//充值商
                {
                    return result && !string.IsNullOrEmpty(PicLicense);
                }
                else
                {
                    return result;
                }
            }
        }
        /// <summary>
        /// 审核状态(0-未审核，1-审核通过，2-审核拒绝)
        /// </summary>
        [JsonIgnore]
        public int Status { get; set; }
        /// <summary>
        /// 状态2（3-冻结,4-锁定）
        /// </summary>
        [JsonIgnore]
        public int StatusTwo { get; set; }
        /// <summary>
        /// 补充资料状态
        /// </summary>
        [JsonIgnore]
        public int StatusThree { get; set; }

        #endregion
    }
    /// <summary>
    /// 上传认证资料Dto
    /// </summary>
    public class UploadAuthDataDto
    {
        /// <summary>
        /// 上传后的审核状态
        /// </summary>
        public string StatusDesc
        {
            get
            {
                if (!IsExt)
                {
                    //状态，Status = 0-未审核，1-审核通过，2-审核拒绝，
                    // Status2 = 3 -冻结,4-锁定
                    switch (Status + StatusTwo)
                    {
                        case 0:
                            {
                                if (!IsUploaded)
                                {
                                    return "未上传资料";
                                }
                                else
                                {
                                    return "等待审核";
                                }
                            }
                        case 1:
                            return "审核通过";
                        case 2:
                            return "审核拒绝";
                        case 3:
                            return "冻结";
                        case 4:
                            return "冻结";
                        default:
                            return "审核拒绝";
                    }
                }
                else
                {
                    switch (StatusThree)
                    {
                        case 0:
                            if (string.IsNullOrEmpty(PicExt))
                            {
                                return "未上传资料";
                            }
                            else
                            {
                                return "等待审核";
                            };
                        case 1:
                            return "审核通过";
                        case 2:
                            return "审核拒绝";
                        default:
                            return "审核拒绝";
                    }
                }
            }
        }

        #region CommonProp
        /// <summary>
        /// 类型
        /// </summary>
        [JsonIgnore]
        public int TypeId { get; set; }
        /// <summary>
        /// 公司照片
        /// </summary>
        [JsonIgnore]
        public string PicCompany { get; set; }
        /// <summary>
        /// 合同照片,多张，用逗号隔开
        /// </summary>
        [JsonIgnore]
        public string PicContract { get; set; }
        /// <summary>
        /// 手持照片
        /// </summary>
        [JsonIgnore]
        public string PicHold { get; set; }
        /// <summary>
        /// 身份证反面照片
        /// </summary>
        [JsonIgnore]
        public string PicIdentback { get; set; }
        /// <summary>
        /// 营业执照
        /// </summary>
        [JsonIgnore]
        public string PicLicense { get; set; }
        /// <summary>
        /// 身份证正面照片
        /// </summary>
        [JsonIgnore]
        public string PicIdentfront { get; set; }
        /// <summary>
        /// 补充资料
        /// </summary>
        [JsonIgnore]
        public string PicExt { get; set; }
        /// <summary>
        /// 是否上传全部的认证资料
        /// </summary>
        [JsonIgnore]
        public bool IsUploaded
        {
            get
            {
                var result = !(string.IsNullOrEmpty(PicHold) || string.IsNullOrEmpty(PicIdentback) || string.IsNullOrEmpty(PicIdentfront));
                if (this.TypeId == 4)//充值商
                {
                    return result && !string.IsNullOrEmpty(PicLicense);
                }
                else
                {
                    return result;
                }
            }
        }
        /// <summary>
        /// 审核状态(0-未审核，1-审核通过，2-审核拒绝)
        /// </summary>
        [JsonIgnore]
        public int Status { get; set; }
        /// <summary>
        /// 状态2（3-冻结,4-锁定）
        /// </summary>
        [JsonIgnore]
        public int StatusTwo { get; set; }
        /// <summary>
        /// 补充资料状态
        /// </summary>
        [JsonIgnore]
        public int StatusThree { get; set; }

        /// <summary>
        /// 是否为补充资料
        /// </summary>
        [JsonIgnore]
        public bool IsExt { get; set; }
        #endregion
    }
    /// <summary>
    ///获取认证资料Dto
    /// </summary>
    public class AuthDataDto
    {
        /// <summary>
        /// 审核状态描述
        /// </summary>
        public string StatusDesc
        {
            //状态，Status = 0-未审核，1-审核通过，2-审核拒绝，
            // Status2 = 3 -冻结,4-锁定
            get
            {
                switch (Status + StatusTwo)
                {
                    case 0:
                        {
                            if (!IsUploaded)
                            {
                                return "未上传资料";
                            }
                            else
                            {
                                return "等待审核";
                            }
                        }
                    case 1:
                        return "审核通过";
                    case 2:
                        return "审核拒绝";
                    case 3:
                        return "冻结";
                    case 4:
                        return "冻结";
                    default:
                        return "审核拒绝";
                };
            }
        }
        /// <summary>
        /// 补充资料审核状态
        /// </summary>
        public string SupplementStatusDesc
        {
            get
            {

                switch (StatusThree)
                {
                    case 0:
                        if (string.IsNullOrEmpty(PicExt))
                        {
                            return "未上传资料";
                        }
                        else
                        {
                            return "等待审核";
                        };
                    case 1:
                        return "审核通过";
                    case 2:
                        return "审核拒绝";
                    default:
                        return "审核拒绝";
                }
            }
        }
        /// <summary>
        /// 审核不通过原因
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 补充资料审核不通过原因
        /// </summary>
        public string SupplementRemark { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 资料url
        /// </summary>
        public List<ImageUrlInfo> ImageUrls { get; set; } = new List<ImageUrlInfo>();

        #region CommonProp
        /// <summary>
        /// 类型
        /// </summary>
        /// <summary>
        /// 公司照片
        /// </summary>
        [JsonIgnore]
        public string PicCompany { get; set; }
        /// <summary>
        /// 合同照片,多张，用逗号隔开
        /// </summary>
        [JsonIgnore]
        public string PicContract { get; set; }
        /// <summary>
        /// 手持照片
        /// </summary>
        [JsonIgnore]
        public string PicHold { get; set; }
        /// <summary>
        /// 身份证反面照片
        /// </summary>
        [JsonIgnore]
        public string PicIdentback { get; set; }
        /// <summary>
        /// 营业执照
        /// </summary>
        [JsonIgnore]
        public string PicLicense { get; set; }
        /// <summary>
        /// 身份证正面照片
        /// </summary>
        [JsonIgnore]
        public string PicIdentfront { get; set; }
        /// <summary>
        /// 补充资料
        /// </summary>
        [JsonIgnore]
        public string PicExt { get; set; }
        /// <summary>
        /// 是否上传全部的认证资料
        /// </summary>
        [JsonIgnore]
        public bool IsUploaded
        {
            get
            {
                var result = !(string.IsNullOrEmpty(PicHold) || string.IsNullOrEmpty(PicIdentback) || string.IsNullOrEmpty(PicIdentfront));
                if (this.TypeId == 4)//充值商
                {
                    return result && !string.IsNullOrEmpty(PicLicense);
                }
                else
                {
                    return result;
                }
            }
        }
        /// <summary>
        /// 审核状态(0-未审核，1-审核通过，2-审核拒绝)
        /// </summary>
        [JsonIgnore]
        public int Status { get; set; }
        /// <summary>
        /// 状态2（3-冻结,4-锁定）
        /// </summary>
        [JsonIgnore]
        public int StatusTwo { get; set; }
        /// <summary>
        /// 补充资料状态
        /// </summary>
        [JsonIgnore]
        public int StatusThree { get; set; }
        #endregion
    }
    /// <summary>
    /// 图片信息类
    /// </summary>
    public class ImageUrlInfo
    {
        /// <summary>
        /// url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 类型(1：身份证正面图片；2：身份证反面图片;3:手持身份证正面照；4：公司照片;5:营业执照；6：开户许可证;7:租赁合同)
        /// </summary>
        public ImageActionType ImageActionType { get; set; }
    }
    /// <summary>
    /// 获取续费信息Dto
    /// </summary>
    public class RenewInfoDto
    {
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobileno { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Nodename { get; set; }
        /// <summary>
        /// 当前级别
        /// </summary>
        public string TypeIdDesc
        {
            get
            {
                //4=充值商 5=代理人,
                switch (TypeId)
                {
                    case 4:
                        return "充值商";
                    case 5:
                        return "代理人";
                    default:
                        return "未知";
                }
            }
        }
        /// <summary>
        /// 到期时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 级别Id
        /// </summary>
        [JsonIgnore]
        public int TypeId { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public int Amount { get; set; }
    }
    /// <summary>
    /// 续费Dto
    /// </summary>
    public class RenewDto
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
    /// 查询用户Dto
    /// </summary>
    public class SearchUserDto
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int NodeId { get; set; }
        /// <summary>
        /// 帐号
        /// </summary>
        public string NodeCode { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobileno { get; set; }
    }
    /// <summary>
    /// 新增代理人Dto
    /// </summary>
    public class AddDealerDto
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
    /// 用户信息Dto
    /// </summary>
    public class FbApUserInfoDto
    {
        /// <summary>
        /// 等级，4-充值商 5-代理人
        /// </summary>
        public int Typeid { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 身份描述
        /// </summary>
        public string IdentityDesc { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string AppPhoto { get; set; }
        /// <summary>
        /// 是否绑定了优谷帐号
        /// </summary>
        public bool IsBind { get; set; }
        /// <summary>
        /// 优谷帐号
        /// </summary>
        public string Nodecode { get; set; }
        /// <summary>
        /// 是否有活动
        /// </summary>
        public bool IsActivity { get; set; }
    }
    /// <summary>
    /// 兑换类型信息dto
    /// </summary>
    public class ExchangeTypeInfoDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 副标题
        /// </summary>
        public string SubTitle { get; set; }
        /// <summary>
        /// 兑换规则Id(进货支付时所需参数,rule)
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 零售码数量
        /// </summary>
        public int RetailCodeNum { get; set; }
        /// <summary>
        /// 批发码数量
        /// </summary>
        public int WholesaleCodeNum { get; set; }
        /// <summary>
        /// codeDos
        /// </summary>
        public string Dos { get; set; }
        /// <summary>
        /// code信息
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 是否为促销活动
        /// </summary>
        public bool IsPromotion { get; set; }
    }
    /// <summary>
    /// 优惠点兑换充值码(进货)Dto
    /// </summary>
    public class ExChangeRechargeCodeDto
    {
        /// <summary>
        /// 支付信息字符串
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
    /// 我的奖励Dto
    /// </summary>
    public class UserPurseHisDto
    {
        /// <summary>
        /// 类型ID
        /// </summary>
        public int TypeID { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 交易后余额
        /// </summary>
        public string BalanceAfter
        {
            set
            {
            }
            get
            {
                return _balanceafter.ToString("0.00");
            }

        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 单位类型
        /// </summary>
        [JsonIgnore]
        public int CurrType { get; set; }
        /// <summary>
        /// 交易后余额
        /// </summary>
        [JsonIgnore]
        public decimal _balanceafter { get; set; }
    }
    /// <summary>
    /// 获取充值商/代理人图标Dto
    /// </summary>

    public class FbapInitPageDto
    {
        /// <summary>
        /// 1=普通用户重定向申请充值商页面，2=添加代理人请求页面，3=充值商首页
        /// </summary>
        public int Type { get; set; }
    }
    /// <summary>
    /// 充值商申请Dto
    /// </summary>
    public class ApplyFbapDto
    {
        /// <summary>
        /// 支付信息字符串
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
    /// 代开充值商Dto
    /// </summary>
    public class OpenCzsDto
    {
        /// <summary>
        /// 支付信息字符串
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
    /// 获取会议列表Dto
    /// </summary>
    public class MeetInfoDto
    {
        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  标题
        ///</summary>
        public string Title { get; set; }
        /// <summary>
        ///  开始时间,精准到年月日时分秒
        ///</summary>
        public DateTime Starttime { get; set; }
        /// <summary>
        ///  地址
        ///</summary>
        public string Address { get; set; }
        /// <summary>
        ///  详情URL完整地址
        ///</summary>
        public string Detail { get; set; }
        /// <summary>
        /// 状态(-1=已过期，0=未报名，1=已报名)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态描述
        /// </summary>
        public string StatusDesc
        {
            get
            {
                switch (this.Status)
                {
                    case -1: return "已过期";
                    case 0: return "去报名";
                    case 1: return "已报名";
                    default:
                        return "已过期";
                }
            }
        }
    }
    /// <summary>
    /// 获取会议详情Dto
    /// </summary>
    public class MeetInfoDetailDto
    {
        /// <summary>
        ///  标题
        ///</summary>
        public string Title { get; set; }
        /// <summary>
        ///  开始时间,精准到年月日时分秒
        ///</summary>
        public DateTime Starttime { get; set; }
        /// <summary>
        ///  地址
        ///</summary>
        public string Address { get; set; }
        /// <summary>
        ///  详情URL完整地址
        ///</summary>
        public string Detail { get; set; }
        /// <summary>
        /// 状态(-1=已过期，0=未报名，1=已报名)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态描述
        /// </summary>
        public string StatusDesc
        {
            get
            {
                switch (this.Status)
                {
                    case -1: return "已过期";
                    case 0: return "去报名";
                    case 1: return "已报名";
                    default:
                        return "已过期";
                }
            }
        }
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
        public List<JoinMeetingPersonDto> JoinMeetingPersons { get; set; } = new List<JoinMeetingPersonDto>();
    }
    /// <summary>
    /// 参会人
    /// </summary>
    public class JoinMeetingPersonDto
    {
        /// <summary>
        ///  参会人
        ///</summary>
        public string JoinPersonName { get; set; }
        /// <summary>
        ///  参会人手机号码
        ///</summary>
        public string JoinPersonMobileno { get; set; }
    }
    /// <summary>
    /// 查询充值商信息Dto
    /// </summary>
    public class FbapInfoDto
    {
        /// <summary>
        /// 充值商名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 充值商用户名称
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 充值商手机号码
        /// </summary>
        public string Mobileno { get; set; }
    }
    /// <summary>
    /// 查询充值商添加代理人请求列表Dto
    /// </summary>
    public class UserJxsConfirmsDto
    {
        /// <summary>
        /// 同意的充值商nodeid
        /// </summary>
        public int CzsNodeid { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nodename { get; set; }
        /// <summary>
        /// 充值商名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string AppPhoto { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ThridPartyPayDto
    {
        /// <summary>
        /// 转账记录id
        /// </summary>
        public int TransferId { get; set; }
    }
}
