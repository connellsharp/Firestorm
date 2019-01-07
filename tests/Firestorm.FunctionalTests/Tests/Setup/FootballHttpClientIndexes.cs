using System.Collections;
using System.Collections.Generic;

namespace Firestorm.Tests.Examples.Football.Tests
{
    public class FootballHttpClientIndexes : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] { FirestormApiTech.Stems },
            new object[] { FirestormApiTech.Fluent }
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public enum FirestormApiTech
    {
        Stems,
        Fluent
    }
}