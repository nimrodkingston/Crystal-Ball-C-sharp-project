namespace Crystal_Ball;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Sockets;

public partial class MainPage : ContentPage
{
    //FEATURE NOTE (1): I should add a system to tell if the user asks a question or asks for an image and I can use an analysis of the inputted text via API calls to do this, meaning that the output type can be entirely worked out within the get response method
    //FEATURE NOTE(2): I can also make it so that the image is savable to the device if the user chooses. To do this I will have to look into memory streams and change the API response to b64, meaning a few other things will have to change too 
    //FEATURE(2) ADDITIONAL: This change can potentially make displaying images faster as they do not have to be downloaded to be displayed by the image object. Will have to compare URL download speed vs b64 decryption speed
    IPEndPoint destination = new(IPAddress.Parse("ENTER SERVER IP HERE"), 70);
    Boolean outputType = true;
    public MainPage()
    {
        InitializeComponent();
        Layout.Children.Remove(OutputImage);
    }
    async Task<String> GetResponse(String prompt, Boolean outputType)
    {
        using (Socket clientSocket = new(destination.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
        {
            await clientSocket.ConnectAsync(destination);
            byte[] data = Encoding.ASCII.GetBytes(prompt);
            clientSocket.Send(data);
            byte[] buffer = new byte[1024];
            clientSocket.Receive(buffer);
            return Encoding.ASCII.GetString(buffer);
        }
    }
    private async void onTextCompleted(object sender, EventArgs e)
    {
        clearScreen();
        ProgressLabel.Text = "The haze within the crystal ball swirls...";
        String input = TextInput.Text;
        String response = await GetResponse(input, outputType);

        if (outputType)
        {
            ProgressLabel.Text = "A message appears within the ball. It says:";
            Layout.Children.Add(OutputLabel);
            OutputLabel.Text = "\"" + response.Replace("\n", "") + "\"";
        }

        else
        {
            ProgressLabel.Text = "An image appears from within the ball. A sign perhaps?";
            Layout.Children.Add(OutputImage);
            OutputImage.Source = response;
        }

    }
    private void changeOutput(object sender, EventArgs args)
    {
        outputType = !outputType;
        OutputTypeButton.Text = outputType ? "Request a vision from the crystal ball" : "Ask a question to the cystal ball";

    }

    private void clearScreen() // This function is designed to clear the output elements on screen for a new input
    {
        if (Layout.Children.Contains(OutputLabel)) Layout.Children.Remove(OutputLabel);

        if (Layout.Children.Contains(OutputImage))
        {
            OutputImage.Source = "place_holder.png";
            Layout.Children.Remove(OutputImage); 
        }
    }
}
    


