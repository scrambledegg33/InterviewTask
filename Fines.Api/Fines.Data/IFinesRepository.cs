using Fines.Core.Dtos;
using Fines.Core.Enums;
using Fines.Data.Models;

public interface IFinesRepository
{
    Task<IEnumerable<FinesEntity>> GetFinesAsync(FinesFilter? filter = null);
}