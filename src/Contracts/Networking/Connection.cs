namespace PayBolt.Networking;

public class Connection
{
   /// <summary>
   /// Provides the result of a connection attempt.
   /// </summary>
   public  Result Result { get; set; }
   
   
   /// <summary>
   /// Provides the details of a connection error.
   /// </summary>
   public string Error { get; set; }

   public Connection(Result result)
   {
      Result = result;
   }

   public Connection(Result result, string error)
   {
      Result = result;
      Error = error;
   }
   
}