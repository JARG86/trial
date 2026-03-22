using ScaffoldingSystem.Domain.Enums;

namespace ScaffoldingSystem.Domain.Entities;

public class Scaffolding : BaseEntity
{
    public string Code { get; set; } = string.Empty;         // Ej: AND-001
    public string Description { get; set; } = string.Empty;
    public ScaffoldingType Type { get; set; }
    public ScaffoldingStatus Status { get; set; } = ScaffoldingStatus.Disponible;
    public decimal HeightMeters { get; set; }
    public decimal WeightKg { get; set; }
    public string Location { get; set; } = string.Empty;     // Ubicación actual en bodega
    public string? Notes { get; set; }

    // Navegación
    public ICollection<RentalItem> RentalItems { get; set; } = new List<RentalItem>();
    public ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
}
