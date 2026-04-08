using System;
using PoolGame.Core;
using PoolGame.Core.Events;
using PoolGame.Gameplay.Ball;
using PoolGame.Gameplay.Table.Pockets;
using UnityEngine;

namespace PoolGame.Gameplay.GameMode
{
    public class PotAllGameMode : GenericSingleton<PotAllGameMode>
    {
        [SerializeField] private VoidEventChannel spawnObjectBallEvent;
        [SerializeField] private VoidEventChannel spawnCueBallEvent;

        [SerializeField] private BallPocketedChannel ballPocketedEvent;

        private int _score = 0;

        private void OnEnable()
        {
            ballPocketedEvent.Subscribe(BallPocketed);
        }

        private void OnDisable()
        {
            ballPocketedEvent.Unsubscribe(BallPocketed);
        }

        private void Start()
        {
            spawnObjectBallEvent.RaiseEvent();
            spawnCueBallEvent.RaiseEvent();
        }

        private void BallPocketed(BallPocketedEvent evt)
        {
            if (evt.PottedBall == null)
                return;

            Debug.Log("Ball Pocketed");

            if (evt.PottedBall.BallType == BallType.ObjectBall)
            {
                _score++;
                BallContainer.Instance.ReleaseBall(evt.PottedBall);
                Logwin.Log("Score", _score);
                return;
            }

            if (evt.PottedBall.BallType == BallType.CueBall)
            {
                BallContainer.Instance.ReleaseBall(evt.PottedBall);
                spawnCueBallEvent.RaiseEvent();
                _score--;
                Logwin.Log("Score", _score);
            }
        }
    }
}
