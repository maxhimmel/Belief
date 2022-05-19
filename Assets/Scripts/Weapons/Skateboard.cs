using MaulGrab.Gameplay.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace MaulGrab.Gameplay.Weapons
{
    public class Skateboard : MonoBehaviour
    {
		[BoxGroup( "On Collided" )]
		[SerializeField] private float _sleepLinearDrag = 1;
		[BoxGroup( "On Collided" )]
		[SerializeField] private float _sleepAngularDrag = 1;

		private IRigidbody _body;
		private Collider _collider;

		[Inject]
		public void Construct( IRigidbody body,
			Collider collider )
		{
			_body = body;
			_collider = collider;
		}

		private void OnCollisionEnter2D( Collision2D collision )
		{
			_body.LinearDrag = _sleepLinearDrag;
			_body.AngularDrag = _sleepAngularDrag;

			_collider.isTrigger = true;
			_collider.gameObject.layer = LayerMask.GetMask( "Default" );
		}
	}
}
