using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WiredBrainCoffee.CustomersApp.Common;
using WiredBrainCoffee.CustomersApp.ViewModel;

namespace WiredBrainCoffee.CustomersApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;
        protected override async void OnStartup(StartupEventArgs e)
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(builder => { })
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddRepository();
                    RegisteViewModel(services);
                    services.AddTransient<MainWindow>();
                }).Build();
            ServiceProviderHelper.Init(_host);
            await _host.StartAsync();    
            var mainWindow= _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
        protected IServiceCollection RegisteViewModel(IServiceCollection services)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // 遍历所有程序集并获取所有类型
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes()
                .Where(e => e.IsClass && !e.IsAbstract && typeof(ViewModelBase).IsAssignableFrom(e))
                .ToArray();
                foreach (Type type in types)
                {
                    services.AddTransient(type);
                }
            }
            return services;
        }
       
    }

}
