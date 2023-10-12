using Iceni.Lib.EfModels;
using Iceni.Lib.Models.Enums;

namespace Iceni.Lib.Models.Dto;

/// <summary>
///     Dto wrapper for <see cref="Iceni.Lib.EfModels.Payment"/>
/// </summary>
[Serializable]
public class PaymentDto
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
    ///     Pupil paying (ID)
    /// </summary>
    public Guid PupilId { get; set; }
    
    /// <summary>
    ///     Name of pupil
    /// </summary>
    public string? PupilName { get; set; }
    
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
    public decimal AmountPaid { get; set; }
    
    /// <summary>
    ///     Payment type
    /// </summary>
    public PaymentType PaymentType { get; set; }
    
    /// <summary>
    ///     Default ctr
    /// </summary>
    public PaymentDto(){}

    /// <summary>
    ///     Main ctr
    /// </summary>
    /// <param name="payment"></param>
    /// <param name="includePupil"></param>
    public PaymentDto(Payment payment, bool includePupil = false)
    {
        Id = payment.Id;
        DateCreated = payment.DateCreated;
        if (includePupil)
            PupilName = payment.Pupil.FullName;
        PupilId = payment.PupilId;
        TutorId = payment.TutorId;
        Hours = payment.Hours;
        AmountPaid = payment.AmountPaid;
        PaymentType = payment.PaymentType;
    }
}