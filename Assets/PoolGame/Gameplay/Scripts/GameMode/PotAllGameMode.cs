using PoolGame.Core.Events;
using PoolGame.Gameplay.Ball;
using PoolGame.Gameplay.Table.Pockets;
using UnityEngine;
using UnityEngine.Serialization;

namespace PoolGame.Gameplay.GameMode
{
    public class PotAllGameMode : MonoBehaviour
    {
        [FormerlySerializedAs("spawnObjectBallEvent")]
        [SerializeField] private VoidEventChannel spawnObjectBallsCommand;
        [FormerlySerializedAs("spawnCueBallEvent")]
        [SerializeField] private VoidEventChannel spawnCueBallCommand;
        [SerializeField] private BallPocketedChannel ballPocketedEvent;
        [SerializeField] private VoidEventChannel shotTakenEvent;
        [SerializeField] private VoidEventChannel ballsStoppedEvent;
        
        [SerializeField] private BallContainer ballContainer;

        private int _score;
        private bool _cueBallPottedThisShot;
        private bool _ballsInPlay;
        
        private void OnEnable()
        {
            ballPocketedEvent?.Subscribe(BallPocketed);
            ballsStoppedEvent?.Subscribe(BallsStoppedMoving);
            shotTakenEvent?.Subscribe(ShotTaken);
        }

        private void OnDisable()
        {
            ballPocketedEvent?.Unsubscribe(BallPocketed);
            ballsStoppedEvent?.Unsubscribe(BallsStoppedMoving);
            shotTakenEvent?.Unsubscribe(ShotTaken);
        }
        
        private void Start()
        {
            _cueBallPottedThisShot = false;
            spawnObjectBallsCommand.RaiseEvent();
            spawnCueBallCommand.RaiseEvent();
        }

        private void Update()
        {
            Logwin.Log("Cue Potted:", _cueBallPottedThisShot, "gameMode");
        }

        private void BallPocketed(BallPocketedEvent evt)
        {
            if (!_ballsInPlay)
                return;
            
            if (evt.PottedBall == null)
                return;

            switch (evt.PottedBall.BallType)
            {
                case BallType.ObjectBall:
                    _score++;
                    ballContainer.ReleaseBall(evt.PottedBall);
                    Logwin.Log("Score", _score);
                    break;

                case BallType.CueBall:
                    _cueBallPottedThisShot = true;
                    ballContainer.ReleaseBall(evt.PottedBall);
                    break;
            }
        }

        public bool CanTakePlayerShot() => !_ballsInPlay;

        
        private void BallsStoppedMoving()
        {
            if (_cueBallPottedThisShot)
            {
                _score--;
                spawnCueBallCommand.RaiseEvent();
                Logwin.Log("Score", _score);
            }

            if (ballContainer.GetActiveBallCount(BallType.ObjectBall) == 0)
            {
                spawnObjectBallsCommand.RaiseEvent();
            }
            
            _ballsInPlay = false;
            _cueBallPottedThisShot = false;
        }
        
        private void ShotTaken()
        {
            _ballsInPlay = true;
        }
    }
}
