using System;
using System.Collections;
using PoolGame.Gameplay.Pickups;
using UnityEngine;
using UnityEngine.UIElements;

namespace PoolGame.Gameplay.UI
{
    public class PickupDisplay : MonoBehaviour
    {
        [SerializeField] private float pickupDisplayTime = 3f;
        private Label _pickupLabel;
        private Coroutine _pickupDisplayCoroutine;
        
        private void Awake()
        {
            UIDocument document = GetComponent<UIDocument>();
            VisualElement root = document.rootVisualElement;

            _pickupLabel = root.Q<Label>("PickupText");

            if (_pickupLabel != null)
                _pickupLabel.style.display = DisplayStyle.None;
        }

        private void OnEnable()
        {
            Pickup.OnPickupAcquired += BroadcastPickup;
        }

        private void OnDisable()
        {
            Pickup.OnPickupAcquired -= BroadcastPickup;
        }

        private void BroadcastPickup(string broadcastMessage)
        {
            if (_pickupLabel == null) 
                return;
            _pickupDisplayCoroutine = StartCoroutine(DisplayPickup(broadcastMessage));
        }

        private IEnumerator DisplayPickup(string pickupMessage)
        {
            _pickupLabel.style.display = DisplayStyle.Flex;
            _pickupLabel.text = pickupMessage;

            yield return new WaitForSeconds(pickupDisplayTime);

            _pickupLabel.style.display = DisplayStyle.None;
        }
        
        
    }
}
