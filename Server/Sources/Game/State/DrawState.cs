﻿using Coinche.Protobuf;

namespace Coinche.Server.Game.State
{
    public class DrawState : AState
    {
        public DrawState(Lobby lobby) : base("Draw", lobby)
        {
        }
        
        public override void Initialize()
        {
            System.Console.Out.WriteLineAsync("ChooseTeam");
        }

        public override bool IsFinished()
        {
            return false;
        }

        public override AState NextState()
        {
            return null;
        }

        public override void HandleAction(Wrapper command, Client client)
        {
        }
    }
}