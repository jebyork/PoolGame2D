
using PoolGame.Core.Helpers;
using PoolGame.Gameplay.Ball;
using UnityEngine;

namespace PoolGame.Gameplay.Table.Pockets
{
    public class PocketController : MonoBehaviour
    {
        [SerializeField] private LayerMask ballLayers;
        [SerializeField] private BallPocketedChannel ballPocketedEvent;

        private void OnTriggerEnter(Collider other)
        {
            if (!ballLayers.ContainsLayer(other.gameObject.layer)) return;
            BallPocketedEvent pocketedEvt = new BallPocketedEvent();
            pocketedEvt.PottedBall = other.GetComponent<BallController>();
            pocketedEvt.Pocket = this;
                
            ballPocketedEvent?.RaiseEvent(pocketedEvt);
        }
    }
}
