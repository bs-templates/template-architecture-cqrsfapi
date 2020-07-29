using BAYSOFT.Core.Domain.Entities;
using BAYSOFT.Core.Domain.Exceptions;
using BAYSOFT.Core.Domain.Interfaces.Services;
using FluentValidation;
using NetDevPack.Specification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Domain.Services
{
    public abstract class DomainService<TEntity> : IDomainService<TEntity>
        where TEntity : DomainEntity
    {
        private AbstractValidator<TEntity> EntityValidator { get; set; }
        private SpecValidator<TEntity> DomainValidator { get; set; }
        public DomainService(AbstractValidator<TEntity> entityValidator, SpecValidator<TEntity> domainValidator)
        {
            EntityValidator = entityValidator;
            DomainValidator = domainValidator;
        }

        protected void ValidateEntity(TEntity entity)
        {
            var validateResult = EntityValidator.Validate(entity);
            if (!validateResult.IsValid)
            {
                var innerExceptions = new List<Exception>();

                foreach (var error in validateResult.Errors)
                {
                    var errorMessage = string.Format(error.ErrorMessage, error.PropertyName);
                    innerExceptions.Add(new BusinessException(errorMessage));
                }

                var exception = new BusinessException("Operation failed in entity validation!", innerExceptions);

                throw exception;
            }
        }

        protected void ValidateDomain(TEntity entity)
        {
            var validateResult = DomainValidator.Validate(entity);
            if (!validateResult.IsValid)
            {
                var innerExceptions = new List<Exception>();

                foreach (var error in validateResult.Errors)
                {
                    var errorMessage = string.Format(error.ErrorMessage, error.PropertyName);
                    innerExceptions.Add(new BusinessException(errorMessage));
                }

                var exception = new BusinessException("Operation failed in domain validation!", innerExceptions);

                throw exception;
            }
        }
        public abstract Task Run(TEntity entity);
    }
}
