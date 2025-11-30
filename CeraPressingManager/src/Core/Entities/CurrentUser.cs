using CeraPressingManager.Core.Common;

namespace CeraPressingManager.Core.Entities;

// Classe simple pour stocker l'état de l'utilisateur connecté
public class CurrentUser
{
    public int EmployeId { get; private set; } = 0;
    public string NomComplet { get; private set; } = "Non Connecté";
    public RoleEmploye Role { get; private set; } = RoleEmploye.Receptionniste;

    public bool IsLoggedIn => EmployeId != 0;
    public bool IsAdmin => Role == RoleEmploye.Administrateur;

    public void Login(Employe employe)
    {
        EmployeId = employe.Id;
        NomComplet = $"{employe.Prenom} {employe.Nom}";
        Role = employe.Role;
    }

    public void Logout()
    {
        EmployeId = 0;
        NomComplet = "Non Connecté";
        Role = RoleEmploye.Receptionniste;
    }
}
