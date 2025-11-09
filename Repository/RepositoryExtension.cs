using Microsoft.Extensions.DependencyInjection;
using Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddDbContextFactory<AppDataContext>();
            //services.AddScoped<AppDataContext>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // 遍历所有程序集并获取所有类型
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes().Where(e => e.BaseType != null && e.BaseType == typeof(ModelBase)).ToArray();
                var aaaa = assembly.GetTypes();
                foreach (Type type in types)
                {
                    var interfaceType = typeof(IRepository<>);
                    interfaceType = interfaceType.MakeGenericType(type);
                    var repositoryType = typeof(RepositoryBase<>);
                    repositoryType = repositoryType.MakeGenericType(type);
                    services.AddScoped(interfaceType, repositoryType);
                }
            }
            return services;
        }
    }
}
