using System.Text;
using Application.DTO.Payment;
using Application.PaymentProcessing;
using Application.Util;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infrastructure.PaymentProcessing;

public class PayStackService(ISerializerService _serializerService, IOptions<PaystackSettings> _settings, HttpClient _httpClient) : IPaystackService
{
    private readonly HttpClient _apiClient;

   
    public async Task<PaymentResponseModel> InitializePayment(TransactionRequest model)
    {
        try
        {
            string json =JsonConvert.SerializeObject(new
            {
                amount = model.Amount * 100,
                email = model.Email,
                reference = model.RefrenceNo,
                currency = "NGN",
                //callback_url = model.CallbackUrl,
            });
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_settings.Value.BaseUrl}transaction/initialize");
            request.Headers.Add("Authorization", _settings.Value.APIKey);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _apiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return _serializerService.Deserialize<PaymentResponseModel>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<VerifyPaymentResponseModel> VerifyPayment(string PaymentReference)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_settings.Value.BaseUrl}transaction/verify/{PaymentReference}");
            request.Headers.Add("Authorization", _settings.Value.APIKey);
            var response = await _apiClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return _serializerService.Deserialize<VerifyPaymentResponseModel>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}