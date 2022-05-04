using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MaulGrab.Gameplay.Movement
{
    public class CharacterMotor : MonoBehaviour
    {
        [SerializeField] private float _maxSpeed = 5;
        [SerializeField] private float _acceleration = 10;

        private Rigidbody2D _body;
		private Vector3 _velocity;
		private Vector3 _desiredVelocity;

		[Inject]
        public void Construct( Rigidbody2D body )
		{
            _body = body;
		}

		public void ClearVelocity()
		{
			_velocity = Vector3.zero;
			_desiredVelocity = Vector3.zero;
		}

        public void SetDesiredVelocity( Vector3 direction )
		{
			_desiredVelocity = direction * _maxSpeed;
		}

		private void FixedUpdate()
		{
			_velocity = _body.velocity;

			float speedDelta = _acceleration * Time.deltaTime;
			_velocity = Vector3.MoveTowards( _velocity, _desiredVelocity, speedDelta );

			_body.velocity = _velocity;
		}
	}
}
