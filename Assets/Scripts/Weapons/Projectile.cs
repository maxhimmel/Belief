using MaulGrab.Gameplay.Utility;
using UnityEngine;
using Zenject;

namespace MaulGrab.Gameplay.Weapons
{
    public class Projectile : MonoBehaviour, IExpiry
    {
		[SerializeField] private DamageEvent _damageEvent = new DamageEvent();

		private Transform _owner;
		private IRigidbody _body;
		private HitBoxService _hitBoxService;

		[Inject]
		public void Construct( [Inject(Id = InstallerID.Owner)] Transform owner,
			IRigidbody body,
			HitBoxService hitBoxService )
		{
			_owner = owner;
            _body = body;

			_hitBoxService = hitBoxService;
			_hitBoxService.NotifyHit += OnHit;
		}

		private void OnHit( object hitBox, IDamageable victim )
		{
			var newDMG = new DamageEvent( _damageEvent );
			newDMG.HitDirection = (victim.Transform.position - _body.Position).normalized;

			victim.TakeDamage( newDMG, _owner, _body.Transform );
		}

		public void Fire( Vector3 velocity, float torque )
		{
			_body.AddForce( velocity, ForceType.Impulse );
			_body.AddTorque( _body.Transform.up * torque, ForceType.Impulse );
		}

		public void OnExpired()
		{
			Destroy( gameObject );
		}

		public class Factory : PlaceholderFactory<Projectile> { }
    }
}
