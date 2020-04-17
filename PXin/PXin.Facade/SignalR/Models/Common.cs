using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.SignalR.Models
{
    public enum PXinCommandType : uint
    {
        Login = 0x00000001,                         //  登陆
        LoginResp = 0x80000001,                     //  登陆应答
        Logout = 0x00000002,                        //	退出
        LogoutResp = 0x80000002,                    //	退出应答
        Active = 0x00000003,                        //	链路测试
        ActiveResp = 0x80000003,                    //	链路测试应答
        ChatFee = 0x00000004,                       //	聊天计费
        ChatFeeResp = 0x80000004,                   //	聊天计费应答
        ChatFeePush = 0x00000005,                   //	聊天计费推送
        ChatFeePushResp = 0x80000005,               //	聊天计费推送应答
        ChatFeeRateSet = 0x00000006,                //	聊天计费倍率设置
        ChatFeeRateSetResp = 0x80000006,            //	聊天计费倍率设置应答
        ChatFeeRateQuery = 0x00000007,              //	聊天计费倍率查询
        ChatFeeRateQueryResp = 0x80000007,          //	聊天计费倍率查询应答
        ChatFeeRateSetPush = 0x00000008,            //  聊天计费倍率设置推送
        ChatFeeRateSetPushResp = 0x80000008,        //  聊天计费倍率设置推送回复
    }
    public class Util
    {
        private static object sync = new object();
        private static long nSquence = 0;
        public static long GetSquence()
        {
            lock (sync)
            {
                long temp = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss") + "00");
                nSquence++;
                if (nSquence > 100)
                {
                    nSquence = 0;
                }
                return temp + nSquence;
            }
        }
    }
}
