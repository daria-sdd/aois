using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NumberTests
{
    [TestClass]
    public class NumberTests
    {
        [TestMethod]
        public void TestInput()
        {
            Number number = new Number(0);
            number.Input(5);
            Assert.AreEqual(5, number.Output());
        }

        [TestMethod]
        public void TestOutput()
        {
            Number number = new Number(7);
            Assert.AreEqual(7, number.Output());
        }

        [TestMethod]
        public void TestToBinary()
        {
            Number number = new Number(5);
            Assert.AreEqual("0000101", number.ToBinary());
        }

        [TestMethod]
        public void TestToDirectCode_Positive()
        {
            Number number = new Number(5);
            Assert.AreEqual("00000101", number.ToDirectCode());
        }

        [TestMethod]
        public void TestToDirectCode_Negative()
        {
            Number number = new Number(-5);
            Assert.AreEqual("10000101", number.ToDirectCode());
        }

        [TestMethod]
        public void TestToReverseCode_Positive()
        {
            Number number = new Number(5);
            Assert.AreEqual("00000101", number.ToReverseCode());
        }

        [TestMethod]
        public void TestToReverseCode_Negative()
        {
            Number number = new Number(-5);
            Assert.AreEqual("11111010", number.ToReverseCode());
        }

        [TestMethod]
        public void TestToAdditionalCode_Positive()
        {
            Number number = new Number(5);
            Assert.AreEqual("00000101", number.ToAdditionalCode());
        }

        [TestMethod]
        public void TestToAdditionalCode_Negative()
        {
            Number number = new Number(-5);
            Assert.AreEqual("11111011", number.ToAdditionalCode());
        }

        [TestMethod]
        public void TestAdd()
        {
            Number a = new Number(-5);
            Number b = new Number(4);
            Number result = Number.Add(a, b);
            Assert.AreEqual(-1, result.Output());
            Assert.AreEqual("10000001", result.ToDirectCode());
            Assert.AreEqual("11111110", result.ToReverseCode());
            Assert.AreEqual("11111111", result.ToAdditionalCode());
        }

        [TestMethod]
        public void TestSubtract()
        {
            Number a = new Number(-5);
            Number b = new Number(4);
            Number result = Number.Subtract(a, b);
            Assert.AreEqual(-9, result.Output());
            Assert.AreEqual("10001001", result.ToDirectCode());
            Assert.AreEqual("11110110", result.ToReverseCode());
            Assert.AreEqual("11110111", result.ToAdditionalCode());
        }

        [TestMethod]
        public void TestMultiply()
        {
            Number a = new Number(-5);
            Number b = new Number(4);
            Number result = Number.Multiply(a, b);
            Assert.AreEqual(-20, result.Output());
            Assert.AreEqual("10010100", result.ToDirectCode());
            Assert.AreEqual("11101011", result.ToReverseCode());
            Assert.AreEqual("11101100", result.ToAdditionalCode());
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void TestDivideByZero()
        {
            Number a = new Number(5);
            Number b = new Number(0);
            Number result = Number.Divide(a, b);
        }

        [TestMethod]
        public void TestDivide()
        {
            Number a = new Number(-5);
            Number b = new Number(4);
            Number result = Number.Divide(a, b);
            Assert.AreEqual(-1, result.Output());
            Assert.AreEqual("10000001", result.ToDirectCode());
            Assert.AreEqual("11111110", result.ToReverseCode());
            Assert.AreEqual("11111111", result.ToAdditionalCode());
        }

        [TestMethod]
        public void TestBinaryToDecimal()
        {
            string binary = "101";
            int result = Number.BinaryToDecimal(binary);
            Assert.AreEqual(5, result);
        }
    }

    [TestClass]
    public class FloatingPointNumberTests
    {
        [TestMethod]
        public void TestInput()
        {
            FloatingPointNumber number = new FloatingPointNumber(0.0f);
            number.Input(5.5f);
            Assert.AreEqual(5.5f, number.Output());
        }

        [TestMethod]
        public void TestOutput()
        {
            FloatingPointNumber number = new FloatingPointNumber(7.3f);
            Assert.AreEqual(7.3f, number.Output());
        }

        [TestMethod]
        public void TestToBinary()
        {
            FloatingPointNumber number = new FloatingPointNumber(5.5f);
            Assert.AreEqual("01000000101100000000000000000000", number.ToBinary());
        }

        [TestMethod]
        public void TestAdd()
        {
            FloatingPointNumber a = new FloatingPointNumber(5.5f);
            FloatingPointNumber b = new FloatingPointNumber(4.5f);
            FloatingPointNumber result = FloatingPointNumber.Add(a, b);
            Assert.AreEqual(10.0f, result.Output());
        }

        [TestMethod]
        public void TestAddPositive()
        {
            FloatingPointNumber a = new FloatingPointNumber(1.5f);
            FloatingPointNumber b = new FloatingPointNumber(2.75f);
            FloatingPointNumber result = FloatingPointNumber.Add(a, b);
            Assert.AreEqual(4.25f, result.Output());
        }

        [TestMethod]
        public void TestAddPositive_WithDifferentExponents()
        {
            FloatingPointNumber a = new FloatingPointNumber(1.5f);
            FloatingPointNumber b = new FloatingPointNumber(0.375f);
            FloatingPointNumber result = FloatingPointNumber.Add(a, b);
            Assert.AreEqual(1.875f, result.Output());
        }

        [TestMethod]
        public void TestAddPositive_Normalization()
        {
            FloatingPointNumber a = new FloatingPointNumber(1.0f);
            FloatingPointNumber b = new FloatingPointNumber(1.0f);
            FloatingPointNumber result = FloatingPointNumber.Add(a, b);
            Assert.AreEqual(2.0f, result.Output());
        }
    }
}





