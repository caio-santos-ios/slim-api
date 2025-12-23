using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_slim.src.Controllers
{
    [Route("api/service-modules")]
    [ApiController]
    public class ServiceModuleController(IServiceModuleService service) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            PaginationApi<List<dynamic>> response = await service.GetAllAsync(new(Request.Query));
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
        
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            ResponseApi<dynamic?> response = await service.GetByIdAggregateAsync(id);
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }

        [Authorize]
        [HttpGet("select")]
        public async Task<IActionResult> GetSelectAsync()
        {
            ResponseApi<List<dynamic>> response = await service.GetSelectAsync(new(Request.Query));
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
        
        [Authorize]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateServiceModuleDTO serviceModule)
        {
            if (serviceModule == null) return BadRequest("Dados inválidos.");

            ResponseApi<ServiceModule?> response = await service.CreateAsync(serviceModule);

            return StatusCode(response.StatusCode, new { response.Message });
        }
        
        [Authorize]
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] UpdateServiceModuleDTO serviceModule)
        {
            if (serviceModule == null) return BadRequest("Dados inválidos.");

            ResponseApi<ServiceModule?> response = await service.UpdateAsync(serviceModule);

            return StatusCode(response.StatusCode, new { response.Message });
        }

    [Authorize]
        [HttpPut("save-image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateImage([FromForm] UpdateServiceModuleDTO serviceModule)
        {
            if (serviceModule == null) return BadRequest("Dados inválidos.");

            ResponseApi<ServiceModule?> response = await service.UpdateImageAsync(serviceModule);

            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
        
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            ResponseApi<ServiceModule> response = await service.DeleteAsync(id);

            return StatusCode(response.StatusCode, new { response.Message });
        }
    }
}