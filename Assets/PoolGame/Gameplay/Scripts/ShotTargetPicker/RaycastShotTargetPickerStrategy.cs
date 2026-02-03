using PoolGame.Core.Game.States.Gameplay.Shot;
using PoolGame.Game.Scripts.ScreenToWorld;
using PoolGame.Gameplay.Shot;
using UnityEngine;

namespace PoolGame.Gameplay.ShotTargetPicker
{
    [CreateAssetMenu(fileName = "Raycast Shot Target Picker" , menuName = "Shot Target Picker/Raycast")]
    public class RaycastShotTargetPickerStrategy : ShotTargetPickerStrategy
    {
        [SerializeField] private LayerMask cueBallLayerMask;
        [SerializeField] private ScreenToWorldStrategy screenToWorld;

        public override ShotTargetPickResult TryPick()
        {
            if (screenToWorld == null)
            {
                return ReturnFailedShotTargetPickResult();
            }

            
            Vector3 worldPoint = screenToWorld.ScreenToWorld();
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 0f, cueBallLayerMask);
            if (!hit) return ReturnFailedShotTargetPickResult();
            
            IShootable shotTarget = hit.collider.GetComponentInParent<IShootable>();
            Vector3 position = hit.collider.transform.position;
            bool success = hit;
            
            return new ShotTargetPickResult(shotTarget,  position, success);
        }
        
        private static ShotTargetPickResult ReturnFailedShotTargetPickResult()
        {
            return new ShotTargetPickResult(
                null, 
                Vector3.zero, 
                false);
        }
    }
}