using PoolGame.Core.Observers;
using PoolGame.Game.Scripts.ScreenToWorld;
using UnityEngine;

namespace PoolGame.Game.ScreenToWorld
{
    [CreateAssetMenu(fileName = "Camera Screen To World Strategy" , menuName = "Screen To World Strategy/Camera")]
    public class CameraScreenToWorldStrategy : ScreenToWorldStrategy
    {
        [SerializeField] private ObservableVector2 mouseScreenPosition;

        public override Vector3 ScreenToWorld()
        {
            if (Camera.main == null)
                return Vector2.zero;

            return Camera.main.ScreenToWorldPoint(mouseScreenPosition.Value);
        }
    }
}
