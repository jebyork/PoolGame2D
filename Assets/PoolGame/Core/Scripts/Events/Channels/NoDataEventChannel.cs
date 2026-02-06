using UnityEngine;

namespace PoolGame.Core.Events.Channels
{
    [CreateAssetMenu(fileName = "No Data Event Channel", menuName = "Events/No Data Event Channel")]
    public class NoDataEventChannel : AbstractEventChannel<Unit> {}
    
    public struct Unit
    {
        public static readonly Unit Default = new Unit();
    }
}
