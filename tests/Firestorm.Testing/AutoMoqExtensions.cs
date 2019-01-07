using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using Moq;

namespace Firestorm.Testing
{
    public static class AutoMoqExtensions
    {
        public static Mock<T> FreezeMock<T>(this Fixture fixture)
            where T : class
        {
            var mock = fixture.Freeze<Mock<T>>();
            //fixture.Inject(mock.Object);
            return mock;
        }

        public static Mock<T> FreezeMock<T>(this Fixture fixture, Action<Mock<T>> setupAction)
            where T : class
        {
            var mock = fixture.FreezeMock<T>();
            setupAction(mock);
            return mock;
        }

        public static IQueryable<T> FreezeQueryable<T>(this Fixture fixture, int count)
            where T : class
        {
            var queryable = fixture.CreateMany<T>(count).AsQueryable();
            fixture.Inject(queryable);
            return queryable;
        }

        public static void Relay<TFrom, TTo>(this Fixture fixture)
            where TTo : TFrom
        {
            fixture.Customizations.Add(new TypeRelay(typeof(TFrom), typeof(TTo)));
        }
    }
}