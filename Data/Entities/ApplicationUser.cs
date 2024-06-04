

using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class ApplicationUser : IdentityUser
{
    public string? UserProfileId { get; set; }
    public virtual UserProfile? UserProfile { get; set; }

    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }

    public string? UserAddressId { get; set; }
    public virtual UserAddress? UserAddress { get; set; }
    public string? PreferredEmail { get; set; }
    public bool SubscribeNewsletter { get; set; } = false;
    public bool DarkMode { get; set; } = false;
}
