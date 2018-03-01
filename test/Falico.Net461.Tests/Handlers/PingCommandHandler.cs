using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Falico.Net461.Tests.Commands;

namespace Falico.Net461.Tests.Handlers
{
    public class PingCommandHandler : ICommandHandler<PingCommand>
    {
        public async Task Handle(PingCommand command)
        {
            Debug.WriteLine(command.Message);
        }
    }
}
