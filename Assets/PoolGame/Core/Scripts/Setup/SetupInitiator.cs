using System.Collections.Generic;
using UnityEngine;

namespace PoolGame.Core.Setup
{
    public class SetupInitiator : MonoBehaviour, ISetupControl
    {
        [SerializeField] private List<MonoBehaviour> setupComponents = new List<MonoBehaviour>();
        private readonly List<ISetupControl> _setupControls = new List<ISetupControl>();

        private void OnEnable()
        {
            foreach (MonoBehaviour component in setupComponents)
            {
                if (component is ISetupControl setupControl)
                {
                    _setupControls.Add(setupControl);
                    continue;
                }
                Debug.LogError($"{component.name} does not implement ISetupControl", component);
            }
        }

        private void Start()
        {
            Initialize();
            CreateObjects();
            Prepare();
            StartGame();
        }
        
        public void Initialize()
        {
            foreach (ISetupControl setupControl in _setupControls)
            {
                setupControl.Initialize();
            }
        }
        
        public void CreateObjects()
        {
            foreach (ISetupControl setupControl in _setupControls)
            {
                setupControl.CreateObjects();
            }
        }
        
        public void Prepare()
        {
            foreach (ISetupControl setupControl in _setupControls)
            {
                setupControl.Prepare();
            }
        }
        
        public void StartGame()
        {
            foreach (ISetupControl setupControl in _setupControls)
            {
                setupControl.StartGame();
            }
        }
    }
}
