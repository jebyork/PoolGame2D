using PoolGame.Game.ScreenToWorld;
using UnityEngine;

namespace PoolGame.Gameplay.Aim
{
    public class DragAimStrategy : IAimStrategy
    {
        private readonly CameraScreenToWorldStrategy _cursorWorld;
        private readonly float _maxPullDistance;
        private readonly float _minPower01;

        private bool _active;
        private Vector3 _start;
        
        public DragAimStrategy(CameraScreenToWorldStrategy cursorWorld, float maxPullDistance, float minPower01)
        {
            _cursorWorld = cursorWorld;
            _maxPullDistance = maxPullDistance;
            _minPower01 = minPower01;
        }
        
        public void Begin(Vector3 startWorld)
        {
            _active = true;
            _start = startWorld;
            _start.z = 0;
        }
        
        public void End()
        {
            _active = false;
        }
        public bool TryGetSnapshot(out AimSnapshot snapshot)
        {
            snapshot = default;

            if (!_active || _cursorWorld == null)
                return false;

            Vector3 cursorWorld = _cursorWorld.ScreenToWorld();
            cursorWorld.z = 0;

            Vector3 drag = cursorWorld - _start;
            float distance = drag.magnitude;
            
            if (distance <= Mathf.Epsilon)
                return false;

            Vector3 dragDir = drag / distance;
            dragDir.z = 0;

            float clampedDistance = Mathf.Min(distance, _maxPullDistance);
            float pull01 = Mathf.Clamp01(clampedDistance / _maxPullDistance);

            Vector3 clampedEnd = _start + dragDir * clampedDistance;
            clampedEnd.z = 0;

            bool isValid = pull01 >= _minPower01;

            snapshot = new AimSnapshot
            {
                AimingPoint = _start,
                CursorWorldPosition = cursorWorld,
                AimEndPoint = clampedEnd,
                ShotDirection = -dragDir,
                PullDistance = clampedDistance,
                ShotPower01 = pull01, 
                IsValidShot = isValid
            };
            
            return true;
        }
    }
}
