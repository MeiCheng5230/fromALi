using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PXin.Common
{
    public class Log
    {
        private static string _dirRunLog;
        private static object _syncConnection = new object();
        private static object _syncExcept = new object();
        private static object _syncRunInfo = new object();
        private static object _syncMessage = new object();
        static Log()
        {
            _dirRunLog = Path.Combine(PxinConst.DirPlayLog, PxinConst.DirRunLog);
            if (!Directory.Exists(_dirRunLog))
            {
                Directory.CreateDirectory(_dirRunLog);
            }
        }
        public static void ExceptInfo(string msg)
        {
            lock (_syncExcept)
            {
                string fileNameExcept = Path.Combine(_dirRunLog, "Except_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
                File.AppendAllText(fileNameExcept, DateTime.Now + "," + msg + Environment.NewLine);
            }
        }
        public static void RunInfo(string msg)
        {
            lock (_syncRunInfo)
            {
                string fileNameExcept = Path.Combine(_dirRunLog, "Runinfo_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
                File.AppendAllText(fileNameExcept, DateTime.Now + "," + msg + Environment.NewLine);
            }
        }
        public static void MessageInfo(string msg)
        {
            lock (_syncMessage)
            {
                string fileNameExcept = Path.Combine(_dirRunLog, "Messageinfo_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
                File.AppendAllText(fileNameExcept, DateTime.Now + "," + msg + Environment.NewLine);
            }
        }
        public static void ConnectionInfo(string msg)
        {
            lock (_syncMessage)
            {
                string fileNameExcept = Path.Combine(_dirRunLog, "ConnectionInfo_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
                File.AppendAllText(fileNameExcept, DateTime.Now + "," + msg + Environment.NewLine);
            }
        }
    }
}
