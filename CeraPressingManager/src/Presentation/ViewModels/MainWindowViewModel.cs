using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CeraPressingManager.Core.Entities;
using CeraPressingManager.Services;
using CeraPressingManager.Core.Interfaces;
using System.Threading.Tasks;
using CeraPressingManager.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Windows;

namespace CeraPressingManager.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IClientService _clientService;
    private readonly CurrentUser _currentUser;

    [ObservableProperty]
    private object? currentView;

    [ObservableProperty]
    private string titreFenetre = "CERA PRESSING MANAGER";

    public CurrentUser CurrentUser => _currentUser;

    public MainWindowViewModel(IServiceProvider serviceProvider, IClientService clientService, CurrentUser currentUser)
    {
        _serviceProvider = serviceProvider;
        _clientService = clientService;
        _currentUser = currentUser;

        // Démarrer sur le Dashboard
        NavigateToDashboard();
    }

    [RelayCommand]
    public void NavigateToDashboard()
    {
        CurrentView = _serviceProvider.GetRequiredService<DashboardViewModel>();
        TitreFenetre = "CERA PRESSING MANAGER - Tableau de Bord";
    }

    [RelayCommand]
    public void NavigateToNouvelleCommande()
    {
        CurrentView = _serviceProvider.GetRequiredService<NouvelleCommandeViewModel>();
        TitreFenetre = "CERA PRESSING MANAGER - Nouvelle Commande";
    }

    [RelayCommand]
    public void NavigateToGestionClients()
    {
        // Créer un ViewModel pour la gestion des clients
        // CurrentView = _serviceProvider.GetRequiredService<ClientViewModel>();
        TitreFenetre = "CERA PRESSING MANAGER - Gestion des Clients";
    }

    [RelayCommand]
    public void NavigateToGestionEmployes()
    {
        // Créer un ViewModel pour la gestion des employés
        TitreFenetre = "CERA PRESSING MANAGER - Gestion des Employés";
    }

    [RelayCommand]
    public void NavigateToRapports()
    {
        // Créer un ViewModel pour les rapports
        TitreFenetre = "CERA PRESSING MANAGER - Rapports et Statistiques";
    }

    [RelayCommand]
    public void Logout()
    {
        // Logique de déconnexion (à implémenter)
        // Rediriger vers la fenêtre de connexion
        Application.Current.Shutdown();
    }

    [RelayCommand]
    public void Quitter() => Application.Current.Shutdown();
}