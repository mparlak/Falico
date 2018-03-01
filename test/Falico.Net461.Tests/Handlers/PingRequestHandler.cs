using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Falico.Net461.Tests.Queries;

namespace Falico.Net461.Tests.Handlers
{
    public class PingRequestHandler : IRequestHandler<PingRequest, PongResponse>
    {
        public async Task<PongResponse> Handle(PingRequest request)
        {
            return await Task.FromResult(new PongResponse { Message = request.Message + " Pong" });
        }
    }
}
