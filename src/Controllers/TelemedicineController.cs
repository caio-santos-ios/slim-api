using api_slim.src.Interfaces;
using api_slim.src.Models.Base;
using api_slim.src.Responses;
using api_slim.src.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_slim.src.Controllers
{
    [Route("api/telemedicine")]
    [ApiController]
    public class TelemedicineController(ITelemedicineService service) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            ResponseApi<List<dynamic>> response = await service.GetAllAsync();
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
        
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusTelemedicineDTO request)
        {
            ResponseApi<dynamic?> response = await service.UpdateStatusAsync(request);
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
    }
}