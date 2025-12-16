using Microsoft.AspNetCore.Mvc;

namespace api_slim.src.Controllers
{
    [Route("check")]
    [ApiController]
    public class CheckController() : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            
            return Ok(new  {Message = "Sua API esta funcionando"});
        }
    }
}