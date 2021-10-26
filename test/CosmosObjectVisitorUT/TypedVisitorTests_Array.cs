// using System;
// using System.Collections.Generic;
// using Cosmos.Reflection.ObjectVisitors;
// using CosmosObjectVisitorUT.Helpers;
// using CosmosObjectVisitorUT.Model;
// using Shouldly;
// using Xunit;
//
// namespace CosmosObjectVisitorUT
// {
//     [Trait("TypedVisitor", "TypedVisitorForArray")]
//     public class TypedVisitorForArrayTests : Prepare
//     {
//         [Fact]
//         public void CollTest()
//         {
//             var models = new List<NiceAct>();
//             var v = ObjectVisitorFactory.Create(models);
//             
//             v.ShouldNotBeNull();
//         }
//
//         [Fact]
//         public void EnumerableTest()
//         {
//             IEnumerable<NiceAct> models = new List<NiceAct>();
//             var v = ObjectVisitorFactory.Create(models);
//             
//             v.ShouldNotBeNull();
//         }
//
//         [Fact]
//         public void ArrayTest()
//         {
//             var models = new NiceAct[3];
//             var v = ObjectVisitorFactory.Create(models);
//             
//             v.ShouldNotBeNull();
//         }
//
//         [Fact]
//         public void Array2dTest()
//         {
//             var models = new NiceAct[2, 2];
//             var v = ObjectVisitorFactory.Create(models);
//             
//             v.ShouldNotBeNull();
//         }
//     }
// }