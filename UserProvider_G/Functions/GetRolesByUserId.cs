using Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UserProvider_G.Functions;

public class GetRolesByUserId(ILogger<GetRolesByUserId> logger, DataContext context)
{
    private readonly ILogger<GetRolesByUserId> _logger = logger;
    private readonly DataContext _context = context;

    [Function("GetRolesByUserId")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
    {
        try
        {
            var userId = req.Query["userId"].ToString();


            var user = await _context.Users
                  .Include(u => u.UserProfile)
                  .Include(u => u.UserAddress)
                  .FirstOrDefaultAsync(u => u.Id == userId);

            var userRoles = await _context.Roles
                 .Where(r => _context.UserRoles
                  .Where(ur => ur.UserId == user!.Id)
                 .Select(ur => ur.RoleId)
                 .Contains(r.Id))
                 .Select(r => r.Name)
                 .ToListAsync();

            return new OkObjectResult(userRoles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting user by ID");
            return new BadRequestObjectResult("Error while getting user by ID");
        }
    }
}
