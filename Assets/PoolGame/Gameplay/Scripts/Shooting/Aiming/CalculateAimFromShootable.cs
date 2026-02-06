using UnityEngine;

namespace PoolGame.Gameplay.Shooting.Aiming
{
    [CreateAssetMenu(fileName = "Calculate Aim From Shootable", menuName = "Shooting/Aiming/Calculate Aim From Shootable")]
    public class CalculateAimFromShootable : CalculateAimDataStrategy
    {
        protected override Vector3 CalculateDirection(AimingCalculationData aimingCalculationData)
        {
            return aimingCalculationData.CurrentMousePos - aimingCalculationData.InitialMousePos;
        }
    }
}
