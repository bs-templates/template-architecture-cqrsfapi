using ModelWrapper;

namespace BAYSOFT.Core.Application
{
    public abstract class ApplicationResponse<TEntity> : WrapResponse<TEntity>
        where TEntity : class
    {
        protected ApplicationResponse()
        {
        }

        protected ApplicationResponse(WrapRequest<TEntity> request, object data, string message = "Successful operation!", long? resultCount = null) : base(request, data, message, resultCount)
        {
        }
    }
}
