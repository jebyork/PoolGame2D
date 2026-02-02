using PoolGame.Core.Game.Line;
using PoolGame.Core.Game.States.Gameplay.Shot;
using PoolGame.Core.Helpers;
using PoolGame.Core.Values;
using UnityEngine;

namespace PoolGame.Core.Game.States.Gameplay.Ball
{
    public class GuideDrawer : MonoBehaviour
    {
        [SerializeField] private AimUpdatedChannel aimUpdatedChannel;
        [SerializeField] private ShotRequestedChannel shotRequestedChannel;
        [SerializeField] private GameObjectValue cueBallValueStore;
        
        [Space]
        [Header("Distances")]
        [SerializeField] private float maxInitialGuideDistance = 10f;
        [SerializeField] private float maxBallHitGuideDistance = 2f;
        
        [Space]
        [Header("Masks")]
        [SerializeField] private LayerMask guideHitMask;
        [SerializeField] private LayerMask ballHitMask;
        
        [Space]
        [Header("Visuals")]
        [SerializeField] private StraightLine initialGuideLine;
        [SerializeField] private StraightLine hitGuideLine;
        [SerializeField] private CircleLine ghostBallCircle;
        [SerializeField] private float surfaceGap = 0.02f;

        
        private bool _isAiming;
        private AimSnapshot _latestAimSnapshot;
        private GameObject _cachedCueBall;
        private Transform _cueBallTransform;
        private CircleCollider2D _cueBallCollider;
        
        private void OnEnable()
        {
            aimUpdatedChannel?.Subscribe(OnAiming);
            shotRequestedChannel?.Subscribe(OnShotRequested);
        }

        private void OnDisable()
        {
            aimUpdatedChannel?.Unsubscribe(OnAiming);
            shotRequestedChannel?.Unsubscribe(OnShotRequested);
        }
        
        private void OnValidate()
        {
            if (cueBallValueStore == null)
            {
                Debug.LogError("[GuideDrawer] cueBallValueStore is not assigned." , this);
            }

            if (initialGuideLine == null || hitGuideLine == null || ghostBallCircle == null)
            {
                Debug.LogError("[GuideDrawer] One or more guide visuals are not assigned." , this);
            }
        }

        private void Start()
        {
            HideAllGuides();
        }

        private void Update()
        {
            if (_isAiming)
            {
                UpdateGuides();
            }
        }
        
        private void OnAiming(AimSnapshot snapshot)
        {
            _latestAimSnapshot = snapshot;

            if (!PullIsValid(snapshot))
            {
                StopAiming();
                return;
            }

            if (_isAiming)
            {
                return;
            }

            _isAiming = true;
            ShowInitialGuideOnly();
        }
        
        private void OnShotRequested(ShotData _)
        {
            StopAiming();
        }
        
        private void StopAiming()
        {
            _isAiming = false;
            HideAllGuides();
        }
        
        private void ShowInitialGuideOnly()
        {
            initialGuideLine?.SetActive(true);
            HideHitGuides();
        }

        private void HideAllGuides()
        {
            initialGuideLine?.SetActive(false);
            HideHitGuides();
        }

        private void HideHitGuides()
        {
            hitGuideLine?.SetActive(false);
            ghostBallCircle?.SetActive(false);
        }
        
        private void UpdateGuides()
        {
            if (!TryGetCueBall(out Vector2 cueBallCenter, out float cueBallRadiusWorld))
            {
                StopAiming();
                return;
            }

            if (!TryGetAim(out Vector2 shotDirection, out float pullPercent01))
            {
                StopAiming();
                return;
            }

            float initialGuideDistance = maxInitialGuideDistance * pullPercent01;
            float ballHitGuideDistance = maxBallHitGuideDistance * pullPercent01;

            RaycastHit2D hit = Physics2D.CircleCast(
                cueBallCenter,
                cueBallRadiusWorld,
                shotDirection,
                initialGuideDistance,
                guideHitMask
            );

            DrawInitialGuideLine(
                cueBallCenter,
                cueBallRadiusWorld,
                shotDirection,
                initialGuideDistance,
                surfaceGap,
                hit
            );

            HideHitGuides();
            if (!hit)
            {
                return;
            }

            DrawGhostBall(hit, cueBallRadiusWorld);
            if (!IsBallHit(hit))
            {
                return;
            }

            DrawBallHitGuideLine(hit, ballHitGuideDistance, surfaceGap);
        }
        
        private void DrawInitialGuideLine(Vector2 cueBallCenter, float cueBallRadiusWorld, Vector2 shotDirection,
            float guideDistance, float gapFromSurface, RaycastHit2D hit)
        {
            Vector2 rawStart = cueBallCenter + shotDirection * cueBallRadiusWorld;
            Vector2 rawEnd;
            if (hit)
            {
                Vector2 ghostBallCenter = hit.point + hit.normal * cueBallRadiusWorld;
                rawEnd = ghostBallCenter - shotDirection * cueBallRadiusWorld;
            }
            else
            {
                rawEnd = cueBallCenter + shotDirection * guideDistance;
            }
            ApplyEndGaps(rawStart, rawEnd, gapFromSurface, gapFromSurface, 
                out Vector2 lineStart, out Vector2 lineEnd);
            initialGuideLine.SetPositions(lineStart, lineEnd);
        }
        
        private void DrawGhostBall(RaycastHit2D hit, float cueBallRadiusWorld)
        {
            Vector2 contactPoint = hit.point;
            Vector2 surfaceNormal = hit.normal;
            Vector2 cueBallCenterAtImpact = contactPoint + surfaceNormal * cueBallRadiusWorld;
            ghostBallCircle.SetCircle(cueBallCenterAtImpact, cueBallRadiusWorld);
        }
        
        private void DrawBallHitGuideLine(RaycastHit2D hit, float guideDistance, float gapFromSurface)
        {
            Vector2 objectBallDirection = (-hit.normal).normalized;
            CircleCollider2D objectBallCollider = hit.collider as CircleCollider2D;
            Vector2 objectBallCenter = objectBallCollider ? objectBallCollider.transform.position
                : (Vector2)hit.collider.transform.position;
            float objectBallRadiusWorld = objectBallCollider ? objectBallCollider.GetWorldCircleRadius() : 0f;
            Vector2 rawStart = objectBallCenter + objectBallDirection * objectBallRadiusWorld;
            Vector2 rawEnd = rawStart + objectBallDirection * guideDistance;
            ApplyEndGaps(rawStart, rawEnd, gapFromSurface, gapFromSurface,
                out Vector2 lineStart, out Vector2 lineEnd);
            hitGuideLine.SetActive(true);
            hitGuideLine.SetPositions(lineStart, lineEnd);
        }
        
        private static void ApplyEndGaps(Vector2 startPoint, Vector2 endPoint, float gapAtStart, float gapAtEnd,
            out Vector2 gappedStart, out Vector2 gappedEnd)
        {
            Vector2 segment = endPoint - startPoint;
            float segmentLength = segment.magnitude;

            if (segmentLength <= Mathf.Epsilon)
            {
                gappedStart = startPoint;
                gappedEnd = startPoint;
                return;
            }

            Vector2 segmentDirection = segment / segmentLength;
            float totalGap = gapAtStart + gapAtEnd;
            if (segmentLength <= totalGap)
            {
                Vector2 midpoint = startPoint + segmentDirection * (segmentLength * 0.5f);
                gappedStart = midpoint;
                gappedEnd = midpoint;
                return;
            }

            gappedStart = startPoint + segmentDirection * gapAtStart;
            gappedEnd = endPoint - segmentDirection * gapAtEnd;
        }
        
        private bool TryGetCueBall(out Vector2 cueBallCenter, out float cueBallRadiusWorld)
        {
            cueBallCenter = default;
            cueBallRadiusWorld = 0f;

            if (cueBallValueStore == null || cueBallValueStore.Value == null)
                return false;

            GameObject cueBall = cueBallValueStore.Value;

            bool cueBallChanged = _cachedCueBall != cueBall;
            if (cueBallChanged)
            {
                _cachedCueBall = cueBall;
                _cueBallTransform = cueBall.transform;
                _cueBallCollider = cueBall.GetComponent<CircleCollider2D>();
            }

            if (_cueBallTransform == null || _cueBallCollider == null)
                return false;

            cueBallCenter = _cueBallTransform.position;
            cueBallRadiusWorld = _cueBallCollider.GetWorldCircleRadius();

            bool radiusIsValid = cueBallRadiusWorld > 0f;
            return radiusIsValid;
        }

        private bool TryGetAim(out Vector2 shotDirection, out float pullPercent01)
        {
            shotDirection = default;
            pullPercent01 = 0f;

            pullPercent01 = _latestAimSnapshot.ClampedPullPercentage;
            bool pullIsValid = pullPercent01 >= 0f;
            if (!pullIsValid)
                return false;

            Vector2 rawDirection = (Vector2)_latestAimSnapshot.ShotDirection;
            shotDirection = rawDirection.normalized;

            bool directionIsValid = !shotDirection.IsNearlyZero();
            return directionIsValid;
        }

        
        private bool PullIsValid(AimSnapshot snapshot)
        {
            return snapshot.ClampedPullPercentage >= 0f;
        }
        
        private bool IsBallHit(RaycastHit2D firstHit)
        {
            int hitLayer = firstHit.collider.gameObject.layer;
            return ballHitMask.ContainsLayer(hitLayer);
        }
    }
}
