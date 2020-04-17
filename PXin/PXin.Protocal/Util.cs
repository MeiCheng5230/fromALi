using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXin.Protocal
{
    public enum PXin_COMMAND_TYPE : uint
    {
        Login = 0x00000001,	                        //  登陆
        LoginResp = 0x80000001,	                    //  登陆应答
        Logout = 0x00000002,	                    //	退出
        LogoutResp = 0x80000002,	                //	退出应答
        Active = 0x00000003,	                    //	链路测试
        ActiveResp = 0x80000003,	                //	链路测试应答
        ChatFee = 0x00000004,	                    //	聊天计费
        ChatFeeResp = 0x80000004,                   //	聊天计费应答
        ChatFeePush = 0x00000005,	                //	聊天计费推送
        ChatFeePushResp = 0x80000005,               //	聊天计费推送应答
        ChatFeeRateSet = 0x00000006,                //	聊天计费倍率设置
        ChatFeeRateSetResp = 0x80000006,            //	聊天计费倍率设置应答
        ChatFeeRateQuery = 0x00000007,              //	聊天计费倍率查询
        ChatFeeRateQueryResp = 0x80000007,          //	聊天计费倍率查询应答
        ChatFeeRateSetPush = 0x00000008,            //  聊天计费倍率设置推送
        ChatFeeRateSetPushResp = 0x80000008,        //  聊天计费倍率设置推送回复
        ClientCountQuery = 0x00000009,              //  获取当前客户端连接数
        ClientCountQueryResp = 0x80000009,          //  获取当前客户端连接数应答
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
        public static string Get_MMDDHHMMSS_String(DateTime dt)
        {
            string s = dt.Month.ToString().PadLeft(2, '0');
            s += dt.Day.ToString().PadLeft(2, '0');
            s += dt.Hour.ToString().PadLeft(2, '0');
            s += dt.Minute.ToString().PadLeft(2, '0');
            s += dt.Second.ToString().PadLeft(2, '0');
            return s;
        }

        public static string Get_YYYYMMDD_String(DateTime dt)
        {
            string s = dt.Year.ToString().PadLeft(4, '0');
            s += dt.Month.ToString().PadLeft(2, '0');
            s += dt.Day.ToString().PadLeft(2, '0');
            return s;
        }
    }
}
