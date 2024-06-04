using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace UnitTestProject1
{
    [TestClass]
    public class MedicalDictionaryTests
    {
        private MedicalDictionary _dictionary;

        [TestInitialize]
        public void Setup()
        {
            _dictionary = new MedicalDictionary();
        }

        [TestMethod]
        public void TestAddTerm()
        {
            // Act
            _dictionary.AddTerm("пушкин", "литератор");

            // Assert
            string definition = _dictionary.SearchTerm("пушкин");
            Assert.AreEqual("Определение для 'пушкин': литератор", definition);
        }

        [TestMethod]
        public void TestSearchTerm()
        {
            // Arrange
            _dictionary.AddTerm("пушкин", "литератор");

            // Act
            string definition = _dictionary.SearchTerm("пушкин");

            // Assert
            Assert.AreEqual("Определение для 'пушкин': литератор", definition);
        }

        [TestMethod]
        public void TestDeleteTerm()
        {
            // Arrange
            _dictionary.AddTerm("пушкин", "литератор");

            // Act
            string result = _dictionary.DeleteTerm("пушкин");

            // Assert
            Assert.AreEqual("Термин успешно удален.", result);
            Assert.AreEqual("Термин не найден в словаре.", _dictionary.SearchTerm("пушкин"));
        }

        [TestMethod]
        public void TestDisplayAllTerms()
        {
            // Arrange
            _dictionary.AddTerm("пушкин", "литератор");

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                _dictionary.DisplayAllTerms();

                // Assert
                var result = sw.ToString().Trim();
                StringAssert.Contains(result, "[16] пушкин - литератор");
            }
        }
    }
}

