using CeraPressingManager.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace CeraPressingManager.Core.Entities;

public class Client
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom est obligatoire")]
    [MaxLength(100)]
    public string NomComplet { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; }

    public string IndicatifTelephone { get; set; } = "+237";
    public string? NumeroTelephone { get; set; }

    public CategorieClient Categorie { get; set; } = CategorieClient.Occasionnel;
    public int PointsFidelite { get; set; } = 0;
    public int NombreCommandes { get; set; } = 0;

    public DateTime DateInscription { get; set; } = DateTime.Today;

    public List<Commande> Commandes { get; set; } = new();
}