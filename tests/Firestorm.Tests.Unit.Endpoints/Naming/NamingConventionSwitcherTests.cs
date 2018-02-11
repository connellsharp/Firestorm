using System.Collections.Generic;
using Firestorm.Endpoints.Naming;
using Xunit;

namespace Firestorm.Tests.Unit.Endpoints.Naming
{
    public class NamingConventionSwitcherTests
    {
        [Fact]
        public void PascalToSnakeCase()
        {
            var switcher = new NamingConventionSwitcher(new PascalCaseConvention(), new SnakeCaseConvention());
            string switched = switcher.ConvertCodedToDefault("ThisIsPascalCase");
            Assert.Equal("this_is_pascal_case", switched);
        }

        [Fact]
        public void PascalToSnakeCaseWithAcronym()
        {
            var switcher = new NamingConventionSwitcher(new PascalCaseConvention(), new SnakeCaseConvention());
            string switched = switcher.ConvertCodedToDefault("WhatAboutHTTPListener");
            Assert.Equal("what_about_http_listener", switched);
        }

        [Fact]
        public void SnakeToPascalCase()
        {
            var switcher = new NamingConventionSwitcher(new SnakeCaseConvention(), new PascalCaseConvention());
            string switched = switcher.ConvertCodedToDefault("snake_case_start_yeah");
            Assert.Equal("SnakeCaseStartYeah", switched);
        }

        [Fact]
        public void CamelToKebabCase()
        {
            var switcher = new NamingConventionSwitcher(new CamelCaseConvention(), new KebabCaseConvention());
            string switched = switcher.ConvertCodedToDefault("camelCaseThingHere");
            Assert.Equal("camel-case-thing-here", switched);
        }

        [Fact]
        public void KebabToCamelCase()
        {
            var switcher = new NamingConventionSwitcher(new KebabCaseConvention(), new CamelCaseConvention());
            string switched = switcher.ConvertCodedToDefault("kebab-case-makes-me-laugh");
            Assert.Equal("kebabCaseMakesMeLaugh", switched);
        }

        [Fact]
        public void KebabWithTwoLetterAcronymToCamelCase()
        {
            var switcher = new NamingConventionSwitcher(new KebabCaseConvention(), new CamelCaseConvention());
            string switched = switcher.ConvertCodedToDefault("what-about-ui-acronym");
            Assert.Equal("whatAboutUIAcronym", switched);
        }

        [Fact]
        public void AllAllowedCases()
        {
            var switcher = new NamingConventionSwitcher(new PascalCaseConvention(), new SnakeCaseConvention(), new List<ICaseConvention>()
            {
                new PascalCaseConvention(),
                new SnakeCaseConvention(),
                new CamelCaseConvention(),
                new KebabCaseConvention()
            });

            string switched1 = switcher.ConvertRequestedToOutput("OriginallyPascalCase");
            Assert.Equal("originally_pascal_case", switched1);

            string switched2 = switcher.ConvertRequestedToOutput("originallyCamelCase");
            Assert.Equal("originally_camel_case", switched2);

            string switched3 = switcher.ConvertRequestedToOutput("originally_snake_case");
            Assert.Equal("originally_snake_case", switched3);

            string switched4 = switcher.ConvertRequestedToOutput("originally-kebab-case");
            Assert.Equal("originally_kebab_case", switched4);
        }

        [Fact]
        public void RelaxedFieldMappings()
        {
            var switcher = new NamingConventionSwitcher(new PascalCaseConvention(), new SnakeCaseConvention());
            var baseFieldMappings = new StemFieldDictionary<Artist>()
            {
                { "ArtistName", a => a.Name }
            };

            var fieldProvider = new RelaxedNamingFieldProvider<Artist>(baseFieldMappings, switcher);

            Assert.True(fieldProvider.FieldExists("artist_name"));
        }
    }
}