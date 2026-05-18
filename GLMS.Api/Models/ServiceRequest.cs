using GLMS.Api.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLMS.Api.Models;

public class ServiceRequest
{
    public int ServiceRequestId { get; set; }

    [Required]
    public int ContractId { get; set; }

    public Contract? Contract { get; set; }

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal CostUsd { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ExchangeRateUsed { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal CostZar { get; set; }

    public ServiceRequestStatus Status { get; set; }
}