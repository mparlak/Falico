using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Falico.Net461.Tests.Commands
{
    public class PingCommand : ICommand
    {
        public string Message { get; set; }
    }
}
