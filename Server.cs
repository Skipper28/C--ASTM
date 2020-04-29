using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
class Server
{

    // const Byte ACK = Encoding.ASCII(6);
    const int ENQ = 5;
    const int STX = 2;
    const int ETX = 3;
    const int LF = 10;
    const int CR = 13;
    const int EOT = 4;

    TcpListener server = null;
    String ASTM_Message;
    public Server(string ip, int port)
    {
        IPAddress localAddr = IPAddress.Parse(ip);
        this.server = new TcpListener(localAddr, port);

        this.server.Start();
                      

 //StartListener();
    

    }
    public void StartListener()
    {

        try
        {
            while (true)
            {
             //  Console.WriteLine("Waiting for a connection...");
           
    
            //getASTMStream();
             //   Console.WriteLine("ASTM_Message: "+ASTM_Message);
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
            server.Stop();
        }
    }

    public String getASTMStream()
    {

         TcpClient client = server.AcceptTcpClient();
          
        var stream = client.GetStream();

        Boolean transmisson = false;
        Boolean checksum_block = false;
        Boolean message_block = false;
        //   Byte ACK = Encoding.ASCII.GetString(6,0,bytes.Length);

        BinaryWriter writer = new BinaryWriter(client.GetStream(), Encoding.ASCII, true);
        ASCIIEncoding ascii = new ASCIIEncoding();
        Byte[] ACK = ascii.GetBytes("\06");


        string data = null;

        Byte[] bytes = new byte[1];
        int i;
        StringBuilder message = new StringBuilder();
        StringBuilder checksum = new StringBuilder();

 
        try
        {

            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = Encoding.ASCII.GetString(bytes, 0, i);
              //  Console.WriteLine(data.ToCharArray()[0]);
                if (data.ToCharArray()[0] == ENQ)
                {
                    writer.Write(ACK);
                    transmisson = true;
           Console.WriteLine("Übertragung startet!");

                }
                else if (data.ToCharArray()[0] == EOT)
                {

                    transmisson = false;
                  Console.WriteLine("Übertragung beendet!"+message);
                    return message.ToString();

                }

                if (transmisson == true && data.ToCharArray()[0] == STX)
                {
    
                    message_block = true;
                 Console.WriteLine("Daten kommen!");
                }

                if (data.ToCharArray()[0] == ETX)
                {
                    message_block = false;
                    checksum_block = true;


                    Console.WriteLine("Ende der Nachricht!");
                }


                if (message_block == true && data.ToCharArray()[0] == CR)
                {


                }
                else if (checksum_block == true && data.ToCharArray()[0] == CR)
                {

                    checksum_block = false;

                }

                if( checksum_block == false && data.ToCharArray()[0] == CR) {

                        message.Append(data.ToCharArray()[0]);

                }

                if( checksum_block == false && data.ToCharArray()[0] == LF) {

                        message.Append(data.ToCharArray()[0]);

                }

                if (message_block == true)
                {
                    message.Append(data.ToCharArray()[0]);

                }
                else if (checksum_block == true)
                {

                    checksum.Append(data.ToCharArray()[0]);

                }
            

            }
     

        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e.ToString());
            client.Close();
        }

            

        return message.ToString();

    }

}
