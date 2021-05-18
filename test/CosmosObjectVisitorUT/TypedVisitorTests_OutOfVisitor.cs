using System;
using Cosmos.Reflection.ObjectVisitors;
using CosmosObjectVisitorUT.Helpers;
using CosmosObjectVisitorUT.Model;
using Shouldly;
using Xunit;

namespace CosmosObjectVisitorUT
{
    [Trait("TypedVisitor", "TypedVisitor with out of visitor")]
    public class TypedVisitorOutOvVisitorTests : Prepare
    {
        [Fact]
        public void UpdateValueOutOfVisitorTest()
        {
            var act = new NiceAct()
            {
                Name = "Hu",
                Age = 22,
                Country = Country.China,
                Birthday = DateTime.Today
            };

            var v = act.ToVisitor();

            Assert.Equal("Hu", v.GetValue<string>("Name"));
            Assert.Equal(22, v.GetValue<int>("Age"));
            Assert.Equal(Country.China, v.GetValue<Country>("Country"));
            Assert.Equal(DateTime.Today, v.GetValue<DateTime>("Birthday"));
            Assert.False(v.GetValue<bool>("IsValid"));

            act.Name = "Du";
            act.Age = 55;
            act.Country = Country.USA;
            act.Birthday = DateTime.Today.AddDays(-1);
            act.IsValid = true;

            Assert.Equal("Du", v.GetValue<string>("Name"));
            Assert.Equal(55, v.GetValue<int>("Age"));
            Assert.Equal(Country.USA, v.GetValue<Country>("Country"));
            Assert.Equal(DateTime.Today.AddDays(-1), v.GetValue<DateTime>("Birthday"));
            Assert.True(v.GetValue<bool>("IsValid"));
        }

        [Fact]
        public void UpdateValueOutOfVisitorByTwoLevelPathTest()
        {
            var model = new NiceAct4();
            model.Name = "Alex";
            model.Age = 23;
            model.AddressModel = new NiceAct4B() {Address = "Moon"};

            var v = ObjectVisitorFactory.Create(typeof(NiceAct4), model);

            v["Name"].ShouldBe("Alex");
            v["Age"].ShouldBe(23);
            v["AddressModel.Address", AccessMode.Structured].ShouldBe("Moon");

            model.Name = "Lewis";
            model.Age = 22;
            model.AddressModel.Address = "Sun";

            v["Name"].ShouldBe("Lewis");
            v["Age"].ShouldBe(22);
            v["AddressModel.Address", AccessMode.Structured].ShouldBe("Sun");

            model.Name = "Daniel";
            model.Age = 21;
            model.AddressModel = new NiceAct4B() {Address = "Earth"};

            v["Name"].ShouldBe("Daniel");
            v["Age"].ShouldBe(21);
            v["AddressModel.Address", AccessMode.Structured].ShouldBe("Earth");
        }

        [Fact]
        public void UpdateValueOutOfVisitorByThreeLevelPathTest()
        {
            var model = new NiceAct4();
            model.Name = "Alex";
            model.Age = 23;
            model.AddressModel = new NiceAct4B() {Address = "Moon"};
            model.AddressModel.Country = new NiceAct4C() {City = "Shanghai"};

            var v = ObjectVisitorFactory.Create(typeof(NiceAct4), model);

            v["Name"].ShouldBe("Alex");
            v["Age"].ShouldBe(23);
            v["AddressModel.Address", AccessMode.Structured].ShouldBe("Moon");
            v["AddressModel.Country.City", AccessMode.Structured].ShouldBe("Shanghai");

            model.Name = "Daniel";
            model.Age = 21;
            model.AddressModel = new NiceAct4B() {Address = "Earth"};
            model.AddressModel.Country = new NiceAct4C {City = "Beijing"};

            v["Name"].ShouldBe("Daniel");
            v["Age"].ShouldBe(21);
            v["AddressModel.Address", AccessMode.Structured].ShouldBe("Earth");
            v["AddressModel.Country.City", AccessMode.Structured].ShouldBe("Beijing");

           
            model.Name = "Riku";
            model.Age = 20;
            model.AddressModel.Country = new NiceAct4C {City = "Nanjing"};
            model.AddressModel = new NiceAct4B() {Address = "Mars"};

            v["Name"].ShouldBe("Riku");
            v["Age"].ShouldBe(20);
            v["AddressModel.Address", AccessMode.Structured].ShouldBe("Mars");
            v["AddressModel.Country.City", AccessMode.Structured].ShouldBe(null);
        }
    }
}