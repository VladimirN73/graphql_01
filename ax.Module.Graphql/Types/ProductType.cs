using ax.Module.Common;
using GraphQL.Types;

namespace ax.Module.Graphql.Types;

public sealed class ProductType: ObjectGraphType<Product>
{
    public ProductType()
    {
        Field(t => t.Id);
        Field(t => t.Name);
        Field(t => t.Description);
    }
}

