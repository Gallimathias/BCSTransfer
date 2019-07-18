using Newtonsoft.Json;
using System.Collections.Generic;

public class Position
{
    public int? Id { get; set; }
    public string Order { get; set; }
    public int? Positionid { get; set; }
    public int? Item { get; set; }
    public int? Variation { get; set; }
    public string Price { get; set; }
    [JsonProperty(PropertyName = "attendee_name")]
    public string AttendeeName { get; set; }
    [JsonProperty(PropertyName = "attendee_name_parts")]
    public Dictionary<string, string> AttendeeNameParts { get; set; }
    [JsonProperty(PropertyName = "attendee_email")]
    public string AttendeeEmail { get; set; }
    public int? Voucher { get; set; }
    [JsonProperty(PropertyName = "tax_rate")]
    public string TaxRate { get; set; }
    [JsonProperty(PropertyName = "tax_value")]
    public string TaxValue { get; set; }
    [JsonProperty(PropertyName = "tax_rule")]
    public int? TaxRule { get; set; }
    public string Secret { get; set; }
    [JsonProperty(PropertyName = "addon_to")]
    public int? AddonTo { get; set; }
    public int? Subevent { get; set; }
    [JsonProperty(PropertyName = "pseudonymization_id")]
    public string PseudonymizationId { get; set; }
    public Checkin[] Checkins { get; set; }
    public Answer[] Answers { get; set; }
    public Download[] Downloads { get; set; }
}
