using Cysharp.Threading.Tasks;
using UnityEngine;


namespace PoolGame.Managers
{
    public class GameInitiator : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private BallManager ballManager;
        [SerializeField] private TableManager tableManagerPrefab;
        
        private GameManager _gameManager;
        private BallManager _ballManager;
        private TableManager _tableManager;
        private async void Start()
        {
            BindObjects();
            await InitializeObjects();
            await CreateObjects();
            PrepareGame();
            await BeginGame();
        }

        private void BindObjects()
        {
            _gameManager = Instantiate(gameManager, transform);
            _ballManager = Instantiate(ballManager, transform);
            _tableManager = Instantiate(tableManagerPrefab, transform);
        }

        private async UniTask InitializeObjects()
        {
            _gameManager.ChangeGameState(GameState.Setup);
        }
        
        private async UniTask CreateObjects()
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

        private async UniTask BeginGame()
        {
            _ballManager.SetAllBallActiveness(true);
            _tableManager.SetTableActiveness(true);
            
            _gameManager.ChangeGameState(GameState.PlayerTurn);
        }
    }
}
