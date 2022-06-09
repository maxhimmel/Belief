using MaulGrab.Extensions;
using UnityEngine;

namespace MaulGrab.Gameplay.Utility
{
    public class ForceArea : MonoBehaviour
    {
        [SerializeField] private LayerMask _effectLayer = -1;
		
		[Space]
        [SerializeField] private Space _space = Space.World;
		[SerializeField] private ForceMode _mode = ForceMode.Force;
        [SerializeField] private Vector3 _force = Vector3.zero;

		private void OnTriggerEnter( Collider other )
		{
			if ( IsValid( other, out var body ) )
			{
				ApplyForce( body );
			}
		}

		private void OnTriggerStay( Collider other )
		{
			if ( IsValid( other, out var body ) )
			{
				ApplyForce( body );
			}
		}

		private bool IsValid( Collider other, out Rigidbody body )
		{
			body = null;

			LayerMask otherLayer = 1 << other.gameObject.layer;
			if ( !_effectLayer.HasFlag( otherLayer ) )
			{
				return false;
			}

			body = other.attachedRigidbody;
			return body != null;
		}

		private void ApplyForce( Rigidbody body )
		{
			body.AddForce( GetForce(), _mode );
		}

		private Vector3 GetForce()
		{
			return _space == Space.Self
				? transform.TransformDirection( _force )
				: _force;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawRay( transform.position, GetForce() );
		}
	}
}
