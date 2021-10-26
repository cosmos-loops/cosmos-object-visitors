// using System;
// using System.Linq.Expressions;
// using CosmosObjectVisitorUT.Model;
// using Shouldly;
// using Xunit;
//
// namespace CosmosObjectVisitorUT.Helpers
// {
//     public class NiceTests
//     {
//         private string X<T>(Expression<Func<T, object>> selector)
//         {
//             return selector.ToString();
//         }
//
//         [Fact]
//         public void String1Tests()
//         {
//             var a = X<NiceAct>(x => x.Name);
//
//             a.ShouldBe("");
//         }
//
//         [Fact]
//         public void String2Tests()
//         {
//             var a = X<NiceAct4>(x => x.AddressModel.Country.City);
//
//             a.ShouldBe("");
//         }
//
//         [Fact]
//         public void String3Tests()
//         {
//             var a = X<NiceAct2>(x => x.SomeNiceActArray[1]);
//
//             a.ShouldBe("");
//         }
//
//         [Fact]
//         public void String4Tests()
//         {
//             var a = X<NiceAct2>(x => x.SomeNiceActList[2]);
//
//             a.ShouldBe("");
//         }
//     }
// }