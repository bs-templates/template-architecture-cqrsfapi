using BAYSOFT.Core.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application
{
    public abstract class ApplicationRequestHandler<TEntity, TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : ApplicationRequest<TEntity, TResponse>
        where TResponse : ApplicationResponse<TEntity>
        where TEntity : DomainEntity
    {
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
