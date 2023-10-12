using System.ComponentModel;

namespace Iceni.Lib.Models.Enums;

/// <summary>
///     Payment Type enum
/// </summary>
public enum PaymentType
{
    /// <summary>
    ///     Cash payment
    /// </summary>
    [Description("Cash payment")]
    Cash,
    /// <summary>
    ///     Card payment (Stripe)
    /// </summary>
    [Description("Card payment (stripe)")]
    Card,
    /// <summary>
    ///     Paid via a bank transfer
    /// </summary>
    [Description("Bank transfer")]
    BankTransfer,
    /// <summary>
    ///     Paid with ApplePay
    /// </summary>
    [Description("Apple Pay")]
    ApplePay,
    /// <summary>
    ///     Deposit payment
    /// </summary>
    [Description("Deposit")]
    Deposit,
    /// <summary>
    ///     Refund payment
    /// </summary>
    [Description("Refund")]
    Refund
}