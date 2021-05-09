// #if !NET452
// using System.Dynamic;
// using Cosmos.Reflection.Core.Builder;
// using CosmosProwessUT.OvUT.Helpers;
// using Xunit;
//
// namespace CosmosObjectVisitorUT.Model
// {
//     [Trait("DictOperator", "动态类")]
//     public class DictDynamicTest: Prepare
//     {
//         [Fact(DisplayName = "Dynamic/ExpandoObject 测试")]
//         public void DynamicObjTest()
//         {
//             dynamic sampleObject = new ExpandoObject();
//             sampleObject.Name = "Nu";
//             sampleObject.Age = 12;
//
//             var v = PrecisionDictOperator.CreateFromType(typeof(ExpandoObject));
//             v.SetObjInstance(sampleObject);
//             
//             Assert.Equal("Nu", v["Name"]);
//             Assert.Equal(12, v.Get<int>("Age"));
//         }
//     }
// }
// #endif