using PoolGame.Core;
using PoolGame.Core.Observers;
using PoolGame.Core.Setup;
using PoolGame.Gameplay.Table.Pockets;
using UnityEngine;

namespace PoolGame.Gameplay.GameMode
{
    public class PotAllGameMode : GenericSingleton<PotAllGameMode>, ISetupControl
    {
        [SerializeField] private LayerMask objectBallLayer;
        [SerializeField] private LayerMask cueBallLayer;

        [Space] 
        [SerializeField] private ObservableBool playerCanGoBool;

        private int _score = 0;
        
        public void BallPocketed(BallPocketedEvent evt)
        {
            
        }

        public void Initialize()
        {
            _score = 0;
        }

        public void CreateObjects()
        {
            
        }

        public void Prepare()
        {
        }

        public void StartGame()
        {
        }
    }
}