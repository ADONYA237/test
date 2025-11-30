namespace CeraPressingManager.Core.Common;

public enum CategorieClient
{
    Occasionnel,
    Regulier,
    VIP
}

public enum StatutCommande
{
    Enregistree,
    EnCoursLavage,
    EnCoursRepassage,
    Prete,
    Livree,
    Annulee
}

public enum ModePaiement
{
    Cash,
    OrangeMoney,
    MTNMoney,
    Wave,
    CarteBancaire
}

public enum RoleEmploye
{
    Administrateur,
    Caissier,
    Receptionniste,
    Laveur,
    Repasseur,
    Livreur
}

public enum TypeService
{
    Lavage,
    Repassage,
    NettoyageASec,
    CoutureRetouche,
    LavageSpecial
}