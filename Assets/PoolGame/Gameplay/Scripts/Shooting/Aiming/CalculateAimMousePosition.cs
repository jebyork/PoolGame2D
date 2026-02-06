using UnityEngine;

namespace PoolGame.Gameplay.Shooting.Aiming
{
    [CreateAssetMenu(fileName = "Calculate Aim From Mouse", menuName = "Shooting/Aiming/Calculate Aim From Mouse")]
    public class CalculateAimMousePosition : CalculateAimDataStrategy
    {
        protected override Vector3 CalculateDirection(AimingCalculationData aimingCalculationData)
        {
            return aimingCalculationData.Shootable.GetPosition() -  aimingCalculationData.CurrentMousePos;
        }
    }
}
