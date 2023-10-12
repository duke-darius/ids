using System.ComponentModel;

namespace Iceni.Lib.Models.Enums;

/// <summary>
///     Types of pupil
/// </summary>
public enum PupilType
{
    /// <summary>
    ///     Pupil is not currently complete 
    /// </summary>
    [Description("Draft pupil")]
    Draft,
    /// <summary>
    ///     Pupil is active
    /// </summary>
    [Description("Active")]
    Active,
    /// <summary>
    ///     Pupil has left
    /// </summary>
    [Description("Lost pupil")]
    Lost,
    /// <summary>
    ///     Pupil has passed their test
    /// </summary>
    [Description("Passed pupil")]
    Passed,
    /// <summary>
    ///     Pupil has passed and reviewed on TrustPilot
    /// </summary>
    [Description("Passed and reviewed")]
    PassedAndReviewed,
    /// <summary>
    ///     Pupil is waiting for their test
    /// </summary>
    [Description("Waiting for test")]
    Waiting
}