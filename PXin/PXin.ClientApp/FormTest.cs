using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using PXin.Protocal;

namespace PXin.ClientApp
{
    public partial class FormTest : Form
    {
        public ConcurrentDictionary<string, TcpClient> lstClient;
        public int clientCounter;
        private uint ReceiveType;
        private string Receiver;
        public FormTest()
        {
            InitializeComponent();
            lstClient = new ConcurrentDictionary<string, TcpClient>();
            BingCbx();
        }
        private void BingCbx()
        {
            var list = new List<object>
            {
                new { Id=1,Value="单聊"},
                new { Id=2,Value="群聊"},
                new { Id=3,Value="讨论组"}
            };
            comboBox1.DataSource = list;
            comboBox1.ValueMember = "Id";
            comboBox1.DisplayMember = "Value";
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            string ip = ConfigurationManager.AppSettings["IP"];
            System.Threading.Tasks.Task.Run(() =>
            {
                System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                string[] arrLine = File.ReadAllLines("dtn_" + ip + ".txt");
                int nCount = Convert.ToInt32(this.txtCount.Text);
                int index = lstClient.Count;
                foreach (string line in arrLine)
                {
                    if (lstClient.ContainsKey(line))
                    {
                        continue;
                    }
                    //Log.MessageInfo(line);
                    AddClient(line);
                    //Thread.Sleep(100);
                    //while (clientCounter > 100)
                    //{
                    //    Thread.Sleep(1000);
                    //}
                    //Interlocked.Increment(ref clientCounter);
                    //new Thread(new ParameterizedThreadStart(AddClient)).Start(line);
                    index++;
                    if (index >= nCount)
                    {
                        break;
                    }
                }
                //while (clientCounter > 0)
                //{
                //    Thread.Sleep(1000);
                //}
                watch.Stop();
                MessageBox.Show("完成," + watch.Elapsed.TotalSeconds);
            });
        }

        private void AddClient(object line)
        {
            //System.Diagnostics.Trace.WriteLine(DateTime.Now + "-开始连接:" + line.ToString());
            TcpClient client = new TcpClient();
            client.DeviceId = line.ToString();
            client.ConnEventHandler += new CommuHander(_client_ConnHandler);
            client.MsgRecvEventHandler += new MessageHander(_client_MsgRecvHandler);
            client.MsgSendEventHandler += new MessageHander(_client_MsgSendHandler);
            if (!client.Start())
            {
                Log.MessageInfo("--连接失败--" + line);
                System.Threading.Thread.Sleep(1000);
                return;
            }
            //Login login = new Login();
            //login.Body.NodeCode = this.txtDTN.Text;
            //login.Body.Pwd = this.txtPwd.Text;
            //login.Body.Version = 1;
            //login.Body.Sign = Helper.MakeMd5(this.txtDTN.Text + this.txtPwd.Text, "DvUZIrmKXs");

            Login login = new Login();
            login.Body.NodeCode = line.ToString();
            login.Body.Pwd = "123456";
            login.Body.Version = 1;
            login.Body.Sign = Helper.MakeMd5(line.ToString() + "123456", "DvUZIrmKXs");
            client.SendMessageLogin(login);
            //Interlocked.Decrement(ref clientCounter);
        }
        private void _client_MsgSendHandler(object sender, EventMessage e)
        {
            TcpClient client = sender as TcpClient;
            Log.MessageInfo("发送-" + client._identity + ":" + e.Msg);
            System.Diagnostics.Trace.WriteLine(client.DeviceId + "Recv:" + e.Msg);

        }
        private void _client_MsgRecvHandler(object sender, EventMessage e)
        {
            TcpClient client = sender as TcpClient;
            Log.MessageInfo("接收" + client._identity + ":" + e.Msg);
            switch (e.CommandType)
            {
                case PXin_COMMAND_TYPE.Login:
                    break;
                case PXin_COMMAND_TYPE.LoginResp:
                    lstClient.TryAdd(client.DeviceId, client);
                    break;
                //case PXin_COMMAND_TYPE.Logout:
                //    break;
                //case PXin_COMMAND_TYPE.LogoutResp:
                //    break;
                //case PXin_COMMAND_TYPE.Active:
                //    break;
                //case PXin_COMMAND_TYPE.ActiveResp:
                //    ActiveResp actc = e.Package as ActiveResp;
                //    Log.MessageInfo($"链接路测返回:{dic[client._identity]},{actc.ToString()}");
                //    break;
                //case PXin_COMMAND_TYPE.ChatFee:
                //    break;
                //case PXin_COMMAND_TYPE.ChatFeeResp:
                //    ChatFeeResp resr = e.Package as ChatFeeResp;
                //    Log.MessageInfo($"聊天计费返回:{dic[client._identity]},body:{JsonConvert.SerializeObject(resr.Body)}");
                //    break;
                //case PXin_COMMAND_TYPE.ChatFeePush:
                //    ChatFeePush push = e.Package as ChatFeePush;
                //    Log.MessageInfo($"聊天推送:{dic[client._identity]},body:{JsonConvert.SerializeObject(push.Body)}");
                //    break;
                //case PXin_COMMAND_TYPE.ChatFeePushResp:
                //    ChatFeePushResp pushresp = e.Package as ChatFeePushResp;
                //    Log.MessageInfo($"聊天推送返回:{dic[client._identity]},body:{JsonConvert.SerializeObject(pushresp.Body)}");
                //    break;
                //case PXin_COMMAND_TYPE.ChatFeeRateSet:
                //    break;
                //case PXin_COMMAND_TYPE.ChatFeeRateSetResp:
                //    ChatFeeRateSetResp setResp = e.Package as ChatFeeRateSetResp;
                //    Log.MessageInfo($"倍率设置返回:{dic[client._identity]},body:{JsonConvert.SerializeObject(setResp.Body)}");
                //    break;
                //case PXin_COMMAND_TYPE.ChatFeeRateQuery:
                //    break;
                //case PXin_COMMAND_TYPE.ChatFeeRateQueryResp:
                //    ChatFeeRateQueryResp resq = e.Package as ChatFeeRateQueryResp;
                //    Log.MessageInfo($"倍率查询返回:{dic[client._identity]},body:{JsonConvert.SerializeObject(resq.Body)}");
                //    break;
                //case PXin_COMMAND_TYPE.ChatFeeRateSetPush:
                //    ChatFeeRateSetPush setPush = e.Package as ChatFeeRateSetPush;
                //    Log.MessageInfo($"倍率设置推送:{dic[client._identity]},body:{JsonConvert.SerializeObject(setPush.Body)}");
                //    break;
                //case PXin_COMMAND_TYPE.ChatFeeRateSetPushResp:
                //    break;
                default:
                    break;
            }
        }

        private void _client_ConnHandler(object sender, EventCommu e)
        {
            TcpClient client = sender as TcpClient;
            if (e.State == 1)
            {
                lstClient.TryRemove(client.DeviceId, out _);
            }
        }

        private void SendChatFee(object obj)
        {
            TcpClient client = obj as TcpClient;
            client.SendChatFee(ReceiveType, Receiver);
            Interlocked.Decrement(ref clientCounter);
        }
        private void RateQuery(TcpClient client, ChatFeeRateQuery chatFee)
        {
            //TcpClient client = obj as TcpClient;
            client.SendMessageRateQuery(chatFee);
            //Interlocked.Decrement(ref clientCounter);
        }
        private void RateSet(TcpClient client, ChatFeeRateSet rateSet)
        {
            client.SendMessageRateSet(rateSet);
            //Interlocked.Decrement(ref clientCounter);
        }
        private void btnClientCount_Click(object sender, EventArgs e)
        {
            MessageBox.Show(lstClient.Values.Count().ToString());
        }

        private void btnChatFee_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("输入聊天对象");
                return;
            }
            ReceiveType = uint.Parse(comboBox1.SelectedValue.ToString());
            Receiver = textBox1.Text;
            foreach (TcpClient client in lstClient.Values)
            {
                while (clientCounter > 100)
                {
                    Thread.Sleep(1000);
                }
                Interlocked.Increment(ref clientCounter);
                new Thread(new ParameterizedThreadStart(SendChatFee)).Start(client);
            }
            while (clientCounter > 0)
            {
                Thread.Sleep(1000);

            }
            MessageBox.Show("完成");
        }

        private void btnRateQuery_Click(object sender, EventArgs e)
        {
            string ip = ConfigurationManager.AppSettings["IP"];

            if (string.IsNullOrWhiteSpace(txtTargetId.Text))
            {
                MessageBox.Show("Senderid必填");
                return;
            }
            var typetxt = Convert.ToInt32(cbType.SelectedItem);
            var sendertxt = Convert.ToInt32(txtTargetId.Text);
            string[] arrLine = File.ReadAllLines("nodeids_" + ip + ".txt");
            if (arrLine == null)
            {
                MessageBox.Show("根目录配置nodeids.txt");
                return;
            }
            int counter = Convert.ToInt32(this.txtRateQueryCount.Text);
            System.Threading.Tasks.Task.Run(() =>
            {
                System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                watch.Start();

                int i = 0;
                foreach (TcpClient client in lstClient.Values)
                {
                    try
                    {
                        ChatFeeRateQuery chatFeeRateQuery = new ChatFeeRateQuery();
                        chatFeeRateQuery.Body.Type = typetxt;
                        chatFeeRateQuery.Body.Receiver = typetxt == 1 ? int.Parse(arrLine[i]) : sendertxt;
                        chatFeeRateQuery.Body.Sender = typetxt == 1 ? sendertxt : 0;
                        RateQuery(client, chatFeeRateQuery);
                        //Log.ExceptInfo(i.ToString()+"---"+arrLine[i]);
                        i++;
                        if (i > counter)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.ExceptInfo(ex.ToString());
                        i++;
                    }
                }
                watch.Stop();
                MessageBox.Show(watch.ElapsedMilliseconds.ToString());
            });
            MessageBox.Show("完成");
        }

        private void btnSetRate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRate.Text))
            {
                MessageBox.Show("倍率必填");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("senderid必填");
                return;
            }
            var rate = Convert.ToDecimal(txtRate.Text);
            var typeid = int.Parse(comboBox2.Text);
            var senderid = int.Parse(textBox2.Text);
            string[] arrLine = File.ReadAllLines("nodeids.txt");
            if (arrLine == null)
            {
                MessageBox.Show("根目录配置nodeids.txt");
                return;
            }
            int i = 0;
            System.Threading.Tasks.Task.Run(() =>
            {
                foreach (TcpClient client in lstClient.Values)
                {
                    try
                    {
                        ChatFeeRateSet chatFeeRateSet = new ChatFeeRateSet();
                        chatFeeRateSet.Body.Type = typeid;
                        chatFeeRateSet.Body.Rate = rate;
                        chatFeeRateSet.Body.Sender = typeid == 1 ? senderid : 0;
                        chatFeeRateSet.Body.Receiver = typeid == 1 ? int.Parse(arrLine[i]) : senderid;
                        RateSet(client, chatFeeRateSet);
                        i++;
                    }
                    catch (Exception ex)
                    {
                        Log.ExceptInfo(ex.ToString());
                        i++;
                    }
                }

            });
            while (i < lstClient.Count)
            {
                Thread.Sleep(1000);
            }
            MessageBox.Show("完成");
        }

        private void BtnActiveTest_Click(object sender, EventArgs e)
        {
            int counter = Convert.ToInt32(this.txtKeepAliveCount.Text);
            System.Threading.Tasks.Task.Run(() =>
            {
                System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                int i = 0;
                foreach (TcpClient client in lstClient.Values)
                {
                    try
                    {
                        client.SendMessageActive();
                        //Log.ExceptInfo(i.ToString()+"---"+arrLine[i]);
                        i++;
                        if (i > counter)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.ExceptInfo(ex.ToString());
                        i++;
                    }
                }
                watch.Stop();
                MessageBox.Show(watch.ElapsedMilliseconds.ToString());
            });
            MessageBox.Show("完成");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            foreach (var item in lstClient.Values.ToList())
            {
                item.Close();
            }
        }
    }
}