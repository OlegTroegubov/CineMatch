using System.Reflection;
using CineMatch.Application.Common.Interfaces;
using CineMatch.Application.Features.Token;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CineMatch.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ITokenService, TokenService>();
        services.AddMediatR(Assembly.GetExecutingAssembly());
        return services;
    }
}