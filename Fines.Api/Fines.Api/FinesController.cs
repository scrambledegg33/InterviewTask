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

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FinesResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<FinesResponse>>> GetFines()
    {
        var fines = await _finesService.GetFinesAsync();
        return Ok(fines);
    }

    [HttpGet("{fineType:range(0,5)}")]
    [ProducesResponseType(typeof(IEnumerable<FinesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<FinesResponse>>> GetFinesFilteredByFineType(FineType fineType)
    {
        var fines = await _finesService.GetFinesFilteredByFineTypeAsync(fineType);
        if (fines == null)
        {
            return NotFound();
        }
        return Ok(fines);
    }
}
