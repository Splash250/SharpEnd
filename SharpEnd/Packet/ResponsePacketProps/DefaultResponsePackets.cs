namespace SharpEnd.Packet
{
    public static class DefaultResponsePackets
    {
        public static ResponsePacket NotFoundPacket = new ResponsePacket(PacketProtocol.Default,
                                                    ResponseCode.NotFound,
                                                    new PacketHeaders(new string[] { "Content-Length: 0" }),
                                                    "");
    }
}

