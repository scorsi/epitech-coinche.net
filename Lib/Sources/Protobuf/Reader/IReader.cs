using System.Net.Sockets;

namespace Coinche.Protobuf.Reader
{
    public interface IReader
    {
        bool Run(NetworkStream stream, int clientId = 0);
    }
}