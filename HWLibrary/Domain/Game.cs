using System;

namespace HWLibrary.Domain
{
    public class Game
    {
        public Game(bool isXwinner, Guid gameId, string host, string gamer)
        {
            IsXwinner = isXwinner;
            GameId = gameId;
            Host = host;
            Gamer = gamer;
        }

        public bool IsXwinner { get; }
        public Guid GameId { get; }
        public string Host { get; }
        public string Gamer { get; }
    }
}
