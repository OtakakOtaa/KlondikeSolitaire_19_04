using System;
using CodeBase.Core._Card.Data;
using CodeBase.Core._GameField._GameFieldCase;
using UnityEngine;

namespace CodeBase.Core._Card.View
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public sealed class CardComponent : MonoBehaviour
    {
        private Sprite _back;

        public Action<GameFieldCase, CardComponent> CardSettleDown;
        
        public SpriteRenderer SpriteRenderer { get; private set; }
        public Card Card { get; private set; }
        public bool CardOpenFlag { get; private set; }
        
        private void Awake()
            => SpriteRenderer = GetComponent<SpriteRenderer>();

        public void Constructor(Card card, Sprite back)
        {
            _back = back;
            Card = card;
            SpriteRenderer.sprite = card.View;
            CloseCard();
        }
            
        public void CloseCard()
        {
            SpriteRenderer.sprite = _back; 
            CardOpenFlag = false;
        }

        public void OpenCard()
        {
            SpriteRenderer.sprite = Card.View; 
            CardOpenFlag = true;
        }

        public void SetOrder(int layOrder)
            => SpriteRenderer.sortingOrder = layOrder;
    }
}