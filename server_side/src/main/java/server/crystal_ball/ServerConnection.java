package server.crystal_ball;
import java.net.*;

public class ServerConnection { 
    // This class holds the server connection details so that it can be used in the static main method in the Server class
    ServerSocket serverConnection;

    public ServerConnection(){
        // This constructor creates a new Server connection and will throw an exception if the initial server connection cannot be made
        try{
            this.serverConnection = new ServerSocket(70);
            System.out.println("Server socket established sucessfully");
        }

        catch(Exception e){
            System.out.println("There was a problem creating the initial server connection");
            System.out.println(e);
        }
    }

    public void closeConnection(){
        // This method closes the server connection to free up resources
        try{
        this.serverConnection.close();
        }
        catch(Exception e){
            System.out.println("There was an error trying to close the server connection, it may still be trying to accept connections");
            System.out.println(e);
        }
    }
}
