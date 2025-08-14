using FairShare.Application.Constants;
using FairShare.Application.Interfaces.Services;
using FairShare.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FairShare.API.Controllers;

[Route($"api/{AppSettings.ApiVersion}/expense-participants")]
[Authorize]
public class ExpenseParticipantsController : ControllerBase
{
    private readonly IExpenseParticipantsService _expenseParticipantsService;

    public ExpenseParticipantsController(IExpenseParticipantsService expenseParticipantsService)
    {
        _expenseParticipantsService = expenseParticipantsService ?? 
            throw new ArgumentNullException(nameof(expenseParticipantsService));
    }

    // GET api/v1/expense-participants/{expenseId}
    [HttpGet("{expenseId}")]
    public async Task<IActionResult> GetParticipantsByExpenseId(Guid expenseId)
    {
        return Ok(await _expenseParticipantsService.GetParticipantsByExpenseId(expenseId));
    }

    // GET api/v1/expense-participants/{expenseId}/is-participating/{userId}
    [HttpGet("{expenseId}/is-participating/{userId}")]
    public async Task<IActionResult> IsUserParticipating(Guid expenseId, Guid userId)
    {
        return Ok(await _expenseParticipantsService.IsUserParticipating(expenseId, userId));
    }

    // POST api/v1/expense-participants
    [HttpPost]
    public async Task<IActionResult> AddParticipant([FromBody] ExpenseParticipant expenseParticipant)
    {
        await _expenseParticipantsService.AddParticipant(expenseParticipant);

        return CreatedAtAction(nameof(GetParticipantsByExpenseId), new { expenseId = expenseParticipant.ExpenseId }, expenseParticipant);
    }

    // DELETE api/v1/expense-participants/{expenseId}/{userId}
    [HttpDelete("{expenseId}/{userId}")]
    public async Task<IActionResult> RemoveParticipant(Guid expenseId, Guid userId)
    {
        await _expenseParticipantsService.RemoveParticipant(expenseId, userId);

        return NoContent();
    }
}
