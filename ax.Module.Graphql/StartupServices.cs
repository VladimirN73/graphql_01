using GraphQL;
using GraphQL.Instrumentation;
using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Server.Ui.Playground;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ax.Module.Graphql;

public static class StartupServices
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<AxSchema>();

        services.AddSingleton<ISchema, AxSchema>();
        
        services.AddGraphQL(builder => builder
            .AddSchema<AxSchema>()
            .AddGraphTypes(typeof(AxSchema).Assembly)
            .AddHttpMiddleware<AxSchema,  GraphQLHttpMiddleware<AxSchema>>()
            .AddSystemTextJson()
            .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
        );
    }

    public static void ConfigureApp(WebApplication appx)
    {
        appx.UseGraphQL<AxSchema>(); // default url: /graphql
        appx.UseGraphQLPlayground(new PlaygroundOptions()); //default url: /ui/playground
    }
}
