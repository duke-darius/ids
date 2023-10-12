using System.ComponentModel.DataAnnotations;
using Iceni.Lib.EfModels;
using Iceni.Lib.Models.Enums;

namespace Iceni.Lib.Models.Dto;

/// <summary>
///     Data transfer object for Pupil
/// </summary>
[Serializable]
public class PupilDto
{
    /// <summary>
    ///     Auto Generated Id of pupil
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     The User (Tutor)'s Id assigned to the pupil
    /// </summary>
    public Guid TutorId { get; set; }

    /// <summary>
    ///     Date the pupil was created
    /// </summary>
    public DateTime DateCreated { get; set; }

    /// <summary>
    /// The pupils full name
    /// </summary>
    [Required] public string FullName { get; set; } = null!;

    /// <summary>
    ///     The email address of the pupil
    /// </summary>
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = null!;

    /// <summary>
    ///     The pupils type
    /// </summary>
    public PupilType Type { get; set; }

    /// <summary>
    ///     The pupils first line of their address
    /// </summary>
    public string? AddressLine1 { get; set; }
    /// <summary>
    ///     The pupils second line of their address
    /// </summary>
    public string? AddressLine2 { get; set; }
    /// <summary>
    ///     The pupils third line of their address
    /// </summary>
    public string? AddressLine3 { get; set; }
    /// <summary>
    ///     The city the pupil lives in
    /// </summary>
    public string? City { get; set; }
    /// <summary>
    ///     The pupils postcode
    /// </summary>
    public string? Postcode { get; set; }

    /// <summary>
    ///     The pupils mobile number
    /// </summary>
    public string? Telephone { get; set; }
    /// <summary>
    ///     And alternative mobile number if needed
    /// </summary>
    public string? AltTelephone { get; set; }

    /// <summary>
    ///     Total hours purchased by the pupil
    /// </summary>
    public double TotalHours { get; set; }
    
    /// <summary>
    ///     Hours used up by pupil
    /// </summary>
    public double HoursUsed { get; set; }
    
    /// <summary>
    ///     Hours Due in credit
    /// </summary>
    public double HoursCredit { get; set; }

    /// <summary>
    ///     Default ctr
    /// </summary>
    public PupilDto(){}
    
    /// <summary>
    ///     main ctr
    /// </summary>
    /// <param name="pupil"></param>
    public PupilDto(Pupil pupil)
    {
        Id = pupil.Id;
        TutorId = pupil.TutorId;
        DateCreated = pupil.DateCreated;
        FullName = pupil.FullName;
        EmailAddress = pupil.EmailAddress;
        Type = pupil.Type;
        AddressLine1 = pupil.AddressLine1;
        AddressLine2 = pupil.AddressLine2;
        AddressLine3 = pupil.AddressLine3;
        City = pupil.City;
        Postcode = pupil.Postcode;
        Telephone = pupil.Telephone;
        AltTelephone = pupil.AltTelephone;

        TotalHours = pupil.TotalHours;
        HoursUsed = pupil.HoursUsed;
        HoursCredit = pupil.HoursCredit;
    }
}