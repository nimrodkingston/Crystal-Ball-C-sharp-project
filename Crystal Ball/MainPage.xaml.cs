namespace Crystal_Ball;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

public partial class MainPage : ContentPage
{
    //FEATURE NOTE: I should add a system to tell if the user asks a question or asks for an image and I can use an analysis of the inputted text via API calls to do this, meaning that the output type can be entirely worked out within the get response method
    //FEATURE NOTE: I can also make it so that the image is savable to the device if the user chooses. To do this I will have to look into memory streams and change the API response to b64, meaning a few other things will have to change too 
    HttpClient client = new HttpClient();
    Boolean outputType = true;
    String responseBehaviour = "Respond to the following prompt dramatically as if you are a mysterious fortune teller infront of a crystal ball.If the word you is used the prompt is referring to the fortune teller.If the prompt asks you for a joke act as if your time is being wasted and be snarky. Keep the response under 100 words.";
    public MainPage()
	{
		InitializeComponent();
    }
    async Task<String> GetResponse(String prompt, HttpClient client,Boolean outputType)
    {

        using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.openai.com/v1/" + (outputType ? "completions" : "images/generations")))
        {

            request.Headers.TryAddWithoutValidation("Authorization", "Bearer ENTER OPENAI KEY HERE");
            request.Content = new StringContent(outputType ? "{\"model\": \"text-davinci-003\",\"prompt\":" + prompt + ",\"temperature\":0,\"max_tokens\":1000}" : "{\"prompt\":" + prompt + ",\"size\":\"512x512\"}");
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            using (var response = await client.SendAsync(request))
            {
                using (HttpContent content = response.Content)
                {
                    dynamic output = JObject.Parse(content.ReadAsStringAsync().GetAwaiter().GetResult()); //This part will probably have to change as the JSON output is different between the two different cURL requests
                    String answer = outputType ? output.choices[0].text : output.data[0].url; // I can put this straight into the return later but I am leaving it like this for breakpoint reasons
                    OutputLabel.Text = answer;
                    return (answer);
                }
            }
        }
    }
    private async void onTextCompleted(object sender, EventArgs e) 
    {
        String input = outputType ? "\"" + responseBehaviour + TextInput.Text + "\"" : "\"" + TextInput.Text + "\"";
        OutputLabel.Text = "The haze within the crystal ball swirls...";
        String response = await GetResponse(input, client, outputType);
        OutputLabel.Text = response;
    }
    private void changeOutput(object sender, EventArgs args)
    {
        outputType = !outputType;
        OutputTypeButton.Text = outputType ? "Request a vision from the crystal ball" : "Ask a question to the cystal ball";
     
    }
    
}

