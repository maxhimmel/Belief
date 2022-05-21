using System;
using UnityEngine;

namespace MaulGrab.Gameplay.Utility
{
    public interface IRigidbody
    {
		public Transform Transform { get; }
		public Vector3 Velocity { get; set; }
		public Vector3 Position { get; set; }
		public Quaternion Rotation { get; set; }
		public Vector3 WorldCenterOfMass { get; }
		public float LinearDrag { get; set; }
		public float AngularDrag { get; set; }
		public BodyConstraints Constraints { get; set; }

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

	[Flags]
	public enum BodyConstraints
	{
		//
		// Summary:
		//     No constraints.
		None = 0,
		//
		// Summary:
		//     Freeze motion along the X-axis.
		FreezePositionX = 2,
		//
		// Summary:
		//     Freeze motion along the Y-axis.
		FreezePositionY = 4,
		//
		// Summary:
		//     Freeze motion along the Z-axis.
		FreezePositionZ = 8,
		//
		// Summary:
		//     Freeze motion along all axes.
		FreezePosition = 14,
		//
		// Summary:
		//     Freeze rotation along the X-axis.
		FreezeRotationX = 16,
		//
		// Summary:
		//     Freeze rotation along the Y-axis.
		FreezeRotationY = 32,
		//
		// Summary:
		//     Freeze rotation along the Z-axis.
		FreezeRotationZ = 64,
		//
		// Summary:
		//     Freeze rotation along all axes.
		FreezeRotation = 112,
		//
		// Summary:
		//     Freeze rotation and motion along all axes.
		FreezeAll = 126
	}
}
