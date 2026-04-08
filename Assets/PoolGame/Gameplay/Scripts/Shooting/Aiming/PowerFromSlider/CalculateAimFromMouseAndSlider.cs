using PoolGame.Core.Observers;
using UnityEngine;

namespace PoolGame.Gameplay.Shooting.Aiming.PowerFromSlider
{
    [CreateAssetMenu(fileName = "Calculate Aim From Slider", menuName = "Shooting/Aiming/Calculate Aim From Slider", order = 0)]
    public class CalculateAimFromMouseAndSlider : CalculateAimDataStrategy
    {
        [SerializeField] private ObservableFloat powerSlider;


        protected override Vector3 CalculateDirection(AimingCalculationData aimingCalculationData)
        {
            return aimingCalculationData.Shootable.GetPosition() - aimingCalculationData.CurrentMousePos;
        }

        protected override float CalculatePower()
        {
            return powerSlider.Value;
        }
    }
}