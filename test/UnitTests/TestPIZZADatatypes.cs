using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIZZA.Core;

namespace Tests
{
    [TestClass]
    public class TestPIZZADatatypes
    {
        [TestMethod]
        public void TestPIZZAInt()
        {
            var i = new PIZZAInt()
            {
                Value = 257,
            };

            var bytes = i.GetBytes();
            var otherI = PIZZAInt.FromBytes(bytes);

            Assert.AreEqual(i.Value, otherI.Value);
            Assert.AreEqual(257, otherI.Value);
        }

        [TestMethod]
        public void TestPIZZAInt3()
        {
            var i = new PIZZAInt3()
            {
                Value = 333000,
            };

            var bytes = i.GetBytes();
            var otherI = PIZZAInt3.FromBytes(bytes);

            Assert.AreEqual(i.Value, otherI.Value);
            Assert.AreEqual(333000, otherI.Value);
        }

        [TestMethod]
        public void TestPIZZAInt5()
        {
            var i = new PIZZAInt5()
            {
                Value = 999999999,
            };

            var bytes = i.GetBytes();
            var otherI = PIZZAInt5.FromBytes(bytes);

            Assert.AreEqual(i.Value, otherI.Value);
            Assert.AreEqual(999999999, otherI.Value);
        }

        [TestMethod]
        public void TestPIZZAString()
        {
            var i = new PIZZAString()
            {
                Value = "lollollol",
            };

            var bytes = i.GetBytes();
            var otherI = PIZZAString.FromBytes(bytes);

            Assert.AreEqual(i.Value, otherI.Value);
            Assert.AreEqual(9, otherI.Length.Value);
        }
    }
}
