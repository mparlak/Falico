using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Falico.Net461.Tests.Commands;
using NUnit.Framework;

namespace Falico.Net461.Tests
{
    public class SendTests : TestBase
    {
        private IMediator _mediator;
        protected override void FinalizeSetUp()
        {
            IContainer container = Bootstrapper.Container;

            _mediator = container.Resolve<IMediator>();
        }

        [Test]
        public async Task Should_Resolve_Main_Void_Handler()
        {
            await _mediator.Send(new PingCommand { Message = "Hacı ben gönderdim." });

            Assert.IsTrue(true);
        }
    }
}
