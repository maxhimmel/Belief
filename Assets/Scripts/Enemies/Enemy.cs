using System.Collections;
using MaulGrab.Extensions;
using MaulGrab.Gameplay.Utility;
using UnityEngine;
using Zenject;

namespace MaulGrab.Gameplay.Enemies
{
	public class Enemy : MonoBehaviour,
		IDamageable
	{
		public Transform Transform => _body.Transform;

		[Header( "Death FX" )]
		[SerializeField] private float _deathTorque = 7;
		[SerializeField] private AnimationCurve _deathScaleAnim = AnimationCurve.EaseInOut( 0, 1, 0.5f, 0 );

		private bool _isDead = false;
		private IRigidbody _body;
		private PlaceholderFactory<ParticleSystem> _deathVfxFactory;

		[Inject]
		public void Construct( IRigidbody body,
			PlaceholderFactory<ParticleSystem> deathVfxFactory )
		{
			_body = body;
			_deathVfxFactory = deathVfxFactory;
		}

		public float TakeDamage( DamageEvent damageEvent, Transform instigator, Transform causer )
		{
			if ( _isDead )
			{
				return 0;
			}

			_isDead = true;
			StartCoroutine( PlayDeathFx() );

			return damageEvent.Damage;
		}

		private IEnumerator PlayDeathFx()
		{
			PlayDeathSpin();

			yield return PlayDeathScale();

			PlayDeathVfx();

			Cleanup();
		}

		private void PlayDeathSpin()
		{
			_body.Constraints = BodyConstraints.None;
			_body.AddTorque( Vector3.up * _deathTorque, ForceType.VelocityChange );
		}

		private IEnumerator PlayDeathScale()
		{
			float timer = 0;
			float duration = _deathScaleAnim.GetDuration();
			while ( timer < duration )
			{
				timer += Time.deltaTime;
				timer = Mathf.Min( timer, duration );

				float deathScale = _deathScaleAnim.Evaluate( timer );
				transform.localScale = Vector3.one * deathScale;

				yield return null;
			}
		}

		private void PlayDeathVfx()
		{
			var deathVfx = _deathVfxFactory.Create();
			deathVfx.transform.SetPositionAndRotation( transform.position, transform.rotation );
			deathVfx.Play( true );
		}

		private void Cleanup()
		{
			Destroy( gameObject );
		}
	}
}
