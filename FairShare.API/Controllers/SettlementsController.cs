using FairShare.Application.Constants;
using FairShare.Application.Interfaces.Services;
using FairShare.Application.Shared.DTOs;
using FairShare.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FairShare.API.Controllers;

[Route($"api/{AppSettings.ApiVersion}/settlements")]
[Authorize]
public class SettlementsController : ControllerBase
{
    private readonly ISettlementsService _settlementsService;

    public SettlementsController(ISettlementsService settlementsService)
    {
        _settlementsService = settlementsService ?? throw new ArgumentNullException(nameof(settlementsService));
    }

    // GET api/v1/settlements/{settlementId}
    [HttpGet("{settlementId}")]
    public async Task<IActionResult> GetSettlementById(Guid settlementId)
    {
        var result = await _settlementsService.GetSettlementById(settlementId);

        if (!result.IsSuccess) return StatusCode(result.Error.StatusCode, result.Error);

        return Ok(result.Data);
    }

    // GET api/v1/settlements/group/{groupId}
    [HttpGet("group/{groupId}")]
    public async Task<ActionResult<IEnumerable<Settlement>>> GetSettlementsByGroupId(Guid groupId)
    {
        return Ok(await _settlementsService.GetSettlementsByGroupId(groupId));
    }

    // GET api/v1/settlements/from/{userId}
    [HttpGet("from/{userId}")]
    public async Task<ActionResult<IEnumerable<Settlement>>> GetSettlementsFromUserId(Guid userId)
    {
        return Ok(await _settlementsService.GetSettlementsFromUserId(userId));
    }

    // GET api/v1/settlements/to/{userId}
    [HttpGet("to/{userId}")]
    public async Task<ActionResult<IEnumerable<Settlement>>> GetSettlementsToUserId(Guid userId)
    {
        return Ok(await _settlementsService.GetSettlementsToUserId(userId));
    }

    // POST api/v1/settlements
    [HttpPost]
    public async Task<IActionResult> CreateSettlement([FromBody] Settlement settlement)
    {
        var result = await _settlementsService.CreateSettlement(settlement);

        if (!result.IsSuccess) return StatusCode(result.Error.StatusCode, result.Error);

        var response = new ResponsesDTO.Creation(result.Message, result.Data.Id);

        return CreatedAtAction(nameof(GetSettlementById), new { settlementId = result.Data.Id }, response);
    }

    // PUT api/v1/settlements/{settlementId}
    [HttpPut("{settlementId}")]
    public async Task<IActionResult> UpdateSettlement(Guid settlementId, [FromBody] Settlement updateSettlement)
    {
        var result = await _settlementsService.UpdateSettlement(settlementId, updateSettlement);

        if (!result.IsSuccess) return StatusCode(result.Error.StatusCode, result.Error);

        return Ok(result.Message);
    }

    // DELETE api/v1/settlements/{settlementId}
    [HttpDelete("{settlementId}")]
    public async Task<IActionResult> DeleteSettlement(Guid settlementId)
    {
        await _settlementsService.DeleteSettlement(settlementId);

        return NoContent();
    }
}
