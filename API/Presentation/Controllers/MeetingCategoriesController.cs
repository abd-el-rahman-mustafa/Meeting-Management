using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers;

public class MeetingCategoriesController : BaseController
{
    private readonly IMeetingCategoryService _meetingCategoryService;

    public MeetingCategoriesController(IMeetingCategoryService meetingCategoryService)
    {
        _meetingCategoryService = meetingCategoryService;
    }

    [HttpGet]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _meetingCategoryService.GetAllAsync();
        return result.IsSuccess
            ? Ok(result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _meetingCategoryService.GetByIdAsync(id);
        return result.IsSuccess
            ? Ok(result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Create([FromBody] UpsertMeetingCategoryDto payload)
    {
        var result = await _meetingCategoryService.CreateAsync(payload);
        return result.IsSuccess
            ? StatusCode(result.StatusCode, result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Update(int id, [FromBody] UpsertMeetingCategoryDto payload)
    {
        var result = await _meetingCategoryService.UpdateAsync(id, payload);
        return result.IsSuccess
            ? Ok(result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _meetingCategoryService.DeleteAsync(id);
        return result.IsSuccess
            ? Ok(result)
            : Problem(title: result.Title, detail: result.Detail, statusCode: result.StatusCode);
    }
}
