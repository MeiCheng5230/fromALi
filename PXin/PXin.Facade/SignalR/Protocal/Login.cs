using PXin.Facade.SignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.SignalR.Protocal
{
    /// <summary>
    /// 
    /// </summary>
    public class Login : MessageBase<LoginBody>
    {
        /// <summary>
        /// 
        /// </summary>
        public Login()
            : base(PXinCommandType.Login, Util.GetSquence())
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageString"></param>
        public Login(string messageString)
            : base(messageString)
        {

        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class LoginBody
    {
        /// <summary>
        /// 
        /// </summary>
        public uint ClientID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public uint Version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NodeCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Sign { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class LoginResp : MessageBase<LoginRespBody>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequenceId"></param>
        public LoginResp(long sequenceId) :
            base(PXinCommandType.LoginResp, sequenceId)
        {

        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class LoginRespBody
    {
        /// <summary>
        /// 
        /// </summary>
        public string LoginDesc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public uint Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Rate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal VDianBalance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal PDianBalance { get; set; }
    }
}
