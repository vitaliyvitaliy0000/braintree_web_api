using Newtonsoft.Json;
using System.Collections.Generic;

namespace TESTWEB.Models
{
    public class BraintreeResponse : ApiResponse
    {
        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }

        [JsonProperty("transaction_error")]
        public BraintreeErrorProcessingTransaction TransactionError { get; set; }

        [JsonProperty("validation_errors")]
        public List<BraintreeValidationError> ValidationErrors { get; set; }
    }
}