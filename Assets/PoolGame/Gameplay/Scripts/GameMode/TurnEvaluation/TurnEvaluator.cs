using System;
using PoolGame.Core.Helpers;
using PoolGame.Gameplay.Attributes;
using PoolGame.Gameplay.Ball;
using PoolGame.Gameplay.Pockets;
using UnityEngine;

namespace PoolGame.Gameplay.GameMode.TurnEvaluation
{
    public class TurnEvaluator : MonoBehaviour, ITurnOutcomeHandler
    {
        [Header("Components")]
        [SerializeField] private GameState gameState;
        [SerializeField] private TurnModifiers turnModifiers;
        [SerializeField] private Life life;
        [SerializeField] private Score score;
        
        private int _objectBallsPocketed;
        private int _cueBallsPocketed;

        #region Lifecycle 

        private void OnEnable()
        {
            PocketController.OnBallPocketed += OnBallPocketed;
            gameState.RegisterHandler(this);
        }
        
        private void OnDisable()
        {
            PocketController.OnBallPocketed += OnBallPocketed;
            gameState.UnregisterHandler(this);
        }

        private void Update()
        {
            Logwin.Log("Object Balls Pocketed", _objectBallsPocketed, "Turn Evaluator");
            Logwin.Log("Cue Balls Pocketed", _cueBallsPocketed, "Turn Evaluator");
        }
        
        #endregion

        private void OnBallPocketed(BallController ball, PocketController pocket)
        {
            if (ball == null)
                return;

            switch (ball.GetBallType())
            {
                case BallType.ObjectBall:
                    _objectBallsPocketed++;
                    break;
                case BallType.CueBall:
                    _cueBallsPocketed++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnTurnEvaluate(Action onComplete)
        {
            if (HasReasonToReduceLife())
                life.DecreaseAttribute(Mathf.RoundToInt(turnModifiers.lifePunishment.Value));

            if (HasReasonToIncreaseScore())
                score.IncreaseAttribute(Mathf.RoundToInt(turnModifiers.scorePerObjectBall.Value * _objectBallsPocketed));

            _objectBallsPocketed = 0;
            _cueBallsPocketed = 0;

            turnModifiers.UpdateTurn();
            onComplete();
        }

        private bool HasReasonToIncreaseScore()
        {
            return _objectBallsPocketed > 0;
        }

        private bool HasReasonToReduceLife()
        {
            return _objectBallsPocketed == 0 || _cueBallsPocketed > 0;
        }
    }
}
