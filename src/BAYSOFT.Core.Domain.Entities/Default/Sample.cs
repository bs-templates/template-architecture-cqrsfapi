using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Domain.Entities.Default
{
    public class Sample : DomainEntity
    {
        public int SampleID { get; set; }
        public string Description { get; set; }
        public Sample()
        {
        }
    }
}
