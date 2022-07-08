using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Abstractions.Core.Domain.Entities;
using BAYSOFT.Abstractions.Core.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ModelWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Presentations.WebAPI.Abstractions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
        public async Task<ActionResult<TResponse>> Send<TEntity, TResponse>(ApplicationRequest<TEntity, TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
            where TEntity : DomainEntity
            where TResponse : ApplicationResponse<TEntity>
        {
            try
            {
                return Ok(await Mediator.Send(request, cancellationToken));
            }
            catch (BusinessException bex)
            {
                return BadRequest(new WrapResponse(400, 4001001, request.RequestObject, MapBusinessExceptionToDictionary(bex), bex.Message, 0));
            }
            catch (Exception ex)
            {
                return BadRequest(new WrapResponse(400, 4001001, request.RequestObject, ex.InnerException, ex.Message, 0));
            }
        }

        public async Task<TResponse> SendRequest<TEntity, TResponse>(ApplicationRequest<TEntity, TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
            where TEntity : DomainEntity
            where TResponse : ApplicationResponse<TEntity>
        {
            return await Mediator.Send(request, cancellationToken);
        }

        private Dictionary<string, object> MapBusinessExceptionToDictionary(BusinessException businessException)
        {
            Dictionary<string, object> exceptionDictionary = new Dictionary<string, object>();

            exceptionDictionary.Add("message", businessException.Message);

            if (businessException.RequestExceptions != null && businessException.RequestExceptions.Count > 0)
            {
                Dictionary<string, object> requestExceptionDictionary = new Dictionary<string, object>();

                foreach (var group in businessException.RequestExceptions.GroupBy(exception => exception.SourceProperty))
                {
                    requestExceptionDictionary.Add(group.Key, businessException.RequestExceptions.Where(exception => exception.SourceProperty.Equals(group.Key)).Select(exception => exception.Message).ToArray());
                }

                exceptionDictionary.Add("requestExceptions", requestExceptionDictionary);
            }

            if (businessException.EntityExceptions != null && businessException.EntityExceptions.Count > 0)
            {
                Dictionary<string, object> entityExceptionDictionary = new Dictionary<string, object>();

                foreach (var group in businessException.EntityExceptions.GroupBy(x => x.SourceProperty))
                {
                    entityExceptionDictionary.Add(group.Key, businessException.EntityExceptions.Where(exception => exception.SourceProperty.Equals(group.Key)).Select(x => x.Message).ToArray());
                }

                exceptionDictionary.Add("entityExceptions", entityExceptionDictionary);
            }

            if (businessException.DomainExceptions != null && businessException.DomainExceptions.Count > 0)
            {
                exceptionDictionary.Add(
                    "domainExceptions",
                    businessException.DomainExceptions
                        .Select(exception => exception.Message)
                        .ToArray()
                );
            }

            return exceptionDictionary;
        }
    }
}
