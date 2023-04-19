using System.Linq;
using UnityEngine;

namespace CodeBase.Core.DragAndDrop
{
    public sealed class CardDropSystem 
    {
        private const float DetectorRadius = 0.3f;
        
        public void HandleDrop(DraggableCardsHolder draggableCardsHolder)
        {
            var position = draggableCardsHolder.transform.position;
            var hits = Physics2D.CircleCastAll(position, DetectorRadius, direction: Vector2.zero);

            foreach (var hit in hits)
                if (hit.transform.TryGetComponent<CardCollector>(out var collector))
                {
                    if (collector.CanCollect(draggableCardsHolder.Cards.First().Card) is false)
                    {
                        ReturnCards(draggableCardsHolder);
                        return;
                    }
                    collector.Collect(draggableCardsHolder.Cards);
                    draggableCardsHolder.StopGrip();
                    return;
                }
            
            ReturnCards(draggableCardsHolder);
        }

        private void ReturnCards(DraggableCardsHolder draggableCardsHolder)
        {
            draggableCardsHolder.Origin.AddCards(draggableCardsHolder.Cards);
            draggableCardsHolder.StopGrip();
        } 
    }
}