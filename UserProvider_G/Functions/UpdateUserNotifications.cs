using Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

public class UpdateUserNotifications(ILogger<UpdateUserNotifications> logger, DataContext context, HttpClient httpClient)
{
    private readonly ILogger<UpdateUserNotifications> _logger = logger;
    private readonly DataContext _context = context;
    private readonly HttpClient _httpClient = httpClient;

    [Function("UpdateUserNotifications")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        try
        {
            var notificationModel = req.ReadFromJsonAsync<NotificationsFormModel>().Result;
            if (notificationModel == null)
            {
                return new BadRequestObjectResult("Invalid request");
            }
            else
            {
                var user = _context.Users.Find(notificationModel.UserId);
                if (user != null)
                {
                    user.PreferredEmail = notificationModel.Email;
                    user.SubscribeNewsletter = notificationModel.SubscribeNewsletter;
                    user.DarkMode = notificationModel.DarkMode;
                    await _context.SaveChangesAsync();
                    if (user.SubscribeNewsletter)
                    {
                        SubscribeToNewsletter subscribeToNewsletter = new SubscribeToNewsletter
                        {
                            UserEmail = user.Email,
                            PreferredEmail = user.PreferredEmail,
                            AdvertisingUpdates = true,
                            WeekInReview = true,
                            Podcasts = true,
                            StartupsWeekly = true,
                            DailyNewsletter = true,
                            EventUpdates = true,
                        };
                        var subscribeResponse = await _httpClient.PostAsJsonAsync("https://newsletters-g.azurewebsites.net/api/Subscribe?code=5XtUE2UNlWlJoiTXPCwxeZ8I9qPy-nQUVdYUyS3VX8vDAzFu64fJhA%3D%3D", subscribeToNewsletter);
                        if (subscribeResponse.IsSuccessStatusCode)
                        {
                            return new OkObjectResult(user);

                        }

                    }
                    else if (!user.SubscribeNewsletter)
                    {
                        var subscriber = new Subscriber
                        {
                            Email = user.Email
                        };
                        var unsubscribeResponse = await _httpClient.PostAsJsonAsync("https://newsletters-g.azurewebsites.net/api/DeleteSubscriber?code=iKzIGMAg0iqS05aGHG0DPXcOlzpeKQMSKd9XXzLk5S8XAzFuZ7QvVA%3D%3D", subscriber);
                        if (unsubscribeResponse.IsSuccessStatusCode)
                        {
                            return new OkObjectResult(user);
                        }
                        else
                        {
                            return new BadRequestResult();
                        }
                    }

                }

            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user notifications");
            return new BadRequestResult();
        }
        return new BadRequestResult();
    }

}
public class Subscriber
{
    public string Email { get; set; } = null!;
}
public class SubscribeToNewsletter
{
    public string UserEmail { get; set; } = null!;
    public string PreferredEmail { get; set; } = null!;
    public bool AdvertisingUpdates { get; set; } = false;
    public bool WeekInReview { get; set; } = false;
    public bool Podcasts { get; set; } = false;
    public bool StartupsWeekly { get; set; } = false;
    public bool DailyNewsletter { get; set; } = false;
    public bool EventUpdates { get; set; } = false;
}
public class NotificationsFormModel
{
    public string Email { get; set; } = null!;
    public bool SubscribeNewsletter { get; set; }

    public bool DarkMode { get; set; }
    public string UserId { get; set; } = null!;
}
