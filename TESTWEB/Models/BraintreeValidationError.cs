using Newtonsoft.Json;

namespace TESTWEB.Models
{
    public class BraintreeValidationError
    {
        [JsonProperty("attribute")]
        public string Attribute { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        public BraintreeValidationError(string attribute, string code, string message)
        {
            Attribute = attribute;
            Code = code;
            Message = message;
        }
    }
}