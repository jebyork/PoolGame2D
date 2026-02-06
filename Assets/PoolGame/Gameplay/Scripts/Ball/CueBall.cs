using PoolGame.Core.Game.States.Gameplay.Ball;
using PoolGame.Core.Helpers;
using PoolGame.Core.Values;
using PoolGame.Gameplay.Shooting;
using PoolGame.Gameplay.Shooting.Aiming;
using UnityEngine;

namespace PoolGame.Gameplay.Ball
{
    public class CueBall : BallController, IShootable
    {
        [SerializeField] private GameObjectValue cueBallValueStore;
        [SerializeField] private float maxImpulse = 10f;
        
        private Rigidbody2D _rb2D;
        
        
        private void Awake()
        {
            if (cueBallValueStore)
            {
                cueBallValueStore.Value = gameObject;
            }
            _rb2D = GetComponent<Rigidbody2D>();
        }
        
        public void ApplyShot(Vector3 direction , float power01)
        {
            if (_rb2D == null || direction.IsNearlyZero()) return;

            float impulse = power01 * maxImpulse;
            _rb2D.AddForce(direction * impulse, ForceMode2D.Impulse);
        }
        public Vector3 GetPosition()
        {
            return transform.position;
        }
        public void Shoot(AimingData aimingData)
        {
            ApplyShot(aimingData.Direction, aimingData.Power01);
        }
    }
}
