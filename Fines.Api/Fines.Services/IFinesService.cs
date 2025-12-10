using Fines.Core.Dtos;
using Fines.Core.Enums;

public interface IFinesService
{
    Task<IEnumerable<FinesResponse>> GetFinesAsync();
    Task<IEnumerable<FinesResponse>> GetFinesFilteredByFineTypeAsync(FineType fineType);
}
