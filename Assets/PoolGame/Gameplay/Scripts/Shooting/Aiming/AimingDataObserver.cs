using PoolGame.Core.Observers;
using UnityEngine;

namespace PoolGame.Gameplay.Shooting.Aiming
{
    [CreateAssetMenu(fileName = "Aiming Data Observer" , menuName = "Shooting/Aiming/Aiming Data Observer" , order = 0)]
    public class AimingDataObserver : Observer<AimingData>
    {
    }
    
    public struct AimingData
    {
        public readonly Vector3 Direction;
        public readonly float Power01;

        public AimingData(Vector3 direction , float power01)
        {
            Direction = direction;
            Power01 = power01;
        }
    }
}
