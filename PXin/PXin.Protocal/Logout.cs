using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
namespace PXin.Protocal
{
    [Serializable]
    public class Wt_Logout : MessageBase<LogoutBody>
    {
        public Wt_Logout()
            : base(MessageHeader.Length, PXin_COMMAND_TYPE.Logout, Util.GetSquence())
        {
        }
        public Wt_Logout(byte[] bytes)
            : base(bytes)
        {
        }
    }
    [Serializable]
    public class LogoutBody
    {
        public int _reason;
        public int Reason { get { return _reason; } set { _reason = value; } }
    }
    [Serializable]
    public class LogoutResp : MessageBase<LogoutRespBody>
    {
        public LogoutResp(long squeueID)
            : base(MessageHeader.Length, PXin_COMMAND_TYPE.LogoutResp, squeueID)
        {
        }
        public LogoutResp(byte[] bytes)
            : base(bytes)
        {
        }
    }
    [Serializable]
    public class LogoutRespBody
    {
    }
}
