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

		private Rigidbody2D _body;
		private Vector3 _desiredVelocity;
		private Vector3 _velocity;
		private Quaternion _rotation;
		private Vector3 _facingDirection;

		[Inject]
        public void Construct( Rigidbody2D body )
		{
            _body = body;
			_facingDirection = body.transform.up;
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
			_velocity = _body.velocity;
			_rotation = Quaternion.Euler( 0, 0, _body.rotation );

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
			Quaternion facing = Quaternion.LookRotation( _body.transform.forward, _facingDirection );
			_rotation = Quaternion.RotateTowards( _rotation, facing, angleDelta );
		}

		private void Apply()
		{
			_body.velocity = _velocity;
			_body.SetRotation( _rotation );
		}
	}
}
