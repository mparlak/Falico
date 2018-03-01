using System.Threading.Tasks;
using Autofac;
using Falico.Net461.Tests.Queries;
using FluentAssertions;
using NUnit.Framework;

namespace Falico.Net461.Tests
{
    public class RequestTests:TestBase
    {
        private IMediator _mediator;
        protected override void FinalizeSetUp()
        {
            IContainer container = Bootstrapper.Container;

            _mediator = container.Resolve<IMediator>();
        }

        [Test]
        public async Task Should_Resolve_Main_Request_Handler()
        {
            var response = await _mediator.Request(new PingRequest { Message = "Ping" });

            response.Message.Should().Be("Ping Pong");
        }
    }
}
