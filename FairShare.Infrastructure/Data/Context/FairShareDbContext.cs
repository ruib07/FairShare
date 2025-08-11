using FairShare.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FairShare.Infrastructure.Data.Context;

public class FairShareDbContext : DbContext
{
    public FairShareDbContext(DbContextOptions<FairShareDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<ExpenseParticipant> ExpenseParticipants { get; set; }
    public DbSet<Settlement> Settlements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
