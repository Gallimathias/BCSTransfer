using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class InvoiceAddress
{
    [JsonProperty(PropertyName = "last_modified")]
    public DateTime? LastModified { get; set; }
    [JsonProperty(PropertyName = "is_business")]
    public bool IsBusiness { get; set; }
    public string Company { get; set; }
    public string Name { get; set; }
    [JsonProperty(PropertyName = "name_parts")]
    public Dictionary<string, string> NameParts { get; set; }
    public string Street { get; set; }
    public string Zipcode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    [JsonProperty(PropertyName = "internal_reference")]
    public string InternalReference { get; set; }
    [JsonProperty(PropertyName = "vat_id")]
    public string VatId { get; set; }
    [JsonProperty(PropertyName = "vat_id_validated")]
    public bool VatIdValidated { get; set; }
}
