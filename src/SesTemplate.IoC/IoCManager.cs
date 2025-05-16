﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RhbkSdk.Configurations;
using RhbkSdk.Extensions;
using SesTemplate.Aplication.Services;
using SesTemplate.Application.Contracts;
using SesTemplate.Domain;
using SesTemplate.Infra.CrossCutting.ConfigurationModels;
using SesTemplate.Infra.Data;
using SesTemplate.Infra.Data.Contexts;

namespace SesTemplate.IoC;

public static class IoCManager
{
    public static IServiceCollection ConfigureByIoC(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment hostingEnvironment)
    {
        return services
                .AddApplicationDbContext(configuration)
                .AddDomainRepositories()
                .AddAutoMapper()
                .AddApplicationServices()
                .AddRhbkSdk(configuration)
            ;
    }
    
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        DatabaseConfigure dbconfig = new DatabaseConfigure();
        configuration.GetSection(DatabaseConfigure.ConnectionStringsSection).Bind(dbconfig);

        services.AddDbContext<AppDbContext>(optionsAction =>
        {
            optionsAction.UseSqlServer(dbconfig.ConnectionStrings);
            //optionsAction.UseSqlite("Data Source=db.db");
        });
        return services;
    }
    
    public static IServiceCollection AddDomainRepositories(this IServiceCollection services)
    {
        services.AddAllServicesByTypes(typeof(IDomain), typeof(IInfraData));
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAllServicesByTypes(typeof(IApplicationContracts), typeof(IApplicationServices));
        return services;
    }

    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(IApplicationServices));
        return services;
    }

    public static IServiceCollection AddRhbkSdk(this IServiceCollection services, IConfiguration configuration)
    {
        RhbkConfiguration config = configuration.GetSection(RhbkConfiguration.ConfigurationSection).Get<RhbkConfiguration>()!;
        return services.AddRhbkClient(config.KeycloakBaseUrl, ServiceLifetime.Scoped);
    }
    
    #region "Private Methods"

    private static IServiceCollection AddAllServicesByTypes(this IServiceCollection services, Type typeInterface,
        Type implementationType)
    {
        IEnumerable<Type> projectInterfaces = GetDescendentInterfaces(typeInterface);
        return AddServiceOfAllTypesAndDescendentTypes
            (services, projectInterfaces, implementationType);
    }

    private static IServiceCollection AddServiceOfAllTypesAndDescendentTypes(IServiceCollection services,
        IEnumerable<Type> interfaceTypes, Type implementationType)
    {
        foreach (var implementedTye in interfaceTypes)
        {
            var implementedTypes = GetImplementedTypesFromInterface(implementedTye, implementationType);
            AddServiceScoped(services, implementedTye, implementedTypes);
        }

        return services;
    }

    private static IEnumerable<Type> GetDescendentInterfaces(Type typeInterface)
    {
        return typeInterface.Assembly
            .GetTypes()
            .Where(i => i.IsInterface && i != typeInterface);
    }

    private static IEnumerable<Type> GetImplementedTypesFromInterface
        (Type projectInterface, Type implementationType)
    {
        return implementationType
            .Assembly
            .GetTypes()
            .Where(t => !t.IsInterface
                        && !t.IsAbstract
                        && t.IsAssignableTo(projectInterface));
    }

    private static IServiceCollection AddServiceScoped(
        IServiceCollection services,
        Type interfaceType,
        IEnumerable<Type> implementedTypes)
    {
        // Console.ForegroundColor = ConsoleColor.White;
        foreach (Type implementedType in implementedTypes)
        {
            // Console.WriteLine($"Serviço adicionado: <{interfaceType.Name}, {implementedType.Name}>");
            services.AddScoped(interfaceType, implementedType);
        }

        return services;
    }

    #endregion
    
}