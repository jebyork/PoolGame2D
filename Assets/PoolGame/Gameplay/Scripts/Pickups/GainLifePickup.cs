using PoolGame.Gameplay.Attributes;
using UnityEngine;

namespace PoolGame.Gameplay.Pickups
{
    [CreateAssetMenu(fileName = "Gain Life Pickup Effect", menuName = "Pickup/Gain Life", order = 0)]
    public class GainLifePickup : PickupEffect
    {
        [SerializeField] private int lifeGain = 1;
        
        public override void PlayEffect(GameObject pickup)
        {
            Life life = FindFirstObjectByType<Life>();
            if(life == null)
                return;
            life.IncreaseAttribute(lifeGain);
        }
    }
}