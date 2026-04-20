using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers;

[Route("api/meeting-settings")]
public class MeetingSettingsController : BaseController
{
    private readonly IMeetingSettingsService _meetingSettingsService;

    public MeetingSettingsController(IMeetingSettingsService meetingSettingsService)
    {
        _meetingSettingsService = meetingSettingsService;
    }

    [HttpGet]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Get()
    {
        var result = await _meetingSettingsService.GetAsync();
        return result.IsSuccess
            ? Ok(result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }

    [HttpPut]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Update([FromBody] UpsertMeetingSettingsDto payload)
    {
        var result = await _meetingSettingsService.UpsertAsync(payload);
        return result.IsSuccess
            ? Ok(result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }
}
