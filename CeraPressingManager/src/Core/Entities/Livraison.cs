using CeraPressingManager.Core.Common;

namespace CeraPressingManager.Core.Entities;

public class Livraison
{
    public int Id { get; set; }
    public string Adresse { get; set; } = string.Empty;
    public decimal FraisLivraison { get; set; } = 0;
    public string? SignatureClient { get; set; } // base64 ou chemin
    public DateTime? DateLivraison { get; set; }

    public string StatutLivraison { get; set; } = "EnAttente";

    // Clés étrangères
    public int CommandeId { get; set; }
    public Commande Commande { get; set; } = null!;
    public int EmployeId { get; set; }
    public Employe Employe { get; set; } = null!;
}