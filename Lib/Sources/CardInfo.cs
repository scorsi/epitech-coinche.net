using Lib.Game.Card;
using ProtoBuf;

namespace Lib
{
    [ProtoContract]
    public class CardInfo
    {
        /**
         * Face
         */
        private CardFace.EFace _faceId;
        [ProtoMember(1)]
        public CardFace.EFace FaceId
        {
            get => _faceId;

            set
            {
                _faceId = value;
                _face = value == CardFace.EFace.Undefined ? null : CardFace.From(value);
            }
        }

        private CardFace _face;
        public CardFace Face
        {
            get => _face;

            set
            {
                _face = value;
                _faceId = value?.Index ?? CardFace.EFace.Undefined;
            }
        }
        
        /**
         * Card
         */
        private CardColor.EColor _colorId;
        [ProtoMember(2)]
        public CardColor.EColor ColorId
        {
            get => _colorId;

            set
            {
                _colorId = value;
                _color = value == CardColor.EColor.Undefined ? null : CardColor.From(value);
            }
        }

        private CardColor _color;
        public CardColor Color
        {
            get => _color;

            set
            {
                _color = value;
                _colorId = value?.Index ?? CardColor.EColor.Undefined;
            }
        }

        public CardInfo()
        {}

        public CardInfo(CardFace.EFace face, CardColor.EColor color)
        {
            FaceId = face;
            ColorId = color;
        }
    }
}