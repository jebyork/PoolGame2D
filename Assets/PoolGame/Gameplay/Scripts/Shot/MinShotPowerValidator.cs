using PoolGame.Gameplay.Aim;
using UnityEngine;

namespace PoolGame.Gameplay.Shot
{
    [CreateAssetMenu(fileName = "Min Shot Power" , menuName = "Shot Validator/Min Shot Power" , order = 0)]
    public class MinShotPowerValidator : ShotValidatorStrategy
    {
        [SerializeField] private float minPower01 = 0.1f;
        
        public override bool IsValid(ShotCommandContext context)
        {
            return context.Snapshot.ShotPower01 >= minPower01;
        }
    }
}
