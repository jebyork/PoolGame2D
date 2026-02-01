using System.Collections.Generic;
using System.Linq;
using PoolGame.Controllers;
using PoolGame.Events;
using UnityEngine;

namespace PoolGame.Managers
{
    public class BallManager : MonoBehaviour
    {
        [SerializeField] private BoolEventChannel ballsStoppedMovingEventChannel;
        
        [Space]
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private GameObject cueBallPrefab;
        
        [Space]
        [SerializeField] private Transform ballSpawnTransform;
        [SerializeField] private Transform cueBallSpawnTransform;

        [Space] 
        [SerializeField] private int initialBallCount;
        [SerializeField] private float rackGap = 0.001f;
        [SerializeField, Range(0f, 360f)] private float rackAngleDeg = 0f;
        
        private readonly List<BallController> _balls = new();
        private BallController _cueBall;
        private bool _ballsMoving;
        
        private void FixedUpdate()
        {
            if (!_cueBall) 
                return;

            bool anyMoving = AreAnyBallsMoving();

            UpdateMovingStateAndNotify(anyMoving);
        }

        public void CreateBalls()
        {
            if (ballPrefab != null)
            {
                for (int i = 0; i < initialBallCount; i++)
                {
                    BallController ball = Instantiate(ballPrefab, transform).GetComponent<BallController>();
                    if (ball != null)
                        _balls.Add(ball);
                }
            }

            if (cueBallPrefab != null)
            {
                _cueBall = Instantiate(cueBallPrefab, transform).GetComponent<BallController>();
            }
        }

        public void SetInitialBallPosition()
        {
            PlaceInitialNonCueBalls();
            SetBallPosition(_cueBall.gameObject , cueBallSpawnTransform.position);
        }

        public void SetAllBallActiveness(bool isActive)
        {
            foreach (GameObject ball in _balls.Select(b => b.gameObject))
            {
                ball.SetActive(isActive);
            }
            
            GameObject cueBallGObj = _cueBall.gameObject;
            cueBallGObj.SetActive(isActive);
        }
        
        private void PlaceInitialNonCueBalls()
        {
            if (_balls == null || _balls.Count == 0) 
                return;

            GameObject firstBall = _balls[0].gameObject;
            float radius = GetBallRadius(firstBall);
            if (radius < 0f) 
                return;

            int i = 0;
            foreach (var pos in GetRackPositions(_balls.Count, radius))
                _balls[i++].transform.position = pos;
        }

        private void SetBallPosition(GameObject ball , Vector2 position)
        {
            ball.transform.position = position;
        }

        private float GetBallRadius(GameObject ball)
        {
            CircleCollider2D col = ball.GetComponent<CircleCollider2D>();
            if (col == null)
            {
                Debug.LogError("Ball prefab is missing CircleCollider2D");
                return -1;
            }
            return col.radius * ball.transform.lossyScale.x;
        }
        
        private IEnumerable<Vector2> GetRackPositions(int totalBalls, float radius)
        {
            float diameter = (2f * radius) + rackGap;
            float rowStep = (Mathf.Sqrt(3f) * 0.5f) * diameter;

            Vector2 apex = ballSpawnTransform.position;

            float rad = rackAngleDeg * Mathf.Deg2Rad;
            Vector2 forward = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)).normalized;
            Vector2 right = new (forward.y, -forward.x);

            int ballIndex = 0;
            int rowIndex = 0;

            while (ballIndex < totalBalls)
            {
                int ballsThisRow = rowIndex + 1;

                Vector2 rowOrigin = apex + forward * (rowIndex * rowStep);
                float leftOffset = -0.5f * (ballsThisRow - 1) * diameter;

                for (int slotIndex = 0; slotIndex < ballsThisRow; slotIndex++)
                {
                    if (ballIndex >= totalBalls) yield break;

                    float sideOffset = leftOffset + slotIndex * diameter;
                    Vector2 pos = rowOrigin + right * sideOffset;

                    yield return pos;
                    ballIndex++;
                }

                rowIndex++;
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            if (ballSpawnTransform == null || ballPrefab == null) 
                return;
            CircleCollider2D col = ballPrefab.GetComponent<CircleCollider2D>();
            if (col == null) 
                return;
            float radius = col.radius * ballPrefab.transform.lossyScale.x;
            foreach (Vector2 pos in GetRackPositions(initialBallCount , radius))
            {
                MyHelpers.DrawCircle(pos, radius, 12, Color.cyan);
            }
        }

        private bool AreAnyBallsMoving()
        {
            return AreAnyObjectBallsMoving() || IsCueBallMoving();
        }

        private bool IsCueBallMoving()
        {
            return _cueBall != null && _cueBall.IsMoving;
        }

        private bool AreAnyObjectBallsMoving()
        {
            return _balls.Any(b => b != null && b.IsMoving);
        }
        
        private void UpdateMovingStateAndNotify(bool anyMoving)
        {
            if (ShouldFireStoppedEvent(anyMoving))
            {
                _ballsMoving = false;
                ballsStoppedMovingEventChannel?.RaiseEvent(true);
                return;
            }

            if (ShouldMarkAsMoving(anyMoving))
            {
                _ballsMoving = true;
            }
        }
        
        private bool ShouldFireStoppedEvent(bool anyMoving)
        {
            return _ballsMoving && !anyMoving;
        }

        private bool ShouldMarkAsMoving(bool anyMoving)
        {
            return !_ballsMoving && anyMoving;
        }
    }
}
