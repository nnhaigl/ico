using System.Collections.Generic;
using Newtonsoft.Json;

namespace ICOCore.Entities.Extra
{
    public class GoogleCaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}
