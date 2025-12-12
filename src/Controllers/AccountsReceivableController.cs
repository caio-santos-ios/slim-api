using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace api_slim.src.Controllers
{
    [Route("api/accounts-receivable")]
    [ApiController]
    public class AccountsReceivableController(IAccountsReceivableService userService) : ControllerBase
    {
        // [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            PaginationApi<List<dynamic>> users = await userService.GetAllAsync(new(Request.Query));
            return users.IsSuccess ? Ok(users) : BadRequest(new{ users.Message});
        }
        
        // [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            ResponseApi<dynamic?> user = await userService.GetByIdAggregateAsync(id);
            return user.IsSuccess ? Ok(user) : BadRequest(new{ user.Message });
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountsReceivableDTO user)
        {
            if (user == null) return BadRequest("Dados inválidos.");

            ResponseApi<AccountsReceivable?> response = await userService.CreateAsync(user);

            return response.IsSuccess ? Ok(new{response.Message}) : BadRequest(new{response.Message});
        }
        
        // [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAccountsReceivableDTO user)
        {
            if (user == null) return BadRequest("Dados inválidos.");

            ResponseApi<AccountsReceivable?> response = await userService.UpdateAsync(user);

            return response.IsSuccess ? Ok(new{response.Message}) : BadRequest(new{response.Message});
        }
        
        // [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            ResponseApi<AccountsReceivable> response = await userService.DeleteAsync(id);

            return response.IsSuccess ? Ok(new{response.Message}) : BadRequest(new{response.Message});
        }
    }
}