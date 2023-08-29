using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Default.Samples.Entities;

namespace BAYSOFT.Core.Application.Default.Samples.Queries
{
    public class GetSamplesByFilterQuery : ApplicationRequest<Sample, GetSamplesByFilterQueryResponse>
    {
        public GetSamplesByFilterQuery()
        {
            ConfigKeys(x => x.Id);

            // ConfigSuppressedProperties(x => x.Id);
            // ConfigSuppressedResponseProperties(x => x.Id);

            //Validator.RuleFor(x => x.prop).NotEmpty().WithMessage("{0} is required!");
        }
    }
}