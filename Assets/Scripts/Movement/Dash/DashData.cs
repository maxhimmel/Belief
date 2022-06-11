using UnityEngine;

namespace MaulGrab.Gameplay.Movement
{
    [System.Serializable]
    public class DashData
    {
        public float Speed = 20;
        public float Distance = 6;
        
        [Range( 0, 1 )]
        public float EndDeceleration = 0.3f;

        [Space]
        public bool DrawDebug = true;
    }
}
