using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEnd
{
    public static class BaseExceptions
    {
        public static Exception MalformedQueryException = new QueryException("Query was malformed");
        public static Exception MalformedMethodException = new MalformedMethodException("");
    }
}
