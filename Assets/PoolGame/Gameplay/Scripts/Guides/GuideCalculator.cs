using System;
using PoolGame.Core.Helpers;
using PoolGame.Gameplay.Shooting.Aiming;
using PoolGame.Gameplay.Shooting.CanShoot;
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
        [SerializeField] private AimingCalculationDataObserver aimingCalculationDataObserver;
        [SerializeField] private AimingDataObserver aimingDataObserver;
        [SerializeField] private CanShootStrategy canShootStrategy;
        
        public UnityEvent<GuideLineVisualData> OnFirstGuideCalculated;
        public UnityEvent<GuideCircleVisualData, GuideLineVisualData> OnSecondaryGuideCircleCalculated;
        public UnityEvent OnDisableGuides;
        public UnityEvent OnDisableSecondaryGuides;

        private float _checkRadius;

        private void Start()
        {
            if (cueBallPrefab != null)
            {
                CircleCollider2D circleCollider2D = cueBallPrefab.GetComponent<CircleCollider2D>();
                _checkRadius = circleCollider2D.GetWorldCircleRadius();
                Debug.Log(_checkRadius);
            }
        }

        private void Update()
        {
            if (ShouldDrawGuide())
            {
                CalculateFirstGuide();
                return;
            }
            OnDisableGuides?.Invoke();
        }
        
        private bool ShouldDrawGuide()
        {
            if (aimingCalculationDataObserver == null) return false;
            if (aimingCalculationDataObserver.Value.Shootable == null) return false;
            return canShootStrategy.CanShoot();
        }
        
        private void CalculateFirstGuide()
        {
            Vector3 ballCentre = aimingCalculationDataObserver.Value.Shootable.GetPosition();
            Vector3 dir = aimingDataObserver.Value.Direction.normalized;
            Vector3 guideStart = ballCentre + dir * _checkRadius;
            float distance = firstGuideMaxDistance * aimingDataObserver.Value.Power01;
            
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
                OnDisableSecondaryGuides?.Invoke();
            }

            OnFirstGuideCalculated?.Invoke(new GuideLineVisualData
            {
                Start = guideStart,
                End = guideEnd
            });
        }

        private void CalculateSecondaryGuides(RaycastHit2D hit,  Vector3 incomingDir)
        {
            IGuideHitResponse responder = hit.collider.GetComponent<IGuideHitResponse>();
            if (responder == null)
            {
                OnDisableSecondaryGuides?.Invoke();
                return;
            }

            GuideLineVisualData result = responder.Resolve(hit, incomingDir, _checkRadius, secondaryGuideMaxDistance);
            GuideCircleVisualData circleData = new (hit.centroid, _checkRadius);
            OnSecondaryGuideCircleCalculated?.Invoke(circleData, result);
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
        public float Radius;

        public GuideCircleVisualData(Vector3 pos , float radius)
        {
            Pos = pos;
            Radius = radius;
        }
    }
}


