using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using MaulGrab.Gameplay.Utility;

namespace MaulGrab.Gameplay
{
    public class AimAssist : MonoBehaviour
    {
        private const float _debugDrawDuration = 2;

        private Transform Origin => _vision.transform;

        [SerializeField, Range( 0, 1 )] private float _influence = 1;

        [Space]
        [SerializeField] private bool _drawDebug = false;

        private IGameObjectProvider _gameObjectProvider;
		private VisionBounds _vision;

		[Inject]
		public void Construct(IGameObjectProvider gameObjectProvider,
            VisionBounds vision)
		{
            _gameObjectProvider = gameObjectProvider;
            _vision = vision;
		}

        public Quaternion GetTargetDirection()
		{
            Transform target = GetTarget();
            if ( target == null )
			{
                return Origin.rotation;
			}

            Quaternion currentRot = Quaternion.LookRotation( Origin.forward, Origin.up );
            Quaternion targetRot = Quaternion.LookRotation( target.position - Origin.position, Origin.up );
            Quaternion resultRot = Quaternion.Lerp( currentRot, targetRot, _influence );

            if ( _drawDebug )
            {
                Debug.DrawRay( Origin.position, resultRot * Vector3.forward * _vision.Range, Color.black, _debugDrawDuration );
            }

            return resultRot;
		}

		public Transform GetTarget()
		{
            float smallestDistSqr = float.MaxValue;
            float smallestAngle = float.MaxValue;

            Transform closestTarget = null;
            Transform mostAlignedTarget = null;

            foreach ( var other in _gameObjectProvider.GetGameObjects() )
			{
                if ( !_vision.CanDetect( other.transform.position, 
                    out float distSqr, 
                    out float angle ) )
				{
                    continue;
				}

                if ( distSqr < smallestDistSqr )
				{
                    smallestDistSqr = distSqr;
                    closestTarget = other.transform;
				}

                if ( angle < smallestAngle )
				{
                    smallestAngle = angle;
                    mostAlignedTarget = other.transform;
				}
			}

            if ( _drawDebug )
            {
                if ( mostAlignedTarget != null )
                {
                    Debug.DrawRay( mostAlignedTarget.position, Vector3.up * 5, Color.magenta, _debugDrawDuration );
                }
                if ( closestTarget != null )
                {
                    Debug.DrawRay( closestTarget.position, Vector3.up * 5, Color.cyan, _debugDrawDuration );
                }
            }

            return mostAlignedTarget;
		}
	}
}
