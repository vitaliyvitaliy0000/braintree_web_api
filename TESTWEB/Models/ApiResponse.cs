using Newtonsoft.Json;

namespace TESTWEB.Models
{
    public class ApiResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        public ApiResponse()
        {
            Success = true;
        }
    }
}