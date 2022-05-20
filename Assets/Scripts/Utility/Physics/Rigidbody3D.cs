using UnityEngine;

using UForceMode = UnityEngine.ForceMode;

namespace MaulGrab.Gameplay.Utility
{
	public class Rigidbody3D : IRigidbody
	{
		public Transform Transform => _body.transform;

		public Vector3 Velocity {
			get {
				return _body.velocity;
			}
			set {
				_body.velocity = value;
			}
		}

		public Quaternion Rotation {
			get {
				return _body.rotation;
			}
			set {
				_body.rotation = value;
			}
		}

		public float LinearDrag {
			get {
				return _body.drag;
			}
			set {
				_body.drag = value;
			}
		}

		public float AngularDrag {
			get {
				return _body.angularDrag;
			}
			set {
				_body.angularDrag = value;
			}
		}

		public BodyConstraints Constraints {
			get {
				return (BodyConstraints)_body.constraints;
			}
			set {
				_body.constraints = (RigidbodyConstraints)value;
			}
		}

		private readonly Rigidbody _body;

		public Rigidbody3D( Rigidbody body )
		{
			_body = body;
		}

		public void AddForce( Vector3 force, ForceType mode )
		{
			_body.AddForce( force, (UForceMode)mode );
		}

		public void AddTorque( Vector3 torque, ForceType mode )
		{
			_body.AddTorque( torque, (UForceMode)mode );
		}
	}
}
