using System;

namespace GameMaster
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Start Game");
            //Thread t = new Thread(ThreadProc);
            //t.SetApartmentState(ApartmentState.STA);
            //t.Start();
            new GameMaster().StartGame();
        }
    }
}
