using System.Net.Sockets;

namespace Coinche.Protobuf.Writer
{
    public interface IWriter
    {
        bool Run(NetworkStream stream, string input);
    }
}