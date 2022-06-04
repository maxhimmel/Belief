using System.Collections;
using System.Collections.Generic;
using MaulGrab.Gameplay.Utility;
using UnityEngine;
using Zenject;

namespace MaulGrab
{
    public class ForceAttractor : MonoBehaviour
    {
        [SerializeField] private LayerMask _effectLayer = -1;

		[Space]
        [SerializeField] private ForceType _mode = ForceType.Force;
        [SerializeField] private float _force = 10;

		private IRigidbody _body = null;

		[Inject]
		public void Construct( IRigidbody body )
		{
			_body = body;
		}

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

		private bool IsValid( Collider other, out Rigidbody otherBody )
		{
			otherBody = null;

			LayerMask otherLayer = 1 << other.gameObject.layer;
			if ( (otherLayer & _effectLayer) == 0 )
			{
				return false;
			}

			otherBody = other.attachedRigidbody;
			return otherBody != null;
		}

		private void ApplyForce( Rigidbody otherBody )
		{
			Vector3 dir = (otherBody.position - _body.Position).normalized;
			_body.AddForce( dir * _force, _mode );
		}
	}
}
