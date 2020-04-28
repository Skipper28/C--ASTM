using System;
using System.Threading;



    

class Program
{
               // Parser parser = new Parser();

    static void Main(string[] args)
    {
            Console.WriteLine("TEST");        

         Server myserver = new Server("127.0.0.1", 10124);

            Console.WriteLine("Zum Parsen: "+myserver.getASTMMessage());
            Console.WriteLine("TEST");        
/**
        Thread t = new Thread(delegate ()
        {
           
        });
        t.Start();
        
 */   
        
        Console.WriteLine("Server Started...!");
    }
}
