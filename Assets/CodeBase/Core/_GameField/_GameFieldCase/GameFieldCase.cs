using System.Collections.Generic;
using System.Linq;
using CodeBase.Core._Card;
using CodeBase.Core._Card.Data;
using CodeBase.Core._Card.View;
using CodeBase.Core._GameField._GameFieldCase.View;

namespace CodeBase.Core._GameField._GameFieldCase
{
    public sealed class GameFieldCase
    {
        private const string CardLayer = "Card";
        private readonly CardsArranger _cardsArranger;
        
        private readonly GameFieldCaseComponent _fieldCaseComponent;
        private readonly List<CardComponent> _cards;
        
        public GameFieldCase(GameFieldCaseComponent fieldCaseComponent, CardsArranger cardsArranger)
        {
            _cardsArranger = cardsArranger;
            _fieldCaseComponent = fieldCaseComponent;
            _cards = new List<CardComponent>();
        }

        public CardComponent TryGetLastCard()
        {
            var last = _cards.LastOrDefault();
            return last is not null ? last : null;
        }
        
        public bool TryPopCardsFromStarting(CardComponent starCard, out CardComponent[] cards)
        {
            cards = null;
            if (_cards.Contains(starCard) is false) return false;
            
            int cardIndex = _cards.IndexOf(starCard);
            int cardsCount = _cards.Count - cardIndex;
            cards = _cards.GetRange(cardIndex, cardsCount).ToArray();
            _cards.RemoveRange(cardIndex, cardsCount);
            
            _fieldCaseComponent.SetTriggerZoneOnLastCard();

            if (_cards.Count <= 0) return true;
            
            _cardsArranger.ArrangeCards(_cards.ToArray());
            if (_cards.Last().CardOpenFlag is false)
                cards.First().CardSettleDown += OnCardSettleDown;
            
            return true;
        }
        

        public void AssignStartingCards(IEnumerable<CardComponent> startCards)
        {
            _cards.AddRange(startCards);
            foreach (var card in _cards)
            {
                card.CloseCard();
                BindCardToCase(card);
            }
            _cards.Last().OpenCard();
            _cardsArranger.ArrangeCards(_cards.ToArray());
            _fieldCaseComponent.SetTriggerZoneOnLastCard();
        }

        public void AddCards(CardComponent[] cards)
        {
            cards.First().CardSettleDown?.Invoke(this, cards.First());

            _cards.AddRange(cards);
            _cards.ForEach(BindCardToCase);
            _cardsArranger.ArrangeCards(_cards.ToArray());
            _fieldCaseComponent.SetTriggerZoneOnLastCard();
        }

        private void OnCardSettleDown(GameFieldCase gameFieldCase, CardComponent card)
        {
            var cardReturned = gameFieldCase == this;
            if (cardReturned)
            {
                card.CardSettleDown -= OnCardSettleDown;
                return;
            }
            _cards.Last().OpenCard();
        }
        
        private void BindCardToCase(CardComponent card)
        {
            var caseTransform = _fieldCaseComponent.transform;
            var cardTransform = card.transform;
            cardTransform.position = caseTransform.position; 
            cardTransform.parent = caseTransform;
            card.SpriteRenderer.sortingLayerName = CardLayer;
        }
    }
}