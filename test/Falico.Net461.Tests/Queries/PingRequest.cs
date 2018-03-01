using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Falico.Net461.Tests.Queries
{
    public class PingRequest : IRequest<PingRequest, PongResponse>
    {
        public string Message { get; set; }
    }
}
