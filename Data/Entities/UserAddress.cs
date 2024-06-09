

using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class UserAddress
{
    [Key]
    public int Id { get; set; }
    public string AddressType { get; set; } = null!;
    public string AddressLine_1 { get; set; } = null!;
    public string? AddressLine_2 { get; set; }

    public string PostCode { get; set; } = null!;
    public string City { get; set; } = null!;
}
