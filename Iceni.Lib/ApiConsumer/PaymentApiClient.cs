using Iceni.Lib.EfModels;
using Iceni.Lib.Models.Api;
using Iceni.Lib.Models.Dto;
using Iceni.Lib.Models.Enums;
using RestSharp;

namespace Iceni.Lib.ApiConsumer;

/// <summary>
///     Client for accessing the Payment api endpoints
/// </summary>
public class PaymentApiClient : BaseApi
{
    /// <summary>
    ///     ctr
    /// </summary>
    /// <param name="client"></param>
    /// <param name="api"></param>
    public PaymentApiClient(RestClient client, IceniApiClient api) : base(client, api)
    {
    }


    /// <summary>
    ///     Adds a payment
    /// </summary>
    /// <param name="pupilId"></param>
    /// <param name="amount"></param>
    /// <param name="hours"></param>
    /// <param name="paymentType"></param>
    /// <returns></returns>
    public async Task<ApiResponse<PaymentDto>> AddPayment(Guid pupilId, decimal amount, double hours, PaymentType paymentType)
    {
        return await Execute<PaymentDto>($"/api/payment/{pupilId}", Method.Post, req =>
        {
            req.AddParameter("amount", amount);
            req.AddParameter("hours", hours);
            req.AddParameter("paymentType", (int)paymentType);
        });
    }

    /// <summary>
    ///     Deletes a payment by payment id
    /// </summary>
    /// <param name="paymentId"></param>
    /// <returns></returns>
    /// <exception cref="ApiException"></exception>
    public async Task<Messages.RestMessage> DeletePayment(Guid paymentId)
    {
        var req = NewReq($"/api/payment/{paymentId}", Method.Delete);
        return await Execute($"/api/payment/{paymentId}", Method.Delete, _ => { });
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="paymentType"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <param name="pupilId"></param>
    /// <returns></returns>
    public async Task<ApiResponse<PageOf<PaymentDto>>> QueryPayments(Guid? pupilId = null, PaymentType? paymentType = null, int? skip = null,
        int? take = null)
    {
        return await Execute<PageOf<PaymentDto>>($"/api/payment", Method.Get, req =>
        {
            if (pupilId.HasValue)
                req.AddQueryParameter("pupilId", pupilId.Value);
            if (paymentType.HasValue)
                req.AddQueryParameter("paymentType", (int)paymentType.Value);
            if (skip.HasValue)
                req.AddQueryParameter("skip", skip.Value);
            if (take.HasValue)
                req.AddQueryParameter("take", take.Value);
        });
    }
}