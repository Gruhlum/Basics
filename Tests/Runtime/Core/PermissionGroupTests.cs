using NUnit.Framework;
using System;
using HexTecGames.Basics;

namespace HexTecGames.Basics.Tests.Core
{
    public class PermissionGroupTests
    {
        private PermissionGroup group;

        [SetUp]
        public void Setup()
        {
            group = new PermissionGroup();
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // Basic State
        // ─────────────────────────────────────────────────────────────────────────────

        [Test]
        public void Starts_Allowed()
        {
            Assert.IsTrue(group.Allowed);
            Assert.AreEqual(0, group.BlockersCount);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // Adding & Removing Blockers
        // ─────────────────────────────────────────────────────────────────────────────

        [Test]
        public void Adding_Blocker_Disallows()
        {
            var sender = new object();

            group.SetPermissionState(sender, false);

            Assert.IsFalse(group.Allowed);
            Assert.AreEqual(1, group.BlockersCount);
            Assert.IsTrue(group.HasBlocker(sender));
        }

        [Test]
        public void Removing_Blocker_Allows()
        {
            var sender = new object();

            group.SetPermissionState(sender, false);
            group.SetPermissionState(sender, true);

            Assert.IsTrue(group.Allowed);
            Assert.AreEqual(0, group.BlockersCount);
        }

        [Test]
        public void Multiple_Blockers_Only_Allows_When_All_Removed()
        {
            var a = new object();
            var b = new object();

            group.SetPermissionState(a, false);
            group.SetPermissionState(b, false);

            Assert.IsFalse(group.Allowed);
            Assert.AreEqual(2, group.BlockersCount);

            group.SetPermissionState(a, true);

            Assert.IsFalse(group.Allowed);
            Assert.AreEqual(1, group.BlockersCount);

            group.SetPermissionState(b, true);

            Assert.IsTrue(group.Allowed);
            Assert.AreEqual(0, group.BlockersCount);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // Duplicate Senders (HashSet ensures uniqueness)
        // ─────────────────────────────────────────────────────────────────────────────

        [Test]
        public void Adding_Same_Sender_Twice_Does_Not_Duplicate()
        {
            var sender = new object();

            group.SetPermissionState(sender, false);
            group.SetPermissionState(sender, false);

            Assert.AreEqual(1, group.BlockersCount);
            Assert.IsFalse(group.Allowed);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // ClearAll
        // ─────────────────────────────────────────────────────────────────────────────

        [Test]
        public void ClearAll_Removes_All_Blockers()
        {
            var a = new object();
            var b = new object();

            group.SetPermissionState(a, false);
            group.SetPermissionState(b, false);

            group.ClearAll();

            Assert.IsTrue(group.Allowed);
            Assert.AreEqual(0, group.BlockersCount);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // Events
        // ─────────────────────────────────────────────────────────────────────────────

        [Test]
        public void Event_Fires_On_State_Change()
        {
            var sender = new object();
            bool? lastState = null;

            group.OnAllowedChanged += state => lastState = state;

            group.SetPermissionState(sender, false);

            Assert.AreEqual(false, lastState);
        }

        [Test]
        public void Event_Does_Not_Fire_When_State_Does_Not_Change()
        {
            var sender = new object();
            int fireCount = 0;

            group.OnAllowedChanged += _ => fireCount++;

            group.SetPermissionState(sender, false);
            group.SetPermissionState(sender, false); // no change

            Assert.AreEqual(1, fireCount);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // Null Sender Safety
        // ─────────────────────────────────────────────────────────────────────────────

        [Test]
        public void Null_Sender_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                group.SetPermissionState(null, false);
            });
        }
    }

}