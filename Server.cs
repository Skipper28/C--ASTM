using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
class Server
{

    // const Byte ACK = 6;
    const int ENQ = 5;
    const int STX = 2;
    const int ETX = 3;
    const int LF = 10;
    const int CR = 13;
    const int EOT = 4;

    TcpListener server = null;
    public Server(string ip, int port)
    {
        IPAddress localAddr = IPAddress.Parse(ip);
        server = new TcpListener(localAddr, port);
        server.Start();
        StartListener();

    }
    public void StartListener()
    {
        try
        {
            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected!");
                //    Thread t = new Thread(new ParameterizedThreadStart(HandleDeivce));
                //    Thread t = new Thread(new ParameterizedThreadStart(getASTMStream));

                //   t.Start(client);

                getASTMStream(client);
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
            server.Stop();
        }
    }


    public String getASTMStream(TcpClient client)
    {
        var stream = client.GetStream();
        Boolean hasStarted = false;
        Boolean hasEnded = false ;
        Boolean transmisson = false;

        BinaryWriter writer = new BinaryWriter(client.GetStream(), Encoding.ASCII, true);

        Byte[] ACK = new byte[6];



        string data = null;
        Byte[] bytes = new Byte[1];
        int i;

        try
        {

            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine(data.ToCharArray()[0]);
                //Console.WriteLine(data);


                if (data.ToCharArray()[0] == ENQ)
                {
                    writer.Write(ACK);
                    transmisson = true;
                    Console.WriteLine("Übertragung startet!");

                }
                else if (data.ToCharArray()[0] == EOT)
                {

                    transmisson = false;
                    Console.WriteLine("Übertragung beendet!");
                    return data;

                }

                    if (transmisson == true && data.ToCharArray()[0] == STX ) {
                        hasStarted = true;
                        hasEnded = false;
                        Console.WriteLine("Daten kommen!");
                    }

                      if (data.ToCharArray()[0] == ETX ) {
                        hasEnded = true;

                        Console.WriteLine("Ende der Nachricht!");
                    }
                
                
                      if (hasEnded == false && data.ToCharArray()[0] == CR ) {
                        Console.WriteLine("Jetzt kommt gleich Checksumme!");


                    } else if (hasEnded == true && data.ToCharArray()[0] != CR) {

                            Console.WriteLine("Checksumme!");
                    }

            }
            /*

                            if (data.ToCharArray()[0] == ENQ)
                            {
                                writer.Write(ACK);

                            }
                            else if (data.ToCharArray()[0] == EOT)
                            {


                            }
                            else
                            {
                                if (data.ToCharArray()[0] == STX)
                                {
                                    hasStarted = false;
                                    hasEnded = false;

                                    hasStarted = true;


                                }


                                else if (data.ToCharArray()[0] == ETX)
                                {
                                    //TODO PARSER ERROR

                                hasEnded = true;

                            } else if (data.ToCharArray()[0] == LF)
                            {

                            }


                            }
                        }

            */



        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e.ToString());
            client.Close();
        }


        return null;
    }

    public void HandleDeivce(Object obj)
    {
        TcpClient client = (TcpClient)obj;
        var stream = client.GetStream();
        string imei = String.Empty;
        string data = null;
        Byte[] bytes = new Byte[256];
        int i;
        try
        {
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                string hex = BitConverter.ToString(bytes);
                data = Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine(data.ToCharArray()[0]);


                BinaryWriter writer = new BinaryWriter(client.GetStream(), Encoding.ASCII, true);
                if (data.ToCharArray()[0] == ENQ)
                {
                    writer.Write(new Byte[6]);

                }
                else if (data.ToCharArray()[0] == EOT)
                {

                }


                if (data.ToCharArray()[0] == STX)
                {

                    Console.WriteLine("true");
                    writer.Write("<STX>");

                }

                Console.WriteLine("{1}: Received: {0}", data, Thread.CurrentThread.ManagedThreadId);
                string str = "Hey Device!";
                Byte[] reply = System.Text.Encoding.ASCII.GetBytes(str);

                //stream.Write(reply, 0, reply.Length);
                Console.WriteLine("{1}: Sent: {0}", str, Thread.CurrentThread.ManagedThreadId);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e.ToString());
            client.Close();
        }
    }
}
