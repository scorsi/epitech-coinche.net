using System.Text;
using ProtoBuf;

namespace Coinche.Protobuf
{
    [ProtoContract]
    public class Message : Wrapper
    {
        [ProtoMember(1)]
        public string Text { get; set; } = "";

        public Message()
        {
        }
        
        public Message(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.Append("message ").Append(Text);
            return s.ToString();
        }
    }
}