﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEnd
{
    [Serializable]

    public class QueryException : Exception
    {
        public QueryException(string message) : base(message) { }



    }
}
