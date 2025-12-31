using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_slim.src.Controllers
{
    [Route("api/trading-tables")]
    [ApiController]
    public class TradingTableController(ITradingTableService service) : ControllerBase
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
        [HttpGet("accredited-network/{accreditedNetworkId}")]
        public async Task<IActionResult> GetByaccreditedNetworkIdAsync(string accreditedNetworkId)
        {
            ResponseApi<dynamic?> response = await service.GetByaccreditedNetworkIdAsync(accreditedNetworkId);
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
                
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTradingTableDTO address)
        {
            if (address == null) return BadRequest("Dados inválidos.");

            ResponseApi<TradingTable?> response = await service.CreateAsync(address);

            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
        
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTradingTableDTO address)
        {
            if (address == null) return BadRequest("Dados inválidos.");

            ResponseApi<TradingTable?> response = await service.UpdateAsync(address);

            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
        
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            ResponseApi<TradingTable> response = await service.DeleteAsync(id);

            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
    }
}