using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using BootCamp.Chapter.Gambling;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Xunit;
using Xunit.Sdk;

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

        [Theory]
        [ClassData(typeof(DeckDrawFromTopDeckExpectations))]        

        public void DrawFromTop_Draws_Expected_Card_When_Deck_Has_Cards(IList<Card> cards, Card expectedCard)
        {
            Deck deck = new Deck(cards);

            Card actualCard = deck.DrawFromTop();

            Assert.Equal(expectedCard, actualCard);
        }

        [Fact]
        public void DrawFromTop_Throws_IndexOutOfRangeException_When_Deck_Has_No_Cards()
        {
            Deck deck = new Deck(new List<Card>());

            var exception = Record.Exception(() => deck.DrawFromTop());

            Assert.Equal(typeof(ArgumentOutOfRangeException), exception.GetType());
        }

        [Theory]
        [ClassData(typeof(DeckShuffleDrawAtRandomParameters))]
        public void Shuffle_Changes_Order_Of_Cards_In_Deck(IList<Card> cards)
        {
            Deck deck = new Deck(cards);

            deck.Shuffle();

            bool outcome = AreListsMatching(cards, deck);
            Assert.True(outcome);
            
        }

        [Fact]
        public void Shuffle_Does_Nothing_When_Deck_Length_Equals_1()
        {
            Deck deck = new Deck(new List<Card>() { new Card(Card.Suites.Diamonds, Card.Ranks.Ace) });
            Card expectedCard = new Card(Card.Suites.Diamonds, Card.Ranks.Ace);

            deck.Shuffle();

            Assert.Equal(expectedCard, deck.DrawFromTop());
        }

        private bool AreListsMatching(IList<Card> originalList, Deck shuffledDeck)
        {
            int lengthOfOriginalList = originalList.Count;
            List<Card> newList = new List<Card>();
            int count = 0;

            for (int i = 0; i < lengthOfOriginalList; i++)
            {
                newList.Add(shuffledDeck.DrawFromTop());
            }

            for (int i = 0; i < lengthOfOriginalList; i++)
            {
                if (newList[i].ToString() == originalList[i].ToString())
                {
                    count++;
                }
            }
            return count != lengthOfOriginalList;
        }

        // Unit Testing randomness is very difficult as Unit Tests are boolean by nature - either passes or it doesn't.
        // Randomness by definition cannot be easily tested. Here I have checked the distribution of which cards are returned
        // by the 'DrawAtRandom' and applied the condition that anything above 25% (0.25) in a sample of 100 would be
        // adequate.

        // Unable to official claim testing or 'verification' credit for for DrawRandom as it is implicitly tested and not explicitly.
        // Guidance on how to implement this would be obliged
        [Theory]
        [ClassData(typeof(DeckShuffleDrawAtRandomParameters))]
        public void DrawAtRandom_Returns_Random_Card_From_Deck(IList<Card> cards)
        {
            DeckDrawRandomTestSetup setup = new DeckDrawRandomTestSetup();

            float lowestDistribution = setup.CalculateRandomnessDistribution(cards);
            
            Assert.True(0.25 <= lowestDistribution);
        }

        [Theory]
        [ClassData(typeof(DeckDrawAtPassExpectations))]
        public void DrawAt_Returns_Expected_Card_When_Index_LessThan_Deck_Size(IList<Card> cards, int index, Card expected)
        {
            Deck deck = new Deck(cards);

            var actual = deck.DrawAt(index);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [ClassData(typeof(DeckDrawAtFailureExpectations))]
        public void DrawAt_Throws_Exception_When_Index_GreaterThan_Deck_Size(IList<Card> cards, int index, Exception expected)
        {
            Deck deck = new Deck(cards);

            var exception = Record.Exception(() => deck.DrawAt(index));

            Assert.Equal(expected.GetType(), exception.GetType());
        }

        private class DeckDrawAtPassExpectations : IEnumerable<object[]>
        {
            private readonly IList<Card> _listCards = new List<Card>()
            {
                new Card(Card.Suites.Clubs, Card.Ranks.Ace),
                new Card(Card.Suites.Diamonds, Card.Ranks.Eight),
                new Card(Card.Suites.Diamonds, Card.Ranks.Five)
            };

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]{_listCards, 0, new Card(Card.Suites.Clubs, Card.Ranks.Ace)};
                yield return new object[] { _listCards, 1, new Card(Card.Suites.Diamonds, Card.Ranks.Eight)};
                yield return new object[] { _listCards, 2, new Card(Card.Suites.Diamonds, Card.Ranks.Five)};
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private class DeckDrawAtFailureExpectations : IEnumerable<object[]>
        {
            private readonly IList<Card> _listCards = new List<Card>()
            {
                new Card(Card.Suites.Clubs, Card.Ranks.Ace),
                new Card(Card.Suites.Diamonds, Card.Ranks.Eight),
                new Card(Card.Suites.Diamonds, Card.Ranks.Five)
            };

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { _listCards, 4, typeof(OutOfCardsException)};
                yield return new object[] { _listCards, -1, typeof(ArgumentOutOfRangeException)};
                yield return new object[] { _listCards, 100, typeof(OutOfCardsException)};
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private class DeckDrawRandomTestSetup
        {
            private int _numberOfCards = 3;
            private float _numberOfMethodCalls = 100;

            private float _cardCount1 = 0;
            private float _cardCount2 = 0;
            private float _cardCount3 = 0;

            private float[] _distOfCards = new float[3];

            private float _lowestDist;

            public float CalculateRandomnessDistribution(IList<Card> cards)
            {
                

                for (int i = 0; i < _numberOfMethodCalls; i++)
                {
                    Deck deck = new Deck(cards);
                    CardCountIncrementer(deck.DrawRandom());
                }

                CalculateStatistics();

                return _lowestDist;
            }

            private void CalculateStatistics()
            {
                _lowestDist = _numberOfMethodCalls;
                _distOfCards[0] = _cardCount1 / _numberOfMethodCalls;
                _distOfCards[1] = _cardCount2 / _numberOfMethodCalls;
                _distOfCards[2] = _cardCount3 / _numberOfMethodCalls;

                for (int i = 0; i < _numberOfCards; i++)
                {
                    _lowestDist = Math.Min(_lowestDist, _distOfCards[i]);
                }
            }
            private void CardCountIncrementer(Card card)
            {
                switch (card.ToString())
                {
                    case "Ace of Clubs":
                        _cardCount1++;
                        break;

                    case "Eight of Diamonds":
                        _cardCount2++;
                        break;

                    case "Five of Diamonds":
                        _cardCount3++;
                        break;

                    default:
                        return;
                }
            }
        }
        

        private class DeckShuffleDrawAtRandomParameters : IEnumerable<object[]>
        {
            private readonly IList<Card> _listCards1 = new List<Card>()
            {
                new Card(Card.Suites.Clubs, Card.Ranks.Ace),
                new Card(Card.Suites.Diamonds, Card.Ranks.Eight),
                new Card(Card.Suites.Diamonds, Card.Ranks.Five)
            };

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { _listCards1};
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private class DeckDrawFromTopDeckExpectations : IEnumerable<object[]>
        {
            private readonly IList<Card> _listCards = new List<Card>()
            {
                new Card(Card.Suites.Clubs, Card.Ranks.Ace),
                new Card(Card.Suites.Diamonds, Card.Ranks.Eight),
                new Card(Card.Suites.Diamonds, Card.Ranks.Five)
            };


            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {_listCards, new Card(Card.Suites.Diamonds, Card.Ranks.Five) };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
