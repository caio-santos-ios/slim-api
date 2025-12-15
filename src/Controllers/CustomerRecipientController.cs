using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_slim.src.Controllers
{
[Route("api/customer-recipients")]
[ApiController]
public class CustomerRecipientController(ICustomerRecipientService customerRecipientService) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        PaginationApi<List<dynamic>> response = await customerRecipientService.GetAllAsync(new(Request.Query));
        return StatusCode(response.StatusCode, new { response.Message, response.Result });
    }
    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        ResponseApi<dynamic?> response = await customerRecipientService.GetByIdAggregateAsync(id);
        return StatusCode(response.StatusCode, new { response.Message, response.Result });
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRecipientDTO customer)
    {
        if (customer == null) return BadRequest("Dados inválidos.");

        ResponseApi<CustomerRecipient?> response = await customerRecipientService.CreateAsync(customer);

        return StatusCode(response.StatusCode, new { response.Message });
    }
    
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateCustomerRecipientDTO customer)
    {
        if (customer == null) return BadRequest("Dados inválidos.");

        ResponseApi<CustomerRecipient?> response = await customerRecipientService.UpdateAsync(customer);

        return StatusCode(response.StatusCode, new { response.Message });
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        ResponseApi<CustomerRecipient> response = await customerRecipientService.DeleteAsync(id);

        return StatusCode(response.StatusCode, new { response.Message });
    }
}
}