using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using MaulGrab.Gameplay.Movement;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;
using MaulGrab.Gameplay.Weapons;

namespace MaulGrab.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
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
	}
}
