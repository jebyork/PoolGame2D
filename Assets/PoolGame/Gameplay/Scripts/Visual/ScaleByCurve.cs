using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PoolGame.Gameplay.Visual
{
    public class ScaleByCurve : MonoBehaviour
    {
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private Transform scaleObject;
        
        public UnityEvent OnFinishedCurve;

        public void StartScale()
        {
            if (scaleObject == null || curve == null) return;
            
            StartCoroutine(ScaleOverTime());
        }

        private IEnumerator ScaleOverTime()
        {
            if (curve.length == 0)
                yield break;

            float elapsed = 0f;
            float duration = curve.keys[curve.length - 1].time;
            Vector3 startScale = scaleObject.transform.localScale;

            while (elapsed < duration)
            {
                float scale = curve.Evaluate(elapsed);
                scaleObject.transform.localScale = startScale * scale;

                elapsed += Time.deltaTime;
                yield return null;
            }

            scaleObject.transform.localScale = startScale * curve.Evaluate(duration);

            OnFinishedCurve?.Invoke();
        }
    }
}
