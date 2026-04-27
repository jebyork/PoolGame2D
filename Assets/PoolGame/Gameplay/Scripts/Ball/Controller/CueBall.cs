using PoolGame.Core.Helpers;
using PoolGame.Gameplay.Shooting;
using PoolGame.Gameplay.Shooting.Aiming;
using UnityEngine;

namespace PoolGame.Gameplay.Ball.Controller
{
    public class CueBall : BallController, IShootable
    {
        [SerializeField] private float maxImpulse = 10f;
        
        public Vector3 GetPosition()
        {
            return transform.position;
        }
        
        public void Shoot(AimingData aimingData)
        {
            ApplyShot(aimingData.Direction, aimingData.Power01);
        }
        
        private void ApplyShot(Vector3 direction , float power01)
        {
            if (Rb2D == null || direction.IsNearlyZero()) return;

            float impulse = power01 * maxImpulse;
            Rb2D.AddForce(direction * impulse, ForceMode2D.Impulse);
        }
    }
}
