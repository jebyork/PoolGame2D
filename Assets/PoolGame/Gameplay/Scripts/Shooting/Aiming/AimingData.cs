using UnityEngine;

namespace PoolGame.Gameplay.Shooting.Aiming
{
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
