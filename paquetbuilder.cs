namespace paqs.paqet
{
    public static class PaqetBuilder
    {
        public static Paqet[] CreateMultiPacket(int virtualPort, byte type, byte[] data, int targetPacketSize)
        {
            int dataSize = data.Length;
            int currentIndex = 0;
            byte multipacketFlag = MultipacketFlagTypes.MultiPacketMoreToCome;

            int numPackets = (dataSize + targetPacketSize - 1) / targetPacketSize; // Calculate the number of packets needed
            Paqet[] packets = new Paqet[numPackets];

            for (int packetIndex = 0; packetIndex < numPackets; packetIndex++)
            {
                int remainingDataSize = dataSize - currentIndex;
                int packetDataSize = Math.Min(remainingDataSize, targetPacketSize - Paqet.HeaderSize);
                byte[] packetData = new byte[packetDataSize];
                Array.Copy(data, currentIndex, packetData, 0, packetDataSize);

                if (currentIndex + packetDataSize >= dataSize)
                {
                    multipacketFlag = MultipacketFlagTypes.MultiPacketEndOfTransmission;
                }

                packets[packetIndex] = new Paqet(virtualPort, type, multipacketFlag, packetData);

                currentIndex += packetDataSize;
            }

            return packets;
        }
    }
}
