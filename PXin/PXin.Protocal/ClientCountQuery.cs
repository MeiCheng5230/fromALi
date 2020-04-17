using System;

namespace PXin.Protocal
{

    [Serializable]
    public class ClientCountQuery
    {
        public MessageHeader Header { get; }
        public ClientCountQuery(byte[] bytes)
        {
            this.Header = new MessageHeader(bytes);
        }
        public ClientCountQuery()
        {
            this.Header = new MessageHeader(MessageHeader.Length, PXin_COMMAND_TYPE.ClientCountQuery, Util.GetSquence());
        }
        public byte[] ToBytes()
        {
            return this.Header.ToBytes();
        }
        public override string ToString()
        {
            return this.Header.ToString();
        }
    }

    [Serializable]
    public class ClientCountQueryResp : MessageBase<ClientCountQueryRespBody>
    {
        public ClientCountQueryResp(long squeueID)
            : base(MessageHeader.Length, PXin_COMMAND_TYPE.ClientCountQueryResp, squeueID)
        {
        }
        public ClientCountQueryResp(byte[] bytes)
            : base(bytes)
        {
        }
    }

    [Serializable]
    public class ClientCountQueryRespBody
    {
        public int Count { get; set; }
        public int ValidCount { get; set; }
    }
}
