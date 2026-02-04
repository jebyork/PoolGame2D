using UnityEngine;

namespace PoolGame.Gameplay.ShotTargetPicker
{
    public abstract class ShotTargetPickerStrategy : ScriptableObject , IShotTargetPicker
    {
        public abstract ShotTargetPickResult TryPick();
        
        protected static ShotTargetPickResult FailedShotTargetPickResult()
        {
            return new ShotTargetPickResult(
                null, 
                Vector3.zero, 
                false);
        }
    }
}