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
    public class ThirdPartyVerifyDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Storename { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Logo { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ThridPayDto
    {
        /// <summary>
        /// 第三方订单号, 返回状态Result=1成功,-1=传入参数不正确,-2=secretkey不存在,-3=验证字符串错误,-4=用户不存在,-5=支付密码不正确,-6=订单号已存在,-7=转账失败,-8=写支付历史失败
        /// </summary>
        public string Orderno { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderPcn { get; set; }
    }

    /// <summary>
    /// 第三方支付
    /// </summary>
    public class ThridTransferPayReq
    {
        /// <summary>
        /// 
        /// </summary>
        public string Nodecode { get; set; }

        /// <summary>
        /// 金额（元）
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 时间(yyyy-MM-dd HH:mm:ss)
        /// </summary>
        public string ReqTime { get; set; }

        /// <summary>
        /// 加密
        /// </summary>
        public string Sign { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ThridTransferPayDto
    {
        /// <summary>
        /// 转账记录id
        /// </summary>
        public int TransferId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetThridPayhisDto
    {
        /// <summary>
        /// 订单id
        /// </summary>
        public int orderpcn { get; set; }

        /// <summary>
        /// 支付类型:1-P币充值
        /// </summary>
        public int paytype { get; set; }

        /// <summary>
        /// 消费金额(元),精确到小数点后2位
        /// </summary>
        public decimal amount { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string orderno { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime createtime { get; set; }

        /// <summary>
        /// 支付状态,0为未付款,1为付款成功,2为付款失败 ,3关闭的订单 5已退款，6部分退款
        /// </summary>
        public int paystatus { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetThridPayTypeDto
    {
        /// <summary>
        /// 1-P币，2-CV
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TypeName { get; set; }
    }
}
