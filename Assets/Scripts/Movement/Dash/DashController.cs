using System.Collections;
using UnityEngine;

namespace MaulGrab.Gameplay.Movement
{
    public class DashController
    {
		public bool IsDashing => _dashRoutine != null;

		private readonly DashData _data;
		private readonly CharacterMotor _motor;
		private readonly YieldInstruction _yieldInstruction;

		private Coroutine _dashRoutine;
		private float _initialMoveSpeed;
		private float _initialAcceleration;

		public DashController( DashData data, 
			CharacterMotor motor )
		{
			_data = data;
			_motor = motor;
			_yieldInstruction = new WaitForFixedUpdate();
		}

		public void Dash( Vector3 direction )
		{
			if ( IsDashing )
			{
				return;
			}

			_initialMoveSpeed = _motor.MaxSpeed;
			_initialAcceleration = _motor.Acceleration;

			_dashRoutine = _motor.StartCoroutine( UpdateDash( direction ) );
		}

		private IEnumerator UpdateDash( Vector3 direction )
		{
			_motor.ScaleVelocity( 0 );
			_motor.SetMaxSpeed( _data.Speed );
			_motor.SetAcceleration( Mathf.Infinity );

			if ( _data.DrawDebug )
			{
				Debug.DrawRay( _motor.transform.position, Vector3.up * 5, Color.green, 3 );
			}

			Vector3 moveDirection = direction.sqrMagnitude > 0.01f
				? direction.normalized
				: _motor.FacingDirection.normalized;

			float timer = 0;
			float duration = _data.Distance / _data.Speed;
			while ( timer < duration )
			{
				timer += Time.fixedDeltaTime;
				yield return _yieldInstruction;

				_motor.SetDesiredVelocity( moveDirection );
			}

			Cancel();

			if ( _data.DrawDebug )
			{
				Debug.DrawRay( _motor.transform.position, Vector3.up * 5, Color.red, 3 );
			}
		}

		public void Cancel()
		{
			if ( !IsDashing )
			{
				return;
			}

			_motor.ScaleVelocity( 1 - _data.EndDeceleration );
			_motor.SetMaxSpeed( _initialMoveSpeed );
			_motor.SetAcceleration( _initialAcceleration );

			_motor.StopCoroutine( _dashRoutine );
			_dashRoutine = null;
		}
	}
}
