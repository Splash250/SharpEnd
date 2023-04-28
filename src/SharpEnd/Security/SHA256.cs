using System;
using System.Text;

namespace SharpEnd.Security
{
    internal class SHA256
    {
        private uint[] hashValues;
        private uint[] kConstants;
        private uint[] wBuffer;
        private uint a, b, c, d, e, f, g, h;

        public SHA256()
        {
            hashValues = new uint[]
            {
            0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a,
            0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19
            };

            kConstants = new uint[]
            {
            0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5,
            0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
            0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3,
            0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
            0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc,
            0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
            0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7,
            0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
            0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13,
            0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
            0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3,
            0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
            0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5,
            0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
            0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208,
            0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
            };

            wBuffer = new uint[64];
        }
        private uint RotR(uint x, int n)
        {
            return (x >> n) | (x << (32 - n));
        }

        private uint Ch(uint x, uint y, uint z)
        {
            return (x & y) ^ (~x & z);
        }
        private uint Maj(uint x, uint y, uint z)
        {
            return (x & y) ^ (x & z) ^ (y & z);
        }

        private uint Sig0(uint x)
        {
            return RotR(x, 2) ^ RotR(x, 13) ^ RotR(x, 22);
        }

        private uint Sig1(uint x)
        {
            return RotR(x, 6) ^ RotR(x, 11) ^ RotR(x, 25);
        }

        private uint Gamma0(uint x)
        {
            return RotR(x, 7) ^ RotR(x, 18) ^ (x >> 3);
        }

        private uint Gamma1(uint x)
        {
            return RotR(x, 17) ^ RotR(x, 19) ^ (x >> 10);
        }

        private void ProcessBlock(byte[] buffer, int offset)
        {
            for (int i = 0; i < 16; i++)
            {
                wBuffer[i] = (uint)(
                    (buffer[offset + i * 4] << 24) |
                    (buffer[offset + i * 4 + 1] << 16) |
                    (buffer[offset + i * 4 + 2] << 8) |
                    buffer[offset + i * 4 + 3]
                );
            }

            for (int i = 16; i < 64; i++)
            {
                wBuffer[i] = Gamma1(wBuffer[i - 2]) + wBuffer[i - 7] +
                    Gamma0(wBuffer[i - 15]) + wBuffer[i - 16];
            }

            a = hashValues[0];
            b = hashValues[1];
            c = hashValues[2];
            d = hashValues[3];
            e = hashValues[4];
            f = hashValues[5];
            g = hashValues[6];
            h = hashValues[7];

            for (int i = 0; i < 64; i++)
            {
                uint t1 = h + Sig1(e) + Ch(e, f, g) + kConstants[i] + wBuffer[i];
                uint t2 = Sig0(a) + Maj(a, b, c);

                h = g;
                g = f;
                f = e;
                e = d + t1;
                d = c;
                c = b;
                b = a;
                a = t1 + t2;
            }

            hashValues[0] += a;
            hashValues[1] += b;
            hashValues[2] += c;
            hashValues[3] += d;
            hashValues[4] += e;
            hashValues[5] += f;
            hashValues[6] += g;
            hashValues[7] += h;
        }

        public byte[] ComputeHash(string input)
        {
            byte[] message = Encoding.UTF8.GetBytes(input);
            int messageLength = message.Length;
            int paddedLength = ((messageLength + 8) / 64 + 1) * 64;
            byte[] paddedMessage = new byte[paddedLength];
            Array.Copy(message, paddedMessage, messageLength);
            paddedMessage[messageLength] = 0x80;
            ulong bitLength = (ulong)messageLength * 8;
            byte[] bitLengthBytes = BitConverter.GetBytes(bitLength);
            Array.Copy(bitLengthBytes, 0, paddedMessage, paddedLength - 8, 8);
            for (int i = 0; i < paddedLength; i += 64)
            {
                ProcessBlock(paddedMessage, i);
            }

            byte[] hash = new byte[32];

            for (int i = 0; i < 8; i++)
            {
                Array.Copy(BitConverter.GetBytes(hashValues[i]), 0, hash, i * 4, 4);
            }

            return hash;
        }
        public string ComputeHashString(string input)
        {
            byte[] hash = ComputeHash(input);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
