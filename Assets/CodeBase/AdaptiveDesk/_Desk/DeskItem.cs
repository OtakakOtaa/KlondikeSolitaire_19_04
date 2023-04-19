using System;
using UnityEngine;

namespace CodeBase.AdaptiveDesk._Desk
{
    [Serializable] public sealed class DeskItem
    {
        [SerializeField] private Transform _root;
        [SerializeField] private ViewPortPosition _viewPortPosition;

        public Transform Root => _root;
        public Vector3 ViewPort => new (_viewPortPosition.Width, _viewPortPosition.Height, 0);

        public void SetPosition(Vector3 position)
            => _root.position = position;

        [Serializable] public sealed class ViewPortPosition
        {
            [SerializeField, Range(0,1)] private float _width;
            [SerializeField, Range(0,1)] private float _height;

            public float Height => _height;
            public float Width => _width;
        }
    }
}