using PoolGame.Core.Helpers;
using PoolGame.Core.Observers;
using PoolGame.Gameplay.Shooting.Aiming;
using PoolGame.Gameplay.Shooting.CanShoot;
using PoolGame.Gameplay.Shooting.Targeting;
using UnityEngine;

namespace PoolGame.Gameplay.Shooting
{
    public class PlayerShootingController : MonoBehaviour
    {
        [SerializeField] private GetShootableTargetStrategy getShootableTargetStrategy;
        [SerializeField] private ObservableVector2 mouseScreenPosition;
        [SerializeField] private CalculateAimDataStrategy  calculateAimDataStrategy;
        [SerializeField] private CanShootStrategy canShootStrategy;

        [Space] 
        [SerializeField] private AimingCalculationDataObserver calculationDataObserver;
        [SerializeField] private AimingDataObserver aimingDataObserver;
            
        
        private AimingCalculationData _currentCalculationData;
        private AimingData _currentAimingData;
        
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
            calculationDataObserver.Value = _currentCalculationData;
            
        }

        public void OnStoppedAiming()
        {
            if (canShootStrategy.CanShoot())
            {
                _currentCalculationData.Shootable.Shoot(_currentAimingData);
            }
            _currentCalculationData.Shootable = null;
            calculationDataObserver.Value = _currentCalculationData;
        }

        private void Update()
        {
            if(_currentCalculationData.Shootable == null) return;
            
            _currentCalculationData.CurrentMousePos = MyHelpers.GetScreenToWorldPosition(mouseScreenPosition.Value);
            _currentAimingData = calculateAimDataStrategy.CalculateAimData(_currentCalculationData);
            aimingDataObserver.Value = _currentAimingData;
        }
    }
}
