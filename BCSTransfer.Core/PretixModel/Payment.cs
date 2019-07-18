using Newtonsoft.Json;
using System;

public class Payment
{
    [JsonProperty(PropertyName = "local_id")]
    public int? LocalId { get; set; }
    public string State { get; set; }
    public string Amount { get; set; }
    public DateTime? Created { get; set; }
    [JsonProperty(PropertyName = "payment_date")]
    public DateTime? PaymentDate { get; set; }
    public string Provider { get; set; }
}
