using Fines.Core.Enums;

namespace Fines.Core.Dtos;

public class FinesFilter
{
    public FineType? FineType { get; set; }
    public DateTime? FineDate { get; set; }
    public string? VehicleReg { get; set; }
}
