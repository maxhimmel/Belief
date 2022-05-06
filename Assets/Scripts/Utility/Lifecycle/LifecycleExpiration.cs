using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MaulGrab.Gameplay.Utility
{
    public class LifecycleExpiration : LifecycleService
    {
        [SerializeField] private float _lifetime = 1;

		private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

		private async void OnEnable()
		{
			try
			{
				await Task.Delay( (int)_lifetime * 1000, _tokenSource.Token );
				Expire();
			}
			catch { }
		}

		public override void Expire()
		{
			_tokenSource.Cancel();
			_tokenSource.Dispose();

			base.Expire();
		}
	}
}
