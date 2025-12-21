using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_slim.src.Controllers
{
    [Route("api/accounts-receivable")]
    [ApiController]
    public class AccountsReceivableController(IAccountsReceivableService userService) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            PaginationApi<List<dynamic>> response = await userService.GetAllAsync(new(Request.Query));
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
        
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            ResponseApi<dynamic?> response = await userService.GetByIdAggregateAsync(id);
            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountsReceivableDTO user)
        {
            if (user == null) return BadRequest("Dados inválidos.");

            ResponseApi<AccountsReceivable?> response = await userService.CreateAsync(user);

            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
        
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAccountsReceivableDTO user)
        {
            if (user == null) return BadRequest("Dados inválidos.");

            ResponseApi<AccountsReceivable?> response = await userService.UpdateAsync(user);

            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
       
        [Authorize]
        [HttpPut("low")]
        public async Task<IActionResult> Low([FromBody] UpdateAccountsReceivableDTO user)
        {
            if (user == null) return BadRequest("Dados inválidos.");

            ResponseApi<AccountsReceivable?> response = await userService.UpdateLowAsync(user);

            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
        
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            ResponseApi<AccountsReceivable> response = await userService.DeleteAsync(id);

            return StatusCode(response.StatusCode, new { response.Message, response.Result });
        }
    }
}