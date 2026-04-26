using System;
using PoolGame.Gameplay.GameMode;
using PoolGame.Gameplay.Shooting;
using UnityEngine;

namespace PoolGame.Gameplay.Ball
{
    public enum MidTurnShotRule
    {
        Disabled,
        AfterDelay,
        WhenBallsBelowSpeed,
        AfterDelayAndBallsBelowSpeed,
        AfterDelayOrBallsBelowSpeed
    }

    public class MovingBallsChecker : MonoBehaviour
    {
        [Serializable]
        private struct BallSpeedRule
        {
            public bool enabled;
            [Min(0f)] public float speedThreshold;
        }

        [Header("Components")] 
        [SerializeField] private PlayerShootingController playerShootingController;
        [SerializeField] private BallContainer ballContainer;
        [SerializeField] private GameState gameState;

        [Header("Turn Complete")]
        [SerializeField] private bool forceStopBallsWhenTurnCompletes = true;
        [SerializeField] private BallSpeedRule cueBallStopRule = new() { enabled = true, speedThreshold = 0.3f };
        [SerializeField] private BallSpeedRule objectBallStopRule = new() { enabled = true, speedThreshold = 0.3f };

        [Header("Mid-Turn Shot")]
        [SerializeField] private MidTurnShotRule midTurnShotRule = MidTurnShotRule.Disabled;
        [SerializeField, Min(0f)] private float delayBeforeNextShot = 0.5f;
        [SerializeField] private BallSpeedRule cueBallShotRule = new() { enabled = true, speedThreshold = 2f };
        [SerializeField] private BallSpeedRule objectBallShotRule = new() { enabled = false, speedThreshold = 2f };

        public Action OnBallsStoppedMoving;

        private bool _ballsInPlay;
        private float _lastShotTime;

        public bool BallsInPlay => _ballsInPlay;

        private void OnEnable()
        {
            if (playerShootingController)
                playerShootingController.OnShotTaken += OnShotTaken;
            if(gameState) 
                gameState.onGameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            if (playerShootingController)
                playerShootingController.OnShotTaken -= OnShotTaken;
            if(gameState) 
                gameState.onGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameStateEnum state)
        {
            if (state != GameStateEnum.Finished)
                return;

            _ballsInPlay = false;
            ForceStopAllBalls();
        }

        private void FixedUpdate()
        {
            if (!_ballsInPlay)
                return;

            if (AnyBallAboveStopThreshold())
                return;

            if (forceStopBallsWhenTurnCompletes)
                ForceStopAllBalls();

            _ballsInPlay = false;
            OnBallsStoppedMoving?.Invoke();
        }

        private void OnShotTaken()
        {
            _ballsInPlay = true;
            _lastShotTime = Time.time;
        }

        public bool CanTakeShot()
        {
            if (!_ballsInPlay)
                return true;

            return midTurnShotRule switch
            {
                MidTurnShotRule.Disabled => false,
                MidTurnShotRule.AfterDelay => HasDelayElapsed(),
                MidTurnShotRule.WhenBallsBelowSpeed => AreShotSpeedRulesSatisfied(),
                MidTurnShotRule.AfterDelayAndBallsBelowSpeed => HasDelayElapsed() && AreShotSpeedRulesSatisfied(),
                MidTurnShotRule.AfterDelayOrBallsBelowSpeed => HasDelayElapsed() || AreShotSpeedRulesSatisfied(),
                _ => false
            };
        }

        private bool AnyBallAboveStopThreshold()
        {
            return AnyBallMatchesThresholdRule(IsAboveStopThreshold);
        }

        private bool AreShotSpeedRulesSatisfied()
        {
            return !AnyBallMatchesThresholdRule(IsAboveShotThreshold);
        }

        private bool AnyBallMatchesThresholdRule(Func<BallController, bool> matchesRule)
        {
            if (ballContainer == null)
                return false;

            foreach (BallController ball in ballContainer.ActiveBalls)
            {
                if (ball != null && matchesRule(ball))
                    return true;
            }

            return false;
        }

        private bool IsAboveStopThreshold(BallController ball)
        {
            return IsBallAboveThreshold(ball, cueBallStopRule, objectBallStopRule);
        }

        private bool IsAboveShotThreshold(BallController ball)
        {
            return IsBallAboveThreshold(ball, cueBallShotRule, objectBallShotRule);
        }

        private static bool IsBallAboveThreshold(
            BallController ball,
            BallSpeedRule cueBallRule,
            BallSpeedRule objectBallRule)
        {
            return ball.GetBallType() switch
            {
                BallType.CueBall => cueBallRule.enabled && ball.IsMovingAboveSpeed(cueBallRule.speedThreshold),
                BallType.ObjectBall => objectBallRule.enabled && ball.IsMovingAboveSpeed(objectBallRule.speedThreshold),
                _ => false
            };
        }

        private bool HasDelayElapsed()
        {
            return Time.time >= _lastShotTime + delayBeforeNextShot;
        }

        private void ForceStopAllBalls()
        {
            if (ballContainer == null)
                return;

            foreach (BallController ball in ballContainer.ActiveBalls)
            {
                ball?.ForceStop();
            }
        }
    }
}
