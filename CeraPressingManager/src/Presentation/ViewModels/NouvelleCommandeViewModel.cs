using CeraPressingManager.Core.Common;
using CeraPressingManager.Core.Entities;
using CeraPressingManager.Core.Interfaces;
using CeraPressingManager.Services;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QRCoder;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CeraPressingManager.ViewModels;

public partial class NouvelleCommandeViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<Client> clients = new();
    [ObservableProperty] private Client? selectedClient;
    [ObservableProperty] private ObservableCollection<ArticleDepose> articles = new();
    [ObservableProperty] private ObservableCollection<Prestation> prestations = new();
    [ObservableProperty] private BitmapImage? qRCodeImage;
    [ObservableProperty] private decimal montantTotal;
    [ObservableProperty] private string remiseText = "";
    [ObservableProperty] private decimal montantAPayer;

   

    private readonly IClientService _clientService;
    private readonly CommandeService _commandeService;
    private readonly IRepository<Prestation> _prestationRepo;
    private readonly LoggingService _loggingService;

    public NouvelleCommandeViewModel(IClientService clientService, CommandeService commandeService, IRepository<Prestation> prestationRepo, LoggingService loggingService)
    {
        _clientService = clientService;
        _commandeService = commandeService;
        _prestationRepo = prestationRepo;
        _loggingService = loggingService;

        LoadDataAsync();
        CalculerTotal();
    }

    private async void LoadDataAsync()
    {
        var clientsList = await _clientService.GetAllAsync();
        Clients = new ObservableCollection<Client>(clientsList);
        SelectedClient = Clients.FirstOrDefault();

        var prestationsList = (await _prestationRepo.GetAllAsync()).ToList();
        Prestations = new ObservableCollection<Prestation>(prestationsList);

        // Ajouter un article par défaut
        AddArticle();
    }

    [RelayCommand]
    private void AddArticle()
    {
        if (Prestations.Any())
        {
            Articles.Add(new ArticleDepose
            {
                Prestation = Prestations.First(),
                PrestationId = Prestations.First().Id,
                Quantite = 1,
                Couleur = "Blanc",
                EstExpress2h = false,
                EstExpress24h = false
            });
            CalculerTotal();
        }
    }

    [RelayCommand]
    private void RemoveArticle(ArticleDepose article)
    {
        Articles.Remove(article);
        CalculerTotal();
    }

    [RelayCommand]
    private void TakePhoto(object? parameter)
    {
        if (parameter is ArticleDepose article)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp"
            };
            if (dialog.ShowDialog() == true)
            {
                article.PhotoAvant = dialog.FileName;
                // On force la notification de changement pour que la vue se mette à jour
                OnPropertyChanged(nameof(Articles));
            }
        }
    }

    partial void OnSelectedClientChanged(Client? value) => CalculerTotal();
    partial void OnArticlesChanged(ObservableCollection<ArticleDepose> value) => CalculerTotal();

    private void CalculerTotal()
    {
        if (SelectedClient == null) return;

        MontantTotal = Articles.Sum(a => a.PrixTotal);

        decimal remise = SelectedClient.Categorie == CategorieClient.VIP ? MontantTotal * 0.20m :
                         SelectedClient.Categorie == CategorieClient.Regulier ? MontantTotal * 0.10m : 0m;

        RemiseText = remise > 0 ? $"Remise {SelectedClient.Categorie} : -{remise:N0} FCFA" : "";
        MontantAPayer = MontantTotal - remise;

        // La génération du QR Code est déplacée vers la couche UI ou un service dédié
        // QRCodeImage = GenerateQRCode($"CMD-{DateTime.Now:yyyyMMddHHmmss}");
    }

    // La génération du QR Code est déplacée vers la couche UI ou un service dédié
    // private BitmapImage GenerateQRCode(string text) { ... }

    [RelayCommand]
    private async Task SaveCommandExecute()
    {
        if (SelectedClient == null || !Articles.Any())
        {
            MessageBox.Show("Veuillez sélectionner un client et ajouter au moins un article.", "Erreur de Commande", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var estExpress = Articles.Any(a => a.EstExpress2h || a.EstExpress24h);

            var commande = await _commandeService.CreerCommandeAsync(
                SelectedClient.Id,
                Articles.ToList(),
                estExpress
            );

            await _loggingService.LogAsync("Nouvelle Commande", $"Commande N°{commande.Id} enregistrée pour le client {SelectedClient.NomComplet}. Total: {commande.MontantTotal:N0} FCFA.");

            // TODO: Implémenter NotificationService
            // NotificationService.EnvoyerWhatsApp(SelectedClient.Telephone, $"Bonjour {SelectedClient.NomComplet} ! Votre commande N°{commande.Id} a été enregistrée. Total: {commande.MontantTotal:N0} FCFA.");

            MessageBox.Show(
                $"COMMANDE N°{commande.Id} ENREGISTRÉE avec succès !\n" +
                $"Client : {SelectedClient.NomComplet}\n" +
                $"\nTotal : {commande.MontantTotal:N0} FCFA",
                "SUCCÈS TOTAL", MessageBoxButton.OK, MessageBoxImage.Information);

            // Réinitialiser la vue pour une nouvelle commande
            Articles.Clear();
            CalculerTotal();
            LoadDataAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors de la sauvegarde de la commande : " + ex.Message, "Erreur Système", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void PrintTicket()
    {
        MessageBox.Show("Fonctionnalité d'impression de ticket non implémentée (dépend de System.Drawing.Printing non supporté sur Linux).", "Impression", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}