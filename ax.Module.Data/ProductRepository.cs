using ax.Module.Common;

namespace ax.Module.Data;

public class ProductRepository
{
    public Task<List<Product>> GetAllAsync()
    {
        var ret = GetAll();
        return Task.FromResult(ret);
    }

    public List<Product> GetAll()
    {
        var ret = new List<Product>
        {
            new Product {Id = 1, Name = "A", Description = ""},
            new Product {Id = 2, Name = "B", Description = ""},
            new Product {Id = 3, Name = "C", Description = ""},
        };
        return ret;
    }

}

