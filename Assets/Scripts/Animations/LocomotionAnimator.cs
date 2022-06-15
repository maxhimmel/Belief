using MaulGrab.Gameplay.Utility;
using UnityEngine;
using Zenject;
using MaulGrab.Gameplay.Movement;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MaulGrab.Gameplay.Animation
{
    public class LocomotionAnimator : MonoBehaviour
    {
		[Header( "Feet" )]
		[SerializeField] private Transform _rhsFootTarget = default;
		[SerializeField] private Transform _lhsFootTarget = default;

		[Space]
		[SerializeField, MinMaxSlider( 0, 2 )] private Vector2 _gaitRadiusRange = new Vector2( 0, 0.4f );
		[SerializeField, Range( 0, 2 )] private float _gaitSpeedScalar = 1;

		private CharacterMotor _motor;
		private float _counter = 0;

		[Inject]
		public void Construct( CharacterMotor motor )
		{
            _motor = motor;
		}

		private void Update()
		{
			float speed = _motor.Velocity.magnitude;
			_counter += Time.deltaTime * speed * Mathf.PI * _gaitSpeedScalar;

			float gaitRadius = GetGaitRadius();
			Vector3 rhsGaitPos = gaitRadius * Vector3.forward * Mathf.Sin( _counter ) 
								+ gaitRadius * Vector3.up * Mathf.Cos( _counter );
			Vector3 lhsGaitPos = gaitRadius * Vector3.forward * Mathf.Sin( _counter + Mathf.PI ) 
								+ gaitRadius * Vector3.up * Mathf.Cos( _counter + Mathf.PI );

			Vector3 gaitPivot = Vector3.up * gaitRadius;
			_rhsFootTarget.localPosition = rhsGaitPos + gaitPivot;
			_lhsFootTarget.localPosition = lhsGaitPos + gaitPivot;
		}

		private float GetGaitRadius()
		{
			float speed = _motor.Velocity.magnitude;
			float normalizedSpeed = speed / _motor.MaxSpeed;

			return Mathf.Lerp( _gaitRadiusRange.x, _gaitRadiusRange.y, normalizedSpeed );
		}

#if UNITY_EDITOR
		[Header( "Editor/Tools" )]
		[SerializeField] private Color _gaitColor = Color.red;

		private void OnDrawGizmosSelected()
		{
			float gaitRadius = Application.isPlaying
				? GetGaitRadius()
				: _gaitRadiusRange.y;

			Vector3 pivot = transform.position + Vector3.up * gaitRadius;

			Gizmos.color = _gaitColor;
			Gizmos.DrawWireSphere( pivot, gaitRadius * 0.2f );

			Handles.color = _gaitColor;
			Handles.DrawWireDisc( pivot, transform.right, gaitRadius );
		}
#endif
	}
}
