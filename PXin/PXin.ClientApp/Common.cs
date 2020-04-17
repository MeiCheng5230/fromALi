using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PXin.Protocal;
using System.IO;
using System.Security.Cryptography;

namespace PXin.ClientApp
{
    public delegate void CommuHander(object sender, EventCommu e);
    public delegate void MessageHander(object sender, EventMessage e);

    public class EventMessage : EventArgs
    {
        public EventMessage(PXin_COMMAND_TYPE commandType,object package,string msg)
        {
            CommandType = commandType;
            Package = package;
            Msg = msg;
        }
        public PXin_COMMAND_TYPE CommandType { get; set; }
        public object Package { get; set; }
        public string Msg { get; set; }
    }
    public class EventCommu : EventArgs
    {
        private string _clientInfo;
        private int _state;
        public EventCommu(string info)
        {
            _clientInfo = info;
        }
        public EventCommu(string info, int state)
        {
            _clientInfo = info;
            _state = state;
        }
        public string ClientInfo
        {
            get { return _clientInfo; }
            set { _clientInfo = value; }
        }
        /// <summary>
        /// 0-连接，1-断开
        /// </summary>
        public int State
        {
            get { return _state; }
            set { _state = value; }
        }
    }
    public class Log
    {
        private static string _dirRunLog;
        private static object _syncExcept = new object();
        private static object _syncRunInfo = new object();
        private static object _syncMessage = new object();
        static Log()
        {
            _dirRunLog = Directory.GetCurrentDirectory();
            if (!Directory.Exists(_dirRunLog))
            {
                Directory.CreateDirectory(_dirRunLog);
            }
        }
        public static void ExceptInfo(string msg)
        {
            lock (_syncExcept)
            {
                string fileNameExcept = Path.Combine(_dirRunLog, "Except1_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
                File.AppendAllText(fileNameExcept, DateTime.Now + "," + msg + Environment.NewLine);
            }
        }
        public static void RunInfo(string msg)
        {
            lock (_syncRunInfo)
            {
                string fileNameExcept = Path.Combine(_dirRunLog, "Runinfo1_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
                File.AppendAllText(fileNameExcept, DateTime.Now + "," + msg + Environment.NewLine);
            }
        }
        public static void MessageInfo(string msg)
        {
            lock (_syncMessage)
            {
                string fileNameExcept = Path.Combine(_dirRunLog, "Messageinfo1_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
                File.AppendAllText(fileNameExcept, DateTime.Now + "," + msg + Environment.NewLine);
            }
        }
    }
    public static class Helper
    {
        public static string GetMask(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "*";
            string temp = str;
            if (str.Length > 1)
                temp = str.Substring(str.Length - 1);
            string mask = string.Empty;
            int maskLength = str.Length - 1;
            for (int i = 0; i < maskLength; i++)
                mask += "*";
            temp = mask + temp;
            return temp;
        }
        public static string MakeMd5(string str, string _key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.UTF8.GetBytes(str + _key));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
    }

}
