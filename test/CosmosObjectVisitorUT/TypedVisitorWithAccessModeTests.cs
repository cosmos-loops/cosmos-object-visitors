using Cosmos.Reflection.ObjectVisitors;
using CosmosObjectVisitorUT.Helpers;
using CosmosObjectVisitorUT.Model;
using Shouldly;
using Xunit;

namespace CosmosObjectVisitorUT
{
    [Trait("TypedVisitor", "TypedVisitorWithAccessMode")]
    public class TypedVisitorWithAccessModeTests : Prepare
    {
        [Fact]
        public void ToGetValueByTwoLevelPathTest()
        {
            var model = new NiceAct4();
            model.Name = "Alex";
            model.Age = 23;
            model.AddressModel = new NiceAct4B() {Address = "Moon"};

            var v = ObjectVisitor.Create(typeof(NiceAct4), model);

            v["Name"].ShouldBe("Alex");
            v["Age"].ShouldBe(23);

            v.GetValue<NiceAct4B>("AddressModel").Address.ShouldBe("Moon");
            v.GetValue("AddressModel.Address", AccessMode.Structured).ShouldBe("Moon");
            v.GetValue<string>("AddressModel.Address", AccessMode.Structured).ShouldBe("Moon");
        }

        [Fact]
        public void ToGetValueByThreeLevelPathTest()
        {
            var model = new NiceAct4();
            model.Name = "Alex";
            model.Age = 23;
            model.AddressModel = new NiceAct4B() {Address = "Moon"};
            model.AddressModel.Country = new NiceAct4C() {City = "Shanghai"};

            var v = ObjectVisitor.Create(typeof(NiceAct4), model);

            v["Name"].ShouldBe("Alex");
            v["Age"].ShouldBe(23);

            v.GetValue<NiceAct4B>("AddressModel").Address.ShouldBe("Moon");
            v.GetValue("AddressModel.Address", AccessMode.Structured).ShouldBe("Moon");
            v.GetValue<string>("AddressModel.Address", AccessMode.Structured).ShouldBe("Moon");

            v.GetValue<NiceAct4C>("AddressModel.Country", AccessMode.Structured).City.ShouldBe("Shanghai");
            v.GetValue("AddressModel.Country.City", AccessMode.Structured).ShouldBe("Shanghai");
            v.GetValue<string>("AddressModel.Country.City", AccessMode.Structured).ShouldBe("Shanghai");
        }

        [Fact]
        public void ToGetValueByTwoLevelPathWithNullValueTest()
        {
            var model = new NiceAct4();
            model.Name = "Alex";
            model.Age = 23;
            model.AddressModel = null;

            var v = ObjectVisitor.Create(typeof(NiceAct4), model);

            v["Name"].ShouldBe("Alex");
            v["Age"].ShouldBe(23);

            v.GetValue<NiceAct4B>("AddressModel")?.Address.ShouldBeNull();
            v.GetValue("AddressModel.Address", AccessMode.Structured).ShouldBeNull();
            v.GetValue<string>("AddressModel.Address", AccessMode.Structured).ShouldBeNull();
        }

        [Fact]
        public void ToGetValueByThreeLevelPathWithNullValueTest()
        {
            var model = new NiceAct4();
            model.Name = "Alex";
            model.Age = 23;
            model.AddressModel = null;

            var v = ObjectVisitor.Create(typeof(NiceAct4), model);

            v["Name"].ShouldBe("Alex");
            v["Age"].ShouldBe(23);

            v.GetValue<NiceAct4B>("AddressModel")?.Address.ShouldBeNull();
            v.GetValue("AddressModel.Address", AccessMode.Structured).ShouldBeNull();
            v.GetValue<string>("AddressModel.Address", AccessMode.Structured).ShouldBeNull();

            v.GetValue<NiceAct4C>("AddressModel.Country", AccessMode.Structured).ShouldBeNull();
            v.GetValue("AddressModel.Country", AccessMode.Structured).ShouldBeNull();
            v.GetValue<string>("AddressModel.Country", AccessMode.Structured).ShouldBeNull();

            v.GetValue<NiceAct4C>("AddressModel.Country", AccessMode.Structured)?.City.ShouldBeNull();
            v.GetValue("AddressModel.Country.City", AccessMode.Structured).ShouldBeNull();
            v.GetValue<string>("AddressModel.Country.City", AccessMode.Structured).ShouldBeNull();
        }

        [Fact]
        public void ToSetValueByTwoLevelPathTest()
        {
            var model = new NiceAct4();
            model.Name = "Alex";
            model.Age = 23;
            model.AddressModel = new NiceAct4B() {Address = "Moon"};

            var v = ObjectVisitor.Create(typeof(NiceAct4), model);

            v["Name"] = "Lewis";
            v["Age"] = 24;
            v["AddressModel.Address", AccessMode.Structured] = "Sun";

            v["Name"].ShouldBe("Lewis");
            v["Age"].ShouldBe(24);
            v["AddressModel.Address", AccessMode.Structured].ShouldBe("Sun");

            v.GetValue<NiceAct4B>("AddressModel").Address.ShouldBe("Sun");
            v.GetValue("AddressModel.Address", AccessMode.Structured).ShouldBe("Sun");
            v.GetValue<string>("AddressModel.Address", AccessMode.Structured).ShouldBe("Sun");
        }

        [Fact]
        public void ToSetValueByThreeLevelPathTest()
        {
            var model = new NiceAct4();
            model.Name = "Alex";
            model.Age = 23;
            model.AddressModel = new NiceAct4B() {Address = "Moon"};
            model.AddressModel.Country = new NiceAct4C() {City = "Shanghai"};

            var v = ObjectVisitor.Create(typeof(NiceAct4), model);

            v["Name"] = "Lewis";
            v["Age"] = 24;
            v["AddressModel.Address", AccessMode.Structured] = "Sun";
            v["AddressModel.Country.City", AccessMode.Structured] = "Beijing";

            v["Name"].ShouldBe("Lewis");
            v["Age"].ShouldBe(24);
            v["AddressModel.Address", AccessMode.Structured].ShouldBe("Sun");
            v["AddressModel.Country.City", AccessMode.Structured].ShouldBe("Beijing");

            v.GetValue<NiceAct4B>("AddressModel").Address.ShouldBe("Sun");
            v.GetValue("AddressModel.Address", AccessMode.Structured).ShouldBe("Sun");
            v.GetValue<string>("AddressModel.Address", AccessMode.Structured).ShouldBe("Sun");
            v.GetValue<NiceAct4C>("AddressModel.Country", AccessMode.Structured).City.ShouldBe("Beijing");
            v.GetValue("AddressModel.Country.City", AccessMode.Structured).ShouldBe("Beijing");
            v.GetValue<string>("AddressModel.Country.City", AccessMode.Structured).ShouldBe("Beijing");
        }

        [Fact]
        public void ToSetValueByTwoLevelPathWithNullValueTest()
        {
            var model = new NiceAct4();
            model.Name = "Alex";
            model.Age = 23;
            model.AddressModel = null;

            var v = ObjectVisitor.Create(typeof(NiceAct4), model);

            v["Name"].ShouldBe("Alex");
            v["Age"].ShouldBe(23);
            v["AddressModel"].ShouldBeNull();

            v["Name"] = "Lewis";
            v["Age"] = 24;
            v["AddressModel.Address", AccessMode.Structured] = "Moon";

            v["Name"].ShouldBe("Lewis");
            v["Age"].ShouldBe(24);

            v["AddressModel.Address", AccessMode.Structured].ShouldBe("Moon");

            v.GetValue<NiceAct4B>("AddressModel").Address.ShouldBe("Moon");
            v.GetValue<string>("AddressModel.Address").ShouldBeNull();
            v.GetValue<string>("AddressModel.Address", AccessMode.Structured).ShouldBe("Moon");
        }
        
        [Fact]
        public void ToSetValueByThreeLevelPathWithNullValueTest()
        {
            var model = new NiceAct4();
            model.Name = "Alex";
            model.Age = 23;
            model.AddressModel = null;

            var v = ObjectVisitor.Create(typeof(NiceAct4), model);

            v["Name"].ShouldBe("Alex");
            v["Age"].ShouldBe(23);
            v["AddressModel"].ShouldBeNull();

            v["Name"] = "Lewis";
            v["Age"] = 24;
            v["AddressModel.Country.City", AccessMode.Structured] = "Sun";

            v["Name"].ShouldBe("Lewis");
            v["Age"].ShouldBe(24);

            v["AddressModel.Country.City", AccessMode.Structured].ShouldBe("Sun");

            v.GetValue<NiceAct4B>("AddressModel").Country.City.ShouldBe("Sun");
            v.GetValue<NiceAct4C>("AddressModel.Country")?.City.ShouldBeNull();
            v.GetValue<NiceAct4C>("AddressModel.Country", AccessMode.Structured).City.ShouldBe("Sun");
            v.GetValue<string>("AddressModel.Country.City", AccessMode.Structured).ShouldBe("Sun");
        }
    }
}