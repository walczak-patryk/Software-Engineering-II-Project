using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace GameMaster
{
    class Program
    {
        static class GM
        {
            public static GameMaster gm = new GameMaster();
        }

        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            Console.WriteLine("Start Game");
            GM.gm.StartGame(); 
            string testString="";
            Console.WriteLine("Test");
            while (testString != "exit")
            {  
                Console.Write("type \"exit\" to shutdown or close console\n# ");
                testString = Console.ReadLine();
                if (testString.Contains("msg"))
                    GM.gm.SendToGUI(testString);
            }
            GM.gm.EndGame();
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Console.WriteLine("Closing");
            GM.gm.EndGame();
        }
    }
}
