using System;
using PoolGame.Core.Helpers;
using PoolGame.Gameplay.GameMode;
using PoolGame.Gameplay.Pickups;
using UnityEngine;

namespace PoolGame.Gameplay.Ball.Spawning
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PickupSpawner : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D boxCollider;
        [SerializeField] private BallContainer ballContainer;
        
        [SerializeField] private GameObject pickupPrefab;
        
        [SerializeField] private float gapFromBalls = 0.5f;
        [SerializeField] private int maxAttempts = 50;

        private void OnEnable()
        {
            PickupManager.OnSpawnPickup += Spawn;
        }

        private void OnDisable()
        {
            PickupManager.OnSpawnPickup -= Spawn;
        }

        private void Reset()
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }

        private void Awake()
        {
            if (boxCollider == null)
            {
                boxCollider = GetComponent<BoxCollider2D>();
            }
        }
        
        public void Spawn(PickupData pickupData)
        {
            if (!CanSpawn(pickupPrefab))
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
                pickup.SetEffect(pickupData.PickupEffect, pickupData.BallType);
            }
        }

        private bool TryGetSpawnPosition(float pickupRadius, out Vector2 spawnPosition)
        {
            if (!BoxSpawnUtility.TryFindSpawnPosition(
                    boxCollider,
                    pickupRadius,
                    maxAttempts,
                    candidate => IsFarEnoughFromBalls(candidate, pickupRadius),
                    out spawnPosition))
            {
                Debug.LogWarning("[PickupSpawner] Failed to find a valid pickup spawn position.", this);
                return false;
            }

            return true;
        }

        private bool GetPlacementRadius(GameObject pickupPrefab, out float pickupRadius)
        {
            if (!BoxSpawnUtility.TryGetPlacementRadius(
                    pickupPrefab,
                    this,
                    nameof(PickupSpawner),
                    out pickupRadius))
            {
                return false;
            }

            return true;
        }

        private bool CanSpawn(GameObject pickupPrefab)
        {
            if (pickupPrefab == null)
            {
                Debug.LogError("[PickupSpawner] pickupPrefab is null.", this);
                return false;
            }

            if (boxCollider == null)
            {
                Debug.LogError("[PickupSpawner] BoxCollider2D reference is missing.", this);
                return false;
            }

            if (ballContainer == null)
            {
                Debug.LogError("[PickupSpawner] BallContainer reference is missing.", this);
                return false;
            }

            return true;
        }

        private bool IsFarEnoughFromBalls(Vector2 candidate, float pickupRadius)
        {
            foreach (BallController ball in ballContainer.ActiveBalls)
            {
                if (ball == null || !ball.TryGetComponent(out CircleCollider2D ballCollider))
                    continue;

                Vector2 ballCenter = ballCollider.transform.TransformPoint(ballCollider.offset);
                float minDistance = ballCollider.GetWorldCircleRadius() + pickupRadius + gapFromBalls;
                if ((candidate - ballCenter).sqrMagnitude < minDistance * minDistance)
                    return false;
            }

            return true;
        }
    }
}
