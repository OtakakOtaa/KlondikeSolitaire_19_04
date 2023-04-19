using CodeBase.Core._Card.Data;
using CodeBase.Core._Card.View;
using UnityEngine;

namespace CodeBase.Core.DragAndDrop
{
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class CardCollector : MonoBehaviour
    {
        protected BoxCollider2D _trigger;   
        
        public void Awake()
            => _trigger = GetComponent<BoxCollider2D>();

        public abstract void Collect(CardComponent[] cardComponent);

        public abstract bool CanCollect(Card expectedCard);
    }
}