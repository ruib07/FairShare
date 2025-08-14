using FairShare.Application.Constants;
using FairShare.Application.Interfaces.Services;
using FairShare.Application.Shared.DTOs;
using FairShare.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FairShare.API.Controllers;

[Route($"api/{AppSettings.ApiVersion}/expenses")]
[Authorize]
public class ExpensesController : ControllerBase
{
    private readonly IExpensesService _expensesService;

    public ExpensesController(IExpensesService expensesService)
    {
        _expensesService = expensesService ?? throw new ArgumentNullException(nameof(expensesService));
    }

    // GET api/v1/expenses/{expenseId}
    [HttpGet("{expenseId}")]
    public async Task<IActionResult> GetExpenseById(Guid expenseId)
    {
        var result = await _expensesService.GetExpenseById(expenseId);

        if (!result.IsSuccess) return StatusCode(result.Error.StatusCode, result.Error);

        return Ok(result.Data);
    }

    // GET api/v1/expenses/by-group/{groupId}
    [HttpGet("by-group/{groupId}")]
    public async Task<ActionResult<IEnumerable<Expense>>> GetExpensesByGroupId(Guid groupId)
    {
        return Ok(await _expensesService.GetExpensesByGroupId(groupId));
    }

    // GET api/v1/expenses/paid/{userId}
    [HttpGet("paid/{userId}")]
    public async Task<ActionResult<IEnumerable<Expense>>> GetPaidExpensesByUserId(Guid userId)
    {
        return Ok(await _expensesService.GetPaidExpensesByUserId(userId));
    }

    // POST api/v1/expenses
    [HttpPost]
    public async Task<IActionResult> CreateExpense([FromBody] Expense expense)
    {
        var result = await _expensesService.CreateExpense(expense);

        if (!result.IsSuccess) return StatusCode(result.Error.StatusCode, result.Error);

        var response = new ResponsesDTO.Creation(result.Message, result.Data.Id);

        return CreatedAtAction(nameof(GetExpenseById), new { expenseId = result.Data.Id }, response);
    }

    // PUT api/v1/expenses/{expenseId}
    [HttpPut("{expenseId}")]
    public async Task<IActionResult> UpdateExpense(Guid expenseId, [FromBody] Expense updateExpense)
    {
        var result = await _expensesService.UpdateExpense(expenseId, updateExpense);

        if (!result.IsSuccess) return StatusCode(result.Error.StatusCode, result.Error);

        return Ok(result.Message);
    }

    // DELETE api/v1/expenses/{expenseId}
    [HttpDelete("{expenseId}")]
    public async Task<IActionResult> DeleteExpense(Guid expenseId)
    {
        await _expensesService.DeleteExpense(expenseId);

        return NoContent();
    }
}
