using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiredBrainCoffee.CustomersApp
{
    public class DataContextFactory : IDesignTimeDbContextFactory<AppDataContext>
    {
        //public DataContext CreateDbContext(string[] args)
        //{
        //    var configuration = new ConfigurationBuilder()
        //        .SetBasePath(AppContext.BaseDirectory)
        //        .AddJsonFile("appsettings.json")
        //        .Build();
        //    var migrationAssemlyName = AppDomain.CurrentDomain.FriendlyName;
        //    return new DataContext(configuration, migrationAssemlyName);
        //}
        public AppDataContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            var migrationAssemblyName = typeof(DataContextFactory).Assembly.GetName().Name;
            return new AppDataContext(configuration, migrationAssemblyName);
        }
    }
}
