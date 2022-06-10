using System;
using UnityEngine;

namespace MaulGrab.Gameplay
{
    [RequireComponent( typeof( Collider ), typeof( Rigidbody ) )]
    public class HitBox : MonoBehaviour
    {
		public const string HitBoxLayerName = "HitBox";

		public event EventHandler<IDamageable> NotifyHit;

		public void Activate()
		{
			gameObject.SetActive( true );
		}

		public void Deactivate()
		{
			gameObject.SetActive( false );
		}

		private void OnTriggerEnter( Collider other )
		{
			var body = other.attachedRigidbody;
			if ( body == null )
			{
				return;
			}

			if ( body.TryGetComponent<IDamageable>( out var damageable ) )
			{
				NotifyHit?.Invoke( this, damageable );
			}
		}

		private void Awake()
		{
			int hitBoxLayer = LayerMask.NameToLayer( HitBoxLayerName );
			if ( hitBoxLayer != gameObject.layer )
			{
				throw new NotSupportedException( $"{gameObject} should be on the 'HitBox' layer." );
			}
		}
	}
}
