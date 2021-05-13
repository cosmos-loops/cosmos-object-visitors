using System.Collections.Generic;
using System.Dynamic;
using Cosmos.Reflection.ObjectVisitors;
using CosmosObjectVisitorUT.Helpers;
using Xunit;

namespace CosmosObjectVisitorUT
{
    [Trait("DynamicOperator", "动态类")]
    public class DynamicTest : Prepare
    {
        [Fact(DisplayName = "Dynamic/ExpandoObject 测试")]
        public void ExpandoObjectTest()
        {
            dynamic sampleObject = new ExpandoObject();
            sampleObject.Name = "Nu";
            sampleObject.Age = 12;

            var v = ObjectVisitorFactory.Create(typeof(ExpandoObject), (ExpandoObject) sampleObject);
            Assert.Equal("Nu", v.GetValue<string>("Name"));
            Assert.Equal(12, v.GetValue<int>("Age"));
        }

        [Fact(DisplayName = "Dynamic/DynamicObject 测试")]
        public void DynamicObjectTest()
        {
            dynamic sampleObject = new DynamicModel();
            sampleObject.Name = "Nu";
            sampleObject.Age = 12;

            var v = ObjectVisitorFactory.Create(typeof(DynamicModel), (DynamicModel) sampleObject);
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