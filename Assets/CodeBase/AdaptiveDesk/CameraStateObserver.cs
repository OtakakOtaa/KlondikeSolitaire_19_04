using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.AdaptiveDesk
{
    public sealed class CameraStateObserver
    {
        public event Action ResolutionChanged;
        public Camera Camera { get; }

        private readonly TimeSpan _observeDelay = new (0,0,1);

        private int _cachedHeight; 
        private int _cachedWidth; 
        
        public CameraStateObserver(Camera camera)
        {
            Camera = camera;
            CacheCameraResolution();
        }
        
        public async UniTaskVoid StartObserve(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested is false)
            {
                await UniTask.Delay(_observeDelay, cancellationToken: cancellationToken);
                if (!HasCameraResolutionChanged) continue;
                ResolutionChanged?.Invoke();
                CacheCameraResolution();
            }
        }

        private bool HasCameraResolutionChanged
            => (Camera.scaledPixelWidth == _cachedWidth && Camera.scaledPixelHeight == _cachedHeight) is false; 
        
        private void CacheCameraResolution()
        {
            _cachedHeight = Camera.scaledPixelHeight;
            _cachedWidth = Camera.scaledPixelWidth;
        }
    }
}