using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using PXin.Common;
using Wt.DataAccess;

namespace PXin.Commu.DataAccess
{
    public class PXinDb
    {
        #region 聊天计费
        /// <summary>
        /// 添加聊天计费历史
        /// </summary>
        /// <param name="tchatFeehis"></param>
        /// <returns></returns>
        public bool AddChatFee(TchatFeehis tchatFeehis, OracleTransaction transaction)
        {
            var sql = "INSERT INTO TCHAT_FEEHIS(HISID,NODEID,FEETYPE,BUSINESSTYPE,GROUPID,NUM,RECEIVER,AMOUNT,SENDTIME,STATUS,CREATETIME,REMARKS,SEQUENCEID) VALUES(SEQ_TCHAT_FEEHIS.NEXTVAL,'{0}','{1}','{2}','{3}','{4}','{5}','{6}',to_date('{7}','YYYY-MM-DD HH24:MI:SS'),'{8}',to_date('{9}','YYYY-MM-DD HH24:MI:SS'),'{10}','{11}')";
            sql = string.Format(sql, tchatFeehis.Nodeid, tchatFeehis.Feetype, tchatFeehis.Businesstype, tchatFeehis.Groupid, tchatFeehis.Num, tchatFeehis.Receiver, tchatFeehis.Amount, tchatFeehis.Sendtime, tchatFeehis.Status, tchatFeehis.Createtime, tchatFeehis.Remarks, tchatFeehis.Sequenceid);
            return OracleHelper.ExecuteNonQuery(transaction, CommandType.Text, sql) > 0;
        }

        /// <summary>
        /// 根据seq获取聊天计费历史
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public TchatFeehis GetChatFeeHisBySeq(string seq)
        {
            string sql = "SELECT * FROM TCHAT_FEEHIS WHERE SEQUENCEID='{0}'";
            sql = string.Format(sql, seq);
            TchatFeehis tchatFeehis = new TchatFeehis();
            using (var reader = OracleHelper.ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    tchatFeehis.Hisid = Convert.ToInt32(reader["Hisid"]);
                }
            }
            return tchatFeehis;
        }
        /// <summary>
        /// 获取信友圈用户信息
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public TpxinUserinfo GetUserInfoByNodeId(int nodeId)
        {
            string sql = "SELECT * FROM TPXIN_USERINFO WHERE NODEID = {0}";
            sql = string.Format(sql, nodeId);
            TpxinUserinfo tpxinUserinfo = new TpxinUserinfo();
            using (var reader = OracleHelper.ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    tpxinUserinfo.Infoid = Convert.ToInt32(reader["Infoid"]);
                    tpxinUserinfo.Nodeid = Convert.ToInt32(reader["Nodeid"]);
                    tpxinUserinfo.P = Convert.ToDecimal(reader["P"]);
                    tpxinUserinfo.V = Convert.ToDecimal(reader["V"]);
                    tpxinUserinfo.Createtime = Convert.ToDateTime(reader["Createtime"]);
                }
            }
            return tpxinUserinfo;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="tchatFeehis"></param>
        /// <returns></returns>
        public bool UpdateTpxinUserinfo(TpxinUserinfo tpxinUserinfo, OracleTransaction transaction)
        {
            var sql = "UPDATE TPXIN_USERINFO SET P = P + {0},V = V + {1},REMARKS = '{2}' WHERE NODEID = {3}";
            sql = string.Format(sql, tpxinUserinfo.P, tpxinUserinfo.V, tpxinUserinfo.Remarks, tpxinUserinfo.Nodeid);
            return OracleHelper.ExecuteNonQuery(transaction, CommandType.Text, sql) > 0;
        }
        /// <summary>
        /// 查询群组用户ID
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<int> GetGroupUserId(int groupId)
        {
            string sql = "SELECT USERID FROM TCHAT_GROUP_USER WHERE GROUPID = {0}";
            sql = string.Format(sql, groupId);
            List<int> userIdList = new List<int>();
            using (var reader = OracleHelper.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    userIdList.Add(Convert.ToInt32(reader["USERID"]));
                }
            }
            return userIdList;
        }
        #endregion

        /// <summary>
        /// 添加金额变化记录
        /// </summary>
        /// <param name="amountChangeHis"></param>
        /// <returns></returns>
        public bool AddTpxinAmountChangeHis(TpxinAmountChangeHis amountChangeHis, OracleTransaction transaction)
        {
            var sql = "INSERT INTO TPXIN_AMOUNT_CHANGE_HIS(HISID,NODEID,TYPEID,AMOUNT,REASON,TRANSFERID,CREATETIME,REMARKS,AMOUNTBEFORE,AMOUNTAFTER) VALUES(SEQ_TPXIN_AMOUNT_CHANGE_HIS.NEXTVAL,'{0}','{1}','{2}','{3}','{4}',to_date('{5}','YYYY-MM-DD HH24:MI:SS'),'{6}','{7}','{8}')";
            sql = string.Format(sql, amountChangeHis.Nodeid, amountChangeHis.Typeid, amountChangeHis.Amount, amountChangeHis.Reason, amountChangeHis.Transferid, amountChangeHis.Createtime, amountChangeHis.Remarks, amountChangeHis.Amountbefore, amountChangeHis.Amountafter);
            return OracleHelper.ExecuteNonQuery(transaction, CommandType.Text, sql) > 0;
        }

        #region 登录
        /// <summary>
        /// 检查用户是否已冻结
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public bool CheckLockUser(int nodeId)
        {
            string sql = "SELECT * FROM TNET_LOCKUSER WHERE NODEID = {0} AND UNLOCKTIME>SYSDATE AND LOCKTYPE = 1";
            sql = string.Format(sql, nodeId);
            return OracleHelper.ExecuteNonQuery(sql) > 0;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="nodeCode"></param>
        /// <returns></returns>
        public int LoginByNodeCode(string nodeCode)
        {
            string sql = "SELECT * FROM TNET_REGINFO WHERE NODECODE=:NODECODE";
            OracleParameter oracleParameter = new OracleParameter(":NODECODE", nodeCode);
            int nodeid = 0;
            using (var dataRead = OracleHelper.ExecuteReader(sql, oracleParameter))
            {
                if (dataRead.Read())
                {
                    nodeid = Convert.ToInt32(dataRead["Nodeid"]);
                }
            }
            return nodeid;
        }
        #endregion

        #region 聊天倍率

        /// <summary>
        /// 聊天计费倍率设置
        /// </summary>
        /// <param name="tchatRate"></param>
        /// <returns></returns>
        public bool ChatFeeRateSet(TchatRate tchatRate)
        {
            var sql = "INSERT INTO TCHAT_RATE(ID,TYPEID,SENDER,RECEIVER,RATE,CREATETIME,REMARKS) VALUES(SEQ_TCHAT_RATE.NEXTVAL,{0},{1},{2},{3},to_date('{4}','YYYY-MM-DD HH24:MI:SS'),'{5}')";
            sql = string.Format(sql, tchatRate.Typeid, tchatRate.Sender, tchatRate.Receiver, tchatRate.Rate, tchatRate.Createtime, tchatRate.Remarks);
            return OracleHelper.ExecuteNonQuery(sql) > 0;
        }
        /// <summary>
        /// 修改倍率
        /// </summary>
        /// <param name="tchatFeehis"></param>
        /// <returns></returns>
        public bool UpdateChatFeeRate(TchatRate tchatRate)
        {
            var sql = "UPDATE TCHAT_RATE SET RATE = {0} WHERE TYPEID = {1} AND SENDER = {2} AND RECEIVER = {3}";
            sql = string.Format(sql, tchatRate.Rate, tchatRate.Typeid, tchatRate.Sender, tchatRate.Receiver);
            return OracleHelper.ExecuteNonQuery(sql) > 0;
        }
        /// <summary>
        ///  聊天计费倍率查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public TchatRate GetTchatRate(int sender, int receiver, int typeId)
        {
            var sql = "SELECT * FROM TCHAT_RATE WHERE SENDER={0} AND RECEIVER={1} AND TYPEID={2}";
            sql = string.Format(sql, sender, receiver, typeId);
            TchatRate tchatRate = new TchatRate();
            using (var reader = OracleHelper.ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    tchatRate.Id = Convert.ToInt32(reader["Id"]);
                    tchatRate.Typeid = Convert.ToInt32(reader["Typeid"]);
                    tchatRate.Sender = Convert.ToInt32(reader["Sender"]);
                    tchatRate.Receiver = Convert.ToInt32(reader["Receiver"]);
                    tchatRate.Rate = Convert.ToDecimal(reader["Rate"]);
                    tchatRate.Createtime = Convert.ToDateTime(reader["Createtime"]);
                    tchatRate.Remarks = reader["Remarks"].ToString();
                }
            }
            return tchatRate;
        }
        /// <summary>
        /// 获取我的好友NodeId
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public List<int> GetTchatFriendNodeIds(int nodeId)
        {
            var sql = "SELECT FriendNodeId FROM TCHAT_FRIEND WHERE MYNODEID={0}";
            sql = string.Format(sql, nodeId);
            List<int> nodeIdList = new List<int>();
            using (var reader = OracleHelper.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    nodeIdList.Add(Convert.ToInt32(reader["FriendNodeId"]));
                }
            }
            return nodeIdList;
        }
        #endregion

        /// <summary>
        /// 获取聊天计费规则
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public TappConfig GetFeeRules(int sId, string propertyName)
        {
            string sql = "SELECT * FROM TAPP_CONFIG WHERE SID={0} AND PROPERTYNAME='{1}' AND ROWNUM=1";
            sql = string.Format(sql, sId, propertyName);
            TappConfig tappConfig = new TappConfig();
            using (var reader = OracleHelper.ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    tappConfig.Propertyvalue = reader["Propertyvalue"]?.ToString();
                }
            }
            return tappConfig;
        }
        /// <summary>
        /// 根据id获取注册用户
        /// </summary>
        /// <returns></returns>
        public TnetReginfo GetReginfoByNodeid(int nodeId)
        {
            string sql = "SELECT * FROM TNET_REGINFO WHERE NODEID = {0}";
            sql = string.Format(sql, nodeId);
            TnetReginfo tnetReginfo = new TnetReginfo();
            using (var reader = OracleHelper.ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    tnetReginfo.Isenterprise = Convert.ToInt32(reader["Isenterprise"]);
                }
            }
            return tnetReginfo;
        }
    }
}
