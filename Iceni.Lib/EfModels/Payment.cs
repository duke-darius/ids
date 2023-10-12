using System.ComponentModel.DataAnnotations.Schema;
using Iceni.Lib.Models.Enums;

namespace Iceni.Lib.EfModels;

/// <summary>
///     Record of payment by pupil to tutor
/// </summary>
public class Payment
{
    /// <summary>
    ///     Unique payment id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    ///     Date payment was sent
    /// </summary>
    public DateTime DateCreated { get; set; }

    /// <summary>
    ///     Pupil paying
    /// </summary>
    public Pupil Pupil { get; set; } = null!;
    
    /// <summary>
    ///     Pupil paying (ID)
    /// </summary>
    public Guid PupilId { get; set; }

    /// <summary>
    ///     Tutor 
    /// </summary>
    public IceniUser Tutor { get; set; } = null!;
    
    /// <summary>
    ///     Tutor Id
    /// </summary>
    public Guid TutorId { get; set; }
    
    /// <summary>
    ///     Hours bought via payment
    /// </summary>
    public double Hours { get; set; }
    
    /// <summary>
    ///     Money paid
    /// </summary>
    [Column(TypeName="money")]
    public decimal AmountPaid { get; set; }
    
    /// <summary>
    ///     Payment type
    /// </summary>
    public PaymentType PaymentType { get; set; }
}