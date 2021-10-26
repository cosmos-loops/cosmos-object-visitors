using System;
using CosmosObjectVisitorUT.Helpers;
using CosmosStack.Reflection.ObjectVisitors;
using Xunit;

namespace CosmosObjectVisitorUT
{
    [Trait("AnonymousOperator", "匿名类")]
    public class AnonymousTest : Prepare
    {
        [Fact]
        public void AnonymousTypeGettingTest()
        {
            var anonymousObj = new { Name = "Nu", Age = 12 };

            var v = ObjectVisitor.Create(anonymousObj);
            Assert.Equal("Nu", v.GetValue<string>("Name"));
            Assert.Equal("Nu", v.GetValue("Name"));
            Assert.Equal(12, v.GetValue<int>("Age"));
            Assert.Equal(12, v.GetValue("Age"));
        }

        [Fact]
        public void AnonymousTypeSettingTest()
        {
            var anonymousObj = new { Name = "Nu", Age = 12 };
            var options = ObjectVisitorOptions.Default.With(x => x.SilenceIfNotWritable = false);
            var v = ObjectVisitor.Create(anonymousObj, options);
            Assert.Throws<InvalidOperationException>(() => v.SetValue("Name", "Da"));
        }
    }
}