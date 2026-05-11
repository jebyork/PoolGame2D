using System;
using PoolGame.Gameplay.Ball;
using PoolGame.Gameplay.Pickups.Effects;
using UnityEngine;

namespace PoolGame.Gameplay.Pickups
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Pickup : MonoBehaviour
    {
        private PickupEffect _effect;
        private EBallType _eBallType;
        private bool _anyBall;
        private string _broadcastMessage;
        
		public static event Action<string> OnPickupAcquired;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_effect == null)
                return;
            
            if(!other.TryGetComponent(out BallController ball))
                return;

            if (ball.GetBallType() != _eBallType || _anyBall) 
                return;
            
            _effect.PlayEffect(gameObject);
            OnPickupAcquired?.Invoke(_broadcastMessage);
            Destroy(gameObject);
        }

        public void SetEffect(PickupEffect effect, EBallType eBallType, string broadcastMessage ,bool anyBall = false)
        {
            _anyBall = anyBall;
            _effect = effect;
            _eBallType = eBallType;
            _broadcastMessage = broadcastMessage;
        }
    }
}
