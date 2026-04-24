using System;
using System.Collections.Generic;
using PoolGame.Gameplay.Ball;
using PoolGame.Gameplay.Shooting;
using UnityEngine;

namespace PoolGame.Gameplay.GameMode
{
    public enum GameStateEnum
    {
        Starting,
        AwaitingTurn,
        TurnInProgress,
        TurnEvaluation,
        Finished
    }

    public class GameState : MonoBehaviour
    {
        [SerializeField] private PlayerShootingController playerShootingController;
        [SerializeField] private MovingBallsChecker movingBallsChecker;

        private GameStateEnum _gameState;
        public GameStateEnum CurrentGameState { get => _gameState;
            private set
            {
                _gameState = value;
                onGameStateChanged?.Invoke(_gameState);
            }}

        public Action<GameStateEnum> onGameStateChanged;

        private int _turn;
        public int Turn => _turn;

        private readonly List<ITurnOutcomeHandler> _turnOutcomeHandlers = new();

        private void Update()
        {
            Logwin.Log("Turn" , Turn, "Game State");
            Logwin.Log("Game State", CurrentGameState, "Game State");
        }

        private void OnEnable()
        {
            if (playerShootingController)
                playerShootingController.OnShotTaken += ShotTaken;
            if (movingBallsChecker)
                movingBallsChecker.OnBallsStoppedMoving += BallsStopped;
        }

        private void OnDisable()
        {
            if (playerShootingController)
                playerShootingController.OnShotTaken -= ShotTaken;

            if (movingBallsChecker)
                movingBallsChecker.OnBallsStoppedMoving -= BallsStopped;
        }

        private void Start()
        {
            CurrentGameState = GameStateEnum.AwaitingTurn;
        }

        public void RegisterHandler(ITurnOutcomeHandler handler) => _turnOutcomeHandlers.Add(handler);
        public void UnregisterHandler(ITurnOutcomeHandler handler) => _turnOutcomeHandlers.Remove(handler);

        private void ShotTaken()
        {
            if(CurrentGameState == GameStateEnum.Finished)
                return;

            CurrentGameState = GameStateEnum.TurnInProgress;
        }

        private void BallsStopped()
        {
            if(CurrentGameState == GameStateEnum.Finished)
                return;

            _turn++;
            CurrentGameState = GameStateEnum.TurnEvaluation;
            BeginTurnEvaluation();
        }

        private void BeginTurnEvaluation()
        {
            if (_turnOutcomeHandlers.Count == 0)
            {
                CurrentGameState = GameStateEnum.AwaitingTurn;
                return;
            }

            int remaining = _turnOutcomeHandlers.Count;
            foreach (var handler in _turnOutcomeHandlers)
            {
                handler.OnTurnEvaluate(() =>
                {
                    remaining--;
                    if (remaining == 0 && CurrentGameState != GameStateEnum.Finished)
                        CurrentGameState = GameStateEnum.AwaitingTurn;
                });
            }
        }

        public void SetGameOver()
        {
            CurrentGameState = GameStateEnum.Finished;
        }
    }
}