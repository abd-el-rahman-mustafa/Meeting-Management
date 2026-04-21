using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers;

[Route("api/agenda-item-types")]
public class AgendaItemTypesController : BaseController
{
    private readonly IAgendaItemTypeService _agendaItemTypeService;

    public AgendaItemTypesController(IAgendaItemTypeService agendaItemTypeService)
    {
        _agendaItemTypeService = agendaItemTypeService;
    }

    [HttpGet]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _agendaItemTypeService.GetAllAsync();
        return result.IsSuccess
            ? Ok(result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _agendaItemTypeService.GetByIdAsync(id);
        return result.IsSuccess
            ? Ok(result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Create([FromBody] UpsertAgendaItemTypeDto payload)
    {
        var result = await _agendaItemTypeService.CreateAsync(payload);
        return result.IsSuccess
            ? StatusCode(result.StatusCode, result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Update(int id, [FromBody] UpsertAgendaItemTypeDto payload)
    {
        var result = await _agendaItemTypeService.UpdateAsync(id, payload);
        return result.IsSuccess
            ? Ok(result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _agendaItemTypeService.DeleteAsync(id);
        return result.IsSuccess
            ? Ok(result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }
}