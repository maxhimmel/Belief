using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using SooperDooper.Gameplay.Movement;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace SooperDooper.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
		private Rewired.Player _input;
		private CharacterMotor _motor;

		[Inject]
		public void Construct( 
			Rewired.Player input,
			CharacterMotor motor )
		{
			_input = input;
			_motor = motor;
		}

		private void Update()
		{
			Vector2 moveInput = GetMoveDirection();
			_motor.SetDesiredVelocity( moveInput );
		}

		private Vector2 GetMoveDirection()
		{
			Vector3 moveInput = _input.GetAxis2D( ReConsts.Action.Horizontal, ReConsts.Action.Vertical );
			return Vector2.ClampMagnitude( moveInput, 1 );
		}
	}
}
