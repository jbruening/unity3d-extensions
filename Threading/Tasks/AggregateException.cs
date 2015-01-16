using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Threading.Tasks
{
    public class AggregateException : Exception
    {
        public Exception[] InnerExceptions { get; private set; }

        public AggregateException(params Exception[] innerExceptions) : base()
        {
            InnerExceptions = innerExceptions;
        }
    }
}
