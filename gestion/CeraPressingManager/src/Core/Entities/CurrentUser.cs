public static class CurrentUser
{
    public static RoleUtiliseur? Role { get; set; }
    public static string? Nom { get; set; }

    public static bool EstAdmin => Role?.Nom == "Administrateur";
}