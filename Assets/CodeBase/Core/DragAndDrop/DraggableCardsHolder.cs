using CodeBase.Core._Card;
using CodeBase.Core._Card.View;
using CodeBase.Core._GameField._GameFieldCase;
using UnityEngine;

namespace CodeBase.Core.DragAndDrop
{
    public sealed class DraggableCardsHolder : MonoBehaviour
    {
        private const string CardLayer = "SelectedCard"; 
        
        private CardsArranger _cardsArranger;

        public CardComponent[] Cards { get; private set; }
        public GameFieldCase Origin { get; private set; }

        public bool IsGriped => Cards is not null;

        public void InjectArrange(CardsArranger arranger)
            => _cardsArranger = arranger;

        public void Grip(CardComponent[] cards, GameFieldCase origin)
        {
            Cards = cards;
            Origin = origin;
            foreach (var card in cards)
            {
                var cardTransform = card.transform;
                cardTransform.position = transform.position;
                cardTransform.parent = transform;
                card.SpriteRenderer.sortingLayerName = CardLayer;
            }
            _cardsArranger.ArrangeCards(Cards);
        }

        public void StopGrip()
        {
            Cards = default;
            Origin = default;
        }
    }
}