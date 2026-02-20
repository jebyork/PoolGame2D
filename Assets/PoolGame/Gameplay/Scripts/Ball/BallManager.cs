using System.Collections.Generic;
using System.Linq;
using PoolGame.Core.Helpers;
using PoolGame.Core.Setup;
using PoolGame.Gameplay.Ball.Racking;
using UnityEngine;

namespace PoolGame.Gameplay.Ball
{
    public class BallManager : MonoBehaviour, ISetupControl
    {
        [SerializeField] private BallSceneData objectBallSceneData;
        [SerializeField] private BallSceneData cueBallSceneData;
        [SerializeField] private CalculateRackPositionsBase objectBallRacking;
        
        [Space]
        [SerializeField] private Transform objectBallPosition;
        [SerializeField] private Transform cueBallPosition;
        
        private int _currentNumberOfObjects;
        private readonly List<BallController> _ballControllers =  new List<BallController>();
        
        public void Initialize()
        {

        }
        
        public void CreateObjects()
        {
            CreateInitialBalls(objectBallSceneData);
            CreateInitialBalls(cueBallSceneData);
        }
        
        private void CreateInitialBalls(BallSceneData ballData)
        {
            if (ballData == null)
            {
                Debug.LogError($"[Ball Manager] Ball Scene Data is missing on {ballData.name}");
                return;
            }
            
            if (objectBallSceneData.BallPrefab == null)
            {
                Debug.LogError($"[Ball Manager] Ball Prefab is missing on {ballData.name}");
                return;
            }
            
            for (int i = 0; i < ballData.InitialNumberOfBalls; i++)
            {
                SpawnBall(ballData.BallPrefab);
            }
        }

        public void Prepare()
        {
            PrepareBalls(objectBallSceneData, objectBallRacking, GetObjectBalls(), objectBallPosition);
            
            BallController cueBall = GetCueBall();
            if (cueBall == null)
            {
                Debug.LogError("[Ball Manager] Cue ball not found (missing Player tag?)");
                return;
            }
            PrepareBall(cueBall, cueBallPosition);
        }
        
        public void StartGame()
        {
            
        }

        private void SpawnBall(GameObject ball)
        {
            GameObject ballObject = Instantiate(ball, transform);
            BallController ballController = ballObject.GetComponent<BallController>();
            if (ballController == null)
            {
                Debug.LogError($"[Ball Manager] Ball Controller is missing on {ball.name}");
                return;
            }
            _ballControllers.Add(ballController);
        }

        private void PrepareBalls(BallSceneData data, CalculateRackPositionsBase rackPositions, BallController[] balls, Transform parent)
        {
            CircleCollider2D col = data.BallPrefab.GetComponent<CircleCollider2D>();
            float radius = col.GetWorldCircleRadius();
            Vector2[] positions = rackPositions.Calculate(balls.Length, radius);
            int count = Mathf.Min(balls.Length, positions.Length);
            for (int i = 0; i < count; i++)
            {
                balls[i].transform.position = (Vector3)positions[i] + parent.position;
            }
        }
        
        private void PrepareBall(BallController ball, Transform parent)
        {
            ball.transform.position = parent.position;
        }
        
        public BallController[] GetObjectBalls() => _ballControllers.
            Where(b => !b.CompareTag("Player")).
            ToArray();

        public BallController GetCueBall()
        {
            BallController cue = _ballControllers.FirstOrDefault(b => b.CompareTag("Player"));
            if (cue == null)
            {
                Debug.LogError("[Ball Manager] No cue ball found with tag Player.");
            }
            return cue;
        }

    }
}
