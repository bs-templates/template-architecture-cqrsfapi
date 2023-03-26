using BAYSOFT.Abstractions.Core.Domain.Entities;
using BAYSOFT.Abstractions.Core.Domain.Exceptions;
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
    public class EntityNotFoundException : BaysoftException
    {
        public EntityNotFoundException(IStringLocalizer localizer, string nameofEntity)
            : base(404, 4040000, string.Format(localizer["{0} not found!"], localizer[nameofEntity]))

        {
        }
        public EntityNotFoundException() : base(404, 4040000)
        {
        }

        public EntityNotFoundException(string message)
            : base(404, 4040000, message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException)
            : base(404, 4040000, message, innerException)
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(404, 4040000, info, context)
        {
        }
        public EntityNotFoundException(int exceptionCode, int exceptionInternalCode, IStringLocalizer localizer, string nameofEntity)
            : base(exceptionCode, exceptionInternalCode, string.Format(localizer["{0} not found!"], localizer[nameofEntity]))

        {
        }
        public EntityNotFoundException(int exceptionCode, int exceptionInternalCode)
            : base(exceptionCode, exceptionInternalCode)
        {
        }

        public EntityNotFoundException(int exceptionCode, int exceptionInternalCode, string message)
            : base(exceptionCode, exceptionInternalCode, message)
        {
        }

        public EntityNotFoundException(int exceptionCode, int exceptionInternalCode, string message, Exception innerException)
            : base(exceptionCode, exceptionInternalCode, message, innerException)
        {
        }

        protected EntityNotFoundException(int exceptionCode, int exceptionInternalCode, SerializationInfo info, StreamingContext context)
            : base(exceptionCode, exceptionInternalCode, info, context)
        {
        }
    }
}
