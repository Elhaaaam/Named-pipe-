using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.IO;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            run_server();
            Console.ReadLine();   
        }
        static void run_server()
        {
            // Open the named pipe.
            var server = new NamedPipeServerStream("NamedPipedtest");

            Console.WriteLine("Waiting for connection...");
            server.WaitForConnection();

            Console.WriteLine("Connected.");
            var br = new BinaryReader(server);
            var bw = new BinaryWriter(server);

            while (true)
            {
                try
                {
                    var len = (int)br.ReadUInt32();            // Read string length
                    var str = new string(br.ReadChars(len));    // Read string
                    double[] RecAr = str2ar(str);
                    Console.WriteLine("-------------------------------------------------------");
                    Console.Write("Recieved Array: ");
                    printAr(RecAr);
                    

                    double[] SendAr = sampleModification(RecAr);
                    str = ar2str(SendAr);
                    var buf = Encoding.ASCII.GetBytes(str);     // Get ASCII byte array     
                    bw.Write((uint)buf.Length);                // Write string length
                    bw.Write(buf);                              // Write string
                    Console.Write("Sent Array: ");
                    printAr(SendAr);
                    Console.WriteLine("-------------------------------------------------------");

                }
                catch (EndOfStreamException)
                {
                    break;                    // When client disconnects
                }
            }

            Console.WriteLine("Client disconnected.");
            server.Close();
            server.Dispose();
        }
        static double[] str2ar(string str)
        {
            string[] s=str.Split(';');

            double[] res = new double[(int)double.Parse(s[0])];
            int idx = 1;
            for (int i=0;i<res.Length;i++)
                res[i] = double.Parse(s[idx++]);
            return res;
        }
        static string ar2str(double[] ar)
        {
            string str = ""+ar.Length+";";
            for (int i = 0; i < ar.Length; i++)
                str += ar[i] + ";";
            return str;
        }
        static void printAr(double[] ar)
        {
            for (int i = 0; i < ar.Length; i++)
                Console.Write(ar[i] + " ");
            Console.WriteLine();
        }
        static double[] sampleModification(double[] ar)
        {
            double[] res=new double[ar.Length];
            for (int i = 0; i < ar.Length; i++)
            {
                res[i]= -ar[i];
            }
            return res;
        }
    }
}