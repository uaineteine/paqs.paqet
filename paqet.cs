namespace paqs.paqet
{
    using Newtonsoft.Json;
    using System;

    public static class MultipacketFlagTypes
    {
        public const byte SinglePacket = 0;
        public const byte MultiPacketMoreToCome = 1;
        public const byte MultiPacketEndOfTransmission = 2;
    }

    public class Paqet
    {
        public const int paqsmetaver = 1;
        public const int HeaderSize = 10; // Size of the header in bytes (4 bytes for VirtualPort + 1 byte for Type + 1 byte for MultipacketFlag + 4 bytes for DataSize)

        public int VirtualPort { get; set; }
        public byte Type { get; set; }
        public byte MultipacketFlag { get; set; }
        public int DataSize { get; private set; }
        public byte[] Data { get; set; }

        public int PacketSize => HeaderSize + DataSize;

        public Paqet(int virtualPort, byte type, byte multipacketFlag, byte[] data)
        {
            VirtualPort = virtualPort;
            Type = type;
            MultipacketFlag = multipacketFlag;
            Data = data;
            DataSize = data.Length;
        }

        public byte[] ToBytes()
        {
            byte[] packetBytes = new byte[HeaderSize + Data.Length];
            BitConverter.GetBytes(VirtualPort).CopyTo(packetBytes, 0);
            packetBytes[4] = Type;
            packetBytes[5] = MultipacketFlag;
            BitConverter.GetBytes(DataSize).CopyTo(packetBytes, 6);
            Array.Copy(Data, 0, packetBytes, HeaderSize, Data.Length);
            return packetBytes;
        }

        public static Paqet FromBytes(byte[] packetBytes)
        {
            if (packetBytes.Length >= HeaderSize)
            {
                int virtualPort = BitConverter.ToInt32(packetBytes, 0);
                byte type = packetBytes[4];
                byte multipacketFlag = packetBytes[5];
                int dataSize = BitConverter.ToInt32(packetBytes, 6);
                byte[] data = new byte[packetBytes.Length - HeaderSize];
                Array.Copy(packetBytes, HeaderSize, data, 0, data.Length);
                return new Paqet(virtualPort, type, multipacketFlag, data);
            }
            else
            {
                throw new ArgumentException("Invalid packet data");
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Paqet FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Paqet>(json);
        }
    }
}
