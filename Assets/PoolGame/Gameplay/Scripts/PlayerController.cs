using PoolGame.Core.Game.States.Gameplay;
using PoolGame.Core.Game.States.Gameplay.Shot;
using PoolGame.Core.Helpers;
using PoolGame.Core.Input;
using PoolGame.Gameplay.Shot;
using PoolGame.Gameplay.ShotTargetPicker;
using UnityEngine;

namespace PoolGame.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private AimUpdatedChannel aimUpdatedChannel;
        [SerializeField] private ShotRequestedChannel shotRequestedChannel;
        [SerializeField] private ShotTargetPickerStrategy shotTargetPickerStrategy;

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
        private IShootable _currentShootable;
        

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
            ShotTargetPickResult target = shotTargetPickerStrategy.TryPick();
            if (target.HasHit == false)
                return;

            _currentShootable = target.Target;
            _cursorClickPosition = target.HitPoint;
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
                    Shootable = _currentShootable,
                    ShotDirection = _latestAimSnapshot.ShotDirection ,
                    ShotPower01 = _latestAimSnapshot.ClampedPullPercentage
                };
                
                shotRequestedChannel.RaiseEvent(shotData);
            }
            
            _isPulling = false;
            _hasAimSnapshot = false;
            _currentShootable = null;
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
                _currentShootable = null;
            }
        }
    }
}
