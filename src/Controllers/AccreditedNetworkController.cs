using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_slim.src.Controllers
{
    [Route("api/accredited-networks")]
    [ApiController]
    public class AccreditedNetworkController(IAccreditedNetworkService accreditedNetworkService) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            PaginationApi<List<dynamic>> response = await accreditedNetworkService.GetAllAsync(new(Request.Query));
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
        
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            ResponseApi<dynamic?> response = await accreditedNetworkService.GetByIdAggregateAsync(id);
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccreditedNetworkDTO accreditedNetwork)
        {
            if (accreditedNetwork == null) return BadRequest("Dados inválidos.");

            ResponseApi<AccreditedNetwork?> response = await accreditedNetworkService.CreateAsync(accreditedNetwork);

            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
        
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAccreditedNetworkDTO accreditedNetwork)
        {
            if (accreditedNetwork == null) return BadRequest("Dados inválidos.");

            ResponseApi<AccreditedNetwork?> response = await accreditedNetworkService.UpdateAsync(accreditedNetwork);

            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
        
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            ResponseApi<AccreditedNetwork> response = await accreditedNetworkService.DeleteAsync(id);

            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
    }
}