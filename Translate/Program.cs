using System.Text;
using Newtonsoft.Json;

class Program
{
    private static readonly string key = "5d819f81ee5a41c2aab2d9f0495b64e1";
    private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com/";
    private static readonly string location = "westeurope";

    public class DetectedLanguage
    {
        public string language { get; set; }
        public double score { get; set; }
    }

    public class Translation
    {
        public string text { get; set; }
        public string to { get; set; }
    }

    public class TranslationResponse
    {
        public DetectedLanguage detectedLanguage { get; set; }
        public List<Translation> translations { get; set; }
    }

    static async Task Main(string[] args)
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;
        
        Console.Write("Enter your Text: ");
        string textToTranslate =  Console.ReadLine();
        
        Console.Write("Enter the language you want to translate into example(English): ");
        string language =  Console.ReadLine();
        string toLanguage = language.Substring(0, 2).ToLower();
        
        string route = $"/translate?api-version=3.0&to={toLanguage}";
        object[] body = new object[] { new { Text = textToTranslate } };
        var requestBody = JsonConvert.SerializeObject(body);

        using (var client = new HttpClient())
        using (var request = new HttpRequestMessage())
        {
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(endpoint + route);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            request.Headers.Add("Ocp-Apim-Subscription-Key", key);
            request.Headers.Add("Ocp-Apim-Subscription-Region", location);

            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            string json = await response.Content.ReadAsStringAsync();
            
            List<TranslationResponse> translationResponses = JsonConvert.DeserializeObject<List<TranslationResponse>>(json);
            if (translationResponses.Count > 0)
            {
                string translatedText = translationResponses[0].translations[0].text;
                Console.WriteLine("Result: " + translatedText);
                Console.ReadLine();
            }
        }
    }
}