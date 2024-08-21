using AiServiceLabb1;
using Azure;
using Azure.AI.TextAnalytics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AiServiceLabb1AndLabb2.Labb1
{
    public class LanguageService
    {
        private readonly TextAnalyticsClient _textAnalyticsClient;
        private readonly string _translatorEndpoint;
        private readonly string _cognitiveKey;
        private readonly string _cognitiveRegion;

        public LanguageService(ConfigurationSettings settings)
        {
            _textAnalyticsClient = new TextAnalyticsClient(new Uri(settings.CognitiveServicesEndpoint), new AzureKeyCredential(settings.CognitiveServicesKey));
            _translatorEndpoint = "https://api.cognitive.microsofttranslator.com";
            _cognitiveKey = settings.CognitiveServicesKey;
            _cognitiveRegion = settings.CognitiveServicesRegion;
        }
        public async Task<(string TranslatedQuestion, string DetectedLanguage)> DetectAndTranslateAsync(string question)
        {
            // Detect the language of the input text
            var languageResult = _textAnalyticsClient.DetectLanguage(question);
            var detectedLanguage = languageResult.Value.Iso6391Name;

            string translatedQuestion = question;
            if (detectedLanguage != "en")
            {
                translatedQuestion = await TranslateTextAsync(question, detectedLanguage);
            }

            return (translatedQuestion, languageResult.Value.Name);
        }
        private async Task<string> TranslateTextAsync(string text, string sourceLanguage)
        {
            string translation = string.Empty;

            try
            {
                object[] body = new object[] { new { Text = text } };
                var requestBody = JsonConvert.SerializeObject(body);

                using (var client = new HttpClient())
                {
                    using (var request = new HttpRequestMessage())
                    {
                        string path = $"/translate?api-version=3.0&from={sourceLanguage}&to=en";
                        request.Method = HttpMethod.Post;
                        request.RequestUri = new Uri(_translatorEndpoint + path);
                        request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                        request.Headers.Add("Ocp-Apim-Subscription-Key", _cognitiveKey);
                        request.Headers.Add("Ocp-Apim-Subscription-Region", _cognitiveRegion);

                        HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

                        if (response.IsSuccessStatusCode)
                        {
                            // Parse the response to extract the translated text
                            string responseContent = await response.Content.ReadAsStringAsync();
                            JArray jsonResponse = JArray.Parse(responseContent);
                            translation = (string)jsonResponse[0]["translations"][0]["text"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }

            return translation;
        }
    }
}

