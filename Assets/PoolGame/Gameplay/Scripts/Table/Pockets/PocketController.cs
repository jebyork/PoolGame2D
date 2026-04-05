using System;
using PoolGame.Core.Helpers;
using PoolGame.Gameplay.Ball;
using UnityEngine;

namespace PoolGame.Gameplay.Table.Pockets
{
    public class PocketController : MonoBehaviour
    {
        [SerializeField] private LayerMask ballLayers;
        [SerializeField] private BallPocketedChannel ballPocketedEvent;
        [SerializeField, Range(0f, 1f)] private float ballOverlapMinPercentage = 0.4f;
        
        private CircleCollider2D _pocketCollider;


        private void Start()
        {
            _pocketCollider = GetComponent<CircleCollider2D>();
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
            BallPocketedEvent pocketedEvt = new BallPocketedEvent
            {
                PottedBall = ball.GetComponent<BallController>() ,
                Pocket = this
            };
            ballPocketedEvent?.RaiseEvent(pocketedEvt);
        }
    }
}
