using ScaffoldingSystem.Domain.Entities;
using ScaffoldingSystem.Domain.Enums;

namespace ScaffoldingSystem.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetByRoleAsync(UserRole role);
    Task<bool> EmailExistsAsync(string email);
}

public interface IScaffoldingRepository : IRepository<Scaffolding>
{
    Task<IEnumerable<Scaffolding>> GetByStatusAsync(ScaffoldingStatus status);
    Task<IEnumerable<Scaffolding>> GetAvailableAsync();
    Task<Scaffolding?> GetByCodeAsync(string code);
}

public interface IRentalRepository : IRepository<Rental>
{
    Task<Rental?> GetWithDetailsAsync(int id);
    Task<IEnumerable<Rental>> GetByClientAsync(int clientUserId);
    Task<IEnumerable<Rental>> GetByStatusAsync(RentalStatus status);
    Task<string> GenerateRentalNumberAsync();
}

public interface IDispatchRepository : IRepository<Dispatch>
{
    Task<IEnumerable<Dispatch>> GetByRentalAsync(int rentalId);
    Task<IEnumerable<Dispatch>> GetPendingAsync();
}

public interface IReceptionRepository : IRepository<Reception>
{
    Task<Reception?> GetByDispatchAsync(int dispatchId);
}

public interface IInspectionRepository : IRepository<Inspection>
{
    Task<IEnumerable<Inspection>> GetByScaffoldingAsync(int scaffoldingId);
    Task<IEnumerable<Inspection>> GetByInspectorAsync(int inspectorUserId);
    Task<Inspection?> GetLastInspectionAsync(int scaffoldingId);
}
