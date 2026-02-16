using System;
using PoolGame.Game.Line;
using PoolGame.Gameplay.Shooting.Aiming;
using UnityEngine;

namespace PoolGame.Gameplay.Ball
{
    public class GuideDrawer :  MonoBehaviour
    {
        [SerializeField] private StraightLine firstGuide;
        [SerializeField] private StraightLine secondGuide;
        [SerializeField] private CircleLine circleGuide;
        
        [Space]
        [SerializeField] private AimingCalculationDataObserver aimingCalculationDataObserver;
        [SerializeField] private AimingDataObserver aimingDataObserver;

        private void OnValidate()
        {
            if (firstGuide == null)
            {
                Debug.LogWarning($"First guide drawer has not been assigned on {gameObject.name}.");
            }
            
            if (secondGuide == null)
            {
                Debug.LogWarning($"Second guide drawer has not been assigned on {gameObject.name}.");
            }
            
            if (circleGuide == null)
            {
                Debug.LogWarning($"Circle guide drawer has not been assigned on {gameObject.name}.");
            }
        }

        private void Update()
        {
            if (HasTarget())
            {
                Debug.Log($"Guide drawer has been called on {gameObject.name}.");
            }
        }


        private bool HasTarget()
        {
            return aimingCalculationDataObserver != null && 
                   aimingCalculationDataObserver.Value.Shootable != null;
        }
    }
}
