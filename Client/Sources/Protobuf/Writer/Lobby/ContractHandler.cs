using System;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Coinche.Protobuf;
using Coinche.Protobuf.Writer;
using Lib;

namespace Coinche.Client.Protobuf.Writer.Lobby
{
    public class ContractHandler : IWriter
    {
        private bool CheckFormat(string[] args)
        {
            return args.Length >= 2 && args[1].Length > 0 && (args[1].ToLower().Equals("pass") || args.Length >= 3) && (args[1].ToLower().Equals("pass") || args[2].Length > 0);
        }

        private bool CheckValue(string[] args)
        {
            return args[1].ToLower().Equals("pass") || (Convert.ToInt32(args[2]) >= 80 && (Convert.ToInt32(args[2]) <= 650));
        }
        
        public bool Run(NetworkStream stream, string input)
        {
            var args = Regex.Split(input, @"\s+");
            var type = ContractInfo.EType.Undefined;
            var value = 0;

            if (!CheckFormat(args) || !CheckValue(args))
                return false;

            foreach (var enumType in Enum.GetValues(typeof(ContractInfo.EType)))
            {
                if (enumType.ToString().ToLower().Equals(args[1].ToLower()))
                {
                    type = (ContractInfo.EType) enumType;
                }
            }

            if (!args[1].Equals("pass"))
                value = Convert.ToInt32(args[2]);
            
            var proto = new LobbyContract(type, value);
            stream.Write(proto.ProtobufTypeAsBytes, 0, 2);
            ProtoBuf.Serializer.SerializeWithLengthPrefix(stream, proto, ProtoBuf.PrefixStyle.Fixed32);
            return true;
        }
    }
}