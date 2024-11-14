﻿using SF3.Tests;
using static CommonLib.Utils.Compression;

namespace CommonLib.Tests.Utils {
    [TestClass]
    public class CompressionTests {
        [TestMethod]
        public void CompressThenDecompressStringsOfVariousLengthsReturnsOriginalString() {
            var testCases = new TestCase[]{
                new TestCase(""),
                new TestCase("H"),
                new TestCase("He"),
                new TestCase("Hel"),
                new TestCase("Hell"),
                new TestCase("Hello"),
                new TestCase("Hello,"),
                new TestCase("Hello, "),
                new TestCase("Hello, w"),
                new TestCase("Hello, wo"),
                new TestCase("Hello, wor"),
                new TestCase("Hello, worl"),
                new TestCase("Hello, world"),
                new TestCase("Hello, world!"),
                new TestCase("Hello, world! "),
                new TestCase("Hello, world! H"),
                new TestCase("Hello, world! Ho"),
                new TestCase("Hello, world! How"),
                new TestCase("Hello, world! How "),
                new TestCase("Hello, world! How y"),
                new TestCase("Hello, world! How ya"),
                new TestCase("Hello, world! How ya "),
                new TestCase("Hello, world! How ya d"),
                new TestCase("Hello, world! How ya do"),
                new TestCase("Hello, world! How ya doi"),
                new TestCase("Hello, world! How ya doin"),
                new TestCase("Hello, world! How ya doin?"),
            };

            TestCase.Run(testCases, testCase => {
                string originalString = testCase.Name;
                byte[] originalBytes = System.Text.Encoding.UTF8.GetBytes(originalString);

                var compressedBytes = Compress(originalBytes);
                var decompressedBytes = Decompress(compressedBytes);

                var resultString = System.Text.Encoding.UTF8.GetString(decompressedBytes);
                Assert.AreEqual(originalString, resultString);
                throw new Exception("✔️");
            });
        }
    }
}