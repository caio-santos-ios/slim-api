using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_slim.src.Controllers
{
    [Route("api/billings")]
[ApiController]
public class BillingController(IBillingService billingService) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        PaginationApi<List<dynamic>> response = await billingService.GetAllAsync(new(Request.Query));
        return StatusCode(response.StatusCode, new { response.Message, response.Result });
    }
    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        ResponseApi<dynamic?> response = await billingService.GetByIdAggregateAsync(id);
        return StatusCode(response.StatusCode, new { response.Message, response.Result });
    }

    [Authorize]
    [HttpGet("select")]
    public async Task<IActionResult> GetSelect()
    {
        ResponseApi<List<dynamic>> response = await billingService.GetSelectAsync(new(Request.Query));
        return StatusCode(response.StatusCode, new { response.Message, response.Result });
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBillingDTO billing)
    {
        if (billing == null) return BadRequest("Dados inválidos.");

        ResponseApi<Billing?> response = await billingService.CreateAsync(billing);

        return StatusCode(response.StatusCode, new { response.Message, response.Result });
    }
    
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateBillingDTO billing)
    {
        if (billing == null) return BadRequest("Dados inválidos.");

        ResponseApi<Billing?> response = await billingService.UpdateAsync(billing);

        return StatusCode(response.StatusCode, new { response.Message, response.Result });
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        ResponseApi<Billing> response = await billingService.DeleteAsync(id);

        return StatusCode(response.StatusCode, new { response.Message });
    }
}   
}