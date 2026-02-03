using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.Tests
{
    using NUnit.Framework;
    using System.IO;
    using UnityEngine;
    using HexTecGames.Basics;

    public class FileManagerTests
    {
        private string tempDir;

        [SetUp]
        public void Setup()
        {
            tempDir = Path.Combine(Path.GetTempPath(), "FileManagerTests_" + Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);
        }

        [TearDown]
        public void Teardown()
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, true);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // JSON TESTS
        // ─────────────────────────────────────────────────────────────────────────────

        [Test]
        public void SaveJSON_And_LoadJSON_Works()
        {
            var data = new TestData { Name = "Test", Value = 42 };
            string fileName = "test.json";

            FileManager.SaveJSON(data, tempDir, fileName, prettyPrint: true);
            Assert.IsTrue(File.Exists(Path.Combine(tempDir, fileName)));

            var loaded = FileManager.LoadJSON<TestData>(tempDir, fileName);
            Assert.IsNotNull(loaded);
            Assert.AreEqual(data, loaded);
        }

        [Test]
        public void LoadJSON_ReturnsDefault_WhenMissing()
        {
            var result = FileManager.LoadJSON<TestData>(tempDir, "missing.json");
            Assert.IsNull(result);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // XML TESTS
        // ─────────────────────────────────────────────────────────────────────────────

        [Test]
        public void SaveXML_And_LoadXML_Works()
        {
            var data = new TestData { Name = "XMLTest", Value = 99 };
            string fileName = "test.xml";

            FileManager.SaveXML(data, tempDir, fileName);
            Assert.IsTrue(File.Exists(Path.Combine(tempDir, fileName)));

            var loaded = FileManager.LoadXML<TestData>(tempDir, fileName);

            Assert.IsNotNull(loaded);
            Assert.AreEqual(data, loaded);
        }

        [Test]
        public void LoadXML_ReturnsDefault_WhenMissing()
        {
            var result = FileManager.LoadXML<TestData>(tempDir, "missing.xml");
            Assert.IsNull(result);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // FILE IO TESTS
        // ─────────────────────────────────────────────────────────────────────────────

        [Test]
        public void WriteToFile_And_ReadFile_Works()
        {
            string fileName = "textfile";
            var lines = new List<string> { "A", "B", "C" };

            FileManager.WriteToFile(tempDir, fileName, lines);

            var read = FileManager.ReadFile(tempDir, fileName);

            Assert.IsNotNull(read);
            Assert.AreEqual(lines.Count, read.Count);
            Assert.AreEqual(lines[0], read[0]);
        }

        [Test]
        public void DeleteFile_RemovesFile()
        {
            string path = Path.Combine(tempDir, "delete.txt");
            File.WriteAllText(path, "test");

            Assert.IsTrue(File.Exists(path));

            FileManager.DeleteFile(path);

            Assert.IsFalse(File.Exists(path));
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // UNIQUE NAME TEST
        // ─────────────────────────────────────────────────────────────────────────────

        [Test]
        public void GenerateUniqueFileName_AppendsSuffix()
        {
            string baseFile = Path.Combine(tempDir, "file.txt");

            File.WriteAllText(baseFile, "1");
            File.WriteAllText(baseFile.Replace("file", "file_2"), "2");

            string unique = FileManager.GenerateUniqueFileName(baseFile);

            Assert.IsTrue(unique.EndsWith("_3.txt"));
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // JSONALL TESTS
        // ─────────────────────────────────────────────────────────────────────────────

        [Test]
        public void LoadJSONAll_ReturnsAllFiles()
        {
            var data1 = new TestData { Name = "A", Value = 1 };
            var data2 = new TestData { Name = "B", Value = 2 };

            FileManager.SaveJSON(data1, tempDir, "one.json");
            FileManager.SaveJSON(data2, tempDir, "two.json");

            var results = FileManager.LoadJSONAll<TestData>(tempDir);

            Assert.AreEqual(2, results.Count);
            Assert.Contains(data1, results);
            Assert.Contains(data2, results);
        }

        [Test]
        public void LoadJSONAll_ReturnsEmptyList_WhenDirectoryMissing()
        {
            string missingDir = Path.Combine(tempDir, "DoesNotExist");

            var results = FileManager.LoadJSONAll<TestData>(missingDir);

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // XMLALL TESTS
        // ─────────────────────────────────────────────────────────────────────────────

        [Test]
        public void LoadXMLAll_ReturnsAllFiles()
        {
            var data1 = new TestData { Name = "X", Value = 10 };
            var data2 = new TestData { Name = "Y", Value = 20 };

            FileManager.SaveXML(data1, tempDir, "first.xml");
            FileManager.SaveXML(data2, tempDir, "second.xml");

            var results = FileManager.LoadXMLAll<TestData>(tempDir);

            Assert.AreEqual(2, results.Count);
            Assert.Contains(data1, results);
            Assert.Contains(data2, results);
        }

        [Test]
        public void LoadXMLAll_ReturnsEmptyList_WhenDirectoryMissing()
        {
            string missingDir = Path.Combine(tempDir, "MissingXML");

            var results = FileManager.LoadXMLAll<TestData>(missingDir);

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }

    }

}