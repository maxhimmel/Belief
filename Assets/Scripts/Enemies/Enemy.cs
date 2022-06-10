using UnityEngine;

namespace MaulGrab.Gameplay.Enemies
{
	public class Enemy : MonoBehaviour,
		IDamageable
	{
		public Transform Transform => transform;

		public float TakeDamage( DamageEvent damageEvent, Transform instigator, Transform causer )
		{
			Debug.LogError( $"Ouch! I: {instigator} | C: {causer}", this );
			return damageEvent.Damage;
		}
	}
}
