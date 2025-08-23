﻿using DirectoryService.Application.Departments;
using DirectoryService.Application.Locations;
using DirectoryService.Application.Positions;
using DirectoryService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            services.AddScoped<ILocationsRepository, LocationsRepository>();
            services.AddScoped<IDepartmentsRepository, DepartmentsRepositoriy>();
            services.AddScoped<IPositionsRepository, PositionsRepository>();

            return services;
        }
    }
}