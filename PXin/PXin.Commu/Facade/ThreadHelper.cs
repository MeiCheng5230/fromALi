using PXin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PXin.Commu.Facade
{
    public class ThreadData
    {
        public CommuTcpClient CommuTcpClient { get; set; }
        public byte[] MsgData { get; set; }
    }
    public class ThreadHelper
    {
        private static object _sync;
        private static int _threadCounter;
        static ThreadHelper()
        {
            _sync = new object();
        }
        private static void SendMsg(object obj)
        {
            try
            {
                ThreadData threadData = obj as ThreadData;
                if (threadData != null)
                {
                    threadData.CommuTcpClient.SendData(threadData.MsgData);
                }
            }
            catch (System.Exception err)
            {
                Log.ExceptInfo(err.ToString());
            }
            finally
            {
                Interlocked.Decrement(ref _threadCounter);
            }
        }
        public static void InputQueue(ThreadData threadData)
        {
            lock (_sync)
            {
                while (_threadCounter > PxinConst.MaxThreadCount)
                {
                    Thread.Sleep(1000);
                }
            }
            Interlocked.Increment(ref _threadCounter);
            new Thread(new ParameterizedThreadStart(SendMsg)).Start(threadData);
        }
    }
}
