using FairShare.Application.Interfaces.Repositories;
using FairShare.Application.Interfaces.Services;
using FairShare.Application.Shared.Common;
using FairShare.Domain.Enums;
using FairShare.Domain.Models;

namespace FairShare.Infrastructure.Services;

public class ExpensesService : IExpensesService
{
    private readonly IExpenseRepository _expenseRepository;

    public ExpensesService(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository ?? throw new ArgumentNullException(nameof(expenseRepository));
    }

    public async Task<Result<Expense>> GetExpenseById(Guid expenseId)
    {
        var expense = await _expenseRepository.GetExpenseById(expenseId);

        if (expense == null) return Result<Expense>.Fail("Expense not found.", 404);

        return Result<Expense>.Success(expense);
    }

    public async Task<IEnumerable<Expense>> GetExpensesByGroupId(Guid groupId)
    {
        return await _expenseRepository.GetExpensesByGroupId(groupId);
    }

    public async Task<IEnumerable<Expense>> GetPaidExpensesByUserId(Guid userId)
    {
        return await _expenseRepository.GetPaidExpensesByUserId(userId);
    }

    public async Task<Result<Expense>> CreateExpense(Expense expense)
    {
        var validation = ValidateExpenseFields(expense);

        if (!validation.IsSuccess) 
            return Result<Expense>.Fail(validation.Error.Message, validation.Error.StatusCode);

        var createdExpense = await _expenseRepository.CreateExpense(expense);

        return Result<Expense>.Success(createdExpense, "Expense created successfully.");
    }

    public async Task<Result<Expense>> UpdateExpense(Guid expenseId, Expense updateExpense)
    {
        var currentExpense = await _expenseRepository.GetExpenseById(expenseId);

        var validation = ValidateExpenseFields(updateExpense);

        if (!validation.IsSuccess) 
            return Result<Expense>.Fail(validation.Error.Message, validation.Error.StatusCode);

        currentExpense.Description = updateExpense.Description;
        currentExpense.Amount = updateExpense.Amount;
        currentExpense.Date = updateExpense.Date;
        currentExpense.Category = updateExpense.Category;
        currentExpense.ReceiptImage = updateExpense.ReceiptImage;

        await _expenseRepository.UpdateExpense(currentExpense);

        return Result<Expense>.Success(currentExpense, "Expense updated successfully.");
    }

    public async Task DeleteExpense(Guid expenseId)
    {
        await _expenseRepository.DeleteExpense(expenseId);
    }

    #region Private Methods

    private static Result<bool> ValidateExpenseFields(Expense expense)
    {
        if (string.IsNullOrWhiteSpace(expense.Description))
            return Result<bool>.Fail("Description cannot be empty.", 400);

        if (expense.Amount <= 0)
            return Result<bool>.Fail("Amount must be greater than zero.", 400);

        if (string.IsNullOrWhiteSpace(expense.Description))
            return Result<bool>.Fail("Description cannot be empty.", 400);

        if (expense.Date == DateTime.MinValue)
            return Result<bool>.Fail("Date is required.", 400);

        if (!Enum.IsDefined(typeof(Categories), expense.Category))
            return Result<bool>.Fail("Invalid category.", 400);

        return Result<bool>.Success(true);
    }

    #endregion Private Methods
}
