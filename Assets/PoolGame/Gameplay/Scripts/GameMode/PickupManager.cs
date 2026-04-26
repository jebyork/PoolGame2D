using System;
using System.Collections.Generic;
using PoolGame.Gameplay.Pickups;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PoolGame.Gameplay.GameMode
{
    public class PickupManager : MonoBehaviour, ITurnOutcomeHandler
    {
        public static event Action<PickupData> OnSpawnPickup;

        [Header("Components")]
        [SerializeField] private GameState gameState;

        [Header("Spawn Rules")]
        [SerializeField] private bool spawnPickupsOverTime = true;
        [SerializeField] private Vector2Int turnsPerPickup = new(2, 4);
        private int _turnsPerPickup;
        private int _turnsSinceLastPickup;

        [Header("Pickup Pool")]
        [SerializeField] private PickupData[] pickupData;
        [SerializeField, Range(0f, 1f)] private float timeBias = 0.5f;

        private readonly Dictionary<PickupData, int> _lastSpawnTurnByPickup = new();

        private void OnEnable()
        {
            if (gameState != null)
                gameState.RegisterHandler(this);
        }

        private void OnDisable()
        {
            if (gameState != null)
                gameState.UnregisterHandler(this);
        }

        private void Start()
        {
            SetDelayForNextPickup();
        }

        public void OnTurnEvaluate(Action onComplete)
        {
            if (spawnPickupsOverTime && ShouldSpawnPickupThisTurn())
            {
                SetDelayForNextPickup();
                SpawnPickup();
            }
            else
            {
                _turnsSinceLastPickup++;
            }

            onComplete();
        }

        private void SetDelayForNextPickup()
        {
            _turnsSinceLastPickup = 0;
            _turnsPerPickup = Random.Range(turnsPerPickup.x, turnsPerPickup.y + 1);
        }

        [ContextMenu("Spawn Pickup")]
        public void SpawnPickup()
        {
            PickupData nextPickup = GetPickupData();
            if (nextPickup == null)
                return;

            RecordPickupSpawn(nextPickup);
            OnSpawnPickup?.Invoke(nextPickup);
        }

        private bool ShouldSpawnPickupThisTurn()
        {
            if (gameState == null || _turnsPerPickup <= 0)
                return false;

            return _turnsSinceLastPickup >= _turnsPerPickup;
        }

        private PickupData GetPickupData()
        {
            if (pickupData == null || pickupData.Length == 0)
            {
                Debug.LogWarning("[PickupManager] No pickup data configured.", this);
                return null;
            }

            float totalWeight = 0f;
            for (int i = 0; i < pickupData.Length; i++)
            {
                PickupData data = pickupData[i];
                if (data == null)
                    continue;

                totalWeight += GetCombinedSpawnWeight(data);
            }

            if (totalWeight <= 0f)
            {
                Debug.LogWarning("[PickupManager] Pickup weights resolved to zero.", this);
                return null;
            }

            float roll = Random.Range(0f, totalWeight);
            float runningWeight = 0f;

            for (int i = 0; i < pickupData.Length; i++)
            {
                PickupData data = pickupData[i];
                if (data == null)
                    continue;

                runningWeight += GetCombinedSpawnWeight(data);
                if (roll <= runningWeight)
                    return data;
            }

            return pickupData[pickupData.Length - 1];
        }

        private float GetCombinedSpawnWeight(PickupData data)
        {
            float baseWeight = Mathf.Max(0f, data.SpawnRateWeight);
            float timeWeight = GetTurnsSincePickupSpawned(data);
            return Mathf.Max(0f, Mathf.Lerp(baseWeight, timeWeight, timeBias));
        }

        private int GetTurnsSincePickupSpawned(PickupData data)
        {
            if (data == null)
                return 0;

            if (gameState == null)
                return 1;

            if (_lastSpawnTurnByPickup.TryGetValue(data, out int lastSpawnTurn))
                return Mathf.Max(1, gameState.Turn - lastSpawnTurn);

            return Mathf.Max(1, gameState.Turn + 1);
        }

        private void RecordPickupSpawn(PickupData data)
        {
            if (data == null || gameState == null)
                return;

            _lastSpawnTurnByPickup[data] = gameState.Turn;
        }
    }
}
