using System;
using System.Text;
using Newtonsoft.Json;

namespace PXin.Protocal
{
    [Serializable]
    public class Login : MessageBase<LoginBody>
    {
        public Login()
            : base(MessageHeader.Length, PXin_COMMAND_TYPE.Login, Util.GetSquence())
        {
        }
        public Login(byte[] bytes)
            : base(bytes)
        {
        }
    }
    [Serializable]
    public class LoginBody
    {
        public uint ClientID { get; set; }
        public uint Version { get; set; }
        public string NodeCode { get; set; }
        public string Pwd { get; set; }
        public string Sign { get; set; }
    }
    [Serializable]
    public class LoginResp : MessageBase<LoginRespBody>
    {
        public LoginResp(long squeueID)
            : base(MessageHeader.Length, PXin_COMMAND_TYPE.LoginResp, squeueID)
        {
        }
        public LoginResp(byte[] bytes)
            : base(bytes)
        {
        }
    }
    [Serializable]
    public class LoginRespBody
    {
        public string LoginDesc { get; set; }
        public uint Status { get; set; }
        public decimal Rate { get; set; }
        public decimal VDianBalance { get; set; }
        public decimal PDianBalance { get; set; }
    }
}
