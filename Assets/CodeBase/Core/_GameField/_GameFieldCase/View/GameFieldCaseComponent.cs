using CodeBase.Core._Card.Data;
using CodeBase.Core._Card.View;
using CodeBase.Core.DragAndDrop;
using Unity.VisualScripting;


namespace CodeBase.Core._GameField._GameFieldCase.View
{
    public sealed class GameFieldCaseComponent : CardCollector
    {
        private GameFieldCase _gameFieldCase;

        public void InjectFieldCase(GameFieldCase fieldCase)
            => _gameFieldCase = fieldCase;

        public override void Collect(CardComponent[] cardComponent)
            => _gameFieldCase.AddCards(cardComponent);

        public override bool CanCollect(Card expectedCard)
        {
            var lastCard = _gameFieldCase.TryGetLastCard();
            
            if (lastCard is null) return true;
            if (lastCard.Card.Rank is CardRank.Two) return false;
            
            var isExpectedCardOneLess = lastCard.Card.Rank - 1 == expectedCard.Rank;
            var isDifferentCardsSuits = IsRedSuit(expectedCard) != IsRedSuit(lastCard.Card);
            
            return isExpectedCardOneLess && isDifferentCardsSuits;
        }

        public void SetTriggerZoneOnLastCard()
        {
            var lastCard = _gameFieldCase.TryGetLastCard();
            if(lastCard is null) return;
            _trigger.offset = lastCard.transform.localPosition;
        }
        
        private bool IsRedSuit(Card card)
            => card.CardSuit is CardSuit.Diamonds or CardSuit.Hearts;
    }
}