using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Engine.Defaults;
using Firestorm.Engine.Fields;
using Firestorm.Stems;
using Firestorm.Stems.Definitions;
using Firestorm.Stems.Essentials;
using Firestorm.Stems.Fuel.Fields;
using Firestorm.Testing;
using Firestorm.Testing.Models;
using Xunit;

namespace Firestorm.Stems.Tests
{
    public class AttributeFieldProviderTests
    {
        public AttributeFieldProviderTests()
        {
            Stem = new TestStem();
            Stem.SetParent(new TestStartAxis());
            Provider = new AttributeFieldProvider<Artist>(Stem);
            Item = new Artist();
        }

        private TestStem Stem { get; set; }  

        private AttributeFieldProvider<Artist> Provider { get; set; }

        private Artist Item { get; set; }

        [Fact]
        public async Task SetWithExpressionField()
        {
            await Provider.GetWriter("Name").SetValueAsync(Item, TestRepositories.ArtistName, new VoidTransaction());
            Assert.Equal(Item.Name, TestRepositories.ArtistName);
        }

        [Fact]
        public async Task SetReadOnlyField_Throws()
        {
            IFieldWriter<Artist> writer = Provider.GetWriter("ID");
            //Assert.IsNull(writer);

            await Assert.ThrowsAsync<InvalidOperationException>(async delegate
            {
                await writer.SetValueAsync(Item, 111, new VoidTransaction());
            });
        }

        [Fact]
        public void SetWithInstanceMethod()
        {
            var provider1 = new AttributeFieldProvider<Artist>(new LabelSetterStem("first_"));
            var provider2 = new AttributeFieldProvider<Artist>(new LabelSetterStem("second_"));

            var artist = new Artist();

            provider1.GetWriter("Label").SetValueAsync(artist, "Awesome", new VoidTransaction());
            Assert.Equal("first_Awesome", artist.Label);

            provider2.GetWriter("Label").SetValueAsync(artist, "Awesome", new VoidTransaction());
            Assert.Equal("second_Awesome", artist.Label);
        }

        private class LabelSetterStem : Stem<Artist>
        {
            private readonly string _setLabelPrefix;

            public LabelSetterStem(string setLabelPrefix)
            {
                _setLabelPrefix = setLabelPrefix;
                SetParent(new TestStartAxis());
            }

            [Get]
            public static Expression<Func<Artist, string>> Label { get; } = a => a.Label;

            [Set]
            public void SetLabel(Artist artist, string label)
            {
                artist.Label = _setLabelPrefix + label;
            }
        }

        private class TestStem : Stem<Artist> // todo duplicate in StemEndpointTests
        {
            [Get(Display.Nested), Identifier]
            public static Expression<Func<Artist, int>> ID { get; } = a => a.ID;

            [Get, Set]
            public static Expression<Func<Artist, string>> Name { get; } = a => a.Name;

            [Get(Display.Hidden)]
            public static Expression<Func<Artist, ICollection<Album>>> Albums { get; } = a => a.Albums;
        }
    }
}