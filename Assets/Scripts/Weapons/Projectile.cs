using MaulGrab.Gameplay.Utility;
using UnityEngine;
using Zenject;

namespace MaulGrab.Gameplay.Weapons
{
    public class Projectile : MonoBehaviour, IExpiry
    {
		private IRigidbody _body;

		[Inject]
		public void Construct( IRigidbody body )
		{
            _body = body;
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
