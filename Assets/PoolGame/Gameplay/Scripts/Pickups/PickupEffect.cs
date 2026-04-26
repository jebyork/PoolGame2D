using UnityEngine;

namespace PoolGame.Gameplay.Pickups
{
    public abstract class PickupEffect : ScriptableObject
    {
        public abstract void PlayEffect(GameObject pickup);
    }
}