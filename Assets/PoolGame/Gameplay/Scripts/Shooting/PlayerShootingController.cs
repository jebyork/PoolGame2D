using System.Runtime.CompilerServices;
using PoolGame.Core.Helpers;
using PoolGame.Core.Observers;
using PoolGame.Gameplay.Shooting.Aiming;
using PoolGame.Gameplay.Shooting.Targeting;
using UnityEngine;

namespace PoolGame.Gameplay.Shooting
{
    public class PlayerShootingController : MonoBehaviour
    {
        [SerializeField] private GetShootableTagetStrategy getShootableTagetStrategy;
        [SerializeField] private ObservableVector2 mouseScreenPosition;
        [SerializeField] private CalculateAimDataStrategy  calculateAimDataStrategy;

        [Space] 
        [SerializeField] private AimingCalculationDataObserver calculationDataObserver;
        [SerializeField] private AimingDataObserver aimingDataObserver;
            
        
        private AimingCalculationData _currentCalculationData;
        private AimingData _currentAimingData;
        
        public void OnStartedAiming()
        {
            Vector3 worldPoint = MyHelpers.GetScreenToWorldPosition(mouseScreenPosition.Value);
            IShootable shootable = getShootableTagetStrategy.GetShootable();
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
            if (_currentCalculationData.Shootable == null) return;
            
            _currentCalculationData.Shootable.Shoot(_currentAimingData);
            _currentCalculationData.Shootable = null;
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
