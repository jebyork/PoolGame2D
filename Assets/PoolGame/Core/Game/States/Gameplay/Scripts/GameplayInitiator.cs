using PoolGame.Core.Game.States.Gameplay.Ball;
using PoolGame.Core.Game.States.Gameplay.Table;
using UnityEngine;
using UnityEngine.Serialization;

namespace PoolGame.Core.Game.States.Gameplay
{
    public class GameplayInitiator : MonoBehaviour
    {
        [FormerlySerializedAs("gameManager")] [SerializeField] private GameplayManager gameplayManager;
        [SerializeField] private BallManager ballManager;
        [SerializeField] private TableManager tableManagerPrefab;
        
        private GameplayManager _gameplayManager;
        private BallManager _ballManager;
        private TableManager _tableManager;
        private void Start()
        {
            BindObjects();
            InitializeObjects();
            CreateObjects();
            PrepareGame();
            BeginGame();
        }

        private void BindObjects()
        {
            _gameplayManager = Instantiate(gameplayManager, transform);
            _ballManager = Instantiate(ballManager, transform);
            _tableManager = Instantiate(tableManagerPrefab, transform);
        }

        private void InitializeObjects()
        {
            _gameplayManager.ChangeGameState(GameState.Setup);
        }
        
        private void CreateObjects()
        {
            _ballManager.CreateBalls();
            _tableManager.CreateTable();
        }

        private void PrepareGame()
        {
            _ballManager.SetInitialBallPosition();
            _ballManager.SetAllBallActiveness(false);
            
            _tableManager.SetTablePosition();
            _tableManager.SetTableActiveness(false);
        }

        private void BeginGame()
        {
            _ballManager.SetAllBallActiveness(true);
            _tableManager.SetTableActiveness(true);
            
            _gameplayManager.ChangeGameState(GameState.PlayerTurn);
        }
    }
}
