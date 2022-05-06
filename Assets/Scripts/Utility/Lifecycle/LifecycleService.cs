using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MaulGrab.Gameplay.Utility
{
    public abstract class LifecycleService : MonoBehaviour
    {
		private IExpiry[] _expiry;

		[Inject]
		public void Construct( IExpiry[] expiry )
		{
			_expiry = expiry;
		}

		public virtual void Expire()
		{
			foreach ( IExpiry expiry in _expiry )
			{
				expiry.OnExpired();
			}
		}
    }
}
