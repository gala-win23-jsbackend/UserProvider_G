

namespace Data.Models;

public class UsersWithRolesDisplay
{
    public string Id { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<string>? Roles { get; set; }
}
