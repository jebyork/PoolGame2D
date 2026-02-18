using UnityEngine;

namespace PoolGame.Gameplay.Guides
{
    public interface IGuideHitResponse
    {
        public GuideLineVisualData Resolve(RaycastHit2D hit, Vector2 incomingDir, float cueRadius, float maxLen);
    }
}
