using PXin.Protocal;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PXin.ClientApp
{

    public delegate void RTBText(string msg);
    public delegate void RTBTextState(RichTextBox rtb, string msg);
    public delegate void LabelText(Label lbl, string msg);
    public partial class FormClient : Form
    {
        private RTBText rtbText;
        private RTBTextState rtbTextState;
        private LabelText lblText;
        private TcpClient _client = new TcpClient();
        public FormClient()
        {
            InitializeComponent();
            rtbText = new RTBText(msgSetText);
            rtbTextState = new RTBTextState(msgSetTextState);
            lblText = new LabelText(msgSetLabel);
            //_client.IsUV = false;
        }
        #region 委托方法
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
        private void msgSetTextState(RichTextBox rtb, string msg)
        {
            if (!this.InvokeRequired)
            {
                rtb.Text += msg + Environment.NewLine;
            }
            else
            {
                rtb.Invoke(rtbTextState, rtb, msg);
            }
        }
        private void msgSetLabel(Label lbl, string msg)
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
        #endregion
        private void btnStart_Click(object sender, EventArgs e)
        {
            _client.Start();
        }
        private void btnEnd_Click(object sender, EventArgs e)
        {
            _client.Close();
        }
        private void FormClient_Load(object sender, EventArgs e)
        {
            this.Text = ConfigurationManager.AppSettings["ip"];
            cbType.SelectedIndex = 0;
            //订阅客户端tcp连接事件
            _client.ConnEventHandler += new CommuHander(_client_ConnHandler);
            //订阅客户端消息接收事件
            _client.MsgRecvEventHandler += new MessageHander(_client_MsgRecvHandler);
            //订阅客户端消息发送事件
            _client.MsgSendEventHandler += new MessageHander(_client_MsgSendHandler);
        }
        #region 事件方法
        private void _client_ConnHandler(object sender, EventCommu e)
        {
            rtbText(e.ClientInfo);
        }
        void _client_MsgSendHandler(object sender, EventMessage e)
        {
            TcpClient client = sender as TcpClient;
            Log.MessageInfo("发送-" + client._identity + ":" + e.Msg);
            rtbText(e.Msg);
        }
        void _client_MsgRecvHandler(object sender, EventMessage e)
        {
            TcpClient client = sender as TcpClient;
            Log.MessageInfo("接收-" + client._identity + ":" + e.Msg);
            rtbText(e.Msg);
            if (e.CommandType == PXin_COMMAND_TYPE.LoginResp)
            {
                string msg = string.Empty;
                LoginResp resp = e.Package as LoginResp;
                if (resp.Body.Status == 0)
                {
                    msg = "登陆失败";
                }
                else
                {
                    msg = "登陆成功";
                }
                rtbTextState(this.rtbState, msg);
            }
        }
        #endregion
        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Body.NodeCode = this.txtDTN.Text;
            login.Body.Pwd = this.txtPwd.Text;
            login.Body.Version = 1;
            login.Body.Sign = Helper.MakeMd5(this.txtDTN.Text + this.txtPwd.Text, "DvUZIrmKXs");
            _client.SendMessageLogin(login);
            _client.DeviceId = this.txtDTN.Text;
            this.Text = ConfigurationManager.AppSettings["IP"] + "[" + _client.DeviceId + "]" + DateTime.Now.ToString();
        }
        private void btnSendMsgActive_Click(object sender, EventArgs e)
        {
            _client.SendMessageActive();
        }

        private void btnSendMsgLogout_Click(object sender, EventArgs e)
        {
            _client.SendMessageLogout();
        }
        private void btnTest_Click(object sender, EventArgs e)
        {
            FormTest test = new FormTest();
            test.Show();
        }
        private void btnChatFee_Click(object sender, EventArgs e)
        {
            _client.SendChatFee(Convert.ToUInt32(this.comRecvType.SelectedItem),this.txtRecvTargetid.Text);
        }
        private void btnPM_Click(object sender, EventArgs e)
        {
            _client.SendPM();
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSetRate_Click(object sender, EventArgs e)
        {
            ChatFeeRateSet chatFeeRateSet = new ChatFeeRateSet();
            chatFeeRateSet.Body.Type = 2;
            chatFeeRateSet.Body.Rate = Convert.ToDecimal(txtRate.Text);
            chatFeeRateSet.Body.Sender = 0;
            chatFeeRateSet.Body.Receiver = 481;
            _client.SendMessageRateSet(chatFeeRateSet);
        }

        private void btnRateQuery_Click(object sender, EventArgs e)
        {
            ChatFeeRateQuery chatFeeRateQuery = new ChatFeeRateQuery();
            chatFeeRateQuery.Body.Type = Convert.ToInt32(cbType.SelectedItem);
            chatFeeRateQuery.Body.Receiver = Convert.ToInt32(txtNodeId.Text);
            chatFeeRateQuery.Body.Sender = Convert.ToInt32(txtTargetId.Text);
            _client.SendMessageRateQuery(chatFeeRateQuery);
        }

        private void btnServiceClientCount_Click(object sender, EventArgs e)
        {
            ClientCountQuery clientCountQuery = new ClientCountQuery();
            _client.SendMessageClientCountQuery(clientCountQuery);
        }
        /// <summary>
        /// 聊天包重复发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                Task.Run(() =>
                {
                    _client.SendChatFeeRepeat();
                });
            }
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.rtbMsg.Text = "";
            this.rtbState.Text = "";
        }
    }
}
