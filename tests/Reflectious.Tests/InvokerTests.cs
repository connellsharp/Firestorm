using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            
            string returnValue = stub.Reflect()
                .GetMethod(s => s.DoInstanceMethod())
                .Invoke();
            
            Assert.Equal(Stub.MethodExecutedString, returnValue);
        }

        [Fact]
        public void GetMethod_Nameof_CorrectValue()
        {
            var stub = new Stub();

            string returnValue = stub.Reflect()
                .GetMethod(nameof(Stub.DoInstanceMethod))
                .ReturnsType<string>()
                .Invoke();

            Assert.Equal(Stub.MethodExecutedString, returnValue);
        }

        [Fact]
        public void GetProperty_Expression_CorrectValue()
        {
            var stub = new Stub();
            
            string returnValue = stub.Reflect()
                .GetProperty(s => s.InstanceProperty)
                .GetValue();
            
            Assert.Equal(Stub.InitialPropertyValue, returnValue);
        }
        
        [Fact]
        public void SetProperty_Expression_ChangesValue()
        {
            var stub = new Stub();
            
            stub.Reflect()
                .GetProperty(s => s.InstanceProperty)
                .SetValue("Test change");
            
            Assert.Equal("Test change", stub.InstanceProperty);
        }
        
        [Fact]
        public void SetStaticProperty_Expression_ChangesValue()
        {            
            typeof(Stub).Reflect()
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
            
            var result = typeof(Enumerable).Reflect()
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
            var result = typeof(List<>).Reflect()
                .GetConstructor()
                .MakeGeneric<Stub>()
                .Invoke();
            
            Assert.IsType<List<Stub>>(result);
        }
        
        [Fact]
        public void ConstructorWithParameter_GenericList_CreatesObject()
        {
            var arr = new[] {new Stub()};

            var result = typeof(List<>).Reflect()
                .GetConstructor()
                .MakeGeneric<Stub>()
                .Invoke(arr.AsEnumerable());

            var castedResult = Assert.IsType<List<Stub>>(result);
            Assert.Equal(arr[0], castedResult[0]);
        }
        
        [Fact]
        public void TypeMadeGenericConstructor_GenericList_CreatesObject()
        {
            var result = typeof(List<>).Reflect()
                .MakeGeneric<Stub>()
                .GetConstructor()
                .WithParameters()
                .Invoke();
            
            Assert.IsType<List<Stub>>(result);
        }
        
        [Fact]
        public void AlreadyGenericTypeConstructor_GenericList_CreatesObject()
        {
            var result = typeof(List<Stub>).Reflect()
                .GetConstructor()
                .WithParameters()
                .Invoke();
            
            Assert.IsType<List<Stub>>(result);
        }
        
        [Fact]
        public void StrongStaticInvoker_GenericList_CreatesObject()
        {
            var result = new StaticReflector<List<Stub>>()
                .GetConstructor()
                .WithParameters()
                .Invoke();
            
            Assert.IsType<List<Stub>>(result);
        }
        
        [Fact]
        public void StrongStaticCreateInstance_GenericList_CreatesObject()
        {
            var result = new StaticReflector<List<Stub>>()
                .CreateInstance();
            
            Assert.IsType<List<Stub>>(result);
        }
    }
}