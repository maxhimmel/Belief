using MaulGrab.Extensions;
using UnityEngine;

namespace MaulGrab.Gameplay.Utility
{
    public class GravityWell : MonoBehaviour
    {
        [SerializeField] private LayerMask _effectLayer = -1;

        [Space]
        [SerializeField] private ForceMode _mode = ForceMode.Force;
        [SerializeField] private float _force = 10;

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
			Vector3 dir = (transform.position - body.position).normalized;
			body.AddForce( dir * _force, _mode );
		}
	}
}
