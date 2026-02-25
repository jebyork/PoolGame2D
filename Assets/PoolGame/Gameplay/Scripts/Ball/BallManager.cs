using System.Collections.Generic;
using System.Linq;
using PoolGame.Core;
using PoolGame.Core.Helpers;
using PoolGame.Core.Setup;
using PoolGame.Gameplay.Ball.Racking;
using UnityEngine;

namespace PoolGame.Gameplay.Ball
{
    public class BallManager : GenericSingleton<BallManager>, ISetupControl
    {
        [SerializeField] private BallSceneData objectBallSceneData;
        [SerializeField] private BallSceneData cueBallSceneData;
        [SerializeField] private CalculateRackPositionsBase objectBallRacking;
        
        [Space]
        [SerializeField] private Transform objectBallPosition;
        [SerializeField] private Transform cueBallPosition;

        [Space] 
        [SerializeField] private LayerMask spawnBlockers;
        [SerializeField] private int spawnIterationChecks = 32;
        [SerializeField] private float spawnAdjustStepMultiplier = 1.1f;
        
        private int _currentNumberOfObjects;
        private readonly List<BallController> _ballControllers =  new List<BallController>();
        private readonly Collider2D[] _overlapResults = new Collider2D[1];
        private ContactFilter2D _spawnFilter;
        
        public List<BallController> BallControllers => _ballControllers;
        
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
        
        public void Initialize()
        {
            _spawnFilter = new ContactFilter2D
            {
                useLayerMask = true,
                layerMask = spawnBlockers,
                useTriggers = false
            };
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
            
            if (ballData.BallPrefab == null)
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
            PrepareBalls(objectBallRacking, GetObjectBalls(), objectBallPosition);
            
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

        private void PrepareBalls(CalculateRackPositionsBase rackPositions, BallController[] balls, Transform parent)
        {
            if (balls == null || balls.Length == 0)
                return;

            if (!TryGetRadius(balls[0], out float radius))
                return;

            Vector2[] offsets = rackPositions.Calculate(balls.Length, radius);
            int count = Mathf.Min(balls.Length, offsets.Length);

            for (int i = 0; i < count; i++)
            {
                Vector3 desiredPos = parent.position + (Vector3)offsets[i];
                TryPlaceBall(balls[i], radius, desiredPos);
            }
        }

        private void PrepareBall(BallController ball, Transform parent)
        {
            if (!TryGetRadius(ball, out float radius))
                return;

            TryPlaceBall(ball, radius, parent.position);
        }

        private bool TryGetRadius(BallController ball, out float radius)
        {
            radius = 0f;

            if (ball == null)
                return false;

            CircleCollider2D col = ball.GetComponent<CircleCollider2D>();
            if (col == null)
            {
                Debug.LogError($"[Ball Manager] CircleCollider2D missing on {ball.name}");
                return false;
            }

            radius = col.GetWorldCircleRadius();
            return true;
        }
        
        private bool TryPlaceBall(BallController ball, float radius, Vector3 desiredPos)
        {
            Vector3 pos = desiredPos;

            if (!BallCanPlaceHere(radius, pos) && 
                !AdjustSpawnByRadius(radius, desiredPos, out pos))
            {
                Debug.LogError($"[Ball Manager] Ball cannot be placed near {desiredPos}");
                return false;
            }

            ball.transform.position = pos;
            return true;
        }
        
        private bool BallCanPlaceHere(float radius, Vector3 position)
        {
            int count = Physics2D.OverlapCircle(position, radius, _spawnFilter, _overlapResults);
            return count == 0;
        }
        
        private bool AdjustSpawnByRadius(float radius, Vector3 origin, out Vector3 position)
        {
            // if caller passes origin as the desired start, keep it consistent
            position = origin;

            if (BallCanPlaceHere(radius, origin))
                return true;

            float step = radius * spawnAdjustStepMultiplier;

            if (TryRing(origin, radius, step, ref position))
                return true;
            
            if (TryRing(origin, radius, step * 2f, ref position))
                return true;

            return false;
        }
        
        private bool TryRing(Vector3 origin, float radius, float ringDistance, ref Vector3 position)
        {
            for (int i = 0; i < spawnIterationChecks; i++)
            {
                float angle = (i / (float)spawnIterationChecks) * Mathf.PI * 2f;
                Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * ringDistance;
                Vector3 candidate = origin + offset;

                if (!BallCanPlaceHere(radius, candidate))
                    continue;

                position = candidate;
                return true;
            }

            return false;
        }
    }
}
