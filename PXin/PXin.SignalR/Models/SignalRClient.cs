using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PXin.SignalR.Models
{
    /// <summary>
    /// 客户端管理类
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class SignalRClientManager<T1, T2>
    {
        private readonly Dictionary<T1, T2> _connections =
               new Dictionary<T1, T2>();
        private SignalRClientManager() { }
        private static SignalRClientManager<T1, T2> _SignalRClient = null;
        private static object SignalRClient_Lock = new object();
        public static SignalRClientManager<T1, T2> GetInstance()
        {
            if (_SignalRClient == null)
            {
                lock (SignalRClient_Lock)
                {
                    if (_SignalRClient == null)
                    {
                        _SignalRClient = new SignalRClientManager<T1, T2>();
                    }
                }
            }
            return _SignalRClient;
        }
        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }
        public void Add(T1 key, T2 value)
        {
            lock (_connections)
            {
                if (!_connections.TryGetValue(key, out T2 connection))
                {
                    _connections.Add(key, value);
                }
            }
        }
        public T2 GetConnection(T1 key)
        {
            if (_connections.TryGetValue(key, out T2 connection))
            {
                return connection;
            }

            return default;
        }
        public Dictionary<T1, T2> GetConnections()
        {
            return _connections;
        }
        public void Remove(T1 key)
        {
            lock (_connections)
            {
                if (!_connections.TryGetValue(key, out T2 connection))
                {
                    return;
                }

                lock (connection)
                {
                    _connections.Remove(key);
                }
            }
        }
        public void Update(T1 key, T2 value)
        {
            lock (_connections)
            {
                if (!_connections.TryGetValue(key, out T2 connection))
                {
                    Add(key, value);
                    return;
                }
                connection = value;
            }
        }
    }
}