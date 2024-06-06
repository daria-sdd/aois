using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TableGeneratorTests
{
    [TestClass]
    public class TableGeneratorTests
    {
        [TestMethod]
        public void FindWordTest()
        {
            string result = "0000000000000000";
            TableGenerator matrix = new TableGenerator(16);
            matrix.ChangeWord(result, 5);
            Assert.AreEqual(result, matrix.FindWord(5));
        }

        [TestMethod]
        public void FindAddressWordTest()
        {
            string result = "0000000000000000";
            TableGenerator matrix = new TableGenerator(16);
            for (int i = 0; i < 16; i++)
            {
                matrix.ChangeWord(result, i);
            }
            Assert.AreEqual(result, matrix.FindAddressWord(5));
        }

        [TestMethod]
        public void ConstantZeroTest()
        {
            string expected = "0000000000000000";
            TableGenerator matrix = new TableGenerator(16);
            matrix.ConstantZero(2);
            Assert.AreEqual(expected, matrix.FindWord(2));
        }

        [TestMethod]
        public void ConstantOneTest()
        {
            string expected = "1111111111111111";
            TableGenerator matrix = new TableGenerator(16);
            matrix.ConstantOne(2);
            Assert.AreEqual(expected, matrix.FindWord(2));
        }

        [TestMethod]
        public void ReturnSecondWordTest()
        {
            string expected = "1010001001100101";
            string str1 = "1001001001001001";
            string str2 = "1010001001100101";
            TableGenerator matrix = new TableGenerator(16);
            matrix.ChangeWord(str1, 0);
            matrix.ChangeWord(str2, 1);
            matrix.ReturnSecondWord(1, 2);
            Assert.AreEqual(expected, matrix.FindWord(2));
        }

        [TestMethod]
        public void ReverseSecondWordTest()
        {
            string expected = "0101110110011010";
            string str1 = "1001001001001001";
            string str2 = "1010001001100101";
            TableGenerator matrix = new TableGenerator(16);
            matrix.ChangeWord(str1, 0);
            matrix.ChangeWord(str2, 1);
            matrix.ReverseSecondWord(1, 2);
            Assert.AreEqual(expected, matrix.FindWord(2));
        }

        [TestMethod]
        public void MatchFilterTest()
        {
            string key = "111";
            List<string> listToCheck = new List<string>
            {
                "1110000000000000",
                "1111000000000000",
                "0001110000000000",
                "1111110000000000",
                "0000001110000000",
                "1110001110000000"
            };
            List<string> expected = new List<string>
            {
                "1110000000000000",
                "1111000000000000",
                "1111110000000000",
                "1110001110000000"
            };

            TableGenerator matrix = new TableGenerator(16);
            List<string> result = matrix.MatchFilter(key, listToCheck);

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SummaOfFieldsTest()
        {
            string result = "1110010000100011";
            string str1 = "1010101010101010";
            string str2 = "1110010000100000";
            TableGenerator matrix = new TableGenerator(16);
            matrix.ChangeWord(str1, 0);
            matrix.ChangeWord(str2, 1);
            Assert.AreEqual(str1, matrix.FindWord(0));
            Assert.AreEqual(str2, matrix.FindWord(1));
            matrix.SummaOfFields();
            Assert.AreEqual(str1, matrix.FindWord(0));
            Assert.AreEqual(result, matrix.FindWord(1));
        }
    }
}
