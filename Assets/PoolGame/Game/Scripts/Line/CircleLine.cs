using UnityEngine;

namespace PoolGame.Game.Line
{
    public class CircleLine : AbstractLine
    {
        [SerializeField, Min(3)] private int segments = 48;
        
        protected override void Awake()
        {
            base.Awake();
            if (!lineR) return;
            
            lineR.loop = true;
            lineR.useWorldSpace = true;
        }
        
        public void SetCircle(Vector2 center, float radius)
        {
            if (lineR == null)
            {
                return;
            }
            
            if (radius <= 0f)
            {
                SetActive(false);
                return;
            }

            SetActive(true);
            lineR.positionCount = segments;

            float angleStep = 2f * Mathf.PI / segments;

            for (int i = 0; i < segments; i++)
            {
                float angleRadians = i * angleStep;
                Vector2 pointOnCircle = center + new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians)) * radius;

                Vector3 pos = new(pointOnCircle.x , pointOnCircle.y , transform.position.z);
                lineR.SetPosition(i, pos);
            }
        }
    }
}
