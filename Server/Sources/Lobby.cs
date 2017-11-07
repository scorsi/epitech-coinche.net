using Lib.Sources;

namespace Coinche.Server
{
    public class Lobby
    {
        public LobbyInfo Info { get; set; }

        public Lobby(LobbyInfo info) => Info = info;
        public Lobby(string name) => Info = new LobbyInfo(name);
    }
}