namespace api_slim.src.Shared.DTOs
{
    public class UpdateUserDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public List<Module> Modules {get;set;} = [];
    }
}