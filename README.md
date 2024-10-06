# Packet Library v1.0.1

This library provides functionalities to create and parse packets represented as byte arrays. The header structure of the packets is defined as follows:

## Header Structure

### Size
The header size is `10` bytes in total.

### Fields
1. **VirtualPort** (`int`: 4 bytes)
   - Represents the virtual port.
   
2. **Type** (`byte`: 1 byte)
   - Defines the type of the packet.
   
3. **MultipacketFlag** (`byte`: 1 byte)
   - Indicates the packet type:
     - `0` - Single Packet
     - `1` - Multi-Packet (more to come)
     - `2` - Multi-Packet (end of transmission)
     
4. **DataSize** (`int`: 4 bytes)
   - Represents the size of the data payload.

## Packet Creation

To create a packet:

1. Instantiate a `Paqet` object with the necessary parameters: `VirtualPort`, `Type`, `MultipacketFlag`, and `Data`.
2. Use the `ToBytes()` method to convert the packet into a byte array.

## Packet Parsing

To parse a byte array into a `Paqet` object:

1. Use the `FromBytes(byte[] packetBytes)` method, providing the byte array representing the packet.
2. It reconstructs a `Paqet` object from the byte array.

## Example Usage

```csharp
// Example usage of the Paqet class
using System;

namespace YourNamespace
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creating a packet
            int virtualPort = 1234;
            byte type = 5;
            byte multipacketFlag = MultipacketFlagTypes.SinglePacket;
            byte[] data = new byte[] { 0x01, 0x02, 0x03 }; // Example data

            Paqet packet = new Paqet(virtualPort, type, multipacketFlag, data);
            byte[] packetBytes = packet.ToBytes();

            // Parsing a packet
            byte[] receivedBytes = /* Your received byte array */;
            Paqet receivedPacket = Paqet.FromBytes(receivedBytes);

            // Accessing packet properties
            Console.WriteLine($"Received Packet - VirtualPort: {receivedPacket.VirtualPort}, Type: {receivedPacket.Type}, MultipacketFlag: {receivedPacket.MultipacketFlag}, DataSize: {receivedPacket.DataSize}");
            Console.WriteLine("Data:");
            foreach (byte b in receivedPacket.Data)
            {
                Console.Write($"{b:X} ");
            }
        }
    }
}
```
