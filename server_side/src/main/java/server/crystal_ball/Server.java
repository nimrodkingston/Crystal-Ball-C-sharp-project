package server.crystal_ball;
import java.net.*;
import java.io.*;
import java.lang.ProcessBuilder;
import org.json.JSONObject;

public class Server {
    public static void main(String[] args){
        try{
            String recievedMessage = "";
            String responseBehaviour = "Respond to the following prompt dramatically as if you are a mysterious fortune teller infront of a crystal ball.If the word you is used the prompt is referring to the fortune teller.If the prompt asks you for a joke act as if your time is being wasted and be snarky. Keep the response under 20 words:";
            ServerSocket serverConnection = new ServerSocket(70);
                System.out.println("Server socket established sucessfully");

            while (recievedMessage != "exit"){
                
                Socket clientConnection = serverConnection.accept();
                System.out.println("Client side connection succesful");

                InputStream inputStream = clientConnection.getInputStream();
                byte[] buffer = new byte[1024];
                int length = inputStream.read(buffer);
                recievedMessage = responseBehaviour + new String(buffer,0,length);
                String command = "curl https://api.openai.com/v1/completions -H \"Content-Type: application/json\" -H \"Authorization: Bearer ENTER API KEY HERE\" -d \"{\\\"model\\\": \\\"text-davinci-003\\\", \\\"prompt\\\": \\\"" + recievedMessage + "\\\", \\\"max_tokens\\\": 30, \\\"temperature\\\": 0}";
                ProcessBuilder commandBuilder = new ProcessBuilder(command.split(" "));
                
                try{
                    Process commandProcess = commandBuilder.start();
                    InputStream apiResponse = commandProcess.getInputStream();
                    BufferedReader inputReader = new BufferedReader(new InputStreamReader(apiResponse));
                    JSONObject json = new JSONObject(inputReader.readLine());
                    String response = new JSONObject(json.getJSONArray("choices").get(0).toString()).getString("text").trim();
                    byte[] data = response.getBytes();
                    OutputStream socketOutput = clientConnection.getOutputStream();
                    socketOutput.write(data);
                }

                catch(Exception e){
                    System.out.println("There was a problem with recieving the JSON from the openai api");
                    System.out.println(e);
                }

                clientConnection.close();
                System.out.println("Client connection closed");
            }

            System.out.println("Server connection has been closed");
            
        }
        catch (Exception e){
            System.out.println("There was an error establishing an active socket");
            System.out.println(e);
        }
    }
}