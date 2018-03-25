using System;
using Xunit;

namespace Firestorm.Tests
{
    public class ServiceProviderTests
    {
        [Fact]
        public void InvokeMethod_TwoClassArgs_CorrectString()
        {
            var sp = new ActivatorServiceProvider();
            
            var result = typeof(SpStub).Reflect()
                .WithNewInstance()
                .GetMethod(nameof(SpStub.GetValue))
                .WithParameters<ParamStub1, ParamStub2>()
                .FromServiceProvider(sp)
                .Invoke();
            
            Assert.Equal("emptyctor_ParamStub1Value_ParamStub2Value_", result);
        }
        
        [Fact]
        public void ServiceProvider_NewInstance_CorrectValue()
        {
            var sp = new ActivatorServiceProvider();
            
            var result = typeof(SpStub).Reflect()
                .WithNewInstance()
                .FromServiceProvider(sp)
                .GetMethod(nameof(SpStub.GetValue))
                .Invoke(new ParamStub1(), new ParamStub2());
            
            Assert.Equal("emptyctor_ParamStub1Value_ParamStub2Value_", result);
        }
        
        [Fact]
        public void ServiceProvider_ConstructorArgs_CorrectValue()
        {
            var sp = new ActivatorServiceProvider();
            
            var result = typeof(SpStub).Reflect()
                .WithNewInstance()
                .UsingConstructor<ParamStub1>()
                .WithArgumentsFromServiceProvider(sp)
                .GetMethod(nameof(SpStub.GetValue))
                .Invoke(new ParamStub1(), new ParamStub2());
            
            Assert.Equal("stub2ctor_ParamStub1Value_ParamStub2Value_", result);
        }

        private class SpStub
        {
            private readonly string _prefix;

            public SpStub()
            {
                _prefix = "emptyctor_";
            }
            
            public SpStub(ParamStub1 stub1)
            {
                _prefix = "stub2ctor_";
            }
            
            public string GetValue(ParamStub1 paramStub1, ParamStub2 paramStub2)
            {
                return _prefix + paramStub1.Value + paramStub2.Value;
            }
        }

        private class ParamStub1
        {
            public string Value { get; } = "ParamStub1Value_";
        }

        private class ParamStub2
        {
            public string Value { get; } = "ParamStub2Value_";
        }
    }

    internal class ActivatorServiceProvider : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            return Activator.CreateInstance(serviceType);
        }
    }
}