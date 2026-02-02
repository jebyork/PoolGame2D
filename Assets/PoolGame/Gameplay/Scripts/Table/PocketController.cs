using PoolGame.Core.Helpers;
using UnityEngine;

namespace PoolGame.Core.Game.States.Gameplay.Table
{
    public class PocketController : MonoBehaviour
    {
        private CircleCollider2D _collider2D;

        public float Radius => Circle2D.GetWorldCircleRadius();
        public Vector3 TopAnchor => new(transform.position.x, transform.position.y + Radius, transform.position.z);
        public Vector3 BottomAnchor => new(transform.position.x, transform.position.y - Radius, transform.position.z);

        private CircleCollider2D Circle2D
        {
            get
            {
                if (_collider2D == null)
                {
                    _collider2D = GetComponent<CircleCollider2D>();
                }
                return _collider2D;
            }
        }
    }
}
