using System;
using Coinche.Protobuf;
using Lib;
using Lib.Game.Card;

namespace Coinche.Server.Game.State
{
    public class ContractState : AState
    {
        private int Turn { get; set; }
        private Contract Contract { get; set; }
        
        public ContractState(Lobby lobby) : base("Contract", lobby)
        {
        }

        public override void Initialize()
        {
            Lobby.Broadcast("You can now put contracts.");
            Turn = 0;
            Contract = null;
            DisplayTurnMessage();
        }

        public override bool IsFinished()
        {
            return Turn >= 4;
        }

        public override AState NextState()
        {
            if (Contract != null)
                return new GameState(Lobby);
            Lobby.Broadcast("Anyone put contract. Replay the round.");
            return new DrawState(Lobby);
        }

        public override bool HandleAction(Wrapper command, Client client)
        {
            if (command.ProtobufType == Wrapper.Type.LobbyContract)
            {
                if (HandleContract((LobbyContract) command, client))
                {
                    ++Turn;
                    DisplayTurnMessage();
                    return true;
                }
            }
            return false;
        }

        private bool HandleContract(LobbyContract command, Client client)
        {
            if (command.ContractType == ContractInfo.EType.Undefined || client.Info.Id != Lobby.Info.Clients[Turn].Id) return false;
            
            if (command.ContractType == ContractInfo.EType.Pass)
            {
                Lobby.Broadcast(client.Info.Name + " passed.");
                return true;
            }
            if (Contract != null && Contract.Team == client.Info.Team)
                return false;
            if (command.ContractType == ContractInfo.EType.Coinche)
            {
                if (Contract == null)
                    return false;
                if (Contract.Type == ContractInfo.EType.Coinche)
                    Lobby.Broadcast(client.Info.Name + " called recoinche.");
            }
            if (Contract?.Value <= command.ContractValue)
                return false;

            Contract = new Contract(client.Info.Team, command.ContractType, command.ContractValue);
            Lobby.Broadcast(client.Info.Name + " put a contract " + command.ContractType + " of " +
                            command.ContractValue + ".");
            return true;
        }

        private void DisplayTurnMessage()
        {
            try
            {
                Lobby.Broadcast("This is the turn of " + Lobby.Info.Clients[Turn].Name + " to put a contract.");
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}