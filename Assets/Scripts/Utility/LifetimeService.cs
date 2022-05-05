using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MaulGrab.Gameplay.Utility
{
    public class LifetimeService
    {
		private readonly float _lifetime;
		private readonly IExpiry _expiry;
		private readonly CancellationTokenSource _tokenSource;

		public LifetimeService( float lifetime, IExpiry expiry )
		{
			_lifetime = lifetime;
			_expiry = expiry;

			_tokenSource = new CancellationTokenSource();
		}

		public async void Start()
		{
			try
			{
				await Task.Delay( (int)_lifetime * 1000, _tokenSource.Token );
				Expire();
			}
			catch { }
		}

		public void Expire()
		{
			_tokenSource.Cancel();
			_tokenSource.Dispose();

			_expiry.OnExpired();
		}
    }
}
