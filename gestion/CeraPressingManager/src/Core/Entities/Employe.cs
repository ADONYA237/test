using CeraPressingManager.Core.Common;

namespace CeraPressingManager.Core.Entities;

public class Employe
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public RoleEmploye Role { get; set; } = RoleEmploye.Receptionniste;
    public string Login { get; set; } = string.Empty;
    public string MotDePasseHash { get; set; } = string.Empty;
    public decimal TauxHoraire { get; set; } = 0;
    public bool Actif { get; set; } = true;

    // Navigation
    public List<Livraison> Livraisons { get; set; } = new();
}