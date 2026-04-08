using PoolGame.Core.Observers;
using PoolGame.Game.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace PoolGame.Gameplay.UI
{
    public class VerticalPowerSliderListener : MonoBehaviour
    {
        [SerializeField] private ObservableFloat power;
        private VerticalSlider _slider;
        
        private void OnEnable()
        {
            UIDocument ui = GetComponent<UIDocument>();
            VisualElement root = ui.rootVisualElement;
            
            _slider = root.Q<VerticalSlider>();

            _slider.onValueChangedAction += PowerChanged;
        }

        private void OnDisable()
        {
            _slider.onValueChangedAction -= PowerChanged;
        }

        private void PowerChanged(float obj)
        {
            power.Value = obj;
        }
    }
}
