using UnityEngine;

namespace PoolGame.Gameplay.Visual
{
    public class ChangeSpriteLayer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private int sortingLayer;
        [SerializeField] private int sortingOrder;

        public void ChangeLayerSettings()
        {
            if (!spriteRenderer)
                return;
            
            spriteRenderer.sortingLayerID = SortingLayer.layers[sortingLayer].id;
            spriteRenderer.sortingOrder = sortingOrder;
        }
    }
}