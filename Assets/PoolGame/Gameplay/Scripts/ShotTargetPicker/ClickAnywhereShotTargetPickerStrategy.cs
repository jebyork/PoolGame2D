using PoolGame.Core.Values;
using PoolGame.Gameplay.Shot;
using UnityEngine;

namespace PoolGame.Gameplay.ShotTargetPicker
{
    [CreateAssetMenu(fileName = "Click Anywhere Target Picker" , menuName = "Shot Target Picker/Click Anywhere" , order = 0)]
    public class ClickAnywhereShotTargetPickerStrategy : ShotTargetPickerStrategy
    {
        [SerializeField] private GameObjectValue cueBallValueStore;
        
        public override ShotTargetPickResult TryPick()
        {
            if (cueBallValueStore.Value == null || 
                !cueBallValueStore.Value.TryGetComponent<IShootable>(out var shotTarget)) 
                return FailedShotTargetPickResult();
            
            Vector3 position = cueBallValueStore.Value.transform.position;
            return new ShotTargetPickResult(shotTarget,  position, true);
        }
    }
}
