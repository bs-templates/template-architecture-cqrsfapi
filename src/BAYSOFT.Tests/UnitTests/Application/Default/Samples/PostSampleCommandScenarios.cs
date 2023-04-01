using BAYSOFT.Core.Application.Default.Samples.Commands;
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
    public class PostSampleCommandScenarios
    {
        [TestMethod]
        public async Task POST_Sample_Should_Return_Ok()
        {
            using (var context = MockDefaultHelper.GetInMemoryDefaultDbContext().SetEntities(new List<Sample> { new Sample { Id = 1, Description = "Sample - 001" }, new Sample { Id = 2, Description = "Sample - 002" } }))
            {
                var writer = new DefaultDbContextWriter(context);

                var mockedMediator = new Mock<IMediator>();

                var mockedLocalizer = new Mock<IStringLocalizer<PostSampleCommandHandler>>();

                var handler = new PostSampleCommandHandler(mockedMediator.Object, mockedLocalizer.Object, writer);

                var command = new PostSampleCommand();

                command.Project(model =>
                {
                    model.Description = "Sample - 001";
                });

                var result = await handler.Handle(command, default);

                Assert.AreEqual((long)HttpStatusCode.OK, result.StatusCode);
            }
        }
    }
}
