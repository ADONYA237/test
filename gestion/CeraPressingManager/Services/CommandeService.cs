using CeraPressingManager.Core.Common;
using CeraPressingManager.Core.Entities;
using CeraPressingManager.Core.Interfaces;
using QRCoder;
using System.Drawing;

namespace CeraPressingManager.Services;

public class CommandeService
{
    private readonly IRepository<Commande> _cmdRepo;
    private readonly IRepository<ArticleDepose> _artRepo;
    private readonly ClientService _clientService;

    public CommandeService(IRepository<Commande> cmdRepo, IRepository<ArticleDepose> artRepo, ClientService clientService)
    {
        _cmdRepo = cmdRepo;
        _artRepo = artRepo;
        _clientService = clientService;
    }

    public async Task<Commande> CreerCommandeAsync(int clientId, List<ArticleDepose> articles, bool estExpress = false)
    {
        var client = await _clientService.GetByIdAsync(clientId) ?? throw new InvalidOperationException("Client introuvable");

        var commande = new Commande
        {
            ClientId = clientId,
            DateDepot = DateTime.Now,
            DatePrevue = estExpress ? DateTime.Now.AddHours(2) : DateTime.Now.AddDays(1),
            EstExpress = estExpress,
            Articles = articles,
            QrCodeTicket = $"CMD-{DateTime.Now:yyyyMMddHHmmss}-{clientId}"
        };

        // Calcul du montant
        decimal total = 0;
        foreach (var art in articles)
        {
            decimal prix = art.Prestation.PrixUnitaire * art.Quantite;

            if (estExpress)
            {
                if (commande.DatePrevue <= DateTime.Now.AddHours(2))
                    prix += art.Prestation.SupplementExpress2h;
                else
                    prix += art.Prestation.SupplementExpress24h;
            }
            total += prix;
        }
        commande.MontantTotal = total;

        // Remise fidélité
        if (client.Categorie == CategorieClient.VIP)
            commande.RemiseAppliquee = total * 0.20m;
        else if (client.Categorie == CategorieClient.Regulier)
            commande.RemiseAppliquee = total * 0.10m;

        commande.MontantTotal -= commande.RemiseAppliquee;

        var saved = await _cmdRepo.AddAsync(commande);

        // Points fidélité
        await _clientService.IncrementerPointsEtCommandes(clientId);

        return saved;
    }

    public Bitmap GenererQRCode(string codeQr)
    {
        var generator = new QRCodeGenerator();
        var data = generator.CreateQrCode(codeQr, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new QRCode(data);
        return qrCode.GetGraphic(20);
    }
}