using System;
using PoolGame.Events;
using PoolGame.Managers;
using UnityEngine;

namespace PoolGame
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private AimUpdatedChannel aimUpdatedChannel;
        [SerializeField] private ShotRequestedChannel shotRequestedChannel;

        [Space] 
        [SerializeField] private float minPower01;
        [SerializeField] private float maxPullDistance;
        [SerializeField] private LayerMask cueBallLayerMask;
        
        private Vector2 _cursorScreenPosition;
        private Camera _camera;

        private bool _isPulling;
        private Vector3 _cursorClickPosition;
        private AimSnapshot _latestAimSnapshot;
        private bool _hasAimSnapshot;
        private IShotTarget _currentShotTarget;

        private bool _canGo;
        
        private Vector3 CursorWorldPosition
        {
            get
            {
                if (_camera == null)
                {
                    return Vector3.zero;
                }
                return _camera.ScreenToWorldPoint(_cursorScreenPosition);
            }
        }
        
        private void OnEnable()
        {
            if (inputReader != null)
            {
                inputReader.OnPressEvent += OnPressedStarted;
                inputReader.OnPressCanceledEvent += OnPressedCanceled;
                inputReader.OnCursorEvent += OnCursor;
            }
        }
        
        private void OnDisable()
        {
            if (inputReader != null)
            {
                inputReader.OnPressEvent -= OnPressedStarted;
                inputReader.OnPressCanceledEvent -= OnPressedCanceled;
                inputReader.OnCursorEvent -= OnCursor;
            }
        }
        
        private void Start()
        {
            _camera = Camera.main;
        }
        
        private void Update()
        {
            UpdateAim();
        }
        
        private void OnCursor(Vector2 obj)
        {
            _cursorScreenPosition = obj;
        }

        private void OnPressedStarted()
        {
            if (_camera == null || !_canGo)
                return;

            Vector2 worldPoint = _camera.ScreenToWorldPoint(_cursorScreenPosition);

            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 0f, cueBallLayerMask);
            if (!hit)
                return;

            IShotTarget target = hit.collider.GetComponentInParent<IShotTarget>();
            if (target == null)
                return;

            _currentShotTarget = target;
            _cursorClickPosition = hit.collider.transform.position;
            _cursorClickPosition.z = 0;
            _isPulling = true;
            _hasAimSnapshot = false;
        }
        
        private void OnPressedCanceled()
        {
            if (_isPulling && _hasAimSnapshot && shotRequestedChannel && _canGo)
            {
                ShotData shotData = new()
                {
                    ShotTarget = _currentShotTarget,
                    ShotDirection = _latestAimSnapshot.ShotDirection ,
                    ShotPower01 = _latestAimSnapshot.ClampedPullPercentage
                };
                
                shotRequestedChannel.RaiseEvent(shotData);
            }
            
            _isPulling = false;
            _hasAimSnapshot = false;
            _currentShotTarget = null;
        }
        
        private void UpdateAim()
        {
            if (!_isPulling || _camera == null || !_canGo) 
                return;
            
            Vector3 start = _cursorClickPosition;
            Vector3 cursor = CursorWorldPosition;
            cursor.z = 0;

            Vector3 drag = cursor - start;
            float distance = drag.magnitude;
            
            if (distance.IsNearlyZero())
            {
                _hasAimSnapshot = false;
                return;
            }
            
            Vector3 dragDir = drag / distance;
            dragDir.z = 0;
            
            float clampedDistance = Mathf.Min(distance, maxPullDistance);
            float clampedPull01 = Mathf.Clamp01(clampedDistance / maxPullDistance);
            
            Vector3 clampedEnd = start + dragDir * clampedDistance;
            clampedEnd.z = 0;

            if (!aimUpdatedChannel) 
                return;
            
            float shotPower = clampedPull01 >= minPower01 ? clampedPull01 : -1f;
            
            AimSnapshot snapshot = new()
            {
                AimingPoint = start,
                CursorWorldPoint = cursor,
                ClampedEndAimingPoint = clampedEnd,
                ShotDirection = -dragDir,
                ClampedPullDistance = clampedDistance,
                ClampedPullPercentage = shotPower
            };
            
            _latestAimSnapshot = snapshot;
            _hasAimSnapshot = shotPower >= 0f;
            aimUpdatedChannel.RaiseEvent(snapshot);
        }

        public void OnGameStateChange(GameStateChange data)
        {
            _canGo = data.To == GameState.PlayerTurn;
            
            if (!_canGo)
            {
                _isPulling = false;
                _hasAimSnapshot = false;
                _currentShotTarget = null;
            }
        }
    }
}
