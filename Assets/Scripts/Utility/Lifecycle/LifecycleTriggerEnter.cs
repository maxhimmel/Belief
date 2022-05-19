using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaulGrab.Gameplay.Utility
{
	[RequireComponent( typeof( Collider ) )]

	public class LifecycleTriggerEnter : LifecycleService
	{
		private void OnTriggerEnter( Collider collision )
		{
			Expire();
		}
	}
}
