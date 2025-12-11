using Fines.Core.Dtos;
using Fines.Core.Enums;

public interface IFinesService
{
    Task<IEnumerable<FinesResponse>> GetFinesAsync(FinesFilter? filter = null);
}
