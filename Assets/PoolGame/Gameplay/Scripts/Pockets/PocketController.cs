using System;
using PoolGame.Core.Helpers;
using PoolGame.Gameplay.Ball;
using UnityEngine;

namespace PoolGame.Gameplay.Pockets
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class PocketController : MonoBehaviour
    {
        [SerializeField] private LayerMask ballLayers;
        [SerializeField, Range(0f, 1f)] private float ballOverlapMinPercentage = 0.4f;
        [SerializeField] private BallContainer ballContainer;
        
        private CircleCollider2D _pocketCollider;
        
        public static event Action<BallController, PocketController> OnBallPocketed;


        #region Lifecycle

        private void Reset()
        {
            _pocketCollider = GetComponent<CircleCollider2D>();

            if (ballContainer == null) 
                ballContainer = FindFirstObjectByType<BallContainer>();
        }

        private void Awake()
        {
            _pocketCollider = GetComponent<CircleCollider2D>();
        }

        #endregion
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (!ballLayers.ContainsLayer(other.gameObject.layer)) return;

            if (_pocketCollider == null)
                return;
            
            if (other is not CircleCollider2D ballCircle)
                return;
            
            float overlapPercentage =
                _pocketCollider.GetPercentageOfCircleInside(ballCircle);
            
            Log(other.gameObject, overlapPercentage);

            if (overlapPercentage < ballOverlapMinPercentage)
                return;
            
            BroadcastPocketedEvent(other);
        }
        
        private void BroadcastPocketedEvent(Collider2D ball)
        {
            BallController ballController = ball.GetComponent<BallController>();
            
            OnBallPocketed?.Invoke(ballController, this);
            RemoveBall(ballController);
        }

        private void RemoveBall(BallController ball)
        {
            if (ballContainer == null)
            {
                Destroy(ball.gameObject);
                return;
            }
            
            ballContainer.ReleaseBall(ball);
        }

        private void Log(GameObject overlappingObj, float overlapPercentage)
        {
            Logwin.Log($"{overlappingObj.name} pocket Overlap", overlapPercentage, $"{gameObject.name}");
        }
    }
}
