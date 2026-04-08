using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace PoolGame.Game.UI
{
    [UxmlElement]
    public partial class VerticalSlider : VisualElement
    {
        public event Action<float> onValueChangedAction;
        
        private const string StyleResource = "StyleSheet-VerticalSlider";

        private const string UssBaseSliderPanel = "Vertical_Slider";
        private const string UssBackgroundElement = "Vertical_Slider_Background";
        private const string UssSliderVisualElement = "Vertical_Slider_Slider";

        private VisualElement _sliderVisual;
        private VisualElement _backgroundElement;
        
        private float _selectAmount = .5f;
        private bool _isDragging;


        [UxmlAttribute]
        public float SelectAmount
        {
            get => _selectAmount;
            set
            {
                float clamped = Mathf.Clamp01(value);
                if (Mathf.Approximately(_selectAmount, clamped))
                    return;
                
                
                _selectAmount = clamped;
                
                UpdateSelectAmount();
                UpdateBarColour();
                onValueChangedAction?.Invoke(_selectAmount);
            }
        }
        
        private Gradient _powerGradient = new Gradient();
        
        [UxmlAttribute]
        public Gradient PowerGradient { get => _powerGradient; 
            set
            {
                _powerGradient = value;
                UpdateBarColour();
            } 
        }

        public VerticalSlider()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(StyleResource));
            
            VisualElement basePanel = CreateBaseElement();
            hierarchy.Add(basePanel);

            _backgroundElement = CreateBackgroundElement();
            basePanel.Add(_backgroundElement);
            
            _sliderVisual = CreateSliderVisualElement();
            _backgroundElement.Add(_sliderVisual);
            
            
            _backgroundElement.RegisterCallback<PointerDownEvent >(OnPointerDown);
            
            _backgroundElement.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            
            _backgroundElement.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }
        
        private void UpdateBarColour()
        {
            _sliderVisual.style.backgroundColor = _powerGradient.Evaluate(SelectAmount);
        }
        
        private void UpdateSelectAmount()
        {
            _sliderVisual.style.height = Length.Percent(_selectAmount * 100f);
        }

        private VisualElement CreateBaseElement()
        {
            VisualElement basePanel = new VisualElement
            {
                name = UssBaseSliderPanel
            };
            basePanel.AddToClassList(UssBaseSliderPanel);
            return basePanel;
        }

        private VisualElement CreateBackgroundElement()
        {
            VisualElement backgroundElement = new VisualElement
            {
                name = UssBackgroundElement
            };
            backgroundElement.AddToClassList(UssBackgroundElement);
            return backgroundElement;
        }

        private VisualElement CreateSliderVisualElement()
        {
            _sliderVisual = new VisualElement
            {
                name = UssSliderVisualElement
            };
            _sliderVisual.AddToClassList(UssSliderVisualElement);
            
            UpdateSelectAmount();
            UpdateBarColour();
            return _sliderVisual;
        }

        private void OnPointerDown(PointerDownEvent  evt)
        {
            _isDragging = true;
            _backgroundElement.CapturePointer(evt.pointerId);
            SetValueFromLocalPosition(evt.localPosition);
            
            evt.StopPropagation();
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (!_isDragging)
                return;

            SetValueFromLocalPosition(evt.localPosition);
            evt.StopPropagation();
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (evt.button != 0)
                return;

            _isDragging = false;

            if (_backgroundElement.HasPointerCapture(evt.pointerId))
                _backgroundElement.ReleasePointer(evt.pointerId);

            evt.StopPropagation();
        }
        
        
        private void SetValueFromLocalPosition(Vector3 localPosition)
        {
            float height = _backgroundElement.contentRect.height;
            if (height <= 0f)
                return;

            // y = 0 at top so have to invert it:
            float value = 1f - Mathf.Clamp01(localPosition.y / height);
            SelectAmount = value;
        }
        
        
    }
}