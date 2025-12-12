using api_slim.src.Models;

namespace api_slim.src.Shared.DTOs
{
public class CreateSellerRepresentativeDTO
{
   public string Notes { get; set; } = string.Empty;
        
        public string CNPJ { get; set; } = string.Empty;
        
        public string tradeName { get; set; } = string.Empty;

        public string CorporateName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Whatsapp { get; set; } = string.Empty;

        public DateTime EffectiveDate { get; set; }

        public List<string> Selllers { get; set; } = new();

        public Address Address { get; set; } = new Address();

        public RepresentativeResponsible RepresentativeResponsible { get; set; } = new();
       
        public List<Contact> Contacts { get; set; } = new();
}
}