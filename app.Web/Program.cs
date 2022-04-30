

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

var list = builder.Services.Select(x => x.ServiceType.ToString()).OrderBy(x=>x).ToList();

var app = builder.Build();

ConfigureApp();

app.Run();


void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    ax.Module.Data.StartupServices.ConfigureServices(services);
    ax.Module.Graphql.StartupServices.ConfigureServices(services);
}

void ConfigureApp()
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.MapControllers();

    ax.Module.Graphql.StartupServices.ConfigureApp(app);
}


