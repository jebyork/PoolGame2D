using PoolGame.Core.Helpers;
using PoolGame.Gameplay.Ball;
using UnityEngine;

namespace PoolGame.Gameplay.Table.Pockets
{
    public class PocketController : MonoBehaviour
    {
        [SerializeField] private LayerMask ballLayers;
        [SerializeField] private BallPocketedChannel ballPocketedEvent;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!ballLayers.ContainsLayer(other.gameObject.layer)) return;
            Debug.Log("Ball Pocketed");
            BallPocketedEvent pocketedEvt = new BallPocketedEvent
            {
                PottedBall = other.GetComponent<BallController>() ,
                Pocket = this
            };
            ballPocketedEvent?.RaiseEvent(pocketedEvt);
        }
    }
}
