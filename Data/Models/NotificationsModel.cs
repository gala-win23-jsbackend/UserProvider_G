

namespace Data.Models;

public class NotificationsModel
{
    public string Email { get; set; } = null!;
    public bool SubscribeNewsletter { get; set; }

    public bool DarkMode { get; set; }
    public string UserId { get; set; } = null!;
}
