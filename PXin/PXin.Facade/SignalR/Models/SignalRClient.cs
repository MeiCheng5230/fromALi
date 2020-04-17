using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PXin.Facade.SignalR.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class SignalRClient<T1, T2>
    {
        private readonly Dictionary<T1, T2> _connections =
               new Dictionary<T1, T2>();

        private SignalRClient() { }
        private static SignalRClient<T1, T2> _SignalRClient = null;
        private static object SignalRClient_Lock = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SignalRClient<T1, T2> GetInstance()
        {
            if (_SignalRClient == null)
            {
                lock (SignalRClient_Lock)
                {
                    if (_SignalRClient == null)
                    {
                        _SignalRClient = new SignalRClient<T1, T2>();
                    }
                }
            }
            return _SignalRClient;
        }



        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T2 GetConnection(T1 key)
        {
            if (_connections.TryGetValue(key, out T2 connection))
            {
                return connection;
            }

            return default;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<T1, T2> GetConnections()
        {
            return _connections;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
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
