using PoolGame.Gameplay.Ball;
using UnityEngine;

namespace PoolGame.Gameplay.Pickups
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Pickup : MonoBehaviour
    {
        private PickupEffect _effect;
        private BallType _ballType;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_effect == null)
                return;
            
            if(!other.TryGetComponent(out BallController ball))
                return;

            if (ball.GetBallType() != _ballType) 
                return;
            
            _effect.PlayEffect(gameObject);
            Destroy(gameObject);
        }

        public void SetEffect(PickupEffect effect, BallType ballType)
        {
            _effect = effect;
            _ballType = ballType;
        }
    }
}
