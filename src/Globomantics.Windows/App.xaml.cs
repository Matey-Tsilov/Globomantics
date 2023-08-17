using Globomantics.Domain;
using Globomantics.Infrastructure.Data.Repositories;
using Globomantics.Windows.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace Globomantics.Windows;

public partial class App : Application
{
    public static User CurrentUser { get; set; } = default!;
    public IServiceProvider ServiceProvider { get; init; } 
    public IConfiguration Configuration { get; init; }

    public App()
    {
        var builder = new ConfigurationBuilder();

        Configuration = builder.Build();

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<IRepository<Bug>, ToDoInMemoryRepository<Bug>>();
        serviceCollection.AddSingleton<IRepository<Feature>, ToDoInMemoryRepository<Feature>>();
        serviceCollection.AddSingleton<IRepository<TodoTask>, ToDoInMemoryRepository<TodoTask>>();

        serviceCollection.AddTransient<BugViewModel>();
        serviceCollection.AddTransient<FeatureViewModel>();
        serviceCollection.AddTransient<MainViewModel>();
        serviceCollection.AddTransient<MainWindow>();
        serviceCollection.AddTransient(_ => ServiceProvider!);
        
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    private void OnStartup(object sender, StartupEventArgs e)
    {
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();

        mainWindow?.Show();
    }
}
