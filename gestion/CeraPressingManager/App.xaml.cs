using Microsoft.Extensions.Hosting;
using System.Windows;
using CeraPressingManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CeraPressingManager;

public partial class App : Application
{
    public static IHost? Host { get; private set; }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        Host = Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddDbContext<PressingDbContext>(options =>
                    options.UseSqlite("Data Source=cerapressing.db"));

                services.AddSingleton<MainWindow>();
            })
            .Build();

        await Host.StartAsync();

        // Création de la base + seed
        using var scope = Host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PressingDbContext>();
        db.Database.EnsureCreated();

        // LOGIN OBLIGATOIRE
        var loginWindow = new Views.LoginWindow();
        if (loginWindow.ShowDialog() != true)
        {
            Shutdown();
            return;
        }

        new Views.MainWindow().Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        Host?.StopAsync().Wait();
        Host?.Dispose();
        base.OnExit(e);
    }
}