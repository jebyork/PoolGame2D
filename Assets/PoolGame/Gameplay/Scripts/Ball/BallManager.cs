using System.Collections.Generic;
using PoolGame.Core;
using PoolGame.Core.Helpers;
using PoolGame.Core.Setup;
using PoolGame.Gameplay.Ball.Racking;
using UnityEngine;

namespace PoolGame.Gameplay.Ball
{
    public class BallManager : GenericSingleton<BallManager>, ISetupControl
    {
        #region Inspector

        [Header("Ball Data")]
        [SerializeField] private BallSceneData objectBallSceneData;
        [SerializeField] private BallSceneData cueBallSceneData;
        [SerializeField] private CalculateRackPositionsBase objectBallRacking;

        [Header("Spawn Points")]
        [SerializeField] private Transform objectBallPosition;
        [SerializeField] private Transform cueBallPosition;

        [Header("Spawn Validation")]
        [SerializeField] private LayerMask spawnBlockers;
        [SerializeField] private int spawnIterationChecks = 32;
        [SerializeField] private float spawnAdjustStepMultiplier = 1.1f;

        #endregion

        #region Runtime State

        private readonly List<BallController> _objectBalls = new();
        private readonly List<BallController> _allBalls = new();
        private readonly Collider2D[] _overlapResults = new Collider2D[1];

        private BallController _cueBall;
        private ContactFilter2D _spawnFilter;

        #endregion

        #region Public Properties

        public IReadOnlyList<BallController> ObjectBalls => _objectBalls;
        public IReadOnlyList<BallController> AllBalls => _allBalls;
        public BallController CueBall => _cueBall;

        #endregion

        #region Setup

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
            ClearBallReferences();

            CreateObjectBalls();
            CreateCueBall();
        }

        public void Prepare()
        {
            PrepareBalls(objectBallRacking, _objectBalls, objectBallPosition);
            ResetCueBall();
        }

        public void StartGame()
        {
        }

        #endregion

        #region Creation

        private void ClearBallReferences()
        {
            _objectBalls.Clear();
            _allBalls.Clear();
            _cueBall = null;
        }

        private void CreateObjectBalls()
        {
            if (!TryValidateBallData(objectBallSceneData, out GameObject prefab))
                return;

            for (int i = 0; i < objectBallSceneData.InitialNumberOfBalls; i++)
            {
                BallController ball = SpawnBall(prefab);
                if (ball == null)
                    continue;

                _objectBalls.Add(ball);
                _allBalls.Add(ball);
            }
        }

        private BallController CreateCueBall()
        {
            if (_cueBall != null)
                return _cueBall;

            _cueBall = CreateSingleBall(cueBallSceneData);

            if (_cueBall != null)
            {
                _allBalls.Add(_cueBall);
            }

            return _cueBall;
        }

        private BallController CreateSingleBall(BallSceneData ballData)
        {
            if (!TryValidateBallData(ballData, out GameObject prefab))
                return null;

            return SpawnBall(prefab);
        }

        private bool TryValidateBallData(BallSceneData ballData, out GameObject prefab)
        {
            prefab = null;

            if (ballData == null)
            {
                Debug.LogError("[Ball Manager] Ball Scene Data is missing.");
                return false;
            }

            if (ballData.BallPrefab == null)
            {
                Debug.LogError($"[Ball Manager] Ball Prefab is missing on {ballData.name}");
                return false;
            }

            prefab = ballData.BallPrefab;
            return true;
        }

        private BallController SpawnBall(GameObject ballPrefab)
        {
            GameObject ballObject = Instantiate(ballPrefab, transform);
            BallController ballController = ballObject.GetComponent<BallController>();

            if (ballController != null)
                return ballController;

            Debug.LogError($"[Ball Manager] Ball Controller is missing on {ballPrefab.name}");
            Destroy(ballObject);
            return null;
        }

        #endregion

        #region Reset / Preparation

        public void ResetCueBall()
        {
            BallController cueBall = _cueBall ?? CreateCueBall();

            if (cueBall == null)
            {
                Debug.LogError("[Ball Manager] Cue ball could not be created or found.");
                return;
            }

            PrepareBall(cueBall, cueBallPosition);
        }

        private void PrepareBalls(CalculateRackPositionsBase rackPositions, List<BallController> balls, Transform parent)
        {
            if (balls == null || balls.Count == 0)
                return;

            if (!TryGetRadius(balls[0], out float radius))
                return;

            Vector2[] offsets = rackPositions.Calculate(balls.Count, radius);
            int count = Mathf.Min(balls.Count, offsets.Length);

            for (int i = 0; i < count; i++)
            {
                Vector3 desiredPosition = parent.position + (Vector3)offsets[i];
                TryPlaceBall(balls[i], radius, desiredPosition);
            }
        }

        private void PrepareBall(BallController ball, Transform parent)
        {
            if (!TryGetRadius(ball, out float radius))
                return;

            TryPlaceBall(ball, radius, parent.position);
        }

        #endregion

        #region Placement

        private bool TryGetRadius(BallController ball, out float radius)
        {
            radius = 0f;

            if (ball == null)
                return false;

            CircleCollider2D circleCollider = ball.GetComponent<CircleCollider2D>();
            if (circleCollider == null)
            {
                Debug.LogError($"[Ball Manager] CircleCollider2D missing on {ball.name}");
                return false;
            }

            radius = circleCollider.GetWorldCircleRadius();
            return true;
        }

        private bool TryPlaceBall(BallController ball, float radius, Vector3 desiredPosition)
        {
            Vector3 finalPosition = desiredPosition;

            if (!BallCanPlaceHere(radius, finalPosition) &&
                !AdjustSpawnByRadius(radius, desiredPosition, out finalPosition))
            {
                Debug.LogError($"[Ball Manager] Ball cannot be placed near {desiredPosition}");
                return false;
            }

            ball.transform.position = finalPosition;
            return true;
        }

        private bool BallCanPlaceHere(float radius, Vector3 position)
        {
            int count = Physics2D.OverlapCircle(position, radius, _spawnFilter, _overlapResults);
            return count == 0;
        }

        private bool AdjustSpawnByRadius(float radius, Vector3 origin, out Vector3 position)
        {
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

        #endregion
    }
}