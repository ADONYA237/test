using CeraPressingManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

using CeraPressingManager.Core.Common;

namespace CeraPressingManager.Data;

public class PressingDbContext : DbContext
{
    public PressingDbContext(DbContextOptions<PressingDbContext> options) : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Prestation> Prestations { get; set; } = null!;
    public DbSet<Commande> Commandes { get; set; } = null!;
    public DbSet<ArticleDepose> ArticlesDeposes { get; set; } = null!;
    public DbSet<Employe> Employes { get; set; } = null!;
    public DbSet<Pointage> Pointages { get; set; } = null!;
    public DbSet<LogAction> LogsActions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed des prestations (toujours dispo)
        modelBuilder.Entity<Employe>().HasData(
            new Employe
            {
                Id = 1,
                Nom = "Admin",
                Prenom = "Super",
                Telephone = "0000000000",
                Role = RoleEmploye.Administrateur,
                Login = "admin",
                MotDePasseHash = Employe.HashPassword("admin"), // Mot de passe par défaut: admin
                TauxHoraire = 0,
                Actif = true
            }
        );
        modelBuilder.Entity<Prestation>().HasData(
            new Prestation { Id = 1, TypeVetement = "Chemise", PrixUnitaire = 500, SupplementExpress2h = 3000, SupplementExpress24h = 1000 },
            new Prestation { Id = 2, TypeVetement = "Pantalon", PrixUnitaire = 1000, SupplementExpress2h = 4000, SupplementExpress24h = 1500 },
            new Prestation { Id = 3, TypeVetement = "Costume 3 pièces", PrixUnitaire = 5000, SupplementExpress2h = 15000, SupplementExpress24h = 5000 },
            new Prestation { Id = 4, TypeVetement = "Robe de soirée", PrixUnitaire = 8000, SupplementExpress2h = 20000, SupplementExpress24h = 8000 }
        );
    }
}