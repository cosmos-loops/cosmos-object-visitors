using System.Collections.Generic;
using System.Dynamic;
using CosmosObjectVisitorUT.Helpers;
using Xunit;
using ObjectVisitor = Cosmos.Reflection.ObjectVisitors.ObjectVisitor;

namespace CosmosObjectVisitorUT
{
    [Trait("DynamicOperator", "动态类")]
    public class DynamicTest : Prepare
    {
        [Fact(DisplayName = "Dynamic/ExpandoObject 直接类型测试")]
        public void DirectTypeExpandoObjectTest()
        {
            dynamic sampleObject = new ExpandoObject();
            sampleObject.Name = "Nu";
            sampleObject.Age = 12;

            var v = ObjectVisitor.Create(typeof(ExpandoObject), (object) sampleObject);
            Assert.Equal("Nu", v.GetValue<string>("Name"));
            Assert.Equal(12, v.GetValue<int>("Age"));
        }

        [Fact(DisplayName = "Dynamic/ExpandoObject 泛型类型测试")]
        public void GenericTypeExpandoObjectTest()
        {
            dynamic sampleObject = new ExpandoObject();
            sampleObject.Name = "Nu";
            sampleObject.Age = 12;

            var v = ObjectVisitor.Create((ExpandoObject) sampleObject);
            Assert.Equal("Nu", v.GetValue<string>("Name"));
            Assert.Equal(12, v.GetValue<int>("Age"));
        }

        [Fact(DisplayName = "Dynamic/ExpandoObject Sub Factory 直接类型测试")]
        public void DirectTypeExpandoObjectFromSubFactoryTest()
        {
            dynamic sampleObject = new ExpandoObject();
            sampleObject.Name = "Nu";
            sampleObject.Age = 12;

            var v = ObjectVisitor.Dynamic.CreateForExpandoObject((ExpandoObject) sampleObject);
            Assert.Equal("Nu", v.GetValue<string>("Name"));
            Assert.Equal(12, v.GetValue<int>("Age"));
        }

        [Fact(DisplayName = "Dynamic/DynamicObject 直接类型测试")]
        public void DirectTypeDynamicObjectTest()
        {
            dynamic sampleObject = new DynamicModel();
            sampleObject.Name = "Nu";
            sampleObject.Age = 12;

            var v = ObjectVisitor.Create(typeof(DynamicModel), (DynamicModel) sampleObject);
            Assert.Equal("Nu", v.GetValue<string>("Name"));
            Assert.Equal(12, v.GetValue<int>("Age"));
        }

        [Fact(DisplayName = "Dynamic/DynamicObject 泛型类型测试")]
        public void GenericTypeDynamicObjectTest()
        {
            dynamic sampleObject = new DynamicModel();
            sampleObject.Name = "Nu";
            sampleObject.Age = 12;

            var v = ObjectVisitor.Create((DynamicModel) sampleObject);
            Assert.Equal("Nu", v.GetValue<string>("Name"));
            Assert.Equal(12, v.GetValue<int>("Age"));
        }

        [Fact(DisplayName = "Dynamic/DynamicObject Sub Factory 直接类型测试")]
        public void DirectTypeDynamicObjectFromSubFactoryTest()
        {
            dynamic sampleObject = new DynamicModel();
            sampleObject.Name = "Nu";
            sampleObject.Age = 12;

            var v = ObjectVisitor.Dynamic.CreateForDynamicObject(typeof(DynamicModel), (DynamicModel) sampleObject);
            Assert.Equal("Nu", v.GetValue<string>("Name"));
            Assert.Equal(12, v.GetValue<int>("Age"));
        }

        [Fact(DisplayName = "Dynamic/DynamicObject Sub Factory 泛型类型测试")]
        public void GenericTypeDynamicObjectFromSubFactoryTest()
        {
            dynamic sampleObject = new DynamicModel();
            sampleObject.Name = "Nu";
            sampleObject.Age = 12;

            var v = ObjectVisitor.Dynamic.CreateForDynamicObject((DynamicModel) sampleObject);
            Assert.Equal("Nu", v.GetValue<string>("Name"));
            Assert.Equal(12, v.GetValue<int>("Age"));
        }
    }

    public class DynamicModel : DynamicObject
    {
        private readonly Dictionary<string, object> _dictionary;

        public DynamicModel()
        {
            _dictionary = new();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return _dictionary.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dictionary[binder.Name] = value;
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            return _dictionary.TryGetValue((string) indexes[0], out result);
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            _dictionary[(string) indexes[0]] = value;
            return true;
        }
    }
}