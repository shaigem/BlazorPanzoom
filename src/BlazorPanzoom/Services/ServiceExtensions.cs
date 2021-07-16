using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BlazorPanzoom
{
    [ExcludeFromCodeCoverage]
    public static class ServiceExtensions
    {
        public static IServiceCollection AddJSBlazorPanzoomInterop(this IServiceCollection services)
        {
            services.TryAddScoped<IJSBlazorPanzoomInterop, JSBlazorPanzoomInterop>();
            return services;
        }

        public static IServiceCollection AddPanzoomHelper(this IServiceCollection services)
        {
            services.TryAddScoped<IPanzoomHelper, PanzoomHelper>();
            return services;
        }

        public static IServiceCollection AddBlazorPanzoomServices(this IServiceCollection services)
        {
            return services
                .AddJSBlazorPanzoomInterop()
                .AddPanzoomHelper();
        }
    }
}