using GLMS.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLMS.Web.ViewModels
{
    public class ContractCreateViewModel
    {
        public int ClientId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ServiceLevel { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public List<SelectListItem> Clients { get; set; }
            = new();
    }
}