using System;
using System.ComponentModel.DataAnnotations;

namespace CeraPressingManager.Core.Entities;

public class LogAction
{
    public int Id { get; set; }
    public DateTime DateAction { get; set; } = DateTime.Now;

    [Required]
    public int EmployeId { get; set; }
    public Employe Employe { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string Action { get; set; } = string.Empty;

    [MaxLength(1024)]
    public string? Details { get; set; }
}
