using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BAYSOFT.Core.Domain.Exceptions
{
    public class BusinessException : Exception
    {
        public List<EntityException> EntityExceptions { get; set; }
        public List<DomainException> DomainExceptions { get; set; }
        public BusinessException()
        {
            EntityExceptions = new List<EntityException>();
            DomainExceptions = new List<DomainException>();
        }

        public BusinessException(string message) : base(message)
        {
            EntityExceptions = new List<EntityException>();
            DomainExceptions = new List<DomainException>();
        }

        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {
            EntityExceptions = new List<EntityException>();
            DomainExceptions = new List<DomainException>();
        }

        public BusinessException(string message, List<EntityException> entityExceptions, List<DomainException> domainExceptions) : base(message)
        {
            EntityExceptions = new List<EntityException>();
            DomainExceptions = new List<DomainException>();

            if (entityExceptions != null && entityExceptions.Count > 0)
            {
                EntityExceptions.AddRange(entityExceptions);
            }

            if (domainExceptions != null && domainExceptions.Count > 0)
            {
                DomainExceptions.AddRange(domainExceptions);
            }
        }

        protected BusinessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            EntityExceptions = new List<EntityException>();
            DomainExceptions = new List<DomainException>();
        }
    }
}
