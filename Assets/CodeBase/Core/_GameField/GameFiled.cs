using System.Linq;
using CodeBase.Core._Card;
using CodeBase.Core._Card.View;
using CodeBase.Core._Deck;
using CodeBase.Core._GameField._GameFieldCase;
using CodeBase.Core._GameField._GameFieldCase.View;

namespace CodeBase.Core._GameField
{
    public sealed class GameFiled
    {
        private const int CaseCount = 7;
        private readonly GameFieldCase[] _gameFieldCases;

        public GameFiled(GameFieldCaseComponent[] caseComponents, CardsArranger cardsArranger)
        {
            _gameFieldCases = new GameFieldCase[CaseCount];
            for (var i = 0; i < CaseCount; i++)
            {
                _gameFieldCases[i] = new GameFieldCase(caseComponents[i], cardsArranger);
                caseComponents[i].InjectFieldCase(_gameFieldCases[i]);
            }
        }

        public void FillField(Deck deck)
        {
            for (var i = 0; i < CaseCount; i++) 
                _gameFieldCases[i].AssignStartingCards(deck.PopCardsRange(i + 1));
        }

        public bool TryGetCardsFromField(CardComponent card, out GameFieldCase fieldCase, out CardComponent[] cards)
        {
            fieldCase = default;
            cards = default;
            CardComponent[] fetchingCards = default; 

            fieldCase = _gameFieldCases
                .FirstOrDefault(c => c.TryPopCardsFromStarting(card, out fetchingCards));
            
            if (fieldCase is null || fetchingCards is null) 
                return false;
            cards = fetchingCards;
            
            return true;
        }
    }
}