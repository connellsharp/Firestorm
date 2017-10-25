using System;

namespace Firestorm.Tests.Models
{
    public class Album
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public Artist Artist { get; set; }
    }
}