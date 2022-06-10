using System.Collections.Generic;
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
		[BoxGroup( "On Collided" )]
		[SerializeField] private ForceAttractor _attractor = default;

		private IRigidbody _body;
		private List<Collider> _colliders;
		private HitBoxService _hitBoxService;
		private int _collectableLayer = 0;

		[Inject]
		public void Construct( IRigidbody body,
			List<Collider> collider,
			HitBoxService hitBoxService )
		{
			_body = body;
			_colliders = collider;
			_hitBoxService = hitBoxService;
		}

		private void OnCollisionEnter( Collision collision )
		{
			_body.LinearDrag = _sleepLinearDrag;
			_body.AngularDrag = _sleepAngularDrag;
			_body.Constraints = BodyConstraints.FreezePositionY;

			foreach ( var collider in _colliders )
			{
				collider.isTrigger = true;
				collider.gameObject.layer = _collectableLayer;
			}

			_attractor.gameObject.SetActive( true );
			_hitBoxService.DeactivateHitBoxes();
		}

		private void Awake()
		{
			_collectableLayer = LayerMask.NameToLayer( "Default" );
		}
	}
}
