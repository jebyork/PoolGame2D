using PoolGame.Core.Observers;
using UnityEngine;

namespace PoolGame.Gameplay.Shooting.Aiming
{
    
    [CreateAssetMenu(fileName = "Aiming Calculation Data Observer" , menuName = "Shooting/Aiming/Aiming Calculation Data Observer" , order = 0)]
    public class AimingCalculationDataObserver : Observer<AimingCalculationData>
    {
    }
    
    public struct AimingCalculationData
    {
        public IShootable Shootable;
        public Vector3 InitialMousePos;
        public Vector3 CurrentMousePos;

        public AimingCalculationData(IShootable shootable , Vector3 initialMousePos , Vector3 currentMousePos)
        {
            Shootable = shootable;
            InitialMousePos = initialMousePos;
            CurrentMousePos = currentMousePos;
        }
    }
}