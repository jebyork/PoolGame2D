using System;
using UnityEngine;

[Serializable]
public enum BallType
{
    CueBall,
    ObjectBall
}

namespace PoolGame.Gameplay.Ball
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] private float stopSpeed = .5f;
        [SerializeField] private BallType ballType = BallType.ObjectBall;
        public BallType BallType => ballType;

        private Rigidbody2D _rigidbody;

        public bool IsMoving { get; private set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            if (_rigidbody == null)
            {
                Debug.LogError("[Ball Controller] No RigidBody on ball.", this);
            }
        }

        private void Update()
        {
            CheckIsMoving();
            StopVelocityAtLowSpeed();
        }

        public void Activate(Vector3 position, Transform parent)
        {
            transform.SetParent(parent);
            transform.SetPositionAndRotation(position, Quaternion.identity);
            ResetState();

            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }

        public void Deactivate()
        {
            ResetState();

            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }

        private void StopVelocityAtLowSpeed()
        {
            if (_rigidbody == null)
                return;

            float speed = _rigidbody.linearVelocity.magnitude;
            if (!(speed < stopSpeed)) return;

            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.angularVelocity = 0f;
            IsMoving = false;
        }

        private void CheckIsMoving()
        {
            if (_rigidbody == null || IsMoving)
                return;

            float speed = _rigidbody.linearVelocity.magnitude;
            if (speed > stopSpeed)
            {
                IsMoving = true;
            }
        }

        private void ResetState()
        {
            if (_rigidbody == null)
                return;

            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.angularVelocity = 0f;
            IsMoving = false;
        }
    }
}
