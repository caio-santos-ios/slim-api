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
public class ServiceModuleController(IServiceModuleService serviceModuleService) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        PaginationApi<List<dynamic>> response = await serviceModuleService.GetAllAsync(new(Request.Query));
        return StatusCode(response.StatusCode, new { response.Message, response.Result });
    }
    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        ResponseApi<dynamic?> response = await serviceModuleService.GetByIdAggregateAsync(id);
        return StatusCode(response.StatusCode, new { response.Message, response.Result });
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateServiceModuleDTO serviceModule)
    {
        if (serviceModule == null) return BadRequest("Dados inválidos.");

        ResponseApi<ServiceModule?> response = await serviceModuleService.CreateAsync(serviceModule);

        return StatusCode(response.StatusCode, new { response.Message });
    }
    
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateServiceModuleDTO serviceModule)
    {
        if (serviceModule == null) return BadRequest("Dados inválidos.");

        ResponseApi<ServiceModule?> response = await serviceModuleService.UpdateAsync(serviceModule);

        return StatusCode(response.StatusCode, new { response.Message });
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        ResponseApi<ServiceModule> response = await serviceModuleService.DeleteAsync(id);

        return StatusCode(response.StatusCode, new { response.Message });
    }
}
}