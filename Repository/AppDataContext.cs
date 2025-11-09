using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Repository
{
    public class AppDataContext : DbContext
    {
        private string? _connectionString;
        private string? _migrationsAssemblyName;

        public AppDataContext(IConfiguration configuration, string migrationsAssemblyName = null)
        {
            _migrationsAssemblyName = migrationsAssemblyName;
             _connectionString = configuration.GetSection("ConnectionString").Value;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connectionString != null)
            {
                //optionsBuilder.UseSqlServer(_connectionString, b => b.MigrationsAssembly("WiredBrainCoffee.CustomersApp"));
                optionsBuilder.UseSqlServer(_connectionString, o =>
                {
                    if (!string.IsNullOrEmpty(_migrationsAssemblyName))
                    {
                        o.MigrationsAssembly(_migrationsAssemblyName);
                    }
                });
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 获取当前应用程序域中加载的所有程序集
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // 遍历所有程序集并获取所有类型
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes().Where(e => e.BaseType != null && e.BaseType == typeof(ModelBase)).ToArray();
                foreach (Type type in types)
                {
                    modelBuilder.Entity(type);
                }
            }
        }
        public override int SaveChanges()
        {
            var addList = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
            foreach (var item in addList) {
                var now =DateTime.Now;
                if (item.Properties.Any(e => e.Metadata.Name == "CreateTime"))
                {
                    item.Property("CreateTime").CurrentValue = now;
                }
                if (item.Properties.Any(e => e.Metadata.Name == "EditTime"))
                {
                    item.Property("EditTime").CurrentValue = now;
                }
                if (item.Properties.FirstOrDefault(p => p.Metadata.Name == "IsDeleted") != null)
                {
                    item.Property("IsDeleted").CurrentValue = false;
                }
            }
            var alterList = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);
            foreach (var item in alterList)
            {
                var now = DateTime.Now;
                if (item.Properties.Any(e => e.Metadata.Name == "EditTime"))
                {
                    item.Property("EditTime").CurrentValue = now;
                }
            }
            var deleteList = ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted);
            List<EntityEntry> deleteEntryList = new List<EntityEntry>();
            foreach (var item in deleteList)
            {
                var now = DateTime.Now;
                if (item.Properties.Any(e => e.Metadata.Name == "IsDeleted"))
                {
                    item.State = EntityState.Modified;
                    item.Property("IsDeleted").CurrentValue = true;
                    if (item.Properties.Any(e => e.Metadata.Name == "EditTime"))
                    {
                        item.Property("EditTime").CurrentValue = now;
                    }
                    deleteEntryList.Add(item);
                }
               
            }
            var back = base.SaveChanges();
            foreach (var item in deleteEntryList)
            {
                item.State = EntityState.Detached;
            }
            return back;
        }
    }
}
