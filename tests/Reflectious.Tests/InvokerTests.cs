using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Firestorm;
using Xunit;

namespace Firestorm.Tests
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
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetStaticMethod_LinqEnumerableType_FindsAnyMethod(bool value)
        {
            var stub = new Stub();
            
            var result = typeof(Enumerable).Invoker()
                .GetMethod(nameof(Enumerable.Any))
                .ReturnsType<bool>()
                .MakeGeneric<Stub>()
                .WithParameters<IEnumerable<Stub>, Func<Stub, bool>>()
                .Invoke(new[] { stub }, s => value);
            
            Assert.Equal(value, result);
        }
        
        [Fact]
        public void ConstructorMakeGeneric_GenericList_CreatesObject()
        {
            var result = typeof(List<>).Invoker()
                .GetConstructor()
                .MakeGeneric<Stub>()
                .WithParameters() // TODO maybe default to parameterless ctor?
                .Invoke();
            
            Assert.IsType<List<Stub>>(result);
        }
        
        [Fact]
        public void TypeMadeGenericConstructor_GenericList_CreatesObject()
        {
            var result = typeof(List<>).Invoker()
                .MakeGeneric<Stub>()
                .GetConstructor()
                .WithParameters()
                .Invoke();
            
            Assert.IsType<List<Stub>>(result);
        }
        
        [Fact]
        public void AlreadyGenericTypeConstructor_GenericList_CreatesObject()
        {
            var result = typeof(List<Stub>).Invoker()
                .GetConstructor()
                .WithParameters()
                .Invoke();
            
            Assert.IsType<List<Stub>>(result);
        }
        
        [Fact]
        public void StrongStaticInvoker_GenericList_CreatesObject()
        {
            var result = new StaticInvoker<List<Stub>>()
                .GetConstructor()
                .WithParameters()
                .Invoke();
            
            Assert.IsType<List<Stub>>(result);
        }
        
        [Fact]
        public void StrongStaticCreateInstance_GenericList_CreatesObject()
        {
            var result = new StaticInvoker<List<Stub>>()
                .CreateInstance();
            
            Assert.IsType<List<Stub>>(result);
        }
        
        [Fact]
        public void WeakStaticCreateInstance_GenericList_CreatesObject()
        {
            var result = typeof(List<Stub>).Invoker()
                .CreateInstance();
            
            Assert.IsType<List<Stub>>(result);
        }
    }
}