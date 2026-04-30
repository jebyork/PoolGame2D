using System;
using System.Collections.Generic;
using PoolGame.Gameplay.GameMode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PoolGame.Gameplay.Pickups
{
    public class PickupManager : MonoBehaviour
    {
        public static event Action<PickupData> OnSpawnPickup;
        
        [Header("Spawn Rules")]
        [SerializeField] private bool spawnPickupsOverTime = true;
        [SerializeField] private Vector2Int turnsPerPickup = new(2, 4);
        private int _turnsPerPickup;
        private int _turnsSinceLastPickup;

        [Header("Pickup Pool")]
        [SerializeField] private PickupData[] pickupData;
        [SerializeField, Range(0f, 1f)] private float timeBias = 0.5f;

        private readonly Dictionary<PickupData, int> _lastSpawnTurnByPickup = new();


        #region Lifecycle
        
        private void Start()
        {
            SetDelayForNextPickup();
        }
        
        #endregion


        public void EvaluateTurn(int currentTurn)
        {
            if (spawnPickupsOverTime && ShouldSpawnPickupThisTurn())
            {
                SetDelayForNextPickup();
                SpawnPickup(currentTurn);
            }
            else
            {
                _turnsSinceLastPickup++;
            }
        }
        
        private bool ShouldSpawnPickupThisTurn()
        {
            if (_turnsPerPickup <= 0)
                return false;

            return _turnsSinceLastPickup >= _turnsPerPickup;
        }
        
        private void SetDelayForNextPickup()
        {
            _turnsSinceLastPickup = 0;
            _turnsPerPickup = Random.Range(turnsPerPickup.x, turnsPerPickup.y + 1);
        }

        [ContextMenu("Spawn Pickup")]
        public void SpawnPickup(int currentTurn = 0)
        {
            PickupData nextPickup = GetPickupData(currentTurn);
            if (nextPickup == null)
                return;

            RecordPickupSpawn(nextPickup, currentTurn);
            OnSpawnPickup?.Invoke(nextPickup);
        }

        private PickupData GetPickupData(int currentTurn = 0)
        {
            if (!HasPickupData())
                return null;

            float totalWeight = CalculateTotalWeight(currentTurn);
            if (!(totalWeight <= 0f)) 
                return GetWeightedPickup(totalWeight);
            
            Debug.LogWarning("[PickupManager] Pickup weights resolved to zero.", this);
            return null;

        }

        private bool HasPickupData()
        {
            if (pickupData is { Length: > 0 })
                return true;

            Debug.LogWarning("[PickupManager] No pickup data configured.", this);
            return false;
        }

        private float CalculateTotalWeight(int currentTurn = 0)
        {
            float totalWeight = 0f;

            foreach (PickupData data in pickupData)
            {
                if (data == null)
                    continue;

                totalWeight += GetCombinedSpawnWeight(data, currentTurn);
            }

            return totalWeight;
        }
        
        private float GetCombinedSpawnWeight(PickupData data, int currentTurn = 0)
        {
            float baseWeight = Mathf.Max(0f, data.SpawnRateWeight);
            float timeWeight = GetTurnsSincePickupSpawned(data, currentTurn);
            return Mathf.Max(0f, Mathf.Lerp(baseWeight, timeWeight, timeBias));
        }

        private PickupData GetWeightedPickup(float totalWeight)
        {
            float roll = Random.Range(0f, totalWeight);
            float runningWeight = 0f;
            PickupData lastValidPickup = null;

            foreach (PickupData data in pickupData)
            {
                if (data == null)
                    continue;

                lastValidPickup = data;
                runningWeight += GetCombinedSpawnWeight(data);

                if (roll <= runningWeight)
                    return data;
            }

            return lastValidPickup;
        }
        
        private int GetTurnsSincePickupSpawned(PickupData data, int currentTurn = 0)
        {
            if (data == null)
                return 0;
            
            if (_lastSpawnTurnByPickup.TryGetValue(data, out int lastSpawnTurn))
                return Mathf.Max(1, currentTurn - lastSpawnTurn);

            return Mathf.Max(1, currentTurn + 1);
        }

        private void RecordPickupSpawn(PickupData data, int currentTurn = 0)
        {
            if (data == null)
                return;

            _lastSpawnTurnByPickup[data] = currentTurn;
        }

        public void RemoveAll()
        {
            Pickup[] pickups = FindObjectsByType<Pickup>(FindObjectsSortMode.None);
            foreach (Pickup pickup in pickups)
            {
                Destroy(pickup.gameObject);
            }
        }
    }
}
