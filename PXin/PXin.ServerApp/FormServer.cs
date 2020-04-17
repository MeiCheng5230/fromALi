using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using PXin.Commu.Facade;
using PXin.Common;
using PXin.Commu;
using PXin.Protocal;

namespace PXin.ServerApp
{
    public delegate void RTBText(string msg);
    public delegate void LabelText(Label lbl, string msg);
    public partial class FormServer : Form
    {
        private RTBText rtbText;
        private LabelText lblText;

        private ServerFacade serverFacade = new ServerFacade();

        public FormServer()
        {
            InitializeComponent();
            this.Text += ConfigurationManager.AppSettings["IP"] + ":" + ConfigurationManager.AppSettings["Port"];
        }

        private void FormServer_Load(object sender, EventArgs e)
        {
            rtbText = new RTBText(msgSetText);
            lblText = new LabelText(msgLabelSetText);
            Dictionary<int, int> temp = new Dictionary<int, int>();
            this.serverFacade.ConnectHandler_Show += serverFacade_ConnectHandler_Show;
            this.serverFacade.MsgHandler_Show += serverFacade_MsgHandler_Show;
        }
        /// <summary>
        /// 事件方法-客户端连接时调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serverFacade_ConnectHandler_Show(object sender, CommuEventArgs e)
        {
            Log.ConnectionInfo((e.State == 0 ? "接收连接" : "断开连接") + e.Identity + "," + e.Reason);
            //rtbText((e.State == 0 ? "接收连接" : "断开连接") + e.Identity + "," + e.Reason);
        }
        /// <summary>
        /// 事件方法-发送/接收消息时调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serverFacade_MsgHandler_Show(object sender, MessageEventArgs e)
        {
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(e.To))
            {
                msg = e.To + "发送消息:" + e.Msg;
            }
            else
            {
                msg = e.From + "接收消息:" + e.Msg;
            }
            //rtbText(msg);
            //Log.MessageInfo(msg);
        }

        private void msgSetText(string msg)
        {
            if (!this.InvokeRequired)
            {
                this.rtbMsg.Text += DateTime.Now.ToString() + " " + msg + Environment.NewLine;
            }
            else
            {
                this.rtbMsg.Invoke(rtbText, msg);
            }
        }
        private void msgLabelSetText(Label lbl, string msg)
        {
            if (!this.InvokeRequired)
            {
                lbl.Text = msg;
            }
            else
            {
                lbl.Invoke(lblText, lbl, msg);
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            serverFacade.StartServer();
            this.rtbMsg.Text = DateTime.Now.ToString() + " " + "服务器端准备就绪" + Environment.NewLine;
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            serverFacade.StopServer();
        }

        private void btnSendActiveMsg_Click(object sender, EventArgs e)
        {
            if (serverFacade.CommuClient != null && serverFacade.CommuClient.Count > 0)
            {
                List<CommuTcpClient> clients = serverFacade.CommuClient.Values.ToList();
                for (int i = 0; i < clients.Count; i++)
                {
                    Active actvie = new Active();
                    clients[i].SendData(actvie.ToBytes());
                }
            }
        }

        private void btnClientCount_Click(object sender, EventArgs e)
        {
            this.rtbMsg.Text += DateTime.Now.ToString() + " " + "客户端连接数为:" + serverFacade.CommuClient.Count + Environment.NewLine;
        }

    }
}
