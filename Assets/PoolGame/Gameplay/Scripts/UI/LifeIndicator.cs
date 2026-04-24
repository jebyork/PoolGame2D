using System.Collections.Generic;
using PoolGame.Gameplay.Attributes;
using UnityEngine;
using UnityEngine.UIElements;

namespace PoolGame.Gameplay.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class LifeIndicator : MonoBehaviour
    {
        private const string TemplateResourcePath = "LifeIndicator";
        private const string LifeContainerName = "LifeContainer";
        private const string LifeSlotName = "LifeIndicator";
        private const string LifeClass = "Life";
        private const string NoLifeClass = "NoLife";

        [SerializeField] private Life lifeAttribute;

        private readonly List<VisualElement> _lifeSlots = new();
        private VisualTreeAsset _template;
        private VisualElement _lifeContainer;

        private void OnEnable()
        {
            _template = Resources.Load<VisualTreeAsset>(TemplateResourcePath);

            UIDocument document = GetComponent<UIDocument>();
            _lifeContainer = document.rootVisualElement.Q<VisualElement>(LifeContainerName);

            if (!lifeAttribute)
                return;

            lifeAttribute.OnAttributeChanged += UpdateLifeDisplay;
            lifeAttribute.OnMaxLifeChanged += MaxLifeDisplayChanged;

            BuildDisplay();
        }

        private void OnDisable()
        {
            if (!lifeAttribute)
                return;

            lifeAttribute.OnAttributeChanged -= UpdateLifeDisplay;
            lifeAttribute.OnMaxLifeChanged -= MaxLifeDisplayChanged;
        }

        private void MaxLifeDisplayChanged(int newMax)
        {
            if (_lifeContainer == null || _template == null)
                return;

            SetIconCount(newMax);
            UpdateLifeDisplay(lifeAttribute.GetAttributeValue());
        }

        private void BuildDisplay()
        {
            if (_lifeContainer == null || _template == null)
                return;

            ClearIcons();
            SetIconCount(lifeAttribute.MaxLife);
            UpdateLifeDisplay(lifeAttribute.GetAttributeValue());
        }

        private void SetIconCount(int count)
        {
            while (_lifeSlots.Count < count)
            {
                if (!TryAddIcon())
                    return;
            }

            while (_lifeSlots.Count > count)
                RemoveLastIcon();
        }

        private bool TryAddIcon()
        {
            TemplateContainer instance = _template.Instantiate();
            VisualElement slot = instance.Q<VisualElement>(LifeSlotName);

            if (slot == null)
                return false;

            _lifeContainer.Add(instance);
            _lifeSlots.Add(slot);
            return true;
        }

        private void RemoveLastIcon()
        {
            int lastIndex = _lifeSlots.Count - 1;
            VisualElement slot = _lifeSlots[lastIndex];

            slot.parent.RemoveFromHierarchy();
            _lifeSlots.RemoveAt(lastIndex);
        }

        private void ClearIcons()
        {
            while (_lifeSlots.Count > 0)
                RemoveLastIcon();

            RemoveUntrackedIcons();
        }

        private void RemoveUntrackedIcons()
        {
            if (_lifeContainer == null)
                return;

            List<VisualElement> childrenToRemove = new();

            foreach (VisualElement child in _lifeContainer.Children())
            {
                if (child.name == LifeSlotName || child.Q<VisualElement>(LifeSlotName) != null)
                    childrenToRemove.Add(child);
            }

            foreach (VisualElement child in childrenToRemove)
                child.RemoveFromHierarchy();
        }

        private void UpdateLifeDisplay(int currentLives)
        {
            for (int i = 0; i < _lifeSlots.Count; i++)
                SetSlotFilled(_lifeSlots[i], i < currentLives);
        }

        private static void SetSlotFilled(VisualElement slot, bool isFilled)
        {
            slot.EnableInClassList(LifeClass, isFilled);
            slot.EnableInClassList(NoLifeClass, !isFilled);
        }
    }
}
