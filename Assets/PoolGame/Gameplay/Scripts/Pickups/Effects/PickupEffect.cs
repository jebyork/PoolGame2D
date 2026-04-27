using UnityEngine;

namespace PoolGame.Gameplay.Pickups.Effects
{
    public abstract class PickupEffect : ScriptableObject
    {
        public abstract void PlayEffect(GameObject pickup);
    }
}