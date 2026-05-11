using System.Collections;
using PoolGame.Gameplay.Attributes;
using PoolGame.Gameplay.UI;
using PoolGame.Gameplay.Visual;
using UnityEngine;

namespace PoolGame.Gameplay.GameFlow
{
    public class GameFlowController : MonoBehaviour
    {
        [SerializeField] SceneIntroPlayer introPlayer;
        [SerializeField] GameState gameState;
        [SerializeField] GameOverMenu gameOverMenu;
        
        [SerializeField] Life life;
        [SerializeField] Score score;
        [SerializeField] Turn turn;

        
        void Awake()
        {
            if(!gameState)
                Debug.LogError($"[{gameObject.name}] GameController: No GameState assigned.");
            if(!introPlayer)
                Debug.LogError($"[{gameObject.name}] GameController: No IntroPlayer assigned.");
        }

        IEnumerator Start()
        {
            if (!gameState)
                yield break;

            gameState.Set(EGameState.Entry);
            
            if (gameOverMenu)
                gameOverMenu.Hide();

            if (introPlayer)
                yield return StartCoroutine(introPlayer.PlayIntro());

            gameState.Set(EGameState.Setup);

            SetupGame();

            gameState.Set(EGameState.InPlay);

        }

        void SetupGame()
        {
            if (gameOverMenu)
                gameOverMenu.Hide();
            
            life.ResetAttribute();
            score.ResetAttribute();
            turn.ResetAttribute();
            
            gameState.Set(EGameState.InPlay);
        }
        
        public void ResetGame() => SetupGame();
        
        // public bool CanShoot()
        // {
        //     if (!gameState || !gameMode)
        //         return false;
        //
        //     if (gameState.Current == GameStateEnum.AwaitingTurn)
        //         return true;
        //
        //     if (gameState.Current == GameStateEnum.TurnInProgress)
        //         return ballsChecker != null && ballsChecker.CanTakeShot();
        //
        //     return false;
        // }
        //
        // public bool TryStartShot()
        // {
        //     if (!gameState || !gameMode)
        //         return false;
        //
        //     if (gameState.Current == GameStateEnum.AwaitingTurn)
        //     {
        //         gameState.Set(GameStateEnum.TurnInProgress);
        //         ballsChecker?.NotifyShotTaken();
        //         return true;
        //     }
        //
        //     if (gameState.Current == GameStateEnum.TurnInProgress)
        //     {
        //         if (ballsChecker == null || !ballsChecker.CanTakeShot())
        //             return false;
        //
        //         CompleteTurn();
        //
        //         if (gameState.Current == GameStateEnum.Finished)
        //             return false;
        //
        //         gameState.Set(GameStateEnum.TurnInProgress);
        //         ballsChecker.NotifyShotTaken();
        //         return true;
        //     }
        //
        //     return false;
        // }

        // private void HandleBallsStoppedMoving()
        // {
        //     if (gameState.Current != GameStateEnum.TurnInProgress)
        //         return;
        //
        //     CompleteTurn();
        // }

        // private void CompleteTurn()
        // {
        //     gameState.AdvanceTurn();
        //     gameState.Set(GameStateEnum.TurnEvaluation);
        //
        //     gameMode.EvaluateTurn();
        //
        //     if (gameMode.IsGameOver())
        //     {
        //         gameState.Set(GameStateEnum.Finished);
        //         if (pickupManager)
        //             pickupManager.RemoveAll();
        //         if (gameOverMenu)
        //             gameOverMenu.Show();
        //         return;
        //     }
        //
        //     gameState.Set(GameStateEnum.AwaitingTurn);
        // }

    }
}
