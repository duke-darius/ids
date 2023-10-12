using Iceni.Api.Services;
using Iceni.Lib.Models.Api;
using Iceni.Lib.Models.Dto;
using Iceni.Lib.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iceni.Api.Controllers;

/// <summary>
///     Controller for managing pupil payments
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "adm")]
public class PaymentController : Controller
{
    private readonly PaymentService _paymentService;

    /// <summary>
    ///     ctr
    /// </summary>
    /// <param name="paymentService"></param>
    public PaymentController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    /// <summary>
    ///     Adds a payment
    /// </summary>
    /// <param name="pupilId"></param>
    /// <param name="amount"></param>
    /// <param name="hours"></param>
    /// <param name="paymentType"></param>
    /// <returns></returns>
    [HttpPost("{pupilId:guid}")]
    public async Task<PaymentDto> AddPayment([FromRoute] Guid pupilId, [FromForm] decimal amount, [FromForm] float hours, [FromForm] PaymentType paymentType)
    {
        var res = await _paymentService.AddPayment(pupilId, amount, hours, paymentType);
        return new PaymentDto(res);
    }

    /// <summary>
    ///     Deletes a payment by payment id
    /// </summary>
    /// <param name="paymentId"></param>
    /// <returns></returns>
    /// <exception cref="ApiException"></exception>
    [HttpDelete("{paymentId:guid}")]
    public async Task<IActionResult> DeletePayment(Guid paymentId)
    {
        await _paymentService.DeletePayment(paymentId);
        return Ok();
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="pupilId"></param>
    /// <param name="paymentType"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PageOf<PaymentDto>> QueryPayments(Guid? pupilId = null, PaymentType? paymentType = null,
        int? skip = null, int? take = null)
    {
        var res = await _paymentService.QueryPayments(pupilId, paymentType, skip, take);
        return new PageOf<PaymentDto>(res.Total, res.Rows.Select(x => new PaymentDto(x, true)));
    }
}