using Grpc.Core;
using System.Threading.Tasks;

namespace GrpcGreeter
{
    public class GreeterService : Greeter.GreeterBase
    {
        public override Task<HelloReply> SayHello(
            HelloRequest request, ServerCallContext context)
        {
            var reply = new HelloReply { 
                Message = $"Hello from gRPC, {request.Name}!" 
            };

            return Task.FromResult(reply);
        }
    }
}
