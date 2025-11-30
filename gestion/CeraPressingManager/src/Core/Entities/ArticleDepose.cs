namespace CeraPressingManager.Core.Entities;

public class ArticleDepose
{
    public int Id { get; set; }
    public string Couleur { get; set; } = string.Empty;
    public string? DescriptionDommages { get; set; }
    public string? Commentaire { get; set; }
    public string? PhotoAvant { get; set; }
    public string? PhotoApres { get; set; }

    public int Quantite { get; set; } = 1;  // AJOUTÉ ICI

    public int CommandeId { get; set; }
    public Commande Commande { get; set; } = null!;
    public int PrestationId { get; set; }
    public Prestation Prestation { get; set; } = null!;
    public bool EstExpress { get; set; } = false;
    public decimal PrixTotal => Prestation != null
      ? (Prestation.PrixUnitaire * Quantite) +
        (EstExpress ? (DateTime.Now.Hour < 14 ? Prestation.SupplementExpress2h : Prestation.SupplementExpress24h) : 0)
      : 0;
}