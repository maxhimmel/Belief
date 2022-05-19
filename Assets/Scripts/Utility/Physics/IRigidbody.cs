using UnityEngine;

namespace MaulGrab.Gameplay.Utility
{
    public interface IRigidbody
    {
		public Transform Transform { get; }
		public Vector3 Velocity { get; set; }
		public Quaternion Rotation { get; set; }
		public float LinearDrag { get; set; }
		public float AngularDrag { get; set; }

		void AddForce( Vector3 force, ForceType mode );
		void AddTorque( Vector3 torque, ForceType mode );
	}

	public enum ForceType
	{
		//
		// Summary:
		//     Add a continuous force to the rigidbody, using its mass.
		Force = 0,
		//
		// Summary:
		//     Add an instant force impulse to the rigidbody, using its mass.
		Impulse = 1,
		//
		// Summary:
		//     Add an instant velocity change to the rigidbody, ignoring its mass.
		VelocityChange = 2,
		//
		// Summary:
		//     Add a continuous acceleration to the rigidbody, ignoring its mass.
		Acceleration = 5
	}
}
