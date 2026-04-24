using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace PoolGame.Game.UI
{
    public class AttributeDisplay : MonoBehaviour
    {
        [Header("UI Location")]
        [SerializeField] private String attributeUILabel;
        
        [Header("Attribute Display")]
        [SerializeField] private string attributeName;
        [SerializeField] private Attribute.Attribute attribute;

        private Label _nameLabel;
        private Label _valueLabel;
        
        private void OnEnable()
        {
            if (attribute == null) return;
            
            attribute.OnAttributeChanged += UpdateAttributeUI;
            UIDocument document = GetComponent<UIDocument>();
            VisualElement root = document.rootVisualElement;
            
            VisualElement attributeUI = root.Q<VisualElement>(attributeUILabel);
            if(attributeUI == null) return;
            
            _nameLabel = attributeUI.Q<Label>("LabelTitle");
            
            if (_nameLabel == null) return;
            _nameLabel.text = attributeName;
            
            
            _valueLabel = attributeUI.Q<Label>("LabelValue");
            if (_valueLabel == null) return;
            
            UpdateAttributeUI(attribute.GetAttributeValue());
            
            
        }

        private void OnDisable()
        {
            attribute.OnAttributeChanged -= UpdateAttributeUI;
        }

        private void UpdateAttributeUI(int obj)
        {
            if(_valueLabel == null) return;
            _valueLabel.text = obj.ToString();
        }
    }
}