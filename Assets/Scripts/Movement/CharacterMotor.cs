using MaulGrab.Gameplay.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace MaulGrab.Gameplay.Movement
{
    public class CharacterMotor : MonoBehaviour
    {
		public bool IsGrounded => _groundContactCount > 0;
		private bool IsOnSteep => _steepContactCount > 0;

		[BoxGroup( "Movement" )]
        [SerializeField] private float _maxSpeed = 5;
		[BoxGroup( "Movement" )]
		[SerializeField] private float _acceleration = 10;

		[BoxGroup( "Rotation" )]
		[SerializeField] private float _rotationSpeed = 720;

		[BoxGroup( "Ground Detection" )]
		[SerializeField, Range( 0, 90 )] private float _maxGroundAngle = 50;
		[BoxGroup( "Ground Detection" )]
		[SerializeField, Range( 0, 100 )] private float _maxGroundSnapSpeed = 6;
		[BoxGroup( "Ground Detection" )]
		[SerializeField, Min( 0 )] private LayerMask _groundCheckLayer = -1;
		[BoxGroup( "Ground Detection" )]
		[SerializeField, Min( 0 )] private float _groundProbeDistance = 0.5f;

		private IRigidbody _body;
		private World _world;

		private Vector3 _desiredVelocity;
		private Vector3 _velocity;
		private Quaternion _rotation;
		private Vector3 _facingDirection;
		private int _stepsSinceLastGrounded;
		private Vector3 _groundNormal;
		private int _groundContactCount;
		private Vector3 _steepNormal;
		private int _steepContactCount;

		[Inject]
        public void Construct( IRigidbody body,
			World world )
		{
            _body = body;
			_world = world;

			_facingDirection = body.Transform.forward;
			_groundNormal = world.Up;
			_groundContactCount = 0;
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
			ApplyGravity();
			Apply();

			ClearState();
		}

		private void UpdateState()
		{
			_velocity = _body.Velocity;
			_rotation = _body.Rotation;

			++_stepsSinceLastGrounded;

			if ( IsGrounded || TrySnapToGround() )
			{
				_stepsSinceLastGrounded = 0;
				_groundNormal.Normalize();
			}
			else
			{
				_groundNormal = _world.Up;

				if ( IsOnSteep )
				{
					_steepNormal.Normalize();
				}
			}

			if ( _velocity.sqrMagnitude > 0.01f )
			{
				Vector3 flattenedFacingDir = Vector3.ProjectOnPlane( _velocity, _groundNormal ).normalized;
				if ( flattenedFacingDir.sqrMagnitude > 0.01f )
				{
					_facingDirection = flattenedFacingDir;
				}
			}
		}

		private bool TrySnapToGround()
		{
			if ( _stepsSinceLastGrounded > 1 )
			{
				// Stop trying to snap to the ground after we've checked this once ...
				return false;
			}

			if ( _velocity.sqrMagnitude > _maxGroundSnapSpeed * _maxGroundSnapSpeed )
			{
				return false;
			}
			if ( !Physics.Raycast( _body.Position, -_world.Up, out RaycastHit hitInfo, _groundProbeDistance, _groundCheckLayer ) )
			{
				return false;
			}
			if ( Vector3.Angle( _world.Up, hitInfo.normal ) > _maxGroundAngle )
			{
				return false;
			}

			_groundContactCount = 1;
			_groundNormal = hitInfo.normal;

			float dot = Vector3.Dot( _velocity, hitInfo.normal );
			if ( dot > 0 )
			{
				float speed = _velocity.magnitude;
				_velocity = Vector3.ProjectOnPlane( _velocity, hitInfo.normal ).normalized * speed;
			}

			return true;
		}

		private void Accelerate()
		{
			// Movement
			Vector3 xAxis = Vector3.ProjectOnPlane( Vector3.right, _groundNormal ).normalized;
			Vector3 zAxis = Vector3.ProjectOnPlane( Vector3.forward, _groundNormal ).normalized;

			float currentX = Vector3.Dot( _velocity, xAxis );
			float currentZ = Vector3.Dot( _velocity, zAxis );

			float speedDelta = _acceleration * Time.deltaTime;
			float newX = Mathf.MoveTowards( currentX, _desiredVelocity.x, speedDelta );
			float newZ = Mathf.MoveTowards( currentZ, _desiredVelocity.z, speedDelta );

			float deltaX = newX - currentX;
			float deltaZ = newZ - currentZ;
			Vector3 deltaVelocity = xAxis * deltaX + zAxis * deltaZ;

			if ( !IsGrounded && IsOnSteep )
			{
				Vector3 rotationAxis = Vector3.Cross( _steepNormal, _world.Up );
				Vector3 forward = Quaternion.AngleAxis( 90, rotationAxis ) * _steepNormal;

				deltaVelocity = Vector3.ProjectOnPlane( deltaVelocity, forward );
			}

			_velocity += deltaVelocity;


			// Rotation
			float angleDelta = _rotationSpeed * Time.deltaTime;
			Quaternion facing = Quaternion.LookRotation( _facingDirection, _groundNormal );
			_rotation = Quaternion.RotateTowards( _rotation, facing, angleDelta );
		}

		private void ApplyGravity()
		{
			Vector3 gravity = _world.Gravity;

			if ( IsGrounded && _velocity.sqrMagnitude < 0.01f )
			{
				// Prevents sliding down slopes ...
				Vector3 gravityProjection = Vector3.Project( gravity, _groundNormal );
				_velocity += gravityProjection * Time.deltaTime;
			}
			else
			{
				_velocity += gravity * Time.deltaTime;
			}
		}

		private void Apply()
		{
			_body.Velocity = _velocity;
			_body.Rotation = _rotation;
		}

		private void ClearState()
		{
			_groundContactCount = 0;
			_groundNormal = Vector3.zero;

			_steepContactCount = 0;
			_steepNormal = Vector3.zero;
		}

		private void OnCollisionEnter( Collision collision )
		{
			EvaluateGroundCollision( collision );
		}

		private void OnCollisionStay( Collision collision )
		{
			EvaluateGroundCollision( collision );
		}

		private void EvaluateGroundCollision( Collision collision )
		{
			for ( int idx = 0; idx < collision.contactCount; ++idx )
			{
				var contact = collision.GetContact( idx );
				float angle = Vector3.Angle( _world.Up, contact.normal );

				if ( angle <= _maxGroundAngle )
				{
					++_groundContactCount;
					_groundNormal += contact.normal;
				}
				else
				{
					++_steepContactCount;
					_steepNormal += contact.normal;
				}
			}
		}
	}
}
