using Data.Contexts;
using Data.Entities;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UserProvider_G.Functions;

public class UpdateProfileImg(ILogger<UpdateProfileImg> logger, DataContext context)
{
    private readonly ILogger<UpdateProfileImg> _logger = logger;
    private readonly DataContext _context = context;

    [Function("UpdateProfileImg")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        try
        {
            var userProfileModel = req.ReadFromJsonAsync<ProfileModel>().Result;
            var user = await _context.Users
             .Include(u => u.UserProfile)
             .FirstOrDefaultAsync(u => u.Id == userProfileModel!.UserId);

            if (user != null)
            {
                if (user.UserProfile == null)
                {
                    user.UserProfile = new UserProfile();
                }
                user.UserProfile.ProfileImage = userProfileModel!.ProfileImage;
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            else
            {
                return new NotFoundResult();
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while update image");
            return new BadRequestResult();
        }
    }
}
