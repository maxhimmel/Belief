using UnityEngine;

namespace MaulGrab.Gameplay
{
    [System.Serializable]
    public class DamageEvent 
    {
        public float Damage;
        public float Impulse;
        public Vector3 HitDirection;

        public DamageEvent() { }
        public DamageEvent( DamageEvent source )
		{
            Damage = source.Damage;
            Impulse = source.Impulse;
            HitDirection = source.HitDirection;
		}
    }
}
