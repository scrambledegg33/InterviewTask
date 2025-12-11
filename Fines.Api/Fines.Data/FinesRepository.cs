using Fines.Core.Dtos;
using Fines.Core.Enums;
using Fines.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Fines.Data;

public class FinesRepository : IFinesRepository
{
    private readonly FinesDbContext _context;

    public FinesRepository(FinesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FinesEntity>> GetFinesAsync(FinesFilter? filter = null)
    {
        IQueryable<FinesEntity> query = _context.Fines
                .Include(f => f.Vehicle)
                .AsQueryable();

        if (filter?.FineType.HasValue == true)
        {
            query = query.Where(f => f.FineType == filter.FineType.Value);
        }

        if (filter?.FineDate.HasValue == true)
        {
            query = query.Where(f => f.FineDate.Date == filter.FineDate.Value.Date);
        }

        if (!string.IsNullOrWhiteSpace(filter?.VehicleReg))
        {
            query = query.Where(f => f.Vehicle.RegistrationNumber == filter.VehicleReg);
        }

        return await query.ToListAsync();
    }

}
