//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace PXin.Commu.Facade
//{
//    public class RecvMsgProcess
//    {
//        private MessageQueue _recvQueue = MsgQueue.GetReceive(); //保存接收的位置信息
//        private void InitSendQueue()
//        {
//            if (_recvQueue != null)
//            {
//                _recvQueue.Close();
//            }
//            _recvQueue = MsgQueue.GetReceive();
//            _recvQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(_recvQueue_ReceiveCompleted);
//            _recvQueue.BeginReceive();
//        }
//        public bool Start()
//        {
//            InitSendQueue();
//            return true;
//        }
//        public bool Stop()
//        {
//            if (_recvQueue != null)
//            {
//                _recvQueue.Close();
//                _recvQueue.Dispose();
//                _recvQueue = null;
//            }
//            return true;
//        }
//        private void _recvQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
//        {
//            MessageQueue mqRecv = null;
//            try
//            {
//                mqRecv = (MessageQueue)sender;
//                MqPackage msg = (MqPackage)mqRecv.EndReceive(e.AsyncResult).Body;
//                MessageHeader header = new MessageHeader(msg.Body);
//                if (header.Command_Id == WT_COMMAND_TYPE.Wt_Location)
//                {
//                    //写数据库
//                    Wt_Location loc = new Wt_Location(msg.Body);
//                    Log.MsgQueue("写数据库:" + loc.ToString());
//                }
//                else if (header.Command_Id == WT_COMMAND_TYPE.Wt_SOS)
//                {
//                    //写数据库
//                    Wt_SOS sos = new Wt_SOS(msg.Body);
//                    Log.MsgQueue("写数据库:" + sos.ToString());
//                }
//            }
//            catch (System.Exception err)
//            {
//                Log.ExceptInfo(err.ToString());
//            }

//            try
//            {
//                mqRecv.BeginReceive();
//            }
//            catch (System.Exception err)
//            {
//                Log.ExceptInfo(err.ToString());
//                //防止消息队列异常，停止读取消息,重新启动消息队列
//                InitSendQueue();
//            }
//        }
//    }
//}
