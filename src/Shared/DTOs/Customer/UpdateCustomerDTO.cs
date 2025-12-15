using api_slim.src.Models;

namespace api_slim.src.Shared.DTOs
{
public class UpdateCustomerDTO
{
        public string Id { get; set; } = string.Empty;
        
        public string Type { get; set; } = string.Empty;

        public string CorporateName { get; set; } = string.Empty; // Mapeado do campo "Nome"
        
        public string TradeName { get; set; } = string.Empty; // Mapeado do campo "Nome"

        public string Document { get; set; } = string.Empty; // Mapeado do campo "CPF"

        public string Rg { get; set; } = string.Empty; // Mapeado do campo "RG"

        public DateTime DateOfBirth { get; set; } // Mapeado do campo "Data de Nascimento"

        public string Gender { get; set; } = string.Empty; // Mapeado do campo "Gênero"

        public Address Address { get; set; } = new Address();

        public string Phone { get; set; } = string.Empty; // Mapeado do campo "Telefone"

        public string Whatsapp { get; set; } = string.Empty; // Mapeado do campo "Whatsapp"

        public string Email { get; set; } = string.Empty; // Mapeado do campo "E-mail"

        public string Segments { get; set; } = string.Empty; // Mapeado do campo "Origem" (5)

        public string Origin { get; set; } = string.Empty; // Mapeado do campo "Origem" (5)

        public ContractorResponsible Responsible { get; set; } = new();

        public DateTime? EffectiveDate { get; set; }

        public string PlanId { get; set; } = string.Empty; 

        public string Notes { get; set; } = string.Empty;
       
        public decimal MinimumValue { get; set; }

        public string TypePlan { get; set; } = string.Empty;
        public string SellerId { get; set; } = string.Empty;
}
}