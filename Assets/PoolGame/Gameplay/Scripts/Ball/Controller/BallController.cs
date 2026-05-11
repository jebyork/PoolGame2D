using System;
using PoolGame.Core.Helpers;
using UnityEngine;

[Serializable]
public enum EBallType
{
    CueBall,
    ObjectBall
}

namespace PoolGame.Gameplay.Ball
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] private EBallType eBallType = EBallType.ObjectBall;
        public EBallType GetBallType() => eBallType;

        protected Rigidbody2D Rb2D;
        
        
        #region Lifecycle

        private void Reset()
        {
            Rb2D = GetComponent<Rigidbody2D>();
        }
        
        private void Awake()
        {
            if (Rb2D == null)
            {
                Rb2D = GetComponent<Rigidbody2D>();
                if (Rb2D == null)
                    Debug.LogError("[Ball Controller] No RigidBody on ball.", this);
            }
        }
        
        #endregion
        
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
        
        void ResetState()
        {
            ForceStop();
        }

        public bool IsMovingAboveSpeed(float speedThreshold)
        {
            float speed = Rb2D.linearVelocity.magnitude;
            return speed > speedThreshold;
        } 
    }
}
