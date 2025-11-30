using CeraPressingManager.Core.Common;
using CeraPressingManager.Core.Entities;
using CeraPressingManager.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QRCoder;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Printing;
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

   
    private readonly Window _owner;

    public NouvelleCommandeViewModel(Window owner)
    {
        _owner = owner;


        LoadTestData();
        CalculerTotal();
    }

    private void LoadTestData()
    {
        Clients.Add(new Client { Id = 1, NomComplet = "Mamadou Diallo", Categorie = CategorieClient.VIP });
        Clients.Add(new Client { Id = 2, NomComplet = "Aïcha Camara", Categorie = CategorieClient.Regulier });
        SelectedClient = Clients[0];

        Prestations = new ObservableCollection<Prestation>(new[]
{
    new Prestation { Id = 1, TypeVetement = "Chemise", PrixUnitaire = 500, SupplementExpress2h = 3000, SupplementExpress24h = 1000 },
    new Prestation { Id = 2, TypeVetement = "Pantalon", PrixUnitaire = 1000, SupplementExpress2h = 4000, SupplementExpress24h = 1500 },
    new Prestation { Id = 3, TypeVetement = "Costume 3 pièces", PrixUnitaire = 5000, SupplementExpress2h = 15000, SupplementExpress24h = 5000 },
    new Prestation { Id = 4, TypeVetement = "Robe de soirée", PrixUnitaire = 8000, SupplementExpress2h = 20000, SupplementExpress24h = 8000 }
});

        [RelayCommand]
    private void AddArticle()
    {
        Articles.Add(new ArticleDepose
        {
            Prestation = Prestations[0],
            Quantite = 1,
            Couleur = "Blanc",
            EstExpress = false
        });
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
                OnPropertyChanged(nameof(Articles));
            }
        }
    }

    partial void OnSelectedClientChanged(Client? value) => CalculerTotal();
    partial void OnArticlesChanged(ObservableCollection<ArticleDepose> value) => CalculerTotal();

    private void CalculerTotal()
    {
        if (SelectedClient == null) return;

        decimal total = 0;
        foreach (var article in Articles)
        {
            decimal prix = article.Prestation.PrixUnitaire * article.Quantite;

            if (article.EstExpress)
            {
                decimal supplement = DateTime.Now.Hour < 14 // avant 14h → Express 2h possible
                    ? article.Prestation.SupplementExpress2h
                    : article.Prestation.SupplementExpress24h;
                prix += supplement;
            }

            total += prix;
        }

        MontantTotal = total;

        decimal remise = SelectedClient.Categorie == CategorieClient.VIP ? total * 0.20m :
                         SelectedClient.Categorie == CategorieClient.Regulier ? total * 0.10m : 0m;

        RemiseText = remise > 0 ? $"Remise {SelectedClient.Categorie} : -{remise} FCFA" : "";
        MontantAPayer = total - remise;

        QRCodeImage = GenerateQRCode($"CMD-{DateTime.Now:yyyyMMddHHmmss}");
    }

    private BitmapImage GenerateQRCode(string text)
    {
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new QRCode(qrCodeData);
        var bmp = qrCode.GetGraphic(20);

        using var ms = new MemoryStream();
        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        ms.Position = 0;

        var image = new BitmapImage();
        image.BeginInit();
        image.StreamSource = ms;
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.EndInit();
        return image;
    }

    private async void SaveCommandExecute()
    {
        try
        {
            using var scope = App.Current.Host.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PressingDbContext>();

            var commande = new Commande
            {
                ClientId = SelectedClient!.Id,
                DateDepot = DateTime.Now,
                DatePrevue = DateTime.Now.AddHours(EstExpress() ? 2 : 24),
                MontantTotal = MontantAPayer,
                EstExpress = Articles.Any(a => a.EstExpress),
                CodeQR = $"CMD-{DateTime.Now:yyyyMMddHHmmss}",
                Articles = Articles.Select(a => new ArticleDepose
                {
                    PrestationId = a.Prestation.Id,
                    Quantite = a.Quantite,
                    Couleur = a.Couleur,
                    DescriptionDommages = a.DescriptionDommages,
                    PhotoAvant = a.PhotoAvant,
                    EstExpress = a.EstExpress
                }).ToList()
            };

            db.Commandes.Add(commande);
            await db.SaveChangesAsync();
            // Après db.SaveChangesAsync();
            NotificationService.EnvoyerWhatsApp(client.Telephone,
                $"Bonjour {client.NomComplet} ! Votre commande N°{commande.Id} est prête ! Merci pour votre confiance.");

            NotificationService.EnvoyerEmail(client.Email,
                "Commande prête", $"Votre pressing est prêt ! Passez récupérer vos vêtements.");

            MessageBox.Show(
                $"COMMANDE N°{commande.Id} ENREGISTRÉE avec succès !\n" +
                $"Client : {SelectedClient.NomComplet}\n" +
                $"\nTotal : {MontantAPayer} FCFA",
                "SUCCÈS TOTAL", MessageBoxButton.OK, MessageBoxImage.Information);

            _owner.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur sauvegarde : " + ex.Message);
        }
    }

    private bool EstExpress() => Articles.Any(a => a.EstExpress);

    private void PrintTicket()
    {
        var pd = new PrintDialog();
        if (pd.ShowDialog() == true)
        {
            var doc = new PrintDocument();
            doc.PrintPage += (s, e) =>
            {
                float y = 20;
                e.Graphics?.DrawString("CERA PRESSING", new Font("Arial", 18, FontStyle.Bold), Brushes.Black, 80, y);
                y += 40;
                e.Graphics?.DrawString($"Client : {SelectedClient?.NomComplet}", new Font("Arial", 12), Brushes.Black, 50, y);
                y += 25;
                e.Graphics?.DrawString($"Date   : {DateTime.Now:dd/MM/yyyy HH:mm}", new Font("Arial", 12), Brushes.Black, 50, y);
                y += 30;
                foreach (var a in Articles)
                {
                    string express = a.EstExpress ? " (EXPRESS)" : "";
                    e.Graphics?.DrawString($"- {a.Prestation.TypeVetement} x{a.Quantite}{express}", new Font("Arial", 10), Brushes.Black, 60, y);
                    y += 20;
                }
                y += 20;
                e.Graphics?.DrawString($"TOTAL : {MontantAPayer} FCFA", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, 50, y);
                y += 40;
                e.Graphics?.DrawString("Merci et à bientôt !", new Font("Arial", 12), Brushes.Black, 80, y);
            };
            doc.Print();
        }
    }
}