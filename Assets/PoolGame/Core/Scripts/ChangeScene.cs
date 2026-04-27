using UnityEngine;
using UnityEngine.SceneManagement;

namespace PoolGame.Core
{
    public class ChangeScene : MonoBehaviour
    {
        [SerializeField] private string sceneName;

        public void LoadScene()
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Debug.LogWarning($"[{gameObject.name} | ChangeScene] No scene name assigned.", this);
                return;
            }

            SceneManager.LoadScene(sceneName);
        }
    }
}
