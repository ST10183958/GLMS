using GLMS.Web.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLMS.Web.ViewModels
{
    public class ServiceRequestCreateViewModel
    {
        [Required]
        public int ContractId { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 100000000)]
        public decimal CostUsd { get; set; }

        public decimal ExchangeRateUsed { get; set; }
        public decimal CalculatedCostZar { get; set; }

        [Required]
        public ServiceRequestStatus Status { get; set; }

        public IEnumerable<SelectListItem> Contracts { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}