using System.IO;
using System;
using UserManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddDataAccess(this IServiceCollection services, bool inMemory = true)
    {
        if (inMemory)
        {
            services.AddScoped<IDataContext, DataContext>();
        }
        else
        {
            services.AddDbContext<DataContext>(options =>
            {
                var folder = Environment.SpecialFolder.LocalApplicationData;
                var path = Environment.GetFolderPath(folder);
                var dbPath = Path.Join(path, "UserManagement.Data.DataContext.db");
                options.UseSqlite($"Data Source={dbPath}");
            });
            services.AddScoped<IDataContext, DataContext>();
        }

        return services;
    }
     
}
