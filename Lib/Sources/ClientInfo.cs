using Lib.Game.Card;
using ProtoBuf;

namespace Lib
{
    [ProtoContract]
    public class ClientInfo
    {
        [ProtoMember(1)]
        public int Id { get; set; } = -1;

        [ProtoMember(2)]
        public string Name { get; set; } = "";

        [ProtoMember(3)]
        public int TeamId
        {
            get => TeamId;

            set
            {
                TeamId = value;
                Team = Team.From(TeamId);
            }
        }

        public Team Team
        {
            get => Team;

            set => TeamId = (int) value.Index;
        }

        public ClientInfo()
        {
            TeamId = (int) Team.ETeam.Undefined;
        }
        
        public ClientInfo(int id)
        {
            Id = id;
            Name = Id.ToString();
        }
    }
}