using System;

namespace Coinche.Protobuf
{
    public abstract class Wrapper
    {
        public enum Type
        {
            Unknown = 0,
            Message = 1
        };
        
        public Type ProtobufType { get; }
        public byte[] ProtobufTypeAsBytes { get; }
        
        public Wrapper() {
            var t = this.GetType();
            if (t == typeof(Message))
                ProtobufType = Type.Message;
            else
            {
                ProtobufType = Type.Unknown;
                throw new Exception("Object type unknown");
            }
            
            ProtobufTypeAsBytes = BitConverter.GetBytes((short) ProtobufType);
        }
    }
}