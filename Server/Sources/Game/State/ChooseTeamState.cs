using System;
using System.Collections.Generic;
using System.Linq;
using Coinche.Protobuf;
using Lib;
using Lib.Game.Card;

namespace Coinche.Server.Game.State
{
    public class ChooseTeamState : AState
    {
        public ChooseTeamState(Lobby lobby) : base("ChooseTeam", lobby)
        {
        }

        public override void Initialize()
        {
            foreach (var client in Lobby.Info.Clients)
            {
                client.Team = Team.Undefined;
            }

            Lobby.Info.ResetPoints();
        }

        public override bool IsFinished()
        {
            return Lobby.Info.Clients.All(client => client.Team != Team.Undefined);
        }

        public override AState NextState()
        {
            var players = new List<ClientInfo>();
            ClientInfo lastAddedPlayer = null;
            
            var lobbyPlayers = new List<ClientInfo>(Lobby.Info.Clients);
            var random = new Random();

            while (lobbyPlayers.Count != 0) {
                var playerToAdd = lobbyPlayers[random.Next() * 100 % lobbyPlayers.Count];
                
                if (players.Count != 0 && playerToAdd.Team == lastAddedPlayer.Team) continue;
                
                players.Add(playerToAdd);
                lastAddedPlayer = playerToAdd;
                lobbyPlayers.Remove(playerToAdd);
            }
            
            return null;
        }

        public override void HandleAction(Wrapper command, Client client)
        {
        }
    }
}