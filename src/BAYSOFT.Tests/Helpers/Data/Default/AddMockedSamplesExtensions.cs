using BAYSOFT.Core.Domain.Default.Samples.Entities;
using BAYSOFT.Infrastructures.Data.Default;
using BAYSOFT.Tests.Helpers.Data.Default.Samples;

namespace BAYSOFT.Tests.Helpers.Data.Default
{
    internal static class AddMockedSamplesExtensions
    {
        public static DefaultDbContext SetupSamples(this DefaultDbContext context, List<Sample>? entities = null)
        {
            if(entities == null)
            {
                entities = SamplesCollections.GetDefaultCollection();
            }

            context.AddRange(entities);
            context.SaveChanges();

            return context;
        }
    }
}
