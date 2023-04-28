using SharpEnd.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEnd
{
    [Serializable]

    public class MalformedMethodException : Exception
    {
        public MalformedMethodException(string message) : base(message) { }

    }
}
