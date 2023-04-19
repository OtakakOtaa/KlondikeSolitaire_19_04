using System;
using UnityEngine;

namespace CodeBase.Core._Card.Data
{
    [Serializable] public sealed class Card
    {
        [SerializeField] private Sprite _view;
        [SerializeField] private CardRank _rank;
        [SerializeField] private CardSuit _cardSuit;

        public Sprite View => _view;
        public CardRank Rank => _rank;
        public CardSuit CardSuit => _cardSuit;
    }
}