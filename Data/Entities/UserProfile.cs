
namespace Data.Entities;

public class UserProfile
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string? ProfileImage { get; set; } = "avatar.jpg";
    public string? Bio { get; set; }
}
