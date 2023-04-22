package com.example;

import java.net.*;
import java.io.*;
import java.lang.ProcessBuilder;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;

public class Server {
    //The accept method for the server socket will only accept one connection at a time so I should look into using threads to manage multiple connections
    // The api call now works but escaping escape characters is really messy looking- maybe look into using okttp now that maven is working
    public static void main(String[] args){
        try{
            String recievedMessage = "";
            ServerSocket serverConnection = new ServerSocket(70);
                System.out.println("Server socket established sucessfully");

            while (recievedMessage != "exit"){
                
                Socket clientConnection = serverConnection.accept();
                System.out.println("Client side connection succesful");

                InputStream inputStream = clientConnection.getInputStream();
                byte[] buffer = new byte[1024];
                int length = inputStream.read(buffer);
                recievedMessage = new String(buffer,0,length);
                System.out.println(recievedMessage);

                FileWriter newJson = new FileWriter("D:/Server-Client/demo/src/main/java/com/example/text.json");
                String command = "curl https://api.openai.com/v1/completions -H \"Content-Type: application/json\" -H \"Authorization: Bearer ENTER OPENAI KEY HERE\" -d \"{\\\"model\\\": \\\"text-davinci-003\\\", \\\"prompt\\\": \\\"" + recievedMessage + "\\\", \\\"max_tokens\\\": 25, \\\"temperature\\\": 0}";
                System.out.println(command);
                ProcessBuilder commandBuilder = new ProcessBuilder(command.split(" "));
                try{
                    Process commandProcess = commandBuilder.start();
                    commandProcess.waitFor();
                    InputStream apiResponse = commandProcess.getInputStream();
                    BufferedReader inputReader = new BufferedReader(new InputStreamReader(apiResponse));
                    String line;
                    while((line = inputReader.readLine())!=null){
                        System.out.println(line);
                    }
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
