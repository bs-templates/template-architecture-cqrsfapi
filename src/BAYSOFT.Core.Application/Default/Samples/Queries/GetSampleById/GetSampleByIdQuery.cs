using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Default.Entities;

namespace BAYSOFT.Core.Application.Default.Samples.Queries
{
    public class GetSampleByIdQuery : ApplicationRequest<Sample, GetSampleByIdQueryResponse>
    {
        public GetSampleByIdQuery()
        {
            ConfigKeys(x => x.Id);

            ConfigSuppressedProperties(x => x.Id);

            //Validator.RuleFor(x => x.prop).NotEmpty().WithMessage("{0} is required!");
        }
    }
}