using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
async Task<String> GetResponse(String prompt ,HttpClient client)
{
    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.openai.com/v1/completions"))
    {
        request.Headers.TryAddWithoutValidation("Authorization", "Bearer INSERT-API-KEY-HERE");
        request.Content = new StringContent("{\"model\": \"text-davinci-003\",\"prompt\":" + prompt + ",\"temperature\":0,\"max_tokens\":1000}");
        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

        using (var response = await client.SendAsync(request))
        {
            using (HttpContent content = response.Content)
            {
                dynamic data = JObject.Parse(content.ReadAsStringAsync().GetAwaiter().GetResult());
                return(data.choices[0].text);
            }
        }
    }
}

HttpClient client = new HttpClient();
String responseBehaviour = "Respond to the following prompt dramatically as if you are a mysterious fortune teller infront of a crystal ball.If the word you is used the prompt is referring to the fortune teller.If the prompt seems trivial or makes fun of you act as if your time is being wasted. Keep the response under 100 words.";

while (true) {
    Console.WriteLine("Ask the crystal ball whatever the heart desires and it will show you what you are looking for\n");

    String input = "\"" + responseBehaviour + Console.ReadLine() + "\"";
    String response = await GetResponse(input,client);
    Console.WriteLine(response);
}

