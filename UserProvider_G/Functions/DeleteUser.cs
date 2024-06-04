using Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace UserProvider_G.Functions;

public class DeleteUser(ILogger<DeleteUser> logger, DataContext context)
{
    private readonly ILogger<DeleteUser> _logger = logger;
    private readonly DataContext _context = context;

    [Function("DeleteUser")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        try
        {
            var userId = await req.ReadFromJsonAsync<UserIdModel>();
            var user = await _context.Users.FindAsync(userId!.Id);
            if (user == null)
            {
                return new NotFoundResult();
            }
            else
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error under deleting user");
            return new BadRequestResult();
        }
    }


}
public class UserIdModel
{
    public string Id { get; set; } = null!;
}