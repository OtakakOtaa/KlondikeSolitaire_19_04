using CodeBase.Core._Card.Data;
using UnityEngine;

namespace CodeBase.Core._Card
{
    [CreateAssetMenu(menuName = nameof(CardsConfiguration), order = default, fileName = nameof(CardsConfiguration))]
    public sealed class CardsConfiguration : ScriptableObject
    {
        [SerializeField] private Card[] _cards;

        public Card[] Cards => _cards;
    }
}