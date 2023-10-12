using Iceni.Api.Services;
using Iceni.Lib.Models.Api;
using Iceni.Lib.Models.Dto;
using Iceni.Lib.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iceni.Api.Controllers;

/// <summary>
///     Controller for managing pupils
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "adm")]
public class PupilController : Controller
{
    private readonly PupilService _pupilService;

    /// <summary>
    ///     ctr     
    /// </summary>
    /// <param name="pupilService"></param>
    public PupilController(PupilService pupilService)
    {
        _pupilService = pupilService;
    }

    /// <summary>
    ///     Queries available pupils
    /// </summary>
    /// <param name="query"></param>
    /// <param name="type"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PageOf<PupilDto>> QueryPupils(string? query, PupilType? type, int? skip = null, int? take = null)
    {
        var res = await _pupilService.QueryPupils(query, type, skip, take);
        return new PageOf<PupilDto>(res.Total, res.Rows.Select(x => new PupilDto(x)));
    }

    /// <summary>
    ///     Gets a pupil by id
    /// </summary>
    /// <param name="pupilId"></param>
    /// <returns></returns>
    [HttpGet("{pupilId:guid}")]
    public async Task<PupilDto> GetPupil([FromRoute] Guid pupilId)
    {
        var res = await _pupilService.GetPupil(pupilId);
        return new PupilDto(res);
    }

    /// <summary>
    ///     Creates a new blank pupil and returns it
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<PupilDto> CreateNewPupil()
    {
        var res = await _pupilService.CreateNewPupil();
        return new PupilDto(res);
    }

    /// <summary>
    ///     Updates a pupil and returns it
    /// </summary>
    /// <param name="pupilUpdate"></param>
    /// <returns></returns>
    [HttpPatch]
    public async Task<PupilDto> UpdatePupil([FromBody] PupilDto pupilUpdate)
    {
        var res = await _pupilService.UpdatePupil(pupilUpdate);
        return new PupilDto(res);
    }

    /// <summary>
    ///     Deletes a pupil by id
    /// </summary>
    /// <param name="pupilId"></param>
    /// <returns></returns>
    [HttpDelete("{pupilId:guid}")]
    public async Task<IActionResult> DeletePupil(Guid pupilId)
    {
        await _pupilService.DeletePupil(pupilId);
        return Ok();
    }
}