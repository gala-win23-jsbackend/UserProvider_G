

using Data.Contexts;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public class UserService(DataContext context)
{
    private readonly DataContext _context = context;

    public async Task<List<UsersWithRolesDisplay>> GetAllUsersWithRolesAsync(List<ApplicationUser> users)
    {
        try
        {
            var userWithRolesList = new List<UsersWithRolesDisplay>();

            foreach (var user in users)
            {
                var roles = await _context.Roles
                  .Where(r => _context.UserRoles
                   .Where(ur => ur.UserId == user.Id)
                  .Select(ur => ur.RoleId)
                  .Contains(r.Id))
                  .Select(r => r.Name)
                  .ToListAsync();

                var userWithRoles = new UsersWithRolesDisplay
                {
                    FirstName = user.UserProfile?.FirstName ?? string.Empty,
                    LastName = user.UserProfile?.LastName ?? string.Empty,
                    Email = user.Email!,
                    Id = user.Id,
                    Roles = roles.Count != 0 ? roles : null
                };

                userWithRolesList.Add(userWithRoles);
            }

            return userWithRolesList;
        }
        catch (Exception)
        {
            return null!;
        }
    }
}
