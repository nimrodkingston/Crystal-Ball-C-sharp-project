package server.crystal_ball;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.fail;

public class UnitTests {
    
    @Test
    void testServerConnection(){
      // This tests to see if a server connection can be made
    
      try{
        ServerConnection server = new ServerConnection();
      }

      catch(Exception e){
        fail("Exception thrown, failed to create serverConnection");
      }
    }

    @Test
    void testCloseConnection(){
      // This tests to see if a server connection can be made and then closed if the socket is not currently accepting client connections
      try{
        ServerConnection server = new ServerConnection();
        server.closeConnection();
      }

      catch(Exception e){
        fail("Exception thrown, the server connection could not be closed");
      }
    }
}
