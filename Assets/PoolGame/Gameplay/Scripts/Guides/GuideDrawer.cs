using PoolGame.Game.Line;
using UnityEngine;

namespace PoolGame.Gameplay.Guides
{
    public class GuideDrawer :  MonoBehaviour
    {
        [Header("Line Guide")]
        [SerializeField] private StraightLine firstGuide;
        [SerializeField] private StraightLine secondGuide;
        
        [Space, Header("Circle Guide")]
        [SerializeField] private CircleLine circleGuide;
        
        [SerializeField] private float endOffset;


        private void OnValidate()
        {
            if (firstGuide == null)
            {
                Debug.LogWarning($"First guide drawer has not been assigned on {gameObject.name}.");
            }
            
            if (secondGuide == null)
            {
                Debug.LogWarning($"Second guide drawer has not been assigned on {gameObject.name}.");
            }
            
            if (circleGuide == null)
            {
                Debug.LogWarning($"Circle guide drawer has not been assigned on {gameObject.name}.");
            }
        }

        private void Start()
        {
            HideAllGuide();
        }
        
        public void DrawFirstGuide(GuideLineVisualData data)
        {
            firstGuide?.SetActive(true);
            GuideLineVisualData trimmed = TrimLine(data);
            firstGuide?.SetPositions(trimmed.Start, trimmed.End);
        }

        public void DrawSecondaryGuides(GuideCircleVisualData circleData, GuideLineVisualData lineData)
        {
            circleGuide?.SetActive(true);
            circleGuide?.SetCircle(circleData.Pos, circleData.Radius);
            
            secondGuide?.SetActive(true);
            GuideLineVisualData trimmed = TrimLine(lineData);
            secondGuide?.SetPositions(trimmed.Start, trimmed.End);
        }

        public void HideAllGuide()
        {
            firstGuide?.SetActive(false);
            HideSecondaryGuides();
        }
        
        public void HideSecondaryGuides()
        {
            secondGuide?.SetActive(false);
            circleGuide?.SetActive(false);
        }
        
        private GuideLineVisualData TrimLine(GuideLineVisualData line)
        {
            Vector3 start = line.Start;
            Vector3 end = line.End;

            Vector3 startToEnd = end - start;
            float length = startToEnd.magnitude;

            if (length <= Mathf.Epsilon)
                return line;

            float totalTrim = Mathf.Max(0f, endOffset) + Mathf.Max(0f, endOffset);
            
            if (totalTrim >= length)
            {
                Vector3 midpoint = (start + end) * 0.5f;
                return new GuideLineVisualData(midpoint, midpoint);
            }

            Vector3 direction = startToEnd / length;

            Vector3 trimmedStart = start + direction * endOffset;
            Vector3 trimmedEnd = end - direction * endOffset;

            return new GuideLineVisualData(trimmedStart, trimmedEnd);
        }
    }
    
}