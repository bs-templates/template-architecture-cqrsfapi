using BAYSOFT.Core.Domain.Default.Samples.Entities;

namespace BAYSOFT.Tests.Helpers.Data.Default.Samples
{
    public static class SamplesCollections
    {
        public static List<Sample> GetDefaultCollection()
        {
            return new List<Sample>
            {
                new Sample { Id = 1, Description = "Sample - 001" },
                new Sample { Id = 2, Description = "Sample - 002" },
                new Sample { Id = 3, Description = "Sample - 003" },
                new Sample { Id = 4, Description = "Sample - 004" },
                new Sample { Id = 5, Description = "Sample - 005" },
                new Sample { Id = 6, Description = "Sample - 006" },
                new Sample { Id = 7, Description = "Sample - 007" },
                new Sample { Id = 8, Description = "Sample - 008" },
                new Sample { Id = 9, Description = "Sample - 009" },
                new Sample { Id = 10, Description = "Sample - 010" },
            };
        }
    }
}
