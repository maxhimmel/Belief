using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MaulGrab.Gameplay.Weapons
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _lifetime = 1;

		private Rigidbody2D _body;

		[Inject]
		public void Construct( Rigidbody2D body )
		{
            _body = body;
		}

        public void Fire( Vector3 velocity )
		{
			_body.AddForce( velocity, ForceMode2D.Impulse );
		}

		public class Factory : PlaceholderFactory<Projectile> { }
    }
}
