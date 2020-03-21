using System;

namespace GameMaster
{
    interface IGameMaster
    {
        void GenerateBoard();
        void UpdateBoard(string[] );
        void SendMessage(string message);

        string Lis();
        void MessageManager();
    }
    class GM
    {
       
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
