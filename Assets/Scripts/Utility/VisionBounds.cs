using UnityEngine;
using static UnityEngine.UI.Image;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MaulGrab.Gameplay.Utility
{
    public class VisionBounds : MonoBehaviour
    {
        public float Range => _range;
        public float Angle => _angle;
        private Transform Origin => transform;

        [SerializeField] private float _range = 15;
        [SerializeField, Range( 0, 360 )] private float _angle = 50;

        public bool CanDetect( Vector3 otherPos, out float distSqr, out float angle )
		{
            Vector3 selfToOther = otherPos - Origin.position;
            distSqr = selfToOther.sqrMagnitude;
            angle = float.NaN;

            if ( distSqr > _range * _range )
			{
                return false;
			}

            angle = Vector3.Angle( Origin.forward, selfToOther );
            return angle < _angle / 2f;
        }

        public bool CanDetectPrecise( Vector3 otherPos, out float distance, out float angle )
        {
            Vector3 selfToOther = otherPos - Origin.position;
            distance = selfToOther.magnitude;
            angle = float.NaN;

            if ( distance > _range )
            {
                return false;
            }

            angle = Vector3.Angle( Origin.forward, selfToOther );
            return angle < _angle / 2f;
        }

#if UNITY_EDITOR
        [Header( "Editor/Tools" )]
        [SerializeField] private Color _outline = Color.blue;
        [SerializeField] private Color _fill = new Color( 0, 0, 1, 0.05f );

        private void OnDrawGizmosSelected()
        {
            Vector3 normal = Origin.up;
            Vector3 forward = Origin.forward;
            Vector3 center = Origin.position;

            Vector3 arcLeftDir = Quaternion.Euler( normal * -_angle / 2f ) * forward;
            Vector3 arcRightDir = Quaternion.Euler( normal * _angle / 2f ) * forward;

            Handles.color = _fill;
            Handles.DrawSolidArc( center, normal, arcLeftDir, _angle, _range );

            Handles.color = _outline;
            Handles.DrawWireArc( center, normal, arcLeftDir, _angle, _range );
            Handles.DrawLine( center, center + arcLeftDir * _range );
            Handles.DrawLine( center, center + arcRightDir * _range );
        }
#endif
	}
}
