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
		private LifetimeService _lifetimeService;

		[Inject]
		public void Construct( Rigidbody2D body,
			LifetimeService lifetimeService )
		{
            _body = body;
			_lifetimeService = lifetimeService;
		}

        public void Fire( Vector3 velocity )
		{
			_body.AddForce( velocity, ForceMode2D.Impulse );
			_lifetimeService.Start();
		}

		private void OnTriggerEnter2D( Collider2D collision )
		{
			_lifetimeService.Expire();
		}

		public void OnExpired()
		{
			Destroy( gameObject );
		}

		public class Factory : PlaceholderFactory<Projectile> { }
    }
}
