using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Core._Card.View;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Core._Card
{
    public sealed class CardCreator
    {
        private readonly Settings _settings;
        private readonly CardsConfiguration _cardsConfiguration;
        
        public CardCreator(Settings settings, CardsConfiguration cardsConfiguration)
        {
            _settings = settings;
            _cardsConfiguration = cardsConfiguration;
        }

        public IEnumerable<CardComponent> Create()
            => _cardsConfiguration.Cards
                .Select(c =>
                {
                    var cardComponent = Object.Instantiate(_settings.CardPrefab, parent: _settings.CardRoot);
                    cardComponent.Constructor(c, _settings.CardBack);
                    return cardComponent;
                }).ToArray();

        
        [Serializable] public sealed class Settings
        {
            [SerializeField] private CardComponent _cardPrefab;
            [SerializeField] private Transform _cardRoot;
            [SerializeField] private Sprite _cardBack;

            public Sprite CardBack => _cardBack;
            public CardComponent CardPrefab => _cardPrefab;
            public Transform CardRoot => _cardRoot;
        }
    }
}