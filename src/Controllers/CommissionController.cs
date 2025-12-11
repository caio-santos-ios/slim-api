using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_slim.src.Controllers
{
    [Route("api/commissions")]
[ApiController]
public class CommissionController(ICommissionService commissionService) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        PaginationApi<List<dynamic>> response = await commissionService.GetAllAsync(new(Request.Query));
        return StatusCode(response.StatusCode, new { response.Message, response.Result });
    }
    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        ResponseApi<dynamic?> response = await commissionService.GetByIdAggregateAsync(id);
        return StatusCode(response.StatusCode, new { response.Message, response.Result });
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCommissionDTO commission)
    {
        if (commission == null) return BadRequest("Dados inválidos.");

        ResponseApi<Commission?> response = await commissionService.CreateAsync(commission);

        return StatusCode(response.StatusCode, new { response.Message });
    }
    
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateCommissionDTO commission)
    {
        if (commission == null) return BadRequest("Dados inválidos.");

        ResponseApi<Commission?> response = await commissionService.UpdateAsync(commission);

        return StatusCode(response.StatusCode, new { response.Message });
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        ResponseApi<Commission> response = await commissionService.DeleteAsync(id);

        return StatusCode(response.StatusCode, new { response.Message });
    }
}
}