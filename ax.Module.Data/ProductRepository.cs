using ax.Module.Common;

namespace ax.Module.Data;

public class ProductRepository
{
    public Task<List<Product>> GetAll()
    {
        var ret = new List<Product>
        {
            new Product{Name = "A"},
            new Product{Name = "B"},
            new Product{Name = "C"},
        };
        return Task.FromResult(ret);
    }
}

