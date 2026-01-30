using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
using HexTecGames.Basics;

namespace HexTecGames.Basics.Tests
{
    public class SaveSystemTests
    {
        private string tempDir;

        [SetUp]
        public void Setup()
        {
            tempDir = Path.Combine(Path.GetTempPath(), "SaveSystemTests_" + Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);

            SaveSystemTestHelper.SetBaseDirectory(tempDir);
            SaveSystemTestHelper.ResetState();
        }

        [TearDown]
        public void Teardown()
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, true);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // PROFILES
        // ─────────────────────────────────────────────────────────────────────────────

        [Test]
        public void AddProfile_CreatesDirectory_AndSetsCurrent()
        {
            SaveSystem.AddProfile("TestProfile");

            Assert.AreEqual("TestProfile", SaveSystem.CurrentProfile.Name);
            Assert.IsTrue(Directory.Exists(Path.Combine(tempDir, "TestProfile")));
        }

        [Test]
        public void RenameProfile_ChangesFolderName()
        {
            SaveSystem.AddProfile("OldName");
            var profile = SaveSystem.CurrentProfile;

            SaveSystem.RenameProfile(profile, "NewName");

            Assert.IsTrue(Directory.Exists(Path.Combine(tempDir, "NewName")));
            Assert.IsFalse(Directory.Exists(Path.Combine(tempDir, "OldName")));
        }

        [Test]
        public void RemoveProfile_DeletesFolder()
        {
            SaveSystem.AddProfile("DeleteMe");
            var profile = SaveSystem.CurrentProfile;

            SaveSystem.RemoveProfile(profile);

            Assert.IsFalse(Directory.Exists(Path.Combine(tempDir, "DeleteMe")));
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // SETTINGS
        // ─────────────────────────────────────────────────────────────────────────────

        [Test]
        public void SaveSettings_And_LoadSettings_Works()
        {
            SaveSystem.SaveSettings("volume", 7);

            bool success = SaveSystem.LoadSettings("volume", out int value);

            Assert.IsTrue(success);
            Assert.AreEqual(7, value);
        }

        [Test]
        public void DeleteSettings_RemovesKey()
        {
            SaveSystem.SaveSettings("difficulty", "Hard");
            SaveSystem.DeleteSettings("difficulty");

            bool success = SaveSystem.LoadSettings("difficulty", out string value);

            Assert.IsFalse(success);
            Assert.IsNull(value);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // JSON
        // ─────────────────────────────────────────────────────────────────────────────

        [Test]
        public void SaveJSON_And_LoadJSON_Works()
        {
            SaveSystem.AddProfile("P1");

            var data = new TestData { Name = "A", Value = 1 };
            SaveSystem.SaveJSON(data, "test.json");

            var loaded = SaveSystem.LoadJSON<TestData>("test.json");

            Assert.AreEqual(data, loaded);
        }

        [Test]
        public void LoadJSONAll_ReturnsAllFiles()
        {
            SaveSystem.AddProfile("P1");

            var d1 = new TestData { Name = "One", Value = 1 };
            var d2 = new TestData { Name = "Two", Value = 2 };

            SaveSystem.SaveJSON(d1, "one.json");
            SaveSystem.SaveJSON(d2, "two.json");

            var results = SaveSystem.LoadJSONAll<TestData>("Data");

            Assert.AreEqual(2, results.Count);
            Assert.Contains(d1, results);
            Assert.Contains(d2, results);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // XML
        // ─────────────────────────────────────────────────────────────────────────────

        [Test]
        public void SaveXML_And_LoadXML_Works()
        {
            SaveSystem.AddProfile("P1");

            var data = new TestData { Name = "X", Value = 10 };
            SaveSystem.SaveXML(data, "test.xml");

            var loaded = SaveSystem.LoadXML<TestData>("test.xml");

            Assert.AreEqual(data, loaded);
        }

        [Test]
        public void LoadXMLAll_ReturnsAllFiles()
        {
            SaveSystem.AddProfile("P1");

            var d1 = new TestData { Name = "A", Value = 1 };
            var d2 = new TestData { Name = "B", Value = 2 };

            SaveSystem.SaveXML(d1, "first.xml");
            SaveSystem.SaveXML(d2, "second.xml");

            var results = SaveSystem.LoadXMLAll<TestData>("Data");

            Assert.AreEqual(2, results.Count);
            Assert.Contains(d1, results);
            Assert.Contains(d2, results);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // FILE HELPERS
        // ─────────────────────────────────────────────────────────────────────────────

        [Test]
        public void FileExists_Works()
        {
            SaveSystem.AddProfile("P1");

            var data = new TestData { Name = "A", Value = 1 };
            SaveSystem.SaveJSON(data, "exists.json");

            Assert.IsTrue(SaveSystem.FileExists("exists.json"));
        }

        [Test]
        public void DeleteFile_RemovesFile()
        {
            SaveSystem.AddProfile("P1");

            var data = new TestData { Name = "A", Value = 1 };
            SaveSystem.SaveJSON(data, "delete.json");

            SaveSystem.DeleteFile("delete.json");

            Assert.IsFalse(SaveSystem.FileExists("delete.json"));
        }
    }
}