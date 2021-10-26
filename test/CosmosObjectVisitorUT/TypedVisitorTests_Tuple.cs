using System;
using CosmosObjectVisitorUT.Helpers;
using CosmosStack.Reflection.ObjectVisitors;
using Shouldly;
using Xunit;

namespace CosmosObjectVisitorUT
{
    [Trait("TypedVisitor", "TypedVisitorForTuple")]
    public class TypedVisitorForTupleTests : Prepare
    {
        [Fact]
        public void DirectNormalTupleTest()
        {
            var model = new Tuple<int>(1);
            var v = ObjectVisitor.Create(model.GetType(), model);
            v.ShouldNotBeNull();

            v["Item1"].ShouldBe(1);

            v["Item1"] = 100;

            v["Item1"].ShouldBe(1);
        }

        [Fact]
        public void GenericNormalTupleTest()
        {
            var model = new Tuple<int>(1);
            var v = ObjectVisitor.Create(model);
            v.ShouldNotBeNull();

            v["Item1"].ShouldBe(1);

            v["Item1"] = 100;

            v["Item1"].ShouldBe(1);
        }

        [Fact]
        public void DirectValueTupleTest()
        {
            var model = (1, 1);
            var v = ObjectVisitor.Create(model.GetType(), model);

            v.ShouldNotBeNull();

            v["Item1"].ShouldBe(1);
            v["Item2"].ShouldBe(1);

            v["Item1"] = 100;
            v["Item2"] = 200;

            v["Item1"].ShouldBe(1);
            v["Item2"].ShouldBe(1);
        }

        [Fact]
        public void DirectValueTupleWithNameTest()
        {
            (int Num1, int Num2) model = (1, 1);
            var v = ObjectVisitor.Create(model.GetType(), model);

            v.ShouldNotBeNull();

            v["Item1"].ShouldBe(1);
            v["Item2"].ShouldBe(1);

            v["Item1"] = 100;
            v["Item2"] = 200;

            v["Item1"].ShouldBe(1);
            v["Item2"].ShouldBe(1);
        }

        [Fact]
        public void GenericValueTupleTest()
        {
            var model = (1, 1);
            var v = ObjectVisitor.Create(model);

            v.ShouldNotBeNull();

            v["Item1"].ShouldBe(1);
            v["Item2"].ShouldBe(1);

            v["Item1"] = 100;
            v["Item2"] = 200;

            v["Item1"].ShouldBe(1);
            v["Item2"].ShouldBe(1);
        }

        [Fact]
        public void GenericValueTupleWithNameTest()
        {
            (int Num1, int Num2) model = (1, 1);
            var v = ObjectVisitor.Create(model);

            v.ShouldNotBeNull();

            v["Item1"].ShouldBe(1);
            v["Item2"].ShouldBe(1);

            v["Item1"] = 100;
            v["Item2"] = 200;

            v["Item1"].ShouldBe(1);
            v["Item2"].ShouldBe(1);
        }

        [Fact]
        public void DirectShortcutValueTupleTest()
        {
            var model = (1, 1);
            var v = ObjectVisitor.Create(model.GetType(), model);

            v.ShouldNotBeNull();

            v["Item1"].ShouldBe(1);
            v["Item2"].ShouldBe(1);

            v["Item1"] = 100;
            v["Item2"] = 200;

            v["Item1"].ShouldBe(1);
            v["Item2"].ShouldBe(1);
        }

        [Fact]
        public void DirectShortcutValueTupleWithNameTest()
        {
            (int Num1, int Num2) model = (1, 1);
            var v = ObjectVisitor.Create(model.GetType(), model);

            v.ShouldNotBeNull();

            v["Item1"].ShouldBe(1);
            v["Item2"].ShouldBe(1);

            v["Item1"] = 100;
            v["Item2"] = 200;

            v["Item1"].ShouldBe(1);
            v["Item2"].ShouldBe(1);
        }

        [Fact]
        public void GenericShortcutValueTupleTest()
        {
            var model = (1, 1);
            var v = ObjectVisitor.Create(model);

            v.ShouldNotBeNull();

            v["Item1"].ShouldBe(1);
            v["Item2"].ShouldBe(1);

            v["Item1"] = 100;
            v["Item2"] = 200;

            v["Item1"].ShouldBe(1);
            v["Item2"].ShouldBe(1);
        }

        [Fact]
        public void GenericShortcutValueTupleWithNameTest()
        {
            (int Num1, int Num2) model = (1, 1);
            var v = ObjectVisitor.Create(model);

            v.ShouldNotBeNull();

            v["Item1"].ShouldBe(1);
            v["Item2"].ShouldBe(1);

            v["Item1"] = 100;
            v["Item2"] = 200;

            v["Item1"].ShouldBe(1);
            v["Item2"].ShouldBe(1);
        }
    }
}