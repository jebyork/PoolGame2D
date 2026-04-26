using System;
using PoolGame.Gameplay.Attributes;
using PoolGame.Gameplay.Pockets;
using UnityEngine;

namespace PoolGame.Gameplay.GameMode
{
    public class TurnEvaluator : MonoBehaviour, ITurnOutcomeHandler
    {
        [Header("Components")]
        [SerializeField] private GameState gameState;
        [SerializeField] private TurnModifiers turnModifiers;
        [SerializeField] private Life life;
        [SerializeField] private Score score;

        [Header("Events")]
        [SerializeField] private BallPocketedChannel ballPocketedEvent;

        private int _objectBallsPocketed;
        private int _cueBallsPocketed;

        private void OnEnable()
        {
            ballPocketedEvent?.Subscribe(OnBallPocketed);
            gameState.RegisterHandler(this);
        }

        private void OnDisable()
        {
            ballPocketedEvent?.Unsubscribe(OnBallPocketed);
            gameState.UnregisterHandler(this);
        }

        private void Update()
        {
            Logwin.Log("Object Balls Pocketed", _objectBallsPocketed, "Turn Evaluator");
            Logwin.Log("Cue Balls Pocketed", _cueBallsPocketed, "Turn Evaluator");
        }

        private void OnBallPocketed(BallPocketedEvent evt)
        {
            Debug.Log("Ball Pocketed");

            if (evt.PottedBall == null)
                return;

            switch (evt.PottedBall.GetBallType())
            {
                case BallType.ObjectBall:
                    _objectBallsPocketed++;
                    break;
                case BallType.CueBall:
                    _cueBallsPocketed++;
                    break;
            }
        }

        public void OnTurnEvaluate(Action onComplete)
        {
            if (_objectBallsPocketed == 0 || _cueBallsPocketed > 0)
                life.DecreaseAttribute(Mathf.RoundToInt(turnModifiers.lifePunishment.Value));

            if (_objectBallsPocketed > 0)
                score.IncreaseAttribute(Mathf.RoundToInt(turnModifiers.scorePerObjectBall.Value * _objectBallsPocketed));

            _objectBallsPocketed = 0;
            _cueBallsPocketed = 0;

            turnModifiers.UpdateTurn();
            onComplete();
        }
    }
}
