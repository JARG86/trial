using ScaffoldingSystem.Domain.Enums;

namespace ScaffoldingSystem.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public UserRole Role { get; set; }

    // Navegación
    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    public ICollection<Dispatch> Dispatches { get; set; } = new List<Dispatch>();
    public ICollection<Reception> Receptions { get; set; } = new List<Reception>();
    public ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
}
