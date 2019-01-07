using System;
using Firestorm.Testing.Models;

namespace Firestorm.Tests.Unit
{
    public class Album
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public Artist Artist { get; set; }
    }
}