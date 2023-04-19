using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CodeBase.Core._Card.View;
using CodeBase.Core._GameField;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Core.DragAndDrop
{
    public sealed class Selector
    {
        private readonly GameFiled _gameField;
        private readonly CardDropSystem _cardDropSystem;
        private readonly DraggableCardsHolder _gripedCardHolder;

        public Selector(GameFiled gameField, DraggableCardsHolder draggableCardsHolder, CardDropSystem dropSystem)
        {
            _cardDropSystem = dropSystem;
            _gameField = gameField;
            _gripedCardHolder = draggableCardsHolder;
        }

        public async UniTaskVoid StartObserveSelection(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested is false)
            {
                if (_gripedCardHolder.IsGriped is false)
                    TryGrip();
#if UNITY_EDITOR
                UpdateGrip(Input.GetMouseButton(0), Cursor);
#else
                UpdateGrip(Input.touchCount > 0, Touch);
#endif
                await UniTask.Yield();
            }
        }
        
        private void TryGrip()
        {
            RaycastHit2D[] hits;
            bool isHit;
#if UNITY_EDITOR
            isHit = CastRayInEditor(out hits) && Input.GetMouseButton(0);
#else
            isHit = CastRay(out hits);
#endif
            if (!isHit) return;
            TryGripCards(hits);
        }

        private void UpdateGrip(bool inputFlag, Vector3 position)
        {
            if (_gripedCardHolder.IsGriped && !inputFlag)
                _cardDropSystem.HandleDrop(_gripedCardHolder);

            if (_gripedCardHolder.IsGriped is false) return;
            _gripedCardHolder!.transform.position = position;
        }

        private Vector3 Cursor => new()
        {
            x = Camera.main!.ScreenToWorldPoint(Input.mousePosition).x,
            y = Camera.main!.ScreenToWorldPoint(Input.mousePosition).y,
            z = _gripedCardHolder is null ? 0 : _gripedCardHolder.transform.position.z
        };

        private Vector3 Touch => new()
        {
            x = Camera.main!.ScreenToWorldPoint(Input.GetTouch(0).position).x,
            y = Camera.main!.ScreenToWorldPoint(Input.GetTouch(0).position).y,
            z = _gripedCardHolder is null ? 0 : _gripedCardHolder.transform.position.z
        };

        private bool CastRay(out RaycastHit2D[] hits)
        {
            var main = Camera.main;
            var origin = main!.ScreenToWorldPoint(Input.GetTouch(0).position);
            hits = Physics2D.RaycastAll(origin, Vector2.zero);
            return hits.Length is not 0;
        }

        private bool CastRayInEditor(out RaycastHit2D[] hits)
        {
            var main = Camera.main;
            var origin = main!.ScreenToWorldPoint(Input.mousePosition);
            hits = Physics2D.RaycastAll(origin, Vector2.zero);
            return hits.Length is not 0;
        }

        private void TryGripCards(IEnumerable<RaycastHit2D> hits)
        {
            List<CardComponent> cards = new ();
            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent<CardComponent>(out var card))
                    cards.Add(card);
            }

            if (cards.Count is 0) 
                return;
            
            var selectedCard = cards
                .OrderByDescending(c => c.SpriteRenderer.sortingOrder)
                .First();

            if (selectedCard.CardOpenFlag is false) 
                return;

            if(_gameField.TryGetCardsFromField(selectedCard, out var fieldCase, out var fetchedCards))
                _gripedCardHolder.Grip(fetchedCards, fieldCase);
        }
    }
}