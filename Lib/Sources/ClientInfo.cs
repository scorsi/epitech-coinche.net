using System.Collections.Generic;
using Lib.Game.Card;
using ProtoBuf;

namespace Lib
{
    [ProtoContract]
    public class ClientInfo
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        private int _TeamId;
        [ProtoMember(3)]
        public int TeamId
        {
            get => _TeamId;

            set
            {
                _TeamId = value;
                _Team = Team.From(TeamId);
            }
        }

        private Team _Team;
        public Team Team
        {
            get => _Team;
            
            set
            {
                _Team = value;
                _TeamId = (int) value.Index;
            }
        }

        [ProtoMember(4)]
        public List<CardInfo> Deck { get; set; }

        public ClientInfo()
        {
        }
        
        public ClientInfo(int id)
        {
            Id = id;
            Name = "Player" + Id;
        }
    }
}