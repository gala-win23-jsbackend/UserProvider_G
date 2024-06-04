using Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UserProvider_G.Functions
{
    public class GetOneUser(ILogger<GetOneUser> logger, DataContext context)
    {
        private readonly ILogger<GetOneUser> _logger = logger;
        private readonly DataContext _context = context;

        [Function("GetOneUser")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            var id = req.Query["id"].ToString();

            var user = await _context.Users
                .Include(u => u.UserProfile)
                .Include(u => u.UserAddress)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user != null)
            {
                return new OkObjectResult(user);
            }
            else
            {
                return new NotFoundObjectResult("User not found");
            }
        }
    }
}
