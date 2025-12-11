namespace api_slim.src.Responses
{
    public class AuthResponse
    {
        public string Token {get;set;} = string.Empty; 
        public string RefreshToken  {get;set;} = string.Empty; 
        public string Name {get;set;} = string.Empty; 
        public string Role {get;set;} = string.Empty; 
        public string Id {get;set;} = string.Empty; 
    }
}