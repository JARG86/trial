using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ScaffoldingSystem.Domain.Entities;
using ScaffoldingSystem.Domain.Enums;
using ScaffoldingSystem.Domain.Interfaces;
using ScaffoldingSystem.Infrastructure.Data;

namespace ScaffoldingSystem.Infrastructure.Repositories;

// ── GENERIC REPOSITORY ────────────────────────────────────────────────────────
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _ctx;
    protected readonly DbSet<T> _set;

    public Repository(AppDbContext ctx)
    {
        _ctx = ctx;
        _set = ctx.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id) =>
        await _set.FirstOrDefaultAsync(e => e.Id == id && e.IsActive);

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _set.Where(e => e.IsActive).ToListAsync();

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
        await _set.Where(predicate).ToListAsync();

    public async Task<T> AddAsync(T entity)
    {
        await _set.AddAsync(entity);
        await _ctx.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _set.Update(entity);
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is not null)
        {
            entity.IsActive = false;
            await UpdateAsync(entity);
        }
    }

    public async Task<bool> ExistsAsync(int id) =>
        await _set.AnyAsync(e => e.Id == id && e.IsActive);
}

// ── USER REPOSITORY ───────────────────────────────────────────────────────────
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<User?> GetByEmailAsync(string email) =>
        await _set.FirstOrDefaultAsync(u => u.Email == email.ToLower() && u.IsActive);

    public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role) =>
        await _set.Where(u => u.Role == role && u.IsActive).ToListAsync();

    public async Task<bool> EmailExistsAsync(string email) =>
        await _set.AnyAsync(u => u.Email == email.ToLower());
}

// ── SCAFFOLDING REPOSITORY ────────────────────────────────────────────────────
public class ScaffoldingRepository : Repository<Scaffolding>, IScaffoldingRepository
{
    public ScaffoldingRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<IEnumerable<Scaffolding>> GetByStatusAsync(ScaffoldingStatus status) =>
        await _set.Where(s => s.Status == status && s.IsActive).ToListAsync();

    public async Task<IEnumerable<Scaffolding>> GetAvailableAsync() =>
        await _set.Where(s => s.Status == ScaffoldingStatus.Disponible && s.IsActive).ToListAsync();

    public async Task<Scaffolding?> GetByCodeAsync(string code) =>
        await _set.FirstOrDefaultAsync(s => s.Code == code.ToUpper());
}

// ── RENTAL REPOSITORY ─────────────────────────────────────────────────────────
public class RentalRepository : Repository<Rental>, IRentalRepository
{
    public RentalRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<Rental?> GetWithDetailsAsync(int id) =>
        await _set
            .Include(r => r.Client)
            .Include(r => r.Items).ThenInclude(i => i.Scaffolding)
            .Include(r => r.Dispatches)
            .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);

    public async Task<IEnumerable<Rental>> GetByClientAsync(int clientUserId) =>
        await _set
            .Include(r => r.Client)
            .Include(r => r.Items).ThenInclude(i => i.Scaffolding)
            .Where(r => r.ClientUserId == clientUserId && r.IsActive)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<Rental>> GetByStatusAsync(RentalStatus status) =>
        await _set.Where(r => r.Status == status && r.IsActive).ToListAsync();

    public async Task<string> GenerateRentalNumberAsync()
    {
        var year = DateTime.UtcNow.Year;
        var count = await _set.CountAsync(r => r.CreatedAt.Year == year);
        return $"ALQ-{year}-{(count + 1):D4}";
    }
}

// ── DISPATCH REPOSITORY ───────────────────────────────────────────────────────
public class DispatchRepository : Repository<Dispatch>, IDispatchRepository
{
    public DispatchRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<IEnumerable<Dispatch>> GetByRentalAsync(int rentalId) =>
        await _set
            .Include(d => d.DispatchedBy)
            .Where(d => d.RentalId == rentalId && d.IsActive)
            .ToListAsync();

    public async Task<IEnumerable<Dispatch>> GetPendingAsync() =>
        await _set
            .Include(d => d.Rental)
            .Include(d => d.DispatchedBy)
            .Where(d => d.Status == DispatchStatus.Pendiente && d.IsActive)
            .ToListAsync();
}

// ── RECEPTION REPOSITORY ──────────────────────────────────────────────────────
public class ReceptionRepository : Repository<Reception>, IReceptionRepository
{
    public ReceptionRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<Reception?> GetByDispatchAsync(int dispatchId) =>
        await _set
            .Include(r => r.ReceivedBy)
            .FirstOrDefaultAsync(r => r.DispatchId == dispatchId && r.IsActive);
}

// ── INSPECTION REPOSITORY ─────────────────────────────────────────────────────
public class InspectionRepository : Repository<Inspection>, IInspectionRepository
{
    public InspectionRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<IEnumerable<Inspection>> GetByScaffoldingAsync(int scaffoldingId) =>
        await _set
            .Include(i => i.Inspector)
            .Where(i => i.ScaffoldingId == scaffoldingId && i.IsActive)
            .OrderByDescending(i => i.InspectionDate)
            .ToListAsync();

    public async Task<IEnumerable<Inspection>> GetByInspectorAsync(int inspectorUserId) =>
        await _set
            .Include(i => i.Scaffolding)
            .Where(i => i.InspectorUserId == inspectorUserId && i.IsActive)
            .OrderByDescending(i => i.InspectionDate)
            .ToListAsync();

    public async Task<Inspection?> GetLastInspectionAsync(int scaffoldingId) =>
        await _set
            .Include(i => i.Inspector)
            .Where(i => i.ScaffoldingId == scaffoldingId)
            .OrderByDescending(i => i.InspectionDate)
            .FirstOrDefaultAsync();
}
