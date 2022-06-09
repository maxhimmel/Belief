using UnityEngine;

namespace MaulGrab.Gameplay.Utility
{
    [System.Serializable]
    public class CollisionQueryData
    {
        public LayerMask DetectionLayer = -1;
        public QueryTriggerInteraction QueryTriggers = QueryTriggerInteraction.UseGlobal;
    }
}
