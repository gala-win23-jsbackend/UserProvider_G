

namespace Data.Models;

public class UserForm
{
    public string Id { get; set; } = null!;
    public string? PreferredEmail { get; set; }

    public string AddressType { get; set; } = null!;
    public string AddressLine_1 { get; set; } = null!;
    public string? AddressLine_2 { get; set; }

    public string PostCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}
