using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.CommonService
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }
        /// <summary>
        /// 
        /// </summary>
        public static string ToJson(this object obj)
        {
            if (obj == null)
            {
                return "";
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return json;
        }
        /// <summary>
        /// 
        /// </summary>
        public static T DeserializeFromJson<T>(this string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("Json input string is null, type:" + typeof(T));
            }
            T obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(input);
            return obj;
        }
    }
}
