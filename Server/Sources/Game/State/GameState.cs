using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using Coinche.Protobuf;
using Lib;
using Lib.Game.Card;

namespace Coinche.Server.Game.State
{
    public class GameState : AState
    {
        private Dictionary<Team, int> Points { get; set; }
        private int FirstPlayer { get; set; }
        private int Turn { get; set; }
        private Dictionary<ClientInfo, CardInfo> Table { get; set; }
        private Contract Contract { get; set; }
        
        public GameState(Lobby lobby, Contract contract) : base("Game", lobby)
        {
            Contract = contract;
        }

        public override void Initialize()
        {
            Points = new Dictionary<Team, int>
            {
                [Team.Blue] = 0,
                [Team.Red] = 0
            };
            
            Table = new Dictionary<ClientInfo, CardInfo>();

            FirstPlayer = 0;
            Turn = 0;
            
            Lobby.Broadcast("The game is ready!");
            DisplayTurnMessage();
        }

        public override bool IsFinished()
        {
            return Lobby.Info.Clients.All(client => client.Deck.Count <= 0);
        }

        private Team GetOtherTeam(Team team)
        {
            return team == Team.Blue ? Team.Red : Team.Blue;
        }
        
        public override AState NextState()
        {
            if (Points[Contract.Team] >= Contract.Value)
            {
                Lobby.Broadcast("The team " + Contract.Team.Name + " respected their contract." + Environment.NewLine +
                                "They win " + Contract.Value + " points.");
                
                Points[Contract.Team] += Contract.Value;
            }
            else
            {
                Lobby.Broadcast("The team " + Contract.Team.Name + " missed their contract." + Environment.NewLine +
                                "They lose their points and the other team win 162 + " + Contract.Value + " points.");
                Points[Contract.Team] = 0;
                Points[GetOtherTeam(Contract.Team)] = 162 + Contract.Value;
            }

            Lobby.Points[Team.Blue] += Points[Team.Blue];
            Lobby.Points[Team.Red] += Points[Team.Red];
            
            DisplayPartyPoints();
            
            Lobby.Broadcast("Prepare for a new round.");
            return new DrawState(Lobby);
        }

        private int CalculateFoldPoints()
        {
            var foldPoints = 0;
            
            foreach (var card in Table.Values)
            {
                switch (Contract.Type)
                {
                    case ContractInfo.EType.Spade:
                    {
                        if (card.ColorId == CardColor.Spade.Index) {
                            foldPoints += card.GetPointOneTrump();
                        } else {
                            foldPoints += card.GetPointIsNotTrump();
                        }
                        break;                        
                    }
                    case ContractInfo.EType.Heart:
                    {
                        if (card.ColorId == CardColor.Heart.Index) {
                            foldPoints += card.GetPointOneTrump();
                        } else {
                            foldPoints += card.GetPointIsNotTrump();
                        }
                        break;                        
                    }
                    case ContractInfo.EType.Club:
                    {
                        if (card.ColorId == CardColor.Club.Index) {
                            foldPoints += card.GetPointOneTrump();
                        } else {
                            foldPoints += card.GetPointIsNotTrump();
                        }
                        break;                        
                    }
                    case ContractInfo.EType.Diamond:
                    {
                        if (card.ColorId == CardColor.Diamond.Index) {
                            foldPoints += card.GetPointOneTrump();
                        } else {
                            foldPoints += card.GetPointIsNotTrump();
                        }
                        break;                        
                    }
                    case ContractInfo.EType.NoTrump:
                    {
                        foldPoints += card.GetPointNoTrump();
                        break;                        
                    }
                    case ContractInfo.EType.AllTrump:
                    default:
                    {
                        foldPoints += card.GetPointAllTrump();
                        break;                        
                    }
                }
            }
            return foldPoints;
        }

        private ClientInfo CalculateFoldWinnerWithColorTrump()
        {
            CardColor color;
            var higherPoint = -1;
            ClientInfo foldWinner = null;
            
            switch (Contract.Type)
            {
                case ContractInfo.EType.Spade:
                    color = CardColor.Spade;
                    break;
                case ContractInfo.EType.Heart:
                    color = CardColor.Heart;
                    break;
                case ContractInfo.EType.Club:
                    color = CardColor.Club;
                    break;
                case ContractInfo.EType.Diamond:
                default:
                    color = CardColor.Diamond;
                    break;
            }
            
            foreach (var entry in Table)
            {
                if (color.Name != entry.Value.Color.Name || entry.Value.GetPointAllTrump() <= higherPoint) continue;
                
                foldWinner = entry.Key;
                higherPoint = entry.Value.GetPointAllTrump();
            }

            return higherPoint == -1 ? CalculateFoldWinnerWithIsNotTrump() : foldWinner;
        }

        private ClientInfo CalculateFoldWinnerWithAllTrump()
        {
            var color = Table.ToArray()[0].Value.Color;
            var higherPoint = -1;
            ClientInfo foldWinner = null;

            foreach (var entry in Table)
            {
                if (color.Name != entry.Value.Color.Name || entry.Value.GetPointAllTrump() <= higherPoint) continue;
                
                foldWinner = entry.Key;
                higherPoint = entry.Value.GetPointAllTrump();
            }
            return foldWinner;
        }

        private ClientInfo CalculateFoldWinnerWithNoTrump()
        {
            var color = Table.ToArray()[0].Value.Color;
            var higherPoint = -1;
            ClientInfo foldWinner = null;

            foreach (var entry in Table)
            {
                if (color.Name != entry.Value.Color.Name || entry.Value.GetPointNoTrump() <= higherPoint) continue;
                
                foldWinner = entry.Key;
                higherPoint = entry.Value.GetPointNoTrump();
            }
            return foldWinner;
        }

        private ClientInfo CalculateFoldWinnerWithIsNotTrump()
        {
            var color = Table.ToArray()[0].Value.Color;
            var higherPoint = -1;
            ClientInfo foldWinner = null;

            foreach (var entry in Table)
            {
                if (color.Name != entry.Value.Color.Name || entry.Value.GetPointIsNotTrump() <= higherPoint) continue;
                
                foldWinner = entry.Key;
                higherPoint = entry.Value.GetPointIsNotTrump();
            }
            return foldWinner;
        }
        
        private void CalculateFoldWinner()
        {
            ClientInfo foldWinner = null;
            switch (Contract.Type)
            {
                case ContractInfo.EType.Spade:
                case ContractInfo.EType.Heart:
                case ContractInfo.EType.Club:
                case ContractInfo.EType.Diamond:
                    foldWinner = CalculateFoldWinnerWithColorTrump();
                    break;
                case ContractInfo.EType.AllTrump:
                    foldWinner = CalculateFoldWinnerWithAllTrump();
                    break;
                case ContractInfo.EType.NoTrump:
                default:
                    foldWinner = CalculateFoldWinnerWithNoTrump();
                    break;
            }
            var foldPoints = CalculateFoldPoints();
            Points[foldWinner.Team] += foldPoints;

            FirstPlayer = 0;
            foreach (var client in Lobby.Info.Clients)
            {
                if (client.Id == foldWinner.Id) break;
                FirstPlayer++;
            }

            Turn = FirstPlayer;
            Table.Clear();
            DisplayRoundPoints();
        }

        public override bool HandleAction(Wrapper command, Client client)
        {
            if (command.ProtobufType == Wrapper.Type.LobbyShowCards)
            {
                return true;
            }
            if (command.ProtobufType == Wrapper.Type.LobbyCard)
            {
                return HandleCard((LobbyCard) command, client);
            }
            return false;
        }

        private bool HandleCard(LobbyCard command, Client client)
        {
            if (client.Info.Id != Lobby.Info.Clients[Turn].Id) return false;

            CardInfo cardToRemove = null;
            foreach (var card in client.Info.Deck)
            {
                if (card.ColorId == command.Info.ColorId && card.FaceId == command.Info.FaceId)
                    cardToRemove = card;
            }
            if (cardToRemove == null) return false;
            
            Lobby.Broadcast(client.Info.Name + " played a " + command.Info.Face.Name + " of " + command.Info.Color.Name + ".");
            Table[client.Info] = cardToRemove;
            client.Info.Deck.Remove(cardToRemove);
            
            DisplayTable();
            NextTurn();
            return true;
        }

        private void DisplayTurnMessage()
        {
            Lobby.Broadcast("This is the turn of " + Lobby.Info.Clients[Turn].Name + " to play a card.");
        }

        private void DisplayPartyPoints()
        {
            Lobby.Broadcast("Team points:" + Environment.NewLine + "Red: " + Lobby.Points[Team.Red] + "." + Environment.NewLine + "Blue: " + Lobby.Points[Team.Blue] + ".");
        }

        private void DisplayRoundPoints()
        {
            Lobby.Broadcast("Round team points:" + Environment.NewLine + "Red: " + Points[Team.Red] + "." + Environment.NewLine + "Blue: " + Points[Team.Blue] + ".");
        }

        private void DisplayTable()
        {
            var sb = new StringBuilder();

            sb.Append("Table:");
            foreach (var card in Table.Values)
            {
                sb.Append(Environment.NewLine + card.Face.Name + " " + card.Color.Name);
            }
            Lobby.Broadcast(sb.ToString());
        }

        private void NextTurn()
        {
            Turn++;
            if (Turn == 4)
                Turn = 0;
            if (Turn == FirstPlayer)
                CalculateFoldWinner();
            DisplayTurnMessage();
        }
    }
}