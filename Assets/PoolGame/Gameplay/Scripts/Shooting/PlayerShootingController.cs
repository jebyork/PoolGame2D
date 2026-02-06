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
        
        private AimingCalculationData _currentCalculationData;
        private AimingData _currentAimingData;
        
        public void OnStartedAiming()
        {
            Debug.Log("Here");
            IShootable shootable = getShootableTagetStrategy.GetShootable();
            _currentCalculationData = new AimingCalculationData
            {
                Shootable = shootable,
                InitialMousePos = mouseScreenPosition.Value,
                CurrentMousePos = mouseScreenPosition.Value,
            };

            if (_currentCalculationData.Shootable != null)
            {
                Debug.Log("Has Shootable");
            }
        }

        public void OnStoppedAiming()
        {
            _currentCalculationData.Shootable = null;
        }

        private void Update()
        {
            if(_currentCalculationData.Shootable == null) return;
            
            _currentCalculationData.CurrentMousePos = mouseScreenPosition.Value;
            _currentAimingData = calculateAimDataStrategy.CalculateAimData(_currentCalculationData);
            
            Logwin.Log("Power", _currentAimingData.Power01, "Shooting");
            Logwin.Log("Direction", _currentAimingData.Direction, "Shooting");
            
        }
    }
}
