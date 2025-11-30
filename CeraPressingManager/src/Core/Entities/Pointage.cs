using System;
using System.ComponentModel.DataAnnotations;

namespace CeraPressingManager.Core.Entities;

public class Pointage
{
    public int Id { get; set; }

    [Required]
    public int EmployeId { get; set; }
    public Employe Employe { get; set; } = null!;

    public DateTime HeureArrivee { get; set; }
    public DateTime? HeureDepart { get; set; }

    public TimeSpan? HeuresTravaillees => HeureDepart.HasValue ? HeureDepart.Value - HeureArrivee : null;
}
