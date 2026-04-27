using System;
using PoolGame.Gameplay.GameMode;
using PoolGame.Gameplay.Shooting;
using UnityEngine;

namespace PoolGame.Gameplay.Ball
{
    public class MovingBallsChecker : MonoBehaviour
    {
        [Serializable]
        private struct BallSpeedRule
        {
            public bool enabled;
            [Min(0f)] public float speedThreshold;
        }

        [Serializable]
        private struct MidTurnShotSettings
        {
            public bool enabled;
            public bool requireDelay;
            public bool requireBallsBelowSpeed;
            public bool requireAllEnabledConditions;
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
        [SerializeField] private MidTurnShotSettings midTurnShotSettings = new()
        {
            enabled = false,
            requireDelay = true,
            requireBallsBelowSpeed = false,
            requireAllEnabledConditions = true
        };
        [SerializeField, Min(0f)] private float delayBeforeNextShot = 0.5f;
        [SerializeField] private BallSpeedRule cueBallShotRule = new() { enabled = true, speedThreshold = 2f };
        [SerializeField] private BallSpeedRule objectBallShotRule = new() { enabled = false, speedThreshold = 2f };
        
        public Action OnBallsStoppedMoving;

        private bool _ballsInPlay;
        private float _lastShotTime;

        public bool BallsInPlay => _ballsInPlay;
        

        #region Lifecycle

        private void OnEnable()
        {
            if (playerShootingController)
                playerShootingController.OnShotTaken += OnShotTaken;
            if(gameState) 
                gameState.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            if (playerShootingController)
                playerShootingController.OnShotTaken -= OnShotTaken;
            if(gameState) 
                gameState.OnGameStateChanged -= OnGameStateChanged;
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

        #endregion

        #region Public Functions

        private void OnShotTaken()
        {
            _ballsInPlay = true;
            _lastShotTime = Time.time;
        }

        public bool CanTakeShot()
        {
            if (!_ballsInPlay)
                return true;

            return CanTakeMidTurnShot();
        }

        #endregion

        #region Threshold Checks

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
            BallType ballType = ball.GetBallType();

            return ballType switch
            {
                BallType.CueBall => cueBallRule.enabled && ball.IsMovingAboveSpeed(cueBallRule.speedThreshold),
                BallType.ObjectBall => objectBallRule.enabled && ball.IsMovingAboveSpeed(objectBallRule.speedThreshold),
                _ => false
            };
        }

        #endregion

        #region Mid Turn Rules

        private bool CanTakeMidTurnShot()
        {
            if (!midTurnShotSettings.enabled)
                return false;

            bool shouldCheckDelay = midTurnShotSettings.requireDelay;
            bool shouldCheckSpeed = midTurnShotSettings.requireBallsBelowSpeed;

            if (!shouldCheckDelay && !shouldCheckSpeed)
                return true;

            bool delaySatisfied = !shouldCheckDelay || HasDelayElapsed();
            bool speedSatisfied = !shouldCheckSpeed || AreShotSpeedRulesSatisfied();

            if (midTurnShotSettings.requireAllEnabledConditions)
                return delaySatisfied && speedSatisfied;

            return delaySatisfied || speedSatisfied;
        }

        private bool HasDelayElapsed()
        {
            return Time.time >= _lastShotTime + delayBeforeNextShot;
        }

        #endregion

        #region Stop

        private void ForceStopAllBalls()
        {
            if (ballContainer == null)
                return;

            foreach (BallController ball in ballContainer.ActiveBalls)
            {
                ball?.ForceStop();
            }
        }

        #endregion
    }
}
