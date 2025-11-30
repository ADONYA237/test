using CeraPressingManager.Core.Common;
using CeraPressingManager.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace CeraPressingManager.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty] private int nbCommandesAujourdhui;
    [ObservableProperty] private decimal chiffreAffairesAujourdhui;
    [ObservableProperty] private int nbClientsVIP;

    public DashboardViewModel()
    {
        using var scope = App.Host!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PressingDbContext>();

        var aujourdHui = DateTime.Today;
        var commandes = db.Commandes.Where(c => c.DateDepot.Date == aujourdHui).ToList();

        NbCommandesAujourdhui = commandes.Count;
        ChiffreAffairesAujourdhui = commandes.Sum(c => c.MontantTotal);
        NbClientsVIP = db.Clients.Count(c => c.Categorie == CategorieClient.VIP);
    }
}