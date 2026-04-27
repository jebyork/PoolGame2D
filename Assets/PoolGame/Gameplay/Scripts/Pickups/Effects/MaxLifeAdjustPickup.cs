using PoolGame.Gameplay.Attributes;
using UnityEngine;

namespace PoolGame.Gameplay.Pickups.Effects
{
    [CreateAssetMenu(fileName = "Adjust Max Life Pickup Effect", menuName = "Pickup/Max Life", order = 0)]
    public class MaxLifeAdjustPickup : PickupEffect
    {
        [SerializeField] private int maxLifeAdjust = 1;


        public override void PlayEffect(GameObject pickup)
        {
            Life life = FindFirstObjectByType<Life>();
            if(life == null)
                return;
            life.AdjustMaxLife(maxLifeAdjust);
        }
    }
}