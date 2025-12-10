using Fines.Core.Enums;
using Fines.Data.Models;

public interface IFinesRepository
{
    Task<IEnumerable<FinesEntity>> GetAllFinesAsync();
    Task<IEnumerable<FinesEntity>> GetFinesFilteredByFineTypeAsync(FineType fineType);
}