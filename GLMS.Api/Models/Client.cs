using System.ComponentModel.DataAnnotations;

namespace GLMS.Api.Models;

public class Client
{
    public int ClientId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(200)]
    public string? ContactDetails { get; set; }

    [StringLength(100)]
    public string? Region { get; set; }

    public ICollection<Contract> Contracts { get; set; }
        = new List<Contract>();
}