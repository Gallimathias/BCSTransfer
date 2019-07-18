using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BCSTransfer.Core.PretixModel
{
    public class Order
    {
        public string Code { get; set; }
        public string Status { get; set; }
        public bool Testmode { get; set; }
        public string Secret { get; set; }
        public string Email { get; set; }
        public string Locale { get; set; }
        [JsonProperty(PropertyName = "sales_channel")]
        public string SalesChannel { get; set; }
        public DateTime? Datetime { get; set; }
        public DateTime? Expires { get; set; }
        [JsonProperty(PropertyName = "last_modified")]
        public DateTime? LastModified { get; set; }
        [JsonProperty(PropertyName = "payment_date")]
        public string PaymentDate { get; set; }
        [JsonProperty(PropertyName = "payment_provider")]
        public string PaymentProvider { get; set; }
        public object[] Fees { get; set; }
        public string Total { get; set; }
        public string Comment { get; set; }
        [JsonProperty(PropertyName = "checkin_attention")]
        public bool CheckinAttention { get; set; }
        [JsonProperty(PropertyName = "require_approval")]
        public bool RequireApproval { get; set; }
        [JsonProperty(PropertyName = "invoice_address")]
        public InvoiceAddress InvoiceAddress { get; set; }
        public Position[] Positions { get; set; }
        public Download[] Downloads { get; set; }
        public Payment[] Payments { get; set; }
        public object[] Refunds { get; set; }
    }
}
