using System.Collections;
using PoolGame.Gameplay.Attributes;
using PoolGame.Gameplay.Ball;
using PoolGame.Gameplay.Pickups;
using PoolGame.Gameplay.Shooting;
using PoolGame.Gameplay.UI;
using PoolGame.Gameplay.Visual;
using UnityEngine;

namespace PoolGame.Gameplay.GameMode
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private SceneIntroPlayer introPlayer;
        [SerializeField] private PlayerShootingController shooter;
        [SerializeField] private MovingBallsChecker ballsChecker;
        [SerializeField] private PotAllGameMode gameMode;
        [SerializeField] private GameState gameState;
        [SerializeField] private GameOverMenu gameOverMenu;
        [SerializeField] private PickupManager pickupManager;

        private void OnEnable()
        {
            if (ballsChecker)
                ballsChecker.OnBallsStoppedMoving += HandleBallsStoppedMoving;
        }

        private void OnDisable()
        {
            if (ballsChecker)
                ballsChecker.OnBallsStoppedMoving -= HandleBallsStoppedMoving;
        }

        private void Awake()
        {
            if(!gameState)
                Debug.LogError($"[{gameObject.name}] GameController: No GameState assigned.");
            if(!gameMode)
                Debug.LogError($"[{gameObject.name}] GameController: No GameMode assigned.");
            if(!introPlayer)
                Debug.LogError($"[{gameObject.name}] GameController: No IntroPlayer assigned.");
            if(!ballsChecker)
                Debug.LogError($"[{gameObject.name}] GameController: No BallsChecker assigned.");
            if(!shooter)
                Debug.LogError($"[{gameObject.name}] GameController: No Shooter assigned.");
        }

        private IEnumerator Start()
        {
            if (!gameState)
                yield break;

            gameState.Set(GameStateEnum.Entry);
            
            if (gameOverMenu)
                gameOverMenu.Hide();

            if (introPlayer)
                yield return StartCoroutine(introPlayer.PlayIntro());

            gameState.Set(GameStateEnum.Starting);

            if (gameMode)
                gameMode.StartGame();

            gameState.Set(GameStateEnum.AwaitingTurn);

        }

        public void RestartGame()
        {
            if (gameOverMenu)
                gameOverMenu.Hide();
            
            StartCoroutine(RestartGameRoutine());
        }

        private IEnumerator RestartGameRoutine()
        {
            if (!gameState || !gameMode)
                yield break;

            gameState.Set(GameStateEnum.Starting);

            gameMode.StartGame();

            gameState.Set(GameStateEnum.AwaitingTurn);
        }

        public bool CanShoot()
        {
            if (!gameState || !gameMode)
                return false;

            if (gameState.Current == GameStateEnum.AwaitingTurn)
                return true;

            if (gameState.Current == GameStateEnum.TurnInProgress)
                return ballsChecker != null && ballsChecker.CanTakeShot();

            return false;
        }

        public bool TryStartShot()
        {
            if (!gameState || !gameMode)
                return false;

            if (gameState.Current == GameStateEnum.AwaitingTurn)
            {
                gameState.Set(GameStateEnum.TurnInProgress);
                ballsChecker?.NotifyShotTaken();
                return true;
            }

            if (gameState.Current == GameStateEnum.TurnInProgress)
            {
                if (ballsChecker == null || !ballsChecker.CanTakeShot())
                    return false;

                CompleteTurn();

                if (gameState.Current == GameStateEnum.Finished)
                    return false;

                gameState.Set(GameStateEnum.TurnInProgress);
                ballsChecker.NotifyShotTaken();
                return true;
            }

            return false;
        }

        private void HandleBallsStoppedMoving()
        {
            if (gameState.Current != GameStateEnum.TurnInProgress)
                return;

            CompleteTurn();
        }

        private void CompleteTurn()
        {
            gameState.AdvanceTurn();
            gameState.Set(GameStateEnum.TurnEvaluation);

            gameMode.EvaluateTurn();

            if (gameMode.IsGameOver())
            {
                gameState.Set(GameStateEnum.Finished);
                if (pickupManager)
                    pickupManager.RemoveAll();
                if (gameOverMenu)
                    gameOverMenu.Show();
                return;
            }

            gameState.Set(GameStateEnum.AwaitingTurn);
        }

    }
}
