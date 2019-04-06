using Newtonsoft.Json;

namespace TESTWEB.Models
{
    public class BraintreeRequest
    {
        [JsonProperty(PropertyName = "nonce", Required = Required.Always)]
        public string Nonce { set; get; }
        [JsonProperty(PropertyName = "amount", Required = Required.Always)]
        public decimal Amount { set; get; }
    }
}