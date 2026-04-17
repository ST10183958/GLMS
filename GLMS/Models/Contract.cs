using GLMS.Web.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLMS.Web.Models
{
    public class Contract
    {
        public int ContractId { get; set; }

        [Required]
        public int ClientId { get; set; }

        public Client? Client { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        public ContractStatus Status { get; set; }

        [Required]
        [Display(Name = "Service Level")]
        public string ServiceLevel { get; set; } = string.Empty;

        [Display(Name = "Signed Agreement File")]
        public string? SignedAgreementFilePath { get; set; }

        public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    }
}