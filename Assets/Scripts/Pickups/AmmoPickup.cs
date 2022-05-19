using System.Collections;
using System.Collections.Generic;
using MaulGrab.Gameplay.Pickups;
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
			var collector = collision.GetComponentInParent<ICollector<AmmoPickup>>();
			if ( collector != null )
			{
				collector?.Collect( this );

				if ( _destroyOnPickup )
				{
					Destroy( gameObject );
				}
			}
		}
	}
}
