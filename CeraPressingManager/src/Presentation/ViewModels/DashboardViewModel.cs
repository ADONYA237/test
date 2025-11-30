using CeraPressingManager.Core.Common;
using CeraPressingManager.Core.Interfaces;
using CeraPressingManager.Core.Entities;
using System.Threading.Tasks;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace CeraPressingManager.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty] private int nbCommandesAujourdhui;
    [ObservableProperty] private decimal chiffreAffairesAujourdhui;
    [ObservableProperty] private int nbClientsVIP;

    private readonly IRepository<Commande> _commandeRepo;
    private readonly IRepository<Client> _clientRepo;

    public DashboardViewModel(IRepository<Commande> commandeRepo, IRepository<Client> clientRepo)
    {
        _commandeRepo = commandeRepo;
        _clientRepo = clientRepo;
        LoadDataAsync();
    }

    private async void LoadDataAsync()
    {
        var aujourdHui = DateTime.Today;

        // Commandes du jour
        var commandes = (await _commandeRepo.GetWhereAsync(c => c.DateDepot.Date == aujourdHui)).ToList();
        NbCommandesAujourdhui = commandes.Count;
        ChiffreAffairesAujourdhui = commandes.Sum(c => c.MontantTotal);

        // Clients VIP
        var clientsVIP = (await _clientRepo.GetWhereAsync(c => c.Categorie == CategorieClient.VIP)).ToList();
        NbClientsVIP = clientsVIP.Count;
    }
}