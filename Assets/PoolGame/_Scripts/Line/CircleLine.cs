using UnityEngine;

namespace PoolGame.Line
{
    public class CircleLine : AbstractLine
    {
        [SerializeField, Min(3)] private int segments = 48;
        
        private void Awake()
        {
            if (LineR)
            {
                LineR.loop = true;
                LineR.useWorldSpace = true;
            }
        }
        
        public void SetCircle(Vector2 center, float radius)
        {
            if (LineR == null)
            {
                return;
            }
            
            if (radius <= 0f)
            {
                SetActive(false);
                return;
            }

            SetActive(true);
            LineR.positionCount = segments;

            float angleStep = 2f * Mathf.PI / segments;

            for (int i = 0; i < segments; i++)
            {
                float angleRadians = i * angleStep;
                Vector2 pointOnCircle = center + new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians)) * radius;

                Vector3 pos = new(pointOnCircle.x , pointOnCircle.y , transform.position.z);
                LineR.SetPosition(i, pos);
            }
        }
    }
}
