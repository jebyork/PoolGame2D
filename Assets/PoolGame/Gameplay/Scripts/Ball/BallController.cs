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
        [SerializeField] private BallType ballType = BallType.ObjectBall;
        public BallType GetBallType() => ballType;

        protected Rigidbody2D Rb2D;
        
        private void Awake()
        {
            Rb2D = GetComponent<Rigidbody2D>();
            if (Rb2D == null)
            {
                Debug.LogError("[Ball Controller] No RigidBody on ball.", this);
            }
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

        public void ForceStop()
        {
            if (Rb2D == null)
                return;
            
            Rb2D.linearVelocity = Vector2.zero;
            Rb2D.angularVelocity = 0f;
        }
        
        private void ResetState()
        {
            ForceStop();
        }

        public bool IsMovingAboveSpeed(float speedThreshold)
        {
            return Rb2D != null && Rb2D.linearVelocity.magnitude > speedThreshold;
        } 
    }
}
