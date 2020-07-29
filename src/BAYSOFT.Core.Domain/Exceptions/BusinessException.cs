using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace BAYSOFT.Core.Domain.Exceptions
{
    public class BusinessException : Exception
    {
        public List<Exception> InnerExceptions { get; set; }
        public BusinessException()
        {
            InnerExceptions = new List<Exception>();
        }

        public BusinessException(string message) : base(message)
        {
            InnerExceptions = new List<Exception>();
        }

        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {
            InnerExceptions = new List<Exception>();
            InnerExceptions.Add(innerException);
        }

        public BusinessException(string message, List<Exception> innerExceptions) : base(message, innerExceptions.FirstOrDefault())
        {
            InnerExceptions = new List<Exception>();
            InnerExceptions.AddRange(innerExceptions);
        }

        protected BusinessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            InnerExceptions = new List<Exception>();
        }
    }
}
