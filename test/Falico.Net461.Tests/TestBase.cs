using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;

namespace Falico.Net461.Tests
{
    [TestFixture]
    public abstract class TestBase
    {
        private MockRepository _mockRepository;

        protected IFixture FixtureRepository { get; set; }

        protected bool VerifyAll { get; set; }

        [SetUp]
        public void Setup()
        {
            this._mockRepository = new MockRepository(MockBehavior.Strict);
            this.FixtureRepository = (IFixture)new Fixture();
            this.VerifyAll = true;
            this.FinalizeSetUp();
        }

        [TearDown]
        public void TearDown()
        {
            if (this.VerifyAll)
                this._mockRepository.VerifyAll();
            else
                this._mockRepository.Verify();
            this.FinalizeTearDown();
        }

        protected Mock<T> MockFor<T>() where T : class
        {
            return this._mockRepository.Create<T>();
        }

        protected Mock<T> MockFor<T>(params object[] @params) where T : class
        {
            return this._mockRepository.Create<T>(@params);
        }

        protected T Create<T>()
        {
            return this.FixtureRepository.Create<T>();
        }

        protected IEnumerable<T> CreateMany<T>()
        {
            return this.FixtureRepository.CreateMany<T>();
        }

        protected IEnumerable<T> CreateMany<T>(int count)
        {
            return this.FixtureRepository.CreateMany<T>(count);
        }

        protected void EnableCustomization(ICustomization customization)
        {
            customization.Customize(this.FixtureRepository);
        }

        protected virtual void FinalizeTearDown()
        {
        }

        protected virtual void FinalizeSetUp()
        {
        }
    }
}
