using System;
using System.Linq;
using CodeBase.Core._Card.View;
using UnityEngine;

namespace CodeBase.Core._Card
{
    public sealed class CardsArranger
    {
        private readonly Settings _settings;

        public CardsArranger(Settings settings)
        {
            _settings = settings;
        }
        
        
        public void ArrangeCards(CardComponent[] cards)
        {
            float offset = 0;
            
            if (cards.Length <= _settings.MaxCountOfFree)
                offset -= _settings.FreeOffset;
            if (cards.Length > _settings.MaxCountOfFree)
                offset -= _settings.MediumOffset;
            if (cards.Length > _settings.MaxCountOfMedium)
                offset -= _settings.CloseOffset;

            Vector2 originPosition = cards.First().transform.position;
            cards.First().SetOrder(0);
            for (var i = 1; i < cards.Length; i++)
            {
                cards[i].transform.position = originPosition;
                cards[i].transform.Translate(new Vector3(0,offset, 0));
                originPosition = cards[i].transform.position;
                cards[i].SetOrder(i);
            }
        }
        
        [Serializable] public sealed class Settings
        {
            [SerializeField, Range(0, 1)] private float _freeOffset; 
            [SerializeField, Range(0, 1)] private float _mediumOffset;
            [SerializeField, Range(0, 1)] private float _closeOffset;

            [Space]
            [SerializeField] private int _maxCountOfFree;
            [SerializeField] private int _maxCountOfMedium;

            public float FreeOffset => _freeOffset;
            public float MediumOffset => _mediumOffset;
            public float CloseOffset => _closeOffset;
            public int MaxCountOfFree => _maxCountOfFree;
            public int MaxCountOfMedium => _maxCountOfMedium;
        }
    }
}