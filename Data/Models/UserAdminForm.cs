

namespace Data.Models;

public class UserAdminForm
{
    public string UserId { get; set; } = null!;
    public bool IsAdmin { get; set; }
    public bool IsUser { get; set; }
    public bool IsManager { get; set; }
    public bool IsSuperAdmin { get; set; }
}
