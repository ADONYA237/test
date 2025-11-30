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

    // Méthode utilitaire pour hacher un mot de passe simple (à remplacer par BCrypt en production)
    public static string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
    public decimal TauxHoraire { get; set; } = 0;
    public bool Actif { get; set; } = true;

    // Navigation
    public List<Livraison> Livraisons { get; set; } = new();
}