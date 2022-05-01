using ax.Module.Data;
using ax.Module.Graphql.Types;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

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

        Field<ListGraphType<ProductType>>()
            .Name("products")
            .Resolve(_ => productRepository.GetAll());

        Field<ListGraphType<ProductType>>(
            "productsx",
            resolve: _ => productRepository.GetAll());
    }
}

