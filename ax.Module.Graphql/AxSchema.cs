using ax.Module.Data;
using ax.Module.Graphql.Types;
using GraphQL;
using GraphQL.Instrumentation;
using GraphQL.Transport;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ax.Module.Graphql;

public class AxSchema : Schema
{
    public AxSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Query = new ProductQuery(serviceProvider.GetService<ProductRepository>());
    }
}

public class ProductQuery : ObjectGraphType
{
    public ProductQuery(ProductRepository? productRepository)
    {
        if (productRepository == null)
        {
            throw new ArgumentNullException($"{nameof(productRepository)} is null");
        }

        Field<ListGraphType<ProductType>>(
            "products",
            resolve: _ => productRepository.GetAll());
    }
}

public class GraphQLSettings
{
    public PathString GraphQLPath { get; set; }
    public Func<HttpContext, IDictionary<string, object>> BuildUserContext { get; set; }
    public bool EnableMetrics { get; set; }
    public bool ExposeExceptions { get; set; }
}


public class GraphQLMiddleware : IMiddleware
{
    private readonly GraphQLSettings _settings;
    private readonly IDocumentExecuter _executer;
    private readonly IGraphQLSerializer _serializer;
    private readonly ISchema _schema;

    public GraphQLMiddleware(
        IOptions<GraphQLSettings> options,
        IDocumentExecuter executer,
        IGraphQLSerializer serializer,
        ISchema schema)
    {
        _settings = options.Value;
        _executer = executer;
        _serializer = serializer;
        _schema = schema;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!IsGraphQLRequest(context))
        {
            await next(context).ConfigureAwait(false);
            return;
        }

        await ExecuteAsync(context).ConfigureAwait(false);
    }

    private bool IsGraphQLRequest(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments(_settings.GraphQLPath)
            && string.Equals(context.Request.Method, "POST", StringComparison.OrdinalIgnoreCase);
    }

    private async Task ExecuteAsync(HttpContext context)
    {
        var start = DateTime.UtcNow;

        var request = await _serializer.ReadAsync<GraphQLRequest>(context.Request.Body, context.RequestAborted).ConfigureAwait(false);

        var result = await _executer.ExecuteAsync(options =>
        {
            options.Schema = _schema;
            options.Query = request.Query;
            options.OperationName = request.OperationName;
            options.Variables = request.Variables;
            options.UserContext = _settings.BuildUserContext?.Invoke(context);
            options.EnableMetrics = _settings.EnableMetrics;
            options.RequestServices = context.RequestServices;
            options.CancellationToken = context.RequestAborted;
        }).ConfigureAwait(false);

        if (_settings.EnableMetrics)
        {
            result.EnrichWithApolloTracing(start);
        }

        await WriteResponseAsync(context, result, context.RequestAborted).ConfigureAwait(false);
    }

    private async Task WriteResponseAsync(HttpContext context, ExecutionResult result, CancellationToken cancellationToken)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 200; // OK

        await _serializer.WriteAsync(context.Response.Body, result, cancellationToken).ConfigureAwait(false);
    }
}
