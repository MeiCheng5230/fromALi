using System;

namespace PXin.Protocal
{
    [Serializable]
    public class MessageHeader
    {
        public const int Length = 4 + 4 + 8;
        public PXin_COMMAND_TYPE Command_Id { get; }
        public uint Total_Length { get; set; }
        public long Sequence_Id { get; set; }

        public MessageHeader(uint Total_Length, PXin_COMMAND_TYPE Command_Id, long Sequence_Id)
        {
            this.Total_Length = Total_Length;
            this.Command_Id = Command_Id;
            this.Sequence_Id = Sequence_Id;
        }

        public MessageHeader(byte[] bytes)
        {
            byte[] buffer = new byte[4];
            Buffer.BlockCopy(bytes, 0, buffer, 0, buffer.Length);
            Array.Reverse(buffer);//传输是是高位传输(协定)，由于windows是小端存储，所以需要倒置
            this.Total_Length = BitConverter.ToUInt32(buffer, 0);

            Buffer.BlockCopy(bytes, 4, buffer, 0, buffer.Length);
            Array.Reverse(buffer);
            this.Command_Id = (PXin_COMMAND_TYPE)BitConverter.ToUInt32(buffer, 0);

            buffer = new byte[8];
            Buffer.BlockCopy(bytes, 8, buffer, 0, buffer.Length);
            Array.Reverse(buffer);
            this.Sequence_Id = BitConverter.ToInt64(buffer, 0);
        }

        public byte[] ToBytes()
        {
            byte[] bytes = new byte[MessageHeader.Length];

            byte[] buffer = BitConverter.GetBytes(this.Total_Length);
            Array.Reverse(buffer);
            Buffer.BlockCopy(buffer, 0, bytes, 0, 4);

            buffer = BitConverter.GetBytes((uint)this.Command_Id);
            Array.Reverse(buffer);
            Buffer.BlockCopy(buffer, 0, bytes, 4, 4);

            buffer = BitConverter.GetBytes(this.Sequence_Id);
            Array.Reverse(buffer);
            Buffer.BlockCopy(buffer, 0, bytes, 8, 8);

            return bytes;
        }

        public override string ToString()
        {
            return string.Format("\tMessageHeader:\r\n\t\tCommand_Id:{0}\r\n\t\tSequence_Id:{1}\r\n\t\tTotal_Length:{2}",
                            this.Command_Id,
                            this.Sequence_Id,
                            this.Total_Length);
        }
    }
}
