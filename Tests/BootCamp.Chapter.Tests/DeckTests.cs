using System;
using System.Collections.Generic;
using System.Text;
using BootCamp.Chapter.Gambling;
using Xunit;

namespace BootCamp.Chapter.Tests
{
    public class DeckTests
    {
        [Fact]
        public void Instantiating_Deck_When_CardsEmpty_Throws_ArgumentNullException()
        {
            IList<Card> cards = default;

            var exception = Assert.Throws<ArgumentNullException>(() => new Deck(cards));

            Assert.Equal($"Value cannot be null. (Parameter '{nameof(cards)}')", exception.Message);
        }

        [Fact]
        public void Instantiating_Deck_When_CardsNotEmpty_Does_Not_Throw_Exception()
        {
            IList<Card> cards = new List<Card>() {new Card(Card.Suites.Clubs, Card.Ranks.Ace)};

            var exception = Record.Exception(() => new Deck(cards));

            Assert.Null(exception);
        }


    }
}
