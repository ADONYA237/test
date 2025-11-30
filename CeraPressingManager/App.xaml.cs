using Microsoft.Extensions.Hosting;
using System.Windows;
using CeraPressingManager.Core.Interfaces;
using CeraPressingManager.Data.Repositories;
using CeraPressingManager.Core.Entities;
using CeraPressingManager.Services;
using CeraPressingManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace CeraPressingManager;

public partial class App : Application
{
    public static IHost? Host { get; private set; }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // 1. Écran de chargement (simulé)
        // Dans une application réelle, on utiliserait un SplashScreen ou une fenêtre dédiée.
        // Ici, on simule le temps de chargement de l'hôte.
        await Task.Delay(1000); // Simuler le chargement de l'application

        Host = Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddDbContext<PressingDbContext>(options =>
                    options.UseSqlite("Data Source=cerapressing.db"));

                services.AddSingleton<Views.MainWindow>();
                services.AddTransient<Views.LoginWindow>();
                services.AddTransient<Views.DashboardWindow>();
                services.AddTransient<Views.NouvelleCommandeWindow>();

                // ViewModels
                services.AddSingleton<ViewModels.MainWindowViewModel>();
                services.AddSingleton<IServiceProvider>(sp => sp);
                services.AddTransient<ViewModels.LoginViewModel>();
                services.AddTransient<ViewModels.DashboardViewModel>();
                services.AddTransient<ViewModels.NouvelleCommandeViewModel>();

                // Enregistrement des Repositories et Services
                services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                services.AddSingleton<CurrentUser>(); // État de l'utilisateur connecté
                services.AddScoped<LoggingService>();
                services.AddScoped<PaiementService>();
                services.AddScoped<AuthService>();
                services.AddScoped<IClientService, ClientService>();
                services.AddScoped<CommandeService>(); // Pas d'interface ICommandeService trouvée, on l'enregistre directement
            })
            .Build();

        await Host.StartAsync();

        // Création de la base + seed
        using var scope = Host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PressingDbContext>();
        db.Database.EnsureCreated();

        // LOGIN OBLIGATOIRE
        var loginWindow = Host.Services.GetRequiredService<Views.LoginWindow>();
        if (loginWindow.ShowDialog() != true)
        {
            Shutdown();
            return;
        }

        // Affichage du Dashboard principal
        Host.Services.GetRequiredService<Views.MainWindow>().Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        Host?.StopAsync().Wait();
        Host?.Dispose();
        base.OnExit(e);
    }
}