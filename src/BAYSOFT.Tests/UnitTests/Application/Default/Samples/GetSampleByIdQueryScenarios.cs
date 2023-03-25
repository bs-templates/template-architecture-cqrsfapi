using BAYSOFT.Core.Application.Default.Samples.Queries;
using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Infrastructures.Data.Default;
using BAYSOFT.Tests.Helpers;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;

namespace BAYSOFT.Tests.UnitTests.Application.Default.Samples
{
    [TestClass]
    public class GetSampleByIdQueryScenarios
    {
        [TestMethod]
        public async Task GET_Sample_By_Id_Should_Return_Ok()
        {
            using (var context = MockDefaultHelper.GetInMemoryDefaultDbContext().SetEntities(new List<Sample> { new Sample { Id = 1, Description = "Sample - 001" }, new Sample { Id = 2, Description = "Sample - 002" } }))
            {
                var reader = new DefaultDbContextReader(context);

                var mockedMediator = new Mock<IMediator>();

                var mockedLocalizer = new Mock<IStringLocalizer<GetSampleByIdQueryHandler>>();

                var handler = new GetSampleByIdQueryHandler(mockedLocalizer.Object, reader);

                var command = new GetSampleByIdQuery();

                command.Project(model =>
                {
                    model.Id = 1;
                });

                var result = await handler.Handle(command, default);

                Assert.AreEqual((long)HttpStatusCode.OK, result.StatusCode);
            }
        }
    }
}
