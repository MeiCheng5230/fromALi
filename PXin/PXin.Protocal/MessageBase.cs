using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Protocal
{
    public class MessageBase<T>
        where T : class, new()
    {
        public MessageHeader Header { get; private set; }
        public T Body { get; set; }
        public MessageBase(uint Total_Length, PXin_COMMAND_TYPE Command_Id, long Sequence_Id)
        {
            this.Header = new MessageHeader(Total_Length, Command_Id, Sequence_Id);
            Body = new T();
        }
        public MessageBase(byte[] bytes)
        {
            int index = 0;

            byte[] buffer = new byte[MessageHeader.Length];
            Buffer.BlockCopy(bytes, 0, buffer, 0, MessageHeader.Length);
            Header = new MessageHeader(buffer);
            index += MessageHeader.Length;
            //body
            buffer = new byte[this.Header.Total_Length - MessageHeader.Length];
            Buffer.BlockCopy(bytes, index, buffer, 0, buffer.Length);
            //Array.Reverse(buffer);
            string body = Encoding.UTF8.GetString(buffer).Trim(new char[] { '\0' });
            this.Body = JsonConvert.DeserializeObject<T>(body, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                NullValueHandling = NullValueHandling.Ignore
            });
        }
        public byte[] ToBytes()
        {
            string bodyString = JsonConvert.SerializeObject(Body);
            byte[] bodyBytes = Encoding.UTF8.GetBytes(bodyString);
            byte[] bytes = new byte[MessageHeader.Length + bodyBytes.Length];
            int index = 0;
            Header.Total_Length = (uint)bytes.Length;
            byte[] buffer = Header.ToBytes();
            Buffer.BlockCopy(buffer, 0, bytes, 0, buffer.Length);
            index += MessageHeader.Length;
            //Array.Reverse(bodyBytes);
            Buffer.BlockCopy(bodyBytes, 0, bytes, index, bodyBytes.Length);
            return bytes;
        }
        public override string ToString()
        {
            return "[\r\n"
                + this.Header.ToString() + "\r\n"
                + string.Format("\tMessageBody:{0}]", JsonConvert.SerializeObject(Body));
        }
    }
}
