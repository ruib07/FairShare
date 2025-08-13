using FairShare.Application.Interfaces.Repositories;
using FairShare.Domain.Models;
using FairShare.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FairShare.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FairShareDbContext _context;
    private DbSet<User> Users => _context.Users;

    public UserRepository(FairShareDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<User> GetUserById(Guid userId)
    {
        return await Users.FirstOrDefaultAsync(u => u.Id == userId);
    }
    public async Task<User> GetUserByEmail(string email)
    {
        return await Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> CreateUser(User user)
    {
        await Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task UpdateUser(User user)
    {
        Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(Guid userId)
    {
        var user = await GetUserById(userId);

        Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}
