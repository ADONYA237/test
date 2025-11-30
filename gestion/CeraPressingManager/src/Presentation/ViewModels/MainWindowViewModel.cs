using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CeraPressingManager.Core.Entities;
using CeraPressingManager.Data;
using CeraPressingManager.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace CeraPressingManager.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly ObservableCollection<Client> _allClients = new();

    [ObservableProperty]
    private ObservableCollection<Client> clients = new();

    [ObservableProperty]
    private string rechercheTelephone = string.Empty;

    public MainWindowViewModel()
    {
        LoadClients();
    }

    private void LoadClients()
    {
        using var scope = App.Host!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PressingDbContext>();

        var list = db.Clients.OrderBy(c => c.NomComplet).ToList();
        _allClients.Clear();
        foreach (var client in list)
            _allClients.Add(client);

        Clients = new ObservableCollection<Client>(_allClients);
    }

    // Recherche instantanée par téléphone
    partial void OnRechercheTelephoneChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            Clients = new ObservableCollection<Client>(_allClients);
        }
        else
        {
            var filtered = _allClients
                .Where(c => c.Telephone != null && c.Telephone.Contains(value.Trim()))
                .ToList();

            Clients = new ObservableCollection<Client>(filtered);
        }
    }

    // Nouvelle commande
    [RelayCommand]
    private void OpenNouvelleCommande()
    {
        var window = new NouvelleCommandeWindow();
        if (window.ShowDialog() == true)
        {
            LoadClients(); // rafraîchit si nouveau client ajouté
        }
    }

    // Tableau de bord
    [RelayCommand]
    private void OpenDashboard()
    {
        new DashboardWindow().ShowDialog();
    }

    // Ajouter un client (optionnel – tu peux créer la fenêtre plus tard)
    [RelayCommand]
    private void OpenAjoutClient()
    {
        var window = new AjoutClientWindow(); // crée cette fenêtre quand tu veux
        if (window.ShowDialog() == true)
        {
            LoadClients();
        }
    }

    // Rafraîchir manuellement la liste
    [RelayCommand]
    private void Refresh()
    {
        LoadClients();
        OnPropertyChanged(nameof(Clients));
    }

    // Quitter l'application
    [RelayCommand]
    private void Quitter() => Application.Current.Shutdown();
}