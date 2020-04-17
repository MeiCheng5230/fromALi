using System;

namespace PXin.Protocal
{
    public class ActiveBase
    {
        public MessageHeader Header { get; }
        public ActiveBase(byte[] bytes)
        {
            this.Header = new MessageHeader(bytes);
        }
        public ActiveBase(uint Total_Length, PXin_COMMAND_TYPE Command_Id, long Sequence_Id)
        {
            this.Header = new MessageHeader(Total_Length, Command_Id, Sequence_Id);
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
    public class Active : ActiveBase
    {
        public Active(byte[] bytes)
           : base(bytes)
        {
        }
        public Active()
            : base(MessageHeader.Length, PXin_COMMAND_TYPE.Active, Util.GetSquence())
        {
        }
    }
    [Serializable]
    public class ActiveResp : ActiveBase
    {
        public ActiveResp(byte[] bytes)
             : base(bytes)
        {
        }
        public ActiveResp(long squeueID)
             : base(MessageHeader.Length, PXin_COMMAND_TYPE.ActiveResp, squeueID)
        {
            //this.Header = new MessageHeader(MessageHeader.Length, PXin_COMMAND_TYPE.ActiveResp, squeueID);
        }
    }
}
