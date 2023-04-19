using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.AdaptiveDesk._Desk
{
    public sealed class Desk : IDisposable
    {
        private readonly CameraStateObserver _cameraStateObserver;
        private readonly IEnumerable<DeskItem> _deskItems;
        
        public Desk(IEnumerable<DeskItem> deskItems, CameraStateObserver cameraStateObserver)
        {
            _deskItems = deskItems;
            _cameraStateObserver = cameraStateObserver;

            _cameraStateObserver.ResolutionChanged += ArrangeElements;
            ArrangeElements();
        }

        private void ArrangeElements()
        {
            foreach (var item in _deskItems)
                item.SetPosition(ViewPortToWorld(item));

            Vector3 ViewPortToWorld(DeskItem item)
            {
                var rawPosition = _cameraStateObserver.Camera.ViewportToWorldPoint(item.ViewPort);
                return new Vector3
                {
                    x = rawPosition.x,
                    y = rawPosition.y,
                    z = _cameraStateObserver.Camera.nearClipPlane
                };
            }
        }

        public void Dispose()
        {
            _cameraStateObserver.ResolutionChanged -= ArrangeElements;
        }
    }
}