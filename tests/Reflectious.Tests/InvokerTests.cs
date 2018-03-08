using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Firestorm;
using Xunit;

namespace Reflectious.Tests
{
    public class InvokerTests
    {
        [Fact]
        public void GetMethod_MethodExpression_CorrectValue()
        {
            var stub = new Stub();
            
            string returnValue = stub.Invoker()
                .GetMethod(s => s.DoInstanceMethod())
                .Invoke();
            
            Assert.Equal(Stub.MethodExecutedString, returnValue);
        }
        
        [Fact]
        public void GetMethod_Nameof_CorrectValue()
        {
            var stub = new Stub();
            
            string returnValue = stub.Invoker()
                .GetMethod(nameof(Stub.DoInstanceMethod))
                .ReturnsType<string>()
                .Invoke();
            
            Assert.Equal(Stub.MethodExecutedString, returnValue);
        }
        
        [Fact]
        public void GetProperty_Expression_CorrectValue()
        {
            var stub = new Stub();
            
            string returnValue = stub.Invoker()
                .GetProperty(s => s.InstanceProperty)
                .GetValue();
            
            Assert.Equal(Stub.InitialPropertyValue, returnValue);
        }
        
        [Fact]
        public void SetProperty_Expression_ChangesValue()
        {
            var stub = new Stub();
            
            stub.Invoker()
                .GetProperty(s => s.InstanceProperty)
                .SetValue("Test change");
            
            Assert.Equal("Test change", stub.InstanceProperty);
        }
        
        [Fact]
        public void SetStaticProperty_Expression_ChangesValue()
        {            
            typeof(Stub).Invoker()
                .GetProperty(nameof(Stub.StaticProperty))
                .OfType<string>()
                .SetValue("Test change");
            
            Assert.Equal("Test change", Stub.StaticProperty);
        }
        
        [Fact]
        public void GetStaticMethod_LinqEnumerableType_FindsAnyMethod()
        {
            var stub = new Stub();
            
            var result = typeof(Enumerable).Invoker()
                .GetMethod(nameof(Enumerable.Any))
                .ReturnsType<IEnumerable<Stub>>()
                .MakeGeneric<Stub>()
                .WithParameters<IEnumerable<Stub>, Func<Stub, bool>>()
                .Invoke(new[] { stub }, s => false);
            
            Assert.Empty(result);
        }
    }
}