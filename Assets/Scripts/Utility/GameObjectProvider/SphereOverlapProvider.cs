using System.Collections.Generic;
using UnityEngine;

namespace MaulGrab.Gameplay.Utility
{
    [System.Serializable]
    public class SphereOverlapProvider : IGameObjectProvider
	{
		private readonly IRigidbody _root;
		private readonly VisionBounds _visionBounds;
		private readonly CollisionQueryData _data;

		public SphereOverlapProvider( IRigidbody root,
			VisionBounds vision,
			CollisionQueryData data )
		{
			_root = root;
			_visionBounds = vision;
			_data = data;
		}

		public IEnumerable<GameObject> GetGameObjects()
		{
			Collider[] colliders = Physics.OverlapSphere( _root.Position, _visionBounds.Range, _data.DetectionLayer, _data.QueryTriggers );
			foreach ( var otherCollider in colliders )
			{
				Rigidbody otherBody = otherCollider.attachedRigidbody;
				if ( otherBody == null )
				{
					continue;
				}
				if ( otherBody.transform == _root.Transform )
				{
					continue;
				}

				yield return otherBody.gameObject;
			}
		}
	}
}
