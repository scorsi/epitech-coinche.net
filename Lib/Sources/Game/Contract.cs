namespace Lib.Game.Card
{
    public class Contract 
    {
        public Team Team { get; set; }
        public ContractInfo.EType Type { get; set; }
        public int Value { get; set; }

        public Contract(Team team, ContractInfo.EType type, int value) {
            Team = team;
            Type = type;
            Value = value;
        }
    }
}