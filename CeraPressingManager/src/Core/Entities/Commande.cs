using CeraPressingManager.Core.Common;

namespace CeraPressingManager.Core.Entities;

public class Commande
{
    public int Id { get; set; }
    public DateTime DateDepot { get; set; } = DateTime.Now;
    public DateTime? DatePrevue { get; set; }
    public DateTime? DateLivraison { get; set; }

    public StatutCommande Statut { get; set; } = StatutCommande.Enregistree;
    public bool EstExpress { get; set; } = false;

    public decimal MontantTotal { get; set; }
    public decimal MontantPaye { get; set; } = 0;
    public decimal RemiseAppliquee { get; set; }
    public string? QrCodeTicket { get; set; }

    // FK
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public int? LivreurId { get; set; }
    public Employe? Livreur { get; set; }

    // Navigation
    public List<ArticleDepose> Articles { get; set; } = new();
    public List<Paiement> Paiements { get; set; } = new();
    public Livraison? Livraison { get; set; }
}