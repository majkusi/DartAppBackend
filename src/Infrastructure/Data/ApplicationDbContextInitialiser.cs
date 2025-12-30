using DartAppClean.Domain.Constants;
using DartAppClean.Domain.Entities;
using DartAppClean.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DartAppClean.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    //public async Task InitialiseAsync()
    //{
    //    try
    //    {
    //        // See https://jasontaylor.dev/ef-core-database-initialisation-strategies
    //        await _context.Database.EnsureDeletedAsync();
    //        await _context.Database.EnsureCreatedAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "An error occurred while initialising the database.");
    //        throw;
    //    }
    //}
    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole(Roles.Administrator);

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }
        var user1 = new ApplicationUser { UserName = "majkusi", Email = "majkusi@localhost", EmailConfirmed = true };
        var user2 = new ApplicationUser { UserName = "test1", Email = "majkusi@localhost", EmailConfirmed = true };
        var user3 = new ApplicationUser { UserName = "test2", Email = "majkusi@localhost", EmailConfirmed = true };
        var user4 = new ApplicationUser { UserName = "test3", Email = "majkusi@localhost", EmailConfirmed = true };
        var user5 = new ApplicationUser { UserName = "test4", Email = "majkusi@localhost", EmailConfirmed = true };

        await _userManager.CreateAsync(user1, "Strongpass1!");
        await _userManager.CreateAsync(user2, "Strongpass1!");
        await _userManager.CreateAsync(user3, "Strongpass1!");
        await _userManager.CreateAsync(user4, "Strongpass1!");
        await _userManager.CreateAsync(user5, "Strongpass1!");

        var player1 = new ApplicationUser { UserName = "Player 1", Email = "majkusi1@localhost", EmailConfirmed = true };
        var player2 = new ApplicationUser { UserName = "Player 2", Email = "majkusi2@localhost", EmailConfirmed = true };
        var player3 = new ApplicationUser { UserName = "Player 3", Email = "majkusi3@localhost", EmailConfirmed = true };
        var player4 = new ApplicationUser { UserName = "Player 4", Email = "majkusi4@localhost", EmailConfirmed = true };
        var player5 = new ApplicationUser { UserName = "Player 5", Email = "majkusi5@localhost", EmailConfirmed = true };

        await _userManager.CreateAsync(player1, "Strongpass1!1");
        await _userManager.CreateAsync(player2, "Strongpass1!2");
        await _userManager.CreateAsync(player3, "Strongpass1!3");
        await _userManager.CreateAsync(player4, "Strongpass1!4");
        await _userManager.CreateAsync(player5, "Strongpass1!5");
        // Default data
        // Seed, if necessary
        if (!_context.TodoLists.Any())
        {
            _context.TodoLists.Add(new TodoList
            {
                Title = "Todo List",
                Items =
                {
                    new TodoItem { Title = "Make a todo list 📃" },
                    new TodoItem { Title = "Check off the first item ✅" },
                    new TodoItem { Title = "Realise you've already done two things on the list! 🤯"},
                    new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
                }
            });

            await _context.SaveChangesAsync();
        }
    }
}
