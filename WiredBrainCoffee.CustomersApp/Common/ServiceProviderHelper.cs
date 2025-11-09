using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiredBrainCoffee.CustomersApp.Common
{
    public static class ServiceProviderHelper
    {
        public static IServiceProvider ServiceProvider;
        public static void Init(IHost host)
        {
            if (ServiceProvider == null)
            {
                ServiceProvider = host.Services;
            }
        }
        /// <summary>
        /// 获取新服务范围
        /// </summary>
        /// <returns>服务范围</returns>
        public static IServiceScope GetNewScope()
        {
            return ServiceProvider.CreateScope();
        }
        public static T GetScopeService<T>(IServiceScope scope) where T : class
        {
            return scope.ServiceProvider.GetRequiredService<T>();
        }
        public static T GetService<T>() where T : class
        {
            return ServiceProvider.GetRequiredService<T>();
        }
    }
}
