using CeraPressingManager.Core.Common;

namespace CeraPressingManager.Core.Entities;

public class Prestation
{
    public int Id { get; set; }

    public string TypeVetement { get; set; } = string.Empty; // Chemise, Pantalon, Robe, Tapis...
    public ServiceType TypeService { get; set; }
    public string? Matiere { get; set; } // Coton, Soie, Cuir...

    public decimal PrixUnitaire { get; set; }
    public int DelaiStandardHeures { get; set; } = 24;

    public decimal SupplementExpress24h { get; set; } = 1000;
    public decimal SupplementExpress2h { get; set; } = 5000;
}