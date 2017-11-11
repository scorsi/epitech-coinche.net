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
            Lobby.Broadcast("The lobby is complete, you can now choose your teams.");
            
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
                var playerToAdd = lobbyPlayers[random.Next(0, lobbyPlayers.Count)];
                
                if (players.Count != 0 && playerToAdd.Team == lastAddedPlayer.Team) continue;
                
                players.Add(playerToAdd);
                lastAddedPlayer = playerToAdd;
                lobbyPlayers.Remove(playerToAdd);
            }

            Lobby.Info.Clients = players;
            
            Lobby.Broadcast("All teams are complete.");
            
            return new DrawState(Lobby);
        }

        public override void HandleAction(Wrapper command, Client client)
        {
            if (command.ProtobufType == Wrapper.Type.LobbyTeam)
            {
                HandleTeam((LobbyTeam) command, client);
            }
        }

        private bool IsTeamFull(Client clientToNotCheck, Team teamToJoin)
        {
            return Lobby.Info.Clients
                       .Where(client => client != clientToNotCheck.Info)
                       .Count(client => client.Team == teamToJoin)
                   >= 2;
        }

        private void HandleTeam(LobbyTeam command, Client client)
        {
            var teamToJoin = Team.From(command.Team);
            if (IsTeamFull(client, teamToJoin)) return;
            client.Info.Team = teamToJoin;
            Lobby.Broadcast("Player " + client.Info.Name + " joined team " + teamToJoin.Name + ".", false, client);
        }
    }
}