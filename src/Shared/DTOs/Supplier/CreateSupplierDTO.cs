
using api_slim.src.Models;

namespace api_slim.src.Shared.DTOs
{
        public class CreateSupplierDTO
        {        
                public string Type { get; set; } = "F";

                public string Document { get; set; } = string.Empty;
                public string CorporateName { get; set; } = string.Empty;

                public string TradeName { get; set; } = string.Empty;

                public string Phone { get; set; } = string.Empty;

                public string Email { get; set; } = string.Empty;

                public Address Address { get; set; } = new Address();

                public string StateRegistration { get; set; } = string.Empty;

                public string MunicipalRegistration { get; set; } = string.Empty;

                public string Notes { get; set; } = string.Empty;
        }
}