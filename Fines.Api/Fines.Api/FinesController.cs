using Fines.Core.Dtos;
using Fines.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Fines.Api;

[Route("api/[controller]")]
[ApiController]
public class FinesController : ControllerBase
{
    private readonly IFinesService _finesService;

    public FinesController(IFinesService finesService)
    {
        _finesService = finesService;
    }

    //[HttpGet]
    //[ProducesResponseType(typeof(IEnumerable<FinesResponse>), StatusCodes.Status200OK)]
    //public async Task<ActionResult<IEnumerable<FinesResponse>>> GetFines()
    //{
    //    var fines = await _finesService.GetFinesAsync();
    //    return Ok(fines);
    //}

    //[HttpGet("{fineType:range(0,5)}")]
    //[ProducesResponseType(typeof(IEnumerable<FinesResponse>), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<ActionResult<IEnumerable<FinesResponse>>> GetFinesFilteredByFineType(FineType fineType)
    //{
    //    var fines = await _finesService.GetFinesFilteredByFineTypeAsync(fineType);
    //    if (fines == null)
    //    {
    //        return NotFound();
    //    }
    //    return Ok(fines);
    //}

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FinesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<FinesResponse>>> GetFines([FromQuery] FineType? fineType, [FromQuery] DateTime? fineDate, [FromQuery] string? vehicleReg)
    {
        var filter = new FinesFilter
        {
            FineType = fineType,
            FineDate = fineDate,
            VehicleReg = string.IsNullOrWhiteSpace(vehicleReg) ? null : vehicleReg
        };
        var fines = await _finesService.GetFinesAsync(filter);
        if (fines == null)
        {
            var customResponse = new { message = $"No Fine Found with the given Fine Type: {fineType} and/or fine Date: {fineDate} and/or Vehicle Reg {vehicleReg}" };
            return NotFound(customResponse);
        }
        return Ok(fines);
    }
}
