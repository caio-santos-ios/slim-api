using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_slim.src.Controllers
{
    [Route("api/accounts-payable")]
    [ApiController]
    public class AccountsPayableController(IAccountsPayableService service) : ControllerBase
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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountsPayableDTO user)
        {
            if (user == null) return BadRequest("Dados inválidos.");

            ResponseApi<AccountsPayable?> response = await service.CreateAsync(user);

            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
        
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAccountsPayableDTO user)
        {
            if (user == null) return BadRequest("Dados inválidos.");

            ResponseApi<AccountsPayable?> response = await service.UpdateAsync(user);

            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
       
        [Authorize]
        [HttpPut("low")]
        public async Task<IActionResult> Low([FromBody] UpdateAccountsPayableDTO user)
        {
            if (user == null) return BadRequest("Dados inválidos.");

            ResponseApi<AccountsPayable?> response = await service.UpdateLowAsync(user);

            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
        
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            ResponseApi<AccountsPayable> response = await service.DeleteAsync(id);

            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
    }
}