using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers;

[Route("api/meeting-types")]
public class MeetingTypesController : BaseController
{
    private readonly IMeetingTypeService _meetingTypeService;

    public MeetingTypesController(IMeetingTypeService meetingTypeService)
    {
        _meetingTypeService = meetingTypeService;
    }

    [HttpGet]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _meetingTypeService.GetAllAsync();
        return result.IsSuccess
            ? Ok(result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _meetingTypeService.GetByIdAsync(id);
        return result.IsSuccess
            ? Ok(result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Create([FromBody] UpsertMeetingTypeDto payload)
    {
        var result = await _meetingTypeService.CreateAsync(payload);
        return result.IsSuccess
            ? StatusCode(result.StatusCode, result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Update(int id, [FromBody] UpsertMeetingTypeDto payload)
    {
        var result = await _meetingTypeService.UpdateAsync(id, payload);
        return result.IsSuccess
            ? Ok(result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _meetingTypeService.DeleteAsync(id);
        return result.IsSuccess
            ? Ok(result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }
}
