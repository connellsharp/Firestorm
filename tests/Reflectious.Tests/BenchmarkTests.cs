using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Firestorm;
using Xunit;

namespace Firestorm.Tests
{
    public class BenchmarkTests
    {
        [Fact]
        public void InvokeMethod_Stub_AsFastAsReflection()
        {
            var stub = new Stub();
            const string methodName = nameof(Stub.DoInstanceMethod);

            new BenchmarkActions
                {
                    LibraryCode = () =>
                    {
                        string returnValue = stub.Reflect()
                            .GetMethod(methodName, Assume.UnambiguousName)
                            .ReturnsType<string>()
                            .Invoke();
                    },
                    NativeCode = () =>
                    {
                        string returnValue = (string) typeof(Stub)
                            .GetMethod(methodName)
                            .Invoke(stub, new object[] { });
                    }
                }
                .AssertFasterOrEqual();
        }
        
        [Fact]
        public void ConstructObject_Stub_AsFastAsReflection()
        {
            new BenchmarkActions
                {
                    LibraryCode = () =>
                    {
                        Stub stub = new StaticReflector<Stub>()
                            .GetConstructor()
                            .Invoke();
                    },
                    NativeCode = () =>
                    {
                        Stub stub = (Stub)typeof(Stub)
                            .GetConstructor(new Type[] { })
                            .Invoke(new object[] { });
                    }
                }
                .AssertFasterOrEqual();
        }
        
        [Fact]
        public void ConstructObject_Stub_AsFastAsActivator()
        {
            new BenchmarkActions
                {
                    LibraryCode = () =>
                    {
                        Stub stub = new StaticReflector<Stub>()
                            .CreateInstance();
                    },
                    NativeCode = () =>
                    {
                        Stub stub = Activator.CreateInstance<Stub>();
                    }
                }
                .AssertFasterOrEqual();
        }

        [Fact]
        public void FindMethod_Stub_AsFastAsReflection()
        {
            new BenchmarkActions
                {
                    LibraryCode = () =>
                    {
                        MethodInfo anyMethod = typeof(Enumerable).Reflect()
                            .GetMethod("Any")
                            .MakeGeneric<Stub>()
                            .WithParameters<IEnumerable<Stub>, Func<Stub, bool>>() // TODO handle List and arrays etc ?
                            .MethodInfo;
                    },
                    NativeCode = () =>
                    {
                        MethodInfo anyMethod = GetGenericMethod(typeof(Enumerable), "Any", new[] {typeof(Stub)},
                            new[] {typeof(IEnumerable<Stub>), typeof(Func<Stub, bool>)},
                            BindingFlags.Static | BindingFlags.Public);
                    }
                }
                .AssertFasterOrEqual();

            MethodInfo GetGenericMethod(Type type, string name, Type[] typeArgs, Type[] paramTypes, BindingFlags flags)
            {
                IEnumerable<MethodInfo> methods = type.GetMethods(flags)
                    .Where(m => m.Name == name)
                    .Where(m => m.GetGenericArguments().Length == typeArgs.Length)
                    .Where(m => m.GetParameters().Length == paramTypes.Length)
                    .Select(m => m.MakeGenericMethod(typeArgs));

                return methods.First();
            }
        }

        private class BenchmarkActions
        {
            public Action LibraryCode { get; set; }
            public Action NativeCode { get; set; }

            public void AssertFasterOrEqual()
            {
                BenchmarkTests.AssertFasterOrEqual(Benchmark(LibraryCode), Benchmark(NativeCode));
            }
        }

        private static long Benchmark(Action action, int iterations = 100000)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < iterations; i++)
            {
                action.Invoke();
            }

            stopwatch.Stop();
            return stopwatch.ElapsedTicks / iterations;
        }

        private static void AssertFasterOrEqual(long library, long native)
        {
            double ratio = (double)library / native;
            
            Assert.True(library <= native,
                $"{ratio:#.0}x slower than native. Library {library} ticks. Native {native} ticks.");
        }
    }
}