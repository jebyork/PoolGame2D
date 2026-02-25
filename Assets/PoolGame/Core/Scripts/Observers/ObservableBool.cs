using UnityEngine;

namespace PoolGame.Core.Observers
{
    [CreateAssetMenu(fileName = "Observable Bool" , menuName = "Observable/Bool" , order = 0)]
    public class ObservableBool : Observer<bool>
    {
    }
}