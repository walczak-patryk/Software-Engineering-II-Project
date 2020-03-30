using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace GameMaster
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Start Game");
            new GameMaster().StartGame();           
        }
    }
}
