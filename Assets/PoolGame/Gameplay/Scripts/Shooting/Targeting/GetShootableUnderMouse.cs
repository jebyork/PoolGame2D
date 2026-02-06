using PoolGame.Core.Helpers;
using PoolGame.Core.Observers;
using UnityEngine;


namespace PoolGame.Gameplay.Shooting.Targeting
{
    [CreateAssetMenu(fileName = "Get Shootable Under Mouse", menuName = "Shooting/Getting Shootable/Get Shootable Under Mouse")]
    public class GetShootableUnderMouse : GetShootableTagetStrategy
    {
        [SerializeField] private LayerMask cueBallLayerMask;
        [SerializeField] private ObservableVector2 mouseScreenPosition;
        
        public override IShootable GetShootable()
        {
            Vector3 worldPoint = MyHelpers.GetScreenToWorldPosition(mouseScreenPosition.Value);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 0f, cueBallLayerMask);
            return !hit ? 
                null : 
                hit.collider.GetComponentInParent<IShootable>();
        }
    }
}
