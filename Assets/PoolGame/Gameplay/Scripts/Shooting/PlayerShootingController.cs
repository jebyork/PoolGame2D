using System;
using PoolGame.Core.Helpers;
using PoolGame.Core.Observers;
using PoolGame.Gameplay.GameMode;
using PoolGame.Gameplay.Shooting.Aiming;
using PoolGame.Gameplay.Shooting.Targeting;
using UnityEngine;

namespace PoolGame.Gameplay.Shooting
{
    public class PlayerShootingController : MonoBehaviour
    {
        [Header("Strategies")]
        [SerializeField] private GetShootableTargetStrategy getShootableTargetStrategy;
        [SerializeField] private ObservableVector2 mouseScreenPosition;
        [SerializeField] private CalculateAimDataStrategy calculateAimDataStrategy;
        
        [Header("References")]
        [SerializeField] private PotAllGameMode potAllGameMode;
        
        [Header("Shot Settings")]
        [SerializeField, Range(0f, 1f)] private float minShotPower = 0.05f;
        
        public event Action OnShotTaken;
        
        private AimingCalculationData _currentCalculationData;
        private AimingData _currentAimingData;
        

        public AimingCalculationData CurrentCalculationData => _currentCalculationData;
        public AimingData CurrentAimingData => _currentAimingData;
        public bool HasActiveAim => _currentCalculationData.Shootable != null;

        #region Lifecycle
        
        private void Awake()
        {
            if (potAllGameMode == null)
                potAllGameMode = FindFirstObjectByType<PotAllGameMode>();
        }
        
        private void Update()
        {
            if (_currentCalculationData.Shootable == null)
                return;

            _currentCalculationData.CurrentMousePos = MyHelpers.GetScreenToWorldPosition(mouseScreenPosition.Value);
            _currentAimingData = calculateAimDataStrategy.CalculateAimData(_currentCalculationData);
        }
        
        #endregion

        public void OnStartedAiming()
        {
            if (potAllGameMode != null && !potAllGameMode.CanTakePlayerShot())
                return;

            Vector3 worldPoint = MyHelpers.GetScreenToWorldPosition(mouseScreenPosition.Value);
            IShootable shootable = getShootableTargetStrategy.GetShootable();
            _currentCalculationData = new AimingCalculationData
            {
                Shootable = shootable,
                InitialMousePos = worldPoint,
                CurrentMousePos = worldPoint,
            };
        }

        public void OnStoppedAiming()
        {
            if (CanShootCurrentAim())
            {
                _currentCalculationData.Shootable.Shoot(_currentAimingData);
                OnShotTaken?.Invoke();
            }

            _currentCalculationData.Shootable = null;
        }
        
        public bool CanShootCurrentAim()
        {
            if (potAllGameMode != null && !potAllGameMode.CanTakePlayerShot())
                return false;

            if (_currentCalculationData.Shootable == null)
                return false;

            return _currentAimingData.Power01 >= minShotPower;
        }
    }
}
