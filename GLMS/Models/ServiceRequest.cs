using GLMS.Web.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLMS.Web.Models
{
    public class ServiceRequest
    {
        public int ServiceRequestId { get; set; }

        [Required]
        public int ContractId { get; set; }

        public Contract? Contract { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostUsd { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ExchangeRateUsed { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostZar { get; set; }

        [Required]
        public ServiceRequestStatus Status { get; set; }
    }
}