using System;
using PoolGame.Core.Helpers;
using PoolGame.Core.Observers;
using PoolGame.Game.Audio;
using PoolGame.Gameplay.Shooting.Aiming;
using PoolGame.Gameplay.Shooting.CanShoot;
using PoolGame.Gameplay.Shooting.Targeting;
using UnityEngine;

namespace PoolGame.Gameplay.Shooting
{
    public class PlayerShootingController : MonoBehaviour
    {
        [Header("Strategies")]
        [SerializeField] GetShootableTargetStrategy getShootableTargetStrategy;
        [SerializeField] ObservableVector2 mouseScreenPosition;
        [SerializeField] CalculateAimDataStrategy calculateAimDataStrategy;
        [SerializeField] CanShootStrategy canShootStrategy;
        
        [SerializeField] AudioClip[] shotSound;
        public event Action OnShotTaken;
        
        AimingCalculationData _currentCalculationData;
        AimingData _currentAimingData;

        public AimingCalculationData CurrentCalculationData => _currentCalculationData;
        public AimingData CurrentAimingData => _currentAimingData;
        public bool HasActiveAim => _currentCalculationData.Shootable != null;

        #region Lifecycle
        
        void Update()
        {
            if (_currentCalculationData.Shootable == null)
                return;

            _currentCalculationData.CurrentMousePos = MyHelpers.GetScreenToWorldPosition(mouseScreenPosition.Value);
            _currentAimingData = calculateAimDataStrategy.CalculateAimData(_currentCalculationData);
        }
        
        #endregion
        
        public float GetAimPower() => _currentAimingData.Power01;
        public IShootable GetShootable() => _currentCalculationData.Shootable;

        public void OnStartedAiming()
        {
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
                SoundManagerSO.PlaySound(shotSound, transform.position, 1f);
            }
            _currentCalculationData.Shootable = null;
        }
        
        public bool CanShootCurrentAim()
        {
            return canShootStrategy.CanShoot(this);
        }
    }
}
