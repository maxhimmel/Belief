using MaulGrab.Gameplay.Movement;
using UnityEngine;
using Zenject;
using MaulGrab.Gameplay.Weapons;
using MaulGrab.Gameplay.Pickups;
using Cinemachine;

namespace MaulGrab.Gameplay.Player
{
    public class PlayerController : MonoBehaviour,
		ICollector<AmmoPickup>
    {
		private Rewired.Player _input;
		private CharacterMotor _motor;
		private DashController _dashController;
		private Gun _gun;
		private CinemachineBrain _cineBrain;

		[Inject]
		public void Construct( 
			Rewired.Player input,
			CharacterMotor motor,
			DashController dashController,
			Gun gun,
			CinemachineBrain cineBrain)
		{
			_input = input;
			_motor = motor;
			_dashController = dashController;
			_gun = gun;
			_cineBrain = cineBrain;
		}

		private void Update()
		{
			Vector3 moveInput = GetMoveDirection();

			if ( !_dashController.IsDashing )
			{
				_motor.SetDesiredVelocity( moveInput );
			}

			HandleDashInput( moveInput );
			HandleGunInput();
		}

		private Vector3 GetMoveDirection()
		{
			Vector3 moveInput = new Vector3( _input.GetAxis( ReConsts.Action.Horizontal ), 0, _input.GetAxis( ReConsts.Action.Vertical ) );
			moveInput = Quaternion.Euler( 0, _cineBrain.transform.eulerAngles.y, 0 ) * moveInput;

			return Vector3.ClampMagnitude( moveInput, 1 );
		}

		private void HandleDashInput( Vector3 moveInput )
		{
			if ( _input.GetButtonDown( ReConsts.Action.Jump ) )
			{
				if ( CanDash() )
				{
					_dashController.Dash( moveInput );
				}
			}
		}

		private bool CanDash()
		{
			if ( !_motor.IsGrounded )
			{
				return false;
			}
			if ( _gun.IsMagazineEmpty )
			{
				return false;
			}

			return !_dashController.IsDashing;
		}

		private void HandleGunInput()
		{
			if ( !CanShoot() )
			{
				return;
			}

			if ( _input.GetButtonDown( ReConsts.Action.Fire ) )
			{
				_gun.StartFiring();
			}
			else if ( _input.GetButtonUp( ReConsts.Action.Fire ) )
			{
				_gun.StopFiring();
			}
		}

		private bool CanShoot()
		{
			if ( _dashController.IsDashing )
			{
				return false;
			}

			return _motor.IsGrounded;
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
