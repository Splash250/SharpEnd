using SharpEnd.Packet;
using SharpEnd.Server;
using System.Net.Sockets;
using System.Text;

namespace SharpEnd.Utils
{
    internal static class NetworkUtils
    {
        public static string GetResponse(Socket responseFrom, int bufferLength)
        {
            byte[] responseBytes = new byte[bufferLength];
            _ = responseFrom.Receive(responseBytes);
            return Encoding.UTF8.GetString(responseBytes);
        }
        public static bool SendBytes(Socket destination, byte[] bytes)
        {
            //send bytes to destination in 1024 byte chunks until all bytes are sent if the destination is not connected ot the sending fails then return false
            int sent = 0;
            while (sent < bytes.Length)
            {
                int bytesSent = destination.Send(bytes, sent, bytes.Length - sent, SocketFlags.None);
                if (bytesSent == 0)
                {
                    return false;
                }
                sent += bytesSent;
            }
            return true;

        }

        public static byte[] ReceiveAll(this Socket socket)
        {
            var buffer = new List<byte>();

            while (socket.Available > 0)
            {
                byte[] currByte = new Byte[1];
                int byteCounter = socket.Receive(currByte, currByte.Length, SocketFlags.None);

                if (byteCounter.Equals(1))
                {
                    buffer.Add(currByte[0]);
                }
            }

            return buffer.ToArray();
        }

        public static async Task<byte[]> ReadAllBytesAsync(Socket client)
        {
            byte[] buffer = new byte[BasicUtility.DefaultBufferSize];
            int bytesRead;
            using MemoryStream ms = new MemoryStream();
            do
            {
                var receiveAR = client.BeginReceive(buffer, 0, buffer.Length, 0, null, null);
                bytesRead = await Task.Factory.FromAsync(receiveAR, iar => client.EndReceive(iar));
                ms.Write(buffer, 0, bytesRead);
            } while (client.Available > 0);

            return ms.ToArray();
        }

        public static async void SendFileAsync(Socket client, string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            int chunkSize = 1024;
            int index = 0;
            while (index < fileBytes.Length)
            {
                int remaining = fileBytes.Length - index;
                int size = Math.Min(chunkSize, remaining);
                client.Send(fileBytes, index, size, SocketFlags.None);
                index += size;
            }
        }



        public static async void SendBytesAsync(Socket client, byte[] bytes)
        {
            IAsyncResult sendAR = client.BeginSend(bytes, 0, bytes.Length, 0, null, null);
            await Task.Factory.FromAsync(sendAR, iar => client.EndSend(iar));

        }

        public static async void SendResponsePacketAsync(Socket client, ResponsePacket responsePacket)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(responsePacket.ToString());
            IAsyncResult sendAR = client.BeginSend(bytes, 0, bytes.Length, 0, null, null);
            await Task.Factory.FromAsync(sendAR, iar => client.EndSend(iar));

        }


        public static bool IsFileRequest(RequestPacket requestPacket)
        {
            return BasicUtility.IsFilePath(WebServer.HTMLPath + requestPacket.Uri.Path);
        }
        public static ResponsePacket CraftFileSendHeaderPacket(int contentLength) 
        {
            return new ResponsePacket(PacketProtocol.Default,
                                        ResponseCode.OK,
                                        new PacketHeaderCollection(new string[]
                                        {
                                            "Content-Length: " + contentLength
                                        }), "");
        }
    }
}
