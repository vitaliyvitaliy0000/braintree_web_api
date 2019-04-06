using Newtonsoft.Json;

namespace TESTWEB.Models
{
    public class BraintreeErrorProcessingTransaction
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }

        public BraintreeErrorProcessingTransaction(string status, string code, string text)
        {
            Status = status;
            Code = code;
            Text = text;
        }
    }
}