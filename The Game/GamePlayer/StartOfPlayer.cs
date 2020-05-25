using System;
using System.Diagnostics;
using System.IO;
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
            if (ip == "")
                ip = "127.0.0.1";
            Console.Write("Please type port number of server\n# ");
            string port = Console.ReadLine();
            if (port == "")
                port = "13000";
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
    }
}
