using PoolGame.Core.Game.States.Gameplay.Shot;
using PoolGame.Core.Helpers;
using PoolGame.Core.Values;
using UnityEngine;

namespace PoolGame.Core.Game.States.Gameplay.Ball
{
    public class CueBall : BallController, IShotTarget
    {
        [SerializeField] private ShotRequestedChannel shotRequestedChannel;
        [SerializeField] private GameObjectValue cueBallValueStore;
        [SerializeField] private float maxImpulse = 10f;
        
        private Rigidbody2D _rb2D;
        
        public Vector3 AimPointWorld => transform.position;
        
        private void OnEnable()
        {
            if (shotRequestedChannel != null)
            {
                shotRequestedChannel.Subscribe(ShotRequested);
            }
        }

        private void OnDisable()
        {
            if (shotRequestedChannel != null)
            {
                shotRequestedChannel.Unsubscribe(ShotRequested);
            }
        }

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
            if (_rb2D == null || direction.IsNearlyZero()) 
                return;
            
            direction.Normalize();
            float impulse = Mathf.Clamp01(power01) * maxImpulse;
            
            _rb2D.AddForce(direction * impulse, ForceMode2D.Impulse);
        }
        
        private void ShotRequested(ShotData shotData)
        {
            if (shotData.ShotTarget != (IShotTarget)this)
                return;
            
            if (shotData.ShotPower01 < 0)
            {
                return;
            }
            
            ApplyShot(shotData.ShotDirection, shotData.ShotPower01);
        }
    }
}
