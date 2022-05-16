using MaulGrab.Gameplay.Movement;
using UnityEngine;
using Zenject;
using MaulGrab.Gameplay.Weapons;
using MaulGrab.Gameplay.Pickups;

namespace MaulGrab.Gameplay.Player
{
    public class PlayerController : MonoBehaviour,
		ICollector<AmmoPickup>
    {
		private Rewired.Player _input;
		private CharacterMotor _motor;
		private Gun _gun;

		[Inject]
		public void Construct( 
			Rewired.Player input,
			CharacterMotor motor,
			Gun gun)
		{
			_input = input;
			_motor = motor;
			_gun = gun;
		}

		private void Update()
		{
			Vector2 moveInput = GetMoveDirection();
			_motor.SetDesiredVelocity( moveInput );

			HandleGunInput();
		}

		private Vector2 GetMoveDirection()
		{
			Vector3 moveInput = _input.GetAxis2D( ReConsts.Action.Horizontal, ReConsts.Action.Vertical );
			return Vector2.ClampMagnitude( moveInput, 1 );
		}

		private void HandleGunInput()
		{
			if ( _input.GetButtonDown( ReConsts.Action.Fire ) )
			{
				_gun.StartFiring();
			}
			else if ( _input.GetButtonUp( ReConsts.Action.Fire ) )
			{
				_gun.StopFiring();
			}
		}

		public void Collect( AmmoPickup pickup )
		{
			_gun.AddAmmo( pickup.AmmoCount );

			if ( _gun.IsMagazineEmpty )
			{
				_gun.Reload();
			}
		}
	}
}
