using CME.Business.Implementations;
using CME.Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SERP.Filenet.DB;
using System;
using System.Collections.Generic;
using System.Text;
using Tsoft.Framework.Common.Configs;

namespace CME.Business
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServiceComponents(this IServiceCollection services, IConfiguration configuration)
        { 
            var dbSetings = configuration.GetSection(nameof(DBSettings)).Get<DBSettings>();
            services.AddDbContext<DataContext>(x => x.UseSqlServer(dbSetings.ConnectionString), ServiceLifetime.Transient);

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IOrganizationService, OrganizationService>();
            services.AddTransient<ITitleService, TitleService>();
            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<ITrainingFormService, TrainingFormService>();

            return services;
        }
    }
}
