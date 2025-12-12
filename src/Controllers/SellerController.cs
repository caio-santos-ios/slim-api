using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_slim.src.Controllers
{
    [Route("api/sellers")]
[ApiController]
public class SellerController(ISellerService sellerService) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        PaginationApi<List<dynamic>> response = await sellerService.GetAllAsync(new(Request.Query));
        return StatusCode(response.StatusCode, new { response.Message, response.Result });
    }
    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        ResponseApi<dynamic?> response = await sellerService.GetByIdAggregateAsync(id);
        return StatusCode(response.StatusCode, new { response.Message, response.Result });
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSellerDTO seller)
    {
        if (seller == null) return BadRequest("Dados inválidos.");

        ResponseApi<Seller?> response = await sellerService.CreateAsync(seller);

        return StatusCode(response.StatusCode, new { response.Message });
    }
    
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateSellerDTO seller)
    {
        if (seller == null) return BadRequest("Dados inválidos.");

        ResponseApi<Seller?> response = await sellerService.UpdateAsync(seller);

        return StatusCode(response.StatusCode, new { response.Message });
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        ResponseApi<Seller> response = await sellerService.DeleteAsync(id);

        return StatusCode(response.StatusCode, new { response.Message });
    }
}
}