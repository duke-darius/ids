using Iceni.Lib.EfModels;
using Iceni.Lib.Models.Api;
using Iceni.Lib.Models.Dto;
using Iceni.Lib.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Iceni.Api.Services;

/// <summary>
///     Service for managing pupils
/// </summary>
public class PupilService
{
    private readonly IDbContextFactory<IceniCtx> _contextFactory;
    private readonly CurrentUserService _currentUserService;

    /// <summary>
    ///     ctr
    /// </summary>
    /// <param name="contextFactory"></param>
    /// <param name="currentUserService"></param>
    public PupilService(IDbContextFactory<IceniCtx> contextFactory, CurrentUserService currentUserService)
    {
        _contextFactory = contextFactory;
        _currentUserService = currentUserService;
    }

    /// <summary>
    ///     Queries a list of pupils with some optional parameters
    /// </summary>
    /// <param name="query"></param>
    /// <param name="type"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    public async Task<PageOf<Pupil>> QueryPupils(string? query, PupilType? type, int? skip, int? take)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();

        var q = ctx.Pupils
            .Include(x=> x.Payments)
            .Include(x=> x.Lessons).AsQueryable();
        
        q = q.Where(x => x.TutorId == _currentUserService.GetCurrentUsersId());
        if (!string.IsNullOrEmpty(query))
        {
            q = q.Where(x =>
                x.FullName.Contains(query)
                ||
                x.EmailAddress.Contains(query));
        }

        if (type != null)
        {
            q = q.Where(x => x.Type == type);
        }

        var count = await q.CountAsync();

        if (skip.HasValue)
            q = q.Skip(skip.Value);
        if (take.HasValue)
            q = q.Take(take.Value);

        var res = await q.ToListAsync();
        return new PageOf<Pupil>(count, res);
    }

    /// <summary>
    ///     Gets a pupil by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Pupil> GetPupil(Guid id)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Pupils
            .Include(x=> x.Payments)
            .Include(x=> x.Lessons)
            .SingleAsync(x => x.Id == id);
    }

    /// <summary>
    ///     Creates a new blank pupil
    /// </summary>
    /// <returns></returns>
    public async Task<Pupil> CreateNewPupil()
    {
        var pupil = new Pupil()
        {
            FullName = "New pupil",
            EmailAddress = "changeme@email.com",
            TutorId = _currentUserService.GetCurrentUsersId(),
            DateCreated = DateTime.UtcNow
        };

        await using var ctx = await _contextFactory.CreateDbContextAsync();

        ctx.Pupils.Add(pupil);
        await ctx.SaveChangesAsync();

        return pupil;
    }

    /// <summary>
    ///     Updates a pupil
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    public async Task<Pupil> UpdatePupil(PupilDto update)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        var pupil = await ctx.Pupils
            .Include(x=> x.Payments)
            .Include(x=> x.Lessons)
            .SingleAsync(x => x.Id == update.Id);

        pupil.Type = update.Type;
        pupil.EmailAddress = update.EmailAddress;
        pupil.FullName = update.FullName;
        pupil.AddressLine1 = update.AddressLine1;
        pupil.AddressLine2 = update.AddressLine2;
        pupil.AddressLine3 = update.AddressLine3;
        pupil.Postcode = update.Postcode;
        pupil.City = update.City;

        pupil.Telephone = update.Telephone;
        pupil.AltTelephone = update.AltTelephone;
        

        await ctx.SaveChangesAsync();
        return pupil;
    }

    /// <summary>
    ///     Deletes a pupil by id
    /// </summary>
    /// <param name="pupilId"></param>
    public async Task DeletePupil(Guid pupilId)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        var pupil = await ctx.Pupils.SingleAsync(x => x.Id == pupilId);

        ctx.Pupils.Remove(pupil);
        await ctx.SaveChangesAsync();
    }
}