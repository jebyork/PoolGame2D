using PoolGame.Core;
using PoolGame.Core.Helpers;
using PoolGame.Core.Observers;
using PoolGame.Gameplay.Ball;
using PoolGame.Gameplay.Table.Pockets;
using UnityEngine;

namespace PoolGame.Gameplay.GameMode
{
    public class PotAllGameMode : GenericSingleton<PotAllGameMode>
    {
        [SerializeField] private LayerMask objectBallLayer;
        [SerializeField] private LayerMask cueBallLayer;

        [Space] 
        [SerializeField] private ObservableBool playerCanGoBool;

        private int _score = 0;
        
        public void BallPocketed(BallPocketedEvent evt)
        {
            if (objectBallLayer.ContainsLayer(evt.PottedBall.gameObject.layer))
            {
                _score++;
                Logwin.Log("Score", _score);
            }
            else
            {
                _score--;
                BallManager.Instance.ResetCueBall();
                Logwin.Log("Score", _score);
            }
        }
    }
}