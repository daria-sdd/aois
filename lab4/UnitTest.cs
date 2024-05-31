using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryTableTests
{
    [TestClass]
    public class BinaryTableTests
    {
        private BinaryTable binaryTable;

        [TestInitialize]
        public void Setup()
        {
            binaryTable = new BinaryTable();
        }

        [TestMethod]
        public void TestGenerateSumTable()
        {
            int[,] expectedSumTable = {
                { 0, 0, 0, 0, 0 },
                { 0, 0, 1, 1, 0 },
                { 0, 1, 0, 1, 0 },
                { 0, 1, 1, 0, 1 },
                { 1, 0, 0, 1, 0 },
                { 1, 0, 1, 0, 1 },
                { 1, 1, 0, 0, 1 },
                { 1, 1, 1, 1, 1 }
            };

            var actualSumTable = binaryTable.GetType()
                                            .GetField("tableSum", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                            .GetValue(binaryTable) as int[,];

            CollectionAssert.AreEqual(expectedSumTable, actualSumTable);
        }

        [TestMethod]
        public void TestGenerateShiftTable()
        {
            string[,] expectedShiftTable = {
                { "0", "0", "0", "0", "1", "0", "0", "1" },
                { "0", "0", "0", "1", "1", "0", "1", "0" },
                { "0", "0", "1", "0", "1", "0", "1", "1" },
                { "0", "0", "1", "1", "1", "1", "0", "0" },
                { "0", "1", "0", "0", "1", "1", "0", "1" },
                { "0", "1", "0", "1", "1", "1", "1", "0" },
                { "0", "1", "1", "0", "1", "1", "1", "1" },
                { "0", "1", "1", "1", "-", "-", "-", "-" },
                { "1", "0", "0", "0", "-", "-", "-", "-" },
                { "1", "0", "0", "1", "-", "-", "-", "-" },
                { "1", "0", "1", "0", "-", "-", "-", "-" },
                { "1", "0", "1", "1", "-", "-", "-", "-" },
                { "1", "1", "0", "0", "-", "-", "-", "-" },
                { "1", "1", "0", "1", "-", "-", "-", "-" },
                { "1", "1", "1", "0", "-", "-", "-", "-" },
                { "1", "1", "1", "1", "-", "-", "-", "-" }
            };

            var actualShiftTable = binaryTable.GetType()
                                               .GetField("tableN", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                               .GetValue(binaryTable) as string[,];

            CollectionAssert.AreEqual(expectedShiftTable, actualShiftTable);
        }

        [TestMethod]
        public void TestGetSDNFSumTable()
        {
            char[] variables = { 'a', 'b', 'c' };
            string expectedSumSDNF = "!a & !b & c | !a & b & !c | a & !b & !c | a & b & c";
            string sdnfSum = binaryTable.GetType()
                                        .GetMethod("GetSDNF", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                        .Invoke(binaryTable, new object[] { binaryTable.GetType().GetField("tableSum", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(binaryTable), 3, variables }) as string;

            Assert.AreEqual(expectedSumSDNF, sdnfSum);
        }

        [TestMethod]
        public void TestGetSDNFCarriesTable()
        {
            char[] variables = { 'a', 'b', 'c' };
            string expectedCarrySDNF = "!a & b & c | a & !b & c | a & b & !c | a & b & c";
            string sdnfCarry = binaryTable.GetType()
                                          .GetMethod("GetSDNF", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                          .Invoke(binaryTable, new object[] { binaryTable.GetType().GetField("tableSum", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(binaryTable), 4, variables }) as string;

            Assert.AreEqual(expectedCarrySDNF, sdnfCarry);
        }

        [TestMethod]
        public void TestMinimizeSDNF()
        {
            string sdnfSum = "!a & !b & c | !a & b & !c | a & !b & !c | a & b & c";
            string expectedMinimizedSumSDNF = "!a & !b & c | !a & b & !c | a & !b & !c | a & b & c";
            string minimizedSumSDNF = binaryTable.MinimizeSDNF(sdnfSum);
            Assert.AreEqual(expectedMinimizedSumSDNF, minimizedSumSDNF);

            string sdnfCarry = "!a & b & c | a & !b & c | a & b & !c | a & b & c";
            string expectedMinimizedCarrySDNF = "b & c | a & c | a & b";
            string minimizedCarrySDNF = binaryTable.MinimizeSDNF(sdnfCarry);
            Assert.AreEqual(expectedMinimizedCarrySDNF, minimizedCarrySDNF);
        }

        [TestMethod]
        public void TestGetSDNFShiftTable()
        {
            int[,] shiftTableInt = binaryTable.GetType()
                                              .GetMethod("ConvertShiftTableToInt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                              .Invoke(binaryTable, null) as int[,];

            char[] shiftVariables = { 'a', 'b', 'c', 'd' };

            string expectedSDNF4 = "!a & !b & !c & !d | !a & !b & !c & d | !a & !b & c & !d | !a & !b & c & d | !a & b & !c & !d | !a & b & !c & d | !a & b & c & !d";
            string sdnf4 = binaryTable.GetType()
                                      .GetMethod("GetSDNF", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                      .Invoke(binaryTable, new object[] { shiftTableInt, 4, shiftVariables }) as string;
            Assert.AreEqual(expectedSDNF4, sdnf4);

            string expectedMinimizedSDNF4 = "!a & !b | !a & !c | !a & !d";
            string minimizedSDNF4 = binaryTable.MinimizeSDNF(sdnf4);
            Assert.AreEqual(expectedMinimizedSDNF4, minimizedSDNF4);

            string expectedSDNF5 = "!a & !b & c & d | !a & b & !c & !d | !a & b & !c & d | !a & b & c & !d";
            string sdnf5 = binaryTable.GetType()
                                      .GetMethod("GetSDNF", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                      .Invoke(binaryTable, new object[] { shiftTableInt, 5, shiftVariables }) as string;
            Assert.AreEqual(expectedSDNF5, sdnf5);

            string expectedMinimizedSDNF5 = "!a & b & !c | !a & b & !d";
            string minimizedSDNF5 = binaryTable.MinimizeSDNF(sdnf5);
            Assert.AreEqual(expectedMinimizedSDNF5, minimizedSDNF5);

            string expectedSDNF6 = "!a & !b & !c & d | !a & !b & c & !d | !a & b & !c & d | !a & b & c & !d";
            string sdnf6 = binaryTable.GetType()
                                      .GetMethod("GetSDNF", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                      .Invoke(binaryTable, new object[] { shiftTableInt, 6, shiftVariables }) as string;
            Assert.AreEqual(expectedSDNF6, sdnf6);

            string expectedMinimizedSDNF6 = "!a & !c & d | !a & c & !d";
            string minimizedSDNF6 = binaryTable.MinimizeSDNF(sdnf6);
            Assert.AreEqual(expectedMinimizedSDNF6, minimizedSDNF6);

            string expectedSDNF7 = "!a & !b & !c & !d | !a & !b & c & !d | !a & b & !c & !d | !a & b & c & !d";
            string sdnf7 = binaryTable.GetType()
                                      .GetMethod("GetSDNF", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                      .Invoke(binaryTable, new object[] { shiftTableInt, 7, shiftVariables }) as string;
            Assert.AreEqual(expectedSDNF7, sdnf7);

            string expectedMinimizedSDNF7 = "!a & !d";
            string minimizedSDNF7 = binaryTable.MinimizeSDNF(sdnf7);
            Assert.AreEqual(expectedMinimizedSDNF7, minimizedSDNF7);
        }
    }
}
