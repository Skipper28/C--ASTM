using System;
using System.Threading;



    

class Program
{
    static void Main(string[] args)
    {
        Thread t = new Thread(delegate ()
        {
            // replace the IP with your system IP Address...
            Server myserver = new Server("192.168.1.58", 10124);
        });
        t.Start();
        
        Console.WriteLine("Server Started...!");
    }
}
