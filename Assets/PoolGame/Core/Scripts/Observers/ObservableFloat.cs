using UnityEngine;

namespace PoolGame.Core.Observers
{
    [CreateAssetMenu(fileName = "Observable float", menuName = "Observable/float", order = 0)]
    public class ObservableFloat : Observer<float>
    {
    }
}