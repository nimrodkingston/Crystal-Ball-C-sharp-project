namespace Crystal_Ball;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

public partial class MainPage : ContentPage
{
    HttpClient client = new HttpClient();
    String responseBehaviour = "Respond to the following prompt dramatically as if you are a mysterious fortune teller infront of a crystal ball.If the word you is used the prompt is referring to the fortune teller.If the prompt asks you for a joke act as if your time is being wasted and be snarky. Keep the response under 100 words.";

    public MainPage()
	{
		InitializeComponent();
    }
    async Task<String> GetResponse(String prompt, HttpClient client)
    {
        using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.openai.com/v1/completions"))
        {
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer ENTER-OPENAI-KEY-HERE");
            request.Content = new StringContent("{\"model\": \"text-davinci-003\",\"prompt\":" + prompt + ",\"temperature\":0,\"max_tokens\":1000}");
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            using (var response = await client.SendAsync(request))
            {
                using (HttpContent content = response.Content)
                {
                    dynamic data = JObject.Parse(content.ReadAsStringAsync().GetAwaiter().GetResult());
                    return (data.choices[0].text);
                }
            }
        }
    }
    private async void onTextCompleted(object sender, EventArgs e) 
    {
        String input = "\"" + responseBehaviour + TextInput.Text + "\"";
        String response = await GetResponse(input, client);
        OutputLabel.Text = response;
    }
}

