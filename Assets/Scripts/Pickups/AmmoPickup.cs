using System.Collections;
using System.Collections.Generic;
using MaulGrab.Gameplay.Pickups;
using MaulGrab.Gameplay.Utility;
using UnityEngine;

namespace MaulGrab
{
    public class AmmoPickup : MonoBehaviour
    {
		public int AmmoCount => _ammoCount;

		[SerializeField] private int _ammoCount = 1;
		[SerializeField] private bool _destroyOnPickup = true;

		private void OnTriggerEnter( Collider collision )
		{
			if ( IsValid( collision, out var collector ) )
			{
				collector?.Collect( this );

				if ( _destroyOnPickup )
				{
					Destroy( gameObject );
				}
			}
		}

		private bool IsValid( Collider other, out ICollector<AmmoPickup> collector )
		{
			collector = null;

			var body = other.attachedRigidbody;
			if ( body == null )
			{
				return false;
			}

			collector = body.GetComponent<ICollector<AmmoPickup>>();
			return collector != null;
		}
	}
}
