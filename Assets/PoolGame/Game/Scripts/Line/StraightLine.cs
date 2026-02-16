using UnityEngine;

namespace PoolGame.Game.Line
{
    public class StraightLine : AbstractLine
    {
        public void SetPositions(Vector2 startPos, Vector2 endPos)
        {
            if (!lineR) return;
            float z = transform.position.z;
            lineR.positionCount = 2;
            lineR.SetPosition(0, new Vector3(startPos.x, startPos.y, z));
            lineR.SetPosition(1, new Vector3(endPos.x, endPos.y, z));
        }
    }
}
