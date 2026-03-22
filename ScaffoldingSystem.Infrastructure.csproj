using ScaffoldingSystem.Domain.Enums;

namespace ScaffoldingSystem.Domain.Entities;

// ── CONTRATO DE ALQUILER ──────────────────────────────────────────────────────
public class Rental : BaseEntity
{
    public string RentalNumber { get; set; } = string.Empty;   // Ej: ALQ-2024-001
    public int ClientUserId { get; set; }                       // Arrendatario
    public DateTime StartDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public RentalStatus Status { get; set; } = RentalStatus.Cotizado;
    public decimal TotalAmount { get; set; }
    public string DeliveryAddress { get; set; } = string.Empty;
    public string? Observations { get; set; }

    // Navegación
    public User Client { get; set; } = null!;
    public ICollection<RentalItem> Items { get; set; } = new List<RentalItem>();
    public ICollection<Dispatch> Dispatches { get; set; } = new List<Dispatch>();
}

// ── ITEMS DEL ALQUILER ────────────────────────────────────────────────────────
public class RentalItem : BaseEntity
{
    public int RentalId { get; set; }
    public int ScaffoldingId { get; set; }
    public int Quantity { get; set; }
    public decimal DailyRate { get; set; }

    // Navegación
    public Rental Rental { get; set; } = null!;
    public Scaffolding Scaffolding { get; set; } = null!;
}

// ── DESPACHO ──────────────────────────────────────────────────────────────────
public class Dispatch : BaseEntity
{
    public int RentalId { get; set; }
    public int DispatchedByUserId { get; set; }               // Despachador
    public DateTime DispatchDate { get; set; }
    public DispatchStatus Status { get; set; } = DispatchStatus.Pendiente;
    public string TransportPlate { get; set; } = string.Empty;
    public string DriverName { get; set; } = string.Empty;
    public string? GuideNumber { get; set; }
    public string? Notes { get; set; }

    // Navegación
    public Rental Rental { get; set; } = null!;
    public User DispatchedBy { get; set; } = null!;
    public Reception? Reception { get; set; }
}

// ── RECEPCIÓN ─────────────────────────────────────────────────────────────────
public class Reception : BaseEntity
{
    public int DispatchId { get; set; }
    public int ReceivedByUserId { get; set; }                 // Receptor
    public DateTime ReceptionDate { get; set; }
    public bool IsComplete { get; set; }
    public string? MissingItems { get; set; }
    public string? DamagedItems { get; set; }
    public string? Notes { get; set; }

    // Navegación
    public Dispatch Dispatch { get; set; } = null!;
    public User ReceivedBy { get; set; } = null!;
}

// ── INSPECCIÓN ────────────────────────────────────────────────────────────────
public class Inspection : BaseEntity
{
    public int ScaffoldingId { get; set; }
    public int InspectorUserId { get; set; }                  // Inspector
    public DateTime InspectionDate { get; set; }
    public InspectionResult Result { get; set; }
    public string Findings { get; set; } = string.Empty;
    public string? Recommendations { get; set; }
    public string? PhotoUrl { get; set; }
    public int? RentalId { get; set; }                        // Opcional: ligado a un alquiler

    // Navegación
    public Scaffolding Scaffolding { get; set; } = null!;
    public User Inspector { get; set; } = null!;
    public Rental? Rental { get; set; }
}
