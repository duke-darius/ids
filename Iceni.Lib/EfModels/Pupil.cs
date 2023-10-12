using System.ComponentModel.DataAnnotations;
using Iceni.Lib.Models.Enums;

namespace Iceni.Lib.EfModels;

/// <summary>
///     A pupils information
/// </summary>
public class Pupil
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
    ///     The User (Tutor) assigned to this pupil
    /// </summary>
    public virtual IceniUser Tutor { get; set; } = null!;
    
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
    ///     List of lessons assigned to the pupil
    /// </summary>
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    /// <summary>
    ///     List of payments
    /// </summary>
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    /// <summary>
    ///     Total hours purchased by the pupil
    /// </summary>
    public double TotalHours => Payments.Sum(x => x.Hours);

    /// <summary>
    ///     Hours used up by pupil
    /// </summary>
    public double HoursUsed => Lessons.Sum(x => x.Duration.TotalHours);

    /// <summary>
    ///     Hours Due in credit
    /// </summary>
    public double HoursCredit => TotalHours - HoursUsed;



}