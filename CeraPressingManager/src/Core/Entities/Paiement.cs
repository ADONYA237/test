using CeraPressingManager.Core.Common;

namespace CeraPressingManager.Core.Entities;

public class Paiement
{
    public int Id { get; set; }
    public DateTime DatePaiement { get; set; } = DateTime.Now;
    public decimal Montant { get; set; }
    public ModePaiement Mode { get; set; } = ModePaiement.Cash;
    public string? Description { get; set; }

    // Clés étrangères
    public int CommandeId { get; set; }
    public Commande Commande { get; set; } = null!;
}