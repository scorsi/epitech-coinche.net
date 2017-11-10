namespace Lib.Sources.Game.Card
{
    public class Contract 
    {
        private Team Team { get; set; }
        private ContractInfo.EType Type { get; set; }
        private int Value { get; set; }

        public Contract(Team team, ContractInfo.EType type, int value) {
            Team = team;
            Type = type;
            Value = value;
        }
    }
}