using System;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;
using Lib;

namespace Coinche.Client.Protobuf.Reader.Lobby
{
    public class ContractHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LobbyContract>(stream,
                ProtoBuf.PrefixStyle.Fixed32);

            if (proto.ContractType != ContractInfo.EType.Undefined)
                Console.Out.WriteLineAsync("Your contract has been taken into account.");
            else
                Console.Out.WriteLineAsync("Your contract has not been taken into account.");
            return true;
        }
    }
}