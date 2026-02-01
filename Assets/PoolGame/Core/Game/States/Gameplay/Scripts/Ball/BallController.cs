using UnityEngine;

namespace PoolGame.Core.Game.States.Gameplay.Ball
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] private float stopSpeed = .5f;
        
        private Rigidbody2D _rigidbody;

        public bool IsMoving
        {
            get;
            private set;
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            if (_rigidbody == null)
            {
                Debug.LogError("[Ball Controller] No RigidBody on ball." , this);
            }
        }

        private void Update()
        {
            CheckIsMoving();
            StopVelocityAtLowSpeed();
        }
        private void StopVelocityAtLowSpeed()
        {
            if (_rigidbody == null) 
                return;
            
            float speed = _rigidbody.linearVelocity.magnitude;
            if (speed < stopSpeed)
            {
                _rigidbody.linearVelocity = Vector2.zero;
                _rigidbody.angularVelocity = 0;
                IsMoving = false;
            }
        }

        private void CheckIsMoving()
        {
            if (_rigidbody == null || IsMoving) 
                return;
            
            float speed = _rigidbody.linearVelocity.magnitude;
            if (speed > stopSpeed)
                IsMoving = true;
        }
    }
}
