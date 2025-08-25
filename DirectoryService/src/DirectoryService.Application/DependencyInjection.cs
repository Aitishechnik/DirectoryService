using DirectoryService.Application.Departments.Commands.Add;
using DirectoryService.Application.Departments.Commands.UpdateLocations;
using DirectoryService.Application.Locations.Commands.Add;
using DirectoryService.Application.Positions.Commands.Add;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            services.AddScoped<IAddLocationHandler, AddLocationHandler>();
            services.AddScoped<IAddDepartmentHandler, AddDepartmentHandler>();
            services.AddScoped<IAddPositionHandler, AddPositionHandler>();
            services.AddScoped<IUpdateLocationsHandler, UpdateLocationsHandler>();

            return services;
        }
    }
}