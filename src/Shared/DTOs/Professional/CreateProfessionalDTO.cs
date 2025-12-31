using api_slim.src.Models;

namespace api_slim.src.Shared.DTOs
{
        public class CreateProfessionalDTO
        {
                public string Name { get; set; } = string.Empty;

                public string Email { get; set; } = string.Empty;

                public string Phone { get; set; } = string.Empty;

                public string Cpf { get; set; } = string.Empty;

                public Address Address { get; set; } = new Address();

                public string Type { get; set; } = string.Empty;
                
                public string Specialty { get; set; } = string.Empty;
        
                public string Registration { get; set; } = string.Empty;
        
                public string Number { get; set; } = string.Empty;
                public DateTime? EffectiveDate { get; set; }
        }
}