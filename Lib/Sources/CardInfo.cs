using Coinche.Protobuf;
using ProtoBuf;

namespace Lib
{
    [ProtoContract]
    public class CardInfo : Wrapper
    {
        [ProtoMember(1)]
        public int Face { get; set; } = -1;

        [ProtoMember(2)]
        public int Color { get; set; } = -1;

        public CardInfo()
        {}

        public CardInfo(int face, int color)
        {
            Face = face;
            Color = color;
        }
    }
}