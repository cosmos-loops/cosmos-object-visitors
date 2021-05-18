using System;
using System.Collections.Generic;
using Cosmos.Reflection.ObjectVisitors;
using CosmosObjectVisitorUT.Helpers;
using CosmosObjectVisitorUT.Model;
using Shouldly;
using Xunit;

namespace CosmosObjectVisitorUT
{
    [Trait("TypedVisitor", "TypedVisitorForStruct")]
    public class TypedVisitorForStructTests : Prepare
    {
        [Fact]
        public void ToGetValueFromStructTest()
        {
            var model = new NiceStruct("Alex", 23, DateTime.Today, Country.China, true);
            var v = ObjectVisitorFactory.Create(model);

            v.ShouldNotBeNull();

            v["Name"].ShouldBe("Alex");
            v["Age"].ShouldBe(23);
            v["Birthday"].ShouldBe(DateTime.Today);
            v["Country"].ShouldBe(Country.China);
        }

        [Fact]
        public void ToSetValueIntoStructTest()
        {
            var model = new NiceStruct("Alex", 23, DateTime.Today, Country.China, true);
            var v = ObjectVisitorFactory.Create(model);

            v.ShouldNotBeNull();

            v["Name"].ShouldBe("Alex");

            v["Name"] = "Lewis";

            v["Name"].ShouldBe("Lewis");
        }
    }
}