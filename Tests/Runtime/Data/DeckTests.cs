using NUnit.Framework;
using System.Collections.Generic;
using HexTecGames.Basics;
using HexTecGames.Basics.Decks;
using UnityEngine;

namespace HexTecGames.Basics.Tests.Editor
{
    public class DeckTests
    {
        [SetUp]
        public void Setup()
        {
            Random.InitState(12345); // deterministic rolls
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // Deck<T> Tests
        // ─────────────────────────────────────────────────────────────────────────────

        [Test]
        public void Deck_ConstructsWithCorrectTicketCounts()
        {
            var deck = new Deck<int>(
                new DeckItem<int>(1, 3),
                new DeckItem<int>(2, 2)
            );

            Assert.AreEqual(3, deck.GetRemainingTickets(1));
            Assert.AreEqual(2, deck.GetRemainingTickets(2));
        }

        [Test]
        public void Deck_GetNext_ReducesTickets()
        {
            var deck = new Deck<string>(
                new DeckItem<string>("A", 2),
                new DeckItem<string>("B", 1)
            );

            var result = deck.GetNext();

            Assert.AreEqual(2 + 1 - 1, deck.GetRemainingTickets("A") + deck.GetRemainingTickets("B"));
        }

        [Test]
        public void Deck_RemovesItemWhenTicketsExhausted()
        {
            var deck = new Deck<string>(
                new DeckItem<string>("A", 1),
                new DeckItem<string>("B", 1)
            );

            var first = deck.GetNext(); // could be A or B

            // whichever item was rolled should now have 0 tickets
            Assert.AreEqual(0, deck.GetRemainingTickets(first));

            // the other item should still have 1 ticket
            var other = first == "A" ? "B" : "A";
            Assert.AreEqual(1, deck.GetRemainingTickets(other));

            // next roll must return the remaining item
            var second = deck.GetNext();
            Assert.AreEqual(other, second);
        }


        [Test]
        public void Deck_RegeneratesWhenEmpty()
        {
            var deck = new Deck<int>(
                new DeckItem<int>(1, 1)
            );

            var first = deck.GetNext(); // consumes the only ticket
            Assert.AreEqual(1, first);
            Assert.AreEqual(0, deck.GetRemainingTickets(1));

            var second = deck.GetNext(); // regenerates deck
            Assert.AreEqual(1, second);
        }

        [Test]
        public void Deck_HasRollsLeft_WorksCorrectly()
        {
            var deck = new Deck<int>(
                new DeckItem<int>(1, 1)
            );

            Assert.IsTrue(deck.HasRollsLeft());
            deck.GetNext();
            Assert.IsFalse(deck.HasRollsLeft());
        }

        [Test]
        public void Deck_ChangeOdds_UpdatesTicketCounts()
        {
            var deck = new Deck<int>(
                new DeckItem<int>(1, 1),
                new DeckItem<int>(2, 1)
            );

            deck.ChangeOdds(
                new DeckItem<int>(1, 3),
                new DeckItem<int>(2, 0)
            );

            Assert.AreEqual(3, deck.GetRemainingTickets(1));
            Assert.AreEqual(0, deck.GetRemainingTickets(2));
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // BoolDeck Tests
        // ─────────────────────────────────────────────────────────────────────────────

        [Test]
        public void BoolDeck_ConstructsCorrectly_FromPercent()
        {
            var deck = new BoolDeck(0.25f); // 25%

            Assert.AreEqual(25, deck.GetRemainingTickets(true));
            Assert.AreEqual(75, deck.GetRemainingTickets(false));
        }

        [Test]
        public void BoolDeck_ConstructsCorrectly_FromTrueCards()
        {
            var deck = new BoolDeck(40);

            Assert.AreEqual(40, deck.GetRemainingTickets(true));
            Assert.AreEqual(60, deck.GetRemainingTickets(false));
        }

        [Test]
        public void BoolDeck_ChangeOdds_UpdatesCorrectly()
        {
            var deck = new BoolDeck(10);

            deck.ChangeOdds(70);

            Assert.AreEqual(70, deck.GetRemainingTickets(true));
            Assert.AreEqual(30, deck.GetRemainingTickets(false));
        }

        [Test]
        public void BoolDeck_ChangeOdds_WithExplicitFalseCards()
        {
            var deck = new BoolDeck(10);

            deck.ChangeOdds(20, 5);

            Assert.AreEqual(20, deck.GetRemainingTickets(true));
            Assert.AreEqual(5, deck.GetRemainingTickets(false));
        }
    }
}