using Microsoft.Extensions.DependencyInjection;

namespace ax.Module.Data;

public static class StartupServices
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<ProductRepository>();
    }
}
