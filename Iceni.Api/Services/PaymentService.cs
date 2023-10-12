using Iceni.Lib.EfModels;
using Iceni.Lib.Models.Api;
using Iceni.Lib.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Iceni.Api.Services;

/// <summary>
///     Service for managing pupil payments
/// </summary>
public class PaymentService
{
    private readonly IDbContextFactory<IceniCtx> _contextFactory;
    private readonly CurrentUserService _currentUserService;

    /// <summary>
    ///     ctr
    /// </summary>
    /// <param name="contextFactory"></param>
    /// <param name="currentUserService"></param>
    public PaymentService(IDbContextFactory<IceniCtx> contextFactory, CurrentUserService currentUserService)
    {
        _contextFactory = contextFactory;
        _currentUserService = currentUserService;
    }

    /// <summary>
    ///     Adds a new payment
    /// </summary>
    /// <param name="pupilId"></param>
    /// <param name="amount"></param>
    /// <param name="hours"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public async Task<Payment> AddPayment(Guid pupilId, decimal amount, double hours, PaymentType type)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();

        var pupil = await ctx.Pupils.SingleAsync(x => x.Id == pupilId);
        
        var payment = new Payment()
        {
            TutorId = _currentUserService.GetCurrentUsersId(),
            AmountPaid = amount,
            Hours = hours,
            DateCreated = DateTime.UtcNow,
            PaymentType = type
        };

        pupil.Payments.Add(payment);
        await ctx.SaveChangesAsync();

        return payment;
    }

    /// <summary>
    ///     Deletes a payment
    /// </summary>
    /// <param name="paymentId"></param>
    /// <exception cref="ApiException"></exception>
    public async Task DeletePayment(Guid paymentId)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();

        var payment = await ctx.Payments.SingleAsync(x => x.Id == paymentId);
        if (payment.PaymentType != PaymentType.Cash)
            throw new ApiException(ErrorCodes.UnknownError);

        ctx.Payments.Remove(payment);
        await ctx.SaveChangesAsync();
    }

    /// <summary>
    ///     Queries payments
    /// </summary>
    /// <param name="pupilId"></param>
    /// <param name="paymentType"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    public async Task<PageOf<Payment>> QueryPayments(Guid? pupilId, PaymentType? paymentType, int? skip, int? take)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();

        var payments = ctx.Payments.Include(x=> x.Pupil).AsQueryable();

        if (pupilId.HasValue)
            payments = payments.Where(x => x.PupilId == pupilId);
        if (paymentType.HasValue)
            payments = payments.Where(x => x.PaymentType == paymentType);
        
        var count = await payments.CountAsync();

        if (skip.HasValue)
            payments = payments.Skip(skip.Value);
        if (take.HasValue)
            payments = payments.Take(take.Value);

        var res = await payments.ToListAsync();
        return new PageOf<Payment>(count, res);
    }
}