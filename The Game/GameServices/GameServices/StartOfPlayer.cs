using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace GameMaster
{
    class Program
    {
        static class Pl
        {
            public static Player p = new Player(new Random().Next(), new Team(), false);
        }

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Player");
            string start = "";
            Console.Write("Please type Ip address of server\n# ");
            string ip = Console.ReadLine();
            while (!ValidateIP(ip))
            {
                Console.Write("Please type Ip address of server\n# ");
                ip = Console.ReadLine();
            }
            Console.Write("Please type port number of server\n# ");
            string port = Console.ReadLine();
            while (!ValidatePort(port))
            {
                Console.Write("Please type port number of server\n# ");
                port = Console.ReadLine();
            }
            while (start != "start")
            {
                Console.Write("type \"start\" to connect to the game\n# ");
                start = Console.ReadLine();
            }
            Console.WriteLine("I'm playing");

            Pl.p.Start(ip,port);
            string testString = "";
            while (testString != "exit")
            {
                Console.Write("type \"exit\" to shutdown or close console\n# ");
                testString = Console.ReadLine();
            }
        }

        private static bool ValidateIP(string ip)
        {
            try
            {
                IPAddress.Parse(ip);
                return true;
            }
            catch (ArgumentNullException)
            {
                Console.Write("Exception: IP is null.\n");
                return false;
            }
            catch (FormatException)
            {
                Console.Write("Exception: Wrong IP format.\n");
                return false;
            }
        }

        private static bool ValidatePort(string port)
        {
            try
            {
                int valPort = Convert.ToInt32(port);
                return valPort > 0;
            }
            catch (FormatException)
            {
                Console.Write("Exception: Wrong port format.\n");
                return false;
            }
            catch (OverflowException)
            {
                Console.Write("Exception: Overflow.\n");
                return false;
            }
        }
    }
}
