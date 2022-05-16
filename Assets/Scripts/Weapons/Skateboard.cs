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

		private Rigidbody2D _body;
		private Collider2D _collider;

		[Inject]
		public void Construct( Rigidbody2D body,
			Collider2D collider )
		{
			_body = body;
			_collider = collider;
		}

		private void OnCollisionEnter2D( Collision2D collision )
		{
			_body.drag = _sleepLinearDrag;
			_body.angularDrag = _sleepAngularDrag;

			_collider.isTrigger = true;
			_collider.gameObject.layer = LayerMask.GetMask( "Default" );
		}
	}
}
