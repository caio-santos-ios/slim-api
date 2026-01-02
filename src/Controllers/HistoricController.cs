using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_slim.src.Controllers
{
    [Route("api/historics")]
    [ApiController]
    public class HistoricController(IHistoricService service) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            PaginationApi<List<dynamic>> response = await service.GetAllAsync(new(Request.Query));
            return StatusCode(response.StatusCode, new { response.Result });
        }
               
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHistoricDTO contact)
        {
            if (contact == null) return BadRequest("Dados inválidos.");

            ResponseApi<Historic?> response = await service.CreateAsync(contact);

            return StatusCode(response.StatusCode, new { response.Message });
        }
    }
}