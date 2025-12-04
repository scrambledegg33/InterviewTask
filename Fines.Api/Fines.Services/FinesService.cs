using Fines.Core.Dtos;
using Fines.Data.Models;

namespace Fines.Services;

public class FinesService : IFinesService
{
    private readonly IFinesRepository _finesRepository;

    public FinesService(IFinesRepository finesRepository)
    {
        _finesRepository = finesRepository;
    }

    public async Task<IEnumerable<FinesResponse>> GetFinesAsync()
    {
        var fines = await _finesRepository.GetAllFinesAsync();
        return fines.Select(MapToResponse);
    }

    private static FinesResponse MapToResponse(FinesEntity fine)
    {
        return new FinesResponse
        {
            Id = fine.Id,
            FineNo = fine.FineNo,
            FineDate = fine.FineDate,
            FineType = fine.FineType,
            CustomerName = fine.CustomerName,
            VehicleRegNo = fine.VehicleRegNo,
            VehicleDriverName = fine.VehicleDriverName
        };
    }
}
