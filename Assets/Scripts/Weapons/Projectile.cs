using System.Collections;
using System.Collections.Generic;
using MaulGrab.Gameplay.Utility;
using UnityEngine;
using Zenject;

namespace MaulGrab.Gameplay.Weapons
{
    public class Projectile : MonoBehaviour, IExpiry
    {
		private Rigidbody2D _body;

		[Inject]
		public void Construct( Rigidbody2D body )
		{
            _body = body;
		}

        public void Fire( Vector3 velocity, float torque )
		{
			_body.AddForce( velocity, ForceMode2D.Impulse );
			_body.AddTorque( torque, ForceMode2D.Impulse );
		}

		public void OnExpired()
		{
			Destroy( gameObject );
		}

		public class Factory : PlaceholderFactory<Projectile> { }
    }
}
