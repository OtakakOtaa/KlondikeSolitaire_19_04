using System.Collections.Generic;
using CodeBase.Core._Card.View;
using CodeBase.Core._Deck.View;
using Random = System.Random;

namespace CodeBase.Core._Deck
{
    public sealed class Deck
    {
        private readonly DeckObject _deck;
        private Stack<CardComponent> _cards;
        
        public Deck(IEnumerable<CardComponent> cards, DeckObject deck)
        {
            _deck = deck;
            _cards = new Stack<CardComponent>(cards);
            foreach (var card in _cards)
            {
                card.transform.parent = deck.transform;
                card.CloseCard();
            }
        }

        public void ReshuffleDeck()
        {
            Random random = new ();
            var cardsInArray = _cards.ToArray();
            for (var i = _cards.Count - 1; i >= 1; i--)
            {
                var j = random.Next(i + 1);
                (cardsInArray[j], cardsInArray[i]) = (cardsInArray[i], cardsInArray[j]);
            }
            _cards = new Stack<CardComponent>(cardsInArray);
        }

        public CardComponent PopCard()
            => _cards.Pop();

        public CardComponent[] PopCardsRange(int amount)
        {
            CardComponent[] popCards = new CardComponent[amount];
            for (var i = 0; i < amount; i++)
                popCards[i] = _cards.Pop();
            return popCards;
        }
    }
}