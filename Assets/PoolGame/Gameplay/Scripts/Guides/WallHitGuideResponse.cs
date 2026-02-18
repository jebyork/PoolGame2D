using UnityEngine;

namespace PoolGame.Gameplay.Guides
{
    public class WallHitGuideResponse : MonoBehaviour, IGuideHitResponse
    {
        public GuideLineVisualData Resolve(RaycastHit2D hit, Vector2 incomingDir, float cueRadius, float maxLen)
        {
            Vector2 inDir = incomingDir.normalized;
            Vector2 outDir = Vector2.Reflect(inDir, hit.normal).normalized;
            Vector2 center = hit.centroid;
            Vector2 start = center + outDir * cueRadius;
            Vector2 end = start + outDir * maxLen;

            return new GuideLineVisualData(start, end);
        }
    }
}
