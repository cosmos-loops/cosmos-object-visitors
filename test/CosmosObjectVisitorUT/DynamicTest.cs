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
        public void DynamicObjTest()
        {
            dynamic sampleObject = new ExpandoObject();
            sampleObject.Name = "Nu";
            sampleObject.Age = 12;

            var v = ObjectVisitorFactory.Create(typeof(ExpandoObject), (object) sampleObject);
            Assert.Equal("Nu", v.GetValue<string>("Name"));
            Assert.Equal(12, v.GetValue<int>("Age"));
        }
    }
}