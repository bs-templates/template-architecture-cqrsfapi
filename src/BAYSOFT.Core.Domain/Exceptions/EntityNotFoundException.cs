using BAYSOFT.Abstractions.Core.Domain.Entities;
using Microsoft.Extensions.Localization;
using System;
using System.Runtime.Serialization;

namespace BAYSOFT.Core.Domain.Exceptions
{
    public class EntityNotFoundException<TEntity> : EntityNotFoundException
        where TEntity : DomainEntity
    {
        public EntityNotFoundException(IStringLocalizer localizer)
            : base(localizer, typeof(TEntity).Name)

        {
        }
    }
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(IStringLocalizer localizer, string nameofEntity)
            : base(string.Format(localizer["{0} not found!"], localizer[nameofEntity]))
            
        {
        }
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
