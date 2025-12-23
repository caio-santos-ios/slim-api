namespace api_slim.src.Shared.DTOs
{
    public class UpdateStatusTelemedicineDTO
    {
        public string Id {get;set;} = string.Empty; 
        public bool Status { get; set; }
    }
}