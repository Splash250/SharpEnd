namespace SharpEnd.Packet
{
    public static class DefaultResponsePackets
    {
        public static ResponsePacket NotFoundPacket = new(PacketProtocol.Default, ResponseCode.NotFound, new PacketHeaderCollection(new string[] { "Content-Length: 0" }), "");
        public static ResponsePacket MethodNotAllowed = new(PacketProtocol.Default, ResponseCode.MethodNotAllowed, new PacketHeaderCollection(new string[] { "Content-Length: 0" }), "");
    }
}

