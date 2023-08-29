using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using ModelWrapper;
using System;
using System.Collections.Generic;

namespace BAYSOFT.Core.Application.Default.Samples.Queries
{
    public class GetSamplesByFilterQueryResponse : ApplicationResponse<Sample>
    {
        public GetSamplesByFilterQueryResponse(Tuple<int, int, WrapRequest<Sample>, Dictionary<string, object>, Dictionary<string, object>, string, long?> tuple) : base(tuple)
        {
        }

        public GetSamplesByFilterQueryResponse(WrapRequest<Sample> request, object data, string message = "Successful operation!", long? resultCount = null)
            : base(request, data, message, resultCount)
        {
        }
    }
}