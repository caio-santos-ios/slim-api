using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using api_slim.src.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_slim.src.Controllers
{
    [Route("api/procedures")]
[ApiController]
public class ProcedureController(IProcedureService service) : ControllerBase
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
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProcedureDTO procedure)
    {
        if (procedure == null) return BadRequest("Dados inválidos.");

        ResponseApi<Procedure?> response = await service.CreateAsync(procedure);

        return StatusCode(response.StatusCode, new { response.Message });
    }
    
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateProcedureDTO procedure)
    {
        if (procedure == null) return BadRequest("Dados inválidos.");

        ResponseApi<Procedure?> response = await service.UpdateAsync(procedure);

        return StatusCode(response.StatusCode, new { response.Message });
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        ResponseApi<Procedure> response = await service.DeleteAsync(id);

        return StatusCode(response.StatusCode, new { response.Message });
    }
}
}