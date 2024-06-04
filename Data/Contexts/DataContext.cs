

using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserAddress> UserAddresses { get; set; }
}
