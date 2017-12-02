using System;
using Firestorm.Engine;
using Xunit;

namespace Firestorm.Tests.Unit.Engine
{
    public class ConversionUtilityTests
    {
        [Fact]
        public void BoxedString_Unboxes()
        {
            object s1 = "my string";
            string s2 = ConversionUtility.ConvertValue<string>(s1);
            
            Assert.Same(s1, s2);
        }

        [Fact]
        public void NumberString_ConvertToInt_Parses()
        {
            var convertedValue = ConversionUtility.ConvertValue<int>("4321");

            Assert.Equal(4321, convertedValue);
        }

        [Fact]
        public void HugeNumberString_ConvertToInt_Fail()
        {
            Assert.Throws<OverflowException>(delegate {
                var convertedValue = ConversionUtility.ConvertValue<int>("87623458967234553");
            });
        }

        [Fact]
        public void HugeNumberString_ConvertToLong_Success()
        {
            var convertedValue = ConversionUtility.ConvertValue<long>("87623458967234553");
        }

        [Fact]
        public void DateString_ConvertToDateTime_Parses()
        {
            var convertedValue = ConversionUtility.ConvertValue<DateTime>("2002-04-02");

            Assert.Equal(new DateTime(2002, 04, 02), convertedValue);
        }

        [Fact]
        public void TrueString_ConvertToBoolean_Parses()
        {
            var convertedValue = ConversionUtility.ConvertValue<bool>("true");

            Assert.Equal(true, convertedValue);
        }

        [Fact]
        public void TrueString_ConvertToInt_Fail()
        {
            Assert.Throws<FormatException>(delegate {
                var convertedValue = ConversionUtility.ConvertValue<int>("true");
            });
        }

        [Fact]
        public void Null_ConvertToString_AlsoNull()
        {
            var convertedValue = ConversionUtility.ConvertValue<string>(null);

            Assert.Null(convertedValue);
        }

        [Fact]
        public void Null_ConvertToNullableInt_AlsoNull()
        {
            var convertedValue = ConversionUtility.ConvertValue<int?>(null);

            Assert.Null(convertedValue);
        }

        [Fact]
        public void Null_ConvertToGenericInt_ThrowsArgument()
        {
            Assert.Throws<ArgumentException>(delegate
            {
                var convertedValue = ConversionUtility.ConvertValue<int>(null);
            });
        }

        [Fact]
        public void Null_ConvertToReflectionInt_ThrowsArgument()
        {
            Assert.Throws<ArgumentException>(delegate
            {
                var convertedValue = ConversionUtility.ConvertValue(null, typeof(int));
            });
        }

        [Fact]
        public void Int_ConvertToNullable_Equal()
        {
            var convertedValue = ConversionUtility.ConvertValue<int?>(321);

            Assert.Equal(321, convertedValue);
        }

        [Fact]
        public void NullableInt_ConvertToInt_Equal()
        {
            var convertedValue = ConversionUtility.ConvertValue<int>(new int?(321));

            Assert.Equal(321, convertedValue);
        }

        [Fact]
        public void DateTime_ConvertToString_ParsesBack()
        {
            var dateTime = new DateTime(2004, 07, 21);
            var convertedValue = ConversionUtility.ConvertValue<string>(dateTime);
            var parsedBack = DateTime.Parse(convertedValue);

            Assert.Equal(dateTime, parsedBack);
        }

        [Fact]
        public void AnonymousObject_SameProperties_CleverConverts()
        {
            var obj = new { Name = "Fred", Age = 20 };
            var model = ConversionUtility.CleverConvertValue<TestModel>(obj);

            Assert.Equal(obj.Name, model.Name);
            Assert.Equal(obj.Age, model.Age);
        }

        [Fact]
        public void NestedAnonymousObject_SameProperties_CleverConverts()
        {
            var obj = new { Name = "Fred", Age = 20, Child = new { Name = "Fred's kid", Age = 3 } };
            var model = ConversionUtility.CleverConvertValue<TestModel>(obj);

            Assert.Equal(obj.Name, model.Name);
            Assert.Equal(obj.Age, model.Age);
            Assert.Equal(obj.Child.Name, model.Child.Name);
            Assert.Equal(obj.Child.Age, model.Child.Age);
        }

        [Fact]
        public void AnonymousArray_SameProperties_CleverConverts()
        {
            var arr = new[]
            {
                new { Name = "Fred", Age = 20 },
                new { Name = "Bill", Age = 43 },
                new { Name = "Ben", Age = 65 }
            };

            var model = ConversionUtility.CleverConvertValue<TestModel[]>(arr);

            Assert.Equal(arr[0].Name, model[0].Name);
            Assert.Equal(arr[1].Age, model[1].Age);
            Assert.Equal(arr[2].Name, model[2].Name);
            Assert.Equal(arr[2].Age, model[2].Age);
        }

        [Fact]
        public void StringArray_CleverConverts()
        {
            var arr = new[] { "Uno", "Dos", "Tres" };

            var model = ConversionUtility.CleverConvertValue<string[]>(arr);

            Assert.Equal(arr[0], model[0]);
            Assert.Equal(arr[1], model[1]);
            Assert.Equal(arr[2], model[2]);
        }

        private class TestModel
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public TestModel Child { get; set; }
        }
    }
}
