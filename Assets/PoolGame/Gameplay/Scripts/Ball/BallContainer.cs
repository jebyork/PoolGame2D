using System;
using System.Collections.Generic;
using PoolGame.Core;
using PoolGame.Core.Helpers;
using PoolGame.Gameplay.Ball.Spawning;
using UnityEngine;
using UnityEngine.Serialization;

namespace PoolGame.Gameplay.Ball
{
    public class BallContainer : GenericSingleton<BallContainer>
    {
        [SerializeField] private LayerMask cueBallLayerMask;
        
        [Space]
        [SerializeField] private List<BallController> balls;
        public List<BallController> Balls { get; private set; }

        [Space]
        [Header("Runtime")]
        [SerializeField] private BallController cueBall;
        [SerializeField] private List<BallController> objectBalls = new List<BallController>();
        
        
        private void OnEnable()
        {
            BallController.OnBallSpawned += AddBallToList;
            BallController.OnBallDespawned += RemoveBallFromList;
        }

        private void OnDisable()
        {
            BallController.OnBallSpawned -= AddBallToList;
            BallController.OnBallDespawned -= RemoveBallFromList;
        }

        private void AddBallToList(BallController obj)
        {
            if(balls.Contains(obj)) return;
            balls.Add(obj);
            SortAdd(obj);
        }
        
        private void RemoveBallFromList(BallController obj)
        {
            if(!balls.Contains(obj)) return;
            balls.Remove(obj);
            SortRemove(obj);
        }

        private void SortAdd(BallController ball)
        {
            LayerMask layerMask = ball.gameObject.layer;
            if (cueBallLayerMask.ContainsLayer(layerMask))
            {
                cueBall = ball;
                return;
            }
            
            objectBalls.Add(ball);
        }

        private void SortRemove(BallController ball)
        {
            LayerMask layerMask = ball.gameObject.layer;
            if (cueBallLayerMask.ContainsLayer(layerMask))
            {
                cueBall = null;
                return;
            }
            
            objectBalls.Remove(ball);
        }

        public bool IsCueBall(BallController ball)
        {
            return cueBall == ball;
        }

        public bool IsCueBall(GameObject ball)
        {
            BallController ballController = ball.GetComponent<BallController>();
            if(ballController == null) return false;
            return IsCueBall(ballController);
        }

        public bool IsObjectBall(BallController ball)
        {
            return objectBalls.Contains(ball);
        }

        public bool IsObjectBall(GameObject ball)
        {
            BallController ballController = ball.GetComponent<BallController>();
            if(ballController == null) return false;
            return IsObjectBall(ballController);
        }
    }
}