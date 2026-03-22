namespace ScaffoldingSystem.Domain.Enums;

public enum UserRole
{
    Administrador = 1,
    Arrendatario = 2,
    Despachador = 3,
    Receptor = 4,
    Inspector = 5
}

public enum ScaffoldingStatus
{
    Disponible = 1,
    Alquilado = 2,
    EnTransito = 3,
    EnMantenimiento = 4,
    DadoDeBaja = 5
}

public enum ScaffoldingType
{
    Tubular = 1,
    Multidireccional = 2,
    Colgante = 3,
    Movil = 4,
    Fachada = 5
}

public enum RentalStatus
{
    Cotizado = 1,
    Aprobado = 2,
    Activo = 3,
    Devuelto = 4,
    Cancelado = 5
}

public enum DispatchStatus
{
    Pendiente = 1,
    EnCamino = 2,
    Entregado = 3,
    Cancelado = 4
}

public enum InspectionResult
{
    Aprobado = 1,
    ConObservaciones = 2,
    Rechazado = 3
}
