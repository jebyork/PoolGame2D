using System;
using PoolGame.Gameplay.Ball;
using UnityEngine;
using UnityEngine.Events;

namespace PoolGame.Gameplay.Pockets
{
    public class PocketedListener : MonoBehaviour
    {
        public UnityEvent OnBallPocketed;
        private void OnEnable()
        {
            PocketController.OnBallPocketed += BallPocketed;
        }
        
        private void OnDisable()
        {
            PocketController.OnBallPocketed -= BallPocketed;
        }

        private void BallPocketed(BallController arg1, PocketController arg2)
        {
            OnBallPocketed?.Invoke();
        }
    }
}