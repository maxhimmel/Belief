using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaulGrab.Gameplay.Animation
{
	public class GunModelToggler : MonoBehaviour,
		IGunAnimator
	{
		public void OnFired()
		{
			gameObject.SetActive( false );
		}

		public void OnFiringEnd()
		{
		}

		public void OnFiringStart()
		{
		}

		public void OnReloaded()
		{
			gameObject.SetActive( true );
		}
	}
}
