using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaulGrab.Gameplay.Utility
{
	[RequireComponent( typeof( Collider2D ) )]

	public class LifecycleTriggerEnter : LifecycleService
	{
		private void OnTriggerEnter2D( Collider2D collision )
		{
			Expire();
		}
	}
}
