using GLMS.Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace GLMS.Api.Models;

public class Contract
{
    public int ContractId { get; set; }

    [Required]
    public int ClientId { get; set; }

    public Client? Client { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public ContractStatus Status { get; set; }

    [StringLength(100)]
    public string? ServiceLevel { get; set; }

    public string? SignedAgreementFilePath { get; set; }

    public ICollection<ServiceRequest> ServiceRequests { get; set; }
        = new List<ServiceRequest>();
}