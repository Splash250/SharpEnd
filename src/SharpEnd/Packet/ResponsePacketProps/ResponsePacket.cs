﻿using SharpEnd.Cookies;
using SharpEnd.Resources;
using SharpEnd.Utils;

namespace SharpEnd.Packet
{
    public class ResponsePacket : Packet
    {
        public ResponseStatus Status { get; set; }
        public string Body { get; set; }

        public ResponsePacket(PacketProtocol protocol, ResponseCode code, PacketHeaderCollection headers)
        {
            Protocol = protocol;
            Status = new ResponseStatus(code);
            Headers = headers;
            Body = String.Empty;
        }

        public ResponsePacket(PacketProtocol protocol, ResponseCode code, PacketHeaderCollection headers, string body)
        {
            Protocol = protocol;
            Status = new ResponseStatus(code);
            Headers = headers;
            Body = body;
        }

        public ResponsePacket(PacketProtocol protocol, ResponseCode code, PacketHeaderCollection headers, View body)
        {
            Protocol = protocol;
            Status = new ResponseStatus(code);
            Headers = headers;
            Body = body.Content;
        }


        public ResponsePacket(ResponseCode code, PacketHeaderCollection headers)
        {
            Protocol = PacketProtocol.Default;
            Status = new ResponseStatus(code);
            Headers = headers;
            Body = String.Empty;
        }

        public ResponsePacket(ResponseCode code, PacketHeaderCollection headers, string body)
        {
            Protocol = PacketProtocol.Default;
            Status = new ResponseStatus(code);
            Headers = headers;
            Body = body;
        }
        public ResponsePacket(ResponseCode code, PacketHeaderCollection headers, View body)
        {
            Protocol = PacketProtocol.Default;
            Status = new ResponseStatus(code);
            Headers = headers;
            Body = body.Content;
        }


        public ResponsePacket(string packet)
        {

            if (!packet.Contains("\r\n\r\n"))
            {
                packet += "\r\n\r\n";
            }
            ParsePacket(packet);

        }
        public static ResponsePacket HTMLResponsePacket(string htmlContent) 
        {
            return new ResponsePacket(
             PacketProtocol.Default,
             ResponseCode.OK,
             new PacketHeaderCollection(new string[] {
                "Content-Type: text/html; charset=UTF-8",
                "Content-Length: " + htmlContent.Length
             }),
             htmlContent);
        }
        public static ResponsePacket HTMLResponsePacket(View htmlView)
        {
            string content = htmlView.Content;
            return HTMLResponsePacket(content);
        }
        private void ParsePacket(string packetText) 
        {
            string[] packetParts = packetText.Split(BasicUtility.DoubleNewLineDelimiters, StringSplitOptions.None);
            string[] headerParts = packetParts[0].Split(BasicUtility.NewLineDelimiters, StringSplitOptions.None);
            string[] protocolParts = headerParts[0].Split(new string[] { " " }, StringSplitOptions.None);
            ParseProtocol(protocolParts[0]);
            ParseStatus(protocolParts[1]);
            ParseHeaders(headerParts);
            ParseBody(packetParts[1]);
        }

        public void SetCookie(Cookie cookie) 
        {
            if (Cookies == null) 
                Cookies = new CookieContainer();
            Cookies.AddCookie(cookie);
            Headers.AddHeader("Set-Cookie", cookie.ToString());
        }
        public void ApplySession(Session session)
        {
            if (session != null)
                foreach (Cookie cookie in session.ModifiedCookies)
                    SetCookie(cookie);
        }
        private void ParseProtocol(string protocolText) 
        {
            Protocol = new PacketProtocol(protocolText);
        }

        private void ParseStatus(string statusText) 
        {
            Status = new ResponseStatus((ResponseCode)Convert.ToInt32(statusText));
        }

        private void ParseHeaders(string[] headerParts) 
        {
            string[] headersArray = headerParts.Skip(1).ToArray();
            Headers = new PacketHeaderCollection(headersArray);
        }

        private void ParseBody(string bodyText) 
        {
            Body = bodyText;
        }
        //override ToString() to return the packet as a string
        public override string ToString()
        {
            string packet = String.Empty;
            packet += Protocol.Name + "/" + Protocol.Version + " " + Status.Code + " " + Status.Message + "\r\n";
            packet += Headers.ToString();
            packet += "\n";
            packet += Body;
            return packet;
        }

    }
}
