using PoolGame.Core.Helpers;
using PoolGame.Gameplay.Ball;
using PoolGame.Gameplay.Ball.Spawning;
using PoolGame.Gameplay.GameMode;
using UnityEngine;

namespace PoolGame.Gameplay.Pickups
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PickupSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BallContainer ballContainer;
        
        [Header("Spawn Rules")]
        [SerializeField] private GameObject pickupPrefab;
        [SerializeField] private float gapFromBalls = 0.5f;
        [SerializeField] private int maxAttempts = 50;

        private BoxCollider2D _boxCollider;
        
        #region Lifecycle
        
        private void Reset()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
        }
        
        private void OnEnable()
        {
            PickupManager.OnSpawnPickup += Spawn;
        }

        private void OnDisable()
        {
            PickupManager.OnSpawnPickup -= Spawn;
        }

        private void Awake()
        {
            if (_boxCollider == null)
            {
                _boxCollider = GetComponent<BoxCollider2D>();
            }
        }
        
        #endregion
        
        public void Spawn(PickupData pickupData)
        {
            if (!HasCorrectReferences(pickupPrefab))
                return;

            if (pickupData == null)
                return;

            if (!GetPlacementRadius(pickupPrefab, out var pickupRadius)) 
                return;

            if (!TryGetSpawnPosition(pickupRadius, out var spawnPosition)) 
                return;

            GameObject pickupObject = Instantiate(pickupPrefab, spawnPosition, Quaternion.identity, transform);
            SetPickupData(pickupObject, pickupData);
        }
        
        private bool HasCorrectReferences(GameObject prefab)
        {
            if (pickupPrefab == null)
            {
                Debug.LogError("[PickupSpawner] pickupPrefab is null.", this);
                return false;
            }

            if (_boxCollider == null)
            {
                Debug.LogError("[PickupSpawner] BoxCollider2D reference is missing.", this);
                return false;
            }

            if (ballContainer != null) 
                return true;
            Debug.LogError("[PickupSpawner] BallContainer reference is missing.", this);
            return false;

        }
        
        private bool TryGetSpawnPosition(float pickupRadius, out Vector2 spawnPosition)
        {
            if (BoxSpawnUtility.TryFindSpawnPosition(_boxCollider, 
                    pickupRadius,
                    maxAttempts,
                    pos => IsFarEnoughFromBalls(pos, pickupRadius),
                    out spawnPosition)) 
                return true;
            
            Debug.LogWarning("[PickupSpawner] Failed to find a valid pickup spawn position.", this);
            return false;

        }

        private bool GetPlacementRadius(GameObject prefab, out float pickupRadius)
        {
            return BoxSpawnUtility.TryGetPlacementRadius(prefab, this, nameof(PickupSpawner), out pickupRadius);
        }
        
        private bool IsFarEnoughFromBalls(Vector2 pos, float pickupRadius)
        {
            foreach (BallController ball in ballContainer.ActiveBalls)
            {
                if (ball == null || !ball.TryGetComponent(out CircleCollider2D ballCollider))
                    continue;

                Vector2 ballCenter = ballCollider.transform.TransformPoint(ballCollider.offset);
                float minDistance = ballCollider.GetWorldCircleRadius() + pickupRadius + gapFromBalls;
                if ((pos - ballCenter).sqrMagnitude < minDistance * minDistance)
                    return false;
            }

            return true;
        }
        
        private void SetPickupData(GameObject pickupObject, PickupData pickupData)
        {
            SpriteRenderer spriteRenderer = pickupObject.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = pickupData.Visual;
            }
            
            Pickup pickup = pickupObject.GetComponent<Pickup>();
            if (pickup != null)
            {
                pickup.SetEffect(pickupData.PickupEffect, pickupData.BallType, pickupData.anyBallType);
            }
        }
    }
}
