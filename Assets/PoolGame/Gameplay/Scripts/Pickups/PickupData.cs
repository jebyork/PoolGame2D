using UnityEngine;

namespace PoolGame.Gameplay.Pickups
{
    [CreateAssetMenu(fileName = "Pickup Data", menuName = "Pickup/Data", order = 0)]
    public class PickupData : ScriptableObject
    {
        public Sprite Visual;
        public PickupEffect PickupEffect;
        public string BroadcastMessage;
        public BallType BallType;
        public float SpawnRateWeight = 1;
    }
}