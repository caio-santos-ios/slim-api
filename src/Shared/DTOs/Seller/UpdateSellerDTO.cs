using api_slim.src.Models;

namespace api_slim.src.Shared.DTOs
{
public class UpdateSellerDTO
{
public string Id { get; set; } = string.Empty;
        
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Cpf { get; set; } = string.Empty;

        public Address Address { get; set; } = new Address();

        public string Notes { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Parent { get; set; } = string.Empty;
        public string ParentId { get; set; } = string.Empty;

}
}