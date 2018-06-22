using Newtonsoft.Json;
using System.Collections.Generic;

namespace YogaFitnessClub.Models
{
    /// <summary>
    /// This is Google Recaptcha model this class is used to validate a user to ensure its not a bot when registering 
    /// This class has success and errors properties to check if the validation was successful or not
    /// </summary>
    public class Recaptcha
    {
        [JsonProperty("success")]
        public string Success { get; set; }

        [JsonProperty("error-codes")]
        private List<string> ErrorCodes { get; set; }

        public static string Validate(string EncodedResponse)
        {
            var client = new System.Net.WebClient();
            const string PrivateKey = "6LcprEMUAAAAAKWm7cfxAfiYLytEm-n4vV41O3hw";
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", PrivateKey, EncodedResponse));
            var captchaResponse = JsonConvert.DeserializeObject<Recaptcha>(reply);
            return captchaResponse.Success;
        }
    }
}