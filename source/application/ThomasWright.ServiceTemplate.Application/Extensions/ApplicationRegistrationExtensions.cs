using Microsoft.Extensions.Hosting;

namespace ThomasWright.ServiceTemplate.Extensions;

public static class ApplicationRegistrationExtensions
{
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    {
        return builder;
    }
}