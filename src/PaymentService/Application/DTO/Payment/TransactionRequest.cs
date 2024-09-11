using Newtonsoft.Json;

namespace Application.DTO.Payment;

public class VerifyPaymentResponseData
{
    public uint Id { get; set; }
    public string Domain { get; set; }
    public string Status { get; set; }
    public string Gateway_response { get; set; }
    public string Reference { get; set; }
    public int Amount { get; set; }
    public DateTime? Paid_at { get; set; }
    public DateTime Created_at { get; set; }
    public string Currency { get; set; }
    public string Channel { get; set; }
    public string Ip_address { get; set; }
  }
public class VerifyPaymentResponseModel
{
    public string Status { get; set; }
    public string Message { get; set; }
    public VerifyPaymentResponseData Data { get; set; }
}
public class PaystackTransactionResponse
{
    [JsonProperty("authorization_url")]
    public string AuthorizationUrl { get; set; }
    [JsonProperty("access_code")]
    public string AccessCode { get; set; }
    [JsonProperty("reference")]
    public string Reference { get; set; }
}

public class PaymentResponseModel
{
    public string Status { get; set; }
    public string Message { get; set; }
    public PaystackTransactionResponse Data { get; set; }
}

public class TransactionRequest
{
    public decimal Amount { get; set; }
    public string RefrenceNo { get; set; }
    public string Email { get; set; }
    public string CallbackUrl { get; set; }

}