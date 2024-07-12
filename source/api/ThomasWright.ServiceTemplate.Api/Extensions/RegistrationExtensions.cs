using MassTransit;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ThomasWright.ServiceTemplate.Extensions;

public static class RegistrationExtensions
{
    public static IHostApplicationBuilder AddSerilog(this IHostApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Services.AddSerilog();
        
        return builder;
    }
    
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        var assemblies = new[]
        {
            typeof(ApplicationDbContext).Assembly,
            typeof(RegistrationExtensions).Assembly,
            typeof(ApplicationRegistrationExtensions).Assembly
        };
        
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSnakeCaseNamingConvention();
            
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), cfg =>
            {
                cfg.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            });
        });
        
        builder.Services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();
            
            cfg.AddSagas(assemblies);
            
            cfg.AddSagaStateMachines(assemblies);
            
            cfg.AddConsumers(assemblies);
            
            cfg.SetInMemorySagaRepositoryProvider();
            
            cfg.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));
                
                cfg.ConfigureEndpoints(context);
            });
        });
        
        return builder;
    }
}