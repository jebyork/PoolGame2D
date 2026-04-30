using System.Collections;
using UnityEngine;

namespace PoolGame.Gameplay.Visual
{
    public class SceneIntroPlayer : MonoBehaviour
    {
        [SerializeField] private ScaleByCurve scaleByCurve;

        public IEnumerator PlayIntro()
        {
            if(scaleByCurve)
                yield return StartCoroutine(scaleByCurve.ScaleOverTime());
            Destroy(scaleByCurve.gameObject);
        }
    }
}