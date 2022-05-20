using MaulGrab.Gameplay.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace MaulGrab.Gameplay.Movement
{
    public class CharacterMotor : MonoBehaviour
    {
		[BoxGroup( "Movement" )]
        [SerializeField] private float _maxSpeed = 5;
		[BoxGroup( "Movement" )]
		[SerializeField] private float _acceleration = 10;

		[BoxGroup( "Rotation" )]
		[SerializeField] private float _rotationSpeed = 720;

		private IRigidbody _body;
		private Vector3 _desiredVelocity;
		private Vector3 _velocity;
		private Quaternion _rotation;
		private Vector3 _facingDirection;

		[Inject]
        public void Construct( IRigidbody body )
		{
            _body = body;
			_facingDirection = body.Transform.forward;
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
			UpdateState();
			Accelerate();
			Apply();
		}

		private void UpdateState()
		{
			_velocity = _body.Velocity;
			_rotation = _body.Rotation;

			if ( _velocity.sqrMagnitude > 0.01f )
			{
				_facingDirection = _velocity;
			}
		}

		private void Accelerate()
		{
			// Movement
			float speedDelta = _acceleration * Time.deltaTime;
			_velocity = Vector3.MoveTowards( _velocity, _desiredVelocity, speedDelta );

			// Rotation
			float angleDelta = _rotationSpeed * Time.deltaTime;
			Quaternion facing = Quaternion.LookRotation( _facingDirection, Vector3.up );
			_rotation = Quaternion.RotateTowards( _rotation, facing, angleDelta );
		}

		private void Apply()
		{
			_body.Velocity = _velocity;
			_body.Rotation = _rotation;
		}
	}
}
