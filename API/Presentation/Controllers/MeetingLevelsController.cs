using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers;

[Route("api/meeting-levels")]
public class MeetingLevelsController : BaseController
{
    private readonly IMeetingLevelService _meetingLevelService;

    public MeetingLevelsController(IMeetingLevelService meetingLevelService)
    {
        _meetingLevelService = meetingLevelService;
    }

    [HttpGet]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _meetingLevelService.GetAllAsync();
        return result.IsSuccess
            ? Ok(result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _meetingLevelService.GetByIdAsync(id);
        return result.IsSuccess
            ? Ok(result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Create([FromBody] UpsertMeetingLevelDto payload)
    {
        var result = await _meetingLevelService.CreateAsync(payload);
        return result.IsSuccess
            ? StatusCode(result.StatusCode, result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Update(int id, [FromBody] UpsertMeetingLevelDto payload)
    {
        var result = await _meetingLevelService.UpdateAsync(id, payload);
        return result.IsSuccess
            ? Ok(result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _meetingLevelService.DeleteAsync(id);
        return result.IsSuccess
            ? Ok(result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }
}
