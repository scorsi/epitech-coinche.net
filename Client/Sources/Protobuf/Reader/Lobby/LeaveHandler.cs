﻿using System;
using System.Net.Sockets;
using Coinche.Protobuf;
using Coinche.Protobuf.Reader;

namespace Coinche.Client.Protobuf.Reader.Lobby
{
    public class LeaveHandler : IReader
    {
        public bool Run(NetworkStream stream, int clientId = 0)
        {
            var proto = ProtoBuf.Serializer.DeserializeWithLengthPrefix<LobbyLeave>(stream,
                ProtoBuf.PrefixStyle.Fixed32);
            Console.Out.WriteLineAsync(proto.Name);
            return true;
        }
    }
}