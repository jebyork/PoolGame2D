using UnityEngine;

namespace PoolGame.Line
{
    public class StraightLine : AbstractLine
    {
        public void SetPositions(Vector2 startPos, Vector2 endPos)
        {
            if (LineR)
            {
                float z = transform.position.z;
                LineR.positionCount = 2;
                LineR.SetPosition(0, new Vector3(startPos.x, startPos.y, z));
                LineR.SetPosition(1, new Vector3(endPos.x, endPos.y, z));
            }
        }
    }
}
