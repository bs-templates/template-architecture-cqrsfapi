using BAYSOFT.CLI.Models;
using Pluralize.NET.Core;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;

namespace BAYSOFT.CLI.Templates
{
    public static class FileExtensions
    {
        public static void GenerateEntityFile(this Entity entity)
        {
            var path = @$"src\{entity.Context.Project.Name}.Core.Domain\{entity.Context.Name}\{entity.Name.Pluralize()}\Entities";
            var filePath = Path.Combine(path, $"{entity.Name}.cs");
            var source = @"using BAYSOFT.Abstractions.Core.Domain.Entities;
// TODO: RELATED ENTITIES REFERENCES
// TODO: RELATED COLLECTIONS REFERENCES
using System.Collections.Generic;

namespace BAYSOFT.Core.Domain.Default.Samples.Entities
{
    public class Sample : DomainEntity<int>
    {
        // TODO: PROPERTIES
        
        // TODO: RELATED ENTITIES
        
        // TODO: RELATED COLLECTIONS
        
        public Sample()
        {
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace(
                        "DomainEntity<int>",
                        $"DomainEntity<{string.Join(
                            ", ",
                            entity.Properties
                                .Where(x => x.IsPrimaryKey)
                                .Select(x => x.Type)
                                .ToList())}>"
                    )
                    .Replace("BAYSOFT.Core", $"{entity.Context.Project.Name}.Core")
                    .Replace("Default", entity.Context.Name)
                    .Replace("Samples", entity.Name.Pluralize())
                    .Replace("Sample", entity.Name)
                    .ConditionalReplace(
                        entity.Properties.Any(),
                        "// TODO: PROPERTIES",
                        $"{string.Join(
                            "\n\t\t",
                            entity.Properties
                                .Where(x => !x.IsPrimaryKey)
                                .Select(x => string.Concat($"public {x.Type}{(x.IsNullable ? "?" : "")} {x.Name}", " { get; set; }"))
                                .ToList())}"
                    )
                    .ConditionalReplace(
                        entity.Properties.Where(x => x.IsForeignKey && string.IsNullOrWhiteSpace(x.RelatedEntityName)).Any(),
                        "// TODO: RELATED ENTITIES REFERENCES",
                        $"{string.Join(
                            "\n",
                            entity.Properties
                                .Where(x => x.IsForeignKey && string.IsNullOrWhiteSpace(x.RelatedEntityName))
                                .Select(x => string.Concat($"using BAYSOFT.Core.Domain.Default.{x.RelatedEntityName.Pluralize()}.Entities"))
                                .ToList())}"
                    )
                    .ConditionalReplace(
                        entity.Properties.Where(x => x.IsForeignKey && !string.IsNullOrWhiteSpace(x.RelatedEntityName)).Any(),
                        "// TODO: RELATED ENTITIES",
                        $"{string.Join(
                            "\n\t\t",
                            entity.Properties
                                .Where(x => x.IsForeignKey && !string.IsNullOrWhiteSpace(x.RelatedEntityName))
                                .Select(x => string.Concat($"public virtual {x.RelatedEntityName} {x.RelatedEntityName}", " { get; set; }"))
                                .ToList())}"
                    )
                    .ConditionalReplace(
                        entity.Context.Entities.Where(x => x.Properties.Any(property => property.IsForeignKey && !string.IsNullOrWhiteSpace(property.RelatedEntityName) && property.RelatedEntityName.Equals(entity.Name))).Any(),
                        "// TODO: RELATED ENTITIES REFERENCES",
                        $"{string.Join(
                            "\n",
                            entity.Context.Entities
                                .Where(x => x.Properties.Any(property => property.IsForeignKey && !string.IsNullOrWhiteSpace(property.RelatedEntityName) && property.RelatedEntityName.Equals(entity.Name)))
                                .Select(x => string.Concat($"using BAYSOFT.Core.Domain.Default.{x.Name.Pluralize()}.Entities"))
                                .ToList())}"
                    )
                    .ConditionalReplace(
                        entity.Context.Entities.Where(x => x.Properties.Any(property => property.IsForeignKey && !string.IsNullOrWhiteSpace(property.RelatedEntityName) && property.RelatedEntityName.Equals(entity.Name))).Any(),
                        "// TODO: RELATED COLLECTIONS",
                        $"{string.Join(
                            "\n\t\t",
                            entity.Context.Entities
                                .Where(x => x.Properties.Any(property => property.IsForeignKey && !string.IsNullOrWhiteSpace(property.RelatedEntityName) && property.RelatedEntityName.Equals(entity.Name)))
                                .Select(x => string.Concat($"public virtual ICollection<{x.Name}> {x.Name.Pluralize()}", " { get; set; }"))
                                .ToList())}"
                    )
            );
        }
        public static void GenerateEntityValidationFile(this Entity entity)
        {
            var path = @$"src\{entity.Context.Project.Name}.Core.Domain\{entity.Context.Name}\{entity.Name.Pluralize()}\Validations\EntityValidations";
            var filePath = Path.Combine(path, $"{entity.Name}Validator.cs");
            var source = @"using BAYSOFT.Abstractions.Core.Domain.Entities.Validations;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using FluentValidation;

namespace BAYSOFT.Core.Domain.Default.Samples.Validations.EntityValidations
{
    public class SampleValidator : EntityValidator<Sample>
    {
        public SampleValidator()
        {
            // TODO: RULES
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace(
                        "EntityValidator<Sample>",
                        $"EntityValidator<{entity.Name}>"
                    )
                    .Replace("BAYSOFT.Core", $"{entity.Context.Project.Name}.Core")
                    .Replace("Default", entity.Context.Name)
                    .Replace("Samples", entity.Name.Pluralize())
                    .Replace("Sample", entity.Name)
                    .ConditionalReplace(
                        entity.ValidationRules.Any(),
                        "// TODO: RULES",
                        $"{string.Join(
                            "\n\t\t\t",
                            entity.ValidationRules
                                .Select(x => x.GenerateRule())
                                .ToList())}"
                    )
            );
        }
        public static void GenerateSpecificationFile(this Specification specification)
        {
            var path = @$"src\{specification.Entity.Context.Project.Name}.Core.Domain\{specification.Entity.Context.Name}\{specification.Entity.Name.Pluralize()}\Specifications";
            var filePath = Path.Combine(path, $"{specification.Name}Specification.cs");
            var source = @"using BAYSOFT.Abstractions.Core.Domain.Entities.Specifications;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BAYSOFT.Core.Domain.Default.Samples.Specifications
{
    public class SampleDescriptionAlreadyExistsSpecification : DomainSpecification<Sample>
    {
        private IDefaultDbContextReader Reader { get; set; }
        public SampleDescriptionAlreadyExistsSpecification(IDefaultDbContextReader reader)
        {
            Reader = reader;
            SpecificationMessage = ""A register with this description already exists!"";
        }

        public override Expression<Func<Sample, bool>> ToExpression()
        {
            return true;
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace(
                        "DomainSpecification<Sample>",
                        $"DomainSpecification<{specification.Entity.Name}>"
                    )
                    .Replace("SampleDescriptionAlreadyExistsSpecification", $"{specification.Name}Specification")
                    .Replace("BAYSOFT.Core", $"{specification.Entity.Context.Project.Name}.Core")
                    .Replace("Default", specification.Entity.Context.Name)
                    .Replace("Samples", specification.Entity.Name.Pluralize())
                    .Replace("Sample", specification.Entity.Name)
                    .Replace("A register with this description already exists!", $"{specification.Message}")
                    .ConditionalReplace(
                        !string.IsNullOrWhiteSpace(specification.Rule),
                        "return true;",
                        $"{specification.Rule}"
                    )
            );
        }
        public static void GenerateSpecificationsValidatorFile(this Service service)
        {
            var path = @$"src\{service.Entity.Context.Project.Name}.Core.Domain\{service.Entity.Context.Name}\{service.Entity.Name.Pluralize()}\Validations\DomainValidations";
            var filePath = Path.Combine(path, $"{service.Name}SpecificationsValidator.cs");
            var source = @"using BAYSOFT.Abstractions.Core.Domain.Entities.Validations;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using BAYSOFT.Core.Domain.Default.Samples.Specifications;

namespace BAYSOFT.Core.Domain.Default.Samples.Validations.DomainValidations
{
    public class CreateSampleSpecificationsValidator : DomainValidator<Sample>
    {
        public CreateSampleSpecificationsValidator(
            // TODO: SPECIFICATIONS
        )
        {
            // TODO: ADD SPECIFICATIONS
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace(
                        "DomainValidator<Sample>",
                        $"DomainValidator<{service.Entity.Name}>"
                    )
                    .Replace("CreateSampleSpecificationsValidator", $"{service.Name}SpecificationsValidator")
                    .Replace("BAYSOFT.Core", $"{service.Entity.Context.Project.Name}.Core")
                    .Replace("Default", service.Entity.Context.Name)
                    .Replace("Samples", service.Entity.Name.Pluralize())
                    .Replace("Sample", service.Entity.Name)
                    .ConditionalReplace(
                        service.Specifications.Any(),
                        "// TODO: SPECIFICATIONS",
                        $"{string.Join(
                            ",\n\t\t\t",
                            service.Specifications
                                .Select(specification => $"{specification.Name}Specification ${specification.Name.ToCamelCase()}Specification")
                                .ToList())}"

                    )
                    .ConditionalReplace(
                        service.Specifications.Any(),
                        "// TODO: ADD SPECIFICATIONS",
                        $"{string.Join(
                            ",\n\t\t\t",
                            service.Specifications
                                .Select(specification => $"Add(nameof({specification.Name.ToCamelCase()}Specification), new DomainRule<{specification.Entity.Name}>({specification.Name.ToCamelCase()}Specification, {specification.Name.ToCamelCase()}Specification.ToString()));")
                                .ToList()
                            )}"
                    )
            );
        }
        public static void GenerateServiceRequestFile(this Service service)
        {
            var path = @$"src\{service.Entity.Context.Project.Name}.Core.Domain\{service.Entity.Context.Name}\{service.Entity.Name.Pluralize()}\Services\{service.Name}";
            var filePath = Path.Combine(path, $"{service.Name}ServiceRequest.cs");
            var source = @"using BAYSOFT.Abstractions.Core.Domain.Entities.Services;
using BAYSOFT.Core.Domain.Default.Samples.Entities;

namespace BAYSOFT.Core.Domain.Default.Samples.Services
{
    public class CreateSampleServiceRequest : DomainServiceRequest<Sample>
    {
        public CreateSampleServiceRequest(Sample payload) : base(payload)
        {
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("CreateSampleServiceRequest", $"{service.Name}ServiceRequest")
                    .Replace(
                        "DomainServiceRequest<Sample>",
                        $"DomainServiceRequest<{service.Entity.Name}>"
                    )
                    .Replace("BAYSOFT.Core", $"{service.Entity.Context.Project.Name}.Core")
                    .Replace("Default", service.Entity.Context.Name)
                    .Replace("Samples", service.Entity.Name.Pluralize())
                    .Replace("Sample", service.Entity.Name)
            );
        }
        public static void GenerateServiceRequestHandlerFile(this Service service)
        {
            var path = @$"src\{service.Entity.Context.Project.Name}.Core.Domain\{service.Entity.Context.Name}\{service.Entity.Name.Pluralize()}\Services\{service.Name}";
            var filePath = Path.Combine(path, $"{service.Name}ServiceRequestHandler.cs");
            var source = @"using BAYSOFT.Abstractions.Core.Domain.Entities.Services;
using BAYSOFT.Abstractions.Crosscutting.InheritStringLocalization;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Resources;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using BAYSOFT.Core.Domain.Default.Samples.Resources;
using BAYSOFT.Core.Domain.Default.Samples.Validations.DomainValidations;
using BAYSOFT.Core.Domain.Default.Samples.Validations.EntityValidations;
using BAYSOFT.Core.Domain.Resources;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Domain.Default.Samples.Services.CreateSample
{
    [InheritStringLocalizer(typeof(Messages), Priority = 0)]
    [InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
    [InheritStringLocalizer(typeof(EntitiesSamples), Priority = 2)]
    public class CreateSampleServiceRequestHandler
        : DomainServiceRequestHandler<Sample, CreateSampleServiceRequest>
    {
        private IDefaultDbContextWriter Writer { get; set; }
        public CreateSampleServiceRequestHandler(
            IDefaultDbContextWriter writer,
            IStringLocalizer<CreateSampleServiceRequestHandler> localizer,
            SampleValidator entityValidator,
            CreateSampleSpecificationsValidator domainValidator
        ) : base(localizer, entityValidator, domainValidator)
        {
            Writer = writer;
        }
        public override async Task<Sample> Handle(CreateSampleServiceRequest request, CancellationToken cancellationToken)
        {
            ValidateEntity(request.Payload);

            ValidateDomain(request.Payload);

            await Writer.AddAsync(request.Payload);

            return request.Payload;
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace(
                        "DomainServiceRequestHandler<Sample, CreateSampleServiceRequest>",
                        $"DomainServiceRequestHandler<{service.Entity.Name}, {service.Name}ServiceRequest>"
                    )
                    .Replace("CreateSample", $"{service.Name}")
                    .Replace("BAYSOFT.Core", $"{service.Entity.Context.Project.Name}.Core")
                    .Replace("Default", service.Entity.Context.Name)
                    .Replace("Samples", service.Entity.Name.Pluralize())
                    .Replace("Sample", service.Entity.Name)
            );
        }
        public static void GenerateCommandFile(this Command command)
        {
            var path = @$"src\{command.Entity.Context.Project.Name}.Core.Application\{command.Entity.Context.Name}\{command.Entity.Name.Pluralize()}\Commands\{command.Name}";
            var filePath = Path.Combine(path, $"{command.Name}Command.cs");
            var source = @"using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using FluentValidation;

namespace BAYSOFT.Core.Application.Default.Samples.Commands
{
    public class PostSampleCommand : ApplicationRequest<Sample, PostSampleCommandResponse>
    {
        public PostSampleCommand()
        {
            ConfigKeys(x => x.Id);

            // TODO: SUPPRESSED PROPERTIES
            // TODO: SUPPRESSED RESPONSE PROPERTIES

            // TODO: COMMAND RULES
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace(
                        "ApplicationRequest<Sample, PostSampleCommandResponse>",
                        $"ApplicationRequest<{command.Entity.Name}, {command.Name}CommandResponse>"
                    )
                    .Replace("PostSample", $"{command.Name}")
                    .Replace("BAYSOFT.Core", $"{command.Entity.Context.Project.Name}.Core")
                    .Replace("Default", command.Entity.Context.Name)
                    .Replace("Samples", command.Entity.Name.Pluralize())
                    .Replace("Sample", command.Entity.Name)
                    .Replace(
                        "ConfigKeys(x => x.Id);",
                        $"{string.Join(
                            "\n\t\t\t",
                            command.Entity.Properties.Where(p => p.IsPrimaryKey).Select(p => $"ConfigKeys(x => x.{p.Name});"))}"
                    )
                    .Replace(
                        "// TODO: SUPPRESSED PROPERTIES",
                        $"{string.Join(
                            "\n\t\t\t",
                            command.Entity.Properties
                                .Where(p =>
                                    p.IsPrimaryKey
                                    || command.Entity.Context.Entities
                                        .Any(e => p.Type.Contains(e.Name)))
                                .Select(p => $"ConfigSuppressedProperties(x => x.{p.Name});")
                                .ToList())}"
                    )
                    .ConditionalReplace(
                        command.Entity.Properties.Where(x => x.IsForeignKey && !string.IsNullOrWhiteSpace(x.RelatedEntityName)).Any()
                        || command.Entity.Context.Entities.Where(x => x.Properties.Any(property => property.IsForeignKey && !string.IsNullOrWhiteSpace(property.RelatedEntityName) && property.RelatedEntityName.Equals(command.Entity.Name))).Any(),
                        "// TODO: SUPPRESSED RESPONSE PROPERTIES",
                        $"{string.Join(
                            "\n\t\t\t",
                            command.Entity.Properties
                                .Where(x => x.IsForeignKey && !string.IsNullOrWhiteSpace(x.RelatedEntityName))
                                .Select(x => $"ConfigSuppressedResponseProperties(x => x.{x.RelatedEntityName});")
                                .ToList()
                                .Union(
                                    command.Entity.Context.Entities
                                        .Where(x => x.Properties.Any(property => property.IsForeignKey && !string.IsNullOrWhiteSpace(property.RelatedEntityName) && property.RelatedEntityName.Equals(command.Entity.Name)))
                                        .Select(x => $"ConfigSuppressedResponseProperties(x => x.{x.Name.Pluralize()});")
                                        .ToList()))}"
                    )
                    .ConditionalReplace(
                        command.ValidationRules.Any(),
                        "// TODO: COMMAND RULES",
                        $"{string.Join(
                            "\n\t\t\t",
                            command.ValidationRules
                                .Select(cr => $"Validator.{cr.GenerateRule()}")
                                .ToList())}"
                    )
            );
        }
        public static void GenerateCommandResponseFile(this Command command)
        {
            var path = @$"src\{command.Entity.Context.Project.Name}.Core.Application\{command.Entity.Context.Name}\{command.Entity.Name.Pluralize()}\Commands\{command.Name}";
            var filePath = Path.Combine(path, $"{command.Name}CommandResponse.cs");
            var source = @"using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using ModelWrapper;
using System;
using System.Collections.Generic;

namespace BAYSOFT.Core.Application.Default.Samples.Commands
{
    public class PostSampleCommandResponse : ApplicationResponse<Sample>
    {
        public PostSampleCommandResponse(Tuple<int, int, WrapRequest<Sample>, Dictionary<string, object>, Dictionary<string, object>, string, long?> tuple) : base(tuple)
        {
        }

        public PostSampleCommandResponse(WrapRequest<Sample> request, object data, string message = ""Successful operation!"", long? resultCount = null)
            : base(request, data, message, resultCount)
        {
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace(
                        "ApplicationResponse<Sample>",
                        $"ApplicationResponse<{command.Entity.Name}>"
                    )
                    .Replace("PostSample", $"{command.Name}")
                    .Replace("BAYSOFT.Core", $"{command.Entity.Context.Project.Name}.Core")
                    .Replace("Default", command.Entity.Context.Name)
                    .Replace("Samples", command.Entity.Name.Pluralize())
                    .Replace("Sample", command.Entity.Name)
            );
        }
        public static void GenerateCommandHandlerFile(this Command command)
        {
            var path = @$"src\{command.Entity.Context.Project.Name}.Core.Application\{command.Entity.Context.Name}\{command.Entity.Name.Pluralize()}\Commands\{command.Name}";
            var filePath = Path.Combine(path, $"{command.Name}CommandHandler.cs");
            var source = @"using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Abstractions.Crosscutting.Helpers;
using BAYSOFT.Abstractions.Crosscutting.InheritStringLocalization;
using BAYSOFT.Core.Application.Default.Samples.Notifications;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Resources;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using BAYSOFT.Core.Domain.Default.Samples.Resources;
using BAYSOFT.Core.Domain.Default.Samples.Services;
using BAYSOFT.Core.Domain.Resources;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ModelWrapper.Extensions.Post;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Commands
{

    [InheritStringLocalizer(typeof(Messages), Priority = 0)]
    [InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
    [InheritStringLocalizer(typeof(EntitiesSamples), Priority = 2)]
    public class PostSampleCommandHandler : ApplicationRequestHandler<Sample, PostSampleCommand, PostSampleCommandResponse>
    {
        private ILoggerFactory Logger { get; set; }
        private IMediator Mediator { get; set; }
        private IStringLocalizer Localizer { get; set; }
        private IDefaultDbContextWriter Writer { get; set; }
        public PostSampleCommandHandler(
            ILoggerFactory logger,
            IMediator mediator,
            IStringLocalizer<PostSampleCommandHandler> localizer,
            IDefaultDbContextWriter writer
        )
        {
            Logger = logger;
            Mediator = mediator;
            Localizer = localizer;
            Writer = writer;
        }
        public override async Task<PostSampleCommandResponse> Handle(PostSampleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                request.IsValid(Localizer, true);

                var data = request.Post();

                await Mediator.Send(new CreateSampleServiceRequest(data));

                await Writer.CommitAsync(cancellationToken);

                await Mediator.Publish(new PostSampleNotification(data));

                return new PostSampleCommandResponse(request, data, Localizer[""Successful operation!""], 1);
            }
            catch (Exception exception)
            {
                Logger.CreateLogger<PostSampleCommandHandler>().Log(LogLevel.Error, exception, exception.Message);

                return new PostSampleCommandResponse(ExceptionResponseHelper.CreateTuple(Localizer, request, exception));
            }
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace(
                        "ApplicationRequestHandler<Sample, PostSampleCommand, PostSampleCommandResponse>",
                        $"ApplicationRequestHandler<{command.Entity.Name}, {command.Name}Command, {command.Name}CommandResponse>"
                    )
                    .ConditionalReplace(
                        command.Service != null,
                        "await Mediator.Send(new CreateSampleServiceRequest(data));",
                        $"await Mediator.Send(new {command.Name}ServiceRequest(data));"
                    )
                    .ConditionalReplace(
                        command.Name.StartsWith("Put"),
                        "var data = request.Post();",
                        $"var id = request.Project(x => x.Id);\\n\\n\\t\\t\\t\\tvar data = await Writer\\n\\t\\t\\t\\t\\t.Query<{command.Entity.Name}>()\\n\\t\\t\\t\\t\\t.SingleOrDefaultAsync(x => x.Id == id);\\n\\n\\t\\t\\t\\tif (data == null)\\n\\t\\t\\t\\t{{\\n\\t\\t\\t\\t\\tthrow new EntityNotFoundException<{command.Entity.Name}>(Localizer);\\n\\t\\t\\t\\t}}\\n\\n\\t\\t\\t\\trequest.Put(data);"
                    )
                    .ConditionalReplace(
                        command.Name.StartsWith("Patch"),
                        "var data = request.Post();",
                        $"var id = request.Project(x => x.Id);\\n\\n\\t\\t\\t\\tvar data = await Writer\\n\\t\\t\\t\\t\\t.Query<{command.Entity.Name}>()\\n\\t\\t\\t\\t\\t.SingleOrDefaultAsync(x => x.Id == id);\\n\\n\\t\\t\\t\\tif (data == null)\\n\\t\\t\\t\\t{{\\n\\t\\t\\t\\t\\tthrow new EntityNotFoundException<{command.Entity.Name}>(Localizer);\\n\\t\\t\\t\\t}}\\n\\n\\t\\t\\t\\trequest.Patch(data);"
                    )
                    .ConditionalReplace(
                        command.Name.StartsWith("Delete"),
                        "var data = request.Post();",
                        $"var id = request.Project(x => x.Id);\\n\\n\\t\\t\\t\\tvar data = await Writer\\n\\t\\t\\t\\t\\t.Query<{command.Entity.Name}>()\\n\\t\\t\\t\\t\\t.SingleOrDefaultAsync(x => x.Id == id);\\n\\n\\t\\t\\t\\tif (data == null)\\n\\t\\t\\t\\t{{\\n\\t\\t\\t\\t\\tthrow new EntityNotFoundException<{command.Entity.Name}>(Localizer);\\n\\t\\t\\t\\t}}\\n\\t\\t\\t\\t"
                    )
                    .Replace("PostSample", $"{command.Name}")
                    .Replace("BAYSOFT.Core", $"{command.Entity.Context.Project.Name}.Core")
                    .Replace("Default", command.Entity.Context.Name)
                    .Replace("Samples", command.Entity.Name.Pluralize())
                    .Replace("Sample", command.Entity.Name)
            );
        }
        public static void GenerateNotificationFile(this Command command)
        {
            var path = @$"src\{command.Entity.Context.Project.Name}.Core.Application\{command.Entity.Context.Name}\{command.Entity.Name.Pluralize()}\Notifications\{command.Name}";
            var filePath = Path.Combine(path, $"{command.Name}Notification.cs");
            var source = @"using BAYSOFT.Core.Domain.Default.Samples.Entities;
using MediatR;
using System;
   
namespace BAYSOFT.Core.Application.Default.Samples.Notifications
{
    public class PostSampleNotification : INotification
    {
        public Sample Payload { get; set; }
        public DateTime CreatedAt { get; set; }
        public PostSampleNotification(Sample payload)
        {
            Payload = payload;
            CreatedAt = DateTime.UtcNow;
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("PostSample", $"{command.Name}")
                    .Replace("BAYSOFT.Core", $"{command.Entity.Context.Project.Name}.Core")
                    .Replace("Default", command.Entity.Context.Name)
                    .Replace("Samples", command.Entity.Name.Pluralize())
                    .Replace("Sample", command.Entity.Name)
            );
        }
        public static void GenerateNotificationHandlerFile(this Command command)
        {
            var path = @$"src\{command.Entity.Context.Project.Name}.Core.Application\{command.Entity.Context.Name}\{command.Entity.Name.Pluralize()}\Notifications\{command.Name}";
            var filePath = Path.Combine(path, $"{command.Name}NotificationHandler.cs");
            var source = @"using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Notifications
{
    public class PostSampleNotificationHandler : INotificationHandler<PostSampleNotification>
    {
        private ILoggerFactory Logger { get; set; }
        private IMediator Mediator { get; set; }
        public PostSampleNotificationHandler(
            ILoggerFactory logger,
            IMediator mediator)
        {
            Logger = logger;
            Mediator = mediator;
        }
        public Task Handle(PostSampleNotification notification, CancellationToken cancellationToken)
        {
            Logger.CreateLogger<PostSampleNotificationHandler>().Log(LogLevel.Information, $""Sample posted! - Event Created At: {notification.CreatedAt:yyyy-MM-dd HH:mm:ss} Payload: {JsonConvert.SerializeObject(notification.Payload)}"");

            return Task.CompletedTask;
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace(
                        "INotificationHandler<PostSampleNotification>",
                        $"INotificationHandler<{command.Name}Notification>"
                    )
                    .Replace("PostSample", $"{command.Name}")
                    .Replace("BAYSOFT.Core", $"{command.Entity.Context.Project.Name}.Core")
                    .Replace("Default", command.Entity.Context.Name)
                    .Replace("Samples", command.Entity.Name.Pluralize())
                    .Replace("Sample", command.Entity.Name)
            );
        }
        public static void GenerateGetByIdQuery(this Entity entity)
        {

            var path = @$"src\{entity.Context.Project.Name}.Core.Application\{entity.Context.Name}\{entity.Name.Pluralize()}\Queries\Get{entity.Name}ById";
            var filePath = Path.Combine(path, $"Get{entity.Name}ByIdQuery.cs");
            var source = @"using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using FluentValidation;

namespace BAYSOFT.Core.Application.Default.Samples.Queries
{
    public class GetSampleByIdQuery : ApplicationRequest<Sample, GetSampleByIdQueryResponse>
    {
        public GetSampleByIdQuery()
        {
            ConfigKeys(x => x.Id);

            // TODO: SUPPRESSED PROPERTIES
            // TODO: SUPPRESSED RESPONSE PROPERTIES

            // TODO: COMMAND RULES
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace(
                        "ApplicationRequest<Sample, GetSampleByIdQueryResponse>",
                        $"ApplicationRequest<{entity.Name}, Get{entity.Name}ByIdQueryResponse>"
                    )
                    .Replace("GetSampleByIdQuery", $"Get{entity.Name}ByIdQuery")
                    .Replace("BAYSOFT.Core", $"{entity.Context.Project.Name}.Core")
                    .Replace("Default", entity.Context.Name)
                    .Replace("Samples", entity.Name.Pluralize())
                    .Replace("Sample", entity.Name)
                    .Replace(
                        "ConfigKeys(x => x.Id);",
                        $"{string.Join(
                            "\n\t\t\t",
                            entity.Properties.Where(p => p.IsPrimaryKey).Select(p => $"ConfigKeys(x => x.{p.Name});"))}"
                    )
                    .Replace(
                        "// TODO: SUPPRESSED PROPERTIES",
                        $"{string.Join(
                            "\n\t\t\t",
                            entity.Properties
                                .Where(p =>
                                    p.IsPrimaryKey
                                    || entity.Context.Entities
                                        .Any(e => p.Type.Contains(e.Name)))
                                .Select(p => $"ConfigSuppressedProperties(x => x.{p.Name});")
                                .ToList())}"
                    )
                    .ConditionalReplace(
                        entity.Properties.Where(x => x.IsForeignKey && !string.IsNullOrWhiteSpace(x.RelatedEntityName)).Any()
                        || entity.Context.Entities.Where(x => x.Properties.Any(property => property.IsForeignKey && !string.IsNullOrWhiteSpace(property.RelatedEntityName) && property.RelatedEntityName.Equals(entity.Name))).Any(),
                        "// TODO: SUPPRESSED RESPONSE PROPERTIES",
                        $"{string.Join(
                            "\n\t\t\t",
                            entity.Properties
                                .Where(x => x.IsForeignKey && !string.IsNullOrWhiteSpace(x.RelatedEntityName))
                                .Select(x => $"ConfigSuppressedResponseProperties(x => x.{x.RelatedEntityName});")
                                .ToList()
                                .Union(
                                    entity.Context.Entities
                                        .Where(x => x.Properties.Any(property => property.IsForeignKey && !string.IsNullOrWhiteSpace(property.RelatedEntityName) && property.RelatedEntityName.Equals(entity.Name)))
                                        .Select(x => $"ConfigSuppressedResponseProperties(x => x.{x.Name.Pluralize()});")
                                        .ToList()))}"
                    )
                    .Replace(
                        "// TODO: COMMAND RULES",
                        $"{string.Join(
                            "\n\t\t\t",
                            entity.Properties.Where(property => property.IsPrimaryKey)
                                .Select(property => $"Validator.RuleFor(x => x.{property.Name}).NotEmpty().WithMessage(\"{{0}} is required!\");")
                                .ToList())}"
                    )
            );
        }
        public static void GenerateGetByIdQueryResponse(this Entity entity)
        {
            var path = @$"src\{entity.Context.Project.Name}.Core.Application\{entity.Context.Name}\{entity.Name.Pluralize()}\Queries\Get{entity.Name}ById";
            var filePath = Path.Combine(path, $"Get{entity.Name}ByIdQueryResponse.cs");
            var source = @"using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using ModelWrapper;
using System;
using System.Collections.Generic;

namespace BAYSOFT.Core.Application.Default.Samples.Queries
{
    public class GetSampleByIdQueryResponse : ApplicationResponse<Sample>
    {
        public GetSampleByIdQueryResponse(Tuple<int, int, WrapRequest<Sample>, Dictionary<string, object>, Dictionary<string, object>, string, long?> tuple) : base(tuple)
        {
        }

        public GetSampleByIdQueryResponse(WrapRequest<Sample> request, object data, string message = ""Successful operation!"", long? resultCount = null)
            : base(request, data, message, resultCount)
        {
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace(
                        "ApplicationResponse<Sample>",
                        $"ApplicationResponse<{entity.Name}>"
                    )
                    .Replace("GetSampleByIdQuery", $"Get{entity.Name}ByIdQuery")
                    .Replace("BAYSOFT.Core", $"{entity.Context.Project.Name}.Core")
                    .Replace("Default", entity.Context.Name)
                    .Replace("Samples", entity.Name.Pluralize())
                    .Replace("Sample", entity.Name)
            );
        }
        public static void GenerateGetByIdQueryHandler(this Entity entity)
        {
            var path = @$"src\{entity.Context.Project.Name}.Core.Application\{entity.Context.Name}\{entity.Name.Pluralize()}\Queries\Get{entity.Name}ById";
            var filePath = Path.Combine(path, $"Get{entity.Name}ByIdQueryHandler.cs");
            var source = @"using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Abstractions.Core.Domain.Exceptions;
using BAYSOFT.Abstractions.Crosscutting.Helpers;
using BAYSOFT.Abstractions.Crosscutting.InheritStringLocalization;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Resources;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using BAYSOFT.Core.Domain.Default.Samples.Resources;
using BAYSOFT.Core.Domain.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ModelWrapper.Extensions.Select;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Queries
{

    [InheritStringLocalizer(typeof(Messages), Priority = 0)]
    [InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
    [InheritStringLocalizer(typeof(EntitiesSamples), Priority = 2)]
    public class GetSampleByIdQueryHandler : ApplicationRequestHandler<Sample, GetSampleByIdQuery, GetSampleByIdQueryResponse>
    {
        private ILoggerFactory Logger { get; set; }
        private IMediator Mediator { get; set; }
        private IStringLocalizer Localizer { get; set; }
        private IDefaultDbContextReader Reader { get; set; }
        public GetSampleByIdQueryHandler(
            ILoggerFactory logger,
            IMediator mediator,
            IStringLocalizer<GetSampleByIdQueryHandler> localizer,
            IDefaultDbContextReader reader
        )
        {
            Logger = logger;
            Mediator = mediator;
            Localizer = localizer;
            Reader = reader;
        }
        public override async Task<GetSampleByIdQueryResponse> Handle(GetSampleByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                long resultCount = 1;

                var id = request.Project(x => x.Id);

                var data = await Reader
                    .Query<Sample>()
                    .Where(x => x.Id == id)
                    .Select(request)
                    .SingleOrDefaultAsync();

                if (data == null)
                {
                    throw new EntityNotFoundException<Sample>(Localizer);
                }

                return new GetSampleByIdQueryResponse(request, data, Localizer[""Successful operation!""], resultCount);
            }
            catch (Exception exception)
            {
                Logger.CreateLogger<GetSampleByIdQueryHandler>().Log(LogLevel.Error, exception, exception.Message);

                return new GetSampleByIdQueryResponse(ExceptionResponseHelper.CreateTuple(Localizer, request, exception));
            }
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace(
                        "ApplicationRequestHandler<Sample, GetSampleByIdQuery, GetSampleByIdQueryResponse>",
                        $"ApplicationRequestHandler<{entity.Name}, Get{entity.Name}ByIdQuery, Get{entity.Name}ByIdQueryResponse>"
                    )
                    .Replace("GetSampleByIdQuery", $"Get{entity.Name}ByIdQuery")
                    .Replace("BAYSOFT.Core", $"{entity.Context.Project.Name}.Core")
                    .Replace("Default", entity.Context.Name)
                    .Replace("Samples", entity.Name.Pluralize())
                    .Replace("Sample", entity.Name)
            );
        }
        public static void GenerateGetByFilterQuery(this Entity entity)
        {

            var path = @$"src\{entity.Context.Project.Name}.Core.Application\{entity.Context.Name}\{entity.Name.Pluralize()}\Queries\Get{entity.Name.Pluralize()}ByFilter";
            var filePath = Path.Combine(path, $"Get{entity.Name.Pluralize()}ByFilterQuery.cs");
            var source = @"using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using FluentValidation;

namespace BAYSOFT.Core.Application.Default.Samples.Queries
{
    public class GetSamplesByFilterQuery : ApplicationRequest<Sample, GetSamplesByFilterQueryResponse>
    {
        public GetSamplesByFilterQuery()
        {
            ConfigKeys(x => x.Id);

            // TODO: SUPPRESSED PROPERTIES
            // TODO: SUPPRESSED RESPONSE PROPERTIES

            // TODO: COMMAND RULES
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace(
                        "ApplicationRequest<Sample, GetSamplesByFilterQueryResponse>",
                        $"ApplicationRequest<{entity.Name}, Get{entity.Name.Pluralize()}ByFilterQueryResponse>"
                    )
                    .Replace("GetSamplesByFilterQuery", $"Get{entity.Name.Pluralize()}ByFilterQuery")
                    .Replace("BAYSOFT.Core", $"{entity.Context.Project.Name}.Core")
                    .Replace("Default", entity.Context.Name)
                    .Replace("Samples", entity.Name.Pluralize())
                    .Replace("Sample", entity.Name)
                    .Replace(
                        "ConfigKeys(x => x.Id);",
                        $"{string.Join(
                            "\n\t\t\t",
                            entity.Properties.Where(p => p.IsPrimaryKey).Select(p => $"ConfigKeys(x => x.{p.Name});"))}"
                    )
                    .ConditionalReplace(
                        entity.Properties.Where(x => x.IsForeignKey && !string.IsNullOrWhiteSpace(x.RelatedEntityName)).Any()
                        || entity.Context.Entities.Where(x => x.Properties.Any(property => property.IsForeignKey && !string.IsNullOrWhiteSpace(property.RelatedEntityName) && property.RelatedEntityName.Equals(entity.Name))).Any(),
                        "// TODO: SUPPRESSED RESPONSE PROPERTIES",
                        $"{string.Join(
                            "\n\t\t\t",
                            entity.Properties
                                .Where(x => x.IsForeignKey && !string.IsNullOrWhiteSpace(x.RelatedEntityName))
                                .Select(x => $"ConfigSuppressedResponseProperties(x => x.{x.RelatedEntityName});")
                                .ToList()
                                .Union(
                                    entity.Context.Entities
                                        .Where(x => x.Properties.Any(property => property.IsForeignKey && !string.IsNullOrWhiteSpace(property.RelatedEntityName) && property.RelatedEntityName.Equals(entity.Name)))
                                        .Select(x => $"ConfigSuppressedResponseProperties(x => x.{x.Name.Pluralize()});")
                                        .ToList()))}"
                    )
            );
        }
        public static void GenerateGetByFilterQueryResponse(this Entity entity)
        {
            var path = @$"src\{entity.Context.Project.Name}.Core.Application\{entity.Context.Name}\{entity.Name.Pluralize()}\Queries\Get{entity.Name.Pluralize()}ByFilter";
            var filePath = Path.Combine(path, $"Get{entity.Name.Pluralize()}ByFilterQueryResponse.cs");
            var source = @"using BAYSOFT.Abstractions.Core.Application;
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

        public GetSamplesByFilterQueryResponse(WrapRequest<Sample> request, object data, string message = ""Successful operation!"", long? resultCount = null)
            : base(request, data, message, resultCount)
        {
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace(
                        "ApplicationResponse<Sample>",
                        $"ApplicationResponse<{entity.Name}>"
                    )
                    .Replace("GetSamplesByFilterQuery", $"Get{entity.Name.Pluralize()}ByFilterQuery")
                    .Replace("BAYSOFT.Core", $"{entity.Context.Project.Name}.Core")
                    .Replace("Default", entity.Context.Name)
                    .Replace("Samples", entity.Name.Pluralize())
                    .Replace("Sample", entity.Name)
            );
        }
        public static void GenerateGetByFilterQueryHandler(this Entity entity)
        {
            var path = @$"src\{entity.Context.Project.Name}.Core.Application\{entity.Context.Name}\{entity.Name.Pluralize()}\Queries\Get{entity.Name.Pluralize()}ByFilter";
            var filePath = Path.Combine(path, $"Get{entity.Name.Pluralize()}ByFilterQueryHandler.cs");
            var source = @"using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Abstractions.Crosscutting.Helpers;
using BAYSOFT.Abstractions.Crosscutting.InheritStringLocalization;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Resources;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using BAYSOFT.Core.Domain.Default.Samples.Resources;
using BAYSOFT.Core.Domain.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ModelWrapper.Extensions.FullSearch;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Queries
{

    [InheritStringLocalizer(typeof(Messages), Priority = 0)]
    [InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
    [InheritStringLocalizer(typeof(EntitiesSamples), Priority = 2)]
    public class GetSamplesByFilterQueryHandler : ApplicationRequestHandler<Sample, GetSamplesByFilterQuery, GetSamplesByFilterQueryResponse>
    {
        private ILoggerFactory Logger { get; set; }
        private IMediator Mediator { get; set; }
        private IStringLocalizer Localizer { get; set; }
        private IDefaultDbContextReader Reader { get; set; }
        public GetSamplesByFilterQueryHandler(
            ILoggerFactory logger,
            IMediator mediator,
            IStringLocalizer<GetSamplesByFilterQueryHandler> localizer,
            IDefaultDbContextReader reader
        )
        {
            Logger = logger;
            Mediator = mediator;
            Localizer = localizer;
            Reader = reader;
        }
        public override async Task<GetSamplesByFilterQueryResponse> Handle(GetSamplesByFilterQuery request, CancellationToken cancellationToken)
        {
            try
            {
                long resultCount = 0;

                var data = await Reader
                    .Query<Sample>()
                    .AsNoTracking()
                    .FullSearch(request, out resultCount)
                    .ToListAsync(cancellationToken);

                return new GetSamplesByFilterQueryResponse(request, data, Localizer[""Successful operation!""], resultCount);
            }
            catch (Exception exception)
            {
                Logger.CreateLogger<GetSamplesByFilterQueryHandler>().Log(LogLevel.Error, exception, exception.Message);

                return new GetSamplesByFilterQueryResponse(ExceptionResponseHelper.CreateTuple(Localizer, request, exception));
            }
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace(
                        "ApplicationRequestHandler<Sample, GetSamplesByFilterQuery, GetSamplesByFilterQueryResponse>",
                        $"ApplicationRequestHandler<{entity.Name}, Get{entity.Name.Pluralize()}ByFilterQuery, Get{entity.Name.Pluralize()}ByFilterQueryResponse>"
                    )
                    .Replace("GetSamplesByFilterQuery", $"Get{entity.Name.Pluralize()}ByFilterQuery")
                    .Replace("BAYSOFT.Core", $"{entity.Context.Project.Name}.Core")
                    .Replace("Default", entity.Context.Name)
                    .Replace("Samples", entity.Name.Pluralize())
                    .Replace("Sample", entity.Name)
            );
        }
        public static void GenerateIDbContextReader(this Context context)
        {
            var path = @$"src\{context.Project.Name}.Core.Domain\{context.Name}\Interfaces\Infrastructures\Data";
            var filePath = Path.Combine(path, $"I{context.Name}DbContextReader.cs");
            var source = @"using BAYSOFT.Abstractions.Core.Domain.Interfaces.Infrastructures.Data;

namespace BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data
{
    public interface IDefaultDbContextReader : IReader
    {
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("BAYSOFT.Core", $"{context.Project.Name}.Core")
                    .Replace("Default", context.Name)
            );
        }
        public static void GenerateIDbContextWriter(this Context context)
        {
            var path = @$"src\{context.Project.Name}.Core.Domain\{context.Name}\Interfaces\Infrastructures\Data";
            var filePath = Path.Combine(path, $"I{context.Name}DbContextWriter.cs");
            var source = @"using BAYSOFT.Abstractions.Core.Domain.Interfaces.Infrastructures.Data;

namespace BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data
{
    public interface IDefaultDbContextWriter : IWriter
    {
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("BAYSOFT.Core", $"{context.Project.Name}.Core")
                    .Replace("Default", context.Name)
            );
        }
        public static void GenerateDbContextReader(this Context context)
        {
            var path = @$"src\{context.Project.Name}.Infrastructures.Data\{context.Name}";
            var filePath = Path.Combine(path, $"{context.Name}DbContextReader.cs");
            var source = @"using BAYSOFT.Abstractions.Core.Domain.Entities;
using BAYSOFT.Abstractions.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Infrastructures.Data.Contexts;

namespace BAYSOFT.Infrastructures.Data.Default
{
    public class DefaultDbContextReader: Reader, IDefaultDbContextReader
    {
        public DefaultDbContextReader(DefaultDbContext context) : base(context)
        {
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("BAYSOFT.Infrastructures", $"{context.Project.Name}.Infrastructures")
                    .Replace("BAYSOFT.Core", $"{context.Project.Name}.Core")
                    .Replace("Default", context.Name)
            );
        }
        public static void GenerateDbContextWriter(this Context context)
        {
            var path = @$"src\{context.Project.Name}.Infrastructures.Data\{context.Name}";
            var filePath = Path.Combine(path, $"{context.Name}DbContextWriter.cs");
            var source = @"using BAYSOFT.Abstractions.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Infrastructures.Data.Contexts;

namespace BAYSOFT.Infrastructures.Data.Default
{
    public class DefaultDbContextWriter : Writer, IDefaultDbContextWriter
    {
        public DefaultDbContextWriter(DefaultDbContext context) : base(context)
        {
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("BAYSOFT.Infrastructures", $"{context.Project.Name}.Infrastructures")
                    .Replace("BAYSOFT.Core", $"{context.Project.Name}.Core")
                    .Replace("Default", context.Name)
            );
        }
        public static void GenerateDbContext(this Context context)
        {
            var path = @$"src\{context.Project.Name}.Infrastructures.Data\{context.Name}";
            var filePath = Path.Combine(path, $"{context.Name}DbContext.cs");
            var source = @"// TODO: ENTITY USING REFERENCES
using BAYSOFT.Infrastructures.Data.Default.EntityMappings;
using Microsoft.EntityFrameworkCore;

namespace BAYSOFT.Infrastructures.Data.Contexts
{
    public class DefaultDbContext : DbContext
    {
        // TODO: ENTITY COLLECTIONS

        protected DefaultDbContext()
        {
            if (Database.IsRelational())
            {
                Database.Migrate();
            }
        }

        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {
            if (Database.IsRelational())
            {
                Database.Migrate();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO: SETUP SCHEMA

            // TODO: ENTITY MAPPINGS
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("BAYSOFT.Infrastructures", $"{context.Project.Name}.Infrastructures")
                    .Replace("BAYSOFT.Core", $"{context.Project.Name}.Core")
                    .Replace("Default", context.Name)
                    .ConditionalReplace(
                        context.Entities.Any(),
                        "// TODO: ENTITY USING REFERENCES",
                        $"{string.Join(
                            "\n",
                            context.Entities
                                .Select(entity => $"using {entity.Context.Project.Name}.Core.Domain.{entity.Context.Name}.{entity.Name.Pluralize()}.Entities;")
                                .Order()
                                .ToList()
                            )}"
                    )
                    .ConditionalReplace(
                        context.Entities.Any(),
                        "// TODO: ENTITY COLLECTIONS",
                        $"{string.Join(
                            "\n\t\t",
                            context.Entities
                                .Select(entity => string.Concat($"public DbSet<{entity.Name}> {entity.Name.Pluralize()} ", "{ get; set; }"))
                                .Order()
                                .ToList()
                            )}"
                    )
                    .Replace(
                        "// TODO: SETUP SCHEMA",
                        $"modelBuilder.HasDefaultSchema(\"{(string.IsNullOrWhiteSpace(context.Schema) ? context.Name : context.Schema)}\");"
                    )
                    .ConditionalReplace(
                        context.Entities.Any(),
                        "// TODO: ENTITY MAPPINGS",
                        $"{string.Join(
                            "\n\t\t\t",
                            context.Entities
                                .Select(entity => $"modelBuilder.ApplyConfiguration(new {entity.Name.Pluralize()}Map());")
                                .Order()
                                .ToList()
                            )}"
                    )
            );
        }
        public static void GenerateEntityMapping(this Entity entity)
        {
            var path = @$"src\{entity.Context.Project.Name}.Infrastructures.Data\{entity.Context.Name}\EntityMappings";
            var filePath = Path.Combine(path, $"{entity.Name}Map.cs");
            var source = @"using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BAYSOFT.Core.Domain.Default.Samples.Entities;

namespace BAYSOFT.Infrastructures.Data.Default.EntityMappings
{
    public class SampleMap : IEntityTypeConfiguration<Sample>
    {
        public void Configure(EntityTypeBuilder<Sample> builder)
        {
            // TODO: MAP KEYS PROPERTIES

            // TODO: MAP PROPERTIES

            // TODO: MAP KEYS

            // TODO: MAP TABLE

            // TODO: MAP RELATIONS
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("BAYSOFT.Infrastructures", $"{entity.Context.Project.Name}.Infrastructures")
                    .Replace("BAYSOFT.Core", $"{entity.Context.Project.Name}.Core")
                    .Replace("Default", entity.Context.Name)
                    .Replace("Samples", entity.Name.Pluralize())
                    .Replace("Sample", entity.Name)
                    .ConditionalReplace(
                        entity.Properties.Where(property => property.IsPrimaryKey).Any(),
                        "// TODO: MAP KEYS PROPERTIES",
                        $"{(entity.Properties.Where(property => property.IsPrimaryKey).Count() == 1
                                ? $"builder\r\n\t\t\t\t.Property<{entity.Properties.Where(property => property.IsPrimaryKey).SingleOrDefault().Type}{(entity.Properties.Where(property => property.IsPrimaryKey).SingleOrDefault().IsNullable ? "?" : "")}>(p => p.{entity.Properties.Where(property => property.IsPrimaryKey).SingleOrDefault().Name}\")\r\n\t\t\t\t.HasColumnType(\"{entity.Properties.Where(property => property.IsPrimaryKey).SingleOrDefault().DbType})\r\n\t\t\t\t.HasColumnName(\"{entity.Properties.Where(property => property.IsPrimaryKey).SingleOrDefault().DbName}\")\r\n\t\t\t\t.ValueGeneratedOnAdd()\r\n\t\t\t\t.UseIdentityColumn()\r\n\t\t\t\t.IsRequired({(entity.Properties.Where(property => property.IsPrimaryKey).SingleOrDefault().IsNullable ? "false" : "true")});"
                                : string.Join(
                                    "\n\t\t\t",
                                    entity.Properties.Where(property => property.IsPrimaryKey)
                                        .Select(property => $"builder\r\n\t\t\t\t.Property<{property.Type}{(property.IsNullable ? "?" : "")}>(p => p.{property.Name})\r\n\t\t\t\t.HasColumnType(\"{property.DbType})\r\n\t\t\t\t.HasColumnName(\"{property.DbName}\")\r\n\t\t\t\t.IsRequired({(property.IsNullable ? "false" : "true")});")
                                        .Order()
                                        .ToList()
                                )
                            )}"
                    )
                    .ConditionalReplace(
                        entity.Properties.Where(property => !property.IsPrimaryKey).Any(),
                        "// TODO: MAP PROPERTIES",
                        $"{string.Join(
                            "\n\n\t\t\t",
                            entity.Properties.Where(property => !property.IsPrimaryKey)
                                .Select(property => $"builder\r\n\t\t\t\t.Property<{property.Type}{(property.IsNullable ? "?" : "")}>(p => p.{property.Name})\r\n\t\t\t\t.HasColumnType(\"{property.DbType})\r\n\t\t\t\t.HasColumnName(\"{property.DbName}\")\r\n\t\t\t\t.IsRequired({(property.IsNullable ? "false" : "true")});")
                                .Order()
                                .ToList()
                        )}"
                    )
                    .ConditionalReplace(
                        entity.Properties.Where(property => property.IsPrimaryKey).Any(),
                        "// TODO: MAP KEYS",
                        $"{string.Join(
                            "\n\t\t\t",
                            entity.Properties.Where(property => property.IsPrimaryKey)
                                .Select(property => $"builder\r\n\t\t\t\t.HasKey(x=>x.{property.Name});")
                                .Order()
                                .ToList()
                        )}"
                    )
                    .Replace("// TODO: MAP TABLE", $"builder\r\n\t\t\t\t.ToTable(\"{entity.Name.Pluralize()}\");")
                    .ConditionalReplace(
                        entity.Properties.Where(property => property.IsForeignKey && !string.IsNullOrWhiteSpace(property.RelatedEntityName)).Any(),
                        "// TODO: MAP RELATIONS",
                        $"{string.Join(
                            "\n\t\t\t",
                            entity.Properties.Where(property => property.IsForeignKey && !string.IsNullOrWhiteSpace(property.RelatedEntityName))
                                .Select(property => $"builder\r\n\t\t\t\t.HasOne(x => x.{property.RelatedEntityName})\r\n\t\t\t\t.WithMany(x => x.{property.Entity.Name.Pluralize()})\r\n\t\t\t\t.HasForeignKey(x => x.{property.Name});")
                                .Order()
                                .ToList()
                        )}"
                    )
            );
        }
        public static void GenerateEntityResourceXML(this Entity entity)
        {
            var path = @$"src\{entity.Context.Project.Name}.Core.Domain\{entity.Context.Name}\{entity.Name.Pluralize()}\Resources";
            var filePath = Path.Combine(path, $"Entities{entity.Name.Pluralize()}.resx");
            var source = @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
  <xsd:schema id=""root"" xmlns="""" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
    <xsd:element name=""root"" msdata:IsDataSet=""true"">
      <xsd:complexType>
        <xsd:choice maxOccurs=""unbounded"">
          <xsd:element name=""data"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
                <xsd:element name=""comment"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""2"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" type=""xsd:string"" msdata:Ordinal=""1"" />
              <xsd:attribute name=""type"" type=""xsd:string"" msdata:Ordinal=""3"" />
              <xsd:attribute name=""mimetype"" type=""xsd:string"" msdata:Ordinal=""4"" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name=""resheader"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" type=""xsd:string"" use=""required"" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name=""resmimetype"">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name=""version"">
    <value>1.3</value>
  </resheader>
  <resheader name=""reader"">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=2.0.3500.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name=""writer"">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=2.0.3500.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <!-- TODO: MESSAGES -->
</root>";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("BAYSOFT.Core", $"{entity.Context.Project.Name}.Core")
                    .Replace("Default", entity.Context.Name)
                    .ConditionalReplace(
                        entity.GetMessages.Any(),
                        "<!-- TODO: MESSAGES -->",
                        $"{string.Join(
                            "\n  ",
                            entity.GetMessages.Select(message => $"<data name=\"{message}\" xml:space=\"preserve\">\r\n    <value>{message}</value>\r\n  </data>").ToList()
                        )}")
            );
        }
        public static void GenerateEntityResourceClass(this Entity entity)
        {
            var path = @$"src\{entity.Context.Project.Name}.Core.Domain\{entity.Context.Name}\{entity.Name.Pluralize()}\Resources";
            var filePath = Path.Combine(path, $"Entities{entity.Name.Pluralize()}.Designer.cs");
            var source = @"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BAYSOFT.Core.Domain.Default.Samples.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(""System.Resources.Tools.StronglyTypedResourceBuilder"", ""17.0.0.0"")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class EntitiesSamples {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute(""Microsoft.Performance"", ""CA1811:AvoidUncalledPrivateCode"")]
        internal EntitiesSamples() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager(""BAYSOFT.Core.Domain.Default.Samples.Resources.EntitiesSamples"", typeof(EntitiesSamples).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        // TODO: MESSAGES
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("BAYSOFT.Core", $"{entity.Context.Project.Name}.Core")
                    .Replace("Default", entity.Context.Name)
                    .Replace("Samples", entity.Name.Pluralize())
                    .Replace("Sample", entity.Name)
                    .ConditionalReplace(
                        entity.GetMessages.Any(),
                        "// TODO: MESSAGES",
                        $"{string.Join(
                            "\n\n\t\t",
                            entity.GetMessages.Select(message => $"/// <summary>\r\n\t\t///   Looks up a localized string similar to {message}.\r\n\t\t/// </summary>\r\n\t\tpublic static string {message.ReplaceSpecialCharacters()} {{\r\n\t\t\tget {{\r\n\t\t\t\treturn ResourceManager.GetString(\"{message}\", resourceCulture);\r\n\t\t\t}}\r\n\t\t}}").ToList()
                        )}")
            );
        }
        public static void GenerateContextResourceXML(this Context context)
        {
            var path = @$"src\{context.Project.Name}.Core.Domain\{context.Name}\Resources";
            var filePath = Path.Combine(path, $"Entities{context.Name}.resx");
            var source = @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
  <xsd:schema id=""root"" xmlns="""" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
    <xsd:element name=""root"" msdata:IsDataSet=""true"">
      <xsd:complexType>
        <xsd:choice maxOccurs=""unbounded"">
          <xsd:element name=""data"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
                <xsd:element name=""comment"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""2"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" type=""xsd:string"" msdata:Ordinal=""1"" />
              <xsd:attribute name=""type"" type=""xsd:string"" msdata:Ordinal=""3"" />
              <xsd:attribute name=""mimetype"" type=""xsd:string"" msdata:Ordinal=""4"" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name=""resheader"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" type=""xsd:string"" use=""required"" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name=""resmimetype"">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name=""version"">
    <value>1.3</value>
  </resheader>
  <resheader name=""reader"">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=2.0.3500.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name=""writer"">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=2.0.3500.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <!-- TODO: MESSAGES -->
</root>";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("BAYSOFT.Core", $"{context.Project.Name}.Core")
                    .Replace("Default", context.Name)
                    .ConditionalReplace(
                        context.GetMessages.Any(),
                        "<!-- TODO: MESSAGES -->",
                        $"{string.Join(
                            "\n  ",
                            context.GetMessages.Select(message => $"<data name=\"{message}\" xml:space=\"preserve\">\r\n    <value>{message}</value>\r\n  </data>").ToList()
                        )}"
                    )
            );
        }
        public static void GenerateContextResourceClass(this Context context)
        {
            var path = @$"src\{context.Project.Name}.Core.Domain\{context.Name}\Resources";
            var filePath = Path.Combine(path, $"Entities{context.Name}.Designer.cs");
            var source = @"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BAYSOFT.Core.Domain.Default.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(""System.Resources.Tools.StronglyTypedResourceBuilder"", ""17.0.0.0"")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class EntitiesDefault {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute(""Microsoft.Performance"", ""CA1811:AvoidUncalledPrivateCode"")]
        internal EntitiesDefault() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager(""BAYSOFT.Core.Domain.Default.Resources.EntitiesDefault"", typeof(EntitiesDefault).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        // TODO: MESSAGES
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("BAYSOFT.Core", $"{context.Project.Name}.Core")
                    .Replace("Default", context.Name)
                    .ConditionalReplace(
                        context.GetMessages.Any(),
                        "// TODO: MESSAGES",
                        $"{string.Join(
                            "\n\n\t\t",
                            context.GetMessages.Select(message => $"/// <summary>\r\n\t\t///   Looks up a localized string similar to {message}.\r\n\t\t/// </summary>\r\n\t\tpublic static string {message.ReplaceSpecialCharacters()} {{\r\n\t\t\tget {{\r\n\t\t\t\treturn ResourceManager.GetString(\"{message}\", resourceCulture);\r\n\t\t\t}}\r\n\t\t}}").ToList()
                        )}")
            );
        }
        public static void GenerateProjectResourceXML(this Project project)
        {
            var path = @$"src\{project.Name}.Core.Domain\Resources";
            var filePath = Path.Combine(path, $"Messages.resx");
            var source = @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
  <xsd:schema id=""root"" xmlns="""" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
    <xsd:element name=""root"" msdata:IsDataSet=""true"">
      <xsd:complexType>
        <xsd:choice maxOccurs=""unbounded"">
          <xsd:element name=""data"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
                <xsd:element name=""comment"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""2"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" type=""xsd:string"" msdata:Ordinal=""1"" />
              <xsd:attribute name=""type"" type=""xsd:string"" msdata:Ordinal=""3"" />
              <xsd:attribute name=""mimetype"" type=""xsd:string"" msdata:Ordinal=""4"" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name=""resheader"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" type=""xsd:string"" use=""required"" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name=""resmimetype"">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name=""version"">
    <value>1.3</value>
  </resheader>
  <resheader name=""reader"">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=2.0.3500.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name=""writer"">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=2.0.3500.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <!-- TODO: MESSAGES -->
</root>";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("BAYSOFT.Core", $"{project.Name}.Core")
                    .ConditionalReplace(
                        project.GetMessages.Any(),
                        "<!-- TODO: MESSAGES -->",
                        $"{string.Join(
                            "\n  ",
                            project.GetMessages.Select(message => $"<data name=\"{message}\" xml:space=\"preserve\">\r\n    <value>{message}</value>\r\n  </data>").ToList()
                        )}")
            );
        }
        public static void GenerateProjectResourceClass(this Project project)
        {
            var path = @$"src\{project.Name}.Core.Domain\Resources";
            var filePath = Path.Combine(path, "Messages.Designer.cs");
            var source = @"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BAYSOFT.Core.Domain.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(""System.Resources.Tools.StronglyTypedResourceBuilder"", ""17.0.0.0"")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute(""Microsoft.Performance"", ""CA1811:AvoidUncalledPrivateCode"")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager(""BAYSOFT.Core.Domain.Resources.Messages"", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        // TODO: MESSAGES
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("BAYSOFT.Core", $"{project.Name}.Core")
                    .ConditionalReplace(
                        project.GetMessages.Any(),
                        "// TODO: MESSAGES",
                        $"{string.Join(
                            "\n\n\t\t",
                            project.GetMessages.Select(message => $"/// <summary>\r\n\t\t///   Looks up a localized string similar to {message}.\r\n\t\t/// </summary>\r\n\t\tpublic static string {message.ReplaceSpecialCharacters()} {{\r\n\t\t\tget {{\r\n\t\t\t\treturn ResourceManager.GetString(\"{message}\", resourceCulture);\r\n\t\t\t}}\r\n\t\t}}").ToList()
                        )}")
            );
        }
        public static void GenerateContextInitialMigration(this Context context)
        {
            var path = @$"src\{context.Project.Name}.Infrastructures.Data\{context.Name}\Migrations";
            var filePath = Path.Combine(path, $"InitialMigration{context.Name}DbContext.cs");
            var source = @"using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BAYSOFT.Infrastructures.Data.Default.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationDefaultDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // TODO: UPS
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // TODO: DOWNS
        }
    }
}
";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("BAYSOFT.Core", $"{context.Project.Name}.Core")
                    .Replace("Default", context.Name)
                    .ConditionalReplace(
                        context.Entities.Any(),
                        "// TODO: UPS",
                        $"{string.Join(
                            "\n\n\t\t\t",
                            context.Entities
                                .Select(entity => entity.GenerateMigrationUP())
                                .ToList())}")
                    .ConditionalReplace(
                        context.Entities.Any(),
                        "// TODO: DOWNS",
                        $"{string.Join(
                            "\n\n\t\t\t",
                            context.Entities
                                .Select(entity => entity.GenerateMigrationDOWN())
                                .ToList())}")
            );
        }
        public static string GenerateMigrationUP(this Entity entity)
        {
            var columns = $"{string.Join(",\n\t\t\t\t\t", entity.Properties.Select(property => property.GenerateMigration()).ToList())}";
            var constraintsPK = $"\n\t\t\t\t\ttable.PrimaryKey(\"PK_{entity.Name.Pluralize()}\");";
            var constraintsFK = entity.Properties.Where(property => property.IsForeignKey).Any()
                ? $"\n\t\t\t\t\t{string.Join("\"\\n\\t\\t\\t\\t\\t\\t", entity.Properties.Where(property => property.IsForeignKey).Select(property => $"table.ForeignKey(\"FK_{property.Entity.Name.Pluralize()}_TO_{property.RelatedEntityName.Pluralize()}\", x => x.{property.Name}, \"{property.RelatedEntityName.Pluralize()}\", \"{property.Entity.Context.Entities.Where(e => e.Name == property.RelatedEntityName).SingleOrDefault().Properties.Where(p => p.IsPrimaryKey).FirstOrDefault().DbName}\");").ToList())}\");"
                : "";
            return $"migrationBuilder.CreateTable(\r\n\t\t\t\tname: \"{entity.Name.Pluralize()}\",\r\n\t\t\t\tcolumns: table => new\r\n\t\t\t\t{{\r\n\t\t\t\t\t{columns}\r\n\t\t\t\t}},\r\n\t\t\t\tcolumns: table => new\r\n\t\t\t\t{{{constraintsPK}{constraintsFK}\r\n\t\t\t\t}});";
        }
        public static string GenerateMigrationDOWN(this Entity entity)
        {
            return $"migrationBuilder.DropTable(\r\n\t\t\t\tname: \"{entity.Name.Pluralize()}\");";
        }
        public static string GenerateMigration(this Property property)
        {
            return $"{property.Name} = table.Column<{property.Type}{(property.IsNullable ? "?" : "")}>(name: \"{property.DbName}\", type: \"{property.DbType}\", nullable: {(property.IsNullable ? "true" : "false")}){(property.IsDbGenerated ? "\r\n\t\t\t\t\t\t.Annotation(\"SqlServer:Identity\", \"1, 1\")" : "")}";
        }
        public static void GenerateSnapshot(this Context context)
        {
            var path = @$"src\{context.Project.Name}.Infrastructures.Data\{context.Name}\Migrations";
            var filePath = Path.Combine(path, $"{context.Name}DbContextModelSnapshot.cs");
            var source = @"// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace BAYSOFT.Infrastructures.Data.Default.Migrations
{
    [DbContext(typeof(DefaultDbContext))]
    partial class DefaultDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(""Default"");
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation(""ProductVersion"", ""7.0.4"")
                .HasAnnotation(""Relational:MaxIdentifierLength"", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            // TODO: ENTITIES
#pragma warning restore 612, 618
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .ConditionalReplace(
                        !string.IsNullOrWhiteSpace(context.Schema),
                        "modelBuilder.HasDefaultSchema(\"Default\");",
                        $"modelBuilder.HasDefaultSchema(\"{context.Schema}\");")
                    .ConditionalReplace(
                        context.Entities.Any(),
                        "// TODO: ENTITIES",
                        $"{string.Join(
                            "\n\n\t\t\t",
                            context.Entities
                                .Select(entity => entity.GenerateSnapshot())
                                .ToList())}")
                    .Replace("BAYSOFT.Core", $"{context.Project.Name}.Core")
                    .Replace("Default", context.Name)
            );
        }
        public static string GenerateSnapshot(this Entity entity)
        {
            var properties = $"{string.Join("\n\n\t\t\t\t", entity.Properties.Select(p => p.GenerateSnapshot()).ToList())}";
            var keys = $"\n\n\t\t\t\tb.HasKey(\"{string.Join(", ", entity.Properties.Where(p => p.IsPrimaryKey).Select(p => p.Name).ToList())}\");";
            var relationships = entity.Properties.Where(p => p.IsForeignKey).Any() ? $"\n\n\t\t\t\t{string.Join("\n\t\t\t\t", entity.Properties.Where(p => p.IsForeignKey).Select(p => $"b.HasOne(\"{p.RelatedEntityName}\")\n\t\t\t\t\t.WithMany(\"{p.Entity.Name.Pluralize()}\")\n\t\t\t\t\t.HasForeignKey(\"{string.Join(", ", p.Entity.Context.Entities.Where(e => e.Name == p.RelatedEntityName).SingleOrDefault().Properties.Where(prop => prop.IsPrimaryKey).Select(key => key.Name).ToList())}\");").ToList())}" : "";
            var table = $"\n\n\t\t\t\tb.ToTable(\"{entity.Name.Pluralize()}\",\"{entity.Context.Schema}\");";

            return $"modelBuilder.Entity(\"BAYSOFT.Core.Domain.Default.Samples.Entities\", b => \n\t\t\t{{\n\t\t\t\t{properties}{keys}{relationships}{table}\n\t\t\t}});";
        }
        public static string GenerateSnapshot(this Property property)
        {
            return $"b.Property<{property.Type}{(property.IsNullable ? "?" : "")}>(\"{property.Name}\")\r\n\t\t\t\t\t.HasColumnType(\"{property.DbType}\")\r\n\t\t\t\t\t.HasColumnName(\"{property.DbName}\"){(property.IsDbGenerated ? ".ValueGeneratedOnAdd()" : "")};{(property.IsDbGenerated ? $"\r\n\r\n\t\t\t\tSqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<{property.Type}{(property.IsNullable ? "?" : "")}>(\"{property.Name}\"));" : "")}";
        }
        public static void GenerateAddDbContextConfigurations(this Project project)
        {
            var path = @$"src\{project.Name}.Middleware\AddServices";
            var filePath = Path.Combine(path, $"AddDbContextConfigurations.cs");
            var source = @"// TODO: INTERFACES REFERENCES
// TODO: CONTEXTS REFERENCES
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BAYSOFT.Middleware.AddServices
{
    public static class AddDbContextConfigurations
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            // TODO: READER & WRITER

            // TODO: CONTEXT

            return services;
        }
        public static IServiceCollection AddDbContextsTest(this IServiceCollection services, IConfiguration configuration)
        {
            // TODO: READER & WRITER

            // TODO: TEST CONTEXT

            return services;
        }
    }
}
";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .ConditionalReplace(
                        project.Contexts.Any(),
                        "// TODO: INTERFACES REFERENCES",
                        $"{string.Join(
                            "\n",
                            project.Contexts
                                .Select(context => $"using {context.Project.Name}.Core.Domain.{context.Name}.Interfaces.Infrastructures.Data;")
                                .ToList())}")
                    .ConditionalReplace(
                        project.Contexts.Any(),
                        "// TODO: CONTEXTS REFERENCES",
                        $"{string.Join(
                            "\n",
                            project.Contexts
                                .Select(context => $"using {context.Project.Name}.Infrastructures.Data.{context.Name};")
                                .ToList())}")
                    .ConditionalReplace(
                        project.Contexts.Any(),
                        "// TODO: READER & WRITER",
                        $"{string.Join(
                            "\n\n\t\t\t",
                            project.Contexts
                                .Select(context => $"services.AddTransient<I{context.Name}DbContextWriter, {context.Name}DbContextWriter>();\r\n\t\t\tservices.AddTransient<I{context.Name}DbContextReader, {context.Name}DbContextReader>();")
                                .ToList())}")
                    .ConditionalReplace(
                        project.Contexts.Any(),
                        "// TODO: CONTEXT",
                        $"{string.Join(
                            "\n\n\t\t\t",
                            project.Contexts
                                .Select(context => $"services.AddDbContext<{context.Name}DbContext>(options =>\r\n\t\t\t\toptions.UseSqlServer(\r\n\t\t\t\t\tconfiguration.GetConnectionString(\"{context.Name}Connection\"),\r\n\t\t\t\t\tsql => {{ \r\n\t\t\t\t\t\tsql.MigrationsAssembly(typeof({context.Name}DbContext).Assembly.GetName().Name);\r\n\t\t\t\t\t\tsql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null); \r\n\t\t\t\t\t}}));")
                                .ToList())}")
                    .ConditionalReplace(
                        project.Contexts.Any(),
                        "// TODO: TEST CONTEXT",
                        $"{string.Join(
                            "\n\n\t\t\t",
                            project.Contexts
                                .Select(context => $"services.AddDbContext<{context.Name}DbContext>(options => \r\n\t\t\t\toptions.UseInMemoryDatabase(nameof({context.Name}DbContext), new InMemoryDatabaseRoot()),\r\n\t\t\t\tServiceLifetime.Singleton);")
                                .ToList())}")
                    .Replace("BAYSOFT.Core", $"{project.Name}.Core")
                    .Replace("BAYSOFT.Middleware", $"{project.Name}.Middleware")
            );
        }
        public static void GenerateAddDomainServicesConfigurations(this Project project)
        {
            var path = @$"src\{project.Name}.Middleware\AddServices";
            var filePath = Path.Combine(path, $"AddDomainServicesConfigurations.cs");
            var source = @"using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Services;
using BAYSOFT.Infrastructures.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BAYSOFT.Middleware.AddServices
{
    public static class AddDomainServicesConfigurations
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("BAYSOFT.Core", $"{project.Name}.Core")
                    .Replace("BAYSOFT.Middleware", $"{project.Name}.Middleware")
                    .Replace("BAYSOFT.Infrastructures", $"{project.Name}.Infrastructures")
            );
        }
        public static void GenerateAddValidationsConfigurations(this Project project)
        {
            var path = @$"src\{project.Name}.Middleware\AddServices";
            var filePath = Path.Combine(path, $"AddValidationsConfigurations.cs");
            var source = @"// TODO: REFERENCES
using BAYSOFT.Core.Domain.Default.Samples.Specifications;
using BAYSOFT.Core.Domain.Default.Samples.Validations.DomainValidations;
using BAYSOFT.Core.Domain.Default.Samples.Validations.EntityValidations;
using Microsoft.Extensions.DependencyInjection;

namespace BAYSOFT.Middleware.AddServices
{
    public static class AddValidationsConfigurations
    {
        public static IServiceCollection AddSpecifications(this IServiceCollection services)
        {
            // TODO: SPECIFICATIONS

            return services;
        }
        public static IServiceCollection AddEntityValidations(this IServiceCollection services)
        {
            // TODO: VALIDATIONS

            return services;
        }
        public static IServiceCollection AddDomainValidations(this IServiceCollection services)
        {
            // TODO: SERVICES

            return services;
        }
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .ConditionalReplace(
                        project.Contexts.SelectMany(context => context.Entities).Any(),
                        "// TODO: REFERENCES",
                        $"{string.Join(
                            "\n",
                            project.Contexts
                                .SelectMany(context => context.Entities)
                                .Select(entity => $"using {entity.Context.Project.Name}.Core.Domain.{entity.Context.Name}.{entity.Name.Pluralize()}.Specifications;\r\nusing {entity.Context.Project.Name}.Core.Domain.{entity.Context.Name}.{entity.Name.Pluralize()}.Validations.DomainValidations;\r\nusing {entity.Context.Project.Name}.Core.Domain.{entity.Context.Name}.{entity.Name.Pluralize()}.Validations.EntityValidations;")
                                .ToList()
                        )}"
                    )
                    .ConditionalReplace(
                        project.Contexts.SelectMany(context => context.Entities.SelectMany(e => e.Specifications)).Any(),
                        "// TODO: SPECIFICATIONS",
                        $"{string.Join(
                            "\n\t\t\t",
                            project.Contexts
                                .SelectMany(context => context.Entities.SelectMany(e => e.Specifications))
                                .Select(specification => $"services.AddTransient<{specification.Name}Specification>();")
                                .ToList()
                        )}"
                    )
                    .ConditionalReplace(
                        project.Contexts.SelectMany(context => context.Entities).Any(),
                        "// TODO: VALIDATIONS",
                        $"{string.Join(
                            "\n\t\t\t",
                            project.Contexts
                                .SelectMany(context => context.Entities)
                                .Select(entity => $"services.AddTransient<{entity.Name}Validator>();")
                                .ToList()
                        )}"
                    )
                    .ConditionalReplace(
                        project.Contexts.SelectMany(context => context.Entities.SelectMany(e => e.Services)).Any(),
                        "// TODO: SERVICES",
                        $"{string.Join(
                            "\n\t\t\t",
                            project.Contexts
                                .SelectMany(context => context.Entities.SelectMany(e => e.Services))
                                .Select(service => $"services.AddTransient<{service.Name}SpecificationsValidator>();")
                                .ToList()
                        )}"
                    )
                    .Replace("BAYSOFT.Core", $"{project.Name}.Core")
                    .Replace("BAYSOFT.Middleware", $"{project.Name}.Middleware")
                    .Replace("BAYSOFT.Infrastructures", $"{project.Name}.Infrastructures")
            );
        }
        public static void GenerateControllerFile(this Entity entity)
        {
            var path = @$"src\{entity.Context.Project.Name}.Presentations.Web.API\Resources";
            var filePath = Path.Combine(path, $"{entity.Name.Pluralize()}Controller.cs");
            var source = @"using BAYSOFT.Core.Application.Default.Samples.Commands;
using BAYSOFT.Core.Application.Default.Samples.Queries;
using BAYSOFT.Presentations.Web.API.Abstractions.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Presentations.Web.API.Resources
{
    public class SamplesController : ResourceController
    {
        // TODO: QUERY GETBYFILTER
        // TODO: QUERY GETBYID
        // TODO: COMMAND ACTIONS
    }
}";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("BAYSOFT.Presentations", $"{entity.Context.Project.Name}.Presentations")
                    .Replace("Default", entity.Context.Name)
                    .Replace("Samples", entity.Name.Pluralize())
                    .Replace("Sample", entity.Name)
                    .ConditionalReplace(
                        entity.GetByFilter,
                        "// TODO: QUERY GETBYFILTER",
                        $"[HttpGet]\r\n\t\tpublic async Task<ActionResult<Get{entity.Name.Pluralize()}ByFilterQueryResponse>> Get(Get{entity.Name.Pluralize()}ByFilterQuery request, CancellationToken cancellationToken = default(CancellationToken))\r\n\t\t{{\r\n\t\t\treturn await Send(request, cancellationToken);\r\n\t\t}}\n"
                    )
                    .ConditionalReplace(
                        entity.GetById,
                        "// TODO: QUERY GETBYID",
                        $"[HttpGet(\"{{{entity.Properties.Where(x => x.IsPrimaryKey).SingleOrDefault().Name.ToCamelCase()}:{entity.Properties.Where(x => x.IsPrimaryKey).SingleOrDefault().Type}}}\")]\r\n\t\tpublic async Task<ActionResult<Get{entity.Name}ByIdQueryResponse>> Get(Get{entity.Name}ByIdQuery request, CancellationToken cancellationToken = default(CancellationToken))\r\n\t\t{{\r\n\t\t\treturn await Send(request, cancellationToken);\r\n\t\t}}\n"
                    )
                    .ConditionalReplace(
                        entity.Commands.Any(),
                        "// TODO: COMMAND ACTIONS",
                        $"{string.Join(
                            "\r\n\r\n\t\t",
                            entity.Commands
                                .Select(c => $"[Http{c.HttpMethod}{(!c.HttpMethod.Equals("Post") ? $"(\"{{{entity.Properties.Where(x => x.IsPrimaryKey).SingleOrDefault().Name.ToCamelCase()}:{entity.Properties.Where(x => x.IsPrimaryKey).SingleOrDefault().Type}}}\")" : "")}]\r\n\t\tpublic async Task<ActionResult<{c.Name}CommandResponse>> {c.HttpMethod}({c.Name}Command request, CancellationToken cancellationToken = default(CancellationToken))\r\n\t\t{{\r\n\t\t\treturn await Send(request, cancellationToken);\r\n\t\t}}")
                                .ToList())}"
                    )
            );
        }
        public static void GenerateReactFormFile(this Entity entity)
        {
            var path = @$"src\{entity.Context.Project.Name}.Presentations.Web.SPA.ReactJS\client-app\src\components\organisms";
            var filePath = Path.Combine(path, $"Form{entity.Name}.js");
            var source = @"import React, { useState, useEffect } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';

import { push } from 'connected-react-router';

import { FormControl, TextField, Button, Grid } from '@mui/material';

import { styled } from '@mui/material/styles';

import {
    ApplicationDialogShow, ApplicationDialogClose, ApplicationDialogAddAction
} from '../../state/actions/application/actions';
import { CreateApiService } from '../../state/actions/apiModelWrapper/actions';
import { ServerUrl } from '../atoms/ServerUrlHelper';

const StyledRootGrid = styled(Grid)(({ theme }) => ({
    ...theme.typography.body2,
    display: 'flex',
    flexWrap: 'wrap',
}));

const StyledFormControl = styled(FormControl)(({ theme }) => ({
    ...theme.typography.body2,
    padding: theme.spacing(1)
}));


// TODO: AGGREGATE COLLECTIONS
const COLLECTION = ""samples"";

const FormComponent = (props) => {
    const { parentComponentId, id, /* TODO: AGGREDATE IDS */params } = props;
    const COMPONENT_ID = `${parentComponentId}-${COLLECTION}-form`;
    const endPoint = ServerUrl(`api/${COLLECTION}`);
    // TODO: AGGREGATE URLS
    const returnUrl = `/${COLLECTION}`;
    const { entities, posts, puts } = props;
    const [requestUrl, setRequestUrl] = useState('');
    const [validations, setValidations] = useState([]);
    const [updatedEntity, setUpdatedEntity] = useState({ description: '' });
    const [dbEntity, setDbEntity] = useState(null);
    const api = props.CreateApiService(`${COMPONENT_ID}-service`, endPoint);

    const handleChange = (prop) => (event) => {
        setUpdatedEntity({ ...updatedEntity, [prop]: event.target.value });
    };
    const handleClickSave = () => {
        setValidations(null);
        console.log({ updatedEntity });
        if (id) {
            api.Put(id, updatedEntity, returnUrl);
        } else {
            api.Post(updatedEntity, returnUrl);
        }
    };
    useEffect(() => {
        if (id) {
            setRequestUrl(api.GetById(id));
        }
    }, [api, id, params]);
    useEffect(() => {
        if (id && entities && entities[requestUrl] && entities[requestUrl].response && entities[requestUrl].response.data !== dbEntity) {
            setDbEntity(entities[requestUrl].response.data);
            setUpdatedEntity(entities[requestUrl].response.data);
        }
    }, [id, entities, requestUrl, dbEntity, setDbEntity, setUpdatedEntity]);
    useEffect(() => {
        if (posts && posts[requestUrl] && posts[requestUrl].entityValidations && posts[requestUrl].entityValidations) {
            setValidations(posts[requestUrl].entityValidations);
        }
    }, [posts, requestUrl]);
    useEffect(() => {
        if (puts && puts[requestUrl] && puts[requestUrl].entityValidations && puts[requestUrl].entityValidations) {
            setValidations(puts[requestUrl].entityValidations);
        }
    }, [puts, requestUrl]);
    return (
        <StyledRootGrid container spacing={0}>
            {/* TODO: AGGREGATE FIELDS */}

            {/* TODO: PROPERTY FIELDS */}

            <Grid container spacing={0} justifyContent=""flex-end"" >
                <Grid item lg={2} md={4} xs={6}>
                    <StyledFormControl fullWidth >
                        <Button variant=""contained"" color=""primary"" onClick={handleClickSave}>Save</Button>
                    </StyledFormControl>
                </Grid>
            </Grid>
        </StyledRootGrid>
    );
};

const mapStateToProps = store => ({
    application: store.ApplicationState.application,
    entities: store.ApiModelWrapperState.queries.entities,
    puts: store.ApiModelWrapperState.commands.puts
});

const mapDispatchToProps = dispatch => bindActionCreators({
    push,
    CreateApiService,
    ApplicationDialogShow,
    ApplicationDialogClose,
    ApplicationDialogAddAction
}, dispatch);

const connectedComponent = connect(mapStateToProps, mapDispatchToProps)(FormComponent);

export default connectedComponent;";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("samples", entity.Name.Pluralize().ToCamelCase())
                    .Replace("sample", entity.Name.ToCamelCase())
                    .ConditionalReplace(
                        entity.Properties.Where(p => !p.IsPrimaryKey && p.IsForeignKey).Any(),
                        "// TODO: AGGREGATE COLLECTIONS",
                        $"{string.Join(
                            "\n",
                            entity.Properties.Where(p => !p.IsPrimaryKey && p.IsForeignKey)
                                .Select(p => $"const AGGREGATE_COLLECTION_{p.RelatedEntityName.Pluralize().ToUpper()} = \"{p.RelatedEntityName.Pluralize().ToCamelCase()}\";")
                                .ToList()
                        )}"
                    )
                    .ConditionalReplace(
                        entity.Properties.Where(p => !p.IsPrimaryKey && p.IsForeignKey).Any(),
                        "/* TODO: AGGREDATE IDS */",
                        $"{string.Join(
                            "",
                            entity.Properties.Where(p => !p.IsPrimaryKey && p.IsForeignKey)
                                .Select(p => $"aggregate{p.Name}, ")
                                .ToList()
                        )}"
                    )
                    .ConditionalReplace(
                        entity.Properties.Where(p => !p.IsPrimaryKey && p.IsForeignKey).Any(),
                        "// TODO: AGGREGATE URLS",
                        $"{string.Join(
                            "\n\t",
                            entity.Properties.Where(p => !p.IsPrimaryKey && p.IsForeignKey)
                                .Select(p => $"const {p.RelatedEntityName.Pluralize().ToCamelCase()}AggregateEndPoint = ServerUrl(`api/${{AGGREGATE_COLLECTION_{p.RelatedEntityName.Pluralize().ToUpper()}}}`);")
                                .ToList()
                        )}"
                    )
                    .ConditionalReplace(
                        entity.Properties.Where(p => !p.IsPrimaryKey && p.IsForeignKey).Any(),
                        "`/${COLLECTION}`",
                        entity.Properties.Where(p => !p.IsPrimaryKey && p.IsForeignKey).GenerateReactAggergateUrl()
                    )
                    .ConditionalReplace(
                        entity.Properties.Where(p => !p.IsPrimaryKey && p.IsForeignKey).Any(),
                        "{/* TODO: AGGREGATE FIELDS */}",
                        $"{string.Join(

                            "\n\n\t\t\t",
                            entity.Properties.Where(p => !p.IsPrimaryKey && p.IsForeignKey)
                                .Select(p => $"<Grid container spacing={{0}} >\r\n\t\t\t\t<Grid item xs={{12}}>\r\n\t\t\t\t\t<StyledFormControl fullWidth>\r\n\t\t\t\t\t\t<ApiConnectedAutocomplete\r\n\t\t\t\t\t\t\terror={{validations && validations.{p.Name.ToCamelCase()} && validations.{p.Name.ToCamelCase()}.length > 0}}\r\n\t\t\t\t\t\t\thelperText={{validations && validations.{p.Name.ToCamelCase()} ? validations.{p.Name.ToCamelCase()}.join(' ') : ''}}\r\n\t\t\t\t\t\t\tid='{p.Name.ToCamelCase()}' connectedId={{COMPONENT_ID}}\r\n\t\t\t\t\t\t\tlabel=\"{p.DisplayName}\" optionValue=\"id\" optionText=\"email\" defaultText=\"Selecione...\"\r\n\t\t\t\t\t\t\tdisabled={{aggregate{p.Name} ? true : false}} endPoint={{{p.RelatedEntityName.Pluralize().ToCamelCase()}AggregateEndPoint}}\r\n\t\t\t\t\t\t\tvalue={{updatedEntity?.{p.Name.ToCamelCase()} || ''}} onChange={{handleAutocompleteChange('{p.Name.ToCamelCase()}')}}\r\n\t\t\t\t\t\t/>\r\n\t\t\t\t\t</StyledFormControl>\r\n\t\t\t\t</Grid>\r\n\t\t\t</Grid>")
                                .ToList()
                        )}"
                    )
                    .ConditionalReplace(
                        entity.Properties.Where(p => !p.IsPrimaryKey && !p.IsForeignKey).Any(),
                        "{/* TODO: PROPERTY FIELDS */}",
                        $"{string.Join(
                            "\n\n\t\t\t",
                            entity.Properties.Where(p => !p.IsPrimaryKey && !p.IsForeignKey)
                                .Select(p => $"<Grid container spacing={{0}} >\r\n\t\t\t\t<Grid item xs={{12}}>\r\n\t\t\t\t\t<StyledFormControl fullWidth >\r\n\t\t\t\t\t\t<TextField\r\n\t\t\t\t\t\t\terror={{validations && validations.{p.Name.ToCamelCase()} && validations.{p.Name.ToCamelCase()}.length > 0}}\r\n\t\t\t\t\t\t\thelperText={{validations && validations.{p.Name.ToCamelCase()} ? validations.{p.Name.ToCamelCase()}.join(' ') : ''}}\r\n\t\t\t\t\t\t\tkey=\"{p.Name.ToCamelCase()}\" id=\"{p.Name.ToCamelCase()}\" label=\"{p.DisplayName}\" variant=\"outlined\"\r\n\t\t\t\t\t\t\tvalue={{updatedEntity.{p.Name.ToCamelCase()} || ''}} onChange={{handleChange('{p.Name.ToCamelCase()}')}} />\r\n\t\t\t\t\t</StyledFormControl>\r\n\t\t\t\t</Grid>\r\n\t\t\t</Grid>")
                                .ToList()
                        )}"
                    )
            );
        }
        public static void GenerateReactTableFile(this Entity entity)
        {
            var path = @$"src\{entity.Context.Project.Name}.Presentations.Web.SPA.ReactJS\client-app\src\components\organisms";
            var filePath = Path.Combine(path, $"Table{entity.Name.Pluralize()}.js");
            var source = @"import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { push } from 'connected-react-router';

import ApiConnectedTable from './ApiConnectedTable';

import { ApplicationDialogShow, ApplicationDialogClose, ApplicationDialogAddAction } from '../../state/actions/application/actions';
import { CreateApiService } from '../../state/actions/apiModelWrapper/actions';
import { CheckAccess } from '../../state/actions/signInManager/actions';
import { ServerUrl } from '../atoms/ServerUrlHelper';

const COLLECTION = ""samples"";
const AccessFunctionAdd = (dispatch, state) => CheckAccess(COLLECTION, ""CREATE"", null, true, false, null)(dispatch, state);
const AccessFunctionEdit = (dispatch, state) => CheckAccess(COLLECTION, ""EDIT"", null, true, false, null)(dispatch, state);
const AccessFunctionDelete = (dispatch, state) => CheckAccess(COLLECTION, ""DELETE"", null, true, false, null)(dispatch, state);

const TableComponent = (props) => {
    const { parentComponentId } = props;
    const COMPONENT_ID = `${parentComponentId}-${COLLECTION}-table`;
    const api = props.CreateApiService(`${COMPONENT_ID}-service`, ServerUrl(`api/${COLLECTION}`));
    const config = {
        elevation: props.elevation||1,
        title: 'List of samples',
        configId: COMPONENT_ID,
        endPoint: ServerUrl(`api/${COLLECTION}`),
        id: 'id',
        dense: false,
        defaultPageSize: 5,
        columns: [/* TODO: COLUMNS */],
        actions: {
            'add': { access: AccessFunctionAdd, handler: () => { addHandler(); } },
            'edit': { access: AccessFunctionEdit, handler: (id) => { editHandler(id); } },
            'delete': { access: AccessFunctionDelete, handler: (ids, callback) => { deleteHandler(ids, callback); } },
            //'filter': { component: null }
        }
    };
    const deleteHandler = (ids, callback) => {
        let actions = [];
        console.log(props.ApplicationDialogAddAction);
        actions.push(props.ApplicationDialogAddAction('Cancel', 'outlined', 'error', () => { props.ApplicationDialogClose(); }));
        actions.push(props.ApplicationDialogAddAction('Delete', 'contained', 'error', () => { ids.map(id => api.Delete(id)); props.ApplicationDialogClose(); callback(); }));

        props.ApplicationDialogShow('Confirm', 'Do you want to confirm the delete action?', actions);
    };
    const addHandler = () => {
        props.push(`/${COLLECTION}/create`);
    };
    const editHandler = (id) => {
        props.push(`/${COLLECTION}/${id}`);
    };
    return (<ApiConnectedTable config={config} />);
};

const mapStateToProps = store => ({
    application: store.ApplicationState.application
});

const mapDispatchToProps = dispatch => bindActionCreators({
    push,
    CreateApiService,
    ApplicationDialogShow,
    ApplicationDialogClose,
    ApplicationDialogAddAction
}, dispatch);

const connectedComponent = connect(mapStateToProps, mapDispatchToProps)(TableComponent);

export default connectedComponent;";

            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("samples", entity.Name.Pluralize().ToCamelCase())
                    .Replace("sample", entity.Name.ToCamelCase())
                    .ConditionalReplace(
                        entity.Properties.Where(p => p.IsPrimaryKey).Any(),
                        "id: 'id',",
                        $"id: '{entity.Properties.Where(p => p.IsPrimaryKey).SingleOrDefault().Name.ToCamelCase()}',"
                    )
                    .ConditionalReplace(
                        entity.Properties.Where(p => !p.IsPrimaryKey).Any(),
                        "/* TODO: COLUMNS */",
                        $"{string.Join(
                            ", ",
                            entity.Properties.Where(p => !p.IsPrimaryKey)
                                .Select(p => $"{{\r\n\t\t\tid: '{p.Name.ToCamelCase()}',\r\n\t\t\tdisablePadding: false,\r\n\t\t\tlabel: '{p.DisplayName}'\r\n\t\t}}")
                                .ToList()
                        )}"
                    )
            );
        }
        public static void GenerateReactTabsFile(this Entity entity)
        {
            var path = @$"src\{entity.Context.Project.Name}.Presentations.Web.SPA.ReactJS\client-app\src\components\organisms";
            var filePath = Path.Combine(path, $"Tabs{entity.Name.Pluralize()}.js");
            var source = @"import React from 'react';
import PropTypes from 'prop-types';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';

import { push } from 'connected-react-router';

import { Grid, AppBar, Tabs, Tab, Box } from '@mui/material';

import { /* TODO: TABLE REFERENCES */ } from '.';

import { styled } from '@mui/material/styles';

const StyledTabUserRoot = styled(Grid)(({theme})=>({
    display: 'flex',
    flexWrap: 'wrap',
}));

const StyledTabsRoot = styled('div')(({theme})=>({
    flexGrow: 1,
    width: '100%',
    backgroundColor: theme.palette.background.paper,
}));

function TabPanel(props) {
    const { children, value, index, ...other } = props;

    return (
        <div
            role=""tabpanel""
            hidden={value !== index}
            id={`scrollable-force-tabpanel-${index}`}
            aria-labelledby={`scrollable-force-tab-${index}`}
            {...other}
        >
            {value === index && (
                <Box>{children}</Box>
            )}
        </div>
    );
}

TabPanel.propTypes = {
    children: PropTypes.node,
    index: PropTypes.any.isRequired,
    value: PropTypes.any.isRequired,
}

function a11yProps(index) {
    return {
        id: `scrollable-force-tab-${index}`,
        'aria-controls': `scrollable-force-tabpanel-${index}`,
    };
}

const TabsComponent = (props) => {
    const { parentComponentId, id } = props;
    const COMPONENT_ID = `${parentComponentId}-tabs`;
    const [tabIndex, setTabIndex] = React.useState(0);

    const handleChangeTab = (event, index) => {
        setTabIndex(index);
    };

    return (
        <StyledTabUserRoot container spacing={0}>
            {
                id ?
                    (
                        <Grid container spacing={0} >
                            <Grid item xs={12}>
                                <StyledTabsRoot>
                                    <AppBar position=""static"" color=""default"" elevation={0}>
                                        <Tabs scrollButtons allowScrollButtonsMobile
                                            indicatorColor=""primary"" textColor=""primary"" variant={""scrollable""} aria-label=""scrollable force tabs example""
                                            value={tabIndex} onChange={handleChangeTab}>
                                            {/*TODO: TABLABEL*/}
                                        </Tabs>
                                    </AppBar>
                                    {/*TODO: TABPANEL*/}
                                </StyledTabsRoot>
                            </Grid>
                        </Grid>
                    )
                    : null
            }
        </StyledTabUserRoot>
    );
};

const mapStateToProps = store => ({
    application: store.ApplicationState.application,
});

const mapDispatchToProps = dispatch => bindActionCreators({
    push,
}, dispatch);

const connectedComponent = connect(mapStateToProps, mapDispatchToProps)(TabsComponent);

export default connectedComponent;";
            int indexTab = 0;
            int indexTable = 0;
            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("samples", entity.Name.Pluralize().ToCamelCase())
                    .Replace("sample", entity.Name.ToCamelCase())
                    .ConditionalReplace(
                        entity.Context.Entities.SelectMany(e => e.Properties.Where(p => p.IsForeignKey).Select(p => p.RelatedEntityName)).ToList().Where(re => re == entity.Name).Any(),
                        "/* TODO: TABLE REFERENCES */",
                        $"{string.Join(
                            ", ",
                            entity.Context.Entities.SelectMany(e => e.Properties.Where(p => p.IsForeignKey).Select(p => p.RelatedEntityName)).ToList().Where(re => re == entity.Name)
                                .Select(relatedEntity => $"Table{relatedEntity.Pluralize()}")
                                .ToList()
                        )}"
                    )
                    .ConditionalReplace(
                        entity.Context.Entities.SelectMany(e => e.Properties.Where(p => p.IsForeignKey).Select(p => p.RelatedEntityName)).ToList().Where(re => re == entity.Name).Any(),
                        "{/*TODO: TABLABEL*/}",
                        $"{string.Join(
                            "\n\t\t\t\t\t\t\t\t\t\t\t",
                            entity.Context.Entities.SelectMany(e => e.Properties.Where(p => p.IsForeignKey).Select(p => p.RelatedEntityName)).ToList().Where(re => re == entity.Name)
                                .Select(relatedEntity => $"<Tab label=\"{relatedEntity.Pluralize()}\" {{...a11yProps({indexTab++})}} />")
                                .ToList()
                        )}"
                    )
                    .ConditionalReplace(
                        entity.Context.Entities.SelectMany(e => e.Properties.Where(p => p.IsForeignKey).Select(p => p.RelatedEntityName)).ToList().Where(re => re == entity.Name).Any(),
                        "{/*TODO: TABPANEL*/}",
                        $"{string.Join(
                            "\n\t\t\t\t\t\t\t\t\t",
                            entity.Context.Entities.SelectMany(e => e.Properties.Where(p => p.IsForeignKey).Select(p => p.RelatedEntityName)).ToList().Where(re => re == entity.Name)
                                .Select(relatedEntity => $"<TabPanel value={{tabIndex}} index={{{indexTable++}}}><Table{relatedEntity.Pluralize()} parentComponentId={{COMPONENT_ID}} aggregateId={{id}} /></TabPanel>")
                                .ToList()
                        )}"
                    )
            );
        }
        public static void GenerateReactPageIndexFile(this Entity entity)
        {
            var path = @$"src\{entity.Context.Project.Name}.Presentations.Web.SPA.ReactJS\client-app\src\components\pages\{entity.Name.Pluralize().ToCamelCase()}";
            var filePath = Path.Combine(path, $"PageIndex.js");
            var source = @"import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';

import { Page } from '../../molecules';
import { TableSamples } from '../../organisms';

import { CheckAccess } from '../../../state/actions/signInManager/actions';

const COLLECTION = ""samples"";
const ACTION = ""INDEX"";

const PageIndex = (props) => {
    const COMPONENT_ID = `${COLLECTION}-${ACTION.toLowerCase()}`;
    return (
        <Page>
            <TableSamples parentComponentId={COMPONENT_ID} elevation={0} />
        </Page>
    );
}

const mapStateToProps = store => ({});

const mapDispatchToProps = dispatch =>
    bindActionCreators({}, dispatch);

const connectedComponent = connect(mapStateToProps, mapDispatchToProps)(PageIndex);

const AccessFunction = (dispatch, state) => CheckAccess(COLLECTION, ACTION.toUpperCase(), null, true, false, null)(dispatch, state);

export const routes = [
    { access: AccessFunction, name: `${COLLECTION.toUpperCase()}`, path: `/${COLLECTION}`, params: [], component: connectedComponent },
    { access: AccessFunction, name: `${COLLECTION.toUpperCase()}_${ACTION.toUpperCase()}`, path: `/${COLLECTION}/${ACTION.toLowerCase()}`, params: [], component: connectedComponent }
];

export default connectedComponent;";
            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("Samples", entity.Name.Pluralize())
                    .Replace("samples", entity.Name.Pluralize().ToCamelCase())
                    .Replace("sample", entity.Name.ToCamelCase())
            );
        }
        public static void GenerateReactPageCreateFile(this Entity entity)
        {
            var path = @$"src\{entity.Context.Project.Name}.Presentations.Web.SPA.ReactJS\client-app\src\components\pages\{entity.Name.Pluralize().ToCamelCase()}";
            var filePath = Path.Combine(path, $"PageCreate.js");
            var source = @"import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';

import { Page } from '../../molecules';
import { FormSample } from '../../organisms';

import { CheckAccess } from '../../../state/actions/signInManager/actions';

const COLLECTION = ""samples"";
const ACTION = ""Create"";

const PageCreate = (props) => {
    const COMPONENT_ID = `${COLLECTION}-${ACTION.toLowerCase()}`;
    const title = `${ACTION} new sample`;
    const returnUrl = `/${COLLECTION}`;
    return (
        <Page title={title} returnUrl={returnUrl}>
            <FormSample parentComponentId={COMPONENT_ID} />
        </Page>
    );
};

const mapStateToProps = store => ({});

const mapDispatchToProps = dispatch => bindActionCreators({}, dispatch);

const connectedComponent = connect(mapStateToProps, mapDispatchToProps)(PageCreate);

const AccessFunction = (dispatch, state) => CheckAccess(COLLECTION, ACTION.toUpperCase(), null, true, false, null)(dispatch, state);

export const routes = [
    { access: AccessFunction, name: `${COLLECTION.toUpperCase()}_${ACTION.toUpperCase()}`, path: `/${COLLECTION}/${ACTION.toLowerCase()}`, params: [], component: connectedComponent },
];

export default connectedComponent;";
            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("Samples", entity.Name.Pluralize())
                    .Replace("Sample", entity.Name)
                    .Replace("samples", entity.Name.Pluralize().ToCamelCase())
                    .Replace("sample", entity.Name.ToCamelCase())
            );
        }
        public static void GenerateReactPageEditFile(this Entity entity)
        {
            var path = @$"src\{entity.Context.Project.Name}.Presentations.Web.SPA.ReactJS\client-app\src\components\pages\{entity.Name.Pluralize().ToCamelCase()}";
            var filePath = Path.Combine(path, $"PageEdit.js");
            var source = @"import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';

import { Page } from '../../molecules';
import { FormSample } from '../../organisms';

import { CheckAccess } from '../../../state/actions/signInManager/actions';

const COLLECTION = ""samples"";
const ACTION = ""Edit"";

const PageEdit = (props) => {
    const { id } = props.match.params;
    const COMPONENT_ID = `${COLLECTION}-${ACTION.toLowerCase()}`;
    const title = `${ACTION} sample`;
    const returnUrl = `/${COLLECTION}`;
    return (
        <Page title={title} returnUrl={returnUrl}>
            <FormSample parentComponentId={COMPONENT_ID} id={id} params={null} />
        </Page>
    );
};

const mapStateToProps = store => ({});

const mapDispatchToProps = dispatch => bindActionCreators({}, dispatch);

const connectedComponent = connect(mapStateToProps, mapDispatchToProps)(PageEdit);

const AccessFunction = (dispatch, state) => CheckAccess(COLLECTION, ACTION.toUpperCase(), null, true, false, null)(dispatch, state);

export const routes = [
    { access: AccessFunction, name: `${COLLECTION.toUpperCase()}_${ACTION.toUpperCase()}`, path: `/${COLLECTION}/:id`, params: ['id'], component: connectedComponent },
];

export default connectedComponent;";
            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .Replace("Samples", entity.Name.Pluralize())
                    .Replace("Sample", entity.Name)
                    .Replace("samples", entity.Name.Pluralize().ToCamelCase())
                    .Replace("sample", entity.Name.ToCamelCase())
            );
        }
        public static void GenerateReactPagesIndexFile(this Entity entity)
        {
            var path = @$"src\{entity.Context.Project.Name}.Presentations.Web.SPA.ReactJS\client-app\src\components\pages\{entity.Name.Pluralize().ToCamelCase()}";
            var filePath = Path.Combine(path, $"index.js");
            var source = @"import PageIndex, { routes as RouteIndex } from './PageIndex';
import PageCreate, { routes as RouteCreate } from './PageCreate';
import PageEdit, { routes as RouteEdit } from './PageEdit';

const index = {
    routes: [
        ...RouteIndex,
        ...RouteCreate,
        ...RouteEdit
    ],
    pages: {
        Index: PageIndex,
        Create: PageCreate,
        Edit: PageEdit
    }
};

export default index;";
            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
            );
        }
        public static void GenerateReactOrganismsIndexFile(this Context context)
        {
            var path = @$"src\{context.Project.Name}.Presentations.Web.SPA.ReactJS\client-app\src\components\organisms";
            var filePath = Path.Combine(path, $"index.js");
            var source = @"import ApiConnectedTable from './ApiConnectedTable';
import ApiConnectedAutocomplete from './ApiConnectedAutocomplete';

// TODO: IMPORT FORMS

// TODO: IMPORT TABLES

// TODO: IMPORT TABS

export {
    ApiConnectedTable, 
    ApiConnectedAutocomplete,
    // TODO: EXPORT FORMS
    // TODO: EXPORT TABLES
    // TODO: EXPORT TABS
};";
            Directory.CreateDirectory(path);
            File.WriteAllText(
                filePath,
                source
                    .ConditionalReplace(
                        context.Entities.Any(),
                        "// TODO: IMPORT FORMS",
                        $"{string.Join("\n", context.Entities.Select(e => $"import Form{e.Name} from './Form{e.Name}';").ToList())}"
                    )
                    .ConditionalReplace(
                        context.Entities.Any(),
                        "// TODO: IMPORT TABLES",
                        $"{string.Join("\n", context.Entities.Select(e => $"import Table{e.Name.Pluralize()} from './Table{e.Name.Pluralize()}';").ToList())}"
                    )
                    .ConditionalReplace(
                        context.Entities.SelectMany(e => e.Properties.Where(p => p.IsForeignKey).Select(p => p.RelatedEntityName)).Any(),
                        "// TODO: IMPORT TABS",
                        $"{string.Join("\n", context.Entities.SelectMany(e => e.Properties.Where(p => p.IsForeignKey).Select(p => p.RelatedEntityName).Distinct().Select(relatedEntity => $"import Tabs{relatedEntity} from './Tabs{relatedEntity}';")))}"
                    )
                    .ConditionalReplace(
                        context.Entities.Any(),
                        "// TODO: EXPORT FORMS",
                        $"{string.Join("\n\t", context.Entities.Select(e => $"Form{e.Name},").ToList())}"
                    )
                    .ConditionalReplace(
                        context.Entities.Any(),
                        "// TODO: EXPORT TABLES",
                        $"{string.Join("\n\t", context.Entities.Select(e => $"Table{e.Name.Pluralize()},").ToList())}"
                    )
                    .ConditionalReplace(
                        context.Entities.SelectMany(e => e.Properties.Where(p => p.IsForeignKey).Select(p => p.RelatedEntityName)).Any(),
                        "// TODO: EXPORT TABS",
                        $"{string.Join("\n\t", context.Entities.SelectMany(e => e.Properties.Where(p => p.IsForeignKey).Select(p => p.RelatedEntityName).Distinct().Select(relatedEntity => $"Tabs{relatedEntity},")))}"
                    )
            );
        }
        public static string GenerateRule(this EntityRule rule)
        {
            return $"RuleFor(x => x.{rule.Property.Name}).{rule.Rule}.WithMessage(\"{rule.Message}\");";
        }
        private static string GenerateReactAggergateUrl(this IEnumerable<Property> properties)
        {
            var result = "`/${COLLECTION}`";

            foreach (var property in properties)
            {
                result = $"aggregate{property.Name} ? `/${{AGGREGATE_COLLECTION_{property.RelatedEntityName.Pluralize().ToUpper()}}}/${{aggregate{property.Name}}}` : {result}";
            }

            return result;
        }
    }
}

public static class StringExtensions
{
    public static string Pluralize(this string singular)
    {
        Pluralizer pluralizer = new Pluralizer();

        return pluralizer.Pluralize(singular);
    }
    public static string ToCamelCase(this string text)
    {
        return Char.ToLowerInvariant(text[0]) + text.Substring(1);
    }
    public static string ConditionalReplace(this string source, bool condition, string value, string replaced)
    {
        return condition ? source.Replace(value, replaced) : source;
    }
    public static string ReplaceSpecialCharacters(this string source)
    {
        string replacement = string.Empty;
        foreach (char c in source.ToArray())
        {
            if (c.KeepChar())
            {
                replacement += c;
            }
            else
            {
                replacement += '_';
            }
        }

        if (replacement.StartsWith('_'))
        {
            replacement = '@' + replacement;
        }

        return replacement;
    }
    private static bool KeepChar(this char c)
    {
        switch (char.GetUnicodeCategory(c))
        {
            case UnicodeCategory.UppercaseLetter:
            case UnicodeCategory.LowercaseLetter:
            case UnicodeCategory.TitlecaseLetter:
            case UnicodeCategory.ModifierLetter:
            case UnicodeCategory.OtherLetter:
            case UnicodeCategory.LetterNumber:
            case UnicodeCategory.NonSpacingMark:
            case UnicodeCategory.SpacingCombiningMark:
            case UnicodeCategory.DecimalDigitNumber:
            case UnicodeCategory.ConnectorPunctuation:
            case UnicodeCategory.Format:
                return true;
            default:
                return false;
        }
    }
}