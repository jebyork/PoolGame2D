using PoolGame.Core.Helpers;
using PoolGame.Gameplay.Ball;
using UnityEngine;

namespace PoolGame.Gameplay.Pockets
{
    public class PocketController : MonoBehaviour
    {
        [SerializeField] private LayerMask ballLayers;
        [SerializeField] private BallPocketedChannel ballPocketedEvent;
        [SerializeField, Range(0f, 1f)] private float ballOverlapMinPercentage = 0.4f;
        [SerializeField] private BallContainer ballContainer;
        
        private CircleCollider2D _pocketCollider;
        
        private void Start()
        {
            _pocketCollider = GetComponent<CircleCollider2D>();
            
            if (ballContainer != null) return;
            ballContainer = FindFirstObjectByType<BallContainer>();
            if (ballContainer == null)
                Debug.LogError("[PocketController] Ball Container is missing on PocketController.");
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!ballLayers.ContainsLayer(other.gameObject.layer)) return;
            
            if (other is not CircleCollider2D ballCircle)
                return;
            
            float overlapPercentage =
                _pocketCollider.GetPercentageOfCircleInside(ballCircle);
            
            Logwin.Log($"{other.gameObject.name} pocket Overlap", overlapPercentage, "Pockets");

            if (overlapPercentage < ballOverlapMinPercentage)
                return;
            
            BroadcastPocketedEvent(other);
        }

        
        private void BroadcastPocketedEvent(Collider2D ball)
        {
            BallController ballController = ball.GetComponent<BallController>();
            
            BallPocketedEvent pocketedEvt = new BallPocketedEvent
            {
                PottedBall = ballController ,
                Pocket = this
            };
            ballPocketedEvent?.RaiseEvent(pocketedEvt);
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
        
    }
}
