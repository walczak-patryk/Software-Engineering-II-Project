using System;

namespace GameMaster
{
    interface IGameMaster
    {
        void SendMessage(string message);
        void GenerateTeams();
        void StartGame();
        void EndGame();
        string Listen();
        void MessageManager();
    }
    class GameMaster
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
