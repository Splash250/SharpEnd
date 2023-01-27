using System.Net.Sockets;
using System.Text;

namespace SharpEnd.Server
{
    internal static class NetworkUtils
    {
        public static string GetResponse(Socket responseFrom, int bufferLength)
        {
            byte[] responseBytes = new byte[bufferLength];
            _ = responseFrom.Receive(responseBytes);
            return Encoding.UTF8.GetString(responseBytes);
        }

        public static byte[] ReceiveAll(this Socket socket)
        {
            var buffer = new List<byte>();

            while (socket.Available > 0)
            {
                var currByte = new Byte[1];
                var byteCounter = socket.Receive(currByte, currByte.Length, SocketFlags.None);

                if (byteCounter.Equals(1))
                {
                    buffer.Add(currByte[0]);
                }
            }

            return buffer.ToArray();
        }

        public static async Task<byte[]> ReadAllBytesAsync(Socket client)
        {
            byte[] buffer = new byte[Utility.DefaultBufferSize];
            int bytesRead;
            using var ms = new MemoryStream();
            do
            {
                var receiveAR = client.BeginReceive(buffer, 0, buffer.Length, 0, null, null);
                bytesRead = await Task.Factory.FromAsync(receiveAR, iar => client.EndReceive(iar));
                ms.Write(buffer, 0, bytesRead);
            } while (client.Available > 0);

            return ms.ToArray();
        }

        public static async void SendBytesAsync(Socket client, byte[] bytes)
        {
            var sendAR = client.BeginSend(bytes, 0, bytes.Length, 0, null, null);
            await Task.Factory.FromAsync(sendAR, iar => client.EndSend(iar));

        }
    }
}
