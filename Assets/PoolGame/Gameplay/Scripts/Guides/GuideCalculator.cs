using PoolGame.Core.Helpers;
using PoolGame.Gameplay.Shooting;
using PoolGame.Gameplay.Shooting.Aiming;
using UnityEngine;
using UnityEngine.Events;

namespace PoolGame.Gameplay.Guides
{
    public class InitialHitCalculator : MonoBehaviour
    {
        [SerializeField] private GameObject cueBallPrefab;
        [SerializeField] private LayerMask bounceLayer;
        [SerializeField] private float firstGuideMaxDistance;
        [SerializeField] private float secondaryGuideMaxDistance;

        [Space, Header("External Data")]
        [SerializeField] private PlayerShootingController shootingController;

        public UnityEvent<GuideLineVisualData> onFirstGuideCalculated;
        public UnityEvent<GuideCircleVisualData, GuideLineVisualData> onSecondaryGuideCircleCalculated;
        public UnityEvent onDisableGuides;
        public UnityEvent onDisableSecondaryGuides;

        private float _checkRadius;

        private void Start()
        {
            if (cueBallPrefab != null)
            {
                CircleCollider2D circleCollider2D = cueBallPrefab.GetComponent<CircleCollider2D>();
                _checkRadius = circleCollider2D.GetWorldCircleRadius();
            }
        }

        private void Update()
        {
            if (ShouldDrawGuide())
            {
                CalculateFirstGuide();
                return;
            }

            onDisableGuides?.Invoke();
        }

        private bool ShouldDrawGuide()
        {
            if (shootingController == null)
                return false;

            if (!shootingController.HasActiveAim)
                return false;

            return shootingController.CanShootCurrentAim();
        }

        private void CalculateFirstGuide()
        {
            AimingCalculationData calculationData = shootingController.CurrentCalculationData;
            AimingData aimingData = shootingController.CurrentAimingData;

            Vector3 ballCentre = calculationData.Shootable.GetPosition();
            Vector3 dir = aimingData.Direction.normalized;
            Vector3 guideStart = ballCentre + dir * _checkRadius;
            float distance = firstGuideMaxDistance * aimingData.Power01;

            RaycastHit2D hit = Physics2D.CircleCast(
                guideStart,
                _checkRadius,
                dir,
                distance,
                bounceLayer
            );

            Vector3 guideEnd;

            if (hit.collider != null)
            {
                guideEnd = (Vector3)hit.centroid - dir * _checkRadius;
                CalculateSecondaryGuides(hit, dir);
            }
            else
            {
                guideEnd = guideStart + dir * distance;
                onDisableSecondaryGuides?.Invoke();
            }

            onFirstGuideCalculated?.Invoke(new GuideLineVisualData
            {
                Start = guideStart,
                End = guideEnd
            });
        }

        private void CalculateSecondaryGuides(RaycastHit2D hit, Vector3 incomingDir)
        {
            IGuideHitResponse responder = hit.collider.GetComponent<IGuideHitResponse>();
            if (responder == null)
            {
                onDisableSecondaryGuides?.Invoke();
                return;
            }

            GuideLineVisualData result = responder.Resolve(hit, incomingDir, _checkRadius, secondaryGuideMaxDistance);
            GuideCircleVisualData circleData = new(hit.centroid, _checkRadius);
            onSecondaryGuideCircleCalculated?.Invoke(circleData, result);
        }
    }

    public struct GuideLineVisualData
    {
        public Vector3 Start;
        public Vector3 End;

        public GuideLineVisualData(Vector3 start, Vector3 end)
        {
            Start = start;
            End = end;
        }
    }

    public struct GuideCircleVisualData
    {
        public Vector3 Pos;
        public readonly float Radius;

        public GuideCircleVisualData(Vector3 pos, float radius)
        {
            Pos = pos;
            Radius = radius;
        }
    }
}
