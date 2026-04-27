using System;
using System.Collections.Generic;
using PoolGame.Core.Helpers;
using PoolGame.Gameplay.Attributes;
using UnityEngine;
using UnityEngine.UIElements;

namespace PoolGame.Gameplay.UI
{
    public class ScoreIndicator : MonoBehaviour
    {
        private const string TemplateResourcePath = "ScoreBox";
        private const string ScoreContainerName = "ScoreContainer";
        private const string ScoreSlotName = "ScoreBox";
        private const int MaxDisplayedScore = 99999;
        
        [SerializeField] private int maxSlots = 5;
        [SerializeField] private Score scoreAttribute;
        [SerializeField] private int initialIcons = 4;

        private readonly List<VisualElement> _scoreSlots = new();
        private VisualTreeAsset _template;
        private VisualElement _scoreContainer;

        #region Lifecycle
        
        private void OnEnable()
        {
            _template = Resources.Load<VisualTreeAsset>(TemplateResourcePath);
            UIDocument document = GetComponent<UIDocument>();
            _scoreContainer = document.rootVisualElement.Q<VisualElement>(ScoreContainerName);
            
            if (!scoreAttribute)
                return;

            scoreAttribute.OnAttributeChanged += ScoreChanged;
            BuildDisplay();
        }
        
        private void OnDisable()
        {
            if (!scoreAttribute)
                return;

            scoreAttribute.OnAttributeChanged -= ScoreChanged;
        }
        
        #endregion

        private void ScoreChanged(int val)
        {
            int clampedValue = Mathf.Clamp(val, 0, MaxDisplayedScore);
            int minimumSlots = Mathf.Min(initialIcons, maxSlots);
            List<int> digits = MyHelpers.GetDigits(clampedValue, minimumSlots);

            while (_scoreSlots.Count < digits.Count)
            {
                if (!TryAddIcon())
                    return;
            }

            int digitOffset = _scoreSlots.Count - digits.Count;

            for (int i = 0; i < _scoreSlots.Count; i++)
            {
                Label label = _scoreSlots[i].Q<Label>();

                if (label == null)
                    continue;

                label.text = i < digitOffset ? "0" : digits[i - digitOffset].ToString();
            }
        }

        private void BuildDisplay()
        {
            if (_scoreContainer == null || _template == null) 
                return;
            
            ClearIcons();
            SetIconCount(Mathf.Min(initialIcons, maxSlots));
        }
        
        private void SetIconCount(int count)
        {
            while (_scoreSlots.Count < count)
            {
                if (!TryAddIcon())
                    return;
            }

            while (_scoreSlots.Count > count)
                RemoveLastIcon();
        }

        private bool TryAddIcon()
        {
            TemplateContainer instance = _template.Instantiate();
            VisualElement slot = instance.Q<VisualElement>(ScoreSlotName);

            if (slot == null)
                return false;
            
            Label scoreLabel = slot.Q<Label>();
            if (scoreLabel == null)
                return false;

            scoreLabel.text = "0";

            _scoreContainer.Add(instance);
            _scoreSlots.Add(slot);
            return true;
        }

        private void ClearIcons()
        {
            while (_scoreSlots.Count > 0)
                RemoveLastIcon();

            RemoveUntrackedIcons();
        }
        
        
        private void RemoveLastIcon()
        {
            int lastIndex = _scoreSlots.Count - 1;
            VisualElement slot = _scoreSlots[lastIndex];

            slot.parent.RemoveFromHierarchy();
            _scoreSlots.RemoveAt(lastIndex);
        }

        private void RemoveUntrackedIcons()
        {
            if (_scoreContainer == null)
                return;

            List<VisualElement> childrenToRemove = new();

            foreach (VisualElement child in _scoreContainer.Children())
            {
                if (child.name == ScoreSlotName || child.Q<VisualElement>(ScoreSlotName) != null)
                    childrenToRemove.Add(child);
            }

            foreach (VisualElement child in childrenToRemove)
                child.RemoveFromHierarchy();
        }
        
        
    }
}
