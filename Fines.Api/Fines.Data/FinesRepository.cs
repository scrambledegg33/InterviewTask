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

    public async Task<IEnumerable<FinesEntity>> GetAllFinesAsync()
    {
        var result = await _context.Fines
            .Include(f => f.Vehicle)
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<FinesEntity>> GetFinesFilteredByFineTypeAsync(FineType finetype)
    {
        var result = await _context.Fines
            .Include(f => f.Vehicle)
            .Where(f => f.FineType == finetype)
            .ToListAsync();

        return result;
    }
}
