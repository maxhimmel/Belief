using System;
using System.Collections.Generic;
using UnityEngine;

namespace MaulGrab.Gameplay
{
    public class HitBoxService
    {
		public event EventHandler<IDamageable> NotifyHit;

		private readonly List<HitBox> _hitBoxes;

		public HitBoxService( List<HitBox> hitBoxes )
		{
			_hitBoxes = hitBoxes;
			foreach ( var box in hitBoxes )
			{
				box.NotifyHit += OnNotified;
			}
		}

		private void OnNotified( object hitBox, IDamageable victim )
		{
			//Debug.Log( $"{hitBox} notifying hit @ frame: {Time.frameCount}", hitBox as HitBox );
			NotifyHit?.Invoke( hitBox, victim );
		}

		public void ActivateHitBoxes()
		{
			foreach ( var box in _hitBoxes )
			{
				box.Activate();
			}
		}

		public void DeactivateHitBoxes()
		{
			foreach ( var box in _hitBoxes )
			{
				box.Deactivate();
			}
		}

		public void Dispose()
		{
			foreach ( var box in _hitBoxes )
			{
				box.NotifyHit -= OnNotified;
			}
		}
    }
}
