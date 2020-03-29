using System;

namespace Game.GameMaster
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
    public class StartGame : IGameMaster
    {
        public static void Main() { }
        public void EndGame()
        {
            throw new NotImplementedException();
        }

        public void GenerateTeams()
        {
            throw new NotImplementedException();
        }

        public string Listen()
        {
            throw new NotImplementedException();
        }

        public void MessageManager()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void StartGame()
        {
            throw new NotImplementedException();
        }
    }
}
