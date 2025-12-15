using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_slim.src.Controllers
{
    [Route("api/seller-representatives")]
    [ApiController]
    public class SellerRepresentativeController(ISellerRepresentativeService sellerRepresentativeService) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            PaginationApi<List<dynamic>> response = await sellerRepresentativeService.GetAllAsync(new(Request.Query));
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
        
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            ResponseApi<dynamic?> response = await sellerRepresentativeService.GetByIdAggregateAsync(id);
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSellerRepresentativeDTO sellerRepresentative)
        {
            if (sellerRepresentative == null) return BadRequest("Dados inválidos.");

            ResponseApi<SellerRepresentative?> response = await sellerRepresentativeService.CreateAsync(sellerRepresentative);

            return StatusCode(response.StatusCode, new { response.Message, response.Data });
        }
        
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateSellerRepresentativeDTO sellerRepresentative)
        {
            if (sellerRepresentative == null) return BadRequest("Dados inválidos.");

            ResponseApi<SellerRepresentative?> response = await sellerRepresentativeService.UpdateAsync(sellerRepresentative);

            return StatusCode(response.StatusCode, new { response.Message });
        }
        
        [Authorize]
        [HttpPut("responsible")]
        public async Task<IActionResult> UpdateResponsible([FromBody] UpdateSellerRepresentativeDTO sellerRepresentative)
        {
            if (sellerRepresentative == null) return BadRequest("Dados inválidos.");

            ResponseApi<SellerRepresentative?> response = await sellerRepresentativeService.UpdateResponsibleAsync(sellerRepresentative);

            return StatusCode(response.StatusCode, new { response.Message });
        }
        
        [Authorize]
        [HttpPut("seller")]
        public async Task<IActionResult> UpdateSeller([FromBody] UpdateSellerRepresentativeDTO sellerRepresentative)
        {
            if (sellerRepresentative == null) return BadRequest("Dados inválidos.");

            ResponseApi<SellerRepresentative?> response = await sellerRepresentativeService.UpdateSellerAsync(sellerRepresentative);

            return StatusCode(response.StatusCode, new { response.Message });
        }
        
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            ResponseApi<SellerRepresentative> response = await sellerRepresentativeService.DeleteAsync(id);

            return StatusCode(response.StatusCode, new { response.Message });
        }
    }
}